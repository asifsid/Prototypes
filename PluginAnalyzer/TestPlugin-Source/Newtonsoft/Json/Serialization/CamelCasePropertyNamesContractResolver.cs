using System;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		private static readonly object TypeContractCacheLock = new object();

		private static readonly PropertyNameTable NameTable = new PropertyNameTable();

		private static Dictionary<ResolverContractKey, JsonContract> _contractCache;

		public CamelCasePropertyNamesContractResolver()
		{
			base.NamingStrategy = new CamelCaseNamingStrategy
			{
				ProcessDictionaryKeys = true,
				OverrideSpecifiedNames = true
			};
		}

		public override JsonContract ResolveContract(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ResolverContractKey key = new ResolverContractKey(GetType(), type);
			Dictionary<ResolverContractKey, JsonContract> contractCache = _contractCache;
			if (contractCache == null || !contractCache.TryGetValue(key, out var value))
			{
				value = CreateContract(type);
				lock (TypeContractCacheLock)
				{
					contractCache = _contractCache;
					Dictionary<ResolverContractKey, JsonContract> obj = ((contractCache != null) ? new Dictionary<ResolverContractKey, JsonContract>(contractCache) : new Dictionary<ResolverContractKey, JsonContract>());
					obj[key] = value;
					_contractCache = obj;
					return value;
				}
			}
			return value;
		}

		internal override PropertyNameTable GetNameTable()
		{
			return NameTable;
		}
	}
}
