using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SessionSecurityToken : SecurityToken
	{
		private SecurityContextSecurityToken _securityContextToken;

		private bool _securityContextTokenWrapper;

		private SecureConversationVersion _scVersion = SessionSecurityTokenHandler.DefaultSecureConversationVersion;

		private string _context;

		private bool _isPersistent;

		private IClaimsPrincipal _claimsPrincipal;

		private object _claimsPrincipalLock = new object();

		private string _endpointId;

		private bool _isSessionMode;

		public IClaimsPrincipal ClaimsPrincipal
		{
			get
			{
				if (_claimsPrincipal == null)
				{
					lock (_claimsPrincipalLock)
					{
						if (_claimsPrincipal == null)
						{
							if (_securityContextToken.AuthorizationPolicies != null && _securityContextToken.AuthorizationPolicies.Count > 0)
							{
								AuthorizationPolicy authorizationPolicy = null;
								for (int i = 0; i < _securityContextToken.AuthorizationPolicies.Count; i++)
								{
									authorizationPolicy = _securityContextToken.AuthorizationPolicies[i] as AuthorizationPolicy;
									if (authorizationPolicy != null)
									{
										break;
									}
								}
								if (authorizationPolicy != null && authorizationPolicy.IdentityCollection != null)
								{
									_claimsPrincipal = Microsoft.IdentityModel.Claims.ClaimsPrincipal.CreateFromIdentities(authorizationPolicy.IdentityCollection);
								}
							}
							if (_claimsPrincipal == null)
							{
								if (_securityContextTokenWrapper)
								{
									_claimsPrincipal = new ClaimsPrincipal();
								}
								else
								{
									_claimsPrincipal = Microsoft.IdentityModel.Claims.ClaimsPrincipal.AnonymousPrincipal;
								}
							}
						}
					}
				}
				return _claimsPrincipal;
			}
		}

		public string Context => _context;

		public System.Xml.UniqueId ContextId => _securityContextToken.ContextId;

		public string EndpointId => _endpointId;

		public DateTime KeyEffectiveTime => _securityContextToken.KeyEffectiveTime;

		public DateTime KeyExpirationTime => _securityContextToken.KeyExpirationTime;

		public System.Xml.UniqueId KeyGeneration => _securityContextToken.KeyGeneration;

		public override string Id => _securityContextToken.Id;

		public bool IsPersistent
		{
			get
			{
				return _isPersistent;
			}
			set
			{
				_isPersistent = value;
			}
		}

		internal bool IsSecurityContextSecurityTokenWrapper => _securityContextTokenWrapper;

		public bool IsSessionMode
		{
			get
			{
				return _isSessionMode;
			}
			set
			{
				_isSessionMode = value;
			}
		}

		public SecureConversationVersion SecureConversationVersion => _scVersion;

		internal SecurityContextSecurityToken SecurityContextSecurityToken => _securityContextToken;

		public override ReadOnlyCollection<SecurityKey> SecurityKeys => _securityContextToken.SecurityKeys;

		public override DateTime ValidFrom => _securityContextToken.ValidFrom;

		public override DateTime ValidTo => _securityContextToken.ValidTo;

		public SessionSecurityToken(IClaimsPrincipal claimsPrincipal)
			: this(claimsPrincipal, null)
		{
		}

		public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, TimeSpan lifetime)
			: this(claimsPrincipal, null, DateTime.UtcNow, DateTimeUtil.AddNonNegative(DateTime.UtcNow, lifetime))
		{
		}

		public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, string context)
			: this(claimsPrincipal, context, DateTime.UtcNow, DateTimeUtil.AddNonNegative(DateTime.UtcNow, SessionSecurityTokenHandler.DefaultTokenLifetime))
		{
		}

		public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, string context, DateTime? validFrom, DateTime? validTo)
			: this(claimsPrincipal, new System.Xml.UniqueId(), context, string.Empty, validFrom, validTo, null)
		{
		}

		public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, string context, string endpointId, DateTime? validFrom, DateTime? validTo)
			: this(claimsPrincipal, new System.Xml.UniqueId(), context, endpointId, validFrom, validTo, null)
		{
		}

		public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, System.Xml.UniqueId contextId, string context, string endpointId, TimeSpan lifetime, SymmetricSecurityKey key)
			: this(claimsPrincipal, contextId, context, endpointId, DateTime.UtcNow, lifetime, key)
		{
		}

		public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, System.Xml.UniqueId contextId, string context, string endpointId, DateTime validFrom, TimeSpan lifetime, SymmetricSecurityKey key)
			: this(claimsPrincipal, contextId, context, endpointId, validFrom, DateTimeUtil.AddNonNegative(validFrom, lifetime), key)
		{
		}

		public SessionSecurityToken(IClaimsPrincipal claimsPrincipal, System.Xml.UniqueId contextId, string context, string endpointId, DateTime? validFrom, DateTime? validTo, SymmetricSecurityKey key)
		{
			if (claimsPrincipal == null || claimsPrincipal.Identities == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claimsPrincipal");
			}
			if (contextId == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("contextId");
			}
			DateTime dateTime = ((!validFrom.HasValue) ? DateTime.UtcNow : DateTimeUtil.ToUniversalTime(validFrom.Value));
			DateTime dateTime2 = ((!validTo.HasValue) ? DateTimeUtil.Add(dateTime, SessionSecurityTokenHandler.DefaultTokenLifetime) : DateTimeUtil.ToUniversalTime(validTo.Value));
			if (dateTime >= dateTime2)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("validFrom");
			}
			if (dateTime2 < DateTime.UtcNow)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("validTo");
			}
			if (endpointId == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("endpointId");
			}
			_claimsPrincipal = claimsPrincipal;
			IAuthorizationPolicy[] list = new IAuthorizationPolicy[2]
			{
				new AuthorizationPolicy(claimsPrincipal.Identities),
				new EndpointAuthorizationPolicy(endpointId)
			};
			byte[] key2 = ((key == null) ? KeyGenerator.GenerateSymmetricKey(128) : key.GetSymmetricKey());
			_securityContextToken = new SecurityContextSecurityToken(contextId, UniqueId.CreateUniqueId(), key2, dateTime, dateTime2, new System.Xml.UniqueId(), dateTime, dateTime2, new ReadOnlyCollection<IAuthorizationPolicy>(list));
			_context = context;
			_endpointId = endpointId;
		}

		internal SessionSecurityToken(System.Xml.UniqueId contextId, string id, string context, byte[] key, string endpointId, DateTime validFrom, DateTime validTo, System.Xml.UniqueId keyGeneration, DateTime keyEffectiveTime, DateTime keyExpirationTime, ReadOnlyCollection<IAuthorizationPolicy> authorizationPolicies)
		{
			if (key == null)
			{
				key = KeyGenerator.GenerateSymmetricKey(128);
			}
			if (endpointId == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("endpointId");
			}
			_securityContextToken = new SecurityContextSecurityToken(contextId, id, key, validFrom, validTo, keyGeneration, keyEffectiveTime, keyExpirationTime, authorizationPolicies);
			_context = context;
			_endpointId = endpointId;
		}

		public SessionSecurityToken(SecurityContextSecurityToken securityContextToken, SecureConversationVersion version)
		{
			if (securityContextToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityContextToken");
			}
			_securityContextToken = securityContextToken;
			_endpointId = GetEndpointId(_securityContextToken) ?? string.Empty;
			_securityContextTokenWrapper = true;
			_scVersion = version;
			_isSessionMode = !securityContextToken.IsCookieMode;
		}

		private string GetEndpointId(SecurityContextSecurityToken sct)
		{
			if (sct == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sct");
			}
			for (int i = 0; i < sct.AuthorizationPolicies.Count; i++)
			{
				EndpointAuthorizationPolicy endpointAuthorizationPolicy = sct.AuthorizationPolicies[i] as EndpointAuthorizationPolicy;
				if (endpointAuthorizationPolicy != null)
				{
					return endpointAuthorizationPolicy.EndpointId;
				}
			}
			return null;
		}
	}
}
