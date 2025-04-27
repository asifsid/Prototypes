using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public static class WSUtilityConstants
	{
		public static class Attributes
		{
			public const string IdAttribute = "Id";
		}

		public static class ElementNames
		{
			public const string Created = "Created";

			public const string Expires = "Expires";
		}

		public const string NamespaceURI = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

		public const string Prefix = "wsu";
	}
}
