using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class IdentityProviderSingleSignOnDescriptor : SingleSignOnDescriptor
	{
		private bool _wantAuthenticationRequestsSigned;

		private Collection<ProtocolEndpoint> _singleSignOnServices = new Collection<ProtocolEndpoint>();

		private Collection<Saml2Attribute> _supportedAttributes = new Collection<Saml2Attribute>();

		public ICollection<ProtocolEndpoint> SingleSignOnServices => _singleSignOnServices;

		public ICollection<Saml2Attribute> SupportedAttributes => _supportedAttributes;

		public bool WantAuthenticationRequestsSigned
		{
			get
			{
				return _wantAuthenticationRequestsSigned;
			}
			set
			{
				_wantAuthenticationRequestsSigned = value;
			}
		}
	}
}
