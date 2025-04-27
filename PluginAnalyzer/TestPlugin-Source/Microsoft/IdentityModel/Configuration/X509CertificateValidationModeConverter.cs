using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class X509CertificateValidationModeConverter : ConfigurationConverterBase
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(value is X509CertificateValidationMode))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID7008", value));
			}
			string result = null;
			if (value != null)
			{
				result = Enum.GetName(typeof(X509CertificateValidationMode), value);
			}
			return result;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID7008", value));
			}
			X509CertificateValidationMode x509CertificateValidationMode = (X509CertificateValidationMode)Enum.Parse(typeof(X509CertificateValidationMode), text);
			return x509CertificateValidationMode;
		}
	}
}
