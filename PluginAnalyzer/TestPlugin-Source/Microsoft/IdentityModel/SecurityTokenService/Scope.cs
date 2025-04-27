using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class Scope
	{
		private string _appliesToAddress;

		private string _replyToAddress;

		private EncryptingCredentials _encryptingCredentials;

		private SigningCredentials _signingCredentials;

		private bool _symmetricKeyEncryptionRequired = true;

		private bool _tokenEncryptionRequired = true;

		private Dictionary<string, object> _properties = new Dictionary<string, object>();

		public virtual string AppliesToAddress
		{
			get
			{
				return _appliesToAddress;
			}
			set
			{
				_appliesToAddress = value;
			}
		}

		public virtual EncryptingCredentials EncryptingCredentials
		{
			get
			{
				return _encryptingCredentials;
			}
			set
			{
				_encryptingCredentials = value;
			}
		}

		public virtual string ReplyToAddress
		{
			get
			{
				return _replyToAddress;
			}
			set
			{
				_replyToAddress = value;
			}
		}

		public virtual SigningCredentials SigningCredentials
		{
			get
			{
				return _signingCredentials;
			}
			set
			{
				_signingCredentials = value;
			}
		}

		public virtual bool SymmetricKeyEncryptionRequired
		{
			get
			{
				return _symmetricKeyEncryptionRequired;
			}
			set
			{
				_symmetricKeyEncryptionRequired = value;
			}
		}

		public virtual bool TokenEncryptionRequired
		{
			get
			{
				return _tokenEncryptionRequired;
			}
			set
			{
				_tokenEncryptionRequired = value;
			}
		}

		public virtual Dictionary<string, object> Properties => _properties;

		public Scope()
			: this(null, null, null)
		{
		}

		public Scope(string appliesToAddress)
			: this(appliesToAddress, null, null)
		{
		}

		public Scope(string appliesToAddress, SigningCredentials signingCredentials)
			: this(appliesToAddress, signingCredentials, null)
		{
		}

		public Scope(string appliesToAddress, EncryptingCredentials encryptingCredentials)
			: this(appliesToAddress, null, encryptingCredentials)
		{
		}

		public Scope(string appliesToAddress, SigningCredentials signingCredentials, EncryptingCredentials encryptingCredentials)
		{
			_appliesToAddress = appliesToAddress;
			_signingCredentials = signingCredentials;
			_encryptingCredentials = encryptingCredentials;
		}
	}
}
