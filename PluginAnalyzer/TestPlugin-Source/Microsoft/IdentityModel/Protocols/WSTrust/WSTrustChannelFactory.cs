using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(false)]
	public class WSTrustChannelFactory : ChannelFactory<IWSTrustChannelContract>
	{
		private class WSTrustChannelLockedProperties
		{
			public TrustVersion TrustVersion;

			public WSTrustSerializationContext Context;

			public WSTrustRequestSerializer RequestSerializer;

			public WSTrustResponseSerializer ResponseSerializer;
		}

		private object _factoryLock = new object();

		private bool _locked;

		private WSTrustChannelLockedProperties _lockedProperties;

		private TrustVersion _trustVersion;

		private SecurityTokenResolver _securityTokenResolver;

		private SecurityTokenResolver _useKeyTokenResolver;

		private Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager();

		private WSTrustRequestSerializer _wsTrustRequestSerializer;

		private WSTrustResponseSerializer _wsTrustResponseSerializer;

		public TrustVersion TrustVersion
		{
			get
			{
				return _trustVersion;
			}
			set
			{
				lock (_factoryLock)
				{
					if (_locked)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3287"));
					}
					_trustVersion = value;
				}
			}
		}

		public Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager SecurityTokenHandlerCollectionManager
		{
			get
			{
				return _securityTokenHandlerCollectionManager;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				lock (_factoryLock)
				{
					if (_locked)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3287"));
					}
					_securityTokenHandlerCollectionManager = value;
				}
			}
		}

		public SecurityTokenResolver SecurityTokenResolver
		{
			get
			{
				return _securityTokenResolver;
			}
			set
			{
				lock (_factoryLock)
				{
					if (_locked)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3287"));
					}
					_securityTokenResolver = value;
				}
			}
		}

		public SecurityTokenResolver UseKeyTokenResolver
		{
			get
			{
				return _useKeyTokenResolver;
			}
			set
			{
				lock (_factoryLock)
				{
					if (_locked)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3287"));
					}
					_useKeyTokenResolver = value;
				}
			}
		}

		public WSTrustRequestSerializer WSTrustRequestSerializer
		{
			get
			{
				return _wsTrustRequestSerializer;
			}
			set
			{
				lock (_factoryLock)
				{
					if (_locked)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3287"));
					}
					_wsTrustRequestSerializer = value;
				}
			}
		}

		public WSTrustResponseSerializer WSTrustResponseSerializer
		{
			get
			{
				return _wsTrustResponseSerializer;
			}
			set
			{
				lock (_factoryLock)
				{
					if (_locked)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3287"));
					}
					_wsTrustResponseSerializer = value;
				}
			}
		}

		public WSTrustChannelFactory()
		{
		}

		public WSTrustChannelFactory(string endpointConfigurationName)
			: base(endpointConfigurationName)
		{
		}

		public WSTrustChannelFactory(Binding binding)
			: base(binding)
		{
		}

		public WSTrustChannelFactory(ServiceEndpoint endpoint)
			: base(endpoint)
		{
		}

		public WSTrustChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		public WSTrustChannelFactory(Binding binding, EndpointAddress remoteAddress)
			: base(binding, remoteAddress)
		{
		}

		public WSTrustChannelFactory(Binding binding, string remoteAddress)
			: base(binding, remoteAddress)
		{
		}

		public override IWSTrustChannelContract CreateChannel(EndpointAddress address, Uri via)
		{
			IWSTrustChannelContract innerChannel = base.CreateChannel(address, via);
			WSTrustChannelLockedProperties lockedProperties = GetLockedProperties();
			return CreateTrustChannel(innerChannel, lockedProperties.TrustVersion, lockedProperties.Context, lockedProperties.RequestSerializer, lockedProperties.ResponseSerializer);
		}

		protected virtual WSTrustChannel CreateTrustChannel(IWSTrustChannelContract innerChannel, TrustVersion trustVersion, WSTrustSerializationContext context, WSTrustRequestSerializer requestSerializer, WSTrustResponseSerializer responseSerializer)
		{
			return new WSTrustChannel(this, innerChannel, trustVersion, context, requestSerializer, responseSerializer);
		}

		private WSTrustChannelLockedProperties GetLockedProperties()
		{
			lock (_factoryLock)
			{
				if (_lockedProperties == null)
				{
					WSTrustChannelLockedProperties wSTrustChannelLockedProperties = new WSTrustChannelLockedProperties();
					wSTrustChannelLockedProperties.TrustVersion = GetTrustVersion();
					wSTrustChannelLockedProperties.Context = CreateSerializationContext();
					wSTrustChannelLockedProperties.RequestSerializer = GetRequestSerializer(wSTrustChannelLockedProperties.TrustVersion);
					wSTrustChannelLockedProperties.ResponseSerializer = GetResponseSerializer(wSTrustChannelLockedProperties.TrustVersion);
					_lockedProperties = wSTrustChannelLockedProperties;
					_locked = true;
				}
				return _lockedProperties;
			}
		}

		private WSTrustRequestSerializer GetRequestSerializer(TrustVersion trustVersion)
		{
			if (_wsTrustRequestSerializer != null)
			{
				return _wsTrustRequestSerializer;
			}
			if (trustVersion == TrustVersion.WSTrust13)
			{
				return new WSTrust13RequestSerializer();
			}
			if (trustVersion == TrustVersion.WSTrustFeb2005)
			{
				return new WSTrustFeb2005RequestSerializer();
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3137", trustVersion.ToString())));
		}

		private WSTrustResponseSerializer GetResponseSerializer(TrustVersion trustVersion)
		{
			if (_wsTrustResponseSerializer != null)
			{
				return _wsTrustResponseSerializer;
			}
			if (trustVersion == TrustVersion.WSTrust13)
			{
				return new WSTrust13ResponseSerializer();
			}
			if (trustVersion == TrustVersion.WSTrustFeb2005)
			{
				return new WSTrustFeb2005ResponseSerializer();
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3137", trustVersion.ToString())));
		}

		private TrustVersion GetTrustVersion()
		{
			TrustVersion trustVersion = _trustVersion;
			if (trustVersion == null)
			{
				BindingElementCollection bindingElementCollection = base.Endpoint.Binding.CreateBindingElements();
				SecurityBindingElement securityBindingElement = bindingElementCollection.Find<SecurityBindingElement>();
				if (securityBindingElement == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3269")));
				}
				trustVersion = securityBindingElement.MessageSecurityVersion.TrustVersion;
			}
			return trustVersion;
		}

		protected virtual WSTrustSerializationContext CreateSerializationContext()
		{
			SecurityTokenResolver securityTokenResolver = _securityTokenResolver;
			if (securityTokenResolver == null)
			{
				ClientCredentials credentials = base.Credentials;
				if (credentials.ClientCertificate != null && credentials.ClientCertificate.Certificate != null)
				{
					List<SecurityToken> list = new List<SecurityToken>();
					list.Add(new X509SecurityToken(credentials.ClientCertificate.Certificate));
					securityTokenResolver = SecurityTokenResolver.CreateDefaultSecurityTokenResolver(list.AsReadOnly(), canMatchLocalId: false);
				}
			}
			if (securityTokenResolver == null)
			{
				securityTokenResolver = EmptySecurityTokenResolver.Instance;
			}
			SecurityTokenResolver useKeyTokenResolver = _useKeyTokenResolver ?? EmptySecurityTokenResolver.Instance;
			return new WSTrustSerializationContext(_securityTokenHandlerCollectionManager, securityTokenResolver, useKeyTokenResolver);
		}
	}
}
