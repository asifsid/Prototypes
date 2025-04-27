using System;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class FederatedClientCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
	{
		private FederatedClientCredentials _federatedClientCredentials;

		public FederatedClientCredentialsSecurityTokenManager(FederatedClientCredentials federatedClientCredentials)
			: base(federatedClientCredentials)
		{
			_federatedClientCredentials = federatedClientCredentials;
		}

		public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement tokenRequirement)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = FindIssuedTokenClientCredentialsParameters(tokenRequirement);
			if (federatedClientCredentialsParameters == null)
			{
				federatedClientCredentialsParameters = new FederatedClientCredentialsParameters();
			}
			if (federatedClientCredentialsParameters.IssuedSecurityToken != null && IsIssuedSecurityTokenRequirement(tokenRequirement) && !IsNegoOrSCTIssuedToken(tokenRequirement))
			{
				return new SimpleSecurityTokenProvider(federatedClientCredentialsParameters.IssuedSecurityToken, tokenRequirement);
			}
			SecurityTokenProvider securityTokenProvider = base.CreateSecurityTokenProvider(tokenRequirement);
			IssuedSecurityTokenProvider issuedSecurityTokenProvider = securityTokenProvider as IssuedSecurityTokenProvider;
			if (issuedSecurityTokenProvider != null)
			{
				return new FederatedSecurityTokenProvider(federatedClientCredentialsParameters, issuedSecurityTokenProvider, _federatedClientCredentials.SecurityTokenHandlerCollectionManager);
			}
			return securityTokenProvider;
		}

		public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
		{
			if (version == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("version");
			}
			TrustVersion trustVersion = TrustVersion.WSTrust13;
			SecureConversationVersion secureConversationVersion = SecureConversationVersion.WSSecureConversation13;
			_ = SecurityVersion.WSSecurity11;
			foreach (string securitySpecification in version.GetSecuritySpecifications())
			{
				if (StringComparer.Ordinal.Equals(securitySpecification, "http://schemas.xmlsoap.org/ws/2005/02/trust"))
				{
					trustVersion = TrustVersion.WSTrustFeb2005;
				}
				else if (StringComparer.Ordinal.Equals(securitySpecification, "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
				{
					trustVersion = TrustVersion.WSTrust13;
				}
				else if (StringComparer.Ordinal.Equals(securitySpecification, "http://schemas.xmlsoap.org/ws/2005/02/sc"))
				{
					secureConversationVersion = SecureConversationVersion.WSSecureConversationFeb2005;
				}
				else if (StringComparer.Ordinal.Equals(securitySpecification, "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512"))
				{
					secureConversationVersion = SecureConversationVersion.WSSecureConversation13;
				}
			}
			SecurityVersion securityVersion = FederatedSecurityTokenManager.GetSecurityVersion(version);
			FederatedClientCredentials federatedClientCredentials = base.ClientCredentials as FederatedClientCredentials;
			SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager = ((federatedClientCredentials != null) ? federatedClientCredentials.SecurityTokenHandlerCollectionManager : SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager());
			return new SecurityTokenSerializerAdapter(securityTokenHandlerCollectionManager[""], securityVersion, trustVersion, secureConversationVersion, emitBspAttributes: false, null, null, null);
		}

		internal static FederatedClientCredentialsParameters FindIssuedTokenClientCredentialsParameters(SecurityTokenRequirement tokenRequirement)
		{
			FederatedClientCredentialsParameters federatedClientCredentialsParameters = null;
			ChannelParameterCollection result = null;
			if (tokenRequirement.TryGetProperty<ChannelParameterCollection>(ServiceModelSecurityTokenRequirement.ChannelParametersCollectionProperty, out result) && result != null)
			{
				foreach (object item in result)
				{
					federatedClientCredentialsParameters = item as FederatedClientCredentialsParameters;
					if (federatedClientCredentialsParameters != null)
					{
						return federatedClientCredentialsParameters;
					}
				}
				return federatedClientCredentialsParameters;
			}
			return federatedClientCredentialsParameters;
		}

		internal static bool IsNegoOrSCTIssuedToken(SecurityTokenRequirement tokenRequirement)
		{
			string tokenType = tokenRequirement.TokenType;
			if (!(tokenType == ServiceModelSecurityTokenTypes.AnonymousSslnego) && !(tokenType == ServiceModelSecurityTokenTypes.MutualSslnego) && !(tokenType == ServiceModelSecurityTokenTypes.SecureConversation))
			{
				return tokenType == ServiceModelSecurityTokenTypes.Spnego;
			}
			return true;
		}
	}
}
