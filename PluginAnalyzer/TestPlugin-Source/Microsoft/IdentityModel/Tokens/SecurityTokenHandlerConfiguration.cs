using System;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityTokenHandlerConfiguration
	{
		public static readonly bool DefaultDetectReplayedTokens;

		public static readonly IssuerNameRegistry DefaultIssuerNameRegistry = new ConfigurationBasedIssuerNameRegistry();

		public static readonly SecurityTokenResolver DefaultIssuerTokenResolver = Microsoft.IdentityModel.Tokens.IssuerTokenResolver.DefaultInstance;

		public static readonly TimeSpan DefaultMaxClockSkew = new TimeSpan(0, 5, 0);

		public static readonly bool DefaultSaveBootstrapTokens = false;

		public static readonly int DefaultTokenReplayCacheCapacity = 500000;

		public static readonly TimeSpan DefaultTokenReplayCachePurgeInterval = TimeSpan.FromMinutes(1.0);

		public static readonly TimeSpan DefaultTokenReplayCacheExpirationPeriod = TimeSpan.MaxValue;

		public static readonly X509CertificateValidator DefaultCertificateValidator = X509Util.CreateCertificateValidator(ServiceConfiguration.DefaultCertificateValidationMode, ServiceConfiguration.DefaultRevocationMode, ServiceConfiguration.DefaultTrustedStoreLocation);

		private AudienceRestriction _audienceRestriction = new AudienceRestriction();

		private X509CertificateValidator _certificateValidator = DefaultCertificateValidator;

		private bool _detectReplayedTokens = DefaultDetectReplayedTokens;

		private IssuerNameRegistry _issuerNameRegistry = DefaultIssuerNameRegistry;

		private SecurityTokenResolver _issuerTokenResolver = DefaultIssuerTokenResolver;

		private TimeSpan _maxClockSkew = DefaultMaxClockSkew;

		private bool _saveBootstrapTokens = DefaultSaveBootstrapTokens;

		private SecurityTokenResolver _serviceTokenResolver = EmptySecurityTokenResolver.Instance;

		private TokenReplayCache _tokenReplayCache = new DefaultTokenReplayCache(DefaultTokenReplayCacheCapacity, DefaultTokenReplayCachePurgeInterval);

		private TimeSpan _tokenReplayCacheExpirationPeriod = DefaultTokenReplayCacheExpirationPeriod;

		public AudienceRestriction AudienceRestriction
		{
			get
			{
				return _audienceRestriction;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_audienceRestriction = value;
			}
		}

		public X509CertificateValidator CertificateValidator
		{
			get
			{
				return _certificateValidator;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_certificateValidator = value;
			}
		}

		public bool DetectReplayedTokens
		{
			get
			{
				return _detectReplayedTokens;
			}
			set
			{
				_detectReplayedTokens = value;
			}
		}

		public IssuerNameRegistry IssuerNameRegistry
		{
			get
			{
				return _issuerNameRegistry;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_issuerNameRegistry = value;
			}
		}

		public SecurityTokenResolver IssuerTokenResolver
		{
			get
			{
				return _issuerTokenResolver;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_issuerTokenResolver = value;
			}
		}

		public TimeSpan MaxClockSkew
		{
			get
			{
				return _maxClockSkew;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", value, SR.GetString("ID2070"));
				}
				_maxClockSkew = value;
			}
		}

		public bool SaveBootstrapTokens
		{
			get
			{
				return _saveBootstrapTokens;
			}
			set
			{
				_saveBootstrapTokens = value;
			}
		}

		public SecurityTokenResolver ServiceTokenResolver
		{
			get
			{
				return _serviceTokenResolver;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_serviceTokenResolver = value;
			}
		}

		public TokenReplayCache TokenReplayCache
		{
			get
			{
				return _tokenReplayCache;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_tokenReplayCache = value;
			}
		}

		public TimeSpan TokenReplayCacheExpirationPeriod
		{
			get
			{
				return _tokenReplayCacheExpirationPeriod;
			}
			set
			{
				if (value <= TimeSpan.Zero)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", value, SR.GetString("ID0016"));
				}
				_tokenReplayCacheExpirationPeriod = value;
			}
		}
	}
}
