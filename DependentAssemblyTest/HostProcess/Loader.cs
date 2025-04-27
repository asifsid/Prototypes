namespace HostProcess
{
    using System;
    using System.IO;
    using System.Reflection;

    public class DynamicLoader
    {
        public static readonly DynamicLoader Instance = new DynamicLoader();

        private DynamicLoader()
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public void Invoke(string asmName, string typeName, string methodName)
        {
            Assembly asm = Assembly.LoadFile(asmName);
            try
            {
                var type = asm.GetType(typeName);
                var obj = type.InvokeMember(".ctor", BindingFlags.CreateInstance, null, null, null);
                var invoker = type.GetMethod(methodName);
                invoker.Invoke(obj, null);
            }
            catch (Exception e)
            {
                ConsoleExt.WriteLine(e.InnerException?.Message ?? e.Message, ConsoleColor.Red);
            }
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            ConsoleExt.WriteLine($"Loading {args.LoadedAssembly} from {args.LoadedAssembly.Location}", ConsoleColor.Cyan);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            ConsoleExt.WriteLine($"Resolving Assembly: {args.Name}, requested by: {args.RequestingAssembly}", ConsoleColor.Yellow);

            var sourcePath = Path.GetDirectoryName(args.RequestingAssembly.Location);
            return Assembly.LoadFile(Path.Combine(sourcePath, new AssemblyName(args.Name).Name + ".dll"));

        }
    }

    public static class ConsoleExt
    {
        public static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
