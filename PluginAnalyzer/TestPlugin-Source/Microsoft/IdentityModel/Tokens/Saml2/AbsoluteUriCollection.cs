using System;
using System.Collections.ObjectModel;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	internal class AbsoluteUriCollection : Collection<Uri>
	{
		protected override void InsertItem(int index, Uri item)
		{
			if (null == item || !item.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("item", SR.GetString("ID0013"));
			}
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, Uri item)
		{
			if (null == item || !item.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("item", SR.GetString("ID0013"));
			}
			base.SetItem(index, item);
		}
	}
}
