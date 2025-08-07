namespace FullFrameworkConsole
{
    using FullFrameworkLib;
    using NetStandardLib;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("From a Full Framework 4.7.1 Console ...");

            FFComponent.Invoke();
            NetStandardComponent.Invoke();

            Console.ReadKey();
        }
    }
}
