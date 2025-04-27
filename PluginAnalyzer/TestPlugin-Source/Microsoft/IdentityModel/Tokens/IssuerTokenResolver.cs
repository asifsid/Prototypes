using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class IssuerTokenResolver : SecurityTokenResolver
	{
		public static readonly StoreName DefaultStoreName = StoreName.TrustedPeople;

		public static readonly StoreLocation DefaultStoreLocation = StoreLocation.LocalMachine;

		private SecurityTokenResolver _wrappedTokenResolver;

		internal static IssuerTokenResolver DefaultInstance = new IssuerTokenResolver();

		public SecurityTokenResolver WrappedTokenResolver => _wrappedTokenResolver;

		public IssuerTokenResolver()
			: this(new X509CertificateStoreTokenResolver(DefaultStoreName, DefaultStoreLocation))
		{
		}

		public IssuerTokenResolver(SecurityTokenResolver wrappedTokenResolver)
		{
			if (wrappedTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wrappedTokenResolver");
			}
			_wrappedTokenResolver = wrappedTokenResolver;
		}

		protected override bool TryResolveSecurityKeyCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key)
		{
			if (keyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
			}
			key = null;
			X509RawDataKeyIdentifierClause x509RawDataKeyIdentifierClause = keyIdentifierClause as X509RawDataKeyIdentifierClause;
			if (x509RawDataKeyIdentifierClause != null)
			{
				key = x509RawDataKeyIdentifierClause.CreateKey();
				return true;
			}
			RsaKeyIdentifierClause rsaKeyIdentifierClause = keyIdentifierClause as RsaKeyIdentifierClause;
			if (rsaKeyIdentifierClause != null)
			{
				key = rsaKeyIdentifierClause.CreateKey();
				return true;
			}
			if (_wrappedTokenResolver.TryResolveSecurityKey(keyIdentifierClause, out key))
			{
				return true;
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
				if (TryResolveTokenCore(item, out token))
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
			X509RawDataKeyIdentifierClause x509RawDataKeyIdentifierClause = keyIdentifierClause as X509RawDataKeyIdentifierClause;
			if (x509RawDataKeyIdentifierClause != null)
			{
				token = new X509SecurityToken(new X509Certificate2(x509RawDataKeyIdentifierClause.GetX509RawData()));
				return true;
			}
			RsaKeyIdentifierClause rsaKeyIdentifierClause = keyIdentifierClause as RsaKeyIdentifierClause;
			if (rsaKeyIdentifierClause != null)
			{
				token = new RsaSecurityToken(rsaKeyIdentifierClause.Rsa);
				return true;
			}
			if (_wrappedTokenResolver.TryResolveToken(keyIdentifierClause, out token))
			{
				return true;
			}
			return false;
		}
	}
}
