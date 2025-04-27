using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;

namespace JNav
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText("test.json");

            Test("NewtonSoft JsonConvert", ()=>
            {
                var root = JsonConvert.DeserializeObject<Root>(json);
                var s = root.Models[1].Children[0].Path;

                // var foo = JsonConvert.SerializeObject(root);
            });

            Test("Nav", () =>
            {
                var root = NavRoot.Parse(json);
                var s = root.Models[1].Children[0].Path;
            });


            Console.ReadKey();
        }

        private static void Test(string test, Action action)
        {
            const int count = 5000;
            Console.WriteLine($"Test: {test}");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < count; i++)
            {
                action();
            }

            sw.Stop();

            Console.WriteLine($"Exection Time: {sw.ElapsedMilliseconds}");
        }
    }

   
}
