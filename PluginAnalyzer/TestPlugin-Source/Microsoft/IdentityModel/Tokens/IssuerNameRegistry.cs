using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public abstract class IssuerNameRegistry
	{
		public abstract string GetIssuerName(SecurityToken securityToken);

		public virtual string GetIssuerName(SecurityToken securityToken, string requestedIssuerName)
		{
			return GetIssuerName(securityToken);
		}

		public virtual string GetWindowsIssuerName()
		{
			return "LOCAL AUTHORITY";
		}
	}
}
