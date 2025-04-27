using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using NetworkTestPlugin;

namespace InvokePlugin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NetTestPlugin plugin = new NetTestPlugin();

            var ctx = new ExecutionContext();
            ctx.InputParameters.Add("TargetUrl", "www.bing.com");

            plugin.Execute(new ServiceProvider(ctx));

            Console.WriteLine(ctx.OutputParameters["Response"]);
        }

        class ServiceProvider : IServiceProvider
        {
            private ExecutionContext _context;

            public ServiceProvider(ExecutionContext context) 
            { 
                _context = context;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IPluginExecutionContext))
                {
                    return _context;
                }
                return null;
            }
        }

        class ExecutionContext : IPluginExecutionContext
        {
            ParameterCollection _inputs = new ParameterCollection();
            ParameterCollection _outputs = new ParameterCollection();

            public int Stage => throw new NotImplementedException();

            public IPluginExecutionContext ParentContext => throw new NotImplementedException();

            public int Mode => throw new NotImplementedException();

            public int IsolationMode => throw new NotImplementedException();

            public int Depth => throw new NotImplementedException();

            public string MessageName => throw new NotImplementedException();

            public string PrimaryEntityName => throw new NotImplementedException();

            public Guid? RequestId => throw new NotImplementedException();

            public string SecondaryEntityName => throw new NotImplementedException();

            public ParameterCollection InputParameters => _inputs;

            public ParameterCollection OutputParameters => _outputs;

            public ParameterCollection SharedVariables => throw new NotImplementedException();

            public Guid UserId => throw new NotImplementedException();

            public Guid InitiatingUserId => throw new NotImplementedException();

            public Guid BusinessUnitId => throw new NotImplementedException();

            public Guid OrganizationId => throw new NotImplementedException();

            public string OrganizationName => throw new NotImplementedException();

            public Guid PrimaryEntityId => throw new NotImplementedException();

            public EntityImageCollection PreEntityImages => throw new NotImplementedException();

            public EntityImageCollection PostEntityImages => throw new NotImplementedException();

            public EntityReference OwningExtension => throw new NotImplementedException();

            public Guid CorrelationId => throw new NotImplementedException();

            public bool IsExecutingOffline => throw new NotImplementedException();

            public bool IsOfflinePlayback => throw new NotImplementedException();

            public bool IsInTransaction => throw new NotImplementedException();

            public Guid OperationId => throw new NotImplementedException();

            public DateTime OperationCreatedOn => throw new NotImplementedException();
        }
    }
    
}
