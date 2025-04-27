using System.ComponentModel;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class X509CertificateValidationElement : ConfigurationElement
	{
		private const X509CertificateValidationMode DefaultX509CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;

		private const X509RevocationMode DefaultX509RevocationMode = X509RevocationMode.Online;

		private const StoreLocation DefaultStoreLocation = StoreLocation.LocalMachine;

		[ConfigurationProperty("certificateValidationMode", IsRequired = false, DefaultValue = X509CertificateValidationMode.PeerOrChainTrust)]
		[TypeConverter(typeof(X509CertificateValidationModeConverter))]
		public X509CertificateValidationMode ValidationMode
		{
			get
			{
				return (X509CertificateValidationMode)base["certificateValidationMode"];
			}
			set
			{
				base["certificateValidationMode"] = value;
			}
		}

		[TypeConverter(typeof(X509RevocationModeConverter))]
		[ConfigurationProperty("revocationMode", IsRequired = false, DefaultValue = X509RevocationMode.Online)]
		public X509RevocationMode RevocationMode
		{
			get
			{
				return (X509RevocationMode)base["revocationMode"];
			}
			set
			{
				base["revocationMode"] = value;
			}
		}

		[ConfigurationProperty("trustedStoreLocation", IsRequired = false, DefaultValue = StoreLocation.LocalMachine)]
		[TypeConverter(typeof(StoreLocationConverter))]
		public StoreLocation TrustedStoreLocation
		{
			get
			{
				return (StoreLocation)base["trustedStoreLocation"];
			}
			set
			{
				base["trustedStoreLocation"] = value;
			}
		}

		[ConfigurationProperty("certificateValidator", IsRequired = false)]
		public CustomTypeElement CustomType
		{
			get
			{
				return (CustomTypeElement)base["certificateValidator"];
			}
			set
			{
				base["certificateValidator"] = value;
			}
		}

		public bool IsConfigured
		{
			get
			{
				if (ValidationMode == X509CertificateValidationMode.PeerOrChainTrust && RevocationMode == X509RevocationMode.Online && TrustedStoreLocation == StoreLocation.LocalMachine)
				{
					return CustomType.IsConfigured;
				}
				return true;
			}
		}
	}
}
