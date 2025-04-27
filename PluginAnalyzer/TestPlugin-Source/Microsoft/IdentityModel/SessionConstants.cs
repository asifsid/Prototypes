using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public static class SessionConstants
	{
		public static class ElementNames
		{
			public const string Name = "SessionToken";

			public const string Identifier = "Identifier";

			public const string Instance = "Instance";
		}

		public static class Attributes
		{
			public const string Length = "Length";

			public const string Nonce = "Nonce";

			public const string Instance = "Instance";
		}

		public const string Namespace = "http://schemas.microsoft.com/ws/2008/06/identity";

		public const string TokenTypeURI = "http://schemas.microsoft.com/2008/08/sessiontoken";

		public const int DefaultDerivedKeyLength = 32;
	}
}
