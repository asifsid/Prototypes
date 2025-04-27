using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class FederatedServiceCredentials : ServiceCredentials
	{
		private bool _saveBootstrapTokenInSession = true;

		private Microsoft.IdentityModel.Configuration.ServiceConfiguration _configuration;

		public ClaimsAuthenticationManager ClaimsAuthenticationManager
		{
			get
			{
				return _configuration.ClaimsAuthenticationManager;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_configuration.ClaimsAuthenticationManager = value;
			}
		}

		public new ExceptionMapper ExceptionMapper
		{
			get
			{
				return _configuration.ExceptionMapper;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_configuration.ExceptionMapper = value;
			}
		}

		public ClaimsAuthorizationManager ClaimsAuthorizationManager
		{
			get
			{
				return _configuration.ClaimsAuthorizationManager;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_configuration.ClaimsAuthorizationManager = value;
			}
		}

		public TimeSpan MaxClockSkew => _configuration.MaxClockSkew;

		public bool SaveBootstrapTokens => _configuration.SecurityTokenHandlers.Configuration.SaveBootstrapTokens;

		public SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager => _configuration.SecurityTokenHandlerCollectionManager;

		public SecurityTokenHandlerCollection SecurityTokenHandlers => _configuration.SecurityTokenHandlerCollectionManager[""];

		internal Microsoft.IdentityModel.Configuration.ServiceConfiguration ServiceConfiguration => _configuration;

		public FederatedServiceCredentials()
			: this(new ServiceCredentials(), new Microsoft.IdentityModel.Configuration.ServiceConfiguration())
		{
		}

		public FederatedServiceCredentials(ServiceCredentials innerServiceCredentials)
			: this(innerServiceCredentials, new Microsoft.IdentityModel.Configuration.ServiceConfiguration())
		{
		}

		public FederatedServiceCredentials(Microsoft.IdentityModel.Configuration.ServiceConfiguration configuration)
			: this(new ServiceCredentials(), configuration)
		{
		}

		public FederatedServiceCredentials(ServiceCredentials innerServiceCredentials, Microsoft.IdentityModel.Configuration.ServiceConfiguration configuration)
			: base(innerServiceCredentials)
		{
			_configuration = configuration;
		}

		protected FederatedServiceCredentials(FederatedServiceCredentials other)
			: base(other)
		{
			if (other == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("other");
			}
			_configuration = other._configuration;
			_saveBootstrapTokenInSession = other._saveBootstrapTokenInSession;
		}

		protected override ServiceCredentials CloneCore()
		{
			return new FederatedServiceCredentials(this);
		}

		public override SecurityTokenManager CreateSecurityTokenManager()
		{
			FederatedSecurityTokenManager federatedSecurityTokenManager = new FederatedSecurityTokenManager(this, _configuration.SecurityTokenHandlers, _configuration.ClaimsAuthenticationManager);
			federatedSecurityTokenManager.ExceptionMapper = _configuration.ExceptionMapper;
			return federatedSecurityTokenManager;
		}

		public static void ConfigureServiceHost(ServiceHostBase serviceHost)
		{
			if (serviceHost == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceHost");
			}
			ConfigureServiceHost(serviceHost, new Microsoft.IdentityModel.Configuration.ServiceConfiguration());
		}

		public static void ConfigureServiceHost(ServiceHostBase serviceHost, string serviceName)
		{
			if (serviceHost == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceHost");
			}
			ConfigureServiceHost(serviceHost, new Microsoft.IdentityModel.Configuration.ServiceConfiguration(serviceName));
		}

		public static void ConfigureServiceHost(ServiceHostBase serviceHost, Microsoft.IdentityModel.Configuration.ServiceConfiguration configuration)
		{
			if (serviceHost == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceHost");
			}
			if (configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("configuration");
			}
			if (serviceHost.State != 0 && serviceHost.State != CommunicationState.Opening)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4041", serviceHost));
			}
			ServiceCredentials serviceCredentials = serviceHost.Description.Behaviors.Find<ServiceCredentials>();
			if (!(serviceCredentials is FederatedServiceCredentials))
			{
				serviceHost.Description.Behaviors.Remove<ServiceCredentials>();
				FederatedServiceCredentials federatedServiceCredentials = ((serviceCredentials == null) ? new FederatedServiceCredentials(new ServiceCredentials(), configuration) : new FederatedServiceCredentials(serviceCredentials, configuration));
				if (configuration.ServiceCertificate != null)
				{
					federatedServiceCredentials.ServiceCertificate.Certificate = configuration.ServiceCertificate;
				}
				else if (federatedServiceCredentials.ServiceCertificate != null && federatedServiceCredentials.ServiceCertificate.Certificate != null)
				{
					configuration.ServiceCertificate = federatedServiceCredentials.ServiceCertificate.Certificate;
				}
				if (object.ReferenceEquals(configuration.IssuerTokenResolver, SecurityTokenHandlerConfiguration.DefaultIssuerTokenResolver) && federatedServiceCredentials.IssuedTokenAuthentication != null && federatedServiceCredentials.IssuedTokenAuthentication.KnownCertificates != null && federatedServiceCredentials.IssuedTokenAuthentication.KnownCertificates.Count > 0)
				{
					List<SecurityToken> list = new List<SecurityToken>();
					foreach (X509Certificate2 knownCertificate in federatedServiceCredentials.IssuedTokenAuthentication.KnownCertificates)
					{
						list.Add(new X509SecurityToken(knownCertificate));
					}
					SecurityTokenResolver securityTokenResolver = SecurityTokenResolver.CreateDefaultSecurityTokenResolver(list.AsReadOnly(), canMatchLocalId: false);
					configuration.IssuerTokenResolver = new AggregateTokenResolver(new SecurityTokenResolver[2]
					{
						securityTokenResolver,
						SecurityTokenHandlerConfiguration.DefaultIssuerTokenResolver
					});
				}
				if (!configuration.IsInitialized)
				{
					configuration.Initialize();
				}
				federatedServiceCredentials.ClaimsAuthenticationManager = configuration.ClaimsAuthenticationManager;
				federatedServiceCredentials.ClaimsAuthorizationManager = configuration.ClaimsAuthorizationManager;
				ServiceAuthorizationBehavior serviceAuthorizationBehavior = serviceHost.Description.Behaviors.Find<ServiceAuthorizationBehavior>();
				if (serviceAuthorizationBehavior == null)
				{
					serviceAuthorizationBehavior = new ServiceAuthorizationBehavior();
					serviceHost.Description.Behaviors.Add(serviceAuthorizationBehavior);
				}
				serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
				serviceHost.Description.Behaviors.Add(federatedServiceCredentials);
			}
			if (serviceHost.Authorization.ServiceAuthorizationManager == null)
			{
				serviceHost.Authorization.ServiceAuthorizationManager = new IdentityModelServiceAuthorizationManager();
			}
			else if (!(serviceHost.Authorization.ServiceAuthorizationManager is IdentityModelServiceAuthorizationManager))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4039")));
			}
			if (configuration.SecurityTokenHandlers[typeof(SecurityContextSecurityToken)] != null && serviceHost.Credentials.SecureConversationAuthentication.SecurityStateEncoder == null)
			{
				serviceHost.Credentials.SecureConversationAuthentication.SecurityStateEncoder = new NoOpSecurityStateEncoder();
			}
		}
	}
}
