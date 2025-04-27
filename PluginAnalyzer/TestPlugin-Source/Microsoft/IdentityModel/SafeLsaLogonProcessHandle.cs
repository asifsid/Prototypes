using System;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.IdentityModel
{
	internal sealed class SafeLsaLogonProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeLsaLogonProcessHandle InvalidHandle => new SafeLsaLogonProcessHandle(IntPtr.Zero);

		private SafeLsaLogonProcessHandle()
			: base(ownsHandle: true)
		{
		}

		internal SafeLsaLogonProcessHandle(IntPtr handle)
			: base(ownsHandle: true)
		{
			SetHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.LsaDeregisterLogonProcess(handle) >= 0;
		}
	}
}
