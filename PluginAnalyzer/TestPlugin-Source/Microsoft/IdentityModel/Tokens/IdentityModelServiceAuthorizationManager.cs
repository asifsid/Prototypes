using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class IdentityModelServiceAuthorizationManager : ServiceAuthorizationManager
	{
		protected static readonly ReadOnlyCollection<IAuthorizationPolicy> AnonymousAuthorizationPolicy = new ReadOnlyCollection<IAuthorizationPolicy>(new List<IAuthorizationPolicy>
		{
			new AuthorizationPolicy(ClaimsIdentity.AnonymousIdentity)
		});

		protected override ReadOnlyCollection<IAuthorizationPolicy> GetAuthorizationPolicies(OperationContext operationContext)
		{
			ReadOnlyCollection<IAuthorizationPolicy> authorizationPolicies = base.GetAuthorizationPolicies(operationContext);
			if (authorizationPolicies == null)
			{
				return AnonymousAuthorizationPolicy;
			}
			FederatedServiceCredentials federatedServiceCredentials = GetFederatedServiceCredentials();
			AuthorizationPolicy authorizationPolicy = TransformAuthorizationPolicies(authorizationPolicies, federatedServiceCredentials.ClaimsAuthenticationManager, federatedServiceCredentials.SecurityTokenHandlers, includeTransportTokens: true);
			if (authorizationPolicy == null || authorizationPolicy.IdentityCollection.Count == 0)
			{
				return AnonymousAuthorizationPolicy;
			}
			List<IAuthorizationPolicy> list = new List<IAuthorizationPolicy>();
			list.Add(authorizationPolicy);
			return list.AsReadOnly();
		}

		internal static AuthorizationPolicy TransformAuthorizationPolicies(ReadOnlyCollection<IAuthorizationPolicy> baseAuthorizationPolicies, ClaimsAuthenticationManager authnManager, SecurityTokenHandlerCollection securityTokenHandlerCollection, bool includeTransportTokens)
		{
			ClaimsIdentityCollection claimsIdentityCollection = new ClaimsIdentityCollection();
			List<IAuthorizationPolicy> list = new List<IAuthorizationPolicy>();
			foreach (IAuthorizationPolicy baseAuthorizationPolicy in baseAuthorizationPolicies)
			{
				if (!(baseAuthorizationPolicy is SctAuthorizationPolicy) && !(baseAuthorizationPolicy is EndpointAuthorizationPolicy))
				{
					AuthorizationPolicy authorizationPolicy = baseAuthorizationPolicy as AuthorizationPolicy;
					if (authorizationPolicy != null)
					{
						claimsIdentityCollection.AddRange(authorizationPolicy.IdentityCollection);
					}
					else
					{
						list.Add(baseAuthorizationPolicy);
					}
				}
			}
			if (includeTransportTokens && OperationContext.Current != null && OperationContext.Current.IncomingMessageProperties != null && OperationContext.Current.IncomingMessageProperties.Security != null && OperationContext.Current.IncomingMessageProperties.Security.TransportToken != null)
			{
				SecurityToken securityToken = OperationContext.Current.IncomingMessageProperties.Security.TransportToken.SecurityToken;
				ReadOnlyCollection<IAuthorizationPolicy> securityTokenPolicies = OperationContext.Current.IncomingMessageProperties.Security.TransportToken.SecurityTokenPolicies;
				bool flag = true;
				foreach (IAuthorizationPolicy item in securityTokenPolicies)
				{
					if (item is AuthorizationPolicy)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					ClaimsIdentityCollection transportTokenIdentities = GetTransportTokenIdentities(securityToken, authnManager);
					claimsIdentityCollection.AddRange(transportTokenIdentities);
					EliminateTransportTokenPolicy(securityToken, transportTokenIdentities, list);
				}
			}
			if (list.Count > 0)
			{
				claimsIdentityCollection.AddRange(ConvertToIDFxIdentities(list, authnManager, securityTokenHandlerCollection));
			}
			if (claimsIdentityCollection.Count == 0)
			{
				return new AuthorizationPolicy(ClaimsIdentity.AnonymousIdentity);
			}
			return new AuthorizationPolicy(claimsIdentityCollection);
		}

		private static ClaimsIdentityCollection GetTransportTokenIdentities(SecurityToken transportToken, ClaimsAuthenticationManager authnManager)
		{
			if (transportToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("transportToken");
			}
			if (authnManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("authnManager");
			}
			FederatedServiceCredentials federatedServiceCredentials = GetFederatedServiceCredentials();
			ClaimsIdentityCollection identityCollection = null;
			if (transportToken is X509SecurityToken || transportToken is UserNameSecurityToken)
			{
				identityCollection = federatedServiceCredentials.SecurityTokenHandlers.ValidateToken(transportToken);
			}
			WindowsSecurityToken windowsSecurityToken = transportToken as WindowsSecurityToken;
			if (windowsSecurityToken != null)
			{
				string windowsIssuerName = federatedServiceCredentials.SecurityTokenHandlers.Configuration.IssuerNameRegistry.GetWindowsIssuerName();
				WindowsClaimsIdentity windowsClaimsIdentity = new WindowsClaimsIdentity(windowsSecurityToken.WindowsIdentity.Token, "Windows", windowsIssuerName);
				AddAuthenticationMethod(windowsClaimsIdentity, "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows");
				AddAuthenticationInstantClaim(windowsClaimsIdentity, XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), windowsIssuerName);
				if (federatedServiceCredentials.SaveBootstrapTokens)
				{
					windowsClaimsIdentity.BootstrapToken = transportToken;
				}
				identityCollection = new ClaimsIdentityCollection(new IClaimsIdentity[1] { windowsClaimsIdentity });
			}
			IClaimsPrincipal claimsPrincipal = authnManager.Authenticate(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.AbsoluteUri, new ClaimsPrincipal(identityCollection));
			return claimsPrincipal.Identities;
		}

		private static void EliminateTransportTokenPolicy(SecurityToken transportToken, ClaimsIdentityCollection tranportTokenIdentities, List<IAuthorizationPolicy> baseAuthorizationPolicies)
		{
			if (transportToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("transportToken");
			}
			if (tranportTokenIdentities == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tranportTokenIdentities");
			}
			if (baseAuthorizationPolicies == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("baseAuthorizationPolicy");
			}
			if (baseAuthorizationPolicies.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("baseAuthorizationPolicy", SR.GetString("ID0020"));
			}
			IAuthorizationPolicy authorizationPolicy = null;
			foreach (IAuthorizationPolicy baseAuthorizationPolicy in baseAuthorizationPolicies)
			{
				if (DoesPolicyMatchTransportToken(transportToken, tranportTokenIdentities, baseAuthorizationPolicy))
				{
					authorizationPolicy = baseAuthorizationPolicy;
					break;
				}
			}
			if (authorizationPolicy == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4271", transportToken));
			}
			baseAuthorizationPolicies.Remove(authorizationPolicy);
		}

		private static bool DoesPolicyMatchTransportToken(SecurityToken transportToken, ClaimsIdentityCollection tranportTokenIdentities, IAuthorizationPolicy authPolicy)
		{
			if (transportToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("transportToken");
			}
			if (tranportTokenIdentities == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tranportTokenIdentities");
			}
			if (authPolicy == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("authPolicy");
			}
			X509SecurityToken x509SecurityToken = transportToken as X509SecurityToken;
			List<IAuthorizationPolicy> list = new List<IAuthorizationPolicy>();
			list.Add(authPolicy);
			System.IdentityModel.Policy.AuthorizationContext authorizationContext = System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext(list);
			foreach (ClaimSet claimSet in authorizationContext.ClaimSets)
			{
				if (x509SecurityToken != null)
				{
					if (claimSet.ContainsClaim(new System.IdentityModel.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint", x509SecurityToken.Certificate.GetCertHash(), Rights.PossessProperty)))
					{
						return true;
					}
					continue;
				}
				foreach (IClaimsIdentity tranportTokenIdentity in tranportTokenIdentities)
				{
					if (claimSet.ContainsClaim(new System.IdentityModel.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", tranportTokenIdentity.Name, Rights.PossessProperty), new ClaimStringValueComparer()))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static ClaimsIdentityCollection ConvertToIDFxIdentities(IList<IAuthorizationPolicy> authorizationPolicies, ClaimsAuthenticationManager authnManager, SecurityTokenHandlerCollection securityTokenHandlerCollection)
		{
			if (authorizationPolicies == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("authorizationPolicies");
			}
			if (authnManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("authnManager");
			}
			if (securityTokenHandlerCollection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlerCollection");
			}
			ClaimsIdentityCollection claimsIdentityCollection = new ClaimsIdentityCollection();
			SecurityTokenSpecification securityTokenSpecification = null;
			System.IdentityModel.Policy.AuthorizationContext authorizationContext = null;
			if (OperationContext.Current != null && OperationContext.Current.IncomingMessageProperties != null && OperationContext.Current.IncomingMessageProperties.Security != null)
			{
				SecurityMessageProperty security = OperationContext.Current.IncomingMessageProperties.Security;
				foreach (SecurityTokenSpecification item in new SecurityTokenSpecificationEnumerable(security))
				{
					if (item.SecurityToken is KerberosReceiverSecurityToken)
					{
						securityTokenSpecification = item;
						authorizationContext = System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext(securityTokenSpecification.SecurityTokenPolicies);
						break;
					}
				}
			}
			bool flag = false;
			foreach (IAuthorizationPolicy authorizationPolicy in authorizationPolicies)
			{
				IClaimsPrincipal claimsPrincipal = null;
				bool flag2 = false;
				if (securityTokenSpecification != null && !flag)
				{
					if (securityTokenSpecification.SecurityTokenPolicies.Contains(authorizationPolicy))
					{
						flag = true;
					}
					else
					{
						List<IAuthorizationPolicy> list = new List<IAuthorizationPolicy>();
						list.Add(authorizationPolicy);
						System.IdentityModel.Policy.AuthorizationContext authorizationContext2 = System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext(list);
						if (authorizationContext2.ClaimSets.Count == 1)
						{
							bool flag3 = true;
							foreach (System.IdentityModel.Claims.Claim item2 in authorizationContext2.ClaimSets[0])
							{
								if (!authorizationContext.ClaimSets[0].ContainsClaim(item2))
								{
									flag3 = false;
									break;
								}
							}
							flag = flag3;
						}
					}
					if (flag)
					{
						SecurityTokenHandler securityTokenHandler = securityTokenHandlerCollection[securityTokenSpecification.SecurityToken];
						if (securityTokenHandler != null && securityTokenHandler.CanValidateToken)
						{
							ClaimsIdentityCollection identityCollection = securityTokenHandler.ValidateToken(securityTokenSpecification.SecurityToken);
							claimsPrincipal = authnManager.Authenticate(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.AbsoluteUri, new ClaimsPrincipal(identityCollection));
							flag2 = true;
						}
					}
				}
				if (!flag2)
				{
					List<IAuthorizationPolicy> list2 = new List<IAuthorizationPolicy>();
					list2.Add(authorizationPolicy);
					System.IdentityModel.Policy.AuthorizationContext authorizationContext3 = System.IdentityModel.Policy.AuthorizationContext.CreateDefaultAuthorizationContext(list2);
					IClaimsIdentity claimsIdentity = ConvertToIDFxIdentity(authorizationContext3.ClaimSets, securityTokenHandlerCollection.Configuration);
					claimsPrincipal = authnManager.Authenticate(OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.AbsoluteUri, new ClaimsPrincipal(new IClaimsIdentity[1] { claimsIdentity }));
				}
				claimsIdentityCollection.AddRange(claimsPrincipal.Identities);
			}
			return claimsIdentityCollection;
		}

		private static IClaimsIdentity ConvertToIDFxIdentity(IList<ClaimSet> claimSets, SecurityTokenHandlerConfiguration securityTokenHandlerConfiguration)
		{
			if (claimSets == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claimSets");
			}
			IClaimsIdentity claimsIdentity = null;
			foreach (ClaimSet claimSet in claimSets)
			{
				WindowsClaimSet windowsClaimSet = claimSet as WindowsClaimSet;
				if (windowsClaimSet != null)
				{
					string windowsIssuerName = securityTokenHandlerConfiguration.IssuerNameRegistry.GetWindowsIssuerName();
					claimsIdentity = MergeClaims(claimsIdentity, new WindowsClaimsIdentity(windowsClaimSet.WindowsIdentity.Token, "Negotiate", windowsIssuerName));
					AddAuthenticationMethod(claimsIdentity, "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows");
					AddAuthenticationInstantClaim(claimsIdentity, XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), windowsIssuerName);
				}
				else
				{
					claimsIdentity = MergeClaims(claimsIdentity, new ClaimsIdentity(claimSet));
					AddAuthenticationInstantClaim(claimsIdentity, XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated));
				}
			}
			return claimsIdentity;
		}

		private static FederatedServiceCredentials GetFederatedServiceCredentials()
		{
			ServiceCredentials serviceCredentials = null;
			if (OperationContext.Current != null && OperationContext.Current.Host != null && OperationContext.Current.Host.Description != null && OperationContext.Current.Host.Description.Behaviors != null)
			{
				serviceCredentials = OperationContext.Current.Host.Description.Behaviors.Find<ServiceCredentials>();
			}
			FederatedServiceCredentials federatedServiceCredentials = serviceCredentials as FederatedServiceCredentials;
			if (federatedServiceCredentials == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4267", serviceCredentials));
			}
			return federatedServiceCredentials;
		}

		private static void AddAuthenticationMethod(IClaimsIdentity claimsIdentity, string authenticationMethod)
		{
			Microsoft.IdentityModel.Claims.Claim claim2 = claimsIdentity.Claims.FirstOrDefault((Microsoft.IdentityModel.Claims.Claim claim) => claim.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod");
			if (claim2 == null)
			{
				claimsIdentity.Claims.Add(new Microsoft.IdentityModel.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", authenticationMethod));
			}
		}

		private static void AddAuthenticationInstantClaim(IClaimsIdentity claimsIdentity, string authenticationInstant)
		{
			AddAuthenticationInstantClaim(claimsIdentity, authenticationInstant, "LOCAL AUTHORITY");
		}

		private static void AddAuthenticationInstantClaim(IClaimsIdentity claimsIdentity, string authenticationInstant, string issuerName)
		{
			Microsoft.IdentityModel.Claims.Claim claim2 = claimsIdentity.Claims.FirstOrDefault((Microsoft.IdentityModel.Claims.Claim claim) => claim.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant");
			if (claim2 == null)
			{
				claimsIdentity.Claims.Add(new Microsoft.IdentityModel.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", authenticationInstant, "http://www.w3.org/2001/XMLSchema#dateTime", issuerName));
			}
		}

		internal static IClaimsIdentity MergeClaims(IClaimsIdentity identity1, IClaimsIdentity identity2)
		{
			if (identity1 == null && identity2 == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4268"));
			}
			if (identity1 == null)
			{
				return identity2;
			}
			if (identity2 == null)
			{
				return identity1;
			}
			WindowsClaimsIdentity windowsClaimsIdentity = identity1 as WindowsClaimsIdentity;
			if (windowsClaimsIdentity != null)
			{
				windowsClaimsIdentity.Claims.CopyRange(identity2.Claims);
				return windowsClaimsIdentity;
			}
			windowsClaimsIdentity = identity2 as WindowsClaimsIdentity;
			if (windowsClaimsIdentity != null)
			{
				windowsClaimsIdentity.Claims.CopyRange(identity1.Claims);
				return windowsClaimsIdentity;
			}
			identity1.Claims.CopyRange(identity2.Claims);
			return identity1;
		}

		protected override bool CheckAccessCore(OperationContext operationContext)
		{
			if (operationContext == null)
			{
				return false;
			}
			string text = string.Empty;
			if (!string.IsNullOrEmpty(operationContext.IncomingMessageHeaders.Action))
			{
				text = operationContext.IncomingMessageHeaders.Action;
			}
			else
			{
				HttpRequestMessageProperty httpRequestMessageProperty = operationContext.IncomingMessageProperties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
				if (httpRequestMessageProperty != null)
				{
					text = httpRequestMessageProperty.Method;
				}
			}
			Uri to = operationContext.IncomingMessageHeaders.To;
			FederatedServiceCredentials federatedServiceCredentials = GetFederatedServiceCredentials();
			if (federatedServiceCredentials == null || string.IsNullOrEmpty(text) || to == null)
			{
				return false;
			}
			operationContext.IncomingMessageProperties["ServiceConfiguration"] = federatedServiceCredentials.ServiceConfiguration;
			IClaimsPrincipal claimsPrincipal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IClaimsPrincipal;
			if (claimsPrincipal == null || claimsPrincipal.Identities == null)
			{
				return false;
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, SR.GetString("TraceAuthorize"), new AuthorizeTraceRecord(claimsPrincipal, to.AbsoluteUri, text), null);
			}
			bool flag = federatedServiceCredentials.ClaimsAuthorizationManager.CheckAccess(new Microsoft.IdentityModel.Claims.AuthorizationContext(claimsPrincipal, to.AbsoluteUri, text));
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				if (flag)
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceOnAuthorizeRequestSucceed"));
				}
				else
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceOnAuthorizeRequestFailed"));
				}
			}
			return flag;
		}
	}
}
