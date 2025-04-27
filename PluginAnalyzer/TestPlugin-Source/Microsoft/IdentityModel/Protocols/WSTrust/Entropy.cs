using System.Runtime.InteropServices;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class Entropy : ProtectedKey
	{
		public Entropy(int entropySizeInBits)
			: this(CryptoUtil.GenerateRandomBytes(entropySizeInBits))
		{
		}

		public Entropy(byte[] secret)
			: base(secret)
		{
		}

		public Entropy(byte[] secret, EncryptingCredentials wrappingCredentials)
			: base(secret, wrappingCredentials)
		{
		}

		public Entropy(ProtectedKey protectedKey)
			: base(GetKeyBytesFromProtectedKey(protectedKey), GetWrappingCredentialsFromProtectedKey(protectedKey))
		{
		}

		private static byte[] GetKeyBytesFromProtectedKey(ProtectedKey protectedKey)
		{
			if (protectedKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("protectedKey");
			}
			return protectedKey.GetKeyBytes();
		}

		private static EncryptingCredentials GetWrappingCredentialsFromProtectedKey(ProtectedKey protectedKey)
		{
			if (protectedKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("protectedKey");
			}
			return protectedKey.WrappingCredentials;
		}
	}
}
