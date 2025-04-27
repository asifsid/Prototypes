using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class X509RevocationModeConverter : ConfigurationConverterBase
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(value is X509RevocationMode))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID7008", value));
			}
			string result = null;
			if (value != null)
			{
				result = Enum.GetName(typeof(X509RevocationMode), value);
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
			X509RevocationMode x509RevocationMode = (X509RevocationMode)Enum.Parse(typeof(X509RevocationMode), text);
			return x509RevocationMode;
		}
	}
}
