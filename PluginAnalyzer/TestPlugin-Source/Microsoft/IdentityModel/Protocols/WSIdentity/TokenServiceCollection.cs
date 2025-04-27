using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class TokenServiceCollection : KeyedCollection<string, TokenService>
	{
		public TokenServiceCollection()
		{
		}

		public TokenServiceCollection(IEnumerable<TokenService> collection)
		{
			AddRange(collection);
		}

		public void AddRange(IEnumerable<TokenService> collection)
		{
			if (collection == null)
			{
				return;
			}
			foreach (TokenService item in collection)
			{
				Add(item);
			}
		}

		protected override string GetKeyForItem(TokenService item)
		{
			return item.Address.Uri.AbsoluteUri;
		}
	}
}
