using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class ConfigureServiceHostServiceBehavior : IServiceBehavior
	{
		private string _serviceName;

		public ConfigureServiceHostServiceBehavior()
		{
		}

		public ConfigureServiceHostServiceBehavior(string serviceName)
		{
			if (serviceName == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceName");
			}
			_serviceName = serviceName;
		}

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			if (_serviceName != null && !StringComparer.Ordinal.Equals(_serviceName, Microsoft.IdentityModel.Configuration.ServiceConfiguration.DefaultServiceName))
			{
				FederatedServiceCredentials.ConfigureServiceHost(serviceHostBase, _serviceName);
			}
			else
			{
				FederatedServiceCredentials.ConfigureServiceHost(serviceHostBase);
			}
		}
	}
}
