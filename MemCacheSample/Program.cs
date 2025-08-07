namespace MemCacheSample
{
    using BenchmarkDotNet.Analysers;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Columns;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Engines;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Filters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Loggers;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Reports;
    using BenchmarkDotNet.Running;
    using BenchmarkDotNet.Validators;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Program
    {
        private static LazyCache<string> _pluginCache = new LazyCache<string>(10, TimeSpan.FromMinutes(2));

        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<CacheBenchmark>();

            Console.WriteLine(_pluginCache.Get("someKey", () => "Value"));
            Console.WriteLine(_pluginCache.Get("someKey", () => "Value 2"));
            Console.WriteLine(_pluginCache.Get("someKey2", () => "Value 3"));

            Console.ReadKey();

        }

    }

    [SimpleJob(RuntimeMoniker.Net472)]
    [MinIterationTime(1000)]
    [InvocationCount(5000)]
    [WarmupCount(0)]
    public class CacheBenchmark
    {
        private Random _rand = new Random();
        private LazyCache<string> _cache1 = new LazyCache<string>(10, TimeSpan.FromMinutes(2));
        private PluginCache<string> _cache2 = new PluginCache<string>(nameof(_cache2));
        
        [Params(50, 500)]
        public int KeyCount { get; set; }

        [Params(100, 1500)]
        public int GetCost { get; set; }

        public string Key => Keys[_rand.Next(KeyCount)];
        
        public string[] Keys => Enumerable.Range(0, KeyCount).Select(_ => Guid.NewGuid().ToString()).ToArray();
        
        private Func<string> Getter => () =>
        {
            Thread.SpinWait(GetCost);
            return $"The value of key <{Key}>";
        };

        [Benchmark]
        public void LazyMultiGet()
        {
            _cache1.Get(Key, Getter);
        }

        
        [Benchmark]
        public void SimpleGet()
        {
            _cache2.Get(Key, Getter);
        }
    }

    /*
            |       Method | KeyCount | GetCost |      Mean |     Error |    StdDev |
            |------------- |--------- |-------- |----------:|----------:|----------:|
            | LazyMultiGet |       50 |     100 |  66.19 us |  1.313 us |  3.550 us |
            |  LazySyncGet |       50 |     100 |  61.26 us |  1.223 us |  1.590 us |
            |    SimpleGet |       50 |     100 |  64.08 us |  1.275 us |  3.223 us |
            | LazyMultiGet |       50 |    1500 |  68.90 us |  1.303 us |  1.338 us |
            |  LazySyncGet |       50 |    1500 |  68.75 us |  1.342 us |  1.378 us |
            |    SimpleGet |       50 |    1500 |  69.72 us |  1.369 us |  2.362 us |
            | LazyMultiGet |      500 |     100 | 553.85 us | 10.389 us | 10.203 us |
            |  LazySyncGet |      500 |     100 | 556.39 us | 10.994 us |  9.181 us |
            |    SimpleGet |      500 |     100 | 558.46 us |  8.966 us |  8.386 us |
            | LazyMultiGet |      500 |    1500 | 572.98 us |  5.031 us |  4.460 us |
            |  LazySyncGet |      500 |    1500 | 576.85 us | 10.828 us | 10.129 us |
            |    SimpleGet |      500 |    1500 | 558.59 us | 10.121 us |  8.972 us |
    */
}
