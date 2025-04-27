using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.IdentityModel
{
	internal sealed class SafeHGlobalHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public static SafeHGlobalHandle InvalidHandle => new SafeHGlobalHandle(IntPtr.Zero);

		private SafeHGlobalHandle()
			: base(ownsHandle: true)
		{
		}

		private SafeHGlobalHandle(IntPtr handle)
			: base(ownsHandle: true)
		{
			SetHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			Marshal.FreeHGlobal(handle);
			return true;
		}

		public static SafeHGlobalHandle AllocHGlobal(string s)
		{
			byte[] bytes = new byte[checked((s.Length + 1) * 2)];
			Encoding.Unicode.GetBytes(s, 0, s.Length, bytes, 0);
			return AllocHGlobal(bytes);
		}

		public static SafeHGlobalHandle AllocHGlobal(byte[] bytes)
		{
			SafeHGlobalHandle safeHGlobalHandle = AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, safeHGlobalHandle.DangerousGetHandle(), bytes.Length);
			return safeHGlobalHandle;
		}

		public static SafeHGlobalHandle AllocHGlobal(uint cb)
		{
			return AllocHGlobal((int)cb);
		}

		public static SafeHGlobalHandle AllocHGlobal(int cb)
		{
			if (cb < 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentOutOfRangeException("cb", SR.GetString("ID0017")));
			}
			SafeHGlobalHandle safeHGlobalHandle = new SafeHGlobalHandle();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				IntPtr intPtr = Marshal.AllocHGlobal(cb);
				safeHGlobalHandle.SetHandle(intPtr);
			}
			return safeHGlobalHandle;
		}
	}
}
