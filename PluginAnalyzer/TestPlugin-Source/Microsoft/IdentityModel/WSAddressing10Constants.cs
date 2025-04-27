using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public static class WSAddressing10Constants
	{
		public static class Elements
		{
			public const string Action = "Action";

			public const string Address = "Address";

			public const string ReplyTo = "ReplyTo";
		}

		public const string Prefix = "wsa";

		public const string NamespaceUri = "http://www.w3.org/2005/08/addressing";
	}
}
