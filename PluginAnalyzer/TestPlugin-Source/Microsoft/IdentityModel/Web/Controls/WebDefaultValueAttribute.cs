using System;
using System.ComponentModel;

namespace Microsoft.IdentityModel.Web.Controls
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class WebDefaultValueAttribute : DefaultValueAttribute
	{
		private Type _type;

		private bool _localized;

		public override object TypeId => typeof(DefaultValueAttribute);

		public override object Value
		{
			get
			{
				if (!_localized)
				{
					_localized = true;
					string name = (string)base.Value;
					if (!string.IsNullOrEmpty(name))
					{
						object obj = SR.GetString(name);
						if ((object)_type != null)
						{
							try
							{
								obj = TypeDescriptor.GetConverter(_type).ConvertFromInvariantString((string)obj);
							}
							catch (NotSupportedException)
							{
								obj = null;
							}
						}
						SetValue(obj);
					}
				}
				return base.Value;
			}
		}

		public WebDefaultValueAttribute(string value)
			: this(null, value)
		{
		}

		public WebDefaultValueAttribute(Type valueType, string value)
			: base(value)
		{
			_type = valueType;
		}
	}
}
