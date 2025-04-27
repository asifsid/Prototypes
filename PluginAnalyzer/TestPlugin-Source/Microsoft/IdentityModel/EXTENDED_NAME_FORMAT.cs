namespace Microsoft.IdentityModel
{
	internal enum EXTENDED_NAME_FORMAT
	{
		NameUnknown = 0,
		NameFullyQualifiedDN = 1,
		NameSamCompatible = 2,
		NameDisplay = 3,
		NameUniqueId = 6,
		NameCanonical = 7,
		NameUserPrincipalName = 8,
		NameCanonicalEx = 9,
		NameServicePrincipalName = 10,
		NameDnsDomainName = 12
	}
}
