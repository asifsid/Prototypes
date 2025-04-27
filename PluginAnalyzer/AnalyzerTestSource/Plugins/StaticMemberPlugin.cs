namespace AnalyzerTestSource.Plugins
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StaticMemberPlugin : IPlugin
    {
        private static IOrganizationService _cachedService; 

        private static EntityReference _cachedEntityRef; 

        public void Execute(IServiceProvider serviceProvider)
        {
            _cachedService = (IOrganizationService)serviceProvider.GetService(typeof(IOrganizationService));
        }

        public class NestedPlugin : IPlugin 
        {
            private static IOrganizationService _cachedServiceNested;

            public void Execute(IServiceProvider serviceProvider)
            {
                _cachedServiceNested = (IOrganizationService)serviceProvider.GetService(typeof(IOrganizationService));
            }
        }
    }
}
