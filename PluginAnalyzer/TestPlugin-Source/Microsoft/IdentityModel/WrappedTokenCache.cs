using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel
{
	internal class WrappedTokenCache : SecurityTokenResolver, ISecurityContextSecurityTokenCache
	{
		private SecurityTokenCache _tokenCache;

		private SctClaimsHandler _claimsHandler;

		private bool _isSessionMode;

		public bool IsSessionMode => _isSessionMode;

		public WrappedTokenCache(SecurityTokenCache tokenCache, SctClaimsHandler sctClaimsHandler, bool isSessionMode)
		{
			if (tokenCache == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenCache");
			}
			if (sctClaimsHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sctClaimsHandler");
			}
			_tokenCache = tokenCache;
			_claimsHandler = sctClaimsHandler;
			_isSessionMode = isSessionMode;
		}

		public void AddContext(SecurityContextSecurityToken token)
		{
			_claimsHandler.SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(token);
			object key = new SecurityTokenCacheKey(_claimsHandler.EndpointId, token.ContextId, token.KeyGeneration, _isSessionMode);
			Microsoft.IdentityModel.Tokens.SessionSecurityToken value = new Microsoft.IdentityModel.Tokens.SessionSecurityToken(token, SecureConversationVersion.Default);
			if (!_tokenCache.TryAddEntry(key, value))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4234"));
			}
		}

		public void ClearContexts()
		{
			SecurityTokenCacheKey securityTokenCacheKey = new SecurityTokenCacheKey(_claimsHandler.EndpointId, null, null, _isSessionMode);
			securityTokenCacheKey.CanIgnoreContextId = true;
			securityTokenCacheKey.CanIgnoreKeyGeneration = true;
			_tokenCache.TryRemoveAllEntries(securityTokenCacheKey);
		}

		public Collection<SecurityContextSecurityToken> GetAllContexts(System.Xml.UniqueId contextId)
		{
			SecurityTokenCacheKey securityTokenCacheKey = new SecurityTokenCacheKey(_claimsHandler.EndpointId, contextId, null, _isSessionMode);
			securityTokenCacheKey.CanIgnoreKeyGeneration = true;
			Collection<SecurityContextSecurityToken> collection = new Collection<SecurityContextSecurityToken>();
			IList<SecurityToken> tokens = null;
			_tokenCache.TryGetAllEntries(securityTokenCacheKey, out tokens);
			if (tokens != null)
			{
				for (int i = 0; i < tokens.Count; i++)
				{
					SecurityContextSecurityToken securityContextSecurityToken = tokens[i] as SecurityContextSecurityToken;
					if (securityContextSecurityToken == null)
					{
						Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken = tokens[i] as Microsoft.IdentityModel.Tokens.SessionSecurityToken;
						if (sessionSecurityToken != null && sessionSecurityToken.IsSecurityContextSecurityTokenWrapper)
						{
							securityContextSecurityToken = sessionSecurityToken.SecurityContextSecurityToken;
						}
					}
					if (securityContextSecurityToken != null)
					{
						collection.Add(securityContextSecurityToken);
					}
				}
			}
			return collection;
		}

		public SecurityContextSecurityToken GetContext(System.Xml.UniqueId contextId, System.Xml.UniqueId generation)
		{
			object obj = new SecurityTokenCacheKey(_claimsHandler.EndpointId, contextId, generation, _isSessionMode);
			if (obj == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4286"));
			}
			_tokenCache.TryGetEntry(obj, out var value);
			SecurityContextSecurityToken securityContextSecurityToken = value as SecurityContextSecurityToken;
			if (securityContextSecurityToken == null)
			{
				Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken = value as Microsoft.IdentityModel.Tokens.SessionSecurityToken;
				if (sessionSecurityToken != null && sessionSecurityToken.IsSecurityContextSecurityTokenWrapper)
				{
					securityContextSecurityToken = sessionSecurityToken.SecurityContextSecurityToken;
				}
			}
			return securityContextSecurityToken;
		}

		public void RemoveAllContexts(System.Xml.UniqueId contextId)
		{
			SecurityTokenCacheKey securityTokenCacheKey = new SecurityTokenCacheKey(_claimsHandler.EndpointId, contextId, null, _isSessionMode);
			securityTokenCacheKey.CanIgnoreKeyGeneration = true;
			_tokenCache.TryRemoveAllEntries(securityTokenCacheKey);
		}

		public void RemoveContext(System.Xml.UniqueId contextId, System.Xml.UniqueId generation)
		{
			object obj = new SecurityTokenCacheKey(_claimsHandler.EndpointId, contextId, generation, _isSessionMode);
			if (obj == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4286"));
			}
			_tokenCache.TryRemoveEntry(obj);
		}

		public bool TryAddContext(SecurityContextSecurityToken token)
		{
			_claimsHandler.SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(token);
			object obj = new SecurityTokenCacheKey(_claimsHandler.EndpointId, token.ContextId, token.KeyGeneration, _isSessionMode);
			if (obj == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4286"));
			}
			Microsoft.IdentityModel.Tokens.SessionSecurityToken value = new Microsoft.IdentityModel.Tokens.SessionSecurityToken(token, SecureConversationVersion.Default);
			return _tokenCache.TryAddEntry(obj, value);
		}

		public void UpdateContextCachingTime(SecurityContextSecurityToken token, DateTime expirationTime)
		{
			if (!(token.ValidTo <= expirationTime.ToUniversalTime()))
			{
				object obj = new SecurityTokenCacheKey(_claimsHandler.EndpointId, token.ContextId, token.KeyGeneration, _isSessionMode);
				if (obj == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4286"));
				}
				Microsoft.IdentityModel.Tokens.SessionSecurityToken newValue = new Microsoft.IdentityModel.Tokens.SessionSecurityToken(token, SecureConversationVersion.Default);
				if (!_tokenCache.TryReplaceEntry(obj, newValue))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4285", token.ContextId.ToString()));
				}
			}
		}

		protected override bool TryResolveSecurityKeyCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key)
		{
			if (TryResolveTokenCore(keyIdentifierClause, out var token))
			{
				key = ((SecurityContextSecurityToken)token).SecurityKeys[0];
				return true;
			}
			key = null;
			return false;
		}

		protected override bool TryResolveTokenCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityToken token)
		{
			SecurityContextKeyIdentifierClause securityContextKeyIdentifierClause = keyIdentifierClause as SecurityContextKeyIdentifierClause;
			if (securityContextKeyIdentifierClause != null)
			{
				token = GetContext(securityContextKeyIdentifierClause.ContextId, securityContextKeyIdentifierClause.Generation);
			}
			else
			{
				token = null;
			}
			return token != null;
		}

		protected override bool TryResolveTokenCore(SecurityKeyIdentifier keyIdentifier, out SecurityToken token)
		{
			if (keyIdentifier.TryFind<SecurityContextKeyIdentifierClause>(out var clause))
			{
				return TryResolveTokenCore(clause, out token);
			}
			token = null;
			return false;
		}
	}
}
