using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class SignedXml : IDisposable
	{
		private SecurityTokenSerializer _tokenSerializer;

		private Signature _signature;

		private TransformFactory _transformFactory;

		private bool _disposed;

		public string Id
		{
			get
			{
				return _signature.Id;
			}
			set
			{
				_signature.Id = value;
			}
		}

		public SecurityTokenSerializer SecurityTokenSerializer => _tokenSerializer;

		public Signature Signature => _signature;

		public TransformFactory TransformFactory
		{
			get
			{
				return _transformFactory;
			}
			set
			{
				_transformFactory = value;
			}
		}

		public SignedXml(SecurityTokenSerializer securityTokenSerializer)
			: this(new SignedInfo(), securityTokenSerializer)
		{
		}

		internal SignedXml(SignedInfo signedInfo, SecurityTokenSerializer securityTokenSerializer)
		{
			if (signedInfo == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signedInfo");
			}
			if (securityTokenSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenSerializer");
			}
			_transformFactory = TransformFactory.Instance;
			_tokenSerializer = securityTokenSerializer;
			_signature = new Signature(this, signedInfo);
		}

		~SignedXml()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing && !_disposed)
				{
					_signature.Dispose();
				}
				_disposed = true;
			}
		}

		private void ComputeSignature(HashAlgorithm hash, AsymmetricSignatureFormatter formatter, string signatureMethod)
		{
			Signature.SignedInfo.ComputeReferenceDigests();
			Signature.SignedInfo.ComputeHash(hash);
			byte[] signatureValue = ((!StringComparer.Ordinal.Equals("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", signatureMethod)) ? formatter.CreateSignature(hash) : CryptoUtil.CreateSignatureForSha256(formatter, hash));
			Signature.SetSignatureValue(signatureValue);
		}

		private void ComputeSignature(KeyedHashAlgorithm hash)
		{
			Signature.SignedInfo.ComputeReferenceDigests();
			Signature.SignedInfo.ComputeHash(hash);
			byte[] hash2 = hash.Hash;
			Signature.SetSignatureValue(hash2);
		}

		public void ComputeSignature(SecurityKey signingKey)
		{
			string signatureMethod = Signature.SignedInfo.SignatureMethod;
			SymmetricSecurityKey symmetricSecurityKey = signingKey as SymmetricSecurityKey;
			if (symmetricSecurityKey != null)
			{
				using (KeyedHashAlgorithm keyedHashAlgorithm = symmetricSecurityKey.GetKeyedHashAlgorithm(signatureMethod))
				{
					if (keyedHashAlgorithm == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6010", symmetricSecurityKey, signatureMethod)));
					}
					ComputeSignature(keyedHashAlgorithm);
				}
				return;
			}
			AsymmetricSecurityKey asymmetricSecurityKey = signingKey as AsymmetricSecurityKey;
			if (asymmetricSecurityKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6015", signingKey)));
			}
			HashAlgorithm hashAlgorithm;
			AsymmetricSignatureFormatter asymmetricSignatureFormatter;
			if (signatureMethod == "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256")
			{
				hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(signatureMethod);
				asymmetricSignatureFormatter = CryptoUtil.GetSignatureFormatterForSha256(asymmetricSecurityKey);
			}
			else
			{
				hashAlgorithm = asymmetricSecurityKey.GetHashAlgorithmForSignature(signatureMethod);
				if (hashAlgorithm == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6011", signatureMethod)));
				}
				asymmetricSignatureFormatter = asymmetricSecurityKey.GetSignatureFormatter(signatureMethod);
			}
			if (asymmetricSignatureFormatter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6012", signatureMethod, asymmetricSecurityKey)));
			}
			try
			{
				ComputeSignature(hashAlgorithm, asymmetricSignatureFormatter, signatureMethod);
			}
			catch (CryptographicException innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6035", signatureMethod, signingKey.GetType().FullName), innerException));
			}
		}

		public void CompleteSignatureVerification()
		{
			Signature.SignedInfo.EnsureAllReferencesVerified();
		}

		public void EnsureDigestValidity(string id, object resolvedXmlSource)
		{
			Signature.SignedInfo.EnsureDigestValidity(id, resolvedXmlSource);
		}

		public byte[] GetSignatureValue()
		{
			return Signature.GetSignatureBytes();
		}

		public void ReadFrom(XmlDictionaryReader reader)
		{
			_signature.ReadFrom(reader);
		}

		private void VerifySignature(KeyedHashAlgorithm hash)
		{
			Signature.SignedInfo.ComputeHash(hash);
			if (!ByteArrayComparer.Instance.Equals(hash.Hash, GetSignatureValue()))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6013")));
			}
		}

		private void VerifySignature(HashAlgorithm hash, AsymmetricSignatureDeformatter deformatter, string signatureMethod)
		{
			Signature.SignedInfo.ComputeHash(hash);
			if (StringComparer.Ordinal.Equals(signatureMethod, "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"))
			{
				if (!CryptoUtil.VerifySignatureForSha256(deformatter, hash, GetSignatureValue()))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6013")));
				}
			}
			else if (!deformatter.VerifySignature(hash, GetSignatureValue()))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6013")));
			}
		}

		public void StartSignatureVerification(SecurityKey verificationKey)
		{
			string signatureMethod = Signature.SignedInfo.SignatureMethod;
			SymmetricSecurityKey symmetricSecurityKey = verificationKey as SymmetricSecurityKey;
			if (symmetricSecurityKey != null)
			{
				using (KeyedHashAlgorithm keyedHashAlgorithm = symmetricSecurityKey.GetKeyedHashAlgorithm(signatureMethod))
				{
					if (keyedHashAlgorithm == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6014", signatureMethod, symmetricSecurityKey)));
					}
					VerifySignature(keyedHashAlgorithm);
				}
				return;
			}
			AsymmetricSecurityKey asymmetricSecurityKey = verificationKey as AsymmetricSecurityKey;
			if (asymmetricSecurityKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6015", verificationKey)));
			}
			HashAlgorithm hashAlgorithm;
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter;
			if (signatureMethod == "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256")
			{
				hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(signatureMethod);
				asymmetricSignatureDeformatter = CryptoUtil.GetSignatureDeFormatterForSha256(asymmetricSecurityKey);
			}
			else
			{
				hashAlgorithm = asymmetricSecurityKey.GetHashAlgorithmForSignature(signatureMethod);
				if (hashAlgorithm == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6011", signatureMethod)));
				}
				asymmetricSignatureDeformatter = asymmetricSecurityKey.GetSignatureDeformatter(signatureMethod);
			}
			if (asymmetricSignatureDeformatter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6016", signatureMethod, asymmetricSecurityKey)));
			}
			VerifySignature(hashAlgorithm, asymmetricSignatureDeformatter, signatureMethod);
		}

		public void WriteTo(XmlDictionaryWriter writer)
		{
			_signature.WriteTo(writer);
		}
	}
}
