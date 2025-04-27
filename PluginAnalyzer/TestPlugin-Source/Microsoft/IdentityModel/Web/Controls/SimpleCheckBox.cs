using System;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.IdentityModel.Web.Controls
{
	[DefaultProperty("Text")]
	[ControlValueProperty("Checked")]
	internal class SimpleCheckBox : WebControl, ICheckBoxControl
	{
		[DefaultValue(false)]
		[Themeable(false)]
		public virtual bool Checked
		{
			get
			{
				return (bool)(ViewState["Checked"] ?? ((object)false));
			}
			set
			{
				ViewState["Checked"] = value;
			}
		}

		[DefaultValue("")]
		[Bindable(true)]
		[Localizable(true)]
		[WebCategory("Category_Appearance")]
		public virtual string Text
		{
			get
			{
				return ((string)ViewState["Text"]) ?? string.Empty;
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		[WebCategory("Category_Action")]
		public event EventHandler CheckedChanged
		{
			add
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID5008")));
			}
			remove
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID5008")));
			}
		}

		public SimpleCheckBox()
			: base(HtmlTextWriterTag.Input)
		{
		}

		protected override void Render(HtmlTextWriter writer)
		{
			bool flag = false;
			if (base.ControlStyleCreated)
			{
				Style style = base.ControlStyle;
				if (!style.IsEmpty)
				{
					style.AddAttributesToRender(writer, this);
					flag = true;
				}
			}
			if (!base.IsEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
				flag = true;
			}
			string toolTip = ToolTip;
			if (toolTip.Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Title, toolTip);
				flag = true;
			}
			string text = null;
			if (base.HasAttributes)
			{
				System.Web.UI.AttributeCollection attributes = base.Attributes;
				string text2 = attributes["value"];
				if (text2 != null)
				{
					attributes.Remove("value");
				}
				text = attributes["onclick"];
				if (text != null)
				{
					text = ControlUtil.EnsureEndWithSemiColon(text);
					attributes.Remove("onclick");
				}
				if (attributes.Count != 0)
				{
					attributes.AddAttributes(writer);
					flag = true;
				}
				if (text2 != null)
				{
					attributes["value"] = text2;
				}
			}
			if (flag)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Span);
			}
			string text3 = Text;
			string clientID = ClientID;
			if (text3.Length != 0)
			{
				RenderInputTag(writer, clientID, text);
				RenderLabel(writer, text3, clientID);
			}
			else
			{
				RenderInputTag(writer, clientID, text);
			}
			if (flag)
			{
				writer.RenderEndTag();
			}
		}

		private void RenderLabel(HtmlTextWriter writer, string text, string clientID)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.For, HttpUtility.HtmlEncode(clientID));
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			HttpUtility.HtmlEncode(text, writer);
			writer.RenderEndTag();
		}

		private void RenderInputTag(HtmlTextWriter writer, string clientID, string onClick)
		{
			if (clientID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, HttpUtility.HtmlEncode(clientID));
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
			if (UniqueID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
			}
			if (Checked)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			}
			if (!base.IsEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			if (onClick != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Onclick, HttpUtility.HtmlEncode(onClick));
			}
			if (AccessKey.Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, HttpUtility.HtmlEncode(AccessKey));
			}
			if (TabIndex != 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, TabIndex.ToString(NumberFormatInfo.InvariantInfo));
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
		}
	}
}
