using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
	[ComVisible(true)]
	public abstract class WSTrustBindingBase : Binding
	{
		private SecurityMode _securityMode = SecurityMode.Message;

		private TrustVersion _trustVersion = TrustVersion.WSTrust13;

		private bool _enableRsaProofKeys;

		public bool EnableRsaProofKeys
		{
			get
			{
				return _enableRsaProofKeys;
			}
			set
			{
				_enableRsaProofKeys = value;
			}
		}

		public SecurityMode SecurityMode
		{
			get
			{
				return _securityMode;
			}
			set
			{
				ValidateSecurityMode(value);
				_securityMode = value;
			}
		}

		public TrustVersion TrustVersion
		{
			get
			{
				return _trustVersion;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				ValidateTrustVersion(value);
				_trustVersion = value;
			}
		}

		public override string Scheme
		{
			get
			{
				TransportBindingElement transportBindingElement = CreateBindingElements().Find<TransportBindingElement>();
				if (transportBindingElement == null)
				{
					return string.Empty;
				}
				return transportBindingElement.Scheme;
			}
		}

		protected WSTrustBindingBase(SecurityMode securityMode)
			: this(securityMode, TrustVersion.WSTrust13)
		{
		}

		protected WSTrustBindingBase(SecurityMode securityMode, TrustVersion trustVersion)
		{
			if (trustVersion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustVersion");
			}
			ValidateTrustVersion(trustVersion);
			ValidateSecurityMode(securityMode);
			_securityMode = securityMode;
			_trustVersion = trustVersion;
		}

		public override BindingElementCollection CreateBindingElements()
		{
			BindingElementCollection bindingElementCollection = new BindingElementCollection();
			bindingElementCollection.Clear();
			if (SecurityMode.Message == _securityMode || SecurityMode.TransportWithMessageCredential == _securityMode)
			{
				bindingElementCollection.Add(ApplyMessageSecurity(CreateSecurityBindingElement()));
			}
			bindingElementCollection.Add(CreateEncodingBindingElement());
			bindingElementCollection.Add(CreateTransportBindingElement());
			return bindingElementCollection.Clone();
		}

		protected abstract SecurityBindingElement CreateSecurityBindingElement();

		protected virtual MessageEncodingBindingElement CreateEncodingBindingElement()
		{
			TextMessageEncodingBindingElement textMessageEncodingBindingElement = new TextMessageEncodingBindingElement();
			textMessageEncodingBindingElement.ReaderQuotas.MaxArrayLength = 2097152;
			textMessageEncodingBindingElement.ReaderQuotas.MaxStringContentLength = 2097152;
			return textMessageEncodingBindingElement;
		}

		protected static void ValidateSecurityMode(SecurityMode securityMode)
		{
			if (securityMode != 0 && securityMode != SecurityMode.Message && securityMode != SecurityMode.Transport && securityMode != SecurityMode.TransportWithMessageCredential)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("securityMode");
			}
			if (securityMode == SecurityMode.None)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3224"));
			}
		}

		protected void ValidateTrustVersion(TrustVersion trustVersion)
		{
			if (trustVersion != TrustVersion.WSTrust13 && trustVersion != TrustVersion.WSTrustFeb2005)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("trustVersion");
			}
		}

		protected virtual HttpTransportBindingElement CreateTransportBindingElement()
		{
			HttpTransportBindingElement httpTransportBindingElement = ((SecurityMode.Message != _securityMode) ? new HttpsTransportBindingElement() : new HttpTransportBindingElement());
			httpTransportBindingElement.MaxReceivedMessageSize = 2097152L;
			if (SecurityMode.Transport == _securityMode)
			{
				ApplyTransportSecurity(httpTransportBindingElement);
			}
			return httpTransportBindingElement;
		}

		protected abstract void ApplyTransportSecurity(HttpTransportBindingElement transport);

		protected virtual SecurityBindingElement ApplyMessageSecurity(SecurityBindingElement securityBindingElement)
		{
			if (securityBindingElement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityBindingElement");
			}
			if (TrustVersion.WSTrustFeb2005 == _trustVersion)
			{
				securityBindingElement.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10;
			}
			else
			{
				securityBindingElement.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;
			}
			if (_enableRsaProofKeys)
			{
				RsaSecurityTokenParameters rsaSecurityTokenParameters = new RsaSecurityTokenParameters();
				rsaSecurityTokenParameters.InclusionMode = SecurityTokenInclusionMode.Never;
				rsaSecurityTokenParameters.RequireDerivedKeys = false;
				securityBindingElement.OptionalEndpointSupportingTokenParameters.Endorsing.Add(rsaSecurityTokenParameters);
			}
			return securityBindingElement;
		}
	}
}
