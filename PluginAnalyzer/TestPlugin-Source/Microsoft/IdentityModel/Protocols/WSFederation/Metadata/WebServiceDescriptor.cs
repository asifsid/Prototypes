using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ServiceModel;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public abstract class WebServiceDescriptor : RoleDescriptor
	{
		private Collection<DisplayClaim> _claimTypesOffered = new Collection<DisplayClaim>();

		private Collection<DisplayClaim> _claimTypesRequested = new Collection<DisplayClaim>();

		private string _serviceDisplayName;

		private string _serviceDescription;

		private Collection<EndpointAddress> _targetScopes = new Collection<EndpointAddress>();

		private Collection<Uri> _tokenTypesOffered = new Collection<Uri>();

		public ICollection<DisplayClaim> ClaimTypesOffered => _claimTypesOffered;

		public ICollection<DisplayClaim> ClaimTypesRequested => _claimTypesRequested;

		public string ServiceDescription
		{
			get
			{
				return _serviceDescription;
			}
			set
			{
				_serviceDescription = value;
			}
		}

		public string ServiceDisplayName
		{
			get
			{
				return _serviceDisplayName;
			}
			set
			{
				_serviceDisplayName = value;
			}
		}

		public ICollection<EndpointAddress> TargetScopes => _targetScopes;

		public ICollection<Uri> TokenTypesOffered => _tokenTypesOffered;
	}
}
