using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class SecurityTokenServiceDescriptor : WebServiceDescriptor
	{
		private Collection<EndpointAddress> _securityTokenServiceEndpoints = new Collection<EndpointAddress>();

		private Collection<EndpointAddress> _passiveRequestorEndpoints = new Collection<EndpointAddress>();

		public Collection<EndpointAddress> SecurityTokenServiceEndpoints => _securityTokenServiceEndpoints;

		public Collection<EndpointAddress> PassiveRequestorEndpoints => _passiveRequestorEndpoints;
	}
}
