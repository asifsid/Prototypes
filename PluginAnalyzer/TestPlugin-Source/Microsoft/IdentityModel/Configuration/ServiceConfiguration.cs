using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class ServiceConfiguration
	{
		internal const string ServiceConfigurationKey = "ServiceConfiguration";

		public static readonly string DefaultServiceName = "";

		public static readonly TimeSpan DefaultMaxClockSkew = new TimeSpan(0, 5, 0);

		public static readonly X509CertificateValidationMode DefaultCertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;

		public static readonly X509RevocationMode DefaultRevocationMode = X509RevocationMode.Online;

		public static readonly StoreLocation DefaultTrustedStoreLocation = StoreLocation.LocalMachine;

		private X509CertificateValidationMode _certificateValidationMode = DefaultCertificateValidationMode;

		private ClaimsAuthenticationManager _claimsAuthenticationManager = new ClaimsAuthenticationManager();

		private ClaimsAuthorizationManager _claimsAuthorizationManager = new ClaimsAuthorizationManager();

		private bool _disableWsdl;

		private ExceptionMapper _exceptionMapper = new ExceptionMapper();

		private bool _isInitialized;

		private X509RevocationMode _revocationMode = DefaultRevocationMode;

		private Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager;

		private string _serviceName = DefaultServiceName;

		private X509Certificate2 _serviceCertificate;

		private TimeSpan _serviceMaxClockSkew = DefaultMaxClockSkew;

		private Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration _serviceHandlerConfiguration;

		private StoreLocation _trustedStoreLocation = DefaultTrustedStoreLocation;

		public Microsoft.IdentityModel.Tokens.AudienceRestriction AudienceRestriction
		{
			get
			{
				return _serviceHandlerConfiguration.AudienceRestriction;
			}
			set
			{
				_serviceHandlerConfiguration.AudienceRestriction = value;
			}
		}

		public X509CertificateValidationMode CertificateValidationMode
		{
			get
			{
				return _certificateValidationMode;
			}
			set
			{
				_certificateValidationMode = value;
			}
		}

		public X509CertificateValidator CertificateValidator
		{
			get
			{
				return _serviceHandlerConfiguration.CertificateValidator;
			}
			set
			{
				_serviceHandlerConfiguration.CertificateValidator = value;
			}
		}

		public ClaimsAuthenticationManager ClaimsAuthenticationManager
		{
			get
			{
				return _claimsAuthenticationManager;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_claimsAuthenticationManager = value;
			}
		}

		public ClaimsAuthorizationManager ClaimsAuthorizationManager
		{
			get
			{
				return _claimsAuthorizationManager;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_claimsAuthorizationManager = value;
			}
		}

		public bool DetectReplayedTokens
		{
			get
			{
				return _serviceHandlerConfiguration.DetectReplayedTokens;
			}
			set
			{
				_serviceHandlerConfiguration.DetectReplayedTokens = value;
			}
		}

		public bool DisableWsdl
		{
			get
			{
				return _disableWsdl;
			}
			set
			{
				_disableWsdl = value;
			}
		}

		public virtual bool IsInitialized
		{
			get
			{
				return _isInitialized;
			}
			protected set
			{
				_isInitialized = value;
			}
		}

		public TimeSpan MaxClockSkew
		{
			get
			{
				return _serviceHandlerConfiguration.MaxClockSkew;
			}
			set
			{
				_serviceHandlerConfiguration.MaxClockSkew = value;
			}
		}

		public string Name => _serviceName;

		public Microsoft.IdentityModel.Tokens.IssuerNameRegistry IssuerNameRegistry
		{
			get
			{
				return _serviceHandlerConfiguration.IssuerNameRegistry;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_serviceHandlerConfiguration.IssuerNameRegistry = value;
			}
		}

		public ExceptionMapper ExceptionMapper
		{
			get
			{
				return _exceptionMapper;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_exceptionMapper = value;
			}
		}

		public SecurityTokenResolver IssuerTokenResolver
		{
			get
			{
				return _serviceHandlerConfiguration.IssuerTokenResolver;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_serviceHandlerConfiguration.IssuerTokenResolver = value;
			}
		}

		public X509RevocationMode RevocationMode
		{
			get
			{
				return _revocationMode;
			}
			set
			{
				_revocationMode = value;
			}
		}

		public X509Certificate2 ServiceCertificate
		{
			get
			{
				return _serviceCertificate;
			}
			set
			{
				_serviceCertificate = value;
			}
		}

		public SecurityTokenResolver ServiceTokenResolver
		{
			get
			{
				return _serviceHandlerConfiguration.ServiceTokenResolver;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_serviceHandlerConfiguration.ServiceTokenResolver = value;
			}
		}

		public bool SaveBootstrapTokens
		{
			get
			{
				return _serviceHandlerConfiguration.SaveBootstrapTokens;
			}
			set
			{
				_serviceHandlerConfiguration.SaveBootstrapTokens = value;
			}
		}

		public Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager => _securityTokenHandlerCollectionManager;

		public Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection SecurityTokenHandlers => _securityTokenHandlerCollectionManager[""];

		public Microsoft.IdentityModel.Tokens.TokenReplayCache TokenReplayCache
		{
			get
			{
				return _serviceHandlerConfiguration.TokenReplayCache;
			}
			set
			{
				_serviceHandlerConfiguration.TokenReplayCache = value;
			}
		}

		public TimeSpan TokenReplayCacheExpirationPeriod
		{
			get
			{
				return _serviceHandlerConfiguration.TokenReplayCacheExpirationPeriod;
			}
			set
			{
				_serviceHandlerConfiguration.TokenReplayCacheExpirationPeriod = value;
			}
		}

		public StoreLocation TrustedStoreLocation
		{
			get
			{
				return _trustedStoreLocation;
			}
			set
			{
				_trustedStoreLocation = value;
			}
		}

		public ServiceConfiguration()
		{
			ServiceElement element = MicrosoftIdentityModelSection.Current?.ServiceElements.GetElement(DefaultServiceName);
			LoadConfiguration(element);
		}

		public ServiceConfiguration(bool loadConfig)
		{
			if (loadConfig)
			{
				MicrosoftIdentityModelSection current = MicrosoftIdentityModelSection.Current;
				if (current == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7027"));
				}
				ServiceElement element = current.ServiceElements.GetElement(DefaultServiceName);
				LoadConfiguration(element);
			}
			else
			{
				LoadConfiguration(null);
			}
		}

		public ServiceConfiguration(string serviceConfigurationName)
		{
			if (serviceConfigurationName == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceConfigurationName");
			}
			MicrosoftIdentityModelSection current = MicrosoftIdentityModelSection.Current;
			if (current == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7027"));
			}
			_serviceName = serviceConfigurationName;
			ServiceElement element = current.ServiceElements.GetElement(serviceConfigurationName);
			LoadConfiguration(element);
		}

		internal static ServiceConfiguration GetCurrent()
		{
			if (OperationContext.Current != null)
			{
				if (OperationContext.Current.IncomingMessageProperties.ContainsKey("ServiceConfiguration"))
				{
					ServiceConfiguration serviceConfiguration = OperationContext.Current.IncomingMessageProperties["ServiceConfiguration"] as ServiceConfiguration;
					if (serviceConfiguration != null)
					{
						return serviceConfiguration;
					}
				}
				return new ServiceConfiguration();
			}
			return FederatedAuthentication.ServiceConfiguration;
		}

		private static SecurityTokenResolver GetServiceTokenResolver(ServiceElement element)
		{
			try
			{
				return CustomTypeElement.Resolve<SecurityTokenResolver>(element.ServiceTokenResolver, new object[0]);
			}
			catch (ArgumentException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "serviceTokenResolver", inner);
			}
		}

		private static SecurityTokenResolver GetIssuerTokenResolver(ServiceElement element)
		{
			try
			{
				return CustomTypeElement.Resolve<SecurityTokenResolver>(element.IssuerTokenResolver, new object[0]);
			}
			catch (ArgumentException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "issuerTokenResolver", inner);
			}
		}

		private static ClaimsAuthenticationManager GetClaimsAuthenticationManager(ServiceElement element)
		{
			try
			{
				return CustomTypeElement.Resolve<ClaimsAuthenticationManager>(element.ClaimsAuthenticationManager, new object[0]);
			}
			catch (ArgumentException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "claimsAuthenticationManager", inner);
			}
		}

		private static Microsoft.IdentityModel.Tokens.IssuerNameRegistry GetIssuerNameRegistry(ServiceElement element)
		{
			try
			{
				return CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.IssuerNameRegistry>(element.IssuerNameRegistry, new object[0]);
			}
			catch (ArgumentException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "issuerNameRegistry", inner);
			}
		}

		private static X509Certificate2 GetServiceCertificate(ServiceElement element)
		{
			try
			{
				X509Certificate2 certificate = element.ServiceCertificate.GetCertificate();
				if (certificate != null)
				{
					X509Util.EnsureAndGetPrivateRSAKey(certificate);
				}
				return certificate;
			}
			catch (ArgumentException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "serviceCertificate", inner);
			}
		}

		public SecurityTokenResolver CreateAggregateTokenResolver()
		{
			List<SecurityTokenResolver> list = new List<SecurityTokenResolver>();
			if (_serviceCertificate != null)
			{
				List<SecurityToken> list2 = new List<SecurityToken>(1);
				list2.Add(new X509SecurityToken(_serviceCertificate));
				list.Add(SecurityTokenResolver.CreateDefaultSecurityTokenResolver(list2.AsReadOnly(), canMatchLocalId: false));
			}
			if (_serviceHandlerConfiguration != null && _serviceHandlerConfiguration.ServiceTokenResolver != null && !object.ReferenceEquals(_serviceHandlerConfiguration.ServiceTokenResolver, EmptySecurityTokenResolver.Instance))
			{
				list.Add(_serviceHandlerConfiguration.ServiceTokenResolver);
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			if (list.Count > 1)
			{
				return new Microsoft.IdentityModel.Tokens.AggregateTokenResolver(list);
			}
			return EmptySecurityTokenResolver.Instance;
		}

		public virtual void Initialize()
		{
			if (IsInitialized)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7009"));
			}
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlers = SecurityTokenHandlers;
			if (!object.ReferenceEquals(_serviceHandlerConfiguration, securityTokenHandlers.Configuration))
			{
				DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Information, SR.GetString("ID4283"));
				IsInitialized = true;
				return;
			}
			securityTokenHandlers.Configuration.ServiceTokenResolver = CreateAggregateTokenResolver();
			if (CertificateValidationMode != X509CertificateValidationMode.Custom)
			{
				securityTokenHandlers.Configuration.CertificateValidator = X509Util.CreateCertificateValidator(CertificateValidationMode, RevocationMode, TrustedStoreLocation);
			}
			else if (object.ReferenceEquals(securityTokenHandlers.Configuration.CertificateValidator, Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration.DefaultCertificateValidator))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4280")));
			}
			IsInitialized = true;
		}

		protected void LoadConfiguration(ServiceElement element)
		{
			if (element != null)
			{
				if (element.ClaimsAuthenticationManager.IsConfigured)
				{
					_claimsAuthenticationManager = GetClaimsAuthenticationManager(element);
				}
				if (element.ClaimsAuthorizationManager.IsConfigured)
				{
					_claimsAuthorizationManager = CustomTypeElement.Resolve<ClaimsAuthorizationManager>(element.ClaimsAuthorizationManager, new object[0]);
				}
				if (_serviceCertificate == null && element.ServiceCertificate.IsConfigured)
				{
					_serviceCertificate = GetServiceCertificate(element);
				}
				if (element.CertificateValidationElement.IsConfigured)
				{
					_revocationMode = element.CertificateValidationElement.RevocationMode;
					_certificateValidationMode = element.CertificateValidationElement.ValidationMode;
					_trustedStoreLocation = element.CertificateValidationElement.TrustedStoreLocation;
				}
				_serviceHandlerConfiguration = LoadHandlerConfiguration(element);
			}
			_securityTokenHandlerCollectionManager = LoadHandlers(element);
		}

		protected Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager LoadHandlers(ServiceElement serviceElement)
		{
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager.CreateEmptySecurityTokenHandlerCollectionManager();
			if (serviceElement != null)
			{
				if (serviceElement.SecurityTokenHandlerSets.Count > 0)
				{
					foreach (SecurityTokenHandlerElementCollection securityTokenHandlerSet in serviceElement.SecurityTokenHandlerSets)
					{
						try
						{
							Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration securityTokenHandlerConfiguration;
							if (!string.IsNullOrEmpty(securityTokenHandlerSet.Name) && !StringComparer.Ordinal.Equals(securityTokenHandlerSet.Name, ""))
							{
								securityTokenHandlerConfiguration = ((!securityTokenHandlerSet.HandlerConfiguration.IsConfigured) ? new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration() : LoadHandlerConfiguration(null, securityTokenHandlerSet.HandlerConfiguration));
							}
							else
							{
								if (securityTokenHandlerSet.HandlerConfiguration.IsConfigured)
								{
									_serviceHandlerConfiguration = LoadHandlerConfiguration(serviceElement);
									securityTokenHandlerConfiguration = LoadHandlerConfiguration(_serviceHandlerConfiguration, securityTokenHandlerSet.HandlerConfiguration);
								}
								else
								{
									securityTokenHandlerConfiguration = LoadHandlerConfiguration(serviceElement);
								}
								_serviceHandlerConfiguration = securityTokenHandlerConfiguration;
							}
							Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlerCollection = new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection(securityTokenHandlerConfiguration);
							securityTokenHandlerCollectionManager[securityTokenHandlerSet.Name] = securityTokenHandlerCollection;
							foreach (CustomTypeElement item in securityTokenHandlerSet)
							{
								securityTokenHandlerCollection.Add(CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.SecurityTokenHandler>(item, new object[0]));
							}
						}
						catch (ArgumentException inner)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(serviceElement, securityTokenHandlerSet.Name, inner);
						}
					}
				}
				if (!securityTokenHandlerCollectionManager.ContainsKey(""))
				{
					securityTokenHandlerCollectionManager[""] = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(_serviceHandlerConfiguration);
				}
			}
			else
			{
				_serviceHandlerConfiguration = new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration();
				_serviceHandlerConfiguration.MaxClockSkew = _serviceMaxClockSkew;
				if (!securityTokenHandlerCollectionManager.ContainsKey(""))
				{
					securityTokenHandlerCollectionManager[""] = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(_serviceHandlerConfiguration);
				}
			}
			return securityTokenHandlerCollectionManager;
		}

		protected Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration LoadHandlerConfiguration(ServiceElement element)
		{
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration securityTokenHandlerConfiguration = new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration();
			try
			{
				if (_serviceMaxClockSkew == DefaultMaxClockSkew && element.MaximumClockSkew.IsConfigured)
				{
					securityTokenHandlerConfiguration.MaxClockSkew = TimeSpan.Parse(element.MaximumClockSkew.Value);
				}
				else
				{
					securityTokenHandlerConfiguration.MaxClockSkew = _serviceMaxClockSkew;
				}
			}
			catch (ArgumentException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "maximumClockSkew", inner);
			}
			if (element.AudienceUriElements.IsConfigured)
			{
				securityTokenHandlerConfiguration.AudienceRestriction.AudienceMode = element.AudienceUriElements.Mode;
				foreach (AudienceUriElement audienceUriElement in element.AudienceUriElements)
				{
					securityTokenHandlerConfiguration.AudienceRestriction.AllowedAudienceUris.Add(new Uri(audienceUriElement.Value, UriKind.RelativeOrAbsolute));
				}
			}
			if (element.CertificateValidationElement.IsConfigured && element.CertificateValidationElement.CustomType.IsConfigured)
			{
				securityTokenHandlerConfiguration.CertificateValidator = CustomTypeElement.Resolve<X509CertificateValidator>(element.CertificateValidationElement.CustomType, new object[0]);
			}
			if (element.IssuerNameRegistry.IsConfigured)
			{
				securityTokenHandlerConfiguration.IssuerNameRegistry = GetIssuerNameRegistry(element);
			}
			if (element.IssuerTokenResolver.IsConfigured)
			{
				securityTokenHandlerConfiguration.IssuerTokenResolver = GetIssuerTokenResolver(element);
			}
			if (!string.IsNullOrEmpty(element.SaveBootstrapTokens))
			{
				try
				{
					securityTokenHandlerConfiguration.SaveBootstrapTokens = XmlConvert.ToBoolean(element.SaveBootstrapTokens.ToLowerInvariant());
				}
				catch (FormatException inner2)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "saveBootstrapTokens", inner2);
				}
			}
			if (element.ServiceTokenResolver.IsConfigured)
			{
				securityTokenHandlerConfiguration.ServiceTokenResolver = GetServiceTokenResolver(element);
			}
			if (element.TokenReplayDetectionElement.IsConfigured)
			{
				if (element.TokenReplayDetectionElement.CustomType.IsConfigured)
				{
					securityTokenHandlerConfiguration.TokenReplayCache = CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.TokenReplayCache>(element.TokenReplayDetectionElement.CustomType, new object[0]);
				}
				if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.Enabled))
				{
					try
					{
						securityTokenHandlerConfiguration.DetectReplayedTokens = XmlConvert.ToBoolean(element.TokenReplayDetectionElement.Enabled.ToLowerInvariant());
					}
					catch (FormatException inner3)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "saveBootstrapTokens", inner3);
					}
				}
				if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.ExpirationPeriod))
				{
					securityTokenHandlerConfiguration.TokenReplayCacheExpirationPeriod = TimeSpan.Parse(element.TokenReplayDetectionElement.ExpirationPeriod);
				}
				if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.PurgeInterval))
				{
					securityTokenHandlerConfiguration.TokenReplayCache.PurgeInterval = TimeSpan.Parse(element.TokenReplayDetectionElement.PurgeInterval);
				}
				if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.Capacity))
				{
					securityTokenHandlerConfiguration.TokenReplayCache.Capacity = int.Parse(element.TokenReplayDetectionElement.Capacity, CultureInfo.InvariantCulture);
				}
			}
			return securityTokenHandlerConfiguration;
		}

		protected Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration LoadHandlerConfiguration(Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration baseConfiguration, SecurityTokenHandlerConfigurationElement element)
		{
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration securityTokenHandlerConfiguration = ((baseConfiguration == null) ? new Microsoft.IdentityModel.Tokens.SecurityTokenHandlerConfiguration() : baseConfiguration);
			if (element.AudienceUriElements.IsConfigured)
			{
				securityTokenHandlerConfiguration.AudienceRestriction.AudienceMode = AudienceUriMode.Always;
				securityTokenHandlerConfiguration.AudienceRestriction.AllowedAudienceUris.Clear();
				securityTokenHandlerConfiguration.AudienceRestriction.AudienceMode = element.AudienceUriElements.Mode;
				foreach (AudienceUriElement audienceUriElement in element.AudienceUriElements)
				{
					securityTokenHandlerConfiguration.AudienceRestriction.AllowedAudienceUris.Add(new Uri(audienceUriElement.Value, UriKind.RelativeOrAbsolute));
				}
			}
			if (element.CertificateValidationElement.IsConfigured && element.CertificateValidationElement.CustomType.IsConfigured)
			{
				if (object.ReferenceEquals(baseConfiguration, _serviceHandlerConfiguration))
				{
					RevocationMode = DefaultRevocationMode;
					TrustedStoreLocation = DefaultTrustedStoreLocation;
					CertificateValidationMode = X509CertificateValidationMode.Custom;
				}
				securityTokenHandlerConfiguration.CertificateValidator = CustomTypeElement.Resolve<X509CertificateValidator>(element.CertificateValidationElement.CustomType, new object[0]);
			}
			if (element.IssuerNameRegistry.IsConfigured)
			{
				securityTokenHandlerConfiguration.IssuerNameRegistry = CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.IssuerNameRegistry>(element.IssuerNameRegistry, new object[0]);
			}
			if (element.IssuerTokenResolver.IsConfigured)
			{
				securityTokenHandlerConfiguration.IssuerTokenResolver = CustomTypeElement.Resolve<SecurityTokenResolver>(element.IssuerTokenResolver, new object[0]);
			}
			try
			{
				if (element.MaximumClockSkew.IsConfigured)
				{
					securityTokenHandlerConfiguration.MaxClockSkew = TimeSpan.Parse(element.MaximumClockSkew.Value);
				}
			}
			catch (ArgumentException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "maximumClockSkew", inner);
			}
			if (!string.IsNullOrEmpty(element.SaveBootstrapTokens))
			{
				try
				{
					securityTokenHandlerConfiguration.SaveBootstrapTokens = XmlConvert.ToBoolean(element.SaveBootstrapTokens.ToLowerInvariant());
				}
				catch (FormatException inner2)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "saveBootstrapTokens", inner2);
				}
			}
			if (element.ServiceTokenResolver.IsConfigured)
			{
				securityTokenHandlerConfiguration.ServiceTokenResolver = CustomTypeElement.Resolve<SecurityTokenResolver>(element.ServiceTokenResolver, new object[0]);
			}
			if (element.TokenReplayDetectionElement.IsConfigured)
			{
				if (element.TokenReplayDetectionElement.CustomType.IsConfigured)
				{
					securityTokenHandlerConfiguration.TokenReplayCache = CustomTypeElement.Resolve<Microsoft.IdentityModel.Tokens.TokenReplayCache>(element.TokenReplayDetectionElement.CustomType, new object[0]);
				}
				if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.Enabled))
				{
					try
					{
						securityTokenHandlerConfiguration.DetectReplayedTokens = XmlConvert.ToBoolean(element.TokenReplayDetectionElement.Enabled.ToLowerInvariant());
					}
					catch (FormatException inner3)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperConfigurationError(element, "saveBootstrapTokens", inner3);
					}
				}
				if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.ExpirationPeriod))
				{
					securityTokenHandlerConfiguration.TokenReplayCacheExpirationPeriod = TimeSpan.Parse(element.TokenReplayDetectionElement.ExpirationPeriod);
				}
				if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.PurgeInterval))
				{
					securityTokenHandlerConfiguration.TokenReplayCache.PurgeInterval = TimeSpan.Parse(element.TokenReplayDetectionElement.PurgeInterval);
				}
				if (!string.IsNullOrEmpty(element.TokenReplayDetectionElement.Capacity))
				{
					securityTokenHandlerConfiguration.TokenReplayCache.Capacity = int.Parse(element.TokenReplayDetectionElement.Capacity, CultureInfo.InvariantCulture);
				}
			}
			return securityTokenHandlerConfiguration;
		}
	}
}
