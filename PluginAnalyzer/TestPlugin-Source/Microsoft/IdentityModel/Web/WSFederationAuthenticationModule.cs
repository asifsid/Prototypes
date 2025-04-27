using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web.Configuration;
using Microsoft.IdentityModel.Web.Controls;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class WSFederationAuthenticationModule : HttpModuleBase
	{
		private string _authenticationType;

		private string _freshness;

		private string _homeRealm;

		private string _issuer;

		private string _policy;

		private string _realm;

		private string _reply;

		private string _signOutReply = string.Empty;

		private string _request;

		private string _requestPtr;

		private string _resource;

		private string _signInContext;

		private string _signInQueryString = string.Empty;

		private string _signOutQueryString = string.Empty;

		private bool _passiveRedirectEnabled;

		private bool _persistentCookiesOnPassiveRedirects;

		private bool _requireHttps;

		private XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;

		private static readonly string _sessionTokenContextPrefix = "(" + typeof(WSFederationAuthenticationModule).Name + ")";

		private static readonly byte[] _signOutImage = new byte[143]
		{
			71, 73, 70, 56, 57, 97, 17, 0, 13, 0,
			162, 0, 0, 255, 255, 255, 169, 240, 169, 125,
			232, 125, 82, 224, 82, 38, 216, 38, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 33, 249, 4,
			5, 0, 0, 5, 0, 44, 0, 0, 0, 0,
			17, 0, 13, 0, 0, 8, 84, 0, 11, 8,
			28, 72, 112, 32, 128, 131, 5, 19, 22, 56,
			24, 128, 64, 0, 0, 10, 13, 54, 116, 8,
			49, 226, 193, 1, 4, 6, 32, 36, 88, 113,
			97, 0, 140, 26, 11, 30, 68, 8, 64, 0,
			129, 140, 29, 5, 2, 56, 73, 209, 36, 202,
			132, 37, 79, 14, 112, 73, 81, 97, 76, 150,
			53, 109, 210, 36, 32, 32, 37, 76, 151, 33,
			35, 26, 20, 16, 84, 168, 65, 159, 9, 3,
			2, 0, 59
		};

		internal static string SessionTokenContextPrefix => _sessionTokenContextPrefix;

		public string AuthenticationType
		{
			get
			{
				return _authenticationType;
			}
			set
			{
				_authenticationType = value;
			}
		}

		public string Freshness
		{
			get
			{
				return _freshness;
			}
			set
			{
				_freshness = value;
			}
		}

		public string HomeRealm
		{
			get
			{
				return _homeRealm;
			}
			set
			{
				_homeRealm = value;
			}
		}

		public string Issuer
		{
			get
			{
				return _issuer;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "value"));
				}
				if (!UriUtil.CanCreateValidUri(value, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0014", value), "value"));
				}
				_issuer = value;
			}
		}

		public string Realm
		{
			get
			{
				return _realm;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "value"));
				}
				if (!UriUtil.CanCreateValidUri(value, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0014", value), "value"));
				}
				_realm = value;
			}
		}

		public string Policy
		{
			get
			{
				return _policy;
			}
			set
			{
				_policy = value;
			}
		}

		public string Reply
		{
			get
			{
				return _reply;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && !UriUtil.CanCreateValidUri(value, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0014", value), "value"));
				}
				_reply = value;
			}
		}

		public string SignOutReply
		{
			get
			{
				return _signOutReply;
			}
			set
			{
				_signOutReply = value;
			}
		}

		public string Request
		{
			get
			{
				return _request;
			}
			set
			{
				_request = value;
			}
		}

		public string RequestPtr
		{
			get
			{
				return _requestPtr;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && !UriUtil.CanCreateValidUri(value, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0014", value), "value"));
				}
				_requestPtr = value;
			}
		}

		public string Resource
		{
			get
			{
				return _resource;
			}
			set
			{
				_resource = value;
			}
		}

		public bool PassiveRedirectEnabled
		{
			get
			{
				return _passiveRedirectEnabled;
			}
			set
			{
				_passiveRedirectEnabled = value;
			}
		}

		public bool PersistentCookiesOnPassiveRedirects
		{
			get
			{
				return _persistentCookiesOnPassiveRedirects;
			}
			set
			{
				_persistentCookiesOnPassiveRedirects = value;
			}
		}

		public bool RequireHttps
		{
			get
			{
				return _requireHttps;
			}
			set
			{
				_requireHttps = value;
			}
		}

		public string SignInContext
		{
			get
			{
				return _signInContext;
			}
			set
			{
				_signInContext = value;
			}
		}

		public string SignInQueryString
		{
			get
			{
				return _signInQueryString;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("SignInQueryString");
				}
				_signInQueryString = value;
			}
		}

		public string SignOutQueryString
		{
			get
			{
				return _signOutQueryString;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("SignOutQueryString");
				}
				_signOutQueryString = value;
			}
		}

		public XmlDictionaryReaderQuotas XmlDictionaryReaderQuotas
		{
			get
			{
				return _xmlDictionaryReaderQuotas;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_xmlDictionaryReaderQuotas = value;
			}
		}

		public event EventHandler<SecurityTokenReceivedEventArgs> SecurityTokenReceived;

		public event EventHandler<SecurityTokenValidatedEventArgs> SecurityTokenValidated;

		public event EventHandler<SessionSecurityTokenCreatedEventArgs> SessionSecurityTokenCreated;

		public event EventHandler SignedIn;

		public event EventHandler SignedOut;

		public event EventHandler<ErrorEventArgs> SignInError;

		public event EventHandler<SigningOutEventArgs> SigningOut;

		public event EventHandler<ErrorEventArgs> SignOutError;

		public event EventHandler<RedirectingToIdentityProviderEventArgs> RedirectingToIdentityProvider;

		public event EventHandler<AuthorizationFailedEventArgs> AuthorizationFailed;

		public WSFederationAuthenticationModule()
		{
			_xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas
			{
				MaxArrayLength = 2097152,
				MaxStringContentLength = 2097152
			};
		}

		public bool CanReadSignInResponse(HttpRequest request)
		{
			return CanReadSignInResponse(request, onPage: false);
		}

		public virtual bool CanReadSignInResponse(HttpRequest request, bool onPage)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
			if (string.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
			{
				if ((onPage || !sessionAuthenticationModule.ContainsSessionTokenCookie(request.Cookies)) && IsSignInResponse(request))
				{
					return true;
				}
			}
			else
			{
				SignOutCleanupRequestMessage signOutCleanupMessage = GetSignOutCleanupMessage(request);
				if (signOutCleanupMessage != null)
				{
					if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
					{
						DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, "CanReadSignInResponse", new WSFedMessageTraceRecord(signOutCleanupMessage), null);
					}
					SignOut(isIPRequest: true);
					HttpResponse response = HttpContext.Current.Response;
					if (!string.IsNullOrEmpty(signOutCleanupMessage.Reply))
					{
						string signOutRedirectUrl = GetSignOutRedirectUrl(signOutCleanupMessage);
						if (onPage)
						{
							response.Redirect(signOutRedirectUrl);
						}
						else
						{
							response.Redirect(signOutRedirectUrl, endResponse: false);
							HttpContext.Current.ApplicationInstance.CompleteRequest();
						}
					}
					else
					{
						response.Cache.SetCacheability(HttpCacheability.NoCache);
						response.ClearContent();
						response.ContentType = "image/gif";
						response.BinaryWrite(_signOutImage);
						if (onPage)
						{
							response.End();
						}
						else
						{
							HttpContext.Current.ApplicationInstance.CompleteRequest();
						}
					}
				}
			}
			return false;
		}

		protected virtual string GetSignOutRedirectUrl(SignOutCleanupRequestMessage signOutMessage)
		{
			if (signOutMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signOutMessage");
			}
			if (string.IsNullOrEmpty(signOutMessage.Reply))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("signOutMessage.Reply", SR.GetString("ID0022"));
			}
			Uri replyUri = new Uri(signOutMessage.Reply, UriKind.RelativeOrAbsolute);
			Uri issuerUri = new Uri(Issuer, UriKind.RelativeOrAbsolute);
			if (IsSignOutReplySafe(replyUri, issuerUri))
			{
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceResponseRedirect", signOutMessage.Reply));
				}
				return signOutMessage.Reply;
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
			{
				DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceResponseRedirectNotTrusted", signOutMessage.Reply, Issuer));
			}
			return Issuer;
		}

		private static bool IsSignOutReplySafe(Uri replyUri, Uri issuerUri)
		{
			bool result = false;
			if (replyUri.IsAbsoluteUri && replyUri.Scheme == issuerUri.Scheme && replyUri.Port == issuerUri.Port)
			{
				string dnsSafeHost = replyUri.DnsSafeHost;
				string dnsSafeHost2 = issuerUri.DnsSafeHost;
				if (StringComparer.OrdinalIgnoreCase.Equals(dnsSafeHost, dnsSafeHost2) || dnsSafeHost.EndsWith("." + dnsSafeHost2, StringComparison.OrdinalIgnoreCase))
				{
					result = true;
				}
			}
			return result;
		}

		protected virtual string GetReturnUrlFromResponse(HttpRequest request)
		{
			if (PassiveRedirectEnabled)
			{
				WSFederationMessage wSFederationMessage = WSFederationMessage.CreateFromFormPost(request);
				FederatedPassiveContext federatedPassiveContext = new FederatedPassiveContext(wSFederationMessage.Context);
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, "GetReturnUrlFromResponse", new WSFedMessageTraceRecord(wSFederationMessage), null);
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceGetReturnUrlFromResponse", federatedPassiveContext.ReturnUrl));
				}
				return federatedPassiveContext.ReturnUrl;
			}
			return string.Empty;
		}

		public virtual SecurityToken GetSecurityToken(HttpRequest request)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentNullException("request"));
			}
			SignInResponseMessage signInResponseMessage = GetSignInResponseMessage(request);
			return GetSecurityToken(signInResponseMessage);
		}

		public void VerifyProperties()
		{
			if (string.IsNullOrEmpty(Issuer))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1047")));
			}
			if (string.IsNullOrEmpty(Realm))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1048")));
			}
			if (RequireHttps)
			{
				if (!ControlUtil.IsHttps(Issuer))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1056")));
				}
				if (!string.IsNullOrEmpty(Reply) && !ControlUtil.IsHttps(Reply))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1057")));
				}
			}
		}

		public virtual void RedirectToIdentityProvider(string uniqueId, string returnUrl, bool persist)
		{
			VerifyProperties();
			HttpContext current = HttpContext.Current;
			if (current == null || current.Response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1055")));
			}
			SignInRequestMessage signInRequestMessage = CreateSignInRequest(uniqueId, returnUrl, persist);
			RedirectingToIdentityProviderEventArgs redirectingToIdentityProviderEventArgs = new RedirectingToIdentityProviderEventArgs(signInRequestMessage);
			OnRedirectingToIdentityProvider(redirectingToIdentityProviderEventArgs);
			if (!redirectingToIdentityProviderEventArgs.Cancel)
			{
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceRedirectArgsSignInRequestMessageRequestUrl", redirectingToIdentityProviderEventArgs.SignInRequestMessage.RequestUrl));
				}
				current.Response.Redirect(redirectingToIdentityProviderEventArgs.SignInRequestMessage.RequestUrl, endResponse: false);
				current.ApplicationInstance.CompleteRequest();
			}
		}

		protected override void InitializeModule(HttpApplication context)
		{
			if (FederatedAuthentication.SessionAuthenticationModule == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID1060"));
			}
			context.AuthenticateRequest += OnAuthenticateRequest;
			context.EndRequest += OnEndRequest;
			context.PostAuthenticateRequest += OnPostAuthenticateRequest;
			InitializePropertiesFromConfiguration(base.ServiceConfiguration.Name);
		}

		protected virtual void InitializePropertiesFromConfiguration(string serviceName)
		{
			ServiceElement element = MicrosoftIdentityModelSection.Current.ServiceElements.GetElement(serviceName);
			InitializePropertiesFromConfiguration(element);
		}

		internal void InitializePropertiesFromConfiguration(ServiceElement element)
		{
			if (element != null)
			{
				WSFederationAuthenticationElement wSFederation = element.FederatedAuthentication.WSFederation;
				wSFederation.Verify();
				Issuer = wSFederation.Issuer;
				Reply = wSFederation.Reply;
				RequireHttps = wSFederation.RequireHttps;
				Freshness = wSFederation.Freshness;
				AuthenticationType = wSFederation.AuthenticationType;
				HomeRealm = wSFederation.HomeRealm;
				Policy = wSFederation.Policy;
				Realm = wSFederation.Realm;
				Reply = wSFederation.Reply;
				SignOutReply = wSFederation.SignOutReply;
				Request = wSFederation.Request;
				RequestPtr = wSFederation.RequestPtr;
				Resource = wSFederation.Resource;
				SignInQueryString = wSFederation.SignInQueryString;
				SignOutQueryString = wSFederation.SignOutQueryString;
				PassiveRedirectEnabled = wSFederation.PassiveRedirectEnabled;
				PersistentCookiesOnPassiveRedirects = wSFederation.PersistentCookiesOnPassiveRedirects;
			}
		}

		protected virtual void OnAuthenticateRequest(object sender, EventArgs args)
		{
			HttpContext current = HttpContext.Current;
			HttpRequest request = current.Request;
			if (!CanReadSignInResponse(request))
			{
				return;
			}
			try
			{
				SignInWithResponseMessage(request);
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
				ErrorEventArgs errorEventArgs = new ErrorEventArgs(ex);
				OnSignInError(errorEventArgs);
				if (!errorEventArgs.Cancel)
				{
					throw;
				}
			}
		}

		private void SignInWithResponseMessage(HttpRequest request)
		{
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				string[] allKeys = request.Form.AllKeys;
				foreach (string text in allKeys)
				{
					dictionary.Add(text, request.Form[text]);
				}
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, SR.GetString("TraceSignInWithResponseMessage"), new PassiveMessageTraceRecord(dictionary), null);
			}
			HttpContext current = HttpContext.Current;
			SecurityToken securityToken = GetSecurityToken(request);
			SecurityTokenReceivedEventArgs securityTokenReceivedEventArgs = new SecurityTokenReceivedEventArgs(securityToken);
			if (this.SecurityTokenReceived != null)
			{
				this.SecurityTokenReceived(this, securityTokenReceivedEventArgs);
			}
			if (securityTokenReceivedEventArgs.Cancel)
			{
				return;
			}
			TokenReceiver tokenReceiver = new TokenReceiver(base.ServiceConfiguration);
			IClaimsPrincipal claimsPrincipal = tokenReceiver.AuthenticateToken(securityTokenReceivedEventArgs.SecurityToken, ensureBearerToken: true, HttpContext.Current.Request.RawUrl);
			if (claimsPrincipal == null)
			{
				return;
			}
			SecurityTokenValidatedEventArgs securityTokenValidatedEventArgs = new SecurityTokenValidatedEventArgs(claimsPrincipal);
			if (this.SecurityTokenValidated != null)
			{
				this.SecurityTokenValidated(this, securityTokenValidatedEventArgs);
			}
			if (securityTokenValidatedEventArgs.Cancel)
			{
				return;
			}
			SessionAuthenticationModule current2 = SessionAuthenticationModule.Current;
			tokenReceiver.ComputeSessionTokenLifeTime(securityTokenReceivedEventArgs.SecurityToken, out var validFrom, out var validTo);
			Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken = current2.CreateSessionSecurityToken(securityTokenValidatedEventArgs.ClaimsPrincipal, GetSessionTokenContext(), validFrom, validTo, PersistentCookiesOnPassiveRedirects);
			SetPrincipalAndWriteSessionToken(sessionToken, isSession: true);
			OnSignedIn(EventArgs.Empty);
			string returnUrlFromResponse = GetReturnUrlFromResponse(request);
			if (!string.IsNullOrEmpty(returnUrlFromResponse))
			{
				if (!ControlUtil.IsAppRelative(returnUrlFromResponse))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelper(new FederationException(SR.GetString("ID3206", returnUrlFromResponse)), TraceEventType.Error);
				}
				current.Response.Redirect(CookieHandler.MatchCookiePath(returnUrlFromResponse), endResponse: false);
				current.ApplicationInstance.CompleteRequest();
			}
		}

		protected virtual void OnEndRequest(object sender, EventArgs args)
		{
			if (!PassiveRedirectEnabled || FederatedAuthentication.WSFederationAuthenticationModule == null || sender == null)
			{
				return;
			}
			HttpApplication httpApplication = (HttpApplication)sender;
			if (httpApplication.Response.StatusCode != 401)
			{
				return;
			}
			string absoluteUri = httpApplication.Request.Url.AbsoluteUri;
			string text = CookieHandler.MatchCookiePath(absoluteUri);
			if (!StringComparer.Ordinal.Equals(absoluteUri, text))
			{
				httpApplication.Response.Redirect(text, endResponse: false);
				httpApplication.CompleteRequest();
				return;
			}
			AuthorizationFailedEventArgs authorizationFailedEventArgs = new AuthorizationFailedEventArgs();
			OnAuthorizationFailed(authorizationFailedEventArgs);
			if (authorizationFailedEventArgs.RedirectToIdentityProvider)
			{
				SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
				if (sessionAuthenticationModule.CookieHandler.RequireSsl && !ControlUtil.IsHttps(httpApplication.Request.Url))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID1059"));
				}
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("TraceOnEndRequestRedirect", httpApplication.Request.RawUrl));
				}
				RedirectToIdentityProvider("passive", httpApplication.Request.RawUrl, PersistentCookiesOnPassiveRedirects);
			}
		}

		protected virtual void OnPostAuthenticateRequest(object sender, EventArgs e)
		{
			IClaimsPrincipal claimsPrincipal = HttpContext.Current.User as IClaimsPrincipal;
			if (claimsPrincipal == null)
			{
				claimsPrincipal = ClaimsPrincipal.CreateFromHttpContext(HttpContext.Current);
				ClaimsAuthenticationManager claimsAuthenticationManager = base.ServiceConfiguration.ClaimsAuthenticationManager;
				if (claimsAuthenticationManager != null && claimsPrincipal != null && claimsPrincipal.Identity != null)
				{
					claimsPrincipal = claimsAuthenticationManager.Authenticate(HttpContext.Current.Request.Url.AbsoluteUri, claimsPrincipal);
				}
				HttpContext.Current.User = claimsPrincipal;
				Thread.CurrentPrincipal = claimsPrincipal;
			}
		}

		protected virtual void OnSessionSecurityTokenCreated(SessionSecurityTokenCreatedEventArgs args)
		{
			if (this.SessionSecurityTokenCreated != null)
			{
				this.SessionSecurityTokenCreated(this, args);
			}
		}

		protected virtual void OnSignedIn(EventArgs args)
		{
			if (this.SignedIn != null)
			{
				this.SignedIn(this, args);
			}
		}

		protected virtual void OnSignedOut(EventArgs args)
		{
			if (this.SignedOut != null)
			{
				this.SignedOut(this, args);
			}
		}

		protected virtual void OnSignInError(ErrorEventArgs args)
		{
			if (this.SignInError != null)
			{
				this.SignInError(this, args);
			}
		}

		protected virtual void OnSigningOut(SigningOutEventArgs args)
		{
			if (this.SigningOut != null)
			{
				this.SigningOut(this, args);
			}
		}

		protected virtual void OnSignOutError(ErrorEventArgs args)
		{
			if (this.SignOutError != null)
			{
				this.SignOutError(this, args);
			}
		}

		protected virtual void OnRedirectingToIdentityProvider(RedirectingToIdentityProviderEventArgs e)
		{
			if (this.RedirectingToIdentityProvider != null)
			{
				this.RedirectingToIdentityProvider(this, e);
			}
		}

		protected virtual void OnAuthorizationFailed(AuthorizationFailedEventArgs e)
		{
			e.RedirectToIdentityProvider = !Thread.CurrentPrincipal.Identity.IsAuthenticated;
			if (this.AuthorizationFailed != null)
			{
				this.AuthorizationFailed(this, e);
			}
		}

		public void SetPrincipalAndWriteSessionToken(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken, bool isSession)
		{
			if (sessionToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sessionToken");
			}
			SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
			SessionSecurityTokenCreatedEventArgs sessionSecurityTokenCreatedEventArgs = new SessionSecurityTokenCreatedEventArgs(sessionToken);
			sessionSecurityTokenCreatedEventArgs.WriteSessionCookie = isSession;
			OnSessionSecurityTokenCreated(sessionSecurityTokenCreatedEventArgs);
			sessionAuthenticationModule.AuthenticateSessionSecurityToken(sessionSecurityTokenCreatedEventArgs.SessionToken, sessionSecurityTokenCreatedEventArgs.WriteSessionCookie);
		}

		public virtual void SignOut(bool isIPRequest)
		{
			try
			{
				SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
				if (sessionAuthenticationModule != null)
				{
					OnSigningOut(new SigningOutEventArgs(isIPRequest));
					sessionAuthenticationModule.DeleteSessionTokenCookie();
					OnSignedOut(EventArgs.Empty);
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1060")));
			}
			catch (Exception ex)
			{
				if (DiagnosticUtil.IsFatal(ex))
				{
					throw;
				}
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8022", ex));
				}
				ErrorEventArgs errorEventArgs = new ErrorEventArgs(ex);
				OnSignOutError(errorEventArgs);
				if (!errorEventArgs.Cancel)
				{
					throw;
				}
			}
		}

		public SignInRequestMessage CreateSignInRequest(string uniqueId, string returnUrl, bool rememberMeSet)
		{
			if (string.IsNullOrEmpty(Issuer))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5002")));
			}
			if (string.IsNullOrEmpty(Realm))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5001")));
			}
			if (!string.IsNullOrEmpty(Freshness))
			{
				double result = -1.0;
				if (!double.TryParse(Freshness, out result) || !(result >= 0.0))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5020")));
				}
			}
			SignInRequestMessage signInRequestMessage = new SignInRequestMessage(new Uri(Issuer), Realm);
			if (!string.IsNullOrEmpty(Freshness))
			{
				signInRequestMessage.Freshness = Freshness;
			}
			FederatedPassiveContext federatedPassiveContext = new FederatedPassiveContext(uniqueId, SignInContext, returnUrl, rememberMeSet);
			signInRequestMessage.Context = federatedPassiveContext.WCtx;
			signInRequestMessage.CurrentTime = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture) + "Z";
			if (!string.IsNullOrEmpty(AuthenticationType))
			{
				signInRequestMessage.AuthenticationType = AuthenticationType;
			}
			if (!string.IsNullOrEmpty(HomeRealm))
			{
				signInRequestMessage.HomeRealm = HomeRealm;
			}
			if (!string.IsNullOrEmpty(Policy))
			{
				signInRequestMessage.Policy = Policy;
			}
			if (!string.IsNullOrEmpty(Reply))
			{
				signInRequestMessage.Reply = Reply;
			}
			if (!string.IsNullOrEmpty(Resource))
			{
				signInRequestMessage.Resource = Resource;
			}
			if (!string.IsNullOrEmpty(Request))
			{
				signInRequestMessage.Request = Request;
			}
			if (!string.IsNullOrEmpty(RequestPtr))
			{
				signInRequestMessage.RequestPtr = RequestPtr;
			}
			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(SignInQueryString);
			foreach (string key in nameValueCollection.Keys)
			{
				if (!signInRequestMessage.Parameters.ContainsKey(key))
				{
					signInRequestMessage.Parameters.Add(key, nameValueCollection[key]);
				}
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, "CreateSignInRequest", new WSFedMessageTraceRecord(signInRequestMessage), null);
			}
			return signInRequestMessage;
		}

		protected virtual string GetReferencedResult(string resultPtr)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3210", "wresultptr")));
		}

		private string GetResultXml(SignInResponseMessage message)
		{
			if (!string.IsNullOrEmpty(message.Result))
			{
				return message.Result;
			}
			return GetReferencedResult(message.ResultPtr);
		}

		public virtual SecurityToken GetSecurityToken(SignInResponseMessage message)
		{
			if (message == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentNullException("message"));
			}
			string xmlTokenFromMessage = GetXmlTokenFromMessage(message, null);
			TokenReceiver tokenReceiver = new TokenReceiver(base.ServiceConfiguration);
			return tokenReceiver.ReadToken(xmlTokenFromMessage, XmlDictionaryReaderQuotas);
		}

		public virtual SignInResponseMessage GetSignInResponseMessage(HttpRequest request)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentNullException("request"));
			}
			SignInResponseMessage signInResponseMessage = WSFederationMessage.CreateFromFormPost(request) as SignInResponseMessage;
			if (signInResponseMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID1052"));
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, "GetSignInResponseMessage", new WSFedMessageTraceRecord(signInResponseMessage), null);
			}
			return signInResponseMessage;
		}

		private WSFederationSerializer CreateSerializerForResultXml(string resultXml)
		{
			if (string.IsNullOrEmpty(resultXml))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("resultXml");
			}
			using XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(resultXml), _xmlDictionaryReaderQuotas);
			return new WSFederationSerializer(reader);
		}

		public virtual string GetXmlTokenFromMessage(SignInResponseMessage message)
		{
			if (message == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("message");
			}
			string resultXml = GetResultXml(message);
			if (string.IsNullOrEmpty(resultXml))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3001"));
			}
			return GetXmlTokenFromMessage(message, CreateSerializerForResultXml(resultXml));
		}

		public virtual string GetXmlTokenFromMessage(SignInResponseMessage message, WSFederationSerializer federationSerializer)
		{
			if (federationSerializer == null)
			{
				return GetXmlTokenFromMessage(message);
			}
			if (message == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("message");
			}
			WSTrustSerializationContext context = new WSTrustSerializationContext(base.ServiceConfiguration.SecurityTokenHandlerCollectionManager);
			RequestSecurityTokenResponse requestSecurityTokenResponse = federationSerializer.CreateResponse(message, context);
			return requestSecurityTokenResponse.RequestedSecurityToken.SecurityTokenXml.OuterXml;
		}

		public virtual bool IsSignInResponse(HttpRequest request)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			if (request.Form != null && StringComparer.Ordinal.Equals(request.Form["wa"], "wsignin1.0"))
			{
				if (string.IsNullOrEmpty(request.Form["wresult"]))
				{
					return !string.IsNullOrEmpty(request.Form["wresultptr"]);
				}
				return true;
			}
			return false;
		}

		internal static SignOutCleanupRequestMessage GetSignOutCleanupMessage(HttpRequest request)
		{
			SignOutCleanupRequestMessage result = null;
			if (WSFederationMessage.TryCreateFromUri(request.Url, out var fedMsg))
			{
				result = fedMsg as SignOutCleanupRequestMessage;
			}
			return result;
		}

		protected virtual string GetSessionTokenContext()
		{
			string issuer = Issuer;
			if (string.IsNullOrEmpty(issuer))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1058")));
			}
			return SessionTokenContextPrefix + GetFederationPassiveSignOutUrl(issuer, SignOutReply, SignOutQueryString);
		}

		public static void FederatedSignOut(Uri signOutUrl, Uri replyUrl)
		{
			Uri federationPassiveSignOutUrl = GetFederationPassiveSignOutUrl(signOutUrl, replyUrl);
			if (federationPassiveSignOutUrl == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3272"));
			}
			FederatedAuthentication.SessionAuthenticationModule?.DeleteSessionTokenCookie();
			HttpContext.Current.Response.Redirect(federationPassiveSignOutUrl.AbsoluteUri);
		}

		internal static Uri GetFederationPassiveSignOutUrl(Uri signOutUrl, Uri replyUrl)
		{
			if (signOutUrl != null && !signOutUrl.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("signOutUrl", SR.GetString("ID0014", signOutUrl.ToString()));
			}
			Uri uri = null;
			if (signOutUrl == null)
			{
				SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
				if (sessionAuthenticationModule != null)
				{
					uri = sessionAuthenticationModule.GetSignOutUrlFromSessionToken();
				}
			}
			else
			{
				uri = new Uri(signOutUrl, string.Empty);
			}
			if (replyUrl != null)
			{
				if (!replyUrl.IsAbsoluteUri)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("replyUrl", SR.GetString("ID0014", replyUrl.ToString()));
				}
				NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uri.Query);
				nameValueCollection["wreply"] = replyUrl.AbsoluteUri;
				Uri baseUrl = new Uri(uri, new Uri(uri.AbsolutePath, UriKind.Relative));
				SignOutRequestMessage signOutRequestMessage = new SignOutRequestMessage(baseUrl);
				foreach (string key in nameValueCollection.Keys)
				{
					if (!signOutRequestMessage.Parameters.ContainsKey(key))
					{
						signOutRequestMessage.Parameters.Add(key, nameValueCollection[key]);
					}
				}
				uri = new Uri(ControlUtil.GetPathAndQuery(signOutRequestMessage));
			}
			return uri;
		}

		public static string GetFederationPassiveSignOutUrl(string issuer, string signOutReply, string signOutQueryString)
		{
			if (string.IsNullOrEmpty(issuer))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("issuer");
			}
			SignOutRequestMessage signOutRequestMessage = new SignOutRequestMessage(new Uri(issuer));
			if (!string.IsNullOrEmpty(signOutReply))
			{
				signOutRequestMessage.Reply = signOutReply;
			}
			if (!string.IsNullOrEmpty(signOutQueryString))
			{
				NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(signOutQueryString);
				foreach (string key in nameValueCollection.Keys)
				{
					signOutRequestMessage.Parameters.Add(key, nameValueCollection[key]);
				}
			}
			return ControlUtil.GetPathAndQuery(signOutRequestMessage);
		}
	}
}
