namespace MemCacheSample
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.Eventing.Reader;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class LazyCache<T>
    {
        private readonly MemoryCache _cache;

        public LazyCache(int limitMb, TimeSpan pollInterval)
        {
            var config = new NameValueCollection {
                { "cacheMemoryLimitMegabytes", limitMb.ToString() },
                { "pollingInterval", pollInterval.ToString(@"hh\:mm\:ss") }
            };

            _cache = new MemoryCache(nameof(LazyCache<T>), config);
        }

        public T Get(string key, Func<T> getValue)
        {
            var item = _cache[key];
            if (item == null)
            {
                var lazyValue = new Lazy<T>(() => getValue(), LazyThreadSafetyMode.ExecutionAndPublication);
                var cacheItem = new CacheItem(key, lazyValue);

                cacheItem = _cache.AddOrGetExisting(cacheItem, null);

                return ((Lazy<T>)cacheItem?.Value ?? lazyValue).Value;
            }

            return ((Lazy<T>)item).Value;
        }
    }

    internal class PluginCache<T>
    {
        private readonly CacheItemPolicy _policy;
        private readonly MemoryCache _cache;

        public PluginCache(string name, CacheItemPolicy policy = null)
        {
            _cache = new MemoryCache(name);
            _policy = policy ?? new CacheItemPolicy();
        }

        public T Get(string key, Func<T> getValue)
        {
            var item = _cache[key];
            if (item == null)
            {
                _cache.Set(key, item = getValue(), _policy);
            }

            return (T)item;
        }
    }
}
