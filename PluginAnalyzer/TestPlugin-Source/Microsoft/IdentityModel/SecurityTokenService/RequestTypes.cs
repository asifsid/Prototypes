using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public static class RequestTypes
	{
		public const string Cancel = "http://schemas.microsoft.com/idfx/requesttype/cancel";

		public const string Issue = "http://schemas.microsoft.com/idfx/requesttype/issue";

		public const string Renew = "http://schemas.microsoft.com/idfx/requesttype/renew";

		public const string Validate = "http://schemas.microsoft.com/idfx/requesttype/validate";

		public const string IssueCard = "http://schemas.microsoft.com/idfx/requesttype/issueCard";

		public const string GetMetadata = "http://schemas.microsoft.com/idfx/requesttype/getMetadata";
	}
}
