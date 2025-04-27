using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class RsaSha1SignatureCookieTransform : RsaSignatureCookieTransform
	{
		public RsaSha1SignatureCookieTransform(RSA key)
			: base(key)
		{
			base.HashName = "SHA1";
		}

		public RsaSha1SignatureCookieTransform(X509Certificate2 certificate)
			: base(certificate)
		{
			base.HashName = "SHA1";
		}
	}
}
