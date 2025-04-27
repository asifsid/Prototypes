using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class AudienceUriModeConverter : ConfigurationConverterBase
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(value is AudienceUriMode))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID7008", value));
			}
			string result = null;
			if (value != null)
			{
				result = Enum.GetName(typeof(AudienceUriMode), value);
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
			AudienceUriMode audienceUriMode = (AudienceUriMode)Enum.Parse(typeof(AudienceUriMode), text);
			return audienceUriMode;
		}
	}
}
