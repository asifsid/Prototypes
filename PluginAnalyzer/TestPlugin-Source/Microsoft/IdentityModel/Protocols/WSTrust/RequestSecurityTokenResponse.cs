using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class RequestSecurityTokenResponse : WSTrustMessage
	{
		private SecurityKeyIdentifierClause _requestedAttachedReference;

		private DisplayToken _requestedDisplayToken;

		private RequestedProofToken _requestedProofToken;

		private RequestedSecurityToken _requestedSecurityToken;

		private SecurityKeyIdentifierClause _requestedUnattachedReference;

		private bool _requestedTokenCancelled;

		private Status _status;

		private bool _isFinal = true;

		public bool IsFinal
		{
			get
			{
				return _isFinal;
			}
			set
			{
				_isFinal = value;
			}
		}

		public SecurityKeyIdentifierClause RequestedAttachedReference
		{
			get
			{
				return _requestedAttachedReference;
			}
			set
			{
				_requestedAttachedReference = value;
			}
		}

		public DisplayToken RequestedDisplayToken
		{
			get
			{
				return _requestedDisplayToken;
			}
			set
			{
				_requestedDisplayToken = value;
			}
		}

		public RequestedSecurityToken RequestedSecurityToken
		{
			get
			{
				return _requestedSecurityToken;
			}
			set
			{
				_requestedSecurityToken = value;
			}
		}

		public RequestedProofToken RequestedProofToken
		{
			get
			{
				return _requestedProofToken;
			}
			set
			{
				_requestedProofToken = value;
			}
		}

		public SecurityKeyIdentifierClause RequestedUnattachedReference
		{
			get
			{
				return _requestedUnattachedReference;
			}
			set
			{
				_requestedUnattachedReference = value;
			}
		}

		public bool RequestedTokenCancelled
		{
			get
			{
				return _requestedTokenCancelled;
			}
			set
			{
				_requestedTokenCancelled = value;
			}
		}

		public Status Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}

		public RequestSecurityTokenResponse()
		{
		}

		public RequestSecurityTokenResponse(WSTrustMessage message)
		{
			if (message == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("message");
			}
			base.RequestType = message.RequestType;
			base.Context = message.Context;
			base.KeyType = message.KeyType;
			if (message.KeySizeInBits > 0 && StringComparer.Ordinal.Equals(message.KeyType, "http://schemas.microsoft.com/idfx/keytype/symmetric"))
			{
				base.KeySizeInBits = message.KeySizeInBits;
			}
		}
	}
}
