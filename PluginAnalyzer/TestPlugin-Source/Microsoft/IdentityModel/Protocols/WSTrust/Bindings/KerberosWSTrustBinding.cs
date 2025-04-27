using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
	[ComVisible(true)]
	public class KerberosWSTrustBinding : WSTrustBindingBase
	{
		public KerberosWSTrustBinding()
			: this(SecurityMode.TransportWithMessageCredential)
		{
		}

		public KerberosWSTrustBinding(SecurityMode mode)
			: base(mode)
		{
		}

		protected override SecurityBindingElement CreateSecurityBindingElement()
		{
			if (SecurityMode.Message == base.SecurityMode)
			{
				return SecurityBindingElement.CreateKerberosBindingElement();
			}
			if (SecurityMode.TransportWithMessageCredential == base.SecurityMode)
			{
				return SecurityBindingElement.CreateKerberosOverTransportBindingElement();
			}
			return null;
		}

		protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
		{
			transport.AuthenticationScheme = AuthenticationSchemes.Negotiate;
		}
	}
}
