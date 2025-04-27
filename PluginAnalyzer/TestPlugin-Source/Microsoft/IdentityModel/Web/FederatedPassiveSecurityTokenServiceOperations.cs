using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public static class FederatedPassiveSecurityTokenServiceOperations
	{
		public static void ProcessRequest(HttpRequest request, IPrincipal principal, Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService sts, HttpResponse response)
		{
			ProcessRequest(request, principal, sts, response, null);
		}

		public static void ProcessRequest(HttpRequest request, IPrincipal principal, Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService sts, HttpResponse response, WSFederationSerializer federationSerializer)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			if (sts == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sts");
			}
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				string[] allKeys = request.QueryString.AllKeys;
				foreach (string text in allKeys)
				{
					dictionary.Add(text, request.QueryString[text]);
				}
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, SR.GetString("TracePassiveOperationProcessRequest"), new PassiveMessageTraceRecord(dictionary), null);
			}
			string text2 = request.QueryString["wa"];
			Uri uri = null;
			if (string.IsNullOrEmpty(text2))
			{
				return;
			}
			try
			{
				switch (text2)
				{
				case "wsignin1.0":
				{
					uri = uri ?? request.Url;
					SignInRequestMessage requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(uri);
					if (IsAuthenticatedUser(principal))
					{
						SignInResponseMessage signInResponseMessage = ProcessSignInRequest(requestMessage, principal, sts, federationSerializer);
						ProcessSignInResponse(signInResponseMessage, response);
						break;
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new UnauthorizedAccessException());
				}
				case "wsignout1.0":
				case "wsignoutcleanup1.0":
				{
					WSFederationMessage wSFederationMessage = WSFederationMessage.CreateFromUri(request.Url);
					string reply = null;
					if (text2 == "wsignout1.0")
					{
						SignOutRequestMessage signOutRequestMessage = (SignOutRequestMessage)wSFederationMessage;
						reply = signOutRequestMessage.Reply;
					}
					ProcessSignOutRequest(wSFederationMessage, principal, reply, response);
					break;
				}
				default:
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3000", text2)));
				}
			}
			catch (Exception ex)
			{
				if (DiagnosticUtil.IsFatal(ex))
				{
					throw;
				}
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8020", ex));
				}
				throw;
			}
		}

		public static SignInResponseMessage ProcessSignInRequest(SignInRequestMessage requestMessage, IPrincipal principal, Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService sts)
		{
			return ProcessSignInRequest(requestMessage, principal, sts, null);
		}

		public static SignInResponseMessage ProcessSignInRequest(SignInRequestMessage requestMessage, IPrincipal principal, Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService sts, WSFederationSerializer federationSerializer)
		{
			if (requestMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestMessage");
			}
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			if (sts == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sts");
			}
			if (string.IsNullOrEmpty(requestMessage.Realm))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5023")));
			}
			SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager = sts.SecurityTokenServiceConfiguration.SecurityTokenHandlerCollectionManager;
			WSTrustSerializationContext context = new WSTrustSerializationContext(securityTokenHandlerCollectionManager, sts.SecurityTokenServiceConfiguration.CreateAggregateTokenResolver(), sts.SecurityTokenServiceConfiguration.IssuerTokenResolver);
			if (federationSerializer == null)
			{
				federationSerializer = new WSFederationSerializer(sts.SecurityTokenServiceConfiguration.WSTrust13RequestSerializer, sts.SecurityTokenServiceConfiguration.WSTrust13ResponseSerializer);
			}
			RequestSecurityToken request = federationSerializer.CreateRequest(requestMessage, context);
			RequestSecurityTokenResponse requestSecurityTokenResponse = sts.Issue(ClaimsPrincipal.CreateFromPrincipal(principal), request);
			Uri result = null;
			if (!UriUtil.TryCreateValidUri(requestSecurityTokenResponse.ReplyTo, UriKind.Absolute, out result))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5024")));
			}
			return new SignInResponseMessage(result, requestSecurityTokenResponse, federationSerializer, context);
		}

		public static void ProcessSignInResponse(SignInResponseMessage signInResponseMessage, HttpResponse httpResponse)
		{
			if (signInResponseMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signInResponseMessage");
			}
			if (httpResponse == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("httpResponse");
			}
			signInResponseMessage.Write(httpResponse.Output);
			httpResponse.Flush();
			httpResponse.End();
		}

		public static void ProcessSignOutRequest(WSFederationMessage requestMessage, IPrincipal principal, string reply, HttpResponse httpResponse)
		{
			if (requestMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestMessage");
			}
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			if (httpResponse == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("httpResponse");
			}
			if (!(requestMessage is SignOutRequestMessage) && !(requestMessage is SignOutCleanupRequestMessage))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation("ProcessSignOutRequest can only be called with a SignOutRequestMessage or SignOutCleanupRequestMessage");
			}
			if (IsAuthenticatedUser(principal))
			{
				try
				{
					FormsAuthentication.SignOut();
				}
				finally
				{
					FederatedAuthentication.SessionAuthenticationModule?.DeleteSessionTokenCookie();
				}
			}
			if (reply != null)
			{
				httpResponse.Redirect(reply);
			}
		}

		private static bool IsAuthenticatedUser(IPrincipal principal)
		{
			if (principal != null && principal.Identity != null)
			{
				return principal.Identity.IsAuthenticated;
			}
			return false;
		}
	}
}
