namespace StripHtml
{
    using System;
    using System.IO;
    using System.Runtime.Caching;

    public class Program
    {
        static void Main(string[] args)
        {
            string html = File.ReadAllText("Raw.txt");

            Console.WriteLine(html);

            var text = HtmlToText.Convert(html);

            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(text);
            Console.Write(text.Substring()
            Console.ReadKey();
        }

        
    }

    public static class CacheHelper
    {
        private static MemoryCache _cache = new MemoryCache(nameof(CacheHelper));

        public static T Get<T>(string key, Func<string, T> onCacheMiss)
            where T : class
        {
            T item = _cache.Get(key) as T;
            if (item == null)
            {
                item = onCacheMiss(key);

                var cachedItem = _cache.AddOrGetExisting(new CacheItem(key), new CacheItemPolicy());
                cachedItem.Value = item;
            }

            return item;
        }
    }
}
