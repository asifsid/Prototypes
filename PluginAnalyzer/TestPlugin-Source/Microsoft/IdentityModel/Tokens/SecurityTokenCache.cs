using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public abstract class SecurityTokenCache
	{
		private SecurityTokenHandler _owner;

		public SecurityTokenHandler Owner
		{
			get
			{
				return _owner;
			}
			set
			{
				_owner = value;
			}
		}

		public abstract void ClearEntries();

		public abstract bool TryRemoveEntry(object key);

		public abstract bool TryRemoveAllEntries(object key);

		public abstract bool TryAddEntry(object key, SecurityToken value);

		public abstract bool TryGetEntry(object key, out SecurityToken value);

		public abstract bool TryGetAllEntries(object key, out IList<SecurityToken> tokens);

		public abstract bool TryReplaceEntry(object key, SecurityToken newValue);
	}
}
