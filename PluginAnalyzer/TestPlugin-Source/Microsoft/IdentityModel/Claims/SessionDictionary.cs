using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Claims
{
	[ComVisible(true)]
	public sealed class SessionDictionary : XmlDictionary
	{
		private static readonly SessionDictionary instance = new SessionDictionary();

		private XmlDictionaryString _claim;

		private XmlDictionaryString _sct;

		private XmlDictionaryString _issuer;

		private XmlDictionaryString _originalIssuer;

		private XmlDictionaryString _issuerRef;

		private XmlDictionaryString _claimCollection;

		private XmlDictionaryString _actor;

		private XmlDictionaryString _claimProperty;

		private XmlDictionaryString _claimProperties;

		private XmlDictionaryString _value;

		private XmlDictionaryString _valueType;

		private XmlDictionaryString _label;

		private XmlDictionaryString _claimPropertyName;

		private XmlDictionaryString _claimPropertyValue;

		private XmlDictionaryString _type;

		private XmlDictionaryString _subjectId;

		private XmlDictionaryString _contextId;

		private XmlDictionaryString _anonymousIssued;

		private XmlDictionaryString _selfIssued;

		private XmlDictionaryString _authenticationType;

		private XmlDictionaryString _nameClaimType;

		private XmlDictionaryString _roleClaimType;

		private XmlDictionaryString _version;

		private XmlDictionaryString _scVersion;

		private XmlDictionaryString _emptyString;

		private XmlDictionaryString _nullValue;

		private XmlDictionaryString _key;

		private XmlDictionaryString _effectiveTime;

		private XmlDictionaryString _expiryTime;

		private XmlDictionaryString _keyGeneration;

		private XmlDictionaryString _keyEffectiveTime;

		private XmlDictionaryString _keyExpiryTime;

		private XmlDictionaryString _sessionId;

		private XmlDictionaryString _id;

		private XmlDictionaryString _validFrom;

		private XmlDictionaryString _validTo;

		private XmlDictionaryString _sesionToken;

		private XmlDictionaryString _sesionTokenCookie;

		private XmlDictionaryString _bootStrapToken;

		private XmlDictionaryString _context;

		private XmlDictionaryString _claimsPrincipal;

		private XmlDictionaryString _windowsPrincipal;

		private XmlDictionaryString _windowsIdentity;

		private XmlDictionaryString _identity;

		private XmlDictionaryString _identities;

		private XmlDictionaryString _windowsLogonName;

		private XmlDictionaryString _persistentTrue;

		private XmlDictionaryString _sctAuthorizationPolicy;

		private XmlDictionaryString _right;

		private XmlDictionaryString _endpointId;

		private XmlDictionaryString _windowsSidClaim;

		private XmlDictionaryString _denyOnlySidClaim;

		private XmlDictionaryString _x500DistinguishedNameClaim;

		private XmlDictionaryString _x509ThumbprintClaim;

		private XmlDictionaryString _nameClaim;

		private XmlDictionaryString _dnsClaim;

		private XmlDictionaryString _rsaClaim;

		private XmlDictionaryString _mailAddressClaim;

		private XmlDictionaryString _systemClaim;

		private XmlDictionaryString _hashClaim;

		private XmlDictionaryString _spnClaim;

		private XmlDictionaryString _upnClaim;

		private XmlDictionaryString _urlClaim;

		private XmlDictionaryString _sid;

		private XmlDictionaryString _sessionModeTrue;

		public static SessionDictionary Instance => instance;

		public XmlDictionaryString PersistentTrue => _persistentTrue;

		public XmlDictionaryString WindowsLogonName => _windowsLogonName;

		public XmlDictionaryString ClaimsPrincipal => _claimsPrincipal;

		public XmlDictionaryString WindowsPrincipal => _windowsPrincipal;

		public XmlDictionaryString AnonymousIssued => _anonymousIssued;

		public XmlDictionaryString WindowsIdentity => _windowsIdentity;

		public XmlDictionaryString Identity => _identity;

		public XmlDictionaryString Identities => _identities;

		public XmlDictionaryString SessionId => _sessionId;

		public XmlDictionaryString SessionModeTrue => _sessionModeTrue;

		public XmlDictionaryString ValidFrom => _validFrom;

		public XmlDictionaryString ValidTo => _validTo;

		public XmlDictionaryString EffectiveTime => _effectiveTime;

		public XmlDictionaryString ExpiryTime => _expiryTime;

		public XmlDictionaryString KeyEffectiveTime => _keyEffectiveTime;

		public XmlDictionaryString KeyExpiryTime => _keyExpiryTime;

		public XmlDictionaryString Claim => _claim;

		public XmlDictionaryString SelfIssued => _selfIssued;

		public XmlDictionaryString Issuer => _issuer;

		public XmlDictionaryString OriginalIssuer => _originalIssuer;

		public XmlDictionaryString IssuerRef => _issuerRef;

		public XmlDictionaryString ClaimCollection => _claimCollection;

		public XmlDictionaryString Actor => _actor;

		public XmlDictionaryString ClaimProperties => _claimProperties;

		public XmlDictionaryString ClaimProperty => _claimProperty;

		public XmlDictionaryString Value => _value;

		public XmlDictionaryString ValueType => _valueType;

		public XmlDictionaryString Label => _label;

		public XmlDictionaryString Type => _type;

		public XmlDictionaryString SubjectId => _subjectId;

		public XmlDictionaryString ClaimPropertyName => _claimPropertyName;

		public XmlDictionaryString ClaimPropertyValue => _claimPropertyValue;

		public XmlDictionaryString AuthenticationType => _authenticationType;

		public XmlDictionaryString NameClaimType => _nameClaimType;

		public XmlDictionaryString RoleClaimType => _roleClaimType;

		public XmlDictionaryString NullValue => _nullValue;

		public XmlDictionaryString SecurityContextToken => _sct;

		public XmlDictionaryString Version => _version;

		public XmlDictionaryString SecureConversationVersion => _scVersion;

		public XmlDictionaryString EmptyString => _emptyString;

		public XmlDictionaryString Key => _key;

		public XmlDictionaryString KeyGeneration => _keyGeneration;

		public XmlDictionaryString Id => _id;

		public XmlDictionaryString ContextId => _contextId;

		public XmlDictionaryString SessionToken => _sesionToken;

		public XmlDictionaryString SessionTokenCookie => _sesionTokenCookie;

		public XmlDictionaryString BootstrapToken => _bootStrapToken;

		public XmlDictionaryString Context => _context;

		public XmlDictionaryString SctAuthorizationPolicy => _sctAuthorizationPolicy;

		public XmlDictionaryString Right => _right;

		public XmlDictionaryString EndpointId => _endpointId;

		public XmlDictionaryString WindowsSidClaim => _windowsSidClaim;

		public XmlDictionaryString DenyOnlySidClaim => _denyOnlySidClaim;

		public XmlDictionaryString X500DistinguishedNameClaim => _x500DistinguishedNameClaim;

		public XmlDictionaryString X509ThumbprintClaim => _x509ThumbprintClaim;

		public XmlDictionaryString NameClaim => _nameClaim;

		public XmlDictionaryString DnsClaim => _dnsClaim;

		public XmlDictionaryString RsaClaim => _rsaClaim;

		public XmlDictionaryString MailAddressClaim => _mailAddressClaim;

		public XmlDictionaryString SystemClaim => _systemClaim;

		public XmlDictionaryString HashClaim => _hashClaim;

		public XmlDictionaryString SpnClaim => _spnClaim;

		public XmlDictionaryString UpnClaim => _upnClaim;

		public XmlDictionaryString UrlClaim => _urlClaim;

		public XmlDictionaryString Sid => _sid;

		private SessionDictionary()
		{
			_claim = Add("Claim");
			_sct = Add("SecurityContextToken");
			_version = Add("Version");
			_scVersion = Add("SecureConversationVersion");
			_issuer = Add("Issuer");
			_originalIssuer = Add("OriginalIssuer");
			_issuerRef = Add("IssuerRef");
			_claimCollection = Add("ClaimCollection");
			_actor = Add("Actor");
			_claimProperty = Add("ClaimProperty");
			_claimProperties = Add("ClaimProperties");
			_value = Add("Value");
			_valueType = Add("ValueType");
			_label = Add("Label");
			_type = Add("Type");
			_subjectId = Add("subjectID");
			_claimPropertyName = Add("ClaimPropertyName");
			_claimPropertyValue = Add("ClaimPropertyValue");
			_anonymousIssued = Add("http://www.w3.org/2005/08/addressing/anonymous");
			_selfIssued = Add("http://schemas.xmlsoap.org/ws/2005/05/identity/issuer/self");
			_authenticationType = Add("AuthenticationType");
			_nameClaimType = Add("NameClaimType");
			_roleClaimType = Add("RoleClaimType");
			_nullValue = Add("Null");
			_emptyString = Add(string.Empty);
			_key = Add("Key");
			_effectiveTime = Add("EffectiveTime");
			_expiryTime = Add("ExpiryTime");
			_keyGeneration = Add("KeyGeneration");
			_keyEffectiveTime = Add("KeyEffectiveTime");
			_keyExpiryTime = Add("KeyExpiryTime");
			_sessionId = Add("SessionId");
			_id = Add("Id");
			_validFrom = Add("ValidFrom");
			_validTo = Add("ValidTo");
			_contextId = Add("ContextId");
			_sesionToken = Add("SessionToken");
			_sesionTokenCookie = Add("SessionTokenCookie");
			_bootStrapToken = Add("BootStrapToken");
			_context = Add("Context");
			_claimsPrincipal = Add("ClaimsPrincipal");
			_windowsPrincipal = Add("WindowsPrincipal");
			_windowsIdentity = Add("WindowIdentity");
			_identity = Add("Identity");
			_identities = Add("Identities");
			_windowsLogonName = Add("WindowsLogonName");
			_persistentTrue = Add("PersistentTrue");
			_sctAuthorizationPolicy = Add("SctAuthorizationPolicy");
			_right = Add("Right");
			_endpointId = Add("EndpointId");
			_windowsSidClaim = Add("WindowsSidClaim");
			_denyOnlySidClaim = Add("DenyOnlySidClaim");
			_x500DistinguishedNameClaim = Add("X500DistinguishedNameClaim");
			_x509ThumbprintClaim = Add("X509ThumbprintClaim");
			_nameClaim = Add("NameClaim");
			_dnsClaim = Add("DnsClaim");
			_rsaClaim = Add("RsaClaim");
			_mailAddressClaim = Add("MailAddressClaim");
			_systemClaim = Add("SystemClaim");
			_hashClaim = Add("HashClaim");
			_spnClaim = Add("SpnClaim");
			_upnClaim = Add("UpnClaim");
			_urlClaim = Add("UrlClaim");
			_sid = Add("Sid");
			_sessionModeTrue = Add("SessionModeTrue");
		}
	}
}
