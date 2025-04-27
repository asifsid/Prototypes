namespace DependentAssemblyTest
{
    //using AssemblyC;
    using System;
    using System.IO;
    using System.Reflection;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
#if Default_LoadContext
                var host = new HostProcess.Host();
                host.Run();
#else
                var hostAsm = Assembly.LoadFrom(Path.GetFullPath(@"..\..\HostProcess\bin\debug\HostProcess.dll"));
                var hostType = hostAsm.GetType("HostProcess.Host");
                var host = hostType.InvokeMember(".ctor", BindingFlags.CreateInstance, null, null, null);
                var run = hostType.GetMethod("Run", BindingFlags.Public | BindingFlags.Instance);

                run.Invoke(host, new object[] { });
#endif
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException?.Message ?? e.Message);
            }

            Console.ReadKey();
        }
    }
}
