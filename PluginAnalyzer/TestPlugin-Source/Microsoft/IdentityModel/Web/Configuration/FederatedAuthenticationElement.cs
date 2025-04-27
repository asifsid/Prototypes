using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web.Configuration
{
	[ComVisible(true)]
	public class FederatedAuthenticationElement : ConfigurationElement
	{
		[ConfigurationProperty("wsFederation", IsRequired = false)]
		public WSFederationAuthenticationElement WSFederation
		{
			get
			{
				return (WSFederationAuthenticationElement)base["wsFederation"];
			}
			set
			{
				base["wsFederation"] = value;
			}
		}

		[ConfigurationProperty("cookieHandler", IsRequired = false)]
		public CookieHandlerElement CookieHandler
		{
			get
			{
				return (CookieHandlerElement)base["cookieHandler"];
			}
			set
			{
				base["cookieHandler"] = value;
			}
		}

		public bool IsConfigured
		{
			get
			{
				if (!WSFederation.IsConfigured)
				{
					return CookieHandler.IsConfigured;
				}
				return true;
			}
		}
	}
}
