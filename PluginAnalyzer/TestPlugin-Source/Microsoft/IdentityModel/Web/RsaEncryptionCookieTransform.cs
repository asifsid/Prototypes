using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class RsaEncryptionCookieTransform : CookieTransform
	{
		private RSA _encryptionKey;

		private List<RSA> _decryptionKeys = new List<RSA>();

		private string _hashName = "SHA256";

		public virtual RSA EncryptionKey
		{
			get
			{
				return _encryptionKey;
			}
			set
			{
				_encryptionKey = value;
				_decryptionKeys = new List<RSA>(new RSA[1] { _encryptionKey });
			}
		}

		protected virtual ReadOnlyCollection<RSA> DecryptionKeys => _decryptionKeys.AsReadOnly();

		public string HashName
		{
			get
			{
				return _hashName;
			}
			set
			{
				using HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(value);
				if (hashAlgorithm == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID6034", value));
				}
				_hashName = value;
			}
		}

		public RsaEncryptionCookieTransform()
		{
		}

		public RsaEncryptionCookieTransform(RSA key)
		{
			if (key == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("key");
			}
			_encryptionKey = key;
			_decryptionKeys.Add(_encryptionKey);
		}

		public RsaEncryptionCookieTransform(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("certificate");
			}
			_encryptionKey = X509Util.EnsureAndGetPrivateRSAKey(certificate);
			_decryptionKeys.Add(_encryptionKey);
		}

		public override byte[] Decode(byte[] encoded)
		{
			if (encoded == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("encoded");
			}
			if (encoded.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("encoded", SR.GetString("ID6045"));
			}
			ReadOnlyCollection<RSA> decryptionKeys = DecryptionKeys;
			if (decryptionKeys.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6039"));
			}
			RSA rSA = null;
			byte[] rgb;
			byte[] array;
			using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(_hashName))
			{
				int count = hashAlgorithm.HashSize / 8;
				byte[] b;
				using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(encoded)))
				{
					b = binaryReader.ReadBytes(count);
					int count2 = binaryReader.ReadInt32();
					rgb = binaryReader.ReadBytes(count2);
					int count3 = binaryReader.ReadInt32();
					array = binaryReader.ReadBytes(count3);
				}
				foreach (RSA item in decryptionKeys)
				{
					byte[] a = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(item.ToXmlString(includePrivateParameters: false)));
					if (CryptoUtil.AreEqual(a, b))
					{
						rSA = item;
						break;
					}
				}
			}
			if (rSA == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6040"));
			}
			RSACryptoServiceProvider rSACryptoServiceProvider = rSA as RSACryptoServiceProvider;
			if (rSACryptoServiceProvider == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6041"));
			}
			byte[] array2 = rSACryptoServiceProvider.Decrypt(rgb, fOAEP: true);
			SymmetricAlgorithm symmetricAlgorithm = CryptoUtil.Algorithms.NewDefaultEncryption();
			byte[] array3 = new byte[symmetricAlgorithm.KeySize / 8];
			if (array2.Length - array3.Length < 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6047"));
			}
			byte[] array4 = new byte[array2.Length - array3.Length];
			Array.Copy(array2, array3, array3.Length);
			Array.Copy(array2, array3.Length, array4, 0, array4.Length);
			ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateDecryptor(array3, array4);
			return cryptoTransform.TransformFinalBlock(array, 0, array.Length);
		}

		public override byte[] Encode(byte[] value)
		{
			if (value == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
			}
			if (value.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID6044"));
			}
			RSA encryptionKey = EncryptionKey;
			if (encryptionKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6043"));
			}
			byte[] buffer;
			using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create(_hashName))
			{
				buffer = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey.ToXmlString(includePrivateParameters: false)));
			}
			byte[] array;
			byte[] array3;
			using (SymmetricAlgorithm symmetricAlgorithm = CryptoUtil.Algorithms.NewDefaultEncryption())
			{
				symmetricAlgorithm.GenerateIV();
				symmetricAlgorithm.GenerateKey();
				ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor();
				array = cryptoTransform.TransformFinalBlock(value, 0, value.Length);
				RSACryptoServiceProvider rSACryptoServiceProvider = encryptionKey as RSACryptoServiceProvider;
				if (rSACryptoServiceProvider == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6041"));
				}
				byte[] array2 = new byte[symmetricAlgorithm.Key.Length + symmetricAlgorithm.IV.Length];
				Array.Copy(symmetricAlgorithm.Key, array2, symmetricAlgorithm.Key.Length);
				Array.Copy(symmetricAlgorithm.IV, 0, array2, symmetricAlgorithm.Key.Length, symmetricAlgorithm.IV.Length);
				array3 = rSACryptoServiceProvider.Encrypt(array2, fOAEP: true);
			}
			using MemoryStream memoryStream = new MemoryStream();
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(buffer);
				binaryWriter.Write(array3.Length);
				binaryWriter.Write(array3);
				binaryWriter.Write(array.Length);
				binaryWriter.Write(array);
				binaryWriter.Flush();
			}
			return memoryStream.ToArray();
		}
	}
}
