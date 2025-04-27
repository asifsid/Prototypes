using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public static class Saml2MetadataConstants
	{
		public static class Attributes
		{
			public const string Id = "ID";

			public const string ContactType = "contactType";

			public const string Algorithm = "Algorithm";

			public const string Use = "use";

			public const string Binding = "Binding";

			public const string EndpointIndex = "index";

			public const string EndpointIsDefault = "isDefault";

			public const string Location = "Location";

			public const string ResponseLocation = "ResponseLocation";

			public const string EntityId = "entityID";

			public const string ErrorUrl = "errorURL";

			public const string ProtocolsSupported = "protocolSupportEnumeration";

			public const string ValidUntil = "validUntil";

			public const string EntityGroupName = "Name";

			public const string ServiceDescription = "ServiceDescription";

			public const string ServiceDisplayName = "ServiceDisplayName";

			public const string WantAuthenticationRequestsSigned = "WantAuthnRequestsSigned";

			public const string AuthenticationRequestsSigned = "AuthnRequestsSigned";

			public const string WantAssertionsSigned = "WantAssertionsSigned";
		}

		public static class Elements
		{
			public const string EntitiesDescriptor = "EntitiesDescriptor";

			public const string EntityDescriptor = "EntityDescriptor";

			public const string IdpssoDescriptor = "IDPSSODescriptor";

			public const string RoleDescriptor = "RoleDescriptor";

			public const string SpssoDescriptor = "SPSSODescriptor";

			public const string Company = "Company";

			public const string ContactPerson = "ContactPerson";

			public const string EmailAddress = "EmailAddress";

			public const string GivenName = "GivenName";

			public const string Surname = "SurName";

			public const string TelephoneNumber = "TelephoneNumber";

			public const string Organization = "Organization";

			public const string OrganizationDisplayName = "OrganizationDisplayName";

			public const string OrganizationName = "OrganizationName";

			public const string OrganizationUrl = "OrganizationURL";

			public const string EncryptionMethod = "EncryptionMethod";

			public const string KeyDescriptor = "KeyDescriptor";

			public const string ArtifactResolutionService = "ArtifactResolutionService";

			public const string NameIDFormat = "NameIDFormat";

			public const string SingleLogoutService = "SingleLogoutService";

			public const string SingleSignOnService = "SingleSignOnService";

			public const string AssertionConsumerService = "AssertionConsumerService";
		}

		public const string Namespace = "urn:oasis:names:tc:SAML:2.0:metadata";
	}
}
