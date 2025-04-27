using System;

namespace Microsoft.IdentityModel.Claims
{
	internal static class AuthenticationTypeMaps
	{
		public struct Mapping
		{
			public string Normalized;

			public string Unnormalized;

			public Mapping(string normalized, string unnormalized)
			{
				Normalized = normalized;
				Unnormalized = unnormalized;
			}
		}

		public static Mapping[] Saml11 = new Mapping[12]
		{
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/hardwaretoken", "URI:urn:oasis:names:tc:SAML:1.0:am:HardwareToken"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/kerberos", "urn:ietf:rfc:1510"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password", "urn:oasis:names:tc:SAML:1.0:am:password"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/pgp", "urn:oasis:names:tc:SAML:1.0:am:PGP"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/secureremotepassword", "urn:ietf:rfc:2945"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/signature", "urn:ietf:rfc:3075"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/spki", "urn:oasis:names:tc:SAML:1.0:am:SPKI"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/tlsclient", "urn:ietf:rfc:2246"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/unspecified", "urn:oasis:names:tc:SAML:1.0:am:unspecified"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows", "urn:federation:authentication:windows"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/x509", "urn:oasis:names:tc:SAML:1.0:am:X509-PKI"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/xkms", "urn:oasis:names:tc:SAML:1.0:am:XKMS")
		};

		public static Mapping[] Saml2 = new Mapping[12]
		{
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/kerberos", "urn:oasis:names:tc:SAML:2.0:ac:classes:Kerberos"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password", "urn:oasis:names:tc:SAML:2.0:ac:classes:Password"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/pgp", "urn:oasis:names:tc:SAML:2.0:ac:classes:PGP"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/secureremotepassword", "urn:oasis:names:tc:SAML:2.0:ac:classes:SecureRemotePassword"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/signature", "urn:oasis:names:tc:SAML:2.0:ac:classes:XMLDSig"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/spki", "urn:oasis:names:tc:SAML:2.0:ac:classes:SPKI"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/smartcard", "urn:oasis:names:tc:SAML:2.0:ac:classes:Smartcard"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/smartcardpki", "urn:oasis:names:tc:SAML:2.0:ac:classes:SmartcardPKI"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/tlsclient", "urn:oasis:names:tc:SAML:2.0:ac:classes:TLSClient"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/unspecified", "urn:oasis:names:tc:SAML:2.0:ac:classes:Unspecified"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/x509", "urn:oasis:names:tc:SAML:2.0:ac:classes:X509"),
			new Mapping("http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows", "urn:federation:authentication:windows")
		};

		public static string Denormalize(string normalizedAuthenticationMethod, Mapping[] mappingTable)
		{
			for (int i = 0; i < mappingTable.Length; i++)
			{
				Mapping mapping = mappingTable[i];
				if (StringComparer.Ordinal.Equals(normalizedAuthenticationMethod, mapping.Normalized))
				{
					return mapping.Unnormalized;
				}
			}
			return normalizedAuthenticationMethod;
		}

		public static string Normalize(string unnormalizedAuthenticationMethod, Mapping[] mappingTable)
		{
			for (int i = 0; i < mappingTable.Length; i++)
			{
				Mapping mapping = mappingTable[i];
				if (StringComparer.Ordinal.Equals(unnormalizedAuthenticationMethod, mapping.Unnormalized))
				{
					return mapping.Normalized;
				}
			}
			return unnormalizedAuthenticationMethod;
		}
	}
}
