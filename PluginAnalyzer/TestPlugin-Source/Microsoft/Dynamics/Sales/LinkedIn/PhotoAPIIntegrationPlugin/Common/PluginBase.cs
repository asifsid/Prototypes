using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[ComVisible(true)]
	public abstract class PluginBase : IPlugin
	{
		protected class LocalPluginContext
		{
			internal IServiceProvider ServiceProvider { get; private set; }

			internal IOrganizationService OrganizationService { get; private set; }

			internal IOrganizationService SystemUserOrganizationService { get; private set; }

			internal IPluginExecutionContext PluginExecutionContext { get; private set; }

			internal IKeyVaultClient KeyVaultClient { get; private set; }

			internal ITracingService TracingService { get; private set; }

			internal ILocalConfigStore LocalConfigStore { get; private set; }

			private LocalPluginContext()
			{
			}

			internal LocalPluginContext(IServiceProvider serviceProvider)
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Expected O, but got Unknown
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Expected O, but got Unknown
				if (serviceProvider == null)
				{
					throw new InvalidPluginExecutionException("serviceProvider");
				}
				PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
				TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
				IOrganizationServiceFactory val = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
				OrganizationService = val.CreateOrganizationService((Guid?)((IExecutionContext)PluginExecutionContext).get_UserId());
				SystemUserOrganizationService = val.CreateOrganizationService((Guid?)null);
				object service = serviceProvider.GetService(typeof(IKeyVaultClient));
				KeyVaultClient = service as IKeyVaultClient;
				object service2 = serviceProvider.GetService(typeof(ILocalConfigStore));
				LocalConfigStore = service2 as ILocalConfigStore;
				ServiceProvider = serviceProvider;
			}

			internal void Trace(string message)
			{
				if (!string.IsNullOrWhiteSpace(message) && TracingService != null)
				{
					if (PluginExecutionContext == null)
					{
						TracingService.Trace(message, Array.Empty<object>());
						return;
					}
					TracingService.Trace("{0}, Correlation Id: {1}, Initiating User: {2}", new object[3]
					{
						message,
						((IExecutionContext)PluginExecutionContext).get_CorrelationId(),
						((IExecutionContext)PluginExecutionContext).get_InitiatingUserId()
					});
				}
			}
		}

		protected string ChildClassName { get; private set; }

		internal PluginBase(Type childClassName)
		{
			ChildClassName = childClassName.ToString();
		}

		public void Execute(IServiceProvider serviceProvider)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			if (serviceProvider == null)
			{
				throw new InvalidPluginExecutionException("serviceProvider");
			}
			LocalPluginContext localPluginContext = new LocalPluginContext(serviceProvider);
			localPluginContext.Trace(string.Format(CultureInfo.InvariantCulture, "Entered {0}.Execute()", ChildClassName));
			try
			{
				ExecuteCrmPlugin(localPluginContext);
			}
			catch (FaultException<OrganizationServiceFault> ex)
			{
				localPluginContext.Trace(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", ex.ToString()));
				throw new InvalidPluginExecutionException(ex.Message, (Exception)ex);
			}
			finally
			{
				localPluginContext.Trace(string.Format(CultureInfo.InvariantCulture, "Exiting {0}.Execute()", ChildClassName));
			}
		}

		protected abstract void ExecuteCrmPlugin(LocalPluginContext localcontext);
	}
}
