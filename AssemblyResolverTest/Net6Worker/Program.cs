namespace Net6Worker
{
    using Microsoft.Xrm.Sdk;
    using System.Reflection;
    using System.Text.Json;

    internal class Program
    {
        static void Main(string[] args)
        {
            //AppDomainEventHandlers.Init();

            Console.WriteLine($"Host using Json.Text version: {typeof(JsonSerializer).Assembly.GetName().Version}");

            //var force = new ForceSDKLoadPlugin();
            //force.Execute(MockServiceProvider.Default);

            //ExecutePlugin(2, "SamplePlugins.FoobarPlugin");
            //ExecutePlugin(1, "SamplePlugins.FoobarPlugin");
            ExecutePlugin(3, "AssemblyLoader.LoaderPlugin");

            Console.ReadKey();
        }

        private static void ExecutePlugin(int ver, string typeName)
        {
            try
            {
                var path = Path.GetFullPath(Path.Combine("Plugins", "V" + ver.ToString(), "AssemblyLoader.dll"));

                var content = File.ReadAllBytes(path);
                var asm = Assembly.LoadFile(path);
                var type = asm.GetType(typeName);

                if (type == null || !typeof(IPlugin).IsAssignableFrom(type))
                {
                    Console.WriteLine($"Type {typeName} not found or is not IPlugin at {path}");
                    return;
                }

                var plugin = (IPlugin)Activator.CreateInstance(type)!;

                using (ColorContext.Forground(ConsoleColor.Green))
                {
                    plugin.Execute(MockServiceProvider.Default);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }
        }
    }
}