using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Claims
{
	[Serializable]
	[ComVisible(true)]
	public class ClaimsPrincipal : IClaimsPrincipal, IPrincipal
	{
		private ClaimsIdentityCollection _identities = new ClaimsIdentityCollection();

		public static IClaimsPrincipal AnonymousPrincipal => new ClaimsPrincipal(new ClaimsIdentityCollection(new IClaimsIdentity[1] { ClaimsIdentity.AnonymousIdentity }));

		public ClaimsIdentityCollection Identities => _identities;

		public virtual IIdentity Identity => SelectPrimaryIdentity(_identities);

		public static IClaimsPrincipal CreateFromPrincipal(IPrincipal principal)
		{
			return CreateFromPrincipal(principal, "LOCAL AUTHORITY");
		}

		public static IClaimsPrincipal CreateFromPrincipal(IPrincipal principal, string windowsIssuerName)
		{
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			IClaimsPrincipal claimsPrincipal = principal as IClaimsPrincipal;
			if (claimsPrincipal != null)
			{
				return claimsPrincipal;
			}
			WindowsClaimsIdentity windowsClaimsIdentity = principal.Identity as WindowsClaimsIdentity;
			if (windowsClaimsIdentity != null)
			{
				return new WindowsClaimsPrincipal(windowsClaimsIdentity);
			}
			IClaimsIdentity claimsIdentity = principal.Identity as IClaimsIdentity;
			if (claimsIdentity != null)
			{
				ClaimsPrincipal claimsPrincipal2 = new ClaimsPrincipal();
				claimsPrincipal2.Identities.Add(claimsIdentity);
				return claimsPrincipal2;
			}
			WindowsPrincipal windowsPrincipal = principal as WindowsPrincipal;
			if (windowsPrincipal != null)
			{
				return WindowsClaimsPrincipal.CreateFromWindowsIdentity((WindowsIdentity)windowsPrincipal.Identity, windowsIssuerName);
			}
			RolePrincipal rolePrincipal = principal as RolePrincipal;
			if (rolePrincipal != null)
			{
				ClaimsPrincipal claimsPrincipal3 = new ClaimsPrincipal(new IClaimsIdentity[1] { ClaimsIdentity.CreateFromIdentity(rolePrincipal.Identity) });
				string[] roles = rolePrincipal.GetRoles();
				foreach (string value in roles)
				{
					claimsPrincipal3.Identities[0].Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", value));
				}
				return claimsPrincipal3;
			}
			return new ClaimsPrincipal(principal);
		}

		public static IClaimsPrincipal CreateFromIdentities(ClaimsIdentityCollection identities)
		{
			return CreateFromIdentities(identities, "LOCAL AUTHORITY");
		}

		public static IClaimsPrincipal CreateFromIdentities(ClaimsIdentityCollection identities, string windowsIssuerName)
		{
			IClaimsIdentity claimsIdentity = SelectPrimaryIdentity(identities);
			if (claimsIdentity == null)
			{
				return AnonymousPrincipal;
			}
			IClaimsPrincipal claimsPrincipal = CreateFromIdentity(claimsIdentity, windowsIssuerName);
			foreach (IClaimsIdentity identity in identities)
			{
				if (identity != claimsIdentity)
				{
					claimsPrincipal.Identities.Add(identity);
				}
			}
			return claimsPrincipal;
		}

		public static IClaimsPrincipal CreateFromIdentity(IIdentity identity)
		{
			return CreateFromIdentity(identity, "LOCAL AUTHORITY");
		}

		public static IClaimsPrincipal CreateFromIdentity(IIdentity identity, string windowsIssuerName)
		{
			if (identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identity");
			}
			WindowsClaimsIdentity windowsClaimsIdentity = identity as WindowsClaimsIdentity;
			if (windowsClaimsIdentity != null)
			{
				return new WindowsClaimsPrincipal(windowsClaimsIdentity);
			}
			WindowsIdentity windowsIdentity = identity as WindowsIdentity;
			if (windowsIdentity != null)
			{
				return WindowsClaimsPrincipal.CreateFromWindowsIdentity(windowsIdentity, windowsIssuerName);
			}
			IClaimsIdentity claimsIdentity = identity as IClaimsIdentity;
			if (claimsIdentity != null)
			{
				return new ClaimsPrincipal(new ClaimsIdentityCollection(new IClaimsIdentity[1] { claimsIdentity }));
			}
			return new ClaimsPrincipal(new ClaimsIdentityCollection(new IClaimsIdentity[1]
			{
				new ClaimsIdentity(identity)
			}));
		}

		public static IClaimsPrincipal CreateFromHttpContext(HttpContext httpContext)
		{
			return CreateFromHttpContext(httpContext, clientCertificateAuthenticationEnabled: false);
		}

		public static IClaimsPrincipal CreateFromHttpContext(HttpContext httpContext, bool clientCertificateAuthenticationEnabled)
		{
			if (httpContext == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("httpContext");
			}
			ServiceConfiguration current = ServiceConfiguration.GetCurrent();
			IClaimsPrincipal claimsPrincipal = ((httpContext.User == null) ? AnonymousPrincipal : CreateFromPrincipal(httpContext.User, current.IssuerNameRegistry.GetWindowsIssuerName()));
			if (clientCertificateAuthenticationEnabled)
			{
				HttpClientCertificate clientCertificate = httpContext.Request.ClientCertificate;
				if (clientCertificate != null && clientCertificate.IsPresent && clientCertificate.IsValid)
				{
					X509Certificate2 certificate = new X509Certificate2(clientCertificate.Certificate);
					string certificateIssuerName = X509Util.GetCertificateIssuerName(certificate, current.IssuerNameRegistry);
					if (string.IsNullOrEmpty(certificateIssuerName))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4174")));
					}
					ClaimsIdentity claimsIdentity = new ClaimsIdentity(certificate, certificateIssuerName);
					if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
					{
						claimsPrincipal = CreateFromIdentity(claimsIdentity);
					}
					else
					{
						claimsPrincipal.Identities.Add(claimsIdentity);
					}
				}
			}
			return claimsPrincipal;
		}

		public static IClaimsIdentity SelectPrimaryIdentity(ClaimsIdentityCollection identities)
		{
			if (identities == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identities");
			}
			if (identities.Count == 0)
			{
				return null;
			}
			IClaimsIdentity claimsIdentity = null;
			foreach (IClaimsIdentity identity in identities)
			{
				if (identity is WindowsIdentity)
				{
					claimsIdentity = identity;
					break;
				}
				if (!(identity is RsaClaimsIdentity) && claimsIdentity == null)
				{
					claimsIdentity = identity;
				}
			}
			if (claimsIdentity == null)
			{
				claimsIdentity = identities[0];
			}
			return claimsIdentity;
		}

		public ClaimsPrincipal()
		{
		}

		public ClaimsPrincipal(IPrincipal principal)
			: this()
		{
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			if (principal.Identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			IClaimsPrincipal claimsPrincipal = principal as IClaimsPrincipal;
			if (claimsPrincipal == null)
			{
				_identities.Add(ClaimsIdentity.CreateFromIdentity(principal.Identity));
				return;
			}
			if (claimsPrincipal.Identities != null)
			{
				_identities.AddRange(claimsPrincipal.Identities);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("principal", SR.GetString("ID0003", "principal.identities"));
		}

		public ClaimsPrincipal(IEnumerable<IClaimsIdentity> identities)
		{
			if (identities == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identities");
			}
			_identities.AddRange(identities);
		}

		public ClaimsPrincipal(ClaimsIdentityCollection identityCollection)
		{
			if (identityCollection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identityCollection");
			}
			_identities.AddRange(identityCollection);
		}

		public IClaimsPrincipal Copy()
		{
			return new ClaimsPrincipal(Identities.Copy());
		}

		public bool IsInRole(string role)
		{
			foreach (IClaimsIdentity identity in _identities)
			{
				if (identity.Claims == null || identity.RoleClaimType == null)
				{
					continue;
				}
				foreach (Claim claim in identity.Claims)
				{
					if (StringComparer.Ordinal.Equals(claim.ClaimType, identity.RoleClaimType) && StringComparer.Ordinal.Equals(claim.Value, role))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
