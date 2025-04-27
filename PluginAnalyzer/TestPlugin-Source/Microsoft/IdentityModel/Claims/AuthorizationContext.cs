using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public class AuthorizationContext
	{
		private Collection<Claim> _action = new Collection<Claim>();

		private Collection<Claim> _resource = new Collection<Claim>();

		private IClaimsPrincipal _principal;

		public Collection<Claim> Action => _action;

		public Collection<Claim> Resource => _resource;

		public IClaimsPrincipal Principal => _principal;

		public AuthorizationContext(IClaimsPrincipal principal, string resource, string action)
		{
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			if (string.IsNullOrEmpty(resource))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("resource");
			}
			_principal = principal;
			_resource.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", resource));
			if (action != null)
			{
				_action.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", action));
			}
		}

		public AuthorizationContext(IClaimsPrincipal principal, Collection<Claim> resource, Collection<Claim> action)
		{
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			if (resource == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("resource");
			}
			if (action == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("action");
			}
			_principal = principal;
			_resource = resource;
			_action = action;
		}
	}
}
