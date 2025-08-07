namespace SamplePlugins
{
    using PluginDependency;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ForceDependecyLoad : ComponentZ
    {
        public static ComponentZ Default { get; private set; }

        static ForceDependecyLoad()
        {
            Default = new ForceDependecyLoad();
        }
    }
}
