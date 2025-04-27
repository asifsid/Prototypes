using System.Configuration;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Web.Configuration;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class ServiceElement : ConfigurationElement
	{
		[ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey)]
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
		public X509CertificateValidationElement CertificateValidationElement
		{
			get
			{
				return (X509CertificateValidationElement)base["certificateValidation"];
			}
			set
			{
				base["certificateValidation"] = value;
			}
		}

		[ConfigurationProperty("claimsAuthenticationManager", IsRequired = false)]
		public CustomTypeElement ClaimsAuthenticationManager
		{
			get
			{
				return (CustomTypeElement)base["claimsAuthenticationManager"];
			}
			set
			{
				base["claimsAuthenticationManager"] = value;
			}
		}

		[ConfigurationProperty("claimsAuthorizationManager", IsRequired = false)]
		public CustomTypeElement ClaimsAuthorizationManager
		{
			get
			{
				return (CustomTypeElement)base["claimsAuthorizationManager"];
			}
			set
			{
				base["claimsAuthorizationManager"] = value;
			}
		}

		[ConfigurationProperty("federatedAuthentication", IsRequired = false)]
		public FederatedAuthenticationElement FederatedAuthentication
		{
			get
			{
				return (FederatedAuthenticationElement)base["federatedAuthentication"];
			}
			set
			{
				base["federatedAuthentication"] = value;
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

		[ConfigurationProperty("serviceCertificate", IsRequired = false)]
		public ServiceCertificateElement ServiceCertificate
		{
			get
			{
				return (ServiceCertificateElement)base["serviceCertificate"];
			}
			set
			{
				base["serviceCertificate"] = value;
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

		[ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public SecurityTokenHandlerSetElementCollection SecurityTokenHandlerSets => (SecurityTokenHandlerSetElementCollection)base[""];

		[ConfigurationProperty("applicationService", IsRequired = false)]
		internal ApplicationServiceConfigurationElement ApplicationService
		{
			get
			{
				return (ApplicationServiceConfigurationElement)base["applicationService"];
			}
			set
			{
				base["applicationService"] = value;
			}
		}

		public bool IsConfigured
		{
			get
			{
				if (string.IsNullOrEmpty(Name) && !AudienceUriElements.IsConfigured && !CertificateValidationElement.IsConfigured && !ClaimsAuthenticationManager.IsConfigured && !ClaimsAuthorizationManager.IsConfigured && !FederatedAuthentication.IsConfigured && !IssuerNameRegistry.IsConfigured && !IssuerTokenResolver.IsConfigured && !MaximumClockSkew.IsConfigured && string.IsNullOrEmpty(SaveBootstrapTokens) && !ServiceCertificate.IsConfigured && !ServiceTokenResolver.IsConfigured && !TokenReplayDetectionElement.IsConfigured)
				{
					return SecurityTokenHandlerSets.IsConfigured;
				}
				return true;
			}
		}
	}
}
