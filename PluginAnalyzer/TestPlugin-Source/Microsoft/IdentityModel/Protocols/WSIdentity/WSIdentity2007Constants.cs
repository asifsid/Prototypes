using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public static class WSIdentity2007Constants
	{
		public static class Elements
		{
			public const string IssuerInformation = "IssuerInformation";

			public const string IssuerInformationEntry = "IssuerInformationEntry";

			public const string EntryName = "EntryName";

			public const string EntryValue = "EntryValue";

			public const string RequireStrongRecipientIdentity = "RequireStrongRecipientIdentity";
		}

		public const string Namespace = "http://schemas.xmlsoap.org/ws/2007/01/identity";

		public const string Prefix = "ic07";

		public const int MinEntryNameLength = 1;

		public const int MaxEntryNameLength = 255;

		public const int MinEntryValueLength = 1;

		public const int MaxEntryValueLength = 255;
	}
}
