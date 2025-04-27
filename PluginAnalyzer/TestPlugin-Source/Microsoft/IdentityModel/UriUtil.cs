using System;

namespace Microsoft.IdentityModel
{
	internal static class UriUtil
	{
		public static bool CanCreateValidUri(string uriString, UriKind uriKind)
		{
			Uri result;
			return TryCreateValidUri(uriString, uriKind, out result);
		}

		public static bool TryCreateValidUri(string uriString, UriKind uriKind, out Uri result)
		{
			return Uri.TryCreate(uriString, uriKind, out result);
		}
	}
}
