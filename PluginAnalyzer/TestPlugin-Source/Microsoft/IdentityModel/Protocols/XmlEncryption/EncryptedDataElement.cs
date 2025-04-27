using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.IdentityModel.Protocols.XmlSignature;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
	internal class EncryptedDataElement : EncryptedTypeElement
	{
		public static bool CanReadFrom(XmlReader reader)
		{
			return reader?.IsStartElement("EncryptedData", "http://www.w3.org/2001/04/xmlenc#") ?? false;
		}

		public EncryptedDataElement()
			: this(null)
		{
		}

		public EncryptedDataElement(SecurityTokenSerializer tokenSerializer)
			: base(tokenSerializer)
		{
			base.KeyIdentifier = new SecurityKeyIdentifier(new Microsoft.IdentityModel.Protocols.XmlSignature.EmptySecurityKeyIdentifierClause());
		}

		public byte[] Decrypt(SymmetricAlgorithm algorithm)
		{
			if (algorithm == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("algorithm");
			}
			if (base.CipherData == null || base.CipherData.CipherValue == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6000")));
			}
			byte[] cipherValue = base.CipherData.CipherValue;
			return ExtractIVAndDecrypt(algorithm, cipherValue, 0, cipherValue.Length);
		}

		public void Encrypt(SymmetricAlgorithm algorithm, byte[] buffer, int offset, int length)
		{
			GenerateIVAndEncrypt(algorithm, buffer, offset, length, out var iv, out var cipherText);
			base.CipherData.SetCipherValueFragments(iv, cipherText);
		}

		private static byte[] ExtractIVAndDecrypt(SymmetricAlgorithm algorithm, byte[] cipherText, int offset, int count)
		{
			byte[] array = new byte[algorithm.BlockSize / 8];
			if (cipherText.Length - offset < array.Length)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6019", cipherText.Length - offset, array.Length)));
			}
			Buffer.BlockCopy(cipherText, offset, array, 0, array.Length);
			algorithm.Padding = PaddingMode.ISO10126;
			algorithm.Mode = CipherMode.CBC;
			ICryptoTransform cryptoTransform = null;
			try
			{
				cryptoTransform = algorithm.CreateDecryptor(algorithm.Key, array);
				return cryptoTransform.TransformFinalBlock(cipherText, offset + array.Length, count - array.Length);
			}
			finally
			{
				cryptoTransform?.Dispose();
			}
		}

		private static void GenerateIVAndEncrypt(SymmetricAlgorithm algorithm, byte[] plainText, int offset, int length, out byte[] iv, out byte[] cipherText)
		{
			RandomNumberGenerator randomNumberGenerator = CryptoUtil.Algorithms.NewRandomNumberGenerator();
			int num = algorithm.BlockSize / 8;
			iv = new byte[num];
			randomNumberGenerator.GetBytes(iv);
			algorithm.Padding = PaddingMode.PKCS7;
			algorithm.Mode = CipherMode.CBC;
			ICryptoTransform cryptoTransform = algorithm.CreateEncryptor(algorithm.Key, iv);
			cipherText = cryptoTransform.TransformFinalBlock(plainText, offset, length);
			cryptoTransform.Dispose();
		}

		public override void ReadExtensions(XmlDictionaryReader reader)
		{
		}

		public override void ReadXml(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			if (!reader.IsStartElement("EncryptedData", "http://www.w3.org/2001/04/xmlenc#"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4193"));
			}
			base.ReadXml(reader);
		}

		public virtual void WriteXml(XmlWriter writer, SecurityTokenSerializer securityTokenSerializer)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (securityTokenSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenSerializer");
			}
			if (base.KeyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6001")));
			}
			writer.WriteStartElement("xenc", "EncryptedData", "http://www.w3.org/2001/04/xmlenc#");
			if (!string.IsNullOrEmpty(base.Id))
			{
				writer.WriteAttributeString("Id", null, base.Id);
			}
			if (!string.IsNullOrEmpty(base.Type))
			{
				writer.WriteAttributeString("Type", null, base.Type);
			}
			if (base.EncryptionMethod != null)
			{
				base.EncryptionMethod.WriteXml(writer);
			}
			if (base.KeyIdentifier != null)
			{
				securityTokenSerializer.WriteKeyIdentifier(XmlDictionaryWriter.CreateDictionaryWriter(writer), base.KeyIdentifier);
			}
			base.CipherData.WriteXml(writer);
			writer.WriteEndElement();
		}
	}
}
