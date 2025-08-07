namespace Net6Worker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class MockServiceProvider : IServiceProvider
    {
        public static readonly MockServiceProvider Default = new MockServiceProvider();

        public object GetService(Type serviceType)
        {
            return null;
        }
    }
}
