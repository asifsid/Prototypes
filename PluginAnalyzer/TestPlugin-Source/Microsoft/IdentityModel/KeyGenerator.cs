using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public static class KeyGenerator
	{
		private const int _maxKeyIterations = 20;

		private static RandomNumberGenerator _random = CryptoUtil.Algorithms.NewRandomNumberGenerator();

		public static byte[] ComputeCombinedKey(byte[] requestorEntropy, byte[] issuerEntropy, int keySizeInBits)
		{
			if (requestorEntropy == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestorEntropy");
			}
			if (issuerEntropy == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("issuerEntropy");
			}
			int num = ValidateKeySizeInBytes(keySizeInBits);
			byte[] array = new byte[num];
			using KeyedHashAlgorithm keyedHashAlgorithm = CryptoUtil.Algorithms.NewHmacSha1();
			keyedHashAlgorithm.Key = requestorEntropy;
			byte[] array2 = issuerEntropy;
			byte[] array3 = new byte[keyedHashAlgorithm.HashSize / 8 + array2.Length];
			byte[] array4 = null;
			try
			{
				int num2 = 0;
				while (num2 < num)
				{
					keyedHashAlgorithm.Initialize();
					array2 = keyedHashAlgorithm.ComputeHash(array2);
					array2.CopyTo(array3, 0);
					issuerEntropy.CopyTo(array3, array2.Length);
					keyedHashAlgorithm.Initialize();
					array4 = keyedHashAlgorithm.ComputeHash(array3);
					for (int i = 0; i < array4.Length; i++)
					{
						if (num2 >= num)
						{
							break;
						}
						array[num2++] = array4[i];
					}
				}
				return array;
			}
			catch
			{
				Array.Clear(array, 0, array.Length);
				throw;
			}
			finally
			{
				if (array4 != null)
				{
					Array.Clear(array4, 0, array4.Length);
				}
				Array.Clear(array3, 0, array3.Length);
				keyedHashAlgorithm.Clear();
			}
		}

		public static SecurityKeyIdentifier GetSecurityKeyIdentifier(byte[] secret, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials wrappingCredentials)
		{
			if (secret == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("secret");
			}
			if (secret.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("secret", SR.GetString("ID6031"));
			}
			if (wrappingCredentials == null || wrappingCredentials.SecurityKey == null)
			{
				return new SecurityKeyIdentifier(new BinarySecretKeyIdentifierClause(secret));
			}
			byte[] encryptedKey = wrappingCredentials.SecurityKey.EncryptKey(wrappingCredentials.Algorithm, secret);
			return new SecurityKeyIdentifier(new EncryptedKeyIdentifierClause(encryptedKey, wrappingCredentials.Algorithm, wrappingCredentials.SecurityKeyIdentifier));
		}

		public static byte[] GenerateSymmetricKey(int keySizeInBits)
		{
			int num = ValidateKeySizeInBytes(keySizeInBits);
			byte[] array = new byte[num];
			CryptoUtil.GenerateRandomBytes(array);
			return array;
		}

		public static byte[] GenerateSymmetricKey(int keySizeInBits, byte[] senderEntropy, out byte[] receiverEntropy)
		{
			if (senderEntropy == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("senderEntropy");
			}
			int num = ValidateKeySizeInBytes(keySizeInBits);
			receiverEntropy = new byte[num];
			_random.GetNonZeroBytes(receiverEntropy);
			return ComputeCombinedKey(senderEntropy, receiverEntropy, keySizeInBits);
		}

		public static byte[] GenerateDESKey(int keySizeInBits)
		{
			int num = ValidateKeySizeInBytes(keySizeInBits);
			byte[] array = new byte[num];
			int num2 = 0;
			do
			{
				if (num2 > 20)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6048", 20)));
				}
				CryptoUtil.GenerateRandomBytes(array);
				num2++;
			}
			while (TripleDES.IsWeakKey(array));
			return array;
		}

		public static byte[] GenerateDESKey(int keySizeInBits, byte[] senderEntropy, out byte[] receiverEntropy)
		{
			int num = ValidateKeySizeInBytes(keySizeInBits);
			_ = new byte[num];
			int num2 = 0;
			byte[] array;
			do
			{
				if (num2 > 20)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6048", 20)));
				}
				receiverEntropy = new byte[num];
				_random.GetNonZeroBytes(receiverEntropy);
				array = ComputeCombinedKey(senderEntropy, receiverEntropy, keySizeInBits);
				num2++;
			}
			while (TripleDES.IsWeakKey(array));
			return array;
		}

		private static int ValidateKeySizeInBytes(int keySizeInBits)
		{
			int num = keySizeInBits / 8;
			if (keySizeInBits <= 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentOutOfRangeException("keySizeInBits", SR.GetString("ID6033", keySizeInBits)));
			}
			if (num * 8 != keySizeInBits)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID6002", keySizeInBits), "keySizeInBits"));
			}
			return num;
		}
	}
}
