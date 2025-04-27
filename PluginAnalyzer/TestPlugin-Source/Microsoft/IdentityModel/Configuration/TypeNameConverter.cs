using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.Compilation;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public sealed class TypeNameConverter : ConfigurationConverterBase
	{
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(value is Type))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID7008", value));
			}
			string result = null;
			if (value != null)
			{
				result = ((Type)value).AssemblyQualifiedName;
			}
			return result;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Type type = BuildManager.GetType((string)value, throwOnError: true);
			if ((object)type == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID7007", value));
			}
			return type;
		}
	}
}
