using System.Runtime.InteropServices;
using System.ServiceModel;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class RequestSecurityToken : WSTrustMessage
	{
		private AdditionalContext _additionalContext;

		private RequestClaimCollection _claims;

		private string _computedKeyAlgorithm;

		private Renewing _renewing;

		private SecurityTokenElement _renewTarget;

		private bool _requestDisplayToken;

		private string _displayTokenLanguage;

		private InformationCardReference _informationCardReference;

		private string _clientPseudonym;

		private SecurityTokenElement _proofEncryption;

		private RequestSecurityToken _secondaryParameters;

		private SecurityTokenElement _onBehalfOf;

		private EndpointAddress _onBehalfOfIssuer;

		private SecurityTokenElement _actAs;

		private SecurityTokenElement _delegateTo;

		private bool? _forwardable;

		private bool? _delegatable;

		private SecurityTokenElement _cancelTarget;

		private SecurityTokenElement _validateTarget;

		private Participants _participants;

		private SecurityTokenElement _encryption;

		public RequestClaimCollection Claims
		{
			get
			{
				if (_claims == null)
				{
					_claims = new RequestClaimCollection();
				}
				return _claims;
			}
		}

		public string ClientPseudonym
		{
			get
			{
				return _clientPseudonym;
			}
			set
			{
				_clientPseudonym = value;
			}
		}

		public SecurityTokenElement Encryption
		{
			get
			{
				return _encryption;
			}
			set
			{
				_encryption = value;
			}
		}

		public string ComputedKeyAlgorithm
		{
			get
			{
				return _computedKeyAlgorithm;
			}
			set
			{
				_computedKeyAlgorithm = value;
			}
		}

		public bool? Delegatable
		{
			get
			{
				return _delegatable;
			}
			set
			{
				_delegatable = value;
			}
		}

		public SecurityTokenElement DelegateTo
		{
			get
			{
				return _delegateTo;
			}
			set
			{
				_delegateTo = value;
			}
		}

		public bool RequestDisplayToken
		{
			get
			{
				return _requestDisplayToken;
			}
			set
			{
				_requestDisplayToken = value;
			}
		}

		public string DisplayTokenLanguage
		{
			get
			{
				return _displayTokenLanguage;
			}
			set
			{
				_displayTokenLanguage = value;
			}
		}

		public bool? Forwardable
		{
			get
			{
				return _forwardable;
			}
			set
			{
				_forwardable = value;
			}
		}

		public InformationCardReference InformationCardReference
		{
			get
			{
				return _informationCardReference;
			}
			set
			{
				_informationCardReference = value;
			}
		}

		public SecurityTokenElement OnBehalfOf
		{
			get
			{
				return _onBehalfOf;
			}
			set
			{
				_onBehalfOf = value;
			}
		}

		public Participants Participants
		{
			get
			{
				return _participants;
			}
			set
			{
				_participants = value;
			}
		}

		public EndpointAddress Issuer
		{
			get
			{
				return _onBehalfOfIssuer;
			}
			set
			{
				_onBehalfOfIssuer = value;
			}
		}

		public AdditionalContext AdditionalContext
		{
			get
			{
				return _additionalContext;
			}
			set
			{
				_additionalContext = value;
			}
		}

		public SecurityTokenElement ActAs
		{
			get
			{
				return _actAs;
			}
			set
			{
				_actAs = value;
			}
		}

		public SecurityTokenElement CancelTarget
		{
			get
			{
				return _cancelTarget;
			}
			set
			{
				_cancelTarget = value;
			}
		}

		public SecurityTokenElement ProofEncryption
		{
			get
			{
				return _proofEncryption;
			}
			set
			{
				_proofEncryption = value;
			}
		}

		public Renewing Renewing
		{
			get
			{
				return _renewing;
			}
			set
			{
				_renewing = value;
			}
		}

		public SecurityTokenElement RenewTarget
		{
			get
			{
				return _renewTarget;
			}
			set
			{
				_renewTarget = value;
			}
		}

		public RequestSecurityToken SecondaryParameters
		{
			get
			{
				return _secondaryParameters;
			}
			set
			{
				_secondaryParameters = value;
			}
		}

		public SecurityTokenElement ValidateTarget
		{
			get
			{
				return _validateTarget;
			}
			set
			{
				_validateTarget = value;
			}
		}

		public RequestSecurityToken()
			: this(null, null)
		{
		}

		public RequestSecurityToken(string requestType)
			: this(requestType, null)
		{
		}

		public RequestSecurityToken(string requestType, string keyType)
		{
			base.RequestType = requestType;
			switch (keyType)
			{
			case "http://schemas.microsoft.com/idfx/keytype/symmetric":
				base.Entropy = new Entropy(256);
				base.KeySizeInBits = 256;
				break;
			case "http://schemas.microsoft.com/idfx/keytype/bearer":
				base.KeySizeInBits = 0;
				break;
			case "http://schemas.microsoft.com/idfx/keytype/asymmetric":
				base.KeySizeInBits = 1024;
				break;
			}
			base.KeyType = keyType;
		}
	}
}
