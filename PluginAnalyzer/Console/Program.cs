namespace AnalyzerConsole
{
    using PluginAnalyzer;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    class Program
    {
        const string dllExt = ".dll";

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"Missing assembly name");
                return;
            }

            var assemblyName = args[0].EndsWith(dllExt) ? Path.GetFileNameWithoutExtension(args[0]) : args[0];

            Analyze(assemblyName + dllExt);
        }

        private static void Analyze(string assmeblyName)
        {
            try
            {
                var analyzer = new Analyzer(assmeblyName);
                analyzer.Analyze();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

