using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class SecurityTokenServiceConfiguration : ServiceConfiguration
	{
		internal const int DefaultKeySizeInBitsConstant = 256;

		private string _tokenIssuerName;

		private SigningCredentials _signingCredentials;

		private TimeSpan _defaultTokenLifetime = TimeSpan.FromHours(1.0);

		private TimeSpan _maximumTokenLifetime = TimeSpan.FromDays(1.0);

		private string _defaultTokenType = "urn:oasis:names:tc:SAML:1.0:assertion";

		private int _defaultSymmetricKeySizeInBits = 256;

		private int _defaultMaxSymmetricKeySizeInBits = 1024;

		private Collection<ServiceHostEndpointConfiguration> _endpoints = new Collection<ServiceHostEndpointConfiguration>();

		private Type _securityTokenServiceType;

		private WSTrust13RequestSerializer _wsTrust13RequestSerializer = new WSTrust13RequestSerializer();

		private WSTrust13ResponseSerializer _wsTrust13ResponseSerializer = new WSTrust13ResponseSerializer();

		private WSTrustFeb2005RequestSerializer _wsTrustFeb2005RequestSerializer = new WSTrustFeb2005RequestSerializer();

		private WSTrustFeb2005ResponseSerializer _wsTrustFeb2005ResponseSerializer = new WSTrustFeb2005ResponseSerializer();

		public Type SecurityTokenService
		{
			get
			{
				return _securityTokenServiceType;
			}
			set
			{
				if ((object)value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				if (!typeof(Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService).IsAssignableFrom(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID2069"));
				}
				_securityTokenServiceType = value;
			}
		}

		public int DefaultSymmetricKeySizeInBits
		{
			get
			{
				return _defaultSymmetricKeySizeInBits;
			}
			set
			{
				if (value <= 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", SR.GetString("ID0002"));
				}
				_defaultSymmetricKeySizeInBits = value;
			}
		}

		public int DefaultMaxSymmetricKeySizeInBits
		{
			get
			{
				return _defaultMaxSymmetricKeySizeInBits;
			}
			set
			{
				if (value <= 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", SR.GetString("ID0002"));
				}
				_defaultMaxSymmetricKeySizeInBits = value;
			}
		}

		public TimeSpan DefaultTokenLifetime
		{
			get
			{
				return _defaultTokenLifetime;
			}
			set
			{
				_defaultTokenLifetime = value;
			}
		}

		public string DefaultTokenType
		{
			get
			{
				return _defaultTokenType;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				if (base.SecurityTokenHandlers[value] == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID2015", value));
				}
				_defaultTokenType = value;
			}
		}

		public TimeSpan MaximumTokenLifetime
		{
			get
			{
				return _maximumTokenLifetime;
			}
			set
			{
				if (value <= TimeSpan.Zero)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", SR.GetString("ID0016"));
				}
				_maximumTokenLifetime = value;
			}
		}

		public SigningCredentials SigningCredentials
		{
			get
			{
				return _signingCredentials;
			}
			set
			{
				_signingCredentials = value;
			}
		}

		public string TokenIssuerName
		{
			get
			{
				return _tokenIssuerName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_tokenIssuerName = value;
			}
		}

		public Collection<ServiceHostEndpointConfiguration> TrustEndpoints => _endpoints;

		public WSTrust13RequestSerializer WSTrust13RequestSerializer
		{
			get
			{
				return _wsTrust13RequestSerializer;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_wsTrust13RequestSerializer = value;
			}
		}

		public WSTrust13ResponseSerializer WSTrust13ResponseSerializer
		{
			get
			{
				return _wsTrust13ResponseSerializer;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_wsTrust13ResponseSerializer = value;
			}
		}

		public WSTrustFeb2005RequestSerializer WSTrustFeb2005RequestSerializer
		{
			get
			{
				return _wsTrustFeb2005RequestSerializer;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_wsTrustFeb2005RequestSerializer = value;
			}
		}

		public WSTrustFeb2005ResponseSerializer WSTrustFeb2005ResponseSerializer
		{
			get
			{
				return _wsTrustFeb2005ResponseSerializer;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_wsTrustFeb2005ResponseSerializer = value;
			}
		}

		public SecurityTokenServiceConfiguration()
			: this(null, null)
		{
		}

		public SecurityTokenServiceConfiguration(bool loadConfig)
			: this(null, null, loadConfig)
		{
		}

		public SecurityTokenServiceConfiguration(string issuerName)
			: this(issuerName, null)
		{
		}

		public SecurityTokenServiceConfiguration(string issuerName, bool loadConfig)
			: this(issuerName, null, loadConfig)
		{
		}

		public SecurityTokenServiceConfiguration(string issuerName, SigningCredentials signingCredentials)
		{
			_tokenIssuerName = issuerName;
			_signingCredentials = signingCredentials;
		}

		public SecurityTokenServiceConfiguration(string issuerName, SigningCredentials signingCredentials, bool loadConfig)
			: base(loadConfig)
		{
			_tokenIssuerName = issuerName;
			_signingCredentials = signingCredentials;
		}

		public SecurityTokenServiceConfiguration(string issuerName, SigningCredentials signingCredentials, string serviceName)
			: base(serviceName)
		{
			_tokenIssuerName = issuerName;
			_signingCredentials = signingCredentials;
		}

		public virtual Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService CreateSecurityTokenService()
		{
			Type securityTokenService = SecurityTokenService;
			if ((object)securityTokenService == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2073"));
			}
			if (!typeof(Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService).IsAssignableFrom(securityTokenService))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2074", securityTokenService, typeof(Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService)));
			}
			return Activator.CreateInstance(securityTokenService, this) as Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService;
		}
	}
}
