using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public static class ClaimTypes
	{
		public static class Prip
		{
			public const string ClaimTypeNamespace = "http://schemas.xmlsoap.org/claims";

			public const string CommonName = "http://schemas.xmlsoap.org/claims/CommonName";

			public const string Email = "http://schemas.xmlsoap.org/claims/EmailAddress";

			public const string Group = "http://schemas.xmlsoap.org/claims/Group";

			public const string Upn = "http://schemas.xmlsoap.org/claims/UPN";
		}

		public const string ClaimTypeNamespace = "http://schemas.microsoft.com/ws/2008/06/identity/claims";

		public const string AuthenticationInstant = "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant";

		public const string AuthenticationMethod = "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod";

		public const string CookiePath = "http://schemas.microsoft.com/ws/2008/06/identity/claims/cookiepath";

		public const string DenyOnlyPrimarySid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid";

		public const string DenyOnlyPrimaryGroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid";

		public const string Dsa = "http://schemas.microsoft.com/ws/2008/06/identity/claims/dsa";

		public const string Expiration = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration";

		public const string Expired = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expired";

		public const string GroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";

		public const string IsPersistent = "http://schemas.microsoft.com/ws/2008/06/identity/claims/ispersistent";

		public const string PrimaryGroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid";

		public const string PrimarySid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";

		public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

		public const string SerialNumber = "http://schemas.microsoft.com/ws/2008/06/identity/claims/serialnumber";

		public const string UserData = "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata";

		public const string Version = "http://schemas.microsoft.com/ws/2008/06/identity/claims/version";

		public const string WindowsAccountName = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname";

		public const string ClaimType2005Namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";

		public const string Anonymous = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/anonymous";

		public const string Authentication = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication";

		public const string AuthorizationDecision = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authorizationdecision";

		public const string Country = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country";

		public const string DateOfBirth = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth";

		public const string Dns = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dns";

		public const string DenyOnlySid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid";

		public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

		public const string Gender = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender";

		public const string GivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";

		public const string Hash = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/hash";

		public const string HomePhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone";

		public const string Locality = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality";

		public const string MobilePhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone";

		public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

		public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

		public const string OtherPhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone";

		public const string PostalCode = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode";

		public const string PPID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier";

		public const string Rsa = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa";

		public const string Sid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid";

		public const string Spn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/spn";

		public const string StateOrProvince = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince";

		public const string StreetAddress = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress";

		public const string Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

		public const string System = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/system";

		public const string Thumbprint = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint";

		public const string Upn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";

		public const string Uri = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri";

		public const string Webpage = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage";

		public const string X500DistinguishedName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname";

		public const string ClaimType2009Namespace = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims";

		public const string Actor = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor";
	}
}
