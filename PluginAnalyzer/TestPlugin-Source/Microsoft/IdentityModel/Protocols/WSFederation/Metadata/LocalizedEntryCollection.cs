using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class LocalizedEntryCollection<T> : KeyedCollection<CultureInfo, T> where T : LocalizedEntry
	{
		protected override CultureInfo GetKeyForItem(T item)
		{
			return item.Language;
		}
	}
}
