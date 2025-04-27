using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class SecurityTokenHandlerConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("audienceUris", IsRequired = false)]
		public AudienceUriElementCollection AudienceUriElements
		{
			get
			{
				return (AudienceUriElementCollection)base["audienceUris"];
			}
			set
			{
				base["audienceUris"] = value;
			}
		}

		[ConfigurationProperty("certificateValidation", IsRequired = false)]
		public X509CertificateValidationCustomTypeElement CertificateValidationElement
		{
			get
			{
				return (X509CertificateValidationCustomTypeElement)base["certificateValidation"];
			}
			set
			{
				base["certificateValidation"] = value;
			}
		}

		[ConfigurationProperty("issuerNameRegistry", IsRequired = false)]
		public CustomTypeElement IssuerNameRegistry
		{
			get
			{
				return (CustomTypeElement)base["issuerNameRegistry"];
			}
			set
			{
				base["issuerNameRegistry"] = value;
			}
		}

		[ConfigurationProperty("issuerTokenResolver", IsRequired = false)]
		public CustomTypeElement IssuerTokenResolver
		{
			get
			{
				return (CustomTypeElement)base["issuerTokenResolver"];
			}
			set
			{
				base["issuerTokenResolver"] = value;
			}
		}

		[ConfigurationProperty("name", IsRequired = false, Options = ConfigurationPropertyOptions.IsKey)]
		public string Name
		{
			get
			{
				return (string)base["name"];
			}
			set
			{
				base["name"] = value;
			}
		}

		[ConfigurationProperty("saveBootstrapTokens", IsRequired = false)]
		public string SaveBootstrapTokens
		{
			get
			{
				return (string)base["saveBootstrapTokens"];
			}
			set
			{
				base["saveBootstrapTokens"] = value;
			}
		}

		[ConfigurationProperty("maximumClockSkew", IsRequired = false)]
		public ValueTypeElement MaximumClockSkew
		{
			get
			{
				return (ValueTypeElement)base["maximumClockSkew"];
			}
			set
			{
				base["maximumClockSkew"] = value;
			}
		}

		[ConfigurationProperty("serviceTokenResolver", IsRequired = false)]
		public CustomTypeElement ServiceTokenResolver
		{
			get
			{
				return (CustomTypeElement)base["serviceTokenResolver"];
			}
			set
			{
				base["serviceTokenResolver"] = value;
			}
		}

		[ConfigurationProperty("tokenReplayDetection", IsRequired = false)]
		public TokenReplayDetectionElement TokenReplayDetectionElement
		{
			get
			{
				return (TokenReplayDetectionElement)base["tokenReplayDetection"];
			}
			set
			{
				base["tokenReplayDetection"] = value;
			}
		}

		public bool IsConfigured
		{
			get
			{
				if (!AudienceUriElements.IsConfigured && !CertificateValidationElement.IsConfigured && !IssuerNameRegistry.IsConfigured && !IssuerTokenResolver.IsConfigured && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(SaveBootstrapTokens) && !MaximumClockSkew.IsConfigured && !ServiceTokenResolver.IsConfigured)
				{
					return TokenReplayDetectionElement.IsConfigured;
				}
				return true;
			}
		}

		protected override void Init()
		{
			Name = "";
		}
	}
}
