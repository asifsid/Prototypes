using System;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.IdentityModel
{
	internal class SafeCertChainHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeCertChainHandle InvalidHandle => new SafeCertChainHandle(IntPtr.Zero);

		private SafeCertChainHandle()
			: base(ownsHandle: true)
		{
		}

		private SafeCertChainHandle(IntPtr handle)
			: base(ownsHandle: true)
		{
			SetHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			CryptoUtil.CAPI.CertFreeCertificateChain(handle);
			return true;
		}
	}
}
