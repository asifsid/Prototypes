using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public interface IUserCredential
	{
		UserCredentialType CredentialType { get; }
	}
}
