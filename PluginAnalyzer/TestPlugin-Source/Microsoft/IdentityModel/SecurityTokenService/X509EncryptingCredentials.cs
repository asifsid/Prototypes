using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class X509EncryptingCredentials : EncryptingCredentials
	{
		private X509Certificate2 _certificate;

		public X509Certificate2 Certificate => _certificate;

		public X509EncryptingCredentials(X509Certificate2 certificate)
			: this(new X509SecurityToken(certificate))
		{
		}

		public X509EncryptingCredentials(X509Certificate2 certificate, string keyWrappingAlgorithm)
			: this(new X509SecurityToken(certificate), keyWrappingAlgorithm)
		{
		}

		public X509EncryptingCredentials(X509Certificate2 certificate, SecurityKeyIdentifier ski)
			: this(new X509SecurityToken(certificate), ski, "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p")
		{
		}

		public X509EncryptingCredentials(X509Certificate2 certificate, SecurityKeyIdentifier ski, string keyWrappingAlgorithm)
			: this(new X509SecurityToken(certificate), ski, keyWrappingAlgorithm)
		{
		}

		internal X509EncryptingCredentials(X509SecurityToken token)
			: this(token, new SecurityKeyIdentifier(token.CreateKeyIdentifierClause<X509IssuerSerialKeyIdentifierClause>()), "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p")
		{
		}

		internal X509EncryptingCredentials(X509SecurityToken token, string keyWrappingAlgorithm)
			: this(token, new SecurityKeyIdentifier(token.CreateKeyIdentifierClause<X509IssuerSerialKeyIdentifierClause>()), keyWrappingAlgorithm)
		{
		}

		internal X509EncryptingCredentials(X509SecurityToken token, SecurityKeyIdentifier ski, string keyWrappingAlgorithm)
			: base(token.SecurityKeys[0], ski, keyWrappingAlgorithm)
		{
			_certificate = token.Certificate;
		}
	}
}
