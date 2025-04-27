using System.Web.UI;

namespace Microsoft.IdentityModel.Web.Controls
{
	internal class SimpleButtonControlBuilder : ControlBuilder
	{
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}
	}
}
