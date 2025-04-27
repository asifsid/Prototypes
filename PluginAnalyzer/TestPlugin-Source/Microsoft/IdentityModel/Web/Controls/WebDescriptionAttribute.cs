using System;
using System.ComponentModel;

namespace Microsoft.IdentityModel.Web.Controls
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class WebDescriptionAttribute : DescriptionAttribute
	{
		private bool _localized;

		public override string Description
		{
			get
			{
				if (!_localized)
				{
					_localized = true;
					base.DescriptionValue = SR.GetString(base.Description);
				}
				return base.Description;
			}
		}

		public override object TypeId => typeof(DescriptionAttribute);

		internal WebDescriptionAttribute(string description)
			: base(description)
		{
		}
	}
}
