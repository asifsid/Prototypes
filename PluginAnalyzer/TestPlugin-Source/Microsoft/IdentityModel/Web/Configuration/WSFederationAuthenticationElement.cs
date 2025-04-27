using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Configuration
{
	[ComVisible(true)]
	public class WSFederationAuthenticationElement : ConfigurationElement
	{
		private const bool DefaultPassiveRedirectEnabled = true;

		private const bool DefaultPersistentCookiesOnPassiveRedirects = false;

		private const bool DefaultRequireHttps = true;

		[ConfigurationProperty("authenticationType", IsRequired = false)]
		public string AuthenticationType => (string)base["authenticationType"];

		[ConfigurationProperty("freshness", IsRequired = false)]
		public string Freshness => (string)base["freshness"];

		[ConfigurationProperty("homeRealm", IsRequired = false)]
		public string HomeRealm => (string)base["homeRealm"];

		[ConfigurationProperty("issuer", IsRequired = true)]
		public string Issuer => (string)base["issuer"];

		[ConfigurationProperty("policy", IsRequired = false)]
		public string Policy => (string)base["policy"];

		[ConfigurationProperty("realm", IsRequired = true)]
		public string Realm => (string)base["realm"];

		[ConfigurationProperty("reply", IsRequired = false)]
		public string Reply => (string)base["reply"];

		[ConfigurationProperty("request", IsRequired = false)]
		public string Request => (string)base["request"];

		[ConfigurationProperty("requestPtr", IsRequired = false)]
		public string RequestPtr => (string)base["requestPtr"];

		[ConfigurationProperty("resource", IsRequired = false)]
		public string Resource => (string)base["resource"];

		[ConfigurationProperty("signInQueryString", IsRequired = false)]
		public string SignInQueryString => (string)base["signInQueryString"];

		[ConfigurationProperty("signOutQueryString", IsRequired = false)]
		public string SignOutQueryString => (string)base["signOutQueryString"];

		[ConfigurationProperty("signOutReply", IsRequired = false)]
		public string SignOutReply => (string)base["signOutReply"];

		[ConfigurationProperty("passiveRedirectEnabled", DefaultValue = true, IsRequired = false)]
		public bool PassiveRedirectEnabled => (bool)base["passiveRedirectEnabled"];

		[ConfigurationProperty("persistentCookiesOnPassiveRedirects", DefaultValue = false, IsRequired = false)]
		public bool PersistentCookiesOnPassiveRedirects => (bool)base["persistentCookiesOnPassiveRedirects"];

		[ConfigurationProperty("requireHttps", DefaultValue = true, IsRequired = false)]
		public bool RequireHttps => (bool)base["requireHttps"];

		public bool IsConfigured
		{
			get
			{
				if (string.IsNullOrEmpty(AuthenticationType) && string.IsNullOrEmpty(Freshness) && string.IsNullOrEmpty(HomeRealm) && string.IsNullOrEmpty(Issuer) && string.IsNullOrEmpty(Policy) && string.IsNullOrEmpty(Realm) && string.IsNullOrEmpty(Reply) && string.IsNullOrEmpty(Request) && string.IsNullOrEmpty(RequestPtr) && string.IsNullOrEmpty(Resource) && string.IsNullOrEmpty(SignInQueryString) && string.IsNullOrEmpty(SignOutQueryString) && string.IsNullOrEmpty(SignOutReply) && PassiveRedirectEnabled && !PersistentCookiesOnPassiveRedirects)
				{
					return !RequireHttps;
				}
				return true;
			}
		}

		internal void Verify()
		{
			if (!IsConfigured)
			{
				return;
			}
			if (string.IsNullOrEmpty(Issuer))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(this, "issuer", SR.GetString("ID1045"));
			}
			if (RequireHttps && UriUtil.TryCreateValidUri(Issuer, UriKind.Absolute, out var result) && result.Scheme != Uri.UriSchemeHttps)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(this, "issuer", SR.GetString("ID1056"));
			}
			if (string.IsNullOrEmpty(Realm))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(this, "realm", SR.GetString("ID1046"));
			}
			if (!string.IsNullOrEmpty(Freshness))
			{
				double result2 = -1.0;
				if (!double.TryParse(Freshness, out result2) || !(result2 >= 0.0))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(this, "freshness", SR.GetString("ID1050"));
				}
			}
		}
	}
}
