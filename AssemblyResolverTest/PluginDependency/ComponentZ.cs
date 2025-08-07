namespace PluginDependency
{
    using PLuginDependencyNested;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ComponentZ
    {
        public ComponentZ()
        {
            Console.WriteLine("Z created: " + new ComponentB().GetVersion());
        }
    }
}
