using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens.Saml11;

namespace Microsoft.IdentityModel.Tokens
{
	internal class WrappedSaml11SecurityTokenAuthenticator : SamlSecurityTokenAuthenticator
	{
		private Saml11SecurityTokenHandler _wrappedSaml11SecurityTokenHandler;

		private SecurityTokenRequirement _securityTokenRequirement;

		private ClaimsAuthenticationManager _claimsAuthenticationManager;

		private ExceptionMapper _exceptionMapper;

		public WrappedSaml11SecurityTokenAuthenticator(Saml11SecurityTokenHandler saml11SecurityTokenHandler, SecurityTokenRequirement securityTokenRequirement, ClaimsAuthenticationManager claimsAuthenticationManager, ExceptionMapper exceptionMapper)
			: base(new List<SecurityTokenAuthenticator>())
		{
			if (saml11SecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wrappedSaml11SecurityTokenHandler");
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
			_wrappedSaml11SecurityTokenHandler = saml11SecurityTokenHandler;
			_securityTokenRequirement = securityTokenRequirement;
			_claimsAuthenticationManager = claimsAuthenticationManager;
			_exceptionMapper = exceptionMapper;
		}

		protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
		{
			ClaimsIdentityCollection identityCollection = null;
			try
			{
				identityCollection = _wrappedSaml11SecurityTokenHandler.ValidateToken(token);
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
