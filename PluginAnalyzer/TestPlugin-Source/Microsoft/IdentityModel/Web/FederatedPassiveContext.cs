using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
	internal class FederatedPassiveContext
	{
		private const string ControlIdKey = "id";

		private const string ReturnUrlKey = "ru";

		private const string SignInContextKey = "cx";

		private const string RememberMeKey = "rm";

		private string _wctx;

		private string _controlId;

		private string _signInContext;

		private string _returnUrl;

		private bool _rememberMe;

		public string ControlId
		{
			get
			{
				Initialize();
				return _controlId;
			}
		}

		public string SignInContext
		{
			get
			{
				Initialize();
				return _signInContext;
			}
		}

		public string ReturnUrl
		{
			get
			{
				Initialize();
				return _returnUrl;
			}
		}

		public bool RememberMe
		{
			get
			{
				Initialize();
				return _rememberMe;
			}
		}

		public string WCtx
		{
			get
			{
				Initialize();
				return _wctx;
			}
		}

		public FederatedPassiveContext(string controlId, string signInContext, string returnUrl, bool rememberMe)
		{
			_controlId = controlId;
			_signInContext = signInContext;
			_returnUrl = returnUrl;
			_rememberMe = rememberMe;
		}

		public FederatedPassiveContext(string wctx)
		{
			_wctx = wctx;
		}

		private void Initialize()
		{
			if (_wctx == null)
			{
				StringBuilder stringBuilder = new StringBuilder(128);
				stringBuilder.Append("rm");
				stringBuilder.Append('=');
				stringBuilder.Append(_rememberMe ? '1' : '0');
				stringBuilder.Append('&');
				stringBuilder.Append("id");
				stringBuilder.Append('=');
				stringBuilder.Append(HttpUtility.UrlEncode(_controlId));
				if (!string.IsNullOrEmpty(_signInContext))
				{
					stringBuilder.Append('&');
					stringBuilder.Append("cx");
					stringBuilder.Append('=');
					stringBuilder.Append(HttpUtility.UrlEncode(_signInContext));
				}
				if (!string.IsNullOrEmpty(_returnUrl))
				{
					stringBuilder.Append('&');
					stringBuilder.Append("ru");
					stringBuilder.Append('=');
					stringBuilder.Append(HttpUtility.UrlEncode(_returnUrl));
				}
				_wctx = stringBuilder.ToString();
			}
			else
			{
				if (_controlId != null)
				{
					return;
				}
				_controlId = string.Empty;
				NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(_wctx);
				foreach (string key in nameValueCollection.Keys)
				{
					switch (key)
					{
					case "rm":
						_rememberMe = nameValueCollection[key] != "0";
						break;
					case "id":
						_controlId = nameValueCollection[key];
						break;
					case "cx":
						_signInContext = nameValueCollection[key];
						break;
					case "ru":
						_returnUrl = nameValueCollection[key];
						break;
					}
				}
			}
		}
	}
}
