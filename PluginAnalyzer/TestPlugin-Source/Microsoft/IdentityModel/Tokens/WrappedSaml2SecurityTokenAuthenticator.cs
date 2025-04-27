using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens
{
	internal class WrappedSaml2SecurityTokenAuthenticator : SecurityTokenAuthenticator
	{
		private Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler _wrappedSaml2SecurityTokenHandler;

		private SecurityTokenRequirement _securityTokenRequirement;

		private ClaimsAuthenticationManager _claimsAuthenticationManager;

		private ExceptionMapper _exceptionMapper;

		public WrappedSaml2SecurityTokenAuthenticator(Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler saml2SecurityTokenHandler, SecurityTokenRequirement securityTokenRequirement, ClaimsAuthenticationManager claimsAuthenticationManager, ExceptionMapper exceptionMapper)
		{
			if (saml2SecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wrappedSaml2SecurityTokenHandler");
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
			_wrappedSaml2SecurityTokenHandler = saml2SecurityTokenHandler;
			_securityTokenRequirement = securityTokenRequirement;
			_claimsAuthenticationManager = claimsAuthenticationManager;
			_exceptionMapper = exceptionMapper;
		}

		protected override bool CanValidateTokenCore(SecurityToken token)
		{
			if (token is Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityToken)
			{
				return _wrappedSaml2SecurityTokenHandler.CanValidateToken;
			}
			return false;
		}

		protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
		{
			ClaimsIdentityCollection identityCollection = null;
			try
			{
				identityCollection = _wrappedSaml2SecurityTokenHandler.ValidateToken(token);
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
