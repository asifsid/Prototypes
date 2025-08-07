namespace MockWorker
{
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ForceSDKLoadPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Force-load Plugin invoked.");
            var req = new Microsoft.Xrm.Sdk.Messages.CreateAttributeRequest();
            _ = req.ToString();
        }
    }
}
