using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public static class WSSecureConversationFeb2005Constants
	{
		public static class ElementNames
		{
			public const string Name = "SecurityContextToken";

			public const string Identifier = "Identifier";

			public const string Instance = "Instance";
		}

		public static class Attributes
		{
			public const string Length = "Length";

			public const string Nonce = "Nonce";

			public const string Instance = "Instance";
		}

		public const string Namespace = "http://schemas.xmlsoap.org/ws/2005/02/sc";

		public const string TokenTypeURI = "http://schemas.xmlsoap.org/ws/2005/02/sc/sct";

		public const int DefaultDerivedKeyLength = 32;
	}
}
