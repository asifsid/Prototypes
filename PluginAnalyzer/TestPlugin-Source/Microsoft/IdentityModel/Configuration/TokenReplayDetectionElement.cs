using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class TokenReplayDetectionElement : ConfigurationElement
	{
		[ConfigurationProperty("purgeInterval", IsRequired = false)]
		public string PurgeInterval
		{
			get
			{
				return (string)base["purgeInterval"];
			}
			set
			{
				base["purgeInterval"] = value;
			}
		}

		[ConfigurationProperty("capacity", IsRequired = false)]
		public string Capacity
		{
			get
			{
				return (string)base["capacity"];
			}
			set
			{
				base["capacity"] = value;
			}
		}

		[ConfigurationProperty("enabled", IsRequired = false)]
		public string Enabled
		{
			get
			{
				return (string)base["enabled"];
			}
			set
			{
				base["enabled"] = value;
			}
		}

		[ConfigurationProperty("expirationPeriod", IsRequired = false)]
		public string ExpirationPeriod
		{
			get
			{
				return (string)base["expirationPeriod"];
			}
			set
			{
				base["expirationPeriod"] = value;
			}
		}

		[ConfigurationProperty("replayCache", IsRequired = false)]
		public CustomTypeElement CustomType
		{
			get
			{
				return (CustomTypeElement)base["replayCache"];
			}
			set
			{
				base["replayCache"] = value;
			}
		}

		public bool IsConfigured
		{
			get
			{
				if (string.IsNullOrEmpty(PurgeInterval) && string.IsNullOrEmpty(Capacity) && string.IsNullOrEmpty(Enabled) && string.IsNullOrEmpty(ExpirationPeriod))
				{
					return CustomType.IsConfigured;
				}
				return true;
			}
		}
	}
}
