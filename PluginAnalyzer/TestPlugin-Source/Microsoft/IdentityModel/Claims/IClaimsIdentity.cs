using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public interface IClaimsIdentity : IIdentity
	{
		ClaimCollection Claims { get; }

		IClaimsIdentity Actor { get; set; }

		string Label { get; set; }

		string NameClaimType { get; set; }

		string RoleClaimType { get; set; }

		SecurityToken BootstrapToken { get; set; }

		IClaimsIdentity Copy();
	}
}
