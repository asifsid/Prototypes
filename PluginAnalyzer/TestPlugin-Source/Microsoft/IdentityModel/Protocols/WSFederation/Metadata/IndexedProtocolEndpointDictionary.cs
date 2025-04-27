using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class IndexedProtocolEndpointDictionary : SortedList<int, IndexedProtocolEndpoint>
	{
		public IndexedProtocolEndpoint Default
		{
			get
			{
				IndexedProtocolEndpoint indexedProtocolEndpoint = null;
				using (IEnumerator<KeyValuePair<int, IndexedProtocolEndpoint>> enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, IndexedProtocolEndpoint> current = enumerator.Current;
						if (current.Value.IsDefault == true)
						{
							return current.Value;
						}
						if (!current.Value.IsDefault.HasValue && indexedProtocolEndpoint == null)
						{
							indexedProtocolEndpoint = current.Value;
						}
					}
				}
				if (indexedProtocolEndpoint != null)
				{
					return indexedProtocolEndpoint;
				}
				if (base.Count > 0)
				{
					return base[base.Keys[0]];
				}
				return null;
			}
		}
	}
}
