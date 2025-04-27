using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityTokenDescriptor
	{
		private SecurityKeyIdentifierClause _attachedReference;

		private AuthenticationInformation _authenticationInfo;

		private string _tokenIssuerName;

		private ProofDescriptor _proofDescriptor;

		private IClaimsIdentity _subject;

		private SecurityToken _token;

		private string _tokenType;

		private SecurityKeyIdentifierClause _unattachedReference;

		private Lifetime _lifetime;

		private DisplayToken _displayToken;

		private string _appliesToAddress;

		private string _replyToAddress;

		private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _encryptingCredentials;

		private SigningCredentials _signingCredentials;

		private Dictionary<string, object> _properties = new Dictionary<string, object>();

		public string AppliesToAddress
		{
			get
			{
				return _appliesToAddress;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && !UriUtil.CanCreateValidUri(value, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2002")));
				}
				_appliesToAddress = value;
			}
		}

		public string ReplyToAddress
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

		public Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials EncryptingCredentials
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

		public SigningCredentials SigningCredentials
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

		public SecurityKeyIdentifierClause AttachedReference
		{
			get
			{
				return _attachedReference;
			}
			set
			{
				_attachedReference = value;
			}
		}

		public AuthenticationInformation AuthenticationInfo
		{
			get
			{
				return _authenticationInfo;
			}
			set
			{
				_authenticationInfo = value;
			}
		}

		public string TokenIssuerName
		{
			get
			{
				return _tokenIssuerName;
			}
			set
			{
				_tokenIssuerName = value;
			}
		}

		public ProofDescriptor Proof
		{
			get
			{
				return _proofDescriptor;
			}
			set
			{
				_proofDescriptor = value;
			}
		}

		public Dictionary<string, object> Properties => _properties;

		public SecurityToken Token
		{
			get
			{
				return _token;
			}
			set
			{
				_token = value;
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

		public SecurityKeyIdentifierClause UnattachedReference
		{
			get
			{
				return _unattachedReference;
			}
			set
			{
				_unattachedReference = value;
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

		public DisplayToken DisplayToken
		{
			get
			{
				return _displayToken;
			}
			set
			{
				_displayToken = value;
			}
		}

		public IClaimsIdentity Subject
		{
			get
			{
				return _subject;
			}
			set
			{
				_subject = value;
			}
		}

		public void AddAuthenticationClaims(string authType)
		{
			AddAuthenticationClaims(authType, DateTime.UtcNow);
		}

		public void AddAuthenticationClaims(string authType, DateTime time)
		{
			Subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", authType, "http://www.w3.org/2001/XMLSchema#string"));
			Subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(time.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
		}

		public virtual void ApplyTo(RequestSecurityTokenResponse response)
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (_tokenType != null)
			{
				response.TokenType = _tokenType;
			}
			if (_token != null)
			{
				response.RequestedSecurityToken = new RequestedSecurityToken(_token);
			}
			if (_attachedReference != null)
			{
				response.RequestedAttachedReference = _attachedReference;
			}
			if (_unattachedReference != null)
			{
				response.RequestedUnattachedReference = _unattachedReference;
			}
			if (_lifetime != null)
			{
				response.Lifetime = _lifetime;
			}
			if (_displayToken != null)
			{
				response.RequestedDisplayToken = _displayToken;
			}
			if (_proofDescriptor != null)
			{
				_proofDescriptor.ApplyTo(response);
			}
		}
	}
}
