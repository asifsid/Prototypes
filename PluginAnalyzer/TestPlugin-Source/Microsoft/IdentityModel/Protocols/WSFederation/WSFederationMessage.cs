using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[ComVisible(true)]
	public abstract class WSFederationMessage
	{
		private Dictionary<string, string> _parameters = new Dictionary<string, string>();

		private Uri _baseUri;

		public IDictionary<string, string> Parameters => _parameters;

		public string Context
		{
			get
			{
				return GetParameter("wctx");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wctx");
				}
				else
				{
					SetParameter("wctx", value);
				}
			}
		}

		public string Encoding
		{
			get
			{
				return GetParameter("wencoding");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					RemoveParameter("wencoding");
				}
				else
				{
					SetParameter("wencoding", value);
				}
			}
		}

		public string Action
		{
			get
			{
				return GetParameter("wa");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "value"));
				}
				SetParameter("wa", value);
			}
		}

		public Uri BaseUri
		{
			get
			{
				return _baseUri;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				if (!UriUtil.CanCreateValidUri(value.AbsoluteUri, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3003")));
				}
				_baseUri = value;
			}
		}

		protected WSFederationMessage(Uri baseUrl, string action)
		{
			if (baseUrl == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("baseUrl");
			}
			if (!UriUtil.CanCreateValidUri(baseUrl.AbsoluteUri, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3003")));
			}
			if (string.IsNullOrEmpty(action))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "action"));
			}
			_parameters["wa"] = action;
			_baseUri = baseUrl;
		}

		public static WSFederationMessage CreateFromUri(Uri requestUri)
		{
			if (requestUri == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestUri");
			}
			if (TryCreateFromUri(requestUri, out var fedMsg))
			{
				return fedMsg;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3094", requestUri)));
		}

		public static bool TryCreateFromUri(Uri requestUri, out WSFederationMessage fedMsg)
		{
			if (requestUri == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestUri");
			}
			Uri baseUrl = GetBaseUrl(requestUri);
			fedMsg = CreateFromNameValueCollection(baseUrl, ParseQueryString(requestUri));
			return fedMsg != null;
		}

		public static WSFederationMessage CreateFromFormPost(HttpRequest request)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			Uri baseUrl = GetBaseUrl(request.Url);
			WSFederationMessage wSFederationMessage = CreateFromNameValueCollection(baseUrl, request.Form);
			if (wSFederationMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3095")));
			}
			return wSFederationMessage;
		}

		protected virtual void Validate()
		{
			if (_baseUri == null || !UriUtil.CanCreateValidUri(_baseUri.AbsoluteUri, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3003")));
			}
		}

		public abstract void Write(TextWriter writer);

		public static WSFederationMessage CreateFromNameValueCollection(Uri baseUrl, NameValueCollection collection)
		{
			if (baseUrl == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("baseUrl");
			}
			if (collection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("collection");
			}
			WSFederationMessage wSFederationMessage = null;
			string text = collection.Get("wa");
			if (!string.IsNullOrEmpty(text))
			{
				switch (text)
				{
				case "wattr1.0":
					wSFederationMessage = new AttributeRequestMessage(baseUrl);
					break;
				case "wpseudo1.0":
					wSFederationMessage = new PseudonymRequestMessage(baseUrl);
					break;
				case "wsignin1.0":
				{
					string text2 = collection.Get("wresult");
					string text3 = collection.Get("wresultptr");
					bool flag = !string.IsNullOrEmpty(text2);
					bool flag2 = !string.IsNullOrEmpty(text3);
					if (flag)
					{
						wSFederationMessage = new SignInResponseMessage(baseUrl, text2);
						break;
					}
					if (flag2)
					{
						wSFederationMessage = new SignInResponseMessage(baseUrl, new Uri(text3));
						break;
					}
					string realm = collection.Get("wtrealm");
					string reply = collection.Get("wreply");
					wSFederationMessage = new SignInRequestMessage(baseUrl, realm, reply);
					break;
				}
				case "wsignout1.0":
					wSFederationMessage = new SignOutRequestMessage(baseUrl);
					break;
				case "wsignoutcleanup1.0":
					wSFederationMessage = new SignOutCleanupRequestMessage(baseUrl);
					break;
				default:
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3014", text)));
				}
			}
			if (wSFederationMessage != null)
			{
				PopulateMessage(wSFederationMessage, collection);
				wSFederationMessage.Validate();
			}
			return wSFederationMessage;
		}

		private static void PopulateMessage(WSFederationMessage message, NameValueCollection collection)
		{
			foreach (string key in collection.Keys)
			{
				if (string.IsNullOrEmpty(collection[key]))
				{
					switch (key)
					{
					case "wtrealm":
					case "wresult":
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSFederationMessageException(SR.GetString("ID3261", key)));
					}
				}
				else
				{
					message.SetParameter(key, collection[key]);
				}
			}
		}

		public static NameValueCollection ParseQueryString(Uri data)
		{
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			return HttpUtility.ParseQueryString(data.Query);
		}

		public string GetParameter(string parameter)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "parameter"));
			}
			string value = null;
			_parameters.TryGetValue(parameter, out value);
			return value;
		}

		public void SetParameter(string parameter, string value)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "parameter"));
			}
			if (string.IsNullOrEmpty(value))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "value"));
			}
			_parameters[parameter] = value;
		}

		public void SetUriParameter(string parameter, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "value"));
			}
			if (!UriUtil.CanCreateValidUri(value, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0013"), "value"));
			}
			SetParameter(parameter, value);
		}

		public void RemoveParameter(string parameter)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "parameter"));
			}
			if (Parameters.Keys.Contains(parameter))
			{
				_parameters.Remove(parameter);
			}
		}

		public static Uri GetBaseUrl(Uri uri)
		{
			if (uri == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("uri");
			}
			string text = uri.AbsoluteUri;
			int num = text.IndexOf("?", 0, StringComparison.Ordinal);
			if (num > -1)
			{
				text = text.Substring(0, num);
			}
			return new Uri(text);
		}

		public virtual string WriteQueryString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(_baseUri.AbsoluteUri);
			stringBuilder.Append("?");
			bool flag = true;
			foreach (KeyValuePair<string, string> parameter in _parameters)
			{
				if (!flag)
				{
					stringBuilder.Append("&");
				}
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}={1}", new object[2]
				{
					HttpUtility.UrlEncode(parameter.Key),
					HttpUtility.UrlEncode(parameter.Value)
				}));
				flag = false;
			}
			return stringBuilder.ToString();
		}

		public virtual string WriteFormPost()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<html><head><title>{0}</title></head><body><form method=\"POST\" name=\"hiddenform\" action=\"{1}\">", new object[2]
			{
				SR.GetString("HtmlPostTitle"),
				HttpUtility.HtmlAttributeEncode(_baseUri.AbsoluteUri)
			}));
			foreach (KeyValuePair<string, string> parameter in _parameters)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", new object[2]
				{
					HttpUtility.HtmlAttributeEncode(parameter.Key),
					HttpUtility.HtmlAttributeEncode(parameter.Value)
				}));
			}
			stringBuilder.Append("<noscript><p>");
			stringBuilder.Append(SR.GetString("HtmlPostNoScriptMessage"));
			stringBuilder.Append("</p><input type=\"submit\" value=\"");
			stringBuilder.Append(SR.GetString("HtmlPostNoScriptButtonText"));
			stringBuilder.Append("\" /></noscript>");
			stringBuilder.Append("</form><script language=\"javascript\">window.setTimeout('document.forms[0].submit()', 0);</script></body></html>");
			return stringBuilder.ToString();
		}
	}
}
