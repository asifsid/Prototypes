using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public static class WSPolicyConstants
	{
		public static class ElementNames
		{
			public const string AppliesTo = "AppliesTo";
		}

		public const string NamespaceURI = "http://schemas.xmlsoap.org/ws/2004/09/policy";

		public const string Prefix = "wsp";
	}
}
