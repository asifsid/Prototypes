using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class PrefetchEntityCache
	{
		private Dictionary<string, object> entityCollectionCache = new Dictionary<string, object>();

		public bool TryGetValue<T>(string key, ref T value)
		{
			bool result = false;
			if (entityCollectionCache.ContainsKey(key))
			{
				try
				{
					value = (T)entityCollectionCache[key];
					result = true;
				}
				catch (Exception)
				{
				}
			}
			return result;
		}

		public T Get<T>(string key)
		{
			T result = default(T);
			if (entityCollectionCache.ContainsKey(key))
			{
				return (T)entityCollectionCache[key];
			}
			return result;
		}

		public void Set(string key, object value)
		{
			if (!entityCollectionCache.ContainsKey(key))
			{
				entityCollectionCache.Add(key, value);
			}
			else
			{
				entityCollectionCache[key] = value;
			}
		}
	}
}
