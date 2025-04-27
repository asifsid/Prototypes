using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace Microsoft.IdentityModel.Web.Controls
{
	[ComVisible(true)]
	public abstract class SignInControl : CompositeControl
	{
		internal sealed class SignInContainer : WebControlContainer<SignInControl>
		{
			private const string ErrorTextId = "ErrorTextId";

			private HyperLink _createUserLink;

			private LiteralControl _createUserLinkSeparator;

			private Control _errorTextLabel;

			private HyperLink _helpPageLink;

			private IButtonControl _button;

			private Literal _title;

			private Image _createUserIcon;

			private Image _helpPageIcon;

			private WebControl _rememberMeCheckBox;

			private SimpleButton _simpleButton = new SimpleButton();

			protected override bool ConvertingToTemplate => base.Owner.ConvertingToTemplate;

			internal HyperLink CreateUserLink
			{
				get
				{
					return _createUserLink;
				}
				set
				{
					_createUserLink = value;
				}
			}

			internal LiteralControl CreateUserLinkSeparator
			{
				get
				{
					return _createUserLinkSeparator;
				}
				set
				{
					_createUserLinkSeparator = value;
				}
			}

			internal Image HelpPageIcon
			{
				get
				{
					return _helpPageIcon;
				}
				set
				{
					_helpPageIcon = value;
				}
			}

			internal Image CreateUserIcon
			{
				get
				{
					return _createUserIcon;
				}
				set
				{
					_createUserIcon = value;
				}
			}

			internal Control ErrorTextLabel
			{
				get
				{
					return _errorTextLabel;
				}
				set
				{
					_errorTextLabel = value;
				}
			}

			internal HyperLink HelpPageLink
			{
				get
				{
					return _helpPageLink;
				}
				set
				{
					_helpPageLink = value;
				}
			}

			internal IButtonControl Button
			{
				get
				{
					return _button;
				}
				set
				{
					_button = value;
				}
			}

			internal WebControl RememberMeCheckBox
			{
				get
				{
					return _rememberMeCheckBox;
				}
				set
				{
					_rememberMeCheckBox = value;
				}
			}

			internal WebControl ActiveButton => base.Owner.SignInButtonType switch
			{
				ButtonType.Button => _simpleButton, 
				_ => (WebControl)_button, 
			};

			internal Literal Title
			{
				get
				{
					return _title;
				}
				set
				{
					_title = value;
				}
			}

			public SignInContainer(SignInControl owner)
				: base(owner, owner.DesignMode)
			{
			}
		}

		internal class SignInTemplate : ITemplate
		{
			private SignInControl _owner;

			private IButtonControl _button;

			public SignInTemplate(SignInControl owner, IButtonControl button)
			{
				_owner = owner;
				_button = button;
			}

			private void CreateControls(SignInContainer loginContainer)
			{
				Literal literal2 = (loginContainer.Title = new Literal());
				loginContainer.Button = _button;
				WebControl webControl = new CheckBox();
				webControl.ID = "rememberMe";
				loginContainer.RememberMeCheckBox = webControl;
				HyperLink hyperLink2 = (loginContainer.CreateUserLink = new HyperLink());
				LiteralControl literalControl2 = (loginContainer.CreateUserLinkSeparator = new LiteralControl());
				HyperLink hyperLink4 = (loginContainer.HelpPageLink = new HyperLink());
				Literal literal3 = (Literal)(loginContainer.ErrorTextLabel = new Literal());
				loginContainer.HelpPageIcon = new Image();
				loginContainer.CreateUserIcon = new Image();
			}

			private void LayoutControls(SignInContainer loginContainer)
			{
				if (_owner.Orientation == Orientation.Vertical)
				{
					LayoutVertical(loginContainer);
				}
				else
				{
					LayoutHorizontal(loginContainer);
				}
			}

			private void LayoutHorizontal(SignInContainer loginContainer)
			{
				Table table = new Table();
				table.CellPadding = 0;
				TableRow tableRow = new ControlUtil.DisappearingTableRow();
				TableCell tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.ActiveButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new ControlUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(loginContainer.RememberMeCheckBox);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new ControlUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(loginContainer.ErrorTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new ControlUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.ColumnSpan = 2;
				tableCell.Controls.Add(loginContainer.CreateUserIcon);
				tableCell.Controls.Add(loginContainer.CreateUserLink);
				loginContainer.CreateUserLinkSeparator.Text = " ";
				tableCell.Controls.Add(loginContainer.CreateUserLinkSeparator);
				tableCell.Controls.Add(loginContainer.HelpPageIcon);
				tableCell.Controls.Add(loginContainer.HelpPageLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				Table table2 = ControlUtil.CreateChildTable(_owner.ConvertingToTemplate);
				tableRow = new TableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(table);
				tableRow.Cells.Add(tableCell);
				table2.Rows.Add(tableRow);
				loginContainer.LayoutTable = table;
				loginContainer.BorderTable = table2;
				loginContainer.Controls.Add(table2);
			}

			private void LayoutVertical(SignInContainer loginContainer)
			{
				Table table = new Table();
				table.CellPadding = 0;
				TableRow tableRow = new ControlUtil.DisappearingTableRow();
				TableCell tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.Title);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new ControlUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.ActiveButton);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new ControlUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.RememberMeCheckBox);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new ControlUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.HorizontalAlign = HorizontalAlign.Center;
				tableCell.Controls.Add(loginContainer.ErrorTextLabel);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				tableRow = new ControlUtil.DisappearingTableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(loginContainer.CreateUserIcon);
				tableCell.Controls.Add(loginContainer.CreateUserLink);
				tableCell.Controls.Add(loginContainer.CreateUserLinkSeparator);
				loginContainer.CreateUserLinkSeparator.Text = "<br />";
				tableCell.Controls.Add(loginContainer.HelpPageIcon);
				tableCell.Controls.Add(loginContainer.HelpPageLink);
				tableRow.Cells.Add(tableCell);
				table.Rows.Add(tableRow);
				Table table2 = ControlUtil.CreateChildTable(_owner.ConvertingToTemplate);
				tableRow = new TableRow();
				tableCell = new TableCell();
				tableCell.Controls.Add(table);
				tableRow.Cells.Add(tableCell);
				table2.Rows.Add(tableRow);
				loginContainer.LayoutTable = table;
				loginContainer.BorderTable = table2;
				loginContainer.Controls.Add(table2);
			}

			void ITemplate.InstantiateIn(Control container)
			{
				SignInContainer loginContainer = (SignInContainer)container;
				CreateControls(loginContainer);
				LayoutControls(loginContainer);
			}
		}

		private const string ErrorParameterName = "signinfailure";

		private const int ViewStateArrayLength = 5;

		private static readonly object EventRedirectingToIdentityProvider = new object();

		private static readonly object EventSecurityTokenReceived = new object();

		private static readonly object EventSecurityTokenValidated = new object();

		private static readonly object EventSessionSecurityTokenCreated = new object();

		private static readonly object EventSignedIn = new object();

		private static readonly object EventSignInError = new object();

		private TableItemStyle _errorTextStyle;

		private TableItemStyle _titleTextStyle;

		private TableItemStyle _checkBoxStyle;

		private Style _signInButtonStyle;

		private SignInContainer _templateContainer;

		private bool _convertingToTemplate;

		private bool _renderDesignerRegion;

		private IButtonControl _signInButton;

		private ServiceConfiguration _serviceConfiguration;

		protected IButtonControl SignInButton
		{
			get
			{
				return _signInButton;
			}
			set
			{
				_signInButton = value;
			}
		}

		[WebCategory("Category_Behavior")]
		[Themeable(false)]
		[WebDescription("SignIn_AutoSignIn")]
		[DefaultValue(false)]
		public virtual bool AutoSignIn
		{
			get
			{
				return (bool)(ViewState["AutoSignIn"] ?? ((object)false));
			}
			set
			{
				ViewState["AutoSignIn"] = value;
			}
		}

		[WebCategory("Category_Appearance")]
		[WebDescription("SignIn_BorderPadding")]
		[DefaultValue(1)]
		public virtual int BorderPadding
		{
			get
			{
				return (int)(ViewState["BorderPadding"] ?? ((object)1));
			}
			set
			{
				if (value < -1)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value");
				}
				ViewState["BorderPadding"] = value;
			}
		}

		[UrlProperty]
		[DefaultValue("")]
		[WebDescription("SignIn_DestinationPageUrl")]
		[WebCategory("Category_Behavior")]
		[Editor(typeof(UrlEditor), typeof(UITypeEditor))]
		[Themeable(false)]
		public virtual string DestinationPageUrl
		{
			get
			{
				return ((string)ViewState["DestinationPageUrl"]) ?? string.Empty;
			}
			set
			{
				ViewState["DestinationPageUrl"] = value;
			}
		}

		[WebDescription("SignIn_Orientation")]
		[WebCategory("Category_Layout")]
		[DefaultValue(Orientation.Vertical)]
		public virtual Orientation Orientation
		{
			get
			{
				return (Orientation)(ViewState["Orientation"] ?? ((object)Orientation.Vertical));
			}
			set
			{
				if (value < Orientation.Horizontal || value > Orientation.Vertical)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value");
				}
				ViewState["Orientation"] = value;
				base.ChildControlsCreated = false;
			}
		}

		[DefaultValue(LoginFailureAction.Refresh)]
		[WebDescription("SignIn_ErrorAction")]
		[WebCategory("Category_Behavior")]
		[Themeable(false)]
		public virtual LoginFailureAction ErrorAction
		{
			get
			{
				return (LoginFailureAction)(ViewState["ErrorAction"] ?? ((object)LoginFailureAction.Refresh));
			}
			set
			{
				if (value < LoginFailureAction.Refresh || value > LoginFailureAction.RedirectToLoginPage)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value");
				}
				ViewState["ErrorAction"] = value;
			}
		}

		[WebDefaultValue("SignIn_DefaultErrorText")]
		[Localizable(true)]
		[WebCategory("Category_Appearance")]
		[WebDescription("SignIn_ErrorText")]
		public virtual string ErrorText
		{
			get
			{
				return ((string)ViewState["ErrorText"]) ?? SR.GetString("SignIn_DefaultErrorText");
			}
			set
			{
				ViewState["ErrorText"] = value;
			}
		}

		[WebCategory("Category_Styles")]
		[WebDescription("SignIn_ErrorTextStyle")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public TableItemStyle ErrorTextStyle
		{
			get
			{
				if (_errorTextStyle == null)
				{
					_errorTextStyle = new ErrorTableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)_errorTextStyle).TrackViewState();
					}
				}
				return _errorTextStyle;
			}
		}

		[WebDescription("SignIn_SignInButtonStyle")]
		[WebCategory("Category_Styles")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style SignInButtonStyle
		{
			get
			{
				if (_signInButtonStyle == null)
				{
					_signInButtonStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)_signInButtonStyle).TrackViewState();
					}
				}
				return _signInButtonStyle;
			}
		}

		[WebCategory("Category_Appearance")]
		[DefaultValue("")]
		[WebDescription("SignIn_SignInImageUrl")]
		[Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
		[UrlProperty]
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
		[WebDescription("SignIn_SignInText")]
		[Localizable(true)]
		[WebDefaultValue("SignIn_DefaultSignInText")]
		public virtual string SignInText
		{
			get
			{
				return ((string)ViewState["SignInText"]) ?? SR.GetString("SignIn_DefaultSignInText");
			}
			set
			{
				ViewState["SignInText"] = value;
			}
		}

		[WebDescription("SignIn_SignInButtonType")]
		[WebCategory("Category_Appearance")]
		[DefaultValue(ButtonType.Image)]
		public virtual ButtonType SignInButtonType
		{
			get
			{
				return (ButtonType)(ViewState["SignInButtonType"] ?? ((object)ButtonType.Image));
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

		[WebDescription("SignIn_ShowButtonImage")]
		[DefaultValue(true)]
		[WebCategory("Category_Appearance")]
		public virtual bool ShowButtonImage
		{
			get
			{
				return (bool)(ViewState["ShowButtonImage"] ?? ((object)true));
			}
			set
			{
				ViewState["ShowButtonImage"] = value;
			}
		}

		[WebCategory("Category_Behavior")]
		[WebDescription("SignIn_SignInContext")]
		[DefaultValue("")]
		public string SignInContext
		{
			get
			{
				return ((string)ViewState["SignInContext"]) ?? string.Empty;
			}
			set
			{
				ViewState["SignInContext"] = value;
			}
		}

		[WebDescription("SignIn_TitleText")]
		[WebCategory("Category_Appearance")]
		[Localizable(true)]
		[WebDefaultValue("SignIn_DefaultTitleText")]
		public virtual string TitleText
		{
			get
			{
				return ((string)ViewState["TitleText"]) ?? SR.GetString("SignIn_DefaultTitleText");
			}
			set
			{
				ViewState["TitleText"] = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Category_Styles")]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebDescription("SignIn_TitleTextStyle")]
		[DefaultValue(null)]
		public TableItemStyle TitleTextStyle
		{
			get
			{
				if (_titleTextStyle == null)
				{
					_titleTextStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)_titleTextStyle).TrackViewState();
					}
				}
				return _titleTextStyle;
			}
		}

		[Themeable(false)]
		[DefaultValue(true)]
		[WebDescription("SignIn_DisplayRememberMe")]
		[WebCategory("Category_Behavior")]
		public virtual bool DisplayRememberMe
		{
			get
			{
				return (bool)(ViewState["DisplayRememberMe"] ?? ((object)true));
			}
			set
			{
				ViewState["DisplayRememberMe"] = value;
			}
		}

		[WebDescription("SignIn_RememberMeSet")]
		[DefaultValue(false)]
		[WebCategory("Category_Behavior")]
		[Themeable(false)]
		public virtual bool RememberMeSet
		{
			get
			{
				return (bool)(ViewState["RememberMeSet"] ?? ((object)false));
			}
			set
			{
				ViewState["RememberMeSet"] = value;
			}
		}

		[WebCategory("Category_Appearance")]
		[WebDefaultValue("SignIn_DefaultRememberMeText")]
		[WebDescription("SignIn_RememberMeText")]
		[Localizable(true)]
		public virtual string RememberMeText
		{
			get
			{
				return ((string)ViewState["RememberMeText"]) ?? SR.GetString("SignIn_DefaultRememberMeText");
			}
			set
			{
				ViewState["RememberMeText"] = value;
			}
		}

		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebDescription("SignIn_CheckBoxStyle")]
		[WebCategory("Category_Styles")]
		public TableItemStyle CheckBoxStyle
		{
			get
			{
				if (_checkBoxStyle == null)
				{
					_checkBoxStyle = new TableItemStyle();
					if (base.IsTrackingViewState)
					{
						((IStateManager)_checkBoxStyle).TrackViewState();
					}
				}
				return _checkBoxStyle;
			}
		}

		[WebCategory("Category_Behavior")]
		[Themeable(false)]
		[WebDescription("SignIn_VisibleWhenSignedIn")]
		[DefaultValue(true)]
		public virtual bool VisibleWhenSignedIn
		{
			get
			{
				return (bool)(ViewState["VisibleWhenSignedIn"] ?? ((object)true));
			}
			set
			{
				ViewState["VisibleWhenSignedIn"] = value;
			}
		}

		[WebDescription("SignIn_RequireHttps")]
		[DefaultValue(true)]
		[WebCategory("Category_Behavior")]
		public bool RequireHttps
		{
			get
			{
				return (bool)(ViewState["RequireHttps"] ?? ((object)true));
			}
			set
			{
				ViewState["RequireHttps"] = value;
			}
		}

		[DefaultValue(SignInMode.Session)]
		[Themeable(false)]
		[WebDescription("SignIn_SignInMode")]
		[WebCategory("Category_Behavior")]
		public virtual SignInMode SignInMode
		{
			get
			{
				return (SignInMode)(ViewState["SignInMode"] ?? ((object)SignInMode.Session));
			}
			set
			{
				if (!SignInModeHelper.IsDefined(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value");
				}
				ViewState["SignInMode"] = value;
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

		protected string ButtonClientId => _templateContainer.ActiveButton.ClientID;

		protected string RememberMeClientId => _templateContainer.RememberMeCheckBox.ClientID;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		private SignInContainer TemplateContainer
		{
			get
			{
				EnsureChildControls();
				return _templateContainer;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string ClientSideSignInFunction => UniqueID + "OnClick";

		private bool ConvertingToTemplate
		{
			get
			{
				if (base.DesignMode)
				{
					return _convertingToTemplate;
				}
				return false;
			}
		}

		protected WebControl ActiveButton => TemplateContainer.ActiveButton;

		protected virtual bool IsPersistentCookie => ((ICheckBoxControl)_templateContainer.RememberMeCheckBox).Checked;

		[WebCategory("Category_Action")]
		[WebDescription("SignIn_RedirectingToIdentityProvider")]
		public event EventHandler<RedirectingToIdentityProviderEventArgs> RedirectingToIdentityProvider
		{
			add
			{
				base.Events.AddHandler(EventRedirectingToIdentityProvider, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventRedirectingToIdentityProvider, value);
			}
		}

		[WebCategory("Category_Action")]
		[WebDescription("SignIn_TokenReceived")]
		public event EventHandler<SecurityTokenReceivedEventArgs> SecurityTokenReceived
		{
			add
			{
				base.Events.AddHandler(EventSecurityTokenReceived, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventSecurityTokenReceived, value);
			}
		}

		[WebDescription("SignIn_TokenValidated")]
		[WebCategory("Category_Action")]
		public event EventHandler<SecurityTokenValidatedEventArgs> SecurityTokenValidated
		{
			add
			{
				base.Events.AddHandler(EventSecurityTokenValidated, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventSecurityTokenValidated, value);
			}
		}

		[WebCategory("Category_Action")]
		[WebDescription("SignIn_SessionTokenCreated")]
		public event EventHandler<SessionSecurityTokenCreatedEventArgs> SessionSecurityTokenCreated
		{
			add
			{
				base.Events.AddHandler(EventSessionSecurityTokenCreated, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventSessionSecurityTokenCreated, value);
			}
		}

		[WebDescription("SignIn_SignedIn")]
		[WebCategory("Category_Action")]
		public event EventHandler SignedIn
		{
			add
			{
				base.Events.AddHandler(EventSignedIn, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventSignedIn, value);
			}
		}

		[WebDescription("SignIn_SignInError")]
		[WebCategory("Category_Action")]
		public event EventHandler<ErrorEventArgs> SignInError
		{
			add
			{
				base.Events.AddHandler(EventSignInError, value);
			}
			remove
			{
				base.Events.RemoveHandler(EventSignInError, value);
			}
		}

		protected event EventHandler Click;

		protected SignInControl()
		{
			_serviceConfiguration = FederatedAuthentication.ServiceConfiguration;
		}

		protected abstract bool SignIn();

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (!base.DesignMode)
			{
				ControlUtil.EnsureSessionAuthenticationModule();
			}
			switch (SignInButtonType)
			{
			case ButtonType.Image:
				SignInButton = new ImageButton();
				SignInButton.Click += SignInButton_Click;
				break;
			case ButtonType.Link:
				SignInButton = new LinkButton();
				SignInButton.Click += SignInButton_Click;
				break;
			}
		}

		private void SignInButton_Click(object sender, EventArgs e)
		{
			if (this.Click != null)
			{
				this.Click(this, e);
			}
		}

		protected override void CreateChildControls()
		{
			Controls.Clear();
			_templateContainer = new SignInContainer(this);
			_templateContainer.RenderDesignerRegion = _renderDesignerRegion;
			_templateContainer.EnableViewState = false;
			_templateContainer.EnableTheming = false;
			switch (SignInButtonType)
			{
			case ButtonType.Button:
				SignInButton = new Button();
				break;
			case ButtonType.Image:
				SignInButton = new ImageButton();
				SignInButton.Click += SignInButton_Click;
				break;
			case ButtonType.Link:
				SignInButton = new LinkButton();
				SignInButton.Click += SignInButton_Click;
				break;
			}
			ITemplate template = new SignInTemplate(this, SignInButton);
			template.InstantiateIn(_templateContainer);
			_templateContainer.Visible = true;
			Controls.Add(_templateContainer);
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (!base.DesignMode)
			{
				ControlUtil.EnsureAutoSignInNotSetOnMultipleControls(Page);
			}
			if (Page.IsPostBack && Page.Request.Form["__EVENTTARGET"] == UniqueID)
			{
				SignInButton_Click(this, EventArgs.Empty);
			}
			base.OnPreRender(e);
			try
			{
				if (SignIn() && SignInMode == SignInMode.Session)
				{
					string redirectUrl = GetRedirectUrl();
					if (!ControlUtil.IsAppRelative(redirectUrl))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelper(new FederationException(SR.GetString("ID3206", redirectUrl)), TraceEventType.Error);
					}
					Page.Response.Redirect(CookieHandler.MatchCookiePath(redirectUrl));
				}
			}
			catch (Exception exception)
			{
				if (!HandleSignInException(exception))
				{
					throw;
				}
			}
			TemplateContainer.Visible = VisibleWhenSignedIn || !Page.Request.IsAuthenticated;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (Page != null)
			{
				Page.VerifyRenderingInServerForm(this);
			}
			if (base.DesignMode)
			{
				base.ChildControlsCreated = false;
				EnsureChildControls();
			}
			if (TemplateContainer.Visible)
			{
				SetChildProperties();
				RenderContents(writer);
			}
			if (!base.DesignMode)
			{
				if (TemplateContainer.Visible)
				{
					writer.Write(GetClientScript());
				}
				if (Enabled && AutoSignIn && string.IsNullOrEmpty(((ITextControl)TemplateContainer.ErrorTextLabel).Text))
				{
					writer.Write(GetAutoSignInScript());
				}
			}
		}

		private void SetChildProperties()
		{
			SetCommonChildProperties();
			SetDefaultTemplateChildProperties();
		}

		private void SetCommonChildProperties()
		{
			SignInContainer templateContainer = TemplateContainer;
			ControlUtil.CopyBaseAttributesToInnerControl(this, templateContainer);
			templateContainer.ApplyStyle(base.ControlStyle);
			ITextControl textControl = (ITextControl)templateContainer.ErrorTextLabel;
			string errorText = ErrorText;
			if (textControl != null && errorText.Length > 0 && RedirectedFromFailedLogin())
			{
				textControl.Text = errorText;
			}
		}

		private void SetDefaultTemplateChildProperties()
		{
			SignInContainer templateContainer = TemplateContainer;
			templateContainer.BorderTable.CellPadding = BorderPadding;
			templateContainer.BorderTable.CellSpacing = 0;
			Literal title = templateContainer.Title;
			string titleText = TitleText;
			if (titleText.Length > 0)
			{
				title.Text = titleText;
				if (_titleTextStyle != null)
				{
					ControlUtil.SetTableCellStyle(title, TitleTextStyle);
				}
				ControlUtil.SetTableCellVisible(title, visible: true);
			}
			else
			{
				ControlUtil.SetTableCellVisible(title, visible: false);
			}
			WebControl rememberMeCheckBox = templateContainer.RememberMeCheckBox;
			if (DisplayRememberMe)
			{
				CheckBox checkBox = rememberMeCheckBox as CheckBox;
				if (checkBox != null)
				{
					checkBox.Text = RememberMeText;
				}
				else
				{
					((SimpleCheckBox)rememberMeCheckBox).Text = RememberMeText;
				}
				((ICheckBoxControl)rememberMeCheckBox).Checked = RememberMeSet;
				if (_checkBoxStyle != null)
				{
					ControlUtil.SetTableCellStyle(rememberMeCheckBox, CheckBoxStyle);
				}
				ControlUtil.SetTableCellVisible(rememberMeCheckBox, visible: true);
			}
			else
			{
				ControlUtil.SetTableCellVisible(rememberMeCheckBox, visible: false);
			}
			rememberMeCheckBox.TabIndex = TabIndex;
			Type type = GetType();
			bool flag = SignInButtonType == ButtonType.Image;
			string text = null;
			if (!base.DesignMode && !string.IsNullOrEmpty(GetClientScript()))
			{
				text = string.Format(CultureInfo.InvariantCulture, "javascript:{0}({1});", new object[2]
				{
					ClientSideSignInFunction,
					flag ? "true" : string.Empty
				});
			}
			switch (SignInButtonType)
			{
			case ButtonType.Link:
			{
				LinkButton linkButton = (LinkButton)templateContainer.ActiveButton;
				linkButton.Text = SignInText;
				if (!string.IsNullOrEmpty(text))
				{
					linkButton.OnClientClick = text;
				}
				break;
			}
			case ButtonType.Image:
			{
				ImageButton imageButton = (ImageButton)templateContainer.ActiveButton;
				imageButton.ImageUrl = ((!string.IsNullOrEmpty(SignInImageUrl)) ? SignInImageUrl : Page.ClientScript.GetWebResourceUrl(type, type.FullName + ".png"));
				imageButton.AlternateText = SignInText;
				imageButton.ToolTip = ToolTip;
				if (!string.IsNullOrEmpty(text))
				{
					imageButton.OnClientClick = text;
				}
				break;
			}
			case ButtonType.Button:
			{
				SimpleButton simpleButton = (SimpleButton)templateContainer.ActiveButton;
				simpleButton.Text = SignInText;
				if (this.Click != null)
				{
					simpleButton.OnClientClick = $"javascript:__doPostBack('{UniqueID}');";
				}
				else
				{
					simpleButton.OnClientClick = text;
				}
				if (ShowButtonImage)
				{
					simpleButton.ImageUrl = ((!string.IsNullOrEmpty(SignInImageUrl)) ? SignInImageUrl : Page.ClientScript.GetWebResourceUrl(type, type.FullName + "Button.png"));
				}
				break;
			}
			}
			templateContainer.ActiveButton.TabIndex = TabIndex;
			templateContainer.ActiveButton.AccessKey = AccessKey;
			if (_signInButtonStyle != null)
			{
				templateContainer.ActiveButton.ApplyStyle(SignInButtonStyle);
			}
			Image createUserIcon = templateContainer.CreateUserIcon;
			HyperLink createUserLink = templateContainer.CreateUserLink;
			LiteralControl createUserLinkSeparator = templateContainer.CreateUserLinkSeparator;
			HyperLink helpPageLink = templateContainer.HelpPageLink;
			Image helpPageIcon = templateContainer.HelpPageIcon;
			LiteralControl literalControl = new LiteralControl("|");
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = flag3 || flag4;
			bool flag7 = flag2 || flag5;
			helpPageLink.Visible = flag3;
			literalControl.Visible = flag6 && flag7;
			helpPageIcon.Visible = flag4;
			createUserLink.Visible = flag2;
			createUserLinkSeparator.Visible = flag7;
			createUserIcon.Visible = flag5;
			if (flag7 || flag6)
			{
				ControlUtil.SetTableCellVisible(helpPageLink, visible: true);
			}
			else
			{
				ControlUtil.SetTableCellVisible(helpPageLink, visible: false);
			}
			Control errorTextLabel = templateContainer.ErrorTextLabel;
			if (((ITextControl)errorTextLabel).Text.Length > 0)
			{
				ControlUtil.SetTableCellStyle(errorTextLabel, ErrorTextStyle);
				ControlUtil.SetTableCellVisible(errorTextLabel, visible: true);
			}
			else
			{
				ControlUtil.SetTableCellVisible(errorTextLabel, visible: false);
			}
		}

		protected virtual string GetClientScript()
		{
			return string.Empty;
		}

		protected virtual string GetAutoSignInScript()
		{
			return string.Empty;
		}

		protected virtual string GetReturnUrl()
		{
			return ControlUtil.GetReturnUrl(Context, useDefaultIfAbsent: false);
		}

		protected virtual string GetSessionTokenContext()
		{
			return null;
		}

		protected virtual void OnRedirectingToIdentityProvider(RedirectingToIdentityProviderEventArgs e)
		{
			((EventHandler<RedirectingToIdentityProviderEventArgs>)base.Events[EventRedirectingToIdentityProvider])?.Invoke(this, e);
		}

		protected virtual void OnSecurityTokenReceived(SecurityTokenReceivedEventArgs e)
		{
			((EventHandler<SecurityTokenReceivedEventArgs>)base.Events[EventSecurityTokenReceived])?.Invoke(this, e);
		}

		protected virtual void OnSecurityTokenValidated(SecurityTokenValidatedEventArgs e)
		{
			((EventHandler<SecurityTokenValidatedEventArgs>)base.Events[EventSecurityTokenValidated])?.Invoke(this, e);
		}

		protected virtual void OnSessionSecurityTokenCreated(SessionSecurityTokenCreatedEventArgs e)
		{
			((EventHandler<SessionSecurityTokenCreatedEventArgs>)base.Events[EventSessionSecurityTokenCreated])?.Invoke(this, e);
		}

		protected virtual void OnSignedIn(EventArgs e)
		{
			((EventHandler)base.Events[EventSignedIn])?.Invoke(this, e);
		}

		protected virtual void OnSignInError(ErrorEventArgs e)
		{
			((EventHandler<ErrorEventArgs>)base.Events[EventSignInError])?.Invoke(this, e);
		}

		private bool HandleSignInException(Exception exception)
		{
			if (DiagnosticUtil.IsFatal(exception))
			{
				return false;
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
			{
				DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8013", ID, exception));
			}
			ErrorEventArgs errorEventArgs = new ErrorEventArgs(cancel: true, exception);
			OnSignInError(errorEventArgs);
			if (!errorEventArgs.Cancel)
			{
				return false;
			}
			ITextControl textControl = (ITextControl)TemplateContainer.ErrorTextLabel;
			if (textControl != null)
			{
				textControl.Text = ErrorText;
			}
			return true;
		}

		private string GetRedirectUrl()
		{
			if (ControlUtil.OnLoginPage(Context) || AutoSignIn)
			{
				string returnUrl = GetReturnUrl();
				if (!string.IsNullOrEmpty(returnUrl))
				{
					return returnUrl;
				}
				string destinationPageUrl = DestinationPageUrl;
				if (!string.IsNullOrEmpty(destinationPageUrl))
				{
					return ResolveClientUrl(destinationPageUrl);
				}
				return FormsAuthentication.DefaultUrl;
			}
			string destinationPageUrl2 = DestinationPageUrl;
			if (!string.IsNullOrEmpty(destinationPageUrl2))
			{
				return ResolveClientUrl(destinationPageUrl2);
			}
			if (Page.Form != null && string.Equals(Page.Form.Method, "get", StringComparison.OrdinalIgnoreCase))
			{
				return Page.Request.Path;
			}
			return Page.Request.RawUrl;
		}

		protected override void SetDesignModeState(IDictionary data)
		{
			if (data != null)
			{
				object obj = data["ConvertToTemplate"];
				if (obj != null)
				{
					_convertingToTemplate = (bool)obj;
				}
				obj = data["RegionEditing"];
				if (obj != null)
				{
					_renderDesignerRegion = (bool)obj;
				}
			}
		}

		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(savedState);
				return;
			}
			object[] array = (object[])savedState;
			base.LoadViewState(array[0]);
			if (array[1] != null)
			{
				((IStateManager)SignInButtonStyle).LoadViewState(array[1]);
			}
			if (array[2] != null)
			{
				((IStateManager)TitleTextStyle).LoadViewState(array[3]);
			}
			if (array[3] != null)
			{
				((IStateManager)ErrorTextStyle).LoadViewState(array[4]);
			}
			if (array[4] != null)
			{
				((IStateManager)CheckBoxStyle).LoadViewState(array[5]);
			}
		}

		protected override object SaveViewState()
		{
			object[] array = new object[5]
			{
				base.SaveViewState(),
				(_signInButtonStyle != null) ? ((IStateManager)_signInButtonStyle).SaveViewState() : null,
				(_titleTextStyle != null) ? ((IStateManager)_titleTextStyle).SaveViewState() : null,
				(_errorTextStyle != null) ? ((IStateManager)_errorTextStyle).SaveViewState() : null,
				(_checkBoxStyle != null) ? ((IStateManager)_checkBoxStyle).SaveViewState() : null
			};
			for (int i = 0; i < 5; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (_signInButtonStyle != null)
			{
				((IStateManager)_signInButtonStyle).TrackViewState();
			}
			if (_titleTextStyle != null)
			{
				((IStateManager)_titleTextStyle).TrackViewState();
			}
			if (_errorTextStyle != null)
			{
				((IStateManager)_errorTextStyle).TrackViewState();
			}
			if (_checkBoxStyle != null)
			{
				((IStateManager)_checkBoxStyle).TrackViewState();
			}
		}

		private bool RedirectedFromFailedLogin()
		{
			if (!base.DesignMode && Page != null)
			{
				return !Page.IsPostBack && Page.Request.QueryString["signinfailure"] != null;
			}
			return false;
		}
	}
}
