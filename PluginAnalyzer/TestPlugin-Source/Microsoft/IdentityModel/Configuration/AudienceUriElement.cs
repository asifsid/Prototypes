using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class AudienceUriElement : ConfigurationElement
	{
		private const string DefaultValue = " ";

		[ConfigurationProperty("value", IsRequired = true, DefaultValue = " ", IsKey = true)]
		[StringValidator(MinLength = 1)]
		public string Value
		{
			get
			{
				return (string)base["value"];
			}
			set
			{
				base["value"] = value;
			}
		}

		public bool IsConfigured => !string.Equals(Value, " ", StringComparison.Ordinal);
	}
}
