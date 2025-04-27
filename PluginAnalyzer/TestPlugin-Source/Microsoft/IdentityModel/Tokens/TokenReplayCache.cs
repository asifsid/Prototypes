using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public abstract class TokenReplayCache
	{
		public abstract int Capacity { get; set; }

		public abstract TimeSpan PurgeInterval { get; set; }

		public abstract void Clear();

		public abstract int IncreaseCapacity(int size);

		public abstract bool TryAdd(string key, SecurityToken securityToken, DateTime expirationTime);

		public abstract bool TryFind(string key);

		public abstract bool TryGet(string key, out SecurityToken securityToken);

		public abstract bool TryRemove(string key);
	}
}
