using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class EncryptingCredentials
	{
		private string _algorithm;

		private SecurityKey _key;

		private SecurityKeyIdentifier _keyIdentifier;

		public string Algorithm
		{
			get
			{
				return _algorithm;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				_algorithm = value;
			}
		}

		public SecurityKey SecurityKey
		{
			get
			{
				return _key;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_key = value;
			}
		}

		public SecurityKeyIdentifier SecurityKeyIdentifier
		{
			get
			{
				return _keyIdentifier;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_keyIdentifier = value;
			}
		}

		public EncryptingCredentials()
		{
		}

		public EncryptingCredentials(SecurityKey key, SecurityKeyIdentifier keyIdentifier, string algorithm)
		{
			if (key == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("key");
			}
			if (keyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifier");
			}
			if (string.IsNullOrEmpty(algorithm))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("algorithm");
			}
			_algorithm = algorithm;
			_key = key;
			_keyIdentifier = keyIdentifier;
		}
	}
}
