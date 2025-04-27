using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Microsoft.IdentityModel.Protocols.WSFederation;

namespace Microsoft.IdentityModel.Web.Controls
{
	internal static class ControlUtil
	{
		internal class DisappearingTableRow : TableRow
		{
			protected override void Render(HtmlTextWriter writer)
			{
				bool flag = false;
				foreach (TableCell cell in Cells)
				{
					if (cell.Visible)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					base.Render(writer);
				}
			}
		}

		private class ChildTable : Table
		{
			private int _parentLevel;

			internal ChildTable()
				: this(1)
			{
			}

			internal ChildTable(int parentLevel)
			{
				_parentLevel = parentLevel;
			}

			protected override void AddAttributesToRender(HtmlTextWriter writer)
			{
				base.AddAttributesToRender(writer);
				string parentID = GetParentID();
				if (parentID != null)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Id, parentID);
				}
			}

			private string GetParentID()
			{
				if (ID != null)
				{
					return null;
				}
				System.Web.UI.Control control = this;
				for (int i = 0; i < _parentLevel; i++)
				{
					control = control.Parent;
					if (control == null)
					{
						break;
					}
				}
				if (control != null)
				{
					string iD = control.ID;
					if (!string.IsNullOrEmpty(iD))
					{
						return control.ClientID;
					}
				}
				return null;
			}
		}

		public const string ReturnUrl = "ReturnUrl";

		public static bool IsHttps(Uri url)
		{
			if (url.IsAbsoluteUri)
			{
				return StringComparer.OrdinalIgnoreCase.Equals(url.Scheme, Uri.UriSchemeHttps);
			}
			return false;
		}

		public static bool IsHttps(string urlPath)
		{
			if (UriUtil.TryCreateValidUri(urlPath, UriKind.Absolute, out var result))
			{
				return IsHttps(result);
			}
			return false;
		}

		public static void EnsureSessionAuthenticationModule()
		{
			if (FederatedAuthentication.SessionAuthenticationModule == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1060")));
			}
		}

		public static bool OnLoginPage(HttpContext context)
		{
			string text = FormsAuthentication.LoginUrl;
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			int num = text.IndexOf('?');
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			text = GetCompleteLoginUrl(text);
			string path = context.Request.Path;
			if (StringComparer.OrdinalIgnoreCase.Equals(path, text))
			{
				return true;
			}
			if (text.IndexOf('%') >= 0)
			{
				string y = HttpUtility.UrlDecode(text);
				if (StringComparer.OrdinalIgnoreCase.Equals(path, y))
				{
					return true;
				}
				y = HttpUtility.UrlDecode(text, context.Request.ContentEncoding);
				if (StringComparer.OrdinalIgnoreCase.Equals(path, y))
				{
					return true;
				}
			}
			return false;
		}

		public static string GetPathAndQuery(WSFederationMessage request)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			using StringWriter writer = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			request.Write(writer);
			return stringBuilder.ToString();
		}

		public static string GetCompleteLoginUrl(string loginUrl)
		{
			if (string.IsNullOrEmpty(loginUrl))
			{
				return string.Empty;
			}
			if (VirtualPathUtility.IsAppRelative(loginUrl))
			{
				loginUrl = VirtualPathUtility.ToAbsolute(loginUrl);
			}
			return loginUrl;
		}

		public static string EnsureEndWithSemiColon(string value)
		{
			if (value != null)
			{
				int length = value.Length;
				if (length > 0 && value[length - 1] != ';')
				{
					return value + ";";
				}
			}
			return value;
		}

		public static bool IsAppRelative(string path)
		{
			HttpRequest request = HttpContext.Current.Request;
			string appDomainAppVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
			UriBuilder uriBuilder = new UriBuilder(request.Url.Scheme, request.Url.Host, request.Url.Port, appDomainAppVirtualPath.EndsWith("/", StringComparison.Ordinal) ? appDomainAppVirtualPath : (appDomainAppVirtualPath + "/"));
			return IsAppRelative(uriBuilder.Uri, path);
		}

		public static bool IsAppRelative(Uri basePath, string path)
		{
			if (UriUtil.TryCreateValidUri(path, UriKind.RelativeOrAbsolute, out var result))
			{
				if (result.IsAbsoluteUri)
				{
					UriBuilder uriBuilder = new UriBuilder(result);
					uriBuilder.Scheme = basePath.Scheme;
					uriBuilder.Port = basePath.Port;
					result = uriBuilder.Uri;
				}
				else
				{
					result = new Uri(basePath, result);
				}
				return result.AbsoluteUri.StartsWith(basePath.AbsoluteUri, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		public static bool IsDangerousUrl(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return false;
			}
			if (UriUtil.TryCreateValidUri(s, UriKind.RelativeOrAbsolute, out var result))
			{
				if (result.IsAbsoluteUri)
				{
					if (!(result.Scheme == Uri.UriSchemeHttp))
					{
						return !(result.Scheme == Uri.UriSchemeHttps);
					}
					return false;
				}
				return false;
			}
			return true;
		}

		public static void CopyBaseAttributesToInnerControl(WebControl control, WebControl child)
		{
			short tabIndex = control.TabIndex;
			string accessKey = control.AccessKey;
			try
			{
				control.AccessKey = string.Empty;
				control.TabIndex = 0;
				child.CopyBaseAttributes(control);
			}
			finally
			{
				control.TabIndex = tabIndex;
				control.AccessKey = accessKey;
			}
		}

		public static void SetTableCellStyle(System.Web.UI.Control control, Style style)
		{
			System.Web.UI.Control parent = control.Parent;
			if (parent != null)
			{
				((TableCell)parent).ApplyStyle(style);
			}
		}

		public static void SetTableCellVisible(System.Web.UI.Control control, bool visible)
		{
			System.Web.UI.Control parent = control.Parent;
			if (parent != null)
			{
				parent.Visible = visible;
			}
		}

		public static void CopyBorderStyles(WebControl control, Style style)
		{
			if (style != null && !style.IsEmpty)
			{
				control.BorderStyle = style.BorderStyle;
				control.BorderColor = style.BorderColor;
				control.BorderWidth = style.BorderWidth;
				control.BackColor = style.BackColor;
				control.CssClass = style.CssClass;
			}
		}

		public static void CopyStyleToInnerControl(WebControl control, Style style)
		{
			if (style != null && !style.IsEmpty)
			{
				control.ForeColor = style.ForeColor;
				control.Font.CopyFrom(style.Font);
			}
		}

		public static Table CreateChildTable(bool convertingToTemplate)
		{
			if (convertingToTemplate)
			{
				return new Table();
			}
			return new ChildTable(2);
		}

		public static bool EnsureCrossAppRedirect(string toUri, HttpContext context, bool throwIfFail)
		{
			if (!string.IsNullOrEmpty(toUri) && !FormsAuthentication.EnableCrossAppRedirects && !IsPathOnSameServer(toUri, context.Request.Url))
			{
				if (throwIfFail)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5015", toUri)));
				}
				return false;
			}
			return true;
		}

		internal static bool IsPathOnSameServer(string absUriOrLocalPath, Uri currentRequestUri)
		{
			if (UriUtil.CanCreateValidUri(absUriOrLocalPath, UriKind.Relative))
			{
				return true;
			}
			if (!UriUtil.TryCreateValidUri(absUriOrLocalPath, UriKind.Absolute, out var result))
			{
				return false;
			}
			if (!result.IsLoopback)
			{
				return string.Equals(currentRequestUri.Host, result.Host, StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}

		public static string GetLoginPage(HttpContext context, string extraQueryString, bool reuseReturnUrl)
		{
			string text = FormsAuthentication.LoginUrl;
			if (text.IndexOf('?') >= 0)
			{
				text = RemoveQueryStringVariableFromUrl(text, "ReturnUrl");
			}
			int num = text.IndexOf('?');
			if (num < 0)
			{
				text += "?";
			}
			else if (num < text.Length - 1)
			{
				text += "&";
			}
			string text2 = null;
			if (reuseReturnUrl)
			{
				Encoding contentEncoding = context.Request.ContentEncoding;
				contentEncoding = (contentEncoding.Equals(Encoding.Unicode) ? Encoding.UTF8 : contentEncoding);
				text2 = HttpUtility.UrlEncode(GetReturnUrl(context, useDefaultIfAbsent: false), contentEncoding);
			}
			if (text2 == null)
			{
				text2 = HttpUtility.UrlEncode(context.Request.Url.PathAndQuery, context.Request.ContentEncoding);
			}
			text = text + "ReturnUrl=" + text2;
			if (!string.IsNullOrEmpty(extraQueryString))
			{
				text = text + "&" + extraQueryString;
			}
			return text;
		}

		public static string RemoveQueryStringVariableFromUrl(string strUrl, string QSVar)
		{
			int num = strUrl.IndexOf('?');
			if (num < 0)
			{
				return strUrl;
			}
			string text = "&";
			string text2 = "?";
			string token = text + QSVar + "=";
			RemoveQSVar(ref strUrl, num, token, text, text.Length);
			token = text2 + QSVar + "=";
			RemoveQSVar(ref strUrl, num, token, text, text2.Length);
			text = HttpUtility.UrlEncode("&");
			text2 = HttpUtility.UrlEncode("?");
			token = text + HttpUtility.UrlEncode(QSVar + "=");
			RemoveQSVar(ref strUrl, num, token, text, text.Length);
			token = text2 + HttpUtility.UrlEncode(QSVar + "=");
			RemoveQSVar(ref strUrl, num, token, text, text2.Length);
			return strUrl;
		}

		internal static string GetReturnUrl(HttpContext context, bool useDefaultIfAbsent)
		{
			string text = context.Request.QueryString["ReturnUrl"];
			if (text == null)
			{
				text = context.Request.Form["ReturnUrl"];
				if (!string.IsNullOrEmpty(text) && !text.Contains("/") && text.Contains("%"))
				{
					text = HttpUtility.UrlDecode(text);
				}
			}
			if (!EnsureCrossAppRedirect(text, context, throwIfFail: false))
			{
				text = null;
			}
			if (!string.IsNullOrEmpty(text) && IsDangerousUrl(text))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new HttpException(SR.GetString("ID5007")));
			}
			if (text != null || !useDefaultIfAbsent)
			{
				return text;
			}
			return FormsAuthentication.DefaultUrl;
		}

		public static void RemoveQSVar(ref string strUrl, int posQ, string token, string sep, int lenAtStartToLeave)
		{
			for (int num = strUrl.LastIndexOf(token, StringComparison.Ordinal); num >= posQ; num = strUrl.LastIndexOf(token, StringComparison.Ordinal))
			{
				int num2 = strUrl.IndexOf(sep, num + token.Length, StringComparison.Ordinal) + sep.Length;
				if (num2 < sep.Length || num2 >= strUrl.Length)
				{
					strUrl = strUrl.Substring(0, num);
				}
				else
				{
					strUrl = strUrl.Substring(0, num + lenAtStartToLeave) + strUrl.Substring(num2);
				}
			}
		}

		public static DialogResult MessageBoxError(string text, string caption)
		{
			MessageBoxOptions messageBoxOptions = (MessageBoxOptions)0;
			if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
			{
				messageBoxOptions |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			return MessageBox.Show(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, messageBoxOptions);
		}

		internal static void EnsureAutoSignInNotSetOnMultipleControls(Page page)
		{
			bool flag = false;
			foreach (System.Web.UI.Control control in page.Form.Controls)
			{
				SignInControl signInControl = control as SignInControl;
				if (signInControl != null && signInControl.AutoSignIn)
				{
					if (flag)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID5022")));
					}
					flag = true;
				}
			}
		}
	}
}
