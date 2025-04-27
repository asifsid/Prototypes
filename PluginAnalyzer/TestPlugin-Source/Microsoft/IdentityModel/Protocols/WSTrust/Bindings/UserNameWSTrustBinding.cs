using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
	[ComVisible(true)]
	public class UserNameWSTrustBinding : WSTrustBindingBase
	{
		private HttpClientCredentialType _clientCredentialType;

		public HttpClientCredentialType ClientCredentialType
		{
			get
			{
				return _clientCredentialType;
			}
			set
			{
				if (!IsHttpClientCredentialTypeDefined(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value");
				}
				if (SecurityMode.Transport == base.SecurityMode && HttpClientCredentialType.Digest != value && HttpClientCredentialType.Basic != value)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3225", value));
				}
				_clientCredentialType = value;
			}
		}

		public UserNameWSTrustBinding()
			: this(SecurityMode.Message, HttpClientCredentialType.None)
		{
		}

		public UserNameWSTrustBinding(SecurityMode securityMode)
			: base(securityMode)
		{
			if (SecurityMode.Message == securityMode)
			{
				_clientCredentialType = HttpClientCredentialType.None;
			}
		}

		public UserNameWSTrustBinding(SecurityMode mode, HttpClientCredentialType clientCredentialType)
			: base(mode)
		{
			if (!IsHttpClientCredentialTypeDefined(clientCredentialType))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("clientCredentialType");
			}
			if (SecurityMode.Transport == mode && HttpClientCredentialType.Digest != clientCredentialType && HttpClientCredentialType.Basic != clientCredentialType)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3225", clientCredentialType));
			}
			_clientCredentialType = clientCredentialType;
		}

		private static bool IsHttpClientCredentialTypeDefined(HttpClientCredentialType value)
		{
			if (value != 0 && value != HttpClientCredentialType.Basic && value != HttpClientCredentialType.Digest && value != HttpClientCredentialType.Ntlm && value != HttpClientCredentialType.Windows)
			{
				return value == HttpClientCredentialType.Certificate;
			}
			return true;
		}

		protected override SecurityBindingElement CreateSecurityBindingElement()
		{
			if (SecurityMode.Message == base.SecurityMode)
			{
				return SecurityBindingElement.CreateUserNameForCertificateBindingElement();
			}
			if (SecurityMode.TransportWithMessageCredential == base.SecurityMode)
			{
				return SecurityBindingElement.CreateUserNameOverTransportBindingElement();
			}
			return null;
		}

		protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
		{
			if (_clientCredentialType == HttpClientCredentialType.Basic)
			{
				transport.AuthenticationScheme = AuthenticationSchemes.Basic;
			}
			else
			{
				transport.AuthenticationScheme = AuthenticationSchemes.Digest;
			}
		}
	}
}
