using System;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.IdentityModel
{
	internal sealed class SafeLsaReturnBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeLsaReturnBufferHandle InvalidHandle => new SafeLsaReturnBufferHandle(IntPtr.Zero);

		private SafeLsaReturnBufferHandle()
			: base(ownsHandle: true)
		{
		}

		internal SafeLsaReturnBufferHandle(IntPtr handle)
			: base(ownsHandle: true)
		{
			SetHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.LsaFreeReturnBuffer(handle) >= 0;
		}
	}
}
