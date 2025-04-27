using System.Runtime.InteropServices;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class ProtectedKey
	{
		private byte[] _secret;

		private EncryptingCredentials _wrappingCredentials;

		public EncryptingCredentials WrappingCredentials => _wrappingCredentials;

		public ProtectedKey(byte[] secret)
		{
			_secret = secret;
		}

		public ProtectedKey(byte[] secret, EncryptingCredentials wrappingCredentials)
		{
			_secret = secret;
			_wrappingCredentials = wrappingCredentials;
		}

		public byte[] GetKeyBytes()
		{
			return _secret;
		}
	}
}
