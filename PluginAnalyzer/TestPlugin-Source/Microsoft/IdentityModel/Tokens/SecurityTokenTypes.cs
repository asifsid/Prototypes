using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public static class SecurityTokenTypes
	{
		public const string Kerberos = "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Kerberos";

		public const string Rsa = "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Rsa";

		public const string OasisWssSaml11TokenProfile11 = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1";

		public const string OasisWssSaml2TokenProfile11 = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0";

		public const string Saml11TokenProfile11 = "urn:oasis:names:tc:SAML:1.0:assertion";

		public const string Saml2TokenProfile11 = "urn:oasis:names:tc:SAML:2.0:assertion";

		public const string UserName = "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/UserName";

		public const string X509Certificate = "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/X509Certificate";
	}
}
