using Microsoft.Xrm.Sdk;
using System;
using System.IO;
using System.Reflection;

namespace AssemblyLoader
{
    public class LoaderPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().CodeBase);
            Console.WriteLine(this.GetType().Assembly.CodeBase);
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(this.GetType().Assembly.Location);

            LoadAssembly(Path.GetFullPath(Path.Combine("Plugins", "V3", "SamplePlugins.dll")), "SamplePlugins.FoobarPlugin", serviceProvider);
        }

        public void LoadAssembly(string path, string invokeType, IServiceProvider serviceProvider)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"File {path} does not exist.");
                return;
            }

            var asm = Assembly.LoadFile(path);
            foreach (var type in asm.GetTypes())
            {
                Console.WriteLine($"Type: {type}");
                if (type.FullName == invokeType)
                {
                    Console.WriteLine($"Type found: {invokeType}");
                    
                    var plugin = (IPlugin)Activator.CreateInstance(type);
                    plugin.Execute(serviceProvider);
                   
                }
            }
        }
    }
}
