using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Web.Configuration;
using System.Web.Hosting;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class WSTrustServiceHost : ServiceHost
	{
		private WSTrustServiceContract _serviceContract;

		public WSTrustServiceContract ServiceContract => _serviceContract;

		public SecurityTokenServiceConfiguration SecurityTokenServiceConfiguration => _serviceContract.SecurityTokenServiceConfiguration;

		public WSTrustServiceHost(SecurityTokenServiceConfiguration securityTokenServiceConfiguration, params Uri[] baseAddresses)
			: this(new WSTrustServiceContract(securityTokenServiceConfiguration), baseAddresses)
		{
		}

		public WSTrustServiceHost(WSTrustServiceContract serviceContract, params Uri[] baseAddresses)
			: base(serviceContract, baseAddresses)
		{
			if (serviceContract == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceContract");
			}
			if (serviceContract.SecurityTokenServiceConfiguration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceContract.SecurityTokenServiceConfiguration");
			}
			_serviceContract = serviceContract;
			if (_serviceContract.SecurityTokenServiceConfiguration.ServiceCertificate != null)
			{
				base.Credentials.ServiceCertificate.Certificate = _serviceContract.SecurityTokenServiceConfiguration.ServiceCertificate;
			}
			Collection<ServiceHostEndpointConfiguration> trustEndpoints = _serviceContract.SecurityTokenServiceConfiguration.TrustEndpoints;
			for (int i = 0; i < trustEndpoints.Count; i++)
			{
				ServiceHostEndpointConfiguration serviceHostEndpointConfiguration = trustEndpoints[i];
				if (!string.IsNullOrEmpty(serviceHostEndpointConfiguration.Address))
				{
					AddServiceEndpoint(serviceHostEndpointConfiguration.Contract, serviceHostEndpointConfiguration.Binding, serviceHostEndpointConfiguration.Address);
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3098"));
			}
		}

		protected virtual void ConfigureMetadata()
		{
			if (base.BaseAddresses == null || base.BaseAddresses.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3140"));
			}
			ServiceMetadataBehavior serviceMetadataBehavior = base.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (serviceMetadataBehavior == null)
			{
				serviceMetadataBehavior = new ServiceMetadataBehavior();
				base.Description.Behaviors.Add(serviceMetadataBehavior);
			}
			bool flag = base.Description.Endpoints.Find(typeof(IMetadataExchange)) != null;
			Binding binding = null;
			foreach (Uri baseAddress in base.BaseAddresses)
			{
				if (StringComparer.OrdinalIgnoreCase.Equals(baseAddress.Scheme, Uri.UriSchemeHttp))
				{
					serviceMetadataBehavior.HttpGetEnabled = true;
					binding = MetadataExchangeBindings.CreateMexHttpBinding();
				}
				else if (StringComparer.OrdinalIgnoreCase.Equals(baseAddress.Scheme, Uri.UriSchemeHttps))
				{
					serviceMetadataBehavior.HttpsGetEnabled = true;
					binding = MetadataExchangeBindings.CreateMexHttpsBinding();
				}
				else if (StringComparer.OrdinalIgnoreCase.Equals(baseAddress.Scheme, Uri.UriSchemeNetTcp))
				{
					binding = MetadataExchangeBindings.CreateMexTcpBinding();
				}
				else if (StringComparer.OrdinalIgnoreCase.Equals(baseAddress.Scheme, Uri.UriSchemeNetPipe))
				{
					binding = MetadataExchangeBindings.CreateMexNamedPipeBinding();
				}
				if (!flag && binding != null)
				{
					AddServiceEndpoint("IMetadataExchange", binding, "mex");
				}
				binding = null;
			}
		}

		protected override void ApplyConfiguration()
		{
			base.ApplyConfiguration();
			WSTrustServiceContract wSTrustServiceContract = (WSTrustServiceContract)base.SingletonInstance;
			if (!wSTrustServiceContract.SecurityTokenServiceConfiguration.DisableWsdl)
			{
				ConfigureMetadata();
			}
		}

		protected override void InitializeRuntime()
		{
			if (base.Description.Endpoints.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3097")));
			}
			ServiceDebugBehavior serviceDebugBehavior = base.Description.Behaviors.Find<ServiceDebugBehavior>();
			if (HostingEnvironment.IsHosted)
			{
				foreach (Uri baseAddress in base.BaseAddresses)
				{
					System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration(baseAddress.AbsolutePath);
					CompilationSection compilationSection = (CompilationSection)configuration.GetSection("system.web/compilation");
					if (compilationSection != null && compilationSection.Debug)
					{
						if (serviceDebugBehavior == null)
						{
							serviceDebugBehavior = new ServiceDebugBehavior();
							base.Description.Behaviors.Add(serviceDebugBehavior);
						}
						serviceDebugBehavior.IncludeExceptionDetailInFaults = true;
						break;
					}
				}
			}
			InitializeSecurityTokenManager();
			base.InitializeRuntime();
		}

		protected virtual void InitializeSecurityTokenManager()
		{
			FederatedServiceCredentials.ConfigureServiceHost(this, _serviceContract.SecurityTokenServiceConfiguration);
		}
	}
}
