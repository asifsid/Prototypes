using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class ClaimsPrincipalHttpModule : IHttpModule
	{
		private bool _clientCertificateAuthenticationEnabled;

		private ClaimsAuthenticationManager _authenticationManager;

		public bool ClientCertificateAuthenticationEnabled
		{
			get
			{
				return _clientCertificateAuthenticationEnabled;
			}
			set
			{
				_clientCertificateAuthenticationEnabled = value;
			}
		}

		public ClaimsAuthenticationManager AuthenticationManager
		{
			get
			{
				return _authenticationManager;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_authenticationManager = value;
			}
		}

		protected virtual void OnPostAuthenticateRequest(object sender, EventArgs e)
		{
			IClaimsPrincipal claimsPrincipal = HttpContext.Current.User as IClaimsPrincipal;
			if (claimsPrincipal == null)
			{
				claimsPrincipal = ClaimsPrincipal.CreateFromHttpContext(HttpContext.Current, _clientCertificateAuthenticationEnabled);
				ClaimsAuthenticationManager authenticationManager = _authenticationManager;
				if (authenticationManager != null && claimsPrincipal != null && claimsPrincipal.Identity != null)
				{
					claimsPrincipal = authenticationManager.Authenticate(HttpContext.Current.Request.Url.AbsoluteUri, claimsPrincipal);
				}
				HttpContext.Current.User = claimsPrincipal;
				Thread.CurrentPrincipal = claimsPrincipal;
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, "OnPostAuthenticateRequest", new ClaimsPrincipalTraceRecord(claimsPrincipal), null);
				}
			}
		}

		public void Dispose()
		{
			(_authenticationManager as IDisposable)?.Dispose();
		}

		public void Init(HttpApplication context)
		{
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			_authenticationManager = FederatedAuthentication.ServiceConfiguration.ClaimsAuthenticationManager;
			context.PostAuthenticateRequest += OnPostAuthenticateRequest;
		}
	}
}
