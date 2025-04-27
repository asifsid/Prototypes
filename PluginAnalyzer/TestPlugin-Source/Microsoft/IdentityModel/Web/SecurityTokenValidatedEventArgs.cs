using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class SecurityTokenValidatedEventArgs : CancelEventArgs
	{
		private IClaimsPrincipal _claimsPrincipal;

		public IClaimsPrincipal ClaimsPrincipal
		{
			get
			{
				return _claimsPrincipal;
			}
			[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
			set
			{
				_claimsPrincipal = value;
			}
		}

		public SecurityTokenValidatedEventArgs(IClaimsPrincipal claimsPrincipal)
		{
			if (claimsPrincipal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claimsPrincipal");
			}
			_claimsPrincipal = claimsPrincipal;
		}
	}
}
