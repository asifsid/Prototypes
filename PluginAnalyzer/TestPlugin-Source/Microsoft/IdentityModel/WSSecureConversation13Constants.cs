using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public static class WSSecureConversation13Constants
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

		public const string Namespace = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512";

		public const string TokenTypeURI = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512/sct";

		public const int DefaultDerivedKeyLength = 32;
	}
}
