namespace AnalyzerTestSource.Plugins
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EnvironmentExitPlugin : IPlugin
    {
        public EnvironmentExitPlugin()
        {
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            Environment.Exit(1);
        }
    }
}
