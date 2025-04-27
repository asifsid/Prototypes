using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public static class WSAddressing200408Constants
	{
		public static class Elements
		{
			public const string Action = "Action";

			public const string ReplyTo = "ReplyTo";
		}

		public const string Prefix = "wsa";

		public const string NamespaceUri = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
	}
}
