using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Threading;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public abstract class SecurityTokenService
	{
		protected class FederatedAsyncState
		{
			private RequestSecurityToken _request;

			private IClaimsPrincipal _claimsPrincipal;

			private Microsoft.IdentityModel.Tokens.SecurityTokenHandler _securityTokenHandler;

			private IAsyncResult _result;

			public RequestSecurityToken Request => _request;

			public IClaimsPrincipal ClaimsPrincipal => _claimsPrincipal;

			public Microsoft.IdentityModel.Tokens.SecurityTokenHandler SecurityTokenHandler
			{
				get
				{
					return _securityTokenHandler;
				}
				set
				{
					_securityTokenHandler = value;
				}
			}

			public IAsyncResult Result => _result;

			public FederatedAsyncState(FederatedAsyncState federatedAsyncState)
			{
				if (federatedAsyncState == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("FederatedAsyncState");
				}
				_request = federatedAsyncState.Request;
				_claimsPrincipal = federatedAsyncState.ClaimsPrincipal;
				_securityTokenHandler = federatedAsyncState.SecurityTokenHandler;
				_result = federatedAsyncState.Result;
			}

			public FederatedAsyncState(RequestSecurityToken request, IClaimsPrincipal principal, IAsyncResult result)
			{
				if (request == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
				}
				if (result == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("result");
				}
				_request = request;
				_claimsPrincipal = principal;
				_result = result;
			}
		}

		private SecurityTokenServiceConfiguration _securityTokenServiceConfiguration;

		private IClaimsPrincipal _principal;

		private RequestSecurityToken _request;

		private Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor _tokenDescriptor;

		public SecurityTokenServiceConfiguration SecurityTokenServiceConfiguration => _securityTokenServiceConfiguration;

		public IClaimsPrincipal Principal
		{
			get
			{
				return _principal;
			}
			set
			{
				_principal = value;
			}
		}

		public RequestSecurityToken Request
		{
			get
			{
				return _request;
			}
			set
			{
				_request = value;
			}
		}

		public Scope Scope { get; set; }

		protected Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor SecurityTokenDescriptor
		{
			get
			{
				return _tokenDescriptor;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_tokenDescriptor = value;
			}
		}

		protected SecurityTokenService(SecurityTokenServiceConfiguration securityTokenServiceConfiguration)
		{
			if (securityTokenServiceConfiguration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenServiceConfiguration");
			}
			_securityTokenServiceConfiguration = securityTokenServiceConfiguration;
		}

		public virtual IAsyncResult BeginCancel(IClaimsPrincipal principal, RequestSecurityToken request, AsyncCallback callback, object state)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", (request != null) ? request.RequestType : "Cancel")));
		}

		protected virtual IAsyncResult BeginGetScope(IClaimsPrincipal principal, RequestSecurityToken request, AsyncCallback callback, object state)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID2081")));
		}

		public virtual IAsyncResult BeginIssue(IClaimsPrincipal principal, RequestSecurityToken request, AsyncCallback callback, object state)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			_principal = principal;
			_request = request;
			ValidateRequest(request);
			FederatedAsyncState federatedAsyncState = new FederatedAsyncState(request, principal, new TypedAsyncResult<RequestSecurityTokenResponse>(callback, state));
			BeginGetScope(principal, request, OnGetScopeComplete, federatedAsyncState);
			return federatedAsyncState.Result;
		}

		public virtual IAsyncResult BeginRenew(IClaimsPrincipal principal, RequestSecurityToken request, AsyncCallback callback, object state)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", (request != null && request.RequestType != null) ? request.RequestType : "Renew")));
		}

		public virtual IAsyncResult BeginValidate(IClaimsPrincipal principal, RequestSecurityToken request, AsyncCallback callback, object state)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", (request != null && request.RequestType != null) ? request.RequestType : "Validate")));
		}

		public virtual RequestSecurityTokenResponse Cancel(IClaimsPrincipal principal, RequestSecurityToken request)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", (request != null && request.RequestType != null) ? request.RequestType : "Cancel")));
		}

		protected virtual Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor CreateSecurityTokenDescriptor(RequestSecurityToken request, Scope scope)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			if (scope == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("scope");
			}
			Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor securityTokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor();
			securityTokenDescriptor.AppliesToAddress = scope.AppliesToAddress;
			securityTokenDescriptor.ReplyToAddress = scope.ReplyToAddress;
			securityTokenDescriptor.SigningCredentials = scope.SigningCredentials;
			if (securityTokenDescriptor.SigningCredentials == null)
			{
				securityTokenDescriptor.SigningCredentials = SecurityTokenServiceConfiguration.SigningCredentials;
			}
			if (scope.EncryptingCredentials != null && scope.EncryptingCredentials.SecurityKey is AsymmetricSecurityKey && (request.EncryptionAlgorithm == null || request.EncryptionAlgorithm == "http://www.w3.org/2001/04/xmlenc#aes256-cbc") && (request.SecondaryParameters == null || request.SecondaryParameters.EncryptionAlgorithm == null || request.SecondaryParameters.EncryptionAlgorithm == "http://www.w3.org/2001/04/xmlenc#aes256-cbc"))
			{
				securityTokenDescriptor.EncryptingCredentials = new EncryptedKeyEncryptingCredentials(scope.EncryptingCredentials, 256, "http://www.w3.org/2001/04/xmlenc#aes256-cbc");
			}
			return securityTokenDescriptor;
		}

		protected virtual string GetIssuerName()
		{
			return SecurityTokenServiceConfiguration.TokenIssuerName;
		}

		private string GetValidIssuerName()
		{
			string issuerName = GetIssuerName();
			if (string.IsNullOrEmpty(issuerName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2083"));
			}
			return issuerName;
		}

		protected virtual Microsoft.IdentityModel.Tokens.ProofDescriptor GetProofToken(RequestSecurityToken request, Scope scope)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			if (scope == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("scope");
			}
			EncryptingCredentials requestorProofEncryptingCredentials = GetRequestorProofEncryptingCredentials(request);
			if (scope.EncryptingCredentials != null && !(scope.EncryptingCredentials.SecurityKey is AsymmetricSecurityKey))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4179")));
			}
			EncryptingCredentials encryptingCredentials = scope.EncryptingCredentials;
			string x = (string.IsNullOrEmpty(request.KeyType) ? "http://schemas.microsoft.com/idfx/keytype/symmetric" : request.KeyType);
			Microsoft.IdentityModel.Tokens.ProofDescriptor result = null;
			if (StringComparer.Ordinal.Equals(x, "http://schemas.microsoft.com/idfx/keytype/asymmetric"))
			{
				if (request.UseKey == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3091")));
				}
				result = new Microsoft.IdentityModel.Tokens.AsymmetricProofDescriptor(request.UseKey.SecurityKeyIdentifier);
			}
			else if (StringComparer.Ordinal.Equals(x, "http://schemas.microsoft.com/idfx/keytype/symmetric"))
			{
				if (request.ComputedKeyAlgorithm != null && !StringComparer.Ordinal.Equals(request.ComputedKeyAlgorithm, "http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new RequestFailedException(SR.GetString("ID2011", request.ComputedKeyAlgorithm)));
				}
				if (encryptingCredentials == null && scope.SymmetricKeyEncryptionRequired)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new RequestFailedException(SR.GetString("ID4007")));
				}
				if (!request.KeySizeInBits.HasValue)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new RequestFailedException(SR.GetString("ID2059")));
				}
				result = ((request.Entropy == null) ? new Microsoft.IdentityModel.Tokens.SymmetricProofDescriptor(request.KeySizeInBits.Value, encryptingCredentials, requestorProofEncryptingCredentials, request.EncryptWith) : new Microsoft.IdentityModel.Tokens.SymmetricProofDescriptor(request.KeySizeInBits.Value, encryptingCredentials, requestorProofEncryptingCredentials, request.Entropy.GetKeyBytes(), request.EncryptWith));
			}
			else
			{
				StringComparer.Ordinal.Equals(x, "http://schemas.microsoft.com/idfx/keytype/bearer");
			}
			return result;
		}

		protected virtual EncryptingCredentials GetRequestorProofEncryptingCredentials(RequestSecurityToken request)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			if (request.ProofEncryption == null)
			{
				return null;
			}
			X509SecurityToken x509SecurityToken = request.ProofEncryption.GetSecurityToken() as X509SecurityToken;
			if (x509SecurityToken != null)
			{
				return new X509EncryptingCredentials(x509SecurityToken);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new RequestFailedException(SR.GetString("ID2084", request.ProofEncryption.GetSecurityToken())));
		}

		protected virtual Lifetime GetTokenLifetime(Lifetime requestLifetime)
		{
			DateTime dateTime;
			DateTime expires;
			if (requestLifetime != null)
			{
				dateTime = ((!requestLifetime.Created.HasValue) ? DateTime.UtcNow : requestLifetime.Created.Value);
				expires = ((!requestLifetime.Expires.HasValue) ? DateTimeUtil.Add(dateTime, _securityTokenServiceConfiguration.DefaultTokenLifetime) : requestLifetime.Expires.Value);
			}
			else
			{
				dateTime = DateTime.UtcNow;
				expires = DateTimeUtil.Add(dateTime, _securityTokenServiceConfiguration.DefaultTokenLifetime);
			}
			VerifyComputedLifetime(dateTime, expires);
			return new Lifetime(dateTime, expires);
		}

		private void VerifyComputedLifetime(DateTime created, DateTime expires)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (DateTimeUtil.Add(DateTimeUtil.ToUniversalTime(expires), _securityTokenServiceConfiguration.MaxClockSkew) < utcNow)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2075", created, expires, utcNow)));
			}
			if (DateTimeUtil.ToUniversalTime(created) > DateTimeUtil.Add(utcNow + TimeSpan.FromDays(1.0), _securityTokenServiceConfiguration.MaxClockSkew))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2076", created, expires, utcNow)));
			}
			if (expires <= created)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2077", created, expires)));
			}
			if (expires - created > _securityTokenServiceConfiguration.MaximumTokenLifetime)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2078", created, expires, _securityTokenServiceConfiguration.MaximumTokenLifetime)));
			}
		}

		protected virtual RequestSecurityTokenResponse GetResponse(RequestSecurityToken request, Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor != null)
			{
				RequestSecurityTokenResponse requestSecurityTokenResponse = new RequestSecurityTokenResponse(request);
				tokenDescriptor.ApplyTo(requestSecurityTokenResponse);
				if (request.ReplyTo != null)
				{
					requestSecurityTokenResponse.ReplyTo = tokenDescriptor.ReplyToAddress;
				}
				if (!string.IsNullOrEmpty(tokenDescriptor.AppliesToAddress))
				{
					requestSecurityTokenResponse.AppliesTo = new EndpointAddress(tokenDescriptor.AppliesToAddress);
				}
				return requestSecurityTokenResponse;
			}
			return null;
		}

		public virtual RequestSecurityTokenResponse EndCancel(IAsyncResult result)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", "Cancel")));
		}

		protected virtual Scope EndGetScope(IAsyncResult result)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID2081")));
		}

		public virtual RequestSecurityTokenResponse EndIssue(IAsyncResult result)
		{
			if (result == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("result");
			}
			if (!(result is TypedAsyncResult<RequestSecurityTokenResponse>))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2012", typeof(TypedAsyncResult<RequestSecurityTokenResponse>), result.GetType())));
			}
			return TypedAsyncResult<RequestSecurityTokenResponse>.End(result);
		}

		public virtual RequestSecurityTokenResponse EndRenew(IAsyncResult result)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", "Renew")));
		}

		public virtual RequestSecurityTokenResponse EndValidate(IAsyncResult result)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", "Validate")));
		}

		protected abstract Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request);

		protected abstract IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope);

		protected virtual IAsyncResult BeginGetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope, AsyncCallback callback, object state)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID2081")));
		}

		protected virtual IClaimsIdentity EndGetOutputClaimsIdentity(IAsyncResult result)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID2081")));
		}

		public virtual RequestSecurityTokenResponse Issue(IClaimsPrincipal principal, RequestSecurityToken request)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			_principal = principal;
			_request = request;
			ValidateRequest(request);
			Scope scope = GetScope(principal, request);
			if (scope == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2013"));
			}
			Scope = scope;
			SecurityTokenDescriptor = CreateSecurityTokenDescriptor(request, scope);
			if (SecurityTokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2003"));
			}
			if (SecurityTokenDescriptor.SigningCredentials == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2079"));
			}
			if (Scope.TokenEncryptionRequired && SecurityTokenDescriptor.EncryptingCredentials == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4184"));
			}
			Microsoft.IdentityModel.Tokens.SecurityTokenHandler securityTokenHandler = GetSecurityTokenHandler(request.TokenType);
			if (securityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID4010", request.TokenType)));
			}
			_tokenDescriptor.TokenIssuerName = GetValidIssuerName();
			_tokenDescriptor.Lifetime = GetTokenLifetime(request.Lifetime);
			_tokenDescriptor.Proof = GetProofToken(request, scope);
			_tokenDescriptor.Subject = GetOutputClaimsIdentity(principal, request, scope);
			if (!string.IsNullOrEmpty(request.TokenType))
			{
				_tokenDescriptor.TokenType = request.TokenType;
			}
			else
			{
				string[] tokenTypeIdentifiers = securityTokenHandler.GetTokenTypeIdentifiers();
				if (tokenTypeIdentifiers == null || tokenTypeIdentifiers.Length == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4264", request.TokenType)));
				}
				_tokenDescriptor.TokenType = tokenTypeIdentifiers[0];
			}
			_tokenDescriptor.Token = securityTokenHandler.CreateToken(_tokenDescriptor);
			_tokenDescriptor.AttachedReference = securityTokenHandler.CreateSecurityTokenReference(_tokenDescriptor.Token, attached: true);
			_tokenDescriptor.UnattachedReference = securityTokenHandler.CreateSecurityTokenReference(_tokenDescriptor.Token, attached: false);
			if (request.RequestDisplayToken)
			{
				_tokenDescriptor.DisplayToken = GetDisplayToken(request.DisplayTokenLanguage, _tokenDescriptor.Subject);
			}
			return GetResponse(request, _tokenDescriptor);
		}

		protected virtual DisplayToken GetDisplayToken(string requestedDisplayTokenLanguage, IClaimsIdentity subject)
		{
			return null;
		}

		protected virtual Microsoft.IdentityModel.Tokens.SecurityTokenHandler GetSecurityTokenHandler(string requestedTokenType)
		{
			string tokenTypeIdentifier = (string.IsNullOrEmpty(requestedTokenType) ? _securityTokenServiceConfiguration.DefaultTokenType : requestedTokenType);
			return _securityTokenServiceConfiguration.SecurityTokenHandlers[tokenTypeIdentifier];
		}

		private void OnGetScopeComplete(IAsyncResult result)
		{
			if (result == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("result");
			}
			FederatedAsyncState federatedAsyncState = result.AsyncState as FederatedAsyncState;
			if (federatedAsyncState == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2001")));
			}
			Exception ex = null;
			TypedAsyncResult<RequestSecurityTokenResponse> typedAsyncResult = federatedAsyncState.Result as TypedAsyncResult<RequestSecurityTokenResponse>;
			if (typedAsyncResult == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2004", typeof(TypedAsyncResult<RequestSecurityTokenResponse>), federatedAsyncState.Result.GetType())));
			}
			RequestSecurityToken request = federatedAsyncState.Request;
			try
			{
				Scope scope = EndGetScope(result);
				if (scope == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2013"));
				}
				Scope = scope;
				SecurityTokenDescriptor = CreateSecurityTokenDescriptor(request, Scope);
				if (SecurityTokenDescriptor == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2003"));
				}
				if (SecurityTokenDescriptor.SigningCredentials == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2079"));
				}
				if (Scope.TokenEncryptionRequired && SecurityTokenDescriptor.EncryptingCredentials == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4184"));
				}
				Microsoft.IdentityModel.Tokens.SecurityTokenHandler securityTokenHandler = GetSecurityTokenHandler(request?.TokenType);
				if (securityTokenHandler == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID4010", (request == null) ? string.Empty : request.TokenType)));
				}
				federatedAsyncState.SecurityTokenHandler = securityTokenHandler;
				_tokenDescriptor.TokenIssuerName = GetValidIssuerName();
				_tokenDescriptor.Lifetime = GetTokenLifetime(request?.Lifetime);
				_tokenDescriptor.Proof = GetProofToken(request, Scope);
				BeginGetOutputClaimsIdentity(federatedAsyncState.ClaimsPrincipal, federatedAsyncState.Request, scope, OnGetOutputClaimsIdentityComplete, federatedAsyncState);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (ex != null)
			{
				typedAsyncResult.Complete(null, result.CompletedSynchronously, ex);
			}
		}

		private void OnGetOutputClaimsIdentityComplete(IAsyncResult result)
		{
			if (result == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("result");
			}
			FederatedAsyncState federatedAsyncState = result.AsyncState as FederatedAsyncState;
			if (federatedAsyncState == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2001")));
			}
			Microsoft.IdentityModel.Tokens.SecurityTokenHandler securityTokenHandler = federatedAsyncState.SecurityTokenHandler;
			if (securityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2016")));
			}
			Exception exception = null;
			RequestSecurityToken request = federatedAsyncState.Request;
			RequestSecurityTokenResponse result2 = null;
			TypedAsyncResult<RequestSecurityTokenResponse> typedAsyncResult = federatedAsyncState.Result as TypedAsyncResult<RequestSecurityTokenResponse>;
			if (typedAsyncResult == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID2004", typeof(TypedAsyncResult<RequestSecurityTokenResponse>), federatedAsyncState.Result.GetType())));
			}
			try
			{
				if (_tokenDescriptor == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID2003"));
				}
				_tokenDescriptor.Subject = EndGetOutputClaimsIdentity(result);
				if (!string.IsNullOrEmpty(request.TokenType))
				{
					_tokenDescriptor.TokenType = request.TokenType;
				}
				else
				{
					string[] tokenTypeIdentifiers = securityTokenHandler.GetTokenTypeIdentifiers();
					if (tokenTypeIdentifiers == null || tokenTypeIdentifiers.Length == 0)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4264", request.TokenType)));
					}
					_tokenDescriptor.TokenType = tokenTypeIdentifiers[0];
				}
				_tokenDescriptor.Token = securityTokenHandler.CreateToken(_tokenDescriptor);
				_tokenDescriptor.AttachedReference = securityTokenHandler.CreateSecurityTokenReference(_tokenDescriptor.Token, attached: true);
				_tokenDescriptor.UnattachedReference = securityTokenHandler.CreateSecurityTokenReference(_tokenDescriptor.Token, attached: false);
				if (request != null && request.RequestDisplayToken)
				{
					_tokenDescriptor.DisplayToken = GetDisplayToken(request.DisplayTokenLanguage, _tokenDescriptor.Subject);
				}
				result2 = GetResponse(request, _tokenDescriptor);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			typedAsyncResult.Complete(result2, typedAsyncResult.CompletedSynchronously, exception);
		}

		public virtual RequestSecurityTokenResponse Renew(IClaimsPrincipal principal, RequestSecurityToken request)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", (request != null && request.RequestType != null) ? request.RequestType : "Renew")));
		}

		public virtual RequestSecurityTokenResponse Validate(IClaimsPrincipal principal, RequestSecurityToken request)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3141", (request != null && request.RequestType != null) ? request.RequestType : "Validate")));
		}

		protected virtual void ValidateRequest(RequestSecurityToken request)
		{
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2051")));
			}
			if (request.RequestType != null && request.RequestType != "http://schemas.microsoft.com/idfx/requesttype/issue")
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2052")));
			}
			if (request.KeyType != null && !IsKnownType(request.KeyType))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2053")));
			}
			if (StringComparer.Ordinal.Equals(request.KeyType, "http://schemas.microsoft.com/idfx/keytype/bearer") && request.KeySizeInBits.HasValue && request.KeySizeInBits.Value != 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2050")));
			}
			if (GetSecurityTokenHandler(request.TokenType) == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new UnsupportedTokenTypeBadRequestException(request.TokenType));
			}
			request.KeyType = (string.IsNullOrEmpty(request.KeyType) ? "http://schemas.microsoft.com/idfx/keytype/symmetric" : request.KeyType);
			if (!StringComparer.Ordinal.Equals(request.KeyType, "http://schemas.microsoft.com/idfx/keytype/symmetric"))
			{
				return;
			}
			if (request.KeySizeInBits.HasValue)
			{
				if (request.KeySizeInBits.Value > _securityTokenServiceConfiguration.DefaultMaxSymmetricKeySizeInBits)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID2056", request.KeySizeInBits.Value, _securityTokenServiceConfiguration.DefaultMaxSymmetricKeySizeInBits)));
				}
			}
			else
			{
				request.KeySizeInBits = _securityTokenServiceConfiguration.DefaultSymmetricKeySizeInBits;
			}
		}

		private static bool IsKnownType(string keyType)
		{
			if (!StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/symmetric") && !StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/asymmetric"))
			{
				return StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/bearer");
			}
			return true;
		}
	}
}
