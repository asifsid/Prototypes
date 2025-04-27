using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Controls
{
	[ComVisible(true)]
	public enum SignOutAction
	{
		Refresh,
		Redirect,
		RedirectToLoginPage,
		FederatedPassiveSignOut
	}
}
