using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class Participants
	{
		private EndpointAddress _primary;

		private List<EndpointAddress> _participant = new List<EndpointAddress>();

		public EndpointAddress Primary
		{
			get
			{
				return _primary;
			}
			set
			{
				_primary = value;
			}
		}

		public List<EndpointAddress> Participant => _participant;
	}
}
