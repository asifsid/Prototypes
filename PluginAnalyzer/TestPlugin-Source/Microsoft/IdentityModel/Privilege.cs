using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Microsoft.IdentityModel
{
	internal class Privilege
	{
		public const string SeAuditPrivilege = "SeAuditPrivilege";

		public const string SeTcbPrivilege = "SeTcbPrivilege";

		private const uint SE_PRIVILEGE_DISABLED = 0u;

		private const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1u;

		private const uint SE_PRIVILEGE_ENABLED = 2u;

		private const uint SE_PRIVILEGE_USED_FOR_ACCESS = 2147483648u;

		private const int ERROR_SUCCESS = 0;

		private const int ERROR_NO_TOKEN = 1008;

		private const int ERROR_NOT_ALL_ASSIGNED = 1300;

		private static Dictionary<string, LUID> _luids = new Dictionary<string, LUID>();

		private string _privilege;

		private LUID _luid;

		private bool _needToRevert;

		private bool _initialEnabled;

		private bool _isImpersonating;

		private SafeCloseHandle _threadToken;

		public Privilege(string privilege)
		{
			_privilege = privilege;
			_luid = LuidFromPrivilege(privilege);
		}

		public void Enable()
		{
			_threadToken = GetThreadToken();
			EnableTokenPrivilege(_threadToken);
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int Revert()
		{
			if (!_isImpersonating)
			{
				if (_needToRevert && !_initialEnabled)
				{
					TOKEN_PRIVILEGE newState = default(TOKEN_PRIVILEGE);
					newState.PrivilegeCount = 1u;
					newState.Privilege.Luid = _luid;
					newState.Privilege.Attributes = 0u;
					uint returnLength = 0u;
					if (!NativeMethods.AdjustTokenPrivileges(_threadToken, disableAllPrivileges: false, ref newState, TOKEN_PRIVILEGE.Size, out var _, out returnLength))
					{
						return Marshal.GetLastWin32Error();
					}
				}
				_needToRevert = false;
			}
			else
			{
				if (!NativeMethods.RevertToSelf())
				{
					return Marshal.GetLastWin32Error();
				}
				_isImpersonating = false;
			}
			if (_threadToken != null)
			{
				_threadToken.Close();
				_threadToken = null;
			}
			return 0;
		}

		private SafeCloseHandle GetThreadToken()
		{
			if (!NativeMethods.OpenThreadToken(NativeMethods.GetCurrentThread(), TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, openAsSelf: true, out var tokenHandle))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				CryptoUtil.CloseInvalidOutSafeHandle(tokenHandle);
				if (lastWin32Error != 1008)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(lastWin32Error));
				}
				if (!NativeMethods.OpenProcessToken(NativeMethods.GetCurrentProcess(), TokenAccessLevels.Duplicate, out var tokenHandle2))
				{
					lastWin32Error = Marshal.GetLastWin32Error();
					CryptoUtil.CloseInvalidOutSafeHandle(tokenHandle2);
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(lastWin32Error));
				}
				try
				{
					if (!NativeMethods.DuplicateTokenEx(tokenHandle2, TokenAccessLevels.Impersonate | TokenAccessLevels.Query | TokenAccessLevels.AdjustPrivileges, IntPtr.Zero, SECURITY_IMPERSONATION_LEVEL.Impersonation, TokenType.TokenImpersonation, out tokenHandle))
					{
						lastWin32Error = Marshal.GetLastWin32Error();
						CryptoUtil.CloseInvalidOutSafeHandle(tokenHandle);
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(lastWin32Error));
					}
					SetThreadToken(tokenHandle);
					return tokenHandle;
				}
				finally
				{
					tokenHandle2.Close();
				}
			}
			return tokenHandle;
		}

		private void EnableTokenPrivilege(SafeCloseHandle threadToken)
		{
			TOKEN_PRIVILEGE newState = default(TOKEN_PRIVILEGE);
			newState.PrivilegeCount = 1u;
			newState.Privilege.Luid = _luid;
			newState.Privilege.Attributes = 2u;
			uint returnLength = 0u;
			RuntimeHelpers.PrepareConstrainedRegions();
			TOKEN_PRIVILEGE previousState;
			bool flag = NativeMethods.AdjustTokenPrivileges(threadToken, disableAllPrivileges: false, ref newState, TOKEN_PRIVILEGE.Size, out previousState, out returnLength);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (flag && lastWin32Error == 0)
			{
				_initialEnabled = 0 != (previousState.Privilege.Attributes & 2);
				_needToRevert = true;
			}
			if (lastWin32Error == 1300)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new PrivilegeNotHeldException(_privilege));
			}
			if (!flag)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(lastWin32Error));
			}
		}

		private void SetThreadToken(SafeCloseHandle threadToken)
		{
			int error = 0;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (!NativeMethods.SetThreadToken(IntPtr.Zero, threadToken))
				{
					error = Marshal.GetLastWin32Error();
				}
				else
				{
					_isImpersonating = true;
				}
			}
			if (!_isImpersonating)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(error));
			}
		}

		private static LUID LuidFromPrivilege(string privilege)
		{
			LUID value;
			lock (_luids)
			{
				if (_luids.TryGetValue(privilege, out value))
				{
					return value;
				}
			}
			if (!NativeMethods.LookupPrivilegeValueW(null, privilege, out value))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(lastWin32Error));
			}
			lock (_luids)
			{
				if (!_luids.ContainsKey(privilege))
				{
					_luids[privilege] = value;
					return value;
				}
				return value;
			}
		}
	}
}
