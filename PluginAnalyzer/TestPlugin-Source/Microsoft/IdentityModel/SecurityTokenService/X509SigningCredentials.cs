using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class X509SigningCredentials : SigningCredentials
	{
		private X509Certificate2 _certificate;

		public X509Certificate2 Certificate => _certificate;

		public X509SigningCredentials(X509Certificate2 certificate)
			: this(certificate, new SecurityKeyIdentifier(new X509SecurityToken(certificate).CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>()))
		{
		}

		public X509SigningCredentials(X509Certificate2 certificate, string signatureAlgorithm, string digestAlgorithm)
			: this(new X509SecurityToken(certificate), new SecurityKeyIdentifier(new X509SecurityToken(certificate).CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>()), signatureAlgorithm, digestAlgorithm)
		{
		}

		public X509SigningCredentials(X509Certificate2 certificate, SecurityKeyIdentifier ski)
			: this(new X509SecurityToken(certificate), ski, "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", "http://www.w3.org/2001/04/xmlenc#sha256")
		{
		}

		public X509SigningCredentials(X509Certificate2 certificate, SecurityKeyIdentifier ski, string signatureAlgorithm, string digestAlgorithm)
			: this(new X509SecurityToken(certificate), ski, signatureAlgorithm, digestAlgorithm)
		{
		}

		internal X509SigningCredentials(X509SecurityToken token, SecurityKeyIdentifier ski, string signatureAlgorithm, string digestAlgorithm)
			: base(token.SecurityKeys[0], signatureAlgorithm, digestAlgorithm, ski)
		{
			_certificate = token.Certificate;
			if (!_certificate.HasPrivateKey)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID2057"));
			}
		}
	}
}
