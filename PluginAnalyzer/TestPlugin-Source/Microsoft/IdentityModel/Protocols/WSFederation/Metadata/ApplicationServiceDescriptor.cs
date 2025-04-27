using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class ApplicationServiceDescriptor : WebServiceDescriptor
	{
		private Collection<EndpointAddress> _endpoints = new Collection<EndpointAddress>();

		private Collection<EndpointAddress> _passiveRequestorEndpoints = new Collection<EndpointAddress>();

		public ICollection<EndpointAddress> Endpoints => _endpoints;

		public ICollection<EndpointAddress> PassiveRequestorEndpoints => _passiveRequestorEndpoints;
	}
}
