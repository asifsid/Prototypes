using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class KerberosCredential : IUserCredential
	{
		public UserCredentialType CredentialType => UserCredentialType.KerberosV5Credential;
	}
}
