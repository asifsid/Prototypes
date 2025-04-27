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
	internal class WrappedX509SecurityTokenAuthenticator : X509SecurityTokenAuthenticator
	{
		private X509SecurityTokenHandler _wrappedX509SecurityTokenHandler;

		private SecurityTokenRequirement _securityTokenRequirement;

		private ClaimsAuthenticationManager _claimsAuthenticationManager;

		private ExceptionMapper _exceptionMapper;

		public WrappedX509SecurityTokenAuthenticator(X509SecurityTokenHandler wrappedX509SecurityTokenHandler, SecurityTokenRequirement securityTokenRequirement, ClaimsAuthenticationManager claimsAuthenticationManager, ExceptionMapper exceptionMapper)
			: base(X509CertificateValidator.None, GetMapToWindowsSetting(wrappedX509SecurityTokenHandler), includeWindowsGroups: true)
		{
			if (wrappedX509SecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wrappedX509SecurityTokenHandler");
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
			_wrappedX509SecurityTokenHandler = wrappedX509SecurityTokenHandler;
			_securityTokenRequirement = securityTokenRequirement;
			_claimsAuthenticationManager = claimsAuthenticationManager;
			_exceptionMapper = exceptionMapper;
		}

		protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
		{
			ClaimsIdentityCollection claimsIdentityCollection = null;
			try
			{
				claimsIdentityCollection = _wrappedX509SecurityTokenHandler.ValidateToken(token);
			}
			catch (Exception ex)
			{
				if (!_exceptionMapper.HandleSecurityTokenProcessingException(ex))
				{
					throw;
				}
			}
			RecipientServiceModelSecurityTokenRequirement recipientServiceModelSecurityTokenRequirement = _securityTokenRequirement as RecipientServiceModelSecurityTokenRequirement;
			if (recipientServiceModelSecurityTokenRequirement != null)
			{
				string resourceName = null;
				if (recipientServiceModelSecurityTokenRequirement.ListenUri != null)
				{
					resourceName = recipientServiceModelSecurityTokenRequirement.ListenUri.AbsoluteUri;
				}
				IClaimsPrincipal claimsPrincipal = _claimsAuthenticationManager.Authenticate(resourceName, new ClaimsPrincipal(claimsIdentityCollection));
				claimsIdentityCollection = ((claimsPrincipal != null) ? claimsPrincipal.Identities : new ClaimsIdentityCollection());
			}
			bool flag = SecurityTokenHandlerConfiguration.DefaultSaveBootstrapTokens;
			if (_wrappedX509SecurityTokenHandler.Configuration != null)
			{
				flag = _wrappedX509SecurityTokenHandler.Configuration.SaveBootstrapTokens;
			}
			if (flag)
			{
				X509SecurityToken x509SecurityToken = token as X509SecurityToken;
				SecurityToken bootstrapToken = ((x509SecurityToken == null) ? token : new X509SecurityToken(x509SecurityToken.Certificate));
				foreach (IClaimsIdentity item in claimsIdentityCollection)
				{
					item.BootstrapToken = bootstrapToken;
				}
			}
			return new List<IAuthorizationPolicy>(new AuthorizationPolicy[1]
			{
				new AuthorizationPolicy(claimsIdentityCollection)
			}).AsReadOnly();
		}

		private static bool GetMapToWindowsSetting(X509SecurityTokenHandler securityTokenHandler)
		{
			if (securityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandler");
			}
			return securityTokenHandler.MapToWindows;
		}
	}
}
