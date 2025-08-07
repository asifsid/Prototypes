namespace PluginDependency
{
    using PLuginDependencyNested;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ComponentA
    {
        public static void Execute()
        {
            var nested = new ComponentB();

            Console.WriteLine(nested.GetVersion());
            Console.WriteLine(nested.GetJsonTextVersion());
        }
    }
}
