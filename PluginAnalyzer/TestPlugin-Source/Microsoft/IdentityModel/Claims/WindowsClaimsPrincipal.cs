using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Microsoft.IdentityModel.Claims
{
	[Serializable]
	[ComVisible(true)]
	public class WindowsClaimsPrincipal : WindowsPrincipal, IClaimsPrincipal, IPrincipal, IDisposable
	{
		private ClaimsIdentityCollection _identities = new ClaimsIdentityCollection();

		private bool _disposed;

		public new ClaimsIdentityCollection Identities => _identities;

		public override IIdentity Identity
		{
			get
			{
				if (_identities.Count > 0)
				{
					foreach (IClaimsIdentity identity in _identities)
					{
						if (identity is WindowsClaimsIdentity)
						{
							return identity;
						}
					}
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4056")));
			}
		}

		public WindowsClaimsPrincipal(WindowsClaimsIdentity identity)
			: base(identity)
		{
			if (identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identity");
			}
			_identities.Add((WindowsClaimsIdentity)identity.Copy());
		}

		public WindowsClaimsPrincipal(WindowsIdentity identity)
			: base(new WindowsClaimsIdentity(identity))
		{
			_identities.Add((WindowsClaimsIdentity)base.Identity);
		}

		public WindowsClaimsPrincipal(WindowsIdentity identity, string issuerName)
			: base(new WindowsClaimsIdentity(identity, identity.AuthenticationType, issuerName))
		{
			_identities.Add((WindowsClaimsIdentity)base.Identity);
		}

		public static IClaimsPrincipal CreateFromWindowsIdentity(WindowsIdentity identity)
		{
			return CreateFromWindowsIdentity(identity, "LOCAL AUTHORITY");
		}

		public static IClaimsPrincipal CreateFromWindowsIdentity(WindowsIdentity identity, string issuerName)
		{
			if (identity == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("identity");
			}
			if (identity.Token == IntPtr.Zero)
			{
				return ClaimsPrincipal.AnonymousPrincipal;
			}
			return new WindowsClaimsPrincipal(identity, issuerName);
		}

		public IClaimsPrincipal Copy()
		{
			WindowsClaimsPrincipal windowsClaimsPrincipal = new WindowsClaimsPrincipal((WindowsClaimsIdentity)base.Identity);
			windowsClaimsPrincipal._identities = Identities.Copy();
			return windowsClaimsPrincipal;
		}

		public override bool IsInRole(string role)
		{
			if (role == null || role.Length == 0)
			{
				return false;
			}
			NTAccount nTAccount = new NTAccount(role);
			try
			{
				SecurityIdentifier securityIdentifier = nTAccount.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
				if (securityIdentifier != null)
				{
					return IsInRole(securityIdentifier);
				}
			}
			catch (IdentityNotMappedException ex)
			{
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8002", role, ex.Message));
				}
			}
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

		public override bool IsInRole(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sid");
			}
			foreach (IClaimsIdentity identity in _identities)
			{
				if (identity.Claims == null || identity.RoleClaimType == null)
				{
					continue;
				}
				foreach (Claim claim in identity.Claims)
				{
					if ((StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid") || StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid") || StringComparer.Ordinal.Equals(claim.ClaimType, "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid")) && StringComparer.Ordinal.Equals(claim.Value, sid.Value))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				foreach (IClaimsIdentity identity in _identities)
				{
					(identity as WindowsClaimsIdentity)?.Dispose();
				}
				_identities.Clear();
			}
			_disposed = true;
		}
	}
}
