using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class OrganizationCache<TKey, TValue>
	{
		private const int MaxCachedItemsLimit = 15000;

		private ConcurrentDictionary<Tuple<Guid, TKey>, ExpirationCacheItem<TValue>> organizationCache;

		private TimeSpan defaultCacheItemAge;

		private int? maxCacheItems;

		private Guid OrganizationId
		{
			get
			{
				Guid result = Guid.Empty;
				IPluginContext currentContext = PluginContextManager.GetCurrentContext();
				if (currentContext != null)
				{
					result = ((IExecutionContext)currentContext.PluginExecutionContext).get_OrganizationId();
				}
				return result;
			}
		}

		public TimeSpan DefaultCacheItemAge
		{
			get
			{
				if (defaultCacheItemAge == default(TimeSpan))
				{
					defaultCacheItemAge = TimeSpan.FromMinutes(5.0);
				}
				return defaultCacheItemAge;
			}
			set
			{
				defaultCacheItemAge = ((value < TimeSpan.FromHours(2.0)) ? value : TimeSpan.FromHours(2.0));
			}
		}

		public int MaxCachedItems
		{
			get
			{
				if (!maxCacheItems.HasValue)
				{
					maxCacheItems = 15000;
				}
				return maxCacheItems.Value;
			}
			set
			{
				maxCacheItems = ((value < 15000) ? value : 15000);
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				if (TryGetValue(key, out var value))
				{
					return value;
				}
				return default(TValue);
			}
			set
			{
				if (!TryGetValue(key, out var _))
				{
					TryAdd(key, value);
				}
			}
		}

		public OrganizationCache()
		{
			organizationCache = new ConcurrentDictionary<Tuple<Guid, TKey>, ExpirationCacheItem<TValue>>();
		}

		public bool TryAdd(TKey key, TValue value, TimeSpan? cacheItemAge = null)
		{
			if (organizationCache.Count >= MaxCachedItems)
			{
				SweepAndEvict();
				if (organizationCache.Count >= MaxCachedItems)
				{
					return false;
				}
			}
			cacheItemAge = ((!cacheItemAge.HasValue) ? new TimeSpan?(DefaultCacheItemAge) : ((cacheItemAge < DefaultCacheItemAge) ? cacheItemAge : new TimeSpan?(TimeSpan.FromHours(2.0))));
			return organizationCache.TryAdd(Tuple.Create(OrganizationId, key), new ExpirationCacheItem<TValue>(value, cacheItemAge.Value));
		}

		private void SweepAndEvict()
		{
			List<Tuple<Guid, TKey>> list = new List<Tuple<Guid, TKey>>();
			foreach (KeyValuePair<Tuple<Guid, TKey>, ExpirationCacheItem<TValue>> item in organizationCache)
			{
				if (!item.Value.IsValid())
				{
					list.Add(item.Key);
				}
			}
			foreach (Tuple<Guid, TKey> item2 in list)
			{
				organizationCache.TryRemove(item2, out var _);
			}
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			value = default(TValue);
			bool flag = false;
			if (organizationCache.TryGetValue(Tuple.Create(OrganizationId, key), out var value2))
			{
				flag = value2.IsValid();
				if (flag)
				{
					value = value2.Value;
				}
				else
				{
					TryRemove(key);
				}
			}
			return flag;
		}

		public bool TryRemove(TKey key, out TValue value)
		{
			ExpirationCacheItem<TValue> value2;
			bool result = organizationCache.TryRemove(Tuple.Create(OrganizationId, key), out value2);
			value = ((value2 != null) ? value2.Value : default(TValue));
			return result;
		}

		private bool TryRemove(TKey key)
		{
			TValue value;
			return TryRemove(key, out value);
		}

		public bool ContainsKey(TKey key)
		{
			TValue value;
			return TryGetValue(key, out value);
		}

		public void Clear()
		{
			List<Tuple<Guid, TKey>> list = new List<Tuple<Guid, TKey>>();
			foreach (KeyValuePair<Tuple<Guid, TKey>, ExpirationCacheItem<TValue>> item in organizationCache)
			{
				if (item.Key.Item1 == OrganizationId)
				{
					list.Add(item.Key);
				}
			}
			foreach (Tuple<Guid, TKey> item2 in list)
			{
				organizationCache.TryRemove(item2, out var _);
			}
		}
	}
}
