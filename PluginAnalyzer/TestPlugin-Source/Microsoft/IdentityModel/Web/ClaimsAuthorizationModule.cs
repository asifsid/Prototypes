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
	public class ClaimsAuthorizationModule : IHttpModule
	{
		private ClaimsAuthorizationManager _authorizationManager;

		public ClaimsAuthorizationManager ClaimsAuthorizationManager
		{
			get
			{
				return _authorizationManager;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_authorizationManager = value;
			}
		}

		protected virtual bool Authorize()
		{
			bool result = true;
			HttpRequest request = HttpContext.Current.Request;
			IClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as IClaimsPrincipal;
			if (claimsPrincipal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelper(new NotSupportedException(SR.GetString("ID1069")), TraceEventType.Error);
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, SR.GetString("TraceAuthorize"), new AuthorizeTraceRecord(claimsPrincipal, request), null);
			}
			if (ClaimsAuthorizationManager != null)
			{
				result = ClaimsAuthorizationManager.CheckAccess(new AuthorizationContext(claimsPrincipal, request.Url.AbsoluteUri, request.HttpMethod));
			}
			return result;
		}

		protected virtual void OnAuthorizeRequest(object sender, EventArgs args)
		{
			if (!Authorize())
			{
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceOnAuthorizeRequestFailed"));
				}
				HttpResponse response = HttpContext.Current.Response;
				response.StatusCode = 401;
				HttpContext.Current.ApplicationInstance.CompleteRequest();
			}
			else if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceOnAuthorizeRequestSucceed"));
			}
		}

		public void Dispose()
		{
			(_authorizationManager as IDisposable)?.Dispose();
		}

		public void Init(HttpApplication context)
		{
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			_authorizationManager = FederatedAuthentication.ServiceConfiguration.ClaimsAuthorizationManager;
			context.AuthorizeRequest += OnAuthorizeRequest;
		}
	}
}
