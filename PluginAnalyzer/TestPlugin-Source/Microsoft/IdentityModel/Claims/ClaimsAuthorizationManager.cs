using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public class ClaimsAuthorizationManager
	{
		public virtual bool CheckAccess(AuthorizationContext context)
		{
			return true;
		}
	}
}
