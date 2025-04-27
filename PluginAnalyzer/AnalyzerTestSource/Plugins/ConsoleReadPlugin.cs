namespace AnalyzerTestSource.Plugins
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class ConsoleReadPlugin : IPlugin
    {
        public ConsoleReadPlugin() 
        {
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            if (Console.ReadKey().Key != ConsoleKey.X)
            {
                var x = Console.ReadLine();

                _ = Console.Read();
            }
        }
    }
}
