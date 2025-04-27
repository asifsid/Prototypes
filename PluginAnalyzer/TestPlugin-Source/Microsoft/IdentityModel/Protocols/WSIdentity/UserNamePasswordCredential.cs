using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class UserNamePasswordCredential : IUserCredential
	{
		private string _userName;

		public string UserName => _userName;

		public UserCredentialType CredentialType => UserCredentialType.UserNamePasswordCredential;

		public UserNamePasswordCredential()
		{
		}

		public UserNamePasswordCredential(string userName)
		{
			_userName = userName;
		}
	}
}
