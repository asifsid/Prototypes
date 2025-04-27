using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class ServiceCertificateElement : ConfigurationElement
	{
		[ConfigurationProperty("certificateReference", IsRequired = false)]
		public CertificateReferenceElement CertificateReference
		{
			get
			{
				return (CertificateReferenceElement)base["certificateReference"];
			}
			set
			{
				base["certificateReference"] = value;
			}
		}

		public bool IsConfigured => !string.IsNullOrEmpty(CertificateReference.FindValue);

		internal X509Certificate2 GetCertificate()
		{
			if (IsConfigured)
			{
				try
				{
					return X509Util.ResolveCertificate(CertificateReference);
				}
				catch (InvalidOperationException inner)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(this, "certificateReference", inner);
				}
			}
			return null;
		}
	}
}
