using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public static class KeyTypes
	{
		public const string Symmetric = "http://schemas.microsoft.com/idfx/keytype/symmetric";

		public const string Asymmetric = "http://schemas.microsoft.com/idfx/keytype/asymmetric";

		public const string Bearer = "http://schemas.microsoft.com/idfx/keytype/bearer";
	}
}
