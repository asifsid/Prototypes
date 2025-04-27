using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public static class AuthenticationMethods
	{
		public const string Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/";

		public const string HardwareToken = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/hardwaretoken";

		public const string Kerberos = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/kerberos";

		public const string Password = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password";

		public const string Pgp = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/pgp";

		public const string SecureRemotePassword = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/secureremotepassword";

		public const string Signature = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/signature";

		public const string Smartcard = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/smartcard";

		public const string SmartcardPki = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/smartcardpki";

		public const string Spki = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/spki";

		public const string TlsClient = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/tlsclient";

		public const string Unspecified = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/unspecified";

		public const string Windows = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows";

		public const string Xkms = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/xkms";

		public const string X509 = "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/x509";
	}
}
