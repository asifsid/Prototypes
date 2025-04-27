using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public enum UserCredentialType
	{
		None,
		KerberosV5Credential,
		SelfIssuedCredential,
		UserNamePasswordCredential,
		X509V3Credential
	}
}
