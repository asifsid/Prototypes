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
	internal class WrappedRsaSecurityTokenAuthenticator : RsaSecurityTokenAuthenticator
	{
		private RsaSecurityTokenHandler _wrappedRsaSecurityTokenHandler;

		private SecurityTokenRequirement _securityTokenRequirement;

		private ClaimsAuthenticationManager _claimsAuthenticationManager;

		private ExceptionMapper _exceptionMapper;

		public WrappedRsaSecurityTokenAuthenticator(RsaSecurityTokenHandler wrappedRsaSecurityTokenHandler, SecurityTokenRequirement securityTokenRequirement, ClaimsAuthenticationManager claimsAuthenticationManager, ExceptionMapper exceptionMapper)
		{
			if (wrappedRsaSecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wrappedRsaSecurityTokenHandler");
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
			_wrappedRsaSecurityTokenHandler = wrappedRsaSecurityTokenHandler;
			_securityTokenRequirement = securityTokenRequirement;
			_claimsAuthenticationManager = claimsAuthenticationManager;
			_exceptionMapper = exceptionMapper;
		}

		protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
		{
			ClaimsIdentityCollection identityCollection = null;
			try
			{
				identityCollection = _wrappedRsaSecurityTokenHandler.ValidateToken(token);
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
