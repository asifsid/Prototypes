using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityKeyElement : SecurityKey
	{
		private SecurityKey _securityKey;

		private object _keyLock;

		private SecurityTokenResolver _securityTokenResolver;

		private SecurityKeyIdentifier _securityKeyIdentifier;

		public override int KeySize
		{
			get
			{
				if (_securityKey == null)
				{
					ResolveKey();
				}
				return _securityKey.KeySize;
			}
		}

		public SecurityKeyElement(SecurityKeyIdentifierClause securityKeyIdentifierClause, SecurityTokenResolver securityTokenResolver)
		{
			if (securityKeyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityKeyIdentifierClause");
			}
			Initialize(new SecurityKeyIdentifier(securityKeyIdentifierClause), securityTokenResolver);
		}

		public SecurityKeyElement(SecurityKeyIdentifier securityKeyIdentifier, SecurityTokenResolver securityTokenResolver)
		{
			if (securityKeyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityKeyIdentifier");
			}
			Initialize(securityKeyIdentifier, securityTokenResolver);
		}

		private void Initialize(SecurityKeyIdentifier securityKeyIdentifier, SecurityTokenResolver securityTokenResolver)
		{
			_keyLock = new object();
			_securityKeyIdentifier = securityKeyIdentifier;
			_securityTokenResolver = securityTokenResolver;
		}

		public override byte[] DecryptKey(string algorithm, byte[] keyData)
		{
			if (_securityKey == null)
			{
				ResolveKey();
			}
			return _securityKey.DecryptKey(algorithm, keyData);
		}

		public override byte[] EncryptKey(string algorithm, byte[] keyData)
		{
			if (_securityKey == null)
			{
				ResolveKey();
			}
			return _securityKey.EncryptKey(algorithm, keyData);
		}

		public override bool IsAsymmetricAlgorithm(string algorithm)
		{
			switch (algorithm)
			{
			case "http://www.w3.org/2000/09/xmldsig#dsa-sha1":
			case "http://www.w3.org/2000/09/xmldsig#rsa-sha1":
			case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256":
			case "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p":
			case "http://www.w3.org/2001/04/xmlenc#rsa-1_5":
				return true;
			default:
				return false;
			}
		}

		public override bool IsSupportedAlgorithm(string algorithm)
		{
			if (_securityKey == null)
			{
				ResolveKey();
			}
			return _securityKey.IsSupportedAlgorithm(algorithm);
		}

		public override bool IsSymmetricAlgorithm(string algorithm)
		{
			switch (algorithm)
			{
			case "http://www.w3.org/2000/09/xmldsig#dsa-sha1":
			case "http://www.w3.org/2000/09/xmldsig#rsa-sha1":
			case "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256":
			case "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p":
			case "http://www.w3.org/2001/04/xmlenc#rsa-1_5":
				return false;
			case "http://www.w3.org/2000/09/xmldsig#hmac-sha1":
			case "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256":
			case "http://www.w3.org/2001/04/xmlenc#aes128-cbc":
			case "http://www.w3.org/2001/04/xmlenc#aes192-cbc":
			case "http://www.w3.org/2001/04/xmlenc#aes256-cbc":
			case "http://www.w3.org/2001/04/xmlenc#tripledes-cbc":
			case "http://www.w3.org/2001/04/xmlenc#kw-aes128":
			case "http://www.w3.org/2001/04/xmlenc#kw-aes192":
			case "http://www.w3.org/2001/04/xmlenc#kw-aes256":
			case "http://www.w3.org/2001/04/xmlenc#kw-tripledes":
			case "http://schemas.xmlsoap.org/ws/2005/02/sc/dk/p_sha1":
			case "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512/dk/p_sha1":
				return true;
			default:
				return false;
			}
		}

		private void ResolveKey()
		{
			if (_securityKeyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("ski");
			}
			if (_securityKey != null)
			{
				return;
			}
			lock (_keyLock)
			{
				if (_securityKey != null)
				{
					return;
				}
				if (_securityTokenResolver != null)
				{
					for (int i = 0; i < _securityKeyIdentifier.Count; i++)
					{
						if (_securityTokenResolver.TryResolveSecurityKey(_securityKeyIdentifier[i], out _securityKey))
						{
							return;
						}
					}
				}
				if (_securityKeyIdentifier.CanCreateKey)
				{
					_securityKey = _securityKeyIdentifier.CreateKey();
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelper(new SecurityTokenException(SR.GetString("ID2080", (_securityTokenResolver == null) ? "null" : _securityTokenResolver.ToString(), (_securityKeyIdentifier == null) ? "null" : _securityKeyIdentifier.ToString())), TraceEventType.Error);
			}
		}
	}
}
