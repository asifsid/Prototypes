using System;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.IdentityModel
{
	internal class SafeCertStoreHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public static SafeCertStoreHandle InvalidHandle => new SafeCertStoreHandle(IntPtr.Zero);

		private SafeCertStoreHandle()
			: base(ownsHandle: true)
		{
		}

		private SafeCertStoreHandle(IntPtr handle)
			: base(ownsHandle: true)
		{
			SetHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			return CryptoUtil.CAPI.CertCloseStore(handle, 0u);
		}
	}
}
