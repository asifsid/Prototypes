namespace CachingSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Text;
    using System.Threading.Tasks;

    public class PluginCache
    {
        public static PluginCache Default => new PluginCache();

        private MemoryCache _cache = new MemoryCache(nameof(PluginCache));
        private CacheItemPolicy _defaultPolicy;

        public PluginCache()
        {
            _cache.Add()
        }



    }
}
