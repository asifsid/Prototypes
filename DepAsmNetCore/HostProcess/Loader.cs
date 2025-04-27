namespace HostProcess
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;

    public class DynamicLoader : AssemblyLoadContext
    {
        private string _path;
        private Assembly _asm;

        public DynamicLoader(string assemblyName)
        {
            _path = Path.GetDirectoryName(assemblyName);
            _asm = this.LoadFromAssemblyPath(assemblyName);
        }

        public void Invoke(string typeName, string methodName)
        {
            try
            {
                var type = _asm.GetType(typeName);
                var obj = type.InvokeMember(".ctor", BindingFlags.CreateInstance, null, null, null);
                var invoker = type.GetMethod(methodName);
                invoker.Invoke(obj, null);
            }
            catch (Exception e)
            {
                ConsoleExt.WriteLine(e.InnerException?.Message ?? e.Message, ConsoleColor.Red);
            }
        }
        
        protected override Assembly Load(AssemblyName assemblyName)
        {
            ConsoleExt.WriteLine($"Requested assembly {assemblyName}", ConsoleColor.Cyan);

            var local = Path.Combine(_path, assemblyName.Name + ".dll");
            if (File.Exists(local))
            {
                ConsoleExt.WriteLine($"Loading {assemblyName} from {_path}", ConsoleColor.Green);
                return this.LoadFromAssemblyPath(local);
            }
            else
            {
                ConsoleExt.WriteLine($"Resolving default: {assemblyName}", ConsoleColor.Gray);
                return base.Load(assemblyName);
            }
        }

        protected override nint LoadUnmanagedDll(string unmanagedDllName)
        {
            return base.LoadUnmanagedDll(unmanagedDllName);
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
