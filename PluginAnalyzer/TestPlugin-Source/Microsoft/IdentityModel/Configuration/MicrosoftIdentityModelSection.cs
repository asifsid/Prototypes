using System.Configuration;
using System.Runtime.InteropServices;
using System.Web.Configuration;
using System.Web.Hosting;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class MicrosoftIdentityModelSection : ConfigurationSection
	{
		public const string SectionName = "microsoft.identityModel";

		public static MicrosoftIdentityModelSection Current
		{
			get
			{
				if (HostingEnvironment.IsHosted)
				{
					if (HostingEnvironment.ApplicationVirtualPath != null)
					{
						return WebConfigurationManager.GetSection("microsoft.identityModel", HostingEnvironment.ApplicationVirtualPath) as MicrosoftIdentityModelSection;
					}
					return WebConfigurationManager.GetSection("microsoft.identityModel") as MicrosoftIdentityModelSection;
				}
				return ConfigurationManager.GetSection("microsoft.identityModel") as MicrosoftIdentityModelSection;
			}
		}

		public static ServiceElement DefaultServiceElement => Current?.ServiceElements.GetElement("");

		[ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public ServiceElementCollection ServiceElements => (ServiceElementCollection)base[""];

		public bool IsConfigured => ServiceElements.IsConfigured;
	}
}
