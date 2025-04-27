using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	internal class SctClaimsHandler
	{
		private static Type s_type;

		private static Assembly s_assembly;

		private BindingFlags setFieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField;

		private ClaimsAuthenticationManager _claimsAuthenticationManager;

		private SecurityTokenHandlerCollection _securityTokenHandlerCollection;

		private string _endpointId;

		public string EndpointId => _endpointId;

		public SecurityTokenHandlerCollection SecurityTokenHandlerCollection => _securityTokenHandlerCollection;

		public SctClaimsHandler(ClaimsAuthenticationManager claimsAuthenticationManager, SecurityTokenHandlerCollection securityTokenHandlerCollection, string endpointId)
		{
			if (claimsAuthenticationManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claimsAuthenticationManager");
			}
			if (securityTokenHandlerCollection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlerCollection");
			}
			if (endpointId == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("endpointId");
			}
			_claimsAuthenticationManager = claimsAuthenticationManager;
			_securityTokenHandlerCollection = securityTokenHandlerCollection;
			_endpointId = endpointId;
		}

		internal void SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(SecurityContextSecurityToken sct)
		{
			if (sct == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sct");
			}
			List<IAuthorizationPolicy> list = new List<IAuthorizationPolicy>();
			if (sct.AuthorizationPolicies == null || sct.AuthorizationPolicies.Count <= 0 || !ContainsEndpointAuthPolicy(sct.AuthorizationPolicies))
			{
				if (sct.AuthorizationPolicies != null && sct.AuthorizationPolicies.Count > 0)
				{
					AuthorizationPolicy item = IdentityModelServiceAuthorizationManager.TransformAuthorizationPolicies(sct.AuthorizationPolicies, _claimsAuthenticationManager, _securityTokenHandlerCollection, includeTransportTokens: false);
					list.Add(item);
					System.IdentityModel.Claims.Claim primaryIdentityClaim = GetPrimaryIdentityClaim(System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext(sct.AuthorizationPolicies));
					SctAuthorizationPolicy item2 = new SctAuthorizationPolicy(primaryIdentityClaim);
					list.Add(item2);
				}
				list.Add(new EndpointAuthorizationPolicy(_endpointId));
				ReplaceAuthPolicies(sct, list.AsReadOnly());
			}
		}

		private bool ContainsEndpointAuthPolicy(ReadOnlyCollection<IAuthorizationPolicy> policies)
		{
			if (policies == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("policies");
			}
			for (int i = 0; i < policies.Count; i++)
			{
				if (policies[i] is EndpointAuthorizationPolicy)
				{
					return true;
				}
			}
			return false;
		}

		private System.IdentityModel.Claims.Claim GetPrimaryIdentityClaim(System.IdentityModel.Policy.AuthorizationContext authContext)
		{
			if (authContext != null)
			{
				for (int i = 0; i < authContext.ClaimSets.Count; i++)
				{
					ClaimSet claimSet = authContext.ClaimSets[i];
					using IEnumerator<System.IdentityModel.Claims.Claim> enumerator = claimSet.FindClaims(null, Rights.Identity).GetEnumerator();
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
			}
			return null;
		}

		private void ReplaceAuthPolicies(SecurityContextSecurityToken sct, ReadOnlyCollection<IAuthorizationPolicy> policies)
		{
			if ((object)s_assembly == null)
			{
				s_assembly = typeof(SecurityContextSecurityToken).Assembly;
			}
			if ((object)s_type == null)
			{
				s_type = s_assembly.GetType("System.ServiceModel.Security.Tokens.SecurityContextSecurityToken");
			}
			if ((object)s_type != null)
			{
				s_type.InvokeMember("authorizationPolicies", setFieldFlags, null, sct, new object[1] { policies }, CultureInfo.InvariantCulture);
			}
		}

		public void OnTokenIssued(SecurityToken issuedToken, EndpointAddress tokenRequestor)
		{
			SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(issuedToken as SecurityContextSecurityToken);
		}

		public void OnTokenRenewed(SecurityToken issuedToken, SecurityToken oldToken)
		{
			SetPrincipalBootstrapTokensAndBindIdfxAuthPolicy(issuedToken as SecurityContextSecurityToken);
		}
	}
}
