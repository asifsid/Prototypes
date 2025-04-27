using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
	internal static class EmptySecurityTokenResolver
	{
		private static readonly SecurityTokenResolver _instance = SecurityTokenResolver.CreateDefaultSecurityTokenResolver(EmptyReadOnlyCollection<SecurityToken>.Instance, canMatchLocalId: false);

		public static SecurityTokenResolver Instance => _instance;
	}
}
