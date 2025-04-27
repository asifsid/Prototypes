using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Web
{
	internal class SessionSecurityTokenResolver : SecurityTokenResolver
	{
		private SecurityTokenCache _tokenCache;

		private string _endpointId;

		private bool _isSessionMode;

		internal SessionSecurityTokenResolver(SecurityTokenCache tokenCache, string endpointId, bool isSessionMode)
		{
			if (tokenCache == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenCache");
			}
			if (endpointId == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("endpointId");
			}
			_tokenCache = tokenCache;
			_endpointId = endpointId;
			_isSessionMode = isSessionMode;
		}

		protected override bool TryResolveSecurityKeyCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key)
		{
			key = null;
			if (TryResolveTokenCore(keyIdentifierClause, out var token))
			{
				key = token.SecurityKeys[0];
				return true;
			}
			return false;
		}

		protected override bool TryResolveTokenCore(SecurityKeyIdentifier keyIdentifier, out SecurityToken token)
		{
			token = null;
			if (keyIdentifier.TryFind<SecurityContextKeyIdentifierClause>(out var clause))
			{
				return TryResolveTokenCore(clause, out token);
			}
			return false;
		}

		protected override bool TryResolveTokenCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityToken token)
		{
			SecurityContextKeyIdentifierClause securityContextKeyIdentifierClause = keyIdentifierClause as SecurityContextKeyIdentifierClause;
			token = null;
			if (securityContextKeyIdentifierClause != null)
			{
				SecurityTokenCacheKey key = new SecurityTokenCacheKey(_endpointId, securityContextKeyIdentifierClause.ContextId, securityContextKeyIdentifierClause.Generation, _isSessionMode);
				_tokenCache.TryGetEntry(key, out token);
			}
			return token != null;
		}
	}
}
