using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public static class AuthenticationTypes
	{
		public const string Basic = "Basic";

		public const string Federation = "Federation";

		public const string Kerberos = "Kerberos";

		public const string Negotiate = "Negotiate";

		public const string Password = "Password";

		public const string Signature = "Signature";

		public const string Windows = "Windows";

		public const string X509 = "X509";
	}
}
