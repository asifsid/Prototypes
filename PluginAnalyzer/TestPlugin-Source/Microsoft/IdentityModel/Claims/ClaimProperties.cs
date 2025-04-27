using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public static class ClaimProperties
	{
		public const string Namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties";

		public const string SamlAttributeDisplayName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname";

		public const string SamlAttributeNameFormat = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename";

		public const string SamlNameIdentifierFormat = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format";

		public const string SamlNameIdentifierNameQualifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier";

		public const string SamlNameIdentifierSpNameQualifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spnamequalifier";

		public const string SamlNameIdentifierSpProvidedId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spprovidedid";
	}
}
