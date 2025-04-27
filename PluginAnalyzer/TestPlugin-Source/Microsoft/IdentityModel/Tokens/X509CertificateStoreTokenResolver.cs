using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class X509CertificateStoreTokenResolver : SecurityTokenResolver
	{
		private string _storeName;

		private StoreLocation _storeLocation;

		public string StoreName => _storeName;

		public StoreLocation StoreLocation => _storeLocation;

		public X509CertificateStoreTokenResolver()
			: this(System.Security.Cryptography.X509Certificates.StoreName.My, StoreLocation.LocalMachine)
		{
		}

		public X509CertificateStoreTokenResolver(StoreName storeName, StoreLocation storeLocation)
			: this(Enum.GetName(typeof(StoreName), storeName), storeLocation)
		{
		}

		public X509CertificateStoreTokenResolver(string storeName, StoreLocation storeLocation)
		{
			if (string.IsNullOrEmpty(storeName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("storeName");
			}
			_storeName = storeName;
			_storeLocation = storeLocation;
		}

		protected override bool TryResolveSecurityKeyCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key)
		{
			if (keyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
			}
			key = null;
			EncryptedKeyIdentifierClause encryptedKeyIdentifierClause = keyIdentifierClause as EncryptedKeyIdentifierClause;
			if (encryptedKeyIdentifierClause != null)
			{
				SecurityKeyIdentifier encryptingKeyIdentifier = encryptedKeyIdentifierClause.EncryptingKeyIdentifier;
				if (encryptingKeyIdentifier != null && encryptingKeyIdentifier.Count > 0)
				{
					for (int i = 0; i < encryptingKeyIdentifier.Count; i++)
					{
						SecurityKey key2 = null;
						if (TryResolveSecurityKey(encryptingKeyIdentifier[i], out key2))
						{
							byte[] encryptedKey = encryptedKeyIdentifierClause.GetEncryptedKey();
							string encryptionMethod = encryptedKeyIdentifierClause.EncryptionMethod;
							byte[] symmetricKey = key2.DecryptKey(encryptionMethod, encryptedKey);
							key = new InMemorySymmetricSecurityKey(symmetricKey, cloneBuffer: false);
							return true;
						}
					}
				}
			}
			else
			{
				SecurityToken token = null;
				if (TryResolveToken(keyIdentifierClause, out token) && token.SecurityKeys.Count > 0)
				{
					key = token.SecurityKeys[0];
					return true;
				}
			}
			return false;
		}

		protected override bool TryResolveTokenCore(SecurityKeyIdentifier keyIdentifier, out SecurityToken token)
		{
			if (keyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifier");
			}
			token = null;
			foreach (SecurityKeyIdentifierClause item in keyIdentifier)
			{
				if (TryResolveToken(item, out token))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool TryResolveTokenCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityToken token)
		{
			if (keyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
			}
			token = null;
			X509Store x509Store = null;
			X509Certificate2Collection x509Certificate2Collection = null;
			try
			{
				x509Store = new X509Store(_storeName, _storeLocation);
				x509Store.Open(OpenFlags.ReadOnly);
				x509Certificate2Collection = x509Store.Certificates;
				X509Certificate2Enumerator enumerator = x509Certificate2Collection.GetEnumerator();
				while (enumerator.MoveNext())
				{
					X509Certificate2 current = enumerator.Current;
					X509ThumbprintKeyIdentifierClause x509ThumbprintKeyIdentifierClause = keyIdentifierClause as X509ThumbprintKeyIdentifierClause;
					if (x509ThumbprintKeyIdentifierClause != null && x509ThumbprintKeyIdentifierClause.Matches(current))
					{
						token = new X509SecurityToken(current);
						return true;
					}
					X509IssuerSerialKeyIdentifierClause x509IssuerSerialKeyIdentifierClause = keyIdentifierClause as X509IssuerSerialKeyIdentifierClause;
					if (x509IssuerSerialKeyIdentifierClause != null && x509IssuerSerialKeyIdentifierClause.Matches(current))
					{
						token = new X509SecurityToken(current);
						return true;
					}
					X509SubjectKeyIdentifierClause x509SubjectKeyIdentifierClause = keyIdentifierClause as X509SubjectKeyIdentifierClause;
					if (x509SubjectKeyIdentifierClause != null && x509SubjectKeyIdentifierClause.Matches(current))
					{
						token = new X509SecurityToken(current);
						return true;
					}
					X509RawDataKeyIdentifierClause x509RawDataKeyIdentifierClause = keyIdentifierClause as X509RawDataKeyIdentifierClause;
					if (x509RawDataKeyIdentifierClause != null && x509RawDataKeyIdentifierClause.Matches(current))
					{
						token = new X509SecurityToken(current);
						return true;
					}
				}
			}
			finally
			{
				if (x509Certificate2Collection != null)
				{
					for (int i = 0; i < x509Certificate2Collection.Count; i++)
					{
						x509Certificate2Collection[i].Reset();
					}
				}
				x509Store?.Close();
			}
			return false;
		}
	}
}
