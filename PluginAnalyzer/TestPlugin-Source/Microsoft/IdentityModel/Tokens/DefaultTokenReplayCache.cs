using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class DefaultTokenReplayCache : TokenReplayCache
	{
		private BoundedCache<SecurityToken> _internalCache;

		public override int Capacity
		{
			get
			{
				return _internalCache.Capacity;
			}
			set
			{
				if (value <= 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", value, SR.GetString("ID0002"));
				}
				_internalCache.Capacity = value;
			}
		}

		public override TimeSpan PurgeInterval
		{
			get
			{
				return _internalCache.PurgeInterval;
			}
			set
			{
				if (value <= TimeSpan.Zero)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", value, SR.GetString("ID0016"));
				}
				_internalCache.PurgeInterval = value;
			}
		}

		public DefaultTokenReplayCache()
			: this(SecurityTokenHandlerConfiguration.DefaultTokenReplayCacheCapacity, SecurityTokenHandlerConfiguration.DefaultTokenReplayCachePurgeInterval)
		{
		}

		public DefaultTokenReplayCache(int capacity, TimeSpan purgeInterval)
		{
			_internalCache = new BoundedCache<SecurityToken>(capacity, purgeInterval, StringComparer.Ordinal);
		}

		public override void Clear()
		{
			_internalCache.Clear();
		}

		public override int IncreaseCapacity(int size)
		{
			if (size <= 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("size", size, SR.GetString("ID0002"));
			}
			return _internalCache.IncreaseCapacity(size);
		}

		public override bool TryAdd(string key, SecurityToken securityToken, DateTime expirationTime)
		{
			if (DateTime.Equals(expirationTime, DateTime.MaxValue))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID1072"));
			}
			return _internalCache.TryAdd(key, securityToken, expirationTime);
		}

		public override bool TryFind(string key)
		{
			return _internalCache.TryFind(key);
		}

		public override bool TryGet(string key, out SecurityToken securityToken)
		{
			return _internalCache.TryGet(key, out securityToken);
		}

		public override bool TryRemove(string key)
		{
			return _internalCache.TryRemove(key);
		}
	}
}
