using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeDll
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.LoadFrom("");
            foreach (Type type in assembly.GetTypes())
            {
                foreach (MethodBody methodBody in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {

                }
            }
        }
    }
}
