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
                DynamicLoader.Instance.Invoke(Path.GetFullPath(@"..\..\AssemblyA\bin\debug\AssemblyA.dll"), "AssemblyA.ClassA", "InvokeA");
                DynamicLoader.Instance.Invoke(Path.GetFullPath(@"..\..\AssemblyB\bin\debug\AssemblyB.dll"), "AssemblyB.ClassB", "InvokeB");
            }
            catch (Exception e)
            {
                ConsoleExt.WriteLine(e.InnerException?.Message ?? e.Message, ConsoleColor.Red);
            }
        }
    }
}
