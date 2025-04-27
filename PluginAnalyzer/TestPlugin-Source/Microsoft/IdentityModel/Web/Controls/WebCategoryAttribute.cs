using System;
using System.ComponentModel;

namespace Microsoft.IdentityModel.Web.Controls
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class WebCategoryAttribute : CategoryAttribute
	{
		public override object TypeId => typeof(CategoryAttribute);

		public WebCategoryAttribute(string category)
			: base(category)
		{
		}

		protected override string GetLocalizedString(string value)
		{
			string text = base.GetLocalizedString(value);
			if (text == null)
			{
				text = SR.GetString(value);
			}
			return text;
		}
	}
}
