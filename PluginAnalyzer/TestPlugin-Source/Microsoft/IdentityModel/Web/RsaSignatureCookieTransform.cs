using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class RsaSignatureCookieTransform : CookieTransform
	{
		private RSA _signingKey;

		private List<RSA> _verificationKeys = new List<RSA>();

		private string _hashName = "SHA256";

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

		public virtual RSA SigningKey
		{
			get
			{
				return _signingKey;
			}
			set
			{
				_signingKey = value;
				_verificationKeys = new List<RSA>(new RSA[1] { _signingKey });
			}
		}

		protected virtual ReadOnlyCollection<RSA> VerificationKeys => _verificationKeys.AsReadOnly();

		public RsaSignatureCookieTransform()
		{
		}

		public RsaSignatureCookieTransform(RSA key)
		{
			if (key == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("key");
			}
			_signingKey = key;
			_verificationKeys.Add(_signingKey);
		}

		public RsaSignatureCookieTransform(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("certificate");
			}
			_signingKey = X509Util.EnsureAndGetPrivateRSAKey(certificate);
			_verificationKeys.Add(_signingKey);
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
			ReadOnlyCollection<RSA> verificationKeys = VerificationKeys;
			if (verificationKeys.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6036"));
			}
			int num = 0;
			if (encoded.Length < 4)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new FormatException(SR.GetString("ID1012")));
			}
			int num2 = BitConverter.ToInt32(encoded, num);
			if (num2 < 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new FormatException(SR.GetString("ID1005", num2)));
			}
			if (num2 >= encoded.Length - 4)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new FormatException(SR.GetString("ID1013")));
			}
			num += 4;
			byte[] array = new byte[num2];
			Array.Copy(encoded, num, array, 0, array.Length);
			num += array.Length;
			byte[] array2 = new byte[encoded.Length - num];
			Array.Copy(encoded, num, array2, 0, array2.Length);
			bool flag = false;
			try
			{
				using HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(HashName);
				hashAlgorithm.ComputeHash(array2);
				foreach (RSA item in verificationKeys)
				{
					AsymmetricSignatureDeformatter signatureDeformatter = GetSignatureDeformatter(item);
					if ((isSha256() && CryptoUtil.VerifySignatureForSha256(signatureDeformatter, hashAlgorithm, array)) || signatureDeformatter.VerifySignature(hashAlgorithm, array))
					{
						flag = true;
						break;
					}
				}
			}
			catch (CryptographicException innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6035", HashName, verificationKeys[0].GetType().FullName), innerException));
			}
			if (!flag)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID1014")));
			}
			return array2;
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
			RSA signingKey = SigningKey;
			RSACryptoServiceProvider rSACryptoServiceProvider = signingKey as RSACryptoServiceProvider;
			if (signingKey == null || rSACryptoServiceProvider == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6042"));
			}
			if (rSACryptoServiceProvider.PublicOnly)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID6046"));
			}
			byte[] array;
			using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(HashName))
			{
				try
				{
					hashAlgorithm.ComputeHash(value);
					AsymmetricSignatureFormatter signatureFormatter = GetSignatureFormatter(signingKey);
					array = ((!isSha256()) ? signatureFormatter.CreateSignature(hashAlgorithm) : CryptoUtil.CreateSignatureForSha256(signatureFormatter, hashAlgorithm));
				}
				catch (CryptographicException innerException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6035", HashName, signingKey.GetType().FullName), innerException));
				}
			}
			byte[] bytes = BitConverter.GetBytes(array.Length);
			int num = 0;
			byte[] array2 = new byte[bytes.Length + array.Length + value.Length];
			Array.Copy(bytes, 0, array2, num, bytes.Length);
			num += bytes.Length;
			Array.Copy(array, 0, array2, num, array.Length);
			num += array.Length;
			Array.Copy(value, 0, array2, num, value.Length);
			return array2;
		}

		private AsymmetricSignatureFormatter GetSignatureFormatter(RSA rsa)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = rsa as RSACryptoServiceProvider;
			if (isSha256() && rSACryptoServiceProvider != null)
			{
				return CryptoUtil.GetSignatureFormatterForSha256(rSACryptoServiceProvider);
			}
			return new RSAPKCS1SignatureFormatter(rsa);
		}

		private AsymmetricSignatureDeformatter GetSignatureDeformatter(RSA rsa)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = rsa as RSACryptoServiceProvider;
			if (isSha256() && rSACryptoServiceProvider != null)
			{
				return CryptoUtil.GetSignatureDeFormatterForSha256(rSACryptoServiceProvider);
			}
			return new RSAPKCS1SignatureDeformatter(rsa);
		}

		private bool isSha256()
		{
			if (!StringComparer.OrdinalIgnoreCase.Equals(HashName, "SHA256") && !StringComparer.OrdinalIgnoreCase.Equals(HashName, "SHA-256"))
			{
				return StringComparer.OrdinalIgnoreCase.Equals(HashName, "System.Security.Cryptography.SHA256");
			}
			return true;
		}
	}
}
