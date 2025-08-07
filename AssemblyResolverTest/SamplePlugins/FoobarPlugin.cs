namespace SamplePlugins
{
    using Microsoft.Xrm.Sdk;
    using PluginDependency;
    using System;

    public class FoobarPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var version = typeof(FoobarPlugin).Assembly.GetName().Version;
            

#if V2
            Console.WriteLine($"Foobar plugin version {version} executed. New crm version: {typeof(IPlugin).Assembly.GetName().Version}");
            ComponentA.Execute();
#elif V3
            Console.WriteLine($"Foobar plugin version {version} executed. New crm version: {typeof(IPlugin).Assembly.GetName().Version}");
            ComponentA.Execute();
#else
            Console.WriteLine($"Foobar plugin version {version} executed. Old crm version: {typeof(IPlugin).Assembly.GetName().Version}");
#endif
            
            
        }
    }
}
