using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public class ClaimsAuthenticationManager
	{
		public virtual IClaimsPrincipal Authenticate(string resourceName, IClaimsPrincipal incomingPrincipal)
		{
			return incomingPrincipal;
		}
	}
}
