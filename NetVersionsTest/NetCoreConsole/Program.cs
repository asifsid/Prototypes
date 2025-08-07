namespace NetCoreConsole
{
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("From a Net Core 3.1 Console ...");

            NetStandardLib.NetStandardComponent.Invoke();
            FullFrameworkLib.FFComponent.Invoke();

            Console.ReadKey();
        }
    }
}
