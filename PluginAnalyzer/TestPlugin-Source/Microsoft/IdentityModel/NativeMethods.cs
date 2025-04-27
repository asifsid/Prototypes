using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;

namespace Microsoft.IdentityModel
{
	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		private const string ADVAPI32 = "advapi32.dll";

		private const string KERNEL32 = "kernel32.dll";

		private const string SECUR32 = "secur32.dll";

		internal const uint STATUS_ACCOUNT_RESTRICTION = 3221225582u;

		internal const uint KERB_CERTIFICATE_S4U_LOGON_FLAG_CHECK_LOGONHOURS = 2u;

		internal const int ERROR_ACCESS_DENIED = 5;

		internal const int ERROR_BAD_LENGTH = 24;

		internal const int ERROR_INSUFFICIENT_BUFFER = 122;

		internal const uint SE_GROUP_ENABLED = 4u;

		internal const uint SE_GROUP_USE_FOR_DENY_ONLY = 16u;

		internal const uint SE_GROUP_LOGON_ID = 3221225472u;

		internal static byte[] LsaSourceName = GetLsaName("WindowsIdentityFoundation");

		internal static byte[] LsaKerberosName = GetLsaName("Kerberos");

		private static byte[] GetLsaName(string name)
		{
			return Encoding.ASCII.GetBytes(name);
		}

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool LogonUser([In] string lpszUserName, [In] string lpszDomain, [In] string lpszPassword, [In] uint dwLogonType, [In] uint dwLogonProvider, out SafeCloseHandle phToken);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetTokenInformation([In] IntPtr tokenHandle, [In] uint tokenInformationClass, [In] SafeHGlobalHandle tokenInformation, [In] uint tokenInformationLength, out uint returnLength);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool LookupPrivilegeValueW([In] string lpSystemName, [In] string lpName, out LUID Luid);

		[DllImport("advapi32.dll", SetLastError = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool AdjustTokenPrivileges([In] SafeCloseHandle tokenHandle, [In][MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges, [In] ref TOKEN_PRIVILEGE newState, [In] uint bufferLength, out TOKEN_PRIVILEGE previousState, out uint returnLength);

		[DllImport("advapi32.dll", SetLastError = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool RevertToSelf();

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool OpenProcessToken([In] IntPtr processToken, [In] TokenAccessLevels desiredAccess, out SafeCloseHandle tokenHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool OpenThreadToken([In] IntPtr threadHandle, [In] TokenAccessLevels desiredAccess, [In][MarshalAs(UnmanagedType.Bool)] bool openAsSelf, out SafeCloseHandle tokenHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetCurrentProcess();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetCurrentThread();

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DuplicateTokenEx([In] SafeCloseHandle existingTokenHandle, [In] TokenAccessLevels desiredAccess, [In] IntPtr tokenAttributes, [In] SECURITY_IMPERSONATION_LEVEL impersonationLevel, [In] TokenType tokenType, out SafeCloseHandle duplicateTokenHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetThreadToken([In] IntPtr threadHandle, [In] SafeCloseHandle threadToken);

		[DllImport("secur32.dll", CharSet = CharSet.Auto)]
		internal static extern int LsaRegisterLogonProcess([In] ref UNICODE_INTPTR_STRING logonProcessName, out SafeLsaLogonProcessHandle lsaHandle, out IntPtr securityMode);

		[DllImport("secur32.dll", CharSet = CharSet.Auto)]
		internal static extern int LsaConnectUntrusted(out SafeLsaLogonProcessHandle lsaHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern int LsaNtStatusToWinError([In] int status);

		[DllImport("secur32.dll", CharSet = CharSet.Auto)]
		internal static extern int LsaLookupAuthenticationPackage([In] SafeLsaLogonProcessHandle lsaHandle, [In] ref UNICODE_INTPTR_STRING packageName, out uint authenticationPackage);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool AllocateLocallyUniqueId(out LUID Luid);

		[DllImport("secur32.dll")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static extern int LsaFreeReturnBuffer(IntPtr handle);

		[DllImport("secur32.dll", CharSet = CharSet.Auto)]
		internal static extern int LsaLogonUser([In] SafeLsaLogonProcessHandle LsaHandle, [In] ref UNICODE_INTPTR_STRING OriginName, [In] SecurityLogonType LogonType, [In] uint AuthenticationPackage, [In] IntPtr AuthenticationInformation, [In] uint AuthenticationInformationLength, [In] IntPtr LocalGroups, [In] ref TOKEN_SOURCE SourceContext, out SafeLsaReturnBufferHandle ProfileBuffer, out uint ProfileBufferLength, out LUID LogonId, out SafeCloseHandle Token, out QUOTA_LIMITS Quotas, out int SubStatus);

		[DllImport("secur32.dll", CharSet = CharSet.Auto)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static extern int LsaDeregisterLogonProcess([In] IntPtr handle);

		[DllImport("secur32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.U1)]
		internal static extern bool TranslateName(string input, EXTENDED_NAME_FORMAT inputFormat, EXTENDED_NAME_FORMAT outputFormat, StringBuilder outputString, out uint size);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ImpersonateAnonymousToken(IntPtr threadHandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetThreadToken(IntPtr pThreadHandle, IntPtr token);
	}
}
