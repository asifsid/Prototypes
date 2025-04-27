using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class AdditionalContext
	{
		private List<ContextItem> _contextItems = new List<ContextItem>();

		public IList<ContextItem> Items => _contextItems;

		public AdditionalContext()
		{
		}

		public AdditionalContext(IEnumerable<ContextItem> items)
		{
			if (items == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("items");
			}
			foreach (ContextItem item in items)
			{
				_contextItems.Add(item);
			}
		}
	}
}
