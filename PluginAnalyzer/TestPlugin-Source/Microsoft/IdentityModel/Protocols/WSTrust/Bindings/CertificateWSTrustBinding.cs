using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
	[ComVisible(true)]
	public class CertificateWSTrustBinding : WSTrustBindingBase
	{
		public CertificateWSTrustBinding()
			: this(SecurityMode.Message)
		{
		}

		public CertificateWSTrustBinding(SecurityMode securityMode)
			: base(securityMode)
		{
		}

		protected override SecurityBindingElement CreateSecurityBindingElement()
		{
			if (SecurityMode.Message == base.SecurityMode)
			{
				return SecurityBindingElement.CreateMutualCertificateBindingElement();
			}
			if (SecurityMode.TransportWithMessageCredential == base.SecurityMode)
			{
				return SecurityBindingElement.CreateCertificateOverTransportBindingElement();
			}
			return null;
		}

		protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
		{
			transport.AuthenticationScheme = AuthenticationSchemes.Anonymous;
			HttpsTransportBindingElement httpsTransportBindingElement = transport as HttpsTransportBindingElement;
			if (httpsTransportBindingElement != null)
			{
				httpsTransportBindingElement.RequireClientCertificate = true;
			}
		}
	}
}
