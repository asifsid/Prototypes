using System.Runtime.InteropServices;
using System.ServiceModel;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public abstract class WSTrustMessage : OpenObject
	{
		private bool _allowPostdating;

		private EndpointAddress _appliesTo;

		private string _replyTo;

		private string _authenticationType;

		private string _canonicalizationAlgorithm;

		private string _context;

		private string _encryptionAlgorithm;

		private Entropy _entropy;

		private string _issuedTokenEncryptionAlgorithm;

		private string _keyWrapAlgorithm;

		private string _issuedTokenSignatureAlgorithm;

		private int? _keySizeInBits;

		private string _keyType;

		private Lifetime _lifetime;

		private string _requestType;

		private string _signatureAlgorithm;

		private string _tokenType;

		private UseKey _useKey;

		private BinaryExchange _binaryExchange;

		public bool AllowPostdating
		{
			get
			{
				return _allowPostdating;
			}
			set
			{
				_allowPostdating = value;
			}
		}

		public EndpointAddress AppliesTo
		{
			get
			{
				return _appliesTo;
			}
			set
			{
				_appliesTo = value;
			}
		}

		public BinaryExchange BinaryExchange
		{
			get
			{
				return _binaryExchange;
			}
			set
			{
				_binaryExchange = value;
			}
		}

		public string ReplyTo
		{
			get
			{
				return _replyTo;
			}
			set
			{
				_replyTo = value;
			}
		}

		public string AuthenticationType
		{
			get
			{
				return _authenticationType;
			}
			set
			{
				_authenticationType = value;
			}
		}

		public string CanonicalizationAlgorithm
		{
			get
			{
				return _canonicalizationAlgorithm;
			}
			set
			{
				_canonicalizationAlgorithm = value;
			}
		}

		public string Context
		{
			get
			{
				return _context;
			}
			set
			{
				_context = value;
			}
		}

		public string EncryptionAlgorithm
		{
			get
			{
				return _encryptionAlgorithm;
			}
			set
			{
				_encryptionAlgorithm = value;
			}
		}

		public Entropy Entropy
		{
			get
			{
				return _entropy;
			}
			set
			{
				_entropy = value;
			}
		}

		public string EncryptWith
		{
			get
			{
				return _issuedTokenEncryptionAlgorithm;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("IssuedTokenEncryptionAlgorithm");
				}
				_issuedTokenEncryptionAlgorithm = value;
			}
		}

		public string SignWith
		{
			get
			{
				return _issuedTokenSignatureAlgorithm;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_issuedTokenSignatureAlgorithm = value;
			}
		}

		public int? KeySizeInBits
		{
			get
			{
				return _keySizeInBits;
			}
			set
			{
				if (value.HasValue && value.Value < 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value");
				}
				_keySizeInBits = value;
			}
		}

		public string KeyType
		{
			get
			{
				return _keyType;
			}
			set
			{
				_keyType = value;
			}
		}

		public string KeyWrapAlgorithm
		{
			get
			{
				return _keyWrapAlgorithm;
			}
			set
			{
				_keyWrapAlgorithm = value;
			}
		}

		public Lifetime Lifetime
		{
			get
			{
				return _lifetime;
			}
			set
			{
				_lifetime = value;
			}
		}

		public string RequestType
		{
			get
			{
				return _requestType;
			}
			set
			{
				_requestType = value;
			}
		}

		public string SignatureAlgorithm
		{
			get
			{
				return _signatureAlgorithm;
			}
			set
			{
				_signatureAlgorithm = value;
			}
		}

		public string TokenType
		{
			get
			{
				return _tokenType;
			}
			set
			{
				_tokenType = value;
			}
		}

		public UseKey UseKey
		{
			get
			{
				return _useKey;
			}
			set
			{
				_useKey = value;
			}
		}
	}
}
