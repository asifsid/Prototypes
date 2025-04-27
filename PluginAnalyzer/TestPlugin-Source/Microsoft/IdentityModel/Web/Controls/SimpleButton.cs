using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
	[ParseChildren(false)]
	[DefaultProperty("Text")]
	[ControlBuilder(typeof(SimpleButtonControlBuilder))]
	internal class SimpleButton : WebControl
	{
		private Image _img;

		private Label _label;

		[WebDefaultValue("SignIn_DefaultSignInText")]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[Localizable(true)]
		[WebCategory("Category_Appearance")]
		[WebDescription("SignIn_SignInText")]
		public virtual string Text
		{
			get
			{
				return ((string)ViewState["Text"]) ?? SR.GetString("SignIn_DefaultSignInText");
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		[WebCategory("Category_Appearance")]
		[Editor(typeof(ImageUrlEditor), typeof(UITypeEditor))]
		[UrlProperty]
		[DefaultValue("")]
		[WebDescription("SignIn_SignInImageUrl")]
		public virtual string ImageUrl
		{
			get
			{
				return ((string)ViewState["ImageUrl"]) ?? string.Empty;
			}
			set
			{
				ViewState["ImageUrl"] = value;
			}
		}

		[Themeable(false)]
		[WebDescription("SimpleButton_OnClientClick")]
		[WebCategory("Category_Behavior")]
		[DefaultValue("")]
		public virtual string OnClientClick
		{
			get
			{
				return ((string)ViewState["OnClientClick"]) ?? string.Empty;
			}
			set
			{
				ViewState["OnClientClick"] = value;
			}
		}

		public SimpleButton()
			: base(HtmlTextWriterTag.Button)
		{
		}

		protected override void CreateChildControls()
		{
			Controls.Clear();
			_img = new Image();
			_img.ID = "signinimage";
			_img.ImageAlign = ImageAlign.AbsMiddle;
			_img.Style.Add(HtmlTextWriterStyle.MarginRight, "5px");
			Controls.Add(_img);
			_label = new Label();
			_label.ID = "signintext";
			Controls.Add(_label);
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			if (Enabled && !base.IsEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			if (base.IsEnabled && !string.IsNullOrEmpty(OnClientClick))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Onclick, ControlUtil.EnsureEndWithSemiColon(OnClientClick));
			}
			base.AddAttributesToRender(writer);
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState(savedState);
			if (savedState != null)
			{
				string text = (string)ViewState["Text"];
				if (text != null)
				{
					Text = text;
				}
			}
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (base.DesignMode)
			{
				EnsureChildControls();
			}
			_img.Visible = !string.IsNullOrEmpty(ImageUrl);
			if (_img.Visible)
			{
				_img.ImageUrl = ImageUrl;
			}
			_label.Visible = !string.IsNullOrEmpty(Text);
			if (_label.Visible)
			{
				_label.Text = Text;
			}
			base.RenderContents(writer);
		}
	}
}
