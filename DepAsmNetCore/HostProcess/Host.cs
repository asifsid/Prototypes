namespace HostProcess
{
    using AssemblyC;
    using System;
    using System.IO;


    public class Host 
    {
        public void Run()
        {
            Console.WriteLine("Host invoking V3 of C");
            var C = new ClassC();
            C.InvokeV3();
            
            try
            {
                new DynamicLoader(Path.GetFullPath(@"..\..\..\AssemblyA\bin\debug\net8.0\AssemblyA.dll")).Invoke("AssemblyA.ClassA", "InvokeA");
                new DynamicLoader(Path.GetFullPath(@"..\..\..\AssemblyB\bin\debug\net8.0\AssemblyB.dll")).Invoke("AssemblyB.ClassB", "InvokeB");
            }
            catch (Exception e)
            {
                ConsoleExt.WriteLine(e.InnerException?.Message ?? e.Message, ConsoleColor.Red);
            }
        }
    }
}
