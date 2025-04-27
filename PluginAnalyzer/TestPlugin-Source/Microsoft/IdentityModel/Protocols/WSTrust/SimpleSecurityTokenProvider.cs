using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class SimpleSecurityTokenProvider : SecurityTokenProvider
	{
		private SecurityToken _securityToken;

		public SimpleSecurityTokenProvider(SecurityToken token, SecurityTokenRequirement tokenRequirement)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			GenericXmlSecurityToken genericXmlSecurityToken = token as GenericXmlSecurityToken;
			if (genericXmlSecurityToken != null)
			{
				_securityToken = WrapWithAuthPolicy(genericXmlSecurityToken, tokenRequirement);
			}
			else
			{
				_securityToken = token;
			}
		}

		protected override SecurityToken GetTokenCore(TimeSpan timeout)
		{
			return _securityToken;
		}

		private static GenericXmlSecurityToken WrapWithAuthPolicy(GenericXmlSecurityToken issuedToken, SecurityTokenRequirement tokenRequirement)
		{
			EndpointIdentity endpointIdentity = null;
			InitiatorServiceModelSecurityTokenRequirement initiatorServiceModelSecurityTokenRequirement = tokenRequirement as InitiatorServiceModelSecurityTokenRequirement;
			if (initiatorServiceModelSecurityTokenRequirement != null)
			{
				EndpointAddress targetAddress = initiatorServiceModelSecurityTokenRequirement.TargetAddress;
				if (targetAddress.Uri.IsAbsoluteUri)
				{
					endpointIdentity = EndpointIdentity.CreateDnsIdentity(targetAddress.Uri.DnsSafeHost);
				}
			}
			ReadOnlyCollection<IAuthorizationPolicy> serviceAuthorizationPolicies = GetServiceAuthorizationPolicies(endpointIdentity);
			return new GenericXmlSecurityToken(issuedToken.TokenXml, issuedToken.ProofToken, issuedToken.ValidFrom, issuedToken.ValidTo, issuedToken.InternalTokenReference, issuedToken.ExternalTokenReference, serviceAuthorizationPolicies);
		}

		private static ReadOnlyCollection<IAuthorizationPolicy> GetServiceAuthorizationPolicies(EndpointIdentity endpointIdentity)
		{
			if (endpointIdentity != null)
			{
				List<System.IdentityModel.Claims.Claim> list = new List<System.IdentityModel.Claims.Claim>(1);
				list.Add(endpointIdentity.IdentityClaim);
				List<IAuthorizationPolicy> list2 = new List<IAuthorizationPolicy>(1);
				List<ClaimSet> list3 = new List<ClaimSet>();
				list3.Add(new DefaultClaimSet(list));
				List<ClaimSet> list4 = list3;
				list2.Add(new ClaimFactoryPolicy(new ReadOnlyCollection<ClaimSet>(list4)));
				return list2.AsReadOnly();
			}
			return EmptyReadOnlyCollection<IAuthorizationPolicy>.Instance;
		}
	}
}
