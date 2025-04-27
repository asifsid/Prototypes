using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	[ComVisible(true)]
	public static class ExclusiveC14NConstants
	{
		public static class Elements
		{
			public const string PrefixList = "PrefixList";

			public const string InclusiveNamespaces = "InclusiveNamespaces";
		}

		public const string Namespace = "http://www.w3.org/2001/10/xml-exc-c14n#";

		public const string Prefix = "ec";
	}
}
