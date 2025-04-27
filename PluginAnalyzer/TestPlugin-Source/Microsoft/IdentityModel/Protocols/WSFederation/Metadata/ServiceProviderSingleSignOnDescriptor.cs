using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class ServiceProviderSingleSignOnDescriptor : SingleSignOnDescriptor
	{
		private bool _authenticationRequestsSigned;

		private bool _wantAssertionsSigned;

		private IndexedProtocolEndpointDictionary _assertionConsumingServices = new IndexedProtocolEndpointDictionary();

		public IndexedProtocolEndpointDictionary AssertionConsumerService => _assertionConsumingServices;

		public bool AuthenticationRequestsSigned
		{
			get
			{
				return _authenticationRequestsSigned;
			}
			set
			{
				_authenticationRequestsSigned = value;
			}
		}

		public bool WantAssertionsSigned
		{
			get
			{
				return _wantAssertionsSigned;
			}
			set
			{
				_wantAssertionsSigned = value;
			}
		}

		public ServiceProviderSingleSignOnDescriptor()
			: this(new IndexedProtocolEndpointDictionary())
		{
		}

		public ServiceProviderSingleSignOnDescriptor(IndexedProtocolEndpointDictionary collection)
		{
			_assertionConsumingServices = collection;
		}
	}
}
