using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class X509CertificateValidationCustomTypeElement : ConfigurationElement
	{
		[ConfigurationProperty("certificateValidator", IsRequired = true)]
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

		public bool IsConfigured => CustomType.IsConfigured;
	}
}
