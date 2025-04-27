using System;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	internal static class WindowsMappingOperations
	{
		public static string FindUpn(IClaimsIdentity claimsIdentity)
		{
			string text = null;
			foreach (Claim claim in claimsIdentity.Claims)
			{
				if (StringComparer.Ordinal.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn", claim.ClaimType))
				{
					if (text != null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID1053")));
					}
					text = claim.Value;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID1054")));
			}
			return text;
		}
	}
}
