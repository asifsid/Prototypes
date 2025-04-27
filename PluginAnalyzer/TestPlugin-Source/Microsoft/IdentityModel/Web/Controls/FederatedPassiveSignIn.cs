using System;
using System.ComponentModel;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web.Configuration;

namespace Microsoft.IdentityModel.Web.Controls
{
	[Bindable(false)]
	[DefaultEvent("SignedIn")]
	[Designer(typeof(FederatedPassiveSignInDesigner))]
	[ComVisible(true)]
	public sealed class FederatedPassiveSignIn : SignInControl
	{
		private string _responseReturnUrl;

		private WSFederationAuthenticationModule _activeModule;

		private XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;

		private bool _initialized;

		[WebCategory("Category_FederatedPassive")]
		[UrlProperty]
		[DefaultValue("")]
		[WebDescription("FederatedPassiveSignIn_HomeRealm")]
		public string HomeRealm
		{
			get
			{
				return ((string)ViewState["HomeRealm"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("HomeRealm");
				}
				ViewState["HomeRealm"] = value;
			}
		}

		[WebDescription("FederatedPassiveSignIn_Issuer")]
		[DefaultValue("")]
		[WebCategory("Category_FederatedPassive")]
		[UrlProperty]
		public string Issuer
		{
			get
			{
				return ((string)ViewState["Issuer"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("Issuer");
				}
				ViewState["Issuer"] = value;
			}
		}

		[DefaultValue("")]
		[WebDescription("FederatedPassiveSignIn_Realm")]
		[WebCategory("Category_FederatedPassive")]
		public string Realm
		{
			get
			{
				return ((string)ViewState["Realm"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("Realm");
				}
				ViewState["Realm"] = value;
			}
		}

		[WebDescription("FederatedPassiveSignIn_Reply")]
		[DefaultValue("")]
		[WebCategory("Category_FederatedPassive")]
		public string Reply
		{
			get
			{
				return ((string)ViewState["Reply"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("Reply");
				}
				ViewState["Reply"] = value;
			}
		}

		[WebCategory("Category_FederatedPassive")]
		[DefaultValue("")]
		[WebDescription("FederatedPassiveSignIn_Resource")]
		public string Resource
		{
			get
			{
				return ((string)ViewState["Resource"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("Resource");
				}
				ViewState["Resource"] = value;
			}
		}

		[DefaultValue("")]
		[WebCategory("Category_FederatedPassive")]
		[WebDescription("FederatedPassiveSignIn_Request")]
		public string Request
		{
			get
			{
				return ((string)ViewState["Request"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("Request");
				}
				ViewState["Request"] = value;
			}
		}

		[WebDescription("FederatedPassiveSignIn_RequestPtr")]
		[DefaultValue("")]
		[WebCategory("Category_FederatedPassive")]
		public string RequestPtr
		{
			get
			{
				return ((string)ViewState["RequestPtr"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("RequestPtr");
				}
				ViewState["RequestPtr"] = value;
			}
		}

		[WebDescription("FederatedPassiveSignIn_Freshness")]
		[DefaultValue("")]
		[WebCategory("Category_FederatedPassive")]
		public string Freshness
		{
			get
			{
				return ((string)ViewState["Freshness"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("Freshness");
				}
				ViewState["Freshness"] = value;
			}
		}

		[DefaultValue("")]
		[WebDescription("FederatedPassiveSignIn_AuthenticationType")]
		[WebCategory("Category_FederatedPassive")]
		public string AuthenticationType
		{
			get
			{
				return ((string)ViewState["AuthenticationType"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("AuthenticationType");
				}
				ViewState["AuthenticationType"] = value;
			}
		}

		[WebDescription("FederatedPassiveSignIn_Policy")]
		[DefaultValue("")]
		[WebCategory("Category_FederatedPassive")]
		public string Policy
		{
			get
			{
				return ((string)ViewState["Policy"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("Policy");
				}
				ViewState["Policy"] = value;
			}
		}

		[DefaultValue("")]
		[WebDescription("FederatedPassiveSignIn_SignInQueryString")]
		[WebCategory("Category_FederatedPassive")]
		public string SignInQueryString
		{
			get
			{
				return ((string)ViewState["SignInQueryString"]) ?? string.Empty;
			}
			set
			{
				if (base.DesignMode && UseFederationPropertiesFromConfiguration && _initialized)
				{
					ShowFamConfigMessage("SignInQueryString");
				}
				ViewState["SignInQueryString"] = value;
			}
		}

		[DefaultValue(false)]
		[WebCategory("Category_FederatedPassive")]
		[WebDescription("FederatedPassiveSignIn_UseFederationPropertiesFromConfiguration")]
		[Themeable(false)]
		public bool UseFederationPropertiesFromConfiguration
		{
			get
			{
				return (bool)(ViewState["UseFederationPropertiesFromConfiguration"] ?? ((object)false));
			}
			set
			{
				if (base.DesignMode && _initialized && value)
				{
					MessageBoxOptions messageBoxOptions = (MessageBoxOptions)0;
					if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
					{
						messageBoxOptions |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
					}
					MessageBox.Show(SR.GetString("FederatedPassiveSignIn_UseFederationPropertiesSet"), SR.GetString("FederatedPassiveSignIn_Property_Information"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, messageBoxOptions);
				}
				ViewState["UseFederationPropertiesFromConfiguration"] = value;
			}
		}

		public FederatedPassiveSignIn()
		{
			_xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas
			{
				MaxArrayLength = 2097152,
				MaxStringContentLength = 2097152
			};
			base.Init += FederatedPassiveSignIn_Init;
		}

		protected override string GetSessionTokenContext()
		{
			string parameterValue = GetParameterValue("issuer");
			if (string.IsNullOrEmpty(parameterValue))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5002")));
			}
			return WSFederationAuthenticationModule.SessionTokenContextPrefix + WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(parameterValue, string.Empty, string.Empty);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (Enabled)
			{
				base.Click += OnClick;
				CreateWSFederationAuthenticationModule();
			}
		}

		private void RedirectingToIdentityProviderHandler(object sender, RedirectingToIdentityProviderEventArgs e)
		{
			OnRedirectingToIdentityProvider(e);
		}

		internal void CreateWSFederationAuthenticationModule()
		{
			_activeModule = new WSFederationAuthenticationModule();
			_activeModule.ServiceConfiguration = base.ServiceConfiguration;
			_activeModule.RedirectingToIdentityProvider += RedirectingToIdentityProviderHandler;
			string parameterValue = GetParameterValue("homeRealm");
			if (!string.IsNullOrEmpty(parameterValue))
			{
				_activeModule.HomeRealm = parameterValue;
			}
			string parameterValue2 = GetParameterValue("issuer");
			if (!string.IsNullOrEmpty(parameterValue2))
			{
				_activeModule.Issuer = parameterValue2;
			}
			string parameterValue3 = GetParameterValue("realm");
			if (!string.IsNullOrEmpty(parameterValue3))
			{
				_activeModule.Realm = parameterValue3;
			}
			string parameterValue4 = GetParameterValue("authenticationType");
			if (!string.IsNullOrEmpty(parameterValue4))
			{
				_activeModule.AuthenticationType = parameterValue4;
			}
			string parameterValue5 = GetParameterValue("freshness");
			if (!string.IsNullOrEmpty(parameterValue5))
			{
				_activeModule.Freshness = parameterValue5;
			}
			string parameterValue6 = GetParameterValue("policy");
			if (!string.IsNullOrEmpty(parameterValue6))
			{
				_activeModule.Policy = parameterValue6;
			}
			string parameterValue7 = GetParameterValue("reply");
			if (!string.IsNullOrEmpty(parameterValue7))
			{
				_activeModule.Reply = parameterValue7;
			}
			string parameterValue8 = GetParameterValue("request");
			if (!string.IsNullOrEmpty(parameterValue8))
			{
				_activeModule.Request = parameterValue8;
			}
			string parameterValue9 = GetParameterValue("requestPtr");
			if (!string.IsNullOrEmpty(parameterValue9))
			{
				_activeModule.RequestPtr = parameterValue9;
			}
			string parameterValue10 = GetParameterValue("resource");
			if (!string.IsNullOrEmpty(parameterValue10))
			{
				_activeModule.Resource = parameterValue10;
			}
			if (!string.IsNullOrEmpty(base.SignInContext))
			{
				_activeModule.SignInContext = base.SignInContext;
			}
			string parameterValue11 = GetParameterValue("signInQueryString");
			if (!string.IsNullOrEmpty(parameterValue11))
			{
				_activeModule.SignInQueryString = parameterValue11;
			}
			WSFederationAuthenticationElement fAMConfiguration = GetFAMConfiguration();
			if (UseFederationPropertiesFromConfiguration && fAMConfiguration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5003")));
			}
			_activeModule.RequireHttps = (UseFederationPropertiesFromConfiguration ? fAMConfiguration.RequireHttps : base.RequireHttps);
			_activeModule.VerifyProperties();
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (Enabled && StringComparer.OrdinalIgnoreCase.Equals(GetParameterValue("requireHttps"), "true"))
			{
				string parameterValue = GetParameterValue("issuer");
				string parameterValue2 = GetParameterValue("reply");
				if (!ControlUtil.IsHttps(parameterValue))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID1056"));
				}
				if (!string.IsNullOrEmpty(parameterValue2) && !ControlUtil.IsHttps(parameterValue2))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID1057"));
				}
				if (!ControlUtil.IsHttps(Context.Request.Url))
				{
					Enabled = false;
					ToolTip = SR.GetString("ID5016");
				}
			}
			if (Enabled && AutoSignIn && !HttpContext.Current.User.Identity.IsAuthenticated)
			{
				RedirectToIdentityProvider();
			}
		}

		protected override bool SignIn()
		{
			HttpRequest request = Page.Request;
			if (_activeModule != null && _activeModule.CanReadSignInResponse(request, onPage: true))
			{
				FederatedPassiveContext federatedPassiveContext = new FederatedPassiveContext(request.Form["wctx"]);
				if (StringComparer.Ordinal.Equals(UniqueID, federatedPassiveContext.ControlId))
				{
					SignInResponseMessage signInResponseMessage = _activeModule.GetSignInResponseMessage(request);
					return SignInWithResponseMessage(signInResponseMessage);
				}
			}
			return false;
		}

		internal bool SignInWithResponseMessage(SignInResponseMessage message)
		{
			string xmlTokenFromMessage = _activeModule.GetXmlTokenFromMessage(message, null);
			ServiceConfiguration serviceConfiguration = _activeModule.ServiceConfiguration;
			TokenReceiver tokenReceiver = new TokenReceiver(serviceConfiguration);
			SecurityToken securityToken = tokenReceiver.ReadToken(xmlTokenFromMessage, _xmlDictionaryReaderQuotas);
			FederatedPassiveContext federatedPassiveContext = new FederatedPassiveContext(message.Context);
			SecurityTokenReceivedEventArgs securityTokenReceivedEventArgs = new SecurityTokenReceivedEventArgs(securityToken, federatedPassiveContext.SignInContext);
			OnSecurityTokenReceived(securityTokenReceivedEventArgs);
			if (!securityTokenReceivedEventArgs.Cancel)
			{
				IClaimsPrincipal claimsPrincipal = tokenReceiver.AuthenticateToken(securityTokenReceivedEventArgs.SecurityToken, ensureBearerToken: true, HttpContext.Current.Request.RawUrl);
				if (claimsPrincipal != null)
				{
					SecurityTokenValidatedEventArgs securityTokenValidatedEventArgs = new SecurityTokenValidatedEventArgs(claimsPrincipal);
					OnSecurityTokenValidated(securityTokenValidatedEventArgs);
					if (!securityTokenValidatedEventArgs.Cancel)
					{
						SessionAuthenticationModule current = SessionAuthenticationModule.Current;
						tokenReceiver.ComputeSessionTokenLifeTime(securityTokenReceivedEventArgs.SecurityToken, out var validFrom, out var validTo);
						Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken = current.CreateSessionSecurityToken(securityTokenValidatedEventArgs.ClaimsPrincipal, GetSessionTokenContext(), validFrom, validTo, federatedPassiveContext.RememberMe);
						SessionSecurityTokenCreatedEventArgs sessionSecurityTokenCreatedEventArgs = new SessionSecurityTokenCreatedEventArgs(sessionToken);
						sessionSecurityTokenCreatedEventArgs.WriteSessionCookie = SignInMode == SignInMode.Session;
						OnSessionSecurityTokenCreated(sessionSecurityTokenCreatedEventArgs);
						_activeModule.SetPrincipalAndWriteSessionToken(sessionSecurityTokenCreatedEventArgs.SessionToken, sessionSecurityTokenCreatedEventArgs.WriteSessionCookie);
						OnSignedIn(EventArgs.Empty);
						_responseReturnUrl = federatedPassiveContext.ReturnUrl;
						return true;
					}
				}
			}
			return false;
		}

		private void FederatedPassiveSignIn_Init(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				_initialized = true;
			}
		}

		private void OnClick(object sender, EventArgs e)
		{
			RedirectToIdentityProvider();
		}

		private void RedirectToIdentityProvider()
		{
			if (_activeModule != null)
			{
				_activeModule.RedirectToIdentityProvider(UniqueID, Page.Request.QueryString["ReturnUrl"], IsPersistentCookie);
				Page.Response.End();
			}
		}

		protected override string GetReturnUrl()
		{
			if (!string.IsNullOrEmpty(_responseReturnUrl))
			{
				return _responseReturnUrl;
			}
			return base.GetReturnUrl();
		}

		private WSFederationAuthenticationElement GetFAMConfiguration()
		{
			WSFederationAuthenticationElement result = null;
			ServiceElement defaultServiceElement = MicrosoftIdentityModelSection.DefaultServiceElement;
			if (defaultServiceElement != null)
			{
				result = defaultServiceElement.FederatedAuthentication.WSFederation;
			}
			return result;
		}

		private string GetParameterValue(string parameterName)
		{
			WSFederationAuthenticationElement fAMConfiguration = GetFAMConfiguration();
			if (UseFederationPropertiesFromConfiguration && fAMConfiguration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5003")));
			}
			return parameterName switch
			{
				"authenticationType" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.AuthenticationType : AuthenticationType, 
				"freshness" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.Freshness : Freshness, 
				"homeRealm" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.HomeRealm : HomeRealm, 
				"issuer" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.Issuer : Issuer, 
				"policy" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.Policy : Policy, 
				"realm" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.Realm : Realm, 
				"reply" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.Reply : Reply, 
				"request" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.Request : Request, 
				"requestPtr" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.RequestPtr : RequestPtr, 
				"resource" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.Resource : Resource, 
				"signInQueryString" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.SignInQueryString : SignInQueryString, 
				"requireHttps" => UseFederationPropertiesFromConfiguration ? fAMConfiguration.RequireHttps.ToString() : base.RequireHttps.ToString(), 
				_ => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5005", parameterName))), 
			};
		}

		private void ShowFamConfigMessage(string propertyName)
		{
			MessageBoxOptions messageBoxOptions = (MessageBoxOptions)0;
			if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
			{
				messageBoxOptions |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			MessageBox.Show(SR.GetString("FederatedPassiveSignIn_PropertySetWarning", propertyName), SR.GetString("FederatedPassiveSignIn_Property_Information"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, messageBoxOptions);
		}
	}
}
