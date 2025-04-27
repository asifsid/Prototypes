using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class DisplayClaimCollection : Collection<DisplayClaim>
	{
		public DisplayClaimCollection()
		{
		}

		public DisplayClaimCollection(IEnumerable<DisplayClaim> collection)
		{
			AddRange(collection);
		}

		public void AddRange(IEnumerable<DisplayClaim> collection)
		{
			if (collection == null)
			{
				return;
			}
			foreach (DisplayClaim item in collection)
			{
				Add(item);
			}
		}
	}
}
