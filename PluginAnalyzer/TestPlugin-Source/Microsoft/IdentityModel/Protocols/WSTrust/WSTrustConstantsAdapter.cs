using System;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	internal abstract class WSTrustConstantsAdapter
	{
		internal abstract class WSTrustActions
		{
			internal string Cancel;

			internal string CancelResponse;

			internal string Issue;

			internal string IssueResponse;

			internal string Renew;

			internal string RenewResponse;

			internal string RequestSecurityContextToken;

			internal string RequestSecurityContextTokenCancel;

			internal string RequestSecurityContextTokenResponse;

			internal string RequestSecurityContextTokenResponseCancel;

			internal string Validate;

			internal string ValidateResponse;
		}

		internal class WSTrustAttributeNames
		{
			internal string Allow = "Allow";

			internal string Context = "Context";

			internal string Dialect = "Dialect";

			internal string EncodingType = "EncodingType";

			internal string OK = "OK";

			internal string Type = "Type";

			internal string ValueType = "ValueType";
		}

		internal abstract class WSTrustComputedKeyAlgorithm
		{
			internal string Psha1;
		}

		internal class WSTrustElementNames
		{
			internal string AllowPostdating = "AllowPostdating";

			internal string AuthenticationType = "AuthenticationType";

			internal string BinarySecret = "BinarySecret";

			internal string BinaryExchange = "BinaryExchange";

			internal string CancelTarget = "CancelTarget";

			internal string Claims = "Claims";

			internal string ComputedKey = "ComputedKey";

			internal string ComputedKeyAlgorithm = "ComputedKeyAlgorithm";

			internal string CanonicalizationAlgorithm = "CanonicalizationAlgorithm";

			internal string Code = "Code";

			internal string Delegatable = "Delegatable";

			internal string DelegateTo = "DelegateTo";

			internal string Encryption = "Encryption";

			internal string EncryptionAlgorithm = "EncryptionAlgorithm";

			internal string EncryptWith = "EncryptWith";

			internal string Entropy = "Entropy";

			internal string Forwardable = "Forwardable";

			internal string Issuer = "Issuer";

			internal string KeySize = "KeySize";

			internal string KeyType = "KeyType";

			internal string Lifetime = "Lifetime";

			internal string OnBehalfOf = "OnBehalfOf";

			internal string Participant = "Participant";

			internal string Participants = "Participants";

			internal string Primary = "Primary";

			internal string ProofEncryption = "ProofEncryption";

			internal string Reason = "Reason";

			internal string Renewing = "Renewing";

			internal string RenewTarget = "RenewTarget";

			internal string RequestedAttachedReference = "RequestedAttachedReference";

			internal string RequestedProofToken = "RequestedProofToken";

			internal string RequestedSecurityToken = "RequestedSecurityToken";

			internal string RequestedTokenCancelled = "RequestedTokenCancelled";

			internal string RequestedUnattachedReference = "RequestedUnattachedReference";

			internal string RequestKeySize = "RequestKeySize";

			internal string RequestSecurityToken = "RequestSecurityToken";

			internal string RequestSecurityTokenResponse = "RequestSecurityTokenResponse";

			internal string RequestType = "RequestType";

			internal string SecurityContextToken = "SecurityContextToken";

			internal string SignWith = "SignWith";

			internal string SignatureAlgorithm = "SignatureAlgorithm";

			internal string Status = "Status";

			internal string TokenType = "TokenType";

			internal string UseKey = "UseKey";
		}

		internal abstract class WSTrustRequestTypes
		{
			internal string Cancel;

			internal string Issue;

			internal string Renew;

			internal string Validate;
		}

		internal abstract class WSTrustKeyTypes
		{
			internal string Asymmetric;

			internal string Bearer;

			internal string Symmetric;
		}

		internal class FaultCodeValues
		{
			internal string AuthenticationBadElements = "AuthenticationBadElements";

			internal string BadRequest = "BadRequest";

			internal string ExpiredData = "ExpiredData";

			internal string FailedAuthentication = "FailedAuthentication";

			internal string InvalidRequest = "InvalidRequest";

			internal string InvalidScope = "InvalidScope";

			internal string InvalidSecurityToken = "InvalidSecurityToken";

			internal string InvalidTimeRange = "InvalidTimeRange";

			internal string RenewNeeded = "RenewNeeded";

			internal string RequestFailed = "RequestFailed";

			internal string UnableToRenew = "UnableToRenew";
		}

		internal static WSTrustAttributeNames _attributeNames;

		internal static WSTrustElementNames _elementNames;

		internal static FaultCodeValues _faultCodes;

		internal string NamespaceURI;

		internal string Prefix;

		internal abstract WSTrustActions Actions { get; }

		internal virtual WSTrustAttributeNames Attributes
		{
			get
			{
				if (_attributeNames == null)
				{
					_attributeNames = new WSTrustAttributeNames();
				}
				return _attributeNames;
			}
		}

		internal abstract WSTrustComputedKeyAlgorithm ComputedKeyAlgorithm { get; }

		internal virtual WSTrustElementNames Elements
		{
			get
			{
				if (_elementNames == null)
				{
					_elementNames = new WSTrustElementNames();
				}
				return _elementNames;
			}
		}

		internal virtual FaultCodeValues FaultCodes
		{
			get
			{
				if (_faultCodes == null)
				{
					_faultCodes = new FaultCodeValues();
				}
				return _faultCodes;
			}
		}

		internal abstract WSTrustRequestTypes RequestTypes { get; }

		internal abstract WSTrustKeyTypes KeyTypes { get; }

		internal static WSTrustFeb2005ConstantsAdapter TrustFeb2005 => WSTrustFeb2005ConstantsAdapter.Instance;

		internal static WSTrust13ConstantsAdapter Trust13 => WSTrust13ConstantsAdapter.Instance;

		internal static WSTrustConstantsAdapter GetConstantsAdapter(string ns)
		{
			if (StringComparer.Ordinal.Equals(ns, "http://schemas.xmlsoap.org/ws/2005/02/trust"))
			{
				return TrustFeb2005;
			}
			if (StringComparer.Ordinal.Equals(ns, "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
			{
				return Trust13;
			}
			return null;
		}
	}
}
