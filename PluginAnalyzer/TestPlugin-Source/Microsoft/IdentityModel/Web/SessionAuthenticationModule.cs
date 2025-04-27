using System;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel.Security;
using System.Threading;
using System.Web;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web.Controls;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class SessionAuthenticationModule : HttpModuleBase
	{
		private CookieHandler _cookieHandler = new ChunkedCookieHandler();

		private object _lockObject = new object();

		private bool _isSessionMode;

		public CookieHandler CookieHandler
		{
			get
			{
				return _cookieHandler;
			}
			set
			{
				_cookieHandler = value;
			}
		}

		public virtual Microsoft.IdentityModel.Tokens.SessionSecurityToken ContextSessionSecurityToken
		{
			get
			{
				return (Microsoft.IdentityModel.Tokens.SessionSecurityToken)HttpContext.Current.Items[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken).AssemblyQualifiedName];
			}
			internal set
			{
				HttpContext.Current.Items[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken).AssemblyQualifiedName] = value;
			}
		}

		public bool IsSessionMode
		{
			get
			{
				return _isSessionMode;
			}
			set
			{
				_isSessionMode = value;
			}
		}

		internal static SessionAuthenticationModule Current
		{
			get
			{
				SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
				if (sessionAuthenticationModule == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID1060"));
				}
				return sessionAuthenticationModule;
			}
		}

		public event EventHandler<SessionSecurityTokenCreatedEventArgs> SessionSecurityTokenCreated;

		public event EventHandler<SessionSecurityTokenReceivedEventArgs> SessionSecurityTokenReceived;

		public event EventHandler<SigningOutEventArgs> SigningOut;

		public event EventHandler SignedOut;

		public event EventHandler<ErrorEventArgs> SignOutError;

		private SessionSecurityTokenCreatedEventArgs RaiseSessionCreatedEvent(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken, bool reissueCookie)
		{
			SessionSecurityTokenCreatedEventArgs sessionSecurityTokenCreatedEventArgs = new SessionSecurityTokenCreatedEventArgs(sessionToken);
			sessionSecurityTokenCreatedEventArgs.WriteSessionCookie = reissueCookie;
			OnSessionSecurityTokenCreated(sessionSecurityTokenCreatedEventArgs);
			return sessionSecurityTokenCreatedEventArgs;
		}

		public virtual void AuthenticateSessionSecurityToken(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken, bool writeCookie)
		{
			SetPrincipalFromSessionToken(sessionToken);
			if (writeCookie)
			{
				WriteSessionTokenToCookie(sessionToken);
			}
		}

		public bool ContainsSessionTokenCookie(HttpCookieCollection httpCookieCollection)
		{
			if (httpCookieCollection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("httpCookieCollection");
			}
			return httpCookieCollection[CookieHandler.Name] != null;
		}

		public void DeleteSessionTokenCookie()
		{
			CookieHandler.Delete();
			if (ContextSessionSecurityToken != null)
			{
				RemoveSessionTokenFromCache(ContextSessionSecurityToken);
			}
		}

		internal void RemoveSessionTokenFromCache(Microsoft.IdentityModel.Tokens.SessionSecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler sessionSecurityTokenHandler = base.ServiceConfiguration.SecurityTokenHandlers[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)] as Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler;
			if (sessionSecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4010", typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken))));
			}
			SecurityTokenCacheKey key = new SecurityTokenCacheKey(token.EndpointId, token.ContextId, token.KeyGeneration, _isSessionMode);
			sessionSecurityTokenHandler.TokenCache.TryRemoveEntry(key);
		}

		internal Uri GetSignOutUrlFromSessionToken()
		{
			Microsoft.IdentityModel.Tokens.SessionSecurityToken contextSessionSecurityToken = ContextSessionSecurityToken;
			if (contextSessionSecurityToken != null && !string.IsNullOrEmpty(contextSessionSecurityToken.Context))
			{
				string sessionTokenContextPrefix = WSFederationAuthenticationModule.SessionTokenContextPrefix;
				if (contextSessionSecurityToken.Context.StartsWith(sessionTokenContextPrefix, StringComparison.Ordinal))
				{
					return new Uri(contextSessionSecurityToken.Context.Substring(sessionTokenContextPrefix.Length));
				}
			}
			return null;
		}

		protected override void InitializeModule(HttpApplication context)
		{
			context.AuthenticateRequest += OnAuthenticateRequest;
			context.PostAuthenticateRequest += OnPostAuthenticateRequest;
			InitializePropertiesFromConfiguration(base.ServiceConfiguration.Name);
		}

		protected virtual void InitializePropertiesFromConfiguration(string serviceName)
		{
			ServiceElement element = MicrosoftIdentityModelSection.Current.ServiceElements.GetElement(serviceName);
			if (element != null && element.FederatedAuthentication != null && element.FederatedAuthentication.CookieHandler != null && element.FederatedAuthentication.CookieHandler.IsConfigured)
			{
				_cookieHandler = element.FederatedAuthentication.CookieHandler.GetConfiguredCookieHandler();
			}
		}

		protected virtual void OnAuthenticateRequest(object sender, EventArgs eventArgs)
		{
			Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken = null;
			if (!TryReadSessionTokenFromCookie(out sessionToken))
			{
				HttpApplication httpApplication = (HttpApplication)sender;
				if (string.Equals(httpApplication.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
				{
					string absoluteUri = httpApplication.Request.Url.AbsoluteUri;
					string text = CookieHandler.MatchCookiePath(absoluteUri);
					if (!StringComparer.Ordinal.Equals(absoluteUri, text))
					{
						httpApplication.Response.Redirect(text, endResponse: false);
						httpApplication.CompleteRequest();
					}
				}
			}
			if (sessionToken == null)
			{
				return;
			}
			Microsoft.IdentityModel.Tokens.SessionSecurityToken token = sessionToken;
			SessionSecurityTokenReceivedEventArgs sessionSecurityTokenReceivedEventArgs = new SessionSecurityTokenReceivedEventArgs(sessionToken);
			OnSessionSecurityTokenReceived(sessionSecurityTokenReceivedEventArgs);
			if (sessionSecurityTokenReceivedEventArgs.Cancel)
			{
				return;
			}
			sessionToken = sessionSecurityTokenReceivedEventArgs.SessionToken;
			bool flag = sessionSecurityTokenReceivedEventArgs.ReissueCookie;
			if (flag)
			{
				RemoveSessionTokenFromCache(token);
				SessionSecurityTokenCreatedEventArgs sessionSecurityTokenCreatedEventArgs = RaiseSessionCreatedEvent(sessionToken, reissueCookie: true);
				sessionToken = sessionSecurityTokenCreatedEventArgs.SessionToken;
				flag = sessionSecurityTokenCreatedEventArgs.WriteSessionCookie;
			}
			try
			{
				AuthenticateSessionSecurityToken(sessionToken, flag);
			}
			catch (FederatedAuthenticationSessionEndingException ex)
			{
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("ID8021", ex));
				}
				SignOut();
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

		public virtual void SignOut()
		{
			try
			{
				OnSigningOut(SigningOutEventArgs.RPInitiated);
				DeleteSessionTokenCookie();
				OnSignedOut(EventArgs.Empty);
			}
			catch (Exception ex)
			{
				if (!DiagnosticUtil.IsFatal(ex))
				{
					if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
					{
						DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8022", ex));
					}
					ErrorEventArgs errorEventArgs = new ErrorEventArgs(ex);
					OnSignOutError(errorEventArgs);
					if (errorEventArgs.Cancel)
					{
						return;
					}
					throw;
				}
				throw;
			}
		}

		protected virtual void OnSessionSecurityTokenCreated(SessionSecurityTokenCreatedEventArgs args)
		{
			if (this.SessionSecurityTokenCreated != null)
			{
				this.SessionSecurityTokenCreated(this, args);
			}
		}

		protected virtual void OnSessionSecurityTokenReceived(SessionSecurityTokenReceivedEventArgs args)
		{
			if (this.SessionSecurityTokenReceived != null)
			{
				this.SessionSecurityTokenReceived(this, args);
			}
		}

		protected virtual void OnSignedOut(EventArgs e)
		{
			if (this.SignedOut != null)
			{
				this.SignedOut(this, e);
			}
		}

		protected virtual void OnSigningOut(SigningOutEventArgs e)
		{
			if (this.SigningOut != null)
			{
				this.SigningOut(this, e);
			}
		}

		protected virtual void OnSignOutError(ErrorEventArgs e)
		{
			if (this.SignOutError != null)
			{
				this.SignOutError(this, e);
			}
		}

		public Microsoft.IdentityModel.Tokens.SessionSecurityToken CreateSessionSecurityToken(IClaimsPrincipal principal, string context, DateTime validFrom, DateTime validTo, bool isPersistent)
		{
			Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler sessionSecurityTokenHandler = base.ServiceConfiguration.SecurityTokenHandlers[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)] as Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler;
			if (sessionSecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4010", typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken))));
			}
			Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken = sessionSecurityTokenHandler.CreateSessionSecurityToken(principal, context, _cookieHandler.Path, validFrom, validTo);
			sessionSecurityToken.IsPersistent = isPersistent;
			sessionSecurityToken.IsSessionMode = _isSessionMode;
			return sessionSecurityToken;
		}

		public Microsoft.IdentityModel.Tokens.SessionSecurityToken ReadSessionTokenFromCookie(byte[] sessionCookie)
		{
			Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler sessionSecurityTokenHandler = base.ServiceConfiguration.SecurityTokenHandlers[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)] as Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler;
			if (sessionSecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4010", typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken))));
			}
			SessionSecurityTokenResolver sessionSecurityTokenResolver = new SessionSecurityTokenResolver(sessionSecurityTokenHandler.TokenCache, CookieHandler.Path, _isSessionMode);
			SecurityContextKeyIdentifierClause keyId = GetKeyId(sessionCookie);
			SecurityToken token = null;
			bool flag = false;
			if (keyId != null)
			{
				if (sessionSecurityTokenResolver.TryResolveToken(keyId, out token))
				{
					return token as Microsoft.IdentityModel.Tokens.SessionSecurityToken;
				}
				flag = true;
			}
			token = ((!flag) ? sessionSecurityTokenHandler.ReadToken(sessionCookie, sessionSecurityTokenResolver) : sessionSecurityTokenHandler.ReadToken(sessionCookie, EmptySecurityTokenResolver.Instance));
			if (keyId != null)
			{
				sessionSecurityTokenHandler.TokenCache.TryAddEntry(new SecurityTokenCacheKey(CookieHandler.Path, keyId.ContextId, keyId.Generation, _isSessionMode), token);
			}
			return token as Microsoft.IdentityModel.Tokens.SessionSecurityToken;
		}

		private SecurityContextKeyIdentifierClause GetKeyId(byte[] sessionCookie)
		{
			using XmlReader reader = XmlDictionaryReader.CreateTextReader(sessionCookie, XmlDictionaryReaderQuotas.Max);
			System.Xml.UniqueId uniqueId = null;
			_ = SessionDictionary.Instance;
			XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			xmlDictionaryReader.MoveToContent();
			string ns;
			string localname;
			if (xmlDictionaryReader.IsStartElement("SecurityContextToken", "http://schemas.xmlsoap.org/ws/2005/02/sc"))
			{
				ns = "http://schemas.xmlsoap.org/ws/2005/02/sc";
				localname = "Instance";
			}
			else
			{
				if (!xmlDictionaryReader.IsStartElement("SecurityContextToken", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512"))
				{
					return null;
				}
				ns = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512";
				localname = "Instance";
			}
			xmlDictionaryReader.ReadFullStartElement();
			System.Xml.UniqueId uniqueId2 = xmlDictionaryReader.ReadElementContentAsUniqueId();
			if (uniqueId2 == null || string.IsNullOrEmpty(uniqueId2.ToString()))
			{
				return null;
			}
			if (xmlDictionaryReader.IsStartElement(localname, ns))
			{
				uniqueId = xmlDictionaryReader.ReadElementContentAsUniqueId();
			}
			if (uniqueId == null)
			{
				return new SecurityContextKeyIdentifierClause(uniqueId2);
			}
			return new SecurityContextKeyIdentifierClause(uniqueId2, uniqueId);
		}

		protected virtual void SetPrincipalFromSessionToken(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken)
		{
			ClaimsIdentityCollection identities = ValidateSessionToken(sessionSecurityToken);
			IClaimsPrincipal claimsPrincipal = ClaimsPrincipal.CreateFromIdentities(identities);
			HttpContext.Current.User = claimsPrincipal;
			Thread.CurrentPrincipal = claimsPrincipal;
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Information))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Information, TraceCode.Diagnostics, SR.GetString("TraceSetPrincipalFromSessionToken"), new ClaimsPrincipalTraceRecord(claimsPrincipal), null);
			}
			ContextSessionSecurityToken = sessionSecurityToken;
		}

		public bool TryReadSessionTokenFromCookie(out Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken)
		{
			byte[] array = CookieHandler.Read();
			if (array == null)
			{
				sessionToken = null;
				return false;
			}
			sessionToken = ReadSessionTokenFromCookie(array);
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, SR.GetString("TraceValidateToken"), new TokenTraceRecord(sessionToken), null);
			}
			return true;
		}

		protected ClaimsIdentityCollection ValidateSessionToken(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionSecurityToken)
		{
			Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler sessionSecurityTokenHandler = base.ServiceConfiguration.SecurityTokenHandlers[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)] as Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler;
			if (sessionSecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4011", typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)));
			}
			try
			{
				return sessionSecurityTokenHandler.ValidateToken(sessionSecurityToken, _cookieHandler.Path);
			}
			catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new FederatedSessionExpiredException(DateTime.UtcNow, sessionSecurityToken.ValidTo, inner));
			}
			catch (Microsoft.IdentityModel.Tokens.SecurityTokenNotYetValidException inner2)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new FederatedAuthenticationSessionEndingException(SR.GetString("ID1071", DateTime.UtcNow, sessionSecurityToken.ValidFrom), inner2));
			}
		}

		public void WriteSessionTokenToCookie(Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken)
		{
			if (sessionToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sessionToken");
			}
			Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler sessionSecurityTokenHandler = base.ServiceConfiguration.SecurityTokenHandlers[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)] as Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler;
			if (sessionSecurityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4011", typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)));
			}
			byte[] value = sessionSecurityTokenHandler.WriteToken(sessionToken);
			SecurityTokenCacheKey key = new SecurityTokenCacheKey(CookieHandler.Path, sessionToken.ContextId, sessionToken.KeyGeneration, _isSessionMode);
			sessionSecurityTokenHandler.TokenCache.TryAddEntry(key, sessionToken);
			CookieHandler.Write(value, sessionToken.IsPersistent, sessionToken.ValidTo);
		}
	}
}
