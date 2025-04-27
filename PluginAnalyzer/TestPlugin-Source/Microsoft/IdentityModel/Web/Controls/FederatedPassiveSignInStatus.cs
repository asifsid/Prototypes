using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Web.Controls
{
	[Bindable(false)]
	[DefaultEvent("SigningOut")]
	[Designer(typeof(SignInStatusDesigner))]
	[ComVisible(true)]
	public class FederatedPassiveSignInStatus : CompositeControl
	{
		private static readonly object EventSigningOut = new object();

		private static readonly object EventSignedOut = new object();

		private static readonly object EventSignOutError = new object();

		private LinkButton _signInLinkButton;

		private ImageButton _signInImageButton;

		private Button _signInPushButton;

		private LinkButton _signOutLinkButton;

		private ImageButton _signOutImageButton;

		private Button _signOutPushButton;

		private bool _loggedIn;

		private ServiceConfiguration _serviceConfiguration;

		[WebDescription("SignInStatus_SignInButtonType")]
		[WebCategory("Category_Appearance")]
		[DefaultValue(ButtonType.Link)]
		public virtual ButtonType SignInButtonType
		{
			get
			{
				return (ButtonType)(ViewState["SignInButtonType"] ?? ((object)ButtonType.Link));
			}
			set
			{
				if (value < ButtonType.Button || value > ButtonType.Link)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value");
				}
				ViewState["SignInButtonType"] = value;
			}
		}

		[DefaultValue("")]
		[UrlProperty]
		[WebCategory("Category_Appearance")]
		[WebDescription("SignInStatus_SignInImageUrl")]
		[Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
		public virtual string SignInImageUrl
		{
			get
			{
				return ((string)ViewState["SignInImageUrl"]) ?? string.Empty;
			}
			set
			{
				ViewState["SignInImageUrl"] = value;
			}
		}

		[WebCategory("Category_Appearance")]
		[WebDescription("SignInStatus_SignInText")]
		[WebDefaultValue("SignInStatus_DefaultSignInText")]
		[Localizable(true)]
		public virtual string SignInText
		{
			get
			{
				return ((string)ViewState["SignInText"]) ?? SR.GetString("SignInStatus_DefaultSignInText");
			}
			set
			{
				ViewState["SignInText"] = value;
			}
		}

		[WebCategory("Category_Behavior")]
		[DefaultValue(SignOutAction.Refresh)]
		[Themeable(false)]
		[WebDescription("SignInStatus_SignOutAction")]
		public virtual SignOutAction SignOutAction
		{
			get
			{
				return (SignOutAction)(ViewState["SignOutAction"] ?? ((object)SignOutAction.Refresh));
			}
			set
			{
				if (value < SignOutAction.Refresh || value > SignOutAction.FederatedPassiveSignOut)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value");
				}
				ViewState["SignOutAction"] = value;
			}
		}

		[WebCategory("Category_Appearance")]
		[UrlProperty]
		[DefaultValue("")]
		[WebDescription("SignInStatus_SignOutImageUrl")]
		[Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
		public virtual string SignOutImageUrl
		{
			get
			{
				return ((string)ViewState["SignOutImageUrl"]) ?? string.Empty;
			}
			set
			{
				ViewState["SignOutImageUrl"] = value;
			}
		}

		[WebCategory("Category_Behavior")]
		[UrlProperty]
		[DefaultValue("")]
		[WebDescription("SignInStatus_SignOutPageUrl")]
		[Editor(typeof(UrlEditor), typeof(UITypeEditor))]
		[Themeable(false)]
		public virtual string SignOutPageUrl
		{
			get
			{
				return ((string)ViewState["SignOutPageUrl"]) ?? string.Empty;
			}
			set
			{
				ViewState["SignOutPageUrl"] = value;
			}
		}

		[WebDescription("SignInStatus_SignOutText")]
		[WebCategory("Category_Appearance")]
		[Localizable(true)]
		[WebDefaultValue("SignInStatus_DefaultSignOutText")]
		public virtual string SignOutText
		{
			get
			{
				return ((string)ViewState["SignOutText"]) ?? SR.GetString("SignInStatus_DefaultSignOutText");
			}
			set
			{
				ViewState["SignOutText"] = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ServiceConfiguration ServiceConfiguration
		{
			get
			{
				return _serviceConfiguration;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_serviceConfiguration = value;
			}
		}

		[WebDescription("SignInStatus_SignedOut")]
		[WebCategory("Category_Action")]
		public event EventHandler SignedOut
		{
			add
			{
				base.Events.AddHandler(EventSignedOut, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventSignedOut, value);
			}
		}

		[WebCategory("Category_Action")]
		[WebDescription("SignInStatus_SigningOut")]
		public event EventHandler<CancelEventArgs> SigningOut
		{
			add
			{
				base.Events.AddHandler(EventSigningOut, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventSigningOut, value);
			}
		}

		[WebDescription("SignInStatus_SignOutError")]
		[WebCategory("Category_Action")]
		public event EventHandler<ErrorEventArgs> SignOutError
		{
			add
			{
				base.Events.AddHandler(EventSignOutError, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventSignOutError, value);
			}
		}

		public FederatedPassiveSignInStatus()
		{
			_serviceConfiguration = FederatedAuthentication.ServiceConfiguration;
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (!base.DesignMode)
			{
				ControlUtil.EnsureSessionAuthenticationModule();
			}
		}

		protected override void CreateChildControls()
		{
			Controls.Clear();
			_signInLinkButton = new LinkButton();
			_signInLinkButton.ID = "signinLink";
			_signInImageButton = new ImageButton();
			_signInImageButton.ID = "signinImage";
			_signInPushButton = new Button();
			_signInPushButton.ID = "signinButton";
			_signOutLinkButton = new LinkButton();
			_signOutLinkButton.ID = "signoutLink";
			_signOutImageButton = new ImageButton();
			_signOutImageButton.ID = "signoutImage";
			_signOutPushButton = new Button();
			_signOutPushButton.ID = "signoutButton";
			LinkButton signInLinkButton = _signInLinkButton;
			LinkButton signInLinkButton2 = _signInLinkButton;
			bool flag2 = (_signInLinkButton.CausesValidation = false);
			bool enableViewState = (signInLinkButton2.EnableTheming = flag2);
			signInLinkButton.EnableViewState = enableViewState;
			ImageButton signInImageButton = _signInImageButton;
			ImageButton signInImageButton2 = _signInImageButton;
			bool flag5 = (_signInImageButton.CausesValidation = false);
			bool enableViewState2 = (signInImageButton2.EnableTheming = flag5);
			signInImageButton.EnableViewState = enableViewState2;
			Button signInPushButton = _signInPushButton;
			Button signInPushButton2 = _signInPushButton;
			bool flag8 = (_signInPushButton.CausesValidation = false);
			bool enableViewState3 = (signInPushButton2.EnableTheming = flag8);
			signInPushButton.EnableViewState = enableViewState3;
			LinkButton signOutLinkButton = _signOutLinkButton;
			LinkButton signOutLinkButton2 = _signOutLinkButton;
			bool flag11 = (_signOutLinkButton.CausesValidation = false);
			bool enableViewState4 = (signOutLinkButton2.EnableTheming = flag11);
			signOutLinkButton.EnableViewState = enableViewState4;
			ImageButton signOutImageButton = _signOutImageButton;
			ImageButton signOutImageButton2 = _signOutImageButton;
			bool flag14 = (_signOutImageButton.CausesValidation = false);
			bool enableViewState5 = (signOutImageButton2.EnableTheming = flag14);
			signOutImageButton.EnableViewState = enableViewState5;
			Button signOutPushButton = _signOutPushButton;
			Button signOutPushButton2 = _signOutPushButton;
			bool flag17 = (_signOutPushButton.CausesValidation = false);
			bool enableViewState6 = (signOutPushButton2.EnableTheming = flag17);
			signOutPushButton.EnableViewState = enableViewState6;
			CommandEventHandler value = SignOutClicked;
			_signOutLinkButton.Command += value;
			_signOutImageButton.Command += value;
			_signOutPushButton.Command += value;
			value = SignInClicked;
			_signInLinkButton.Command += value;
			_signInImageButton.Command += value;
			_signInPushButton.Command += value;
			Controls.Add(_signOutLinkButton);
			Controls.Add(_signOutImageButton);
			Controls.Add(_signOutPushButton);
			Controls.Add(_signInLinkButton);
			Controls.Add(_signInImageButton);
			Controls.Add(_signInPushButton);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			WSFederationAuthenticationModule wSFederationAuthenticationModule = new WSFederationAuthenticationModule();
			wSFederationAuthenticationModule.ServiceConfiguration = ServiceConfiguration;
			wSFederationAuthenticationModule.CanReadSignInResponse(Page.Request, onPage: true);
			_loggedIn = Page.Request.IsAuthenticated;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (Page != null)
			{
				Page.VerifyRenderingInServerForm(this);
			}
			SetChildProperties();
			base.RenderContents(writer);
		}

		private void SetChildProperties()
		{
			EnsureChildControls();
			LinkButton signInLinkButton = _signInLinkButton;
			bool visible = (_signInLinkButton.Enabled = false);
			signInLinkButton.Visible = visible;
			ImageButton signInImageButton = _signInImageButton;
			bool visible2 = (_signInImageButton.Enabled = false);
			signInImageButton.Visible = visible2;
			Button signInPushButton = _signInPushButton;
			bool visible3 = (_signInPushButton.Enabled = false);
			signInPushButton.Visible = visible3;
			LinkButton signOutLinkButton = _signOutLinkButton;
			bool visible4 = (_signOutLinkButton.Enabled = false);
			signOutLinkButton.Visible = visible4;
			ImageButton signOutImageButton = _signOutImageButton;
			bool visible5 = (_signOutImageButton.Enabled = false);
			signOutImageButton.Visible = visible5;
			Button signOutPushButton = _signOutPushButton;
			bool visible6 = (_signOutPushButton.Enabled = false);
			signOutPushButton.Visible = visible6;
			WebControl webControl = null;
			Type type = GetType();
			if (_loggedIn)
			{
				switch (SignInButtonType)
				{
				case ButtonType.Link:
					_signOutLinkButton.Text = SignOutText;
					webControl = _signOutLinkButton;
					break;
				case ButtonType.Image:
					_signOutImageButton.AlternateText = SignOutText;
					_signOutImageButton.ToolTip = ToolTip;
					_signOutImageButton.ImageUrl = ((!string.IsNullOrEmpty(SignOutImageUrl)) ? SignOutImageUrl : Page.ClientScript.GetWebResourceUrl(type, type.FullName + "SignOut.png"));
					webControl = _signOutImageButton;
					break;
				case ButtonType.Button:
					_signOutPushButton.Text = SignOutText;
					webControl = _signOutPushButton;
					break;
				}
			}
			else
			{
				switch (SignInButtonType)
				{
				case ButtonType.Link:
					_signInLinkButton.Text = SignInText;
					webControl = _signInLinkButton;
					break;
				case ButtonType.Image:
					_signInImageButton.AlternateText = SignInText;
					_signInImageButton.ToolTip = ToolTip;
					_signInImageButton.ImageUrl = ((!string.IsNullOrEmpty(SignInImageUrl)) ? SignInImageUrl : Page.ClientScript.GetWebResourceUrl(type, type.FullName + "SignIn.png"));
					webControl = _signInImageButton;
					break;
				case ButtonType.Button:
					_signInPushButton.Text = SignInText;
					webControl = _signInPushButton;
					break;
				}
			}
			if (webControl != null)
			{
				WebControl webControl2 = webControl;
				bool visible7 = (webControl.Enabled = true);
				webControl2.Visible = visible7;
				webControl.CopyBaseAttributes(this);
				webControl.ApplyStyle(base.ControlStyle);
			}
		}

		private string ResolveSignOutPageUrl(string urlValue)
		{
			_ = string.Empty;
			if (string.IsNullOrEmpty(urlValue))
			{
				urlValue = FormsAuthentication.LoginUrl;
			}
			if (!UriUtil.CanCreateValidUri(urlValue, UriKind.Absolute))
			{
				string path = ResolveUrl(urlValue);
				UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url);
				uriBuilder.Path = path;
				return uriBuilder.Uri.OriginalString;
			}
			return urlValue;
		}

		private void SignOutClicked(object sender, CommandEventArgs e)
		{
			SignOut();
		}

		private void SignOut()
		{
			try
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs();
				OnSigningOut(cancelEventArgs);
				if (cancelEventArgs.Cancel)
				{
					return;
				}
				try
				{
					FormsAuthentication.SignOut();
				}
				finally
				{
					SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
					sessionAuthenticationModule.DeleteSessionTokenCookie();
				}
				OnSignedOut(EventArgs.Empty);
			}
			catch (Exception ex)
			{
				if (DiagnosticUtil.IsFatal(ex))
				{
					throw;
				}
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8012", ID, ex));
				}
				ErrorEventArgs errorEventArgs = new ErrorEventArgs(cancel: true, ex);
				OnSignOutError(errorEventArgs);
				if (!errorEventArgs.Cancel)
				{
					throw;
				}
			}
			string text = ((SignOutAction != SignOutAction.FederatedPassiveSignOut) ? GetSignOutUrl(SignOutAction, ResolveClientUrl(SignOutPageUrl)) : GetSignOutUrl(SignOutAction, ResolveSignOutPageUrl(SignOutPageUrl)));
			if (!string.IsNullOrEmpty(text))
			{
				Page.Response.Redirect(ResolveClientUrl(text));
			}
		}

		internal static string GetSignOutUrl(SignOutAction signOutAction, string signOutPageUrl)
		{
			return GetSignOutUrl(HttpContext.Current.Request, signOutAction, signOutPageUrl);
		}

		internal static string GetSignOutUrl(HttpRequest request, SignOutAction signOutAction, string signOutPageUrl)
		{
			switch (signOutAction)
			{
			case SignOutAction.RedirectToLoginPage:
				return FormsAuthentication.LoginUrl;
			default:
				if (string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
				{
					return request.Path;
				}
				return request.RawUrl;
			case SignOutAction.Redirect:
				if (!string.IsNullOrEmpty(signOutPageUrl))
				{
					return signOutPageUrl;
				}
				return FormsAuthentication.LoginUrl;
			case SignOutAction.FederatedPassiveSignOut:
			{
				string uriString = (string.IsNullOrEmpty(signOutPageUrl) ? FormsAuthentication.LoginUrl : signOutPageUrl);
				return WSFederationAuthenticationModule.GetFederationPassiveSignOutUrl(null, new Uri(uriString)).AbsoluteUri;
			}
			}
		}

		private void SignInClicked(object Source, CommandEventArgs e)
		{
			Page.Response.Redirect(ResolveClientUrl(ControlUtil.GetLoginPage(Context, null, reuseReturnUrl: true)), endResponse: false);
		}

		protected virtual void OnSigningOut(CancelEventArgs e)
		{
			((EventHandler<CancelEventArgs>)base.Events[EventSigningOut])?.Invoke(this, e);
		}

		protected virtual void OnSignedOut(EventArgs e)
		{
			((EventHandler)base.Events[EventSignedOut])?.Invoke(this, e);
		}

		protected virtual void OnSignOutError(ErrorEventArgs e)
		{
			((EventHandler<ErrorEventArgs>)base.Events[EventSignOutError])?.Invoke(this, e);
		}

		protected override void SetDesignModeState(IDictionary data)
		{
			if (data != null)
			{
				object obj = data["LoggedIn"];
				if (obj != null)
				{
					_loggedIn = (bool)obj;
				}
			}
		}
	}
}
