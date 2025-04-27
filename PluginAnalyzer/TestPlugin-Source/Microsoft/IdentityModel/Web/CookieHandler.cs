using System;
using System.Runtime.InteropServices;
using System.Web;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public abstract class CookieHandler
	{
		private const string DefaultCookieName = "FedAuth";

		private string _domain;

		private bool _hideFromClientScript = true;

		private string _name = "FedAuth";

		private string _path = DefaultCookiePath;

		private bool _requireSsl = true;

		private TimeSpan? _persistentSessionLifetime = null;

		private static string DefaultCookiePath
		{
			get
			{
				string stringToEscape = HttpRuntime.AppDomainAppVirtualPath ?? string.Empty;
				stringToEscape = Uri.EscapeUriString(stringToEscape);
				if (!stringToEscape.EndsWith("/", StringComparison.Ordinal))
				{
					return stringToEscape + "/";
				}
				return stringToEscape;
			}
		}

		public string Domain
		{
			get
			{
				return _domain;
			}
			set
			{
				_domain = value;
			}
		}

		public bool HideFromClientScript
		{
			get
			{
				return _hideFromClientScript;
			}
			set
			{
				_hideFromClientScript = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				_name = value;
			}
		}

		public string Path
		{
			get
			{
				return _path;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				_path = value;
			}
		}

		public TimeSpan? PersistentSessionLifetime
		{
			get
			{
				return _persistentSessionLifetime;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", SR.GetString("ID1022"));
				}
				_persistentSessionLifetime = value;
			}
		}

		public bool RequireSsl
		{
			get
			{
				return _requireSsl;
			}
			set
			{
				_requireSsl = value;
			}
		}

		public void Delete()
		{
			Delete(HttpContext.Current);
		}

		public void Delete(string name)
		{
			Delete(name, HttpContext.Current);
		}

		public void Delete(HttpContext context)
		{
			Delete(_name, context);
		}

		public void Delete(string name, HttpContext context)
		{
			Delete(name, _path, _domain, context);
		}

		public void Delete(string name, string path, string domain, HttpContext context)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("name");
			}
			DeleteCore(name, path, domain, context);
		}

		protected abstract void DeleteCore(string name, string path, string domain, HttpContext context);

		public byte[] Read()
		{
			return Read(HttpContext.Current);
		}

		public byte[] Read(string name)
		{
			return Read(name, HttpContext.Current);
		}

		public byte[] Read(HttpContext context)
		{
			return Read(_name, context);
		}

		public byte[] Read(string name, HttpContext context)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("name");
			}
			return ReadCore(name, context);
		}

		protected abstract byte[] ReadCore(string name, HttpContext context);

		public void Write(byte[] value, bool isPersistent, DateTime tokenExpirationTime)
		{
			Write(expirationTime: (!isPersistent) ? DateTime.MinValue : ((!_persistentSessionLifetime.HasValue) ? tokenExpirationTime : (DateTime.UtcNow + _persistentSessionLifetime.Value)), value: value, name: _name, context: HttpContext.Current);
		}

		public void Write(byte[] value, string name, DateTime expirationTime)
		{
			Write(value, name, expirationTime, HttpContext.Current);
		}

		public void Write(byte[] value, string name, DateTime expirationTime, HttpContext context)
		{
			Write(value, name, _path, _domain, expirationTime, _requireSsl, _hideFromClientScript, context);
		}

		public void Write(byte[] value, string name, string path, string domain, DateTime expirationTime, bool requiresSsl, bool hideFromClientScript, HttpContext context)
		{
			if (value == null || value.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("name");
			}
			WriteCore(value, name, path, domain, expirationTime, requiresSsl, hideFromClientScript, context);
		}

		protected abstract void WriteCore(byte[] value, string name, string path, string domain, DateTime expirationTime, bool secure, bool httpOnly, HttpContext context);

		internal static string MatchCookiePath(string targetUrl)
		{
			if (!string.IsNullOrEmpty(targetUrl) && Uri.TryCreate(targetUrl, UriKind.RelativeOrAbsolute, out var result))
			{
				SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
				if (sessionAuthenticationModule != null && sessionAuthenticationModule.CookieHandler != null)
				{
					return sessionAuthenticationModule.CookieHandler.MatchCookiePath(HttpContext.Current.Request.Url, result);
				}
			}
			return targetUrl;
		}

		internal string MatchCookiePath(Uri baseUri, Uri targetUri)
		{
			Uri uri = (targetUri.IsAbsoluteUri ? targetUri : new Uri(baseUri, targetUri));
			string host = uri.Host;
			string pathAndQuery = uri.PathAndQuery;
			string text = _path;
			string text2 = _domain;
			if (text2 == null)
			{
				text2 = host;
			}
			else if (!text2.Equals(host, StringComparison.OrdinalIgnoreCase) && !text2.StartsWith(".", StringComparison.OrdinalIgnoreCase))
			{
				text2 = "." + text2;
			}
			if (!text.EndsWith("/", StringComparison.Ordinal))
			{
				text += "/";
			}
			if (host.EndsWith(text2, StringComparison.OrdinalIgnoreCase) && pathAndQuery.StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				UriBuilder uriBuilder = new UriBuilder(uri);
				if (text.Length < uriBuilder.Path.Length)
				{
					uriBuilder.Path = text + uriBuilder.Path.Substring(text.Length);
				}
				else
				{
					uriBuilder.Path = text;
				}
				if (targetUri.IsAbsoluteUri)
				{
					return uriBuilder.Uri.AbsoluteUri;
				}
				return uriBuilder.Path + uriBuilder.Fragment + uriBuilder.Query;
			}
			return targetUri.OriginalString;
		}
	}
}
