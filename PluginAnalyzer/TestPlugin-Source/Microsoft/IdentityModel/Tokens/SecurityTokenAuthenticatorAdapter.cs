using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	internal class SecurityTokenAuthenticatorAdapter : SecurityTokenAuthenticator
	{
		private SecurityTokenHandler _securityTokenHandler;

		private SecurityTokenRequirement _securityTokenRequirement;

		private ClaimsAuthenticationManager _claimsAuthenticationManager;

		private ExceptionMapper _exceptionMapper;

		public SecurityTokenAuthenticatorAdapter(SecurityTokenHandler securityTokenHandler, SecurityTokenRequirement securityTokenRequirement, ClaimsAuthenticationManager claimsAuthenticationManager, ExceptionMapper exceptionMapper)
		{
			if (securityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandler");
			}
			if (securityTokenRequirement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenRequirement");
			}
			if (claimsAuthenticationManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claimsAuthenticationManager");
			}
			if (exceptionMapper == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("exceptionMapper");
			}
			_securityTokenHandler = securityTokenHandler;
			_securityTokenRequirement = securityTokenRequirement;
			_claimsAuthenticationManager = claimsAuthenticationManager;
			_exceptionMapper = exceptionMapper;
		}

		protected override bool CanValidateTokenCore(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if ((object)token.GetType() == _securityTokenHandler.TokenType)
			{
				return _securityTokenHandler.CanValidateToken;
			}
			return false;
		}

		protected sealed override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
		{
			ClaimsIdentityCollection identityCollection = null;
			try
			{
				identityCollection = _securityTokenHandler.ValidateToken(token);
			}
			catch (Exception ex)
			{
				if (!_exceptionMapper.HandleSecurityTokenProcessingException(ex))
				{
					throw;
				}
			}
			RecipientServiceModelSecurityTokenRequirement recipientServiceModelSecurityTokenRequirement = _securityTokenRequirement as RecipientServiceModelSecurityTokenRequirement;
			string resourceName = null;
			if (recipientServiceModelSecurityTokenRequirement != null && recipientServiceModelSecurityTokenRequirement.ListenUri != null)
			{
				resourceName = recipientServiceModelSecurityTokenRequirement.ListenUri.AbsoluteUri;
			}
			IClaimsPrincipal claimsPrincipal = _claimsAuthenticationManager.Authenticate(resourceName, new ClaimsPrincipal(identityCollection));
			identityCollection = ((claimsPrincipal != null) ? claimsPrincipal.Identities : new ClaimsIdentityCollection());
			return new List<IAuthorizationPolicy>(new AuthorizationPolicy[1]
			{
				new AuthorizationPolicy(identityCollection)
			}).AsReadOnly();
		}
	}
}
