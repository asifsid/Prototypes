using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
	[ComVisible(true)]
	public class WindowsWSTrustBinding : WSTrustBindingBase
	{
		public WindowsWSTrustBinding()
			: this(SecurityMode.Message)
		{
		}

		public WindowsWSTrustBinding(SecurityMode securityMode)
			: base(securityMode)
		{
		}

		protected override SecurityBindingElement CreateSecurityBindingElement()
		{
			if (SecurityMode.Message == base.SecurityMode)
			{
				return SecurityBindingElement.CreateSspiNegotiationBindingElement(requireCancellation: true);
			}
			if (SecurityMode.TransportWithMessageCredential == base.SecurityMode)
			{
				return SecurityBindingElement.CreateSspiNegotiationOverTransportBindingElement(requireCancellation: true);
			}
			return null;
		}

		protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
		{
			transport.AuthenticationScheme = AuthenticationSchemes.Negotiate;
		}
	}
}
