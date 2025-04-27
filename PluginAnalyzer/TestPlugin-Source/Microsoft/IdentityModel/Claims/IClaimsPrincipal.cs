using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public interface IClaimsPrincipal : IPrincipal
	{
		ClaimsIdentityCollection Identities { get; }

		IClaimsPrincipal Copy();
	}
}
