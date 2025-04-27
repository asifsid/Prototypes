using System;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.IdentityModel
{
	internal class SafeCertContextHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeCertContextHandle InvalidHandle => new SafeCertContextHandle(IntPtr.Zero);

		private SafeCertContextHandle()
			: base(ownsHandle: true)
		{
		}

		private SafeCertContextHandle(IntPtr handle)
			: base(ownsHandle: true)
		{
			SetHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			return CryptoUtil.CAPI.CertFreeCertificateContext(handle);
		}
	}
}
