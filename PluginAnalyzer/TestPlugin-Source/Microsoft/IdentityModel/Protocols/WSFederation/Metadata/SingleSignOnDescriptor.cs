using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class SingleSignOnDescriptor : RoleDescriptor
	{
		private IndexedProtocolEndpointDictionary _artifactResolutionServices = new IndexedProtocolEndpointDictionary();

		private Collection<ProtocolEndpoint> _singleLogoutServices = new Collection<ProtocolEndpoint>();

		private Collection<Uri> _nameIdFormats = new Collection<Uri>();

		public ICollection<Uri> NameIdentifierFormats => _nameIdFormats;

		public IndexedProtocolEndpointDictionary ArtifactResolutionServices => _artifactResolutionServices;

		public Collection<ProtocolEndpoint> SingleLogoutServices => _singleLogoutServices;
	}
}
