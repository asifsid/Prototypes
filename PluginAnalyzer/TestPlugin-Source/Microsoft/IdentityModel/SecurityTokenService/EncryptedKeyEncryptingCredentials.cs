using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class EncryptedKeyEncryptingCredentials : EncryptingCredentials
	{
		private EncryptingCredentials _wrappingCredentials;

		private byte[] _keyBytes;

		public EncryptingCredentials WrappingCredentials => _wrappingCredentials;

		public EncryptedKeyEncryptingCredentials(X509Certificate2 certificate)
			: this(new X509EncryptingCredentials(certificate), 256, "http://www.w3.org/2001/04/xmlenc#aes256-cbc")
		{
		}

		public EncryptedKeyEncryptingCredentials(X509Certificate2 certificate, string keyWrappingAlgorithm, int keySizeInBits, string encryptionAlgorithm)
			: this(new X509EncryptingCredentials(certificate, keyWrappingAlgorithm), keySizeInBits, encryptionAlgorithm)
		{
		}

		public EncryptedKeyEncryptingCredentials(EncryptingCredentials wrappingCredentials, int keySizeInBits, string encryptionAlgorithm)
		{
			if (wrappingCredentials == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wrappingCredentials");
			}
			switch (encryptionAlgorithm)
			{
			case "http://www.w3.org/2001/04/xmlenc#des-cbc":
			case "http://www.w3.org/2001/04/xmlenc#tripledes-cbc":
			case "http://www.w3.org/2001/04/xmlenc#kw-tripledes":
				_keyBytes = KeyGenerator.GenerateDESKey(keySizeInBits);
				break;
			default:
				_keyBytes = KeyGenerator.GenerateSymmetricKey(keySizeInBits);
				break;
			}
			base.SecurityKey = new InMemorySymmetricSecurityKey(_keyBytes);
			_wrappingCredentials = wrappingCredentials;
			byte[] encryptedKey = _wrappingCredentials.SecurityKey.EncryptKey(_wrappingCredentials.Algorithm, _keyBytes);
			base.SecurityKeyIdentifier = new SecurityKeyIdentifier(new EncryptedKeyIdentifierClause(encryptedKey, _wrappingCredentials.Algorithm, _wrappingCredentials.SecurityKeyIdentifier));
			base.Algorithm = encryptionAlgorithm;
		}
	}
}
