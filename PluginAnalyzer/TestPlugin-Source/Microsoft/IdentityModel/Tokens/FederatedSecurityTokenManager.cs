using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Tokens.Saml2;
using Microsoft.IdentityModel.Web;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class FederatedSecurityTokenManager : ServiceCredentialsSecurityTokenManager
	{
		private static string ListenUriProperty = "http://schemas.microsoft.com/ws/2006/05/servicemodel/securitytokenrequirement/ListenUri";

		private static Assembly AssemblyName = typeof(TrustVersion).Assembly;

		private static BindingFlags SetPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;

		private static BindingFlags GetPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty;

		private static Type MessageSecurityTokenVersionType = AssemblyName.GetType("System.ServiceModel.Security.MessageSecurityTokenVersion");

		private static SecurityTokenCache DefaultTokenCache = new MruSecurityTokenCache(10000);

		private static ReadOnlyCollection<CookieTransform> DefaultCookieTransforms = new List<CookieTransform>(new CookieTransform[2]
		{
			new DeflateCookieTransform(),
			new ProtectedDataCookieTransform()
		}).AsReadOnly();

		private ClaimsAuthenticationManager _claimsAuthenticationManager;

		private ExceptionMapper _exceptionMapper = new ExceptionMapper();

		private SecurityTokenResolver _defaultTokenResolver;

		private SecurityTokenHandlerCollection _securityTokenHandlerCollection;

		private object _syncObject = new object();

		private ReadOnlyCollection<CookieTransform> _cookieTransforms;

		private SecurityTokenCache _tokenCache;

		public SecurityTokenHandlerCollection SecurityTokenHandlers => _securityTokenHandlerCollection;

		public ExceptionMapper ExceptionMapper
		{
			get
			{
				return _exceptionMapper;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_exceptionMapper = value;
			}
		}

		public FederatedSecurityTokenManager(ServiceCredentials parentCredentials, SecurityTokenHandlerCollection securityTokenHandlerCollection)
			: this(parentCredentials, securityTokenHandlerCollection, new ClaimsAuthenticationManager())
		{
		}

		public FederatedSecurityTokenManager(ServiceCredentials parentCredentials, SecurityTokenHandlerCollection securityTokenHandlerCollection, ClaimsAuthenticationManager claimsAuthenticationManager)
			: base(parentCredentials)
		{
			if (securityTokenHandlerCollection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlerCollection");
			}
			if (claimsAuthenticationManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claimsAuthenticationManager");
			}
			_securityTokenHandlerCollection = securityTokenHandlerCollection;
			_claimsAuthenticationManager = claimsAuthenticationManager;
			SessionSecurityTokenHandler sessionSecurityTokenHandler = _securityTokenHandlerCollection[typeof(SessionSecurityToken)] as SessionSecurityTokenHandler;
			if (sessionSecurityTokenHandler != null)
			{
				_tokenCache = sessionSecurityTokenHandler.TokenCache;
			}
			else
			{
				_tokenCache = DefaultTokenCache;
			}
			_cookieTransforms = DefaultCookieTransforms;
		}

		public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
		{
			if (tokenRequirement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenRequirement");
			}
			outOfBandTokenResolver = null;
			string tokenType = tokenRequirement.TokenType;
			if (string.IsNullOrEmpty(tokenType))
			{
				return CreateSamlSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
			}
			SecurityTokenHandler securityTokenHandler = _securityTokenHandlerCollection[tokenType];
			SecurityTokenAuthenticator result;
			if (securityTokenHandler != null && securityTokenHandler.CanValidateToken)
			{
				outOfBandTokenResolver = GetDefaultOutOfBandTokenResolver();
				if (StringComparer.Ordinal.Equals(tokenType, "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/UserName"))
				{
					UserNameSecurityTokenHandler userNameSecurityTokenHandler = securityTokenHandler as UserNameSecurityTokenHandler;
					if (userNameSecurityTokenHandler == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4072", securityTokenHandler.GetType(), tokenType, typeof(UserNameSecurityTokenHandler))));
					}
					result = new WrappedUserNameSecurityTokenAuthenticator(userNameSecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
				}
				else if (StringComparer.Ordinal.Equals(tokenType, "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Kerberos"))
				{
					result = CreateInnerSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
				}
				else if (StringComparer.Ordinal.Equals(tokenType, "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Rsa"))
				{
					RsaSecurityTokenHandler rsaSecurityTokenHandler = securityTokenHandler as RsaSecurityTokenHandler;
					if (rsaSecurityTokenHandler == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4072", securityTokenHandler.GetType(), tokenType, typeof(RsaSecurityTokenHandler))));
					}
					result = new WrappedRsaSecurityTokenAuthenticator(rsaSecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
				}
				else if (StringComparer.Ordinal.Equals(tokenType, "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/X509Certificate"))
				{
					X509SecurityTokenHandler x509SecurityTokenHandler = securityTokenHandler as X509SecurityTokenHandler;
					if (x509SecurityTokenHandler == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4072", securityTokenHandler.GetType(), tokenType, typeof(X509SecurityTokenHandler))));
					}
					result = new WrappedX509SecurityTokenAuthenticator(x509SecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
				}
				else if (StringComparer.Ordinal.Equals(tokenType, "urn:oasis:names:tc:SAML:1.0:assertion") || StringComparer.Ordinal.Equals(tokenType, "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1"))
				{
					Saml11SecurityTokenHandler saml11SecurityTokenHandler = securityTokenHandler as Saml11SecurityTokenHandler;
					if (saml11SecurityTokenHandler == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4072", securityTokenHandler.GetType(), tokenType, typeof(Saml11SecurityTokenHandler))));
					}
					if (saml11SecurityTokenHandler.Configuration == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
					}
					result = new WrappedSaml11SecurityTokenAuthenticator(saml11SecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
					outOfBandTokenResolver = saml11SecurityTokenHandler.Configuration.ServiceTokenResolver;
				}
				else if (StringComparer.Ordinal.Equals(tokenType, "urn:oasis:names:tc:SAML:2.0:assertion") || StringComparer.Ordinal.Equals(tokenType, "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0"))
				{
					Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler saml2SecurityTokenHandler = securityTokenHandler as Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler;
					if (saml2SecurityTokenHandler == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4072", securityTokenHandler.GetType(), tokenType, typeof(Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler))));
					}
					if (saml2SecurityTokenHandler.Configuration == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
					}
					result = new WrappedSaml2SecurityTokenAuthenticator(saml2SecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
					outOfBandTokenResolver = saml2SecurityTokenHandler.Configuration.ServiceTokenResolver;
				}
				else if (StringComparer.Ordinal.Equals(tokenType, ServiceModelSecurityTokenTypes.SecureConversation))
				{
					RecipientServiceModelSecurityTokenRequirement recipientServiceModelSecurityTokenRequirement = tokenRequirement as RecipientServiceModelSecurityTokenRequirement;
					if (recipientServiceModelSecurityTokenRequirement == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4240", tokenRequirement.GetType().ToString()));
					}
					result = SetupSecureConversationWrapper(recipientServiceModelSecurityTokenRequirement, securityTokenHandler as SessionSecurityTokenHandler, out outOfBandTokenResolver);
				}
				else
				{
					result = new SecurityTokenAuthenticatorAdapter(securityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
				}
			}
			else if (tokenType == ServiceModelSecurityTokenTypes.SecureConversation || tokenType == ServiceModelSecurityTokenTypes.MutualSslnego || tokenType == ServiceModelSecurityTokenTypes.AnonymousSslnego || tokenType == ServiceModelSecurityTokenTypes.SecurityContext || tokenType == ServiceModelSecurityTokenTypes.Spnego)
			{
				RecipientServiceModelSecurityTokenRequirement recipientServiceModelSecurityTokenRequirement2 = tokenRequirement as RecipientServiceModelSecurityTokenRequirement;
				if (recipientServiceModelSecurityTokenRequirement2 == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4240", tokenRequirement.GetType().ToString()));
				}
				result = SetupSecureConversationWrapper(recipientServiceModelSecurityTokenRequirement2, null, out outOfBandTokenResolver);
			}
			else
			{
				result = CreateInnerSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
			}
			return result;
		}

		private SecurityTokenAuthenticator SetupSecureConversationWrapper(RecipientServiceModelSecurityTokenRequirement tokenRequirement, SessionSecurityTokenHandler tokenHandler, out SecurityTokenResolver outOfBandTokenResolver)
		{
			SecurityTokenAuthenticator securityTokenAuthenticator = base.CreateSecurityTokenAuthenticator((SecurityTokenRequirement)tokenRequirement, out outOfBandTokenResolver);
			SessionSecurityTokenHandler sessionSecurityTokenHandler = tokenHandler;
			bool isSessionMode = true;
			if (tokenRequirement.Properties[ServiceModelSecurityTokenRequirement.SupportSecurityContextCancellationProperty] != null)
			{
				isSessionMode = (bool)tokenRequirement.Properties[ServiceModelSecurityTokenRequirement.SupportSecurityContextCancellationProperty];
			}
			if (tokenHandler == null)
			{
				sessionSecurityTokenHandler = new SessionSecurityTokenHandler(_cookieTransforms, _tokenCache, SessionSecurityTokenHandler.DefaultTokenLifetime);
				sessionSecurityTokenHandler.ContainingCollection = _securityTokenHandlerCollection;
				sessionSecurityTokenHandler.Configuration = _securityTokenHandlerCollection.Configuration;
			}
			FederatedServiceCredentials federatedServiceCredentials = base.ServiceCredentials as FederatedServiceCredentials;
			if (federatedServiceCredentials != null)
			{
				sessionSecurityTokenHandler.Configuration.MaxClockSkew = federatedServiceCredentials.MaxClockSkew;
			}
			SctClaimsHandler sctClaimsHandler = new SctClaimsHandler(_claimsAuthenticationManager, _securityTokenHandlerCollection, GetNormalizedEndpointId(tokenRequirement));
			WrappedSessionSecurityTokenAuthenticator wrappedSessionSecurityTokenAuthenticator = new WrappedSessionSecurityTokenAuthenticator(sessionSecurityTokenHandler, securityTokenAuthenticator, sctClaimsHandler, _exceptionMapper);
			WrappedTokenCache wrappedTokenCache = new WrappedTokenCache(_tokenCache, sctClaimsHandler, isSessionMode);
			SetWrappedTokenCache(wrappedTokenCache, securityTokenAuthenticator, wrappedSessionSecurityTokenAuthenticator, sctClaimsHandler);
			outOfBandTokenResolver = wrappedTokenCache;
			return wrappedSessionSecurityTokenAuthenticator;
		}

		private static void SetWrappedTokenCache(WrappedTokenCache wrappedTokenCache, SecurityTokenAuthenticator sta, WrappedSessionSecurityTokenAuthenticator wssta, SctClaimsHandler claimsHandler)
		{
			Type type = null;
			if (sta.GetType().Name == "SecuritySessionSecurityTokenAuthenticator")
			{
				type = AssemblyName.GetType("System.ServiceModel.Security.SecuritySessionSecurityTokenAuthenticator");
			}
			else if (sta.GetType().Name == "AcceleratedTokenAuthenticator")
			{
				type = AssemblyName.GetType("System.ServiceModel.Security.AcceleratedTokenAuthenticator");
			}
			else if (sta.GetType().Name == "SpnegoTokenAuthenticator")
			{
				type = AssemblyName.GetType("System.ServiceModel.Security.SpnegoTokenAuthenticator");
			}
			else if (sta.GetType().Name == "TlsnegoTokenAuthenticator")
			{
				type = AssemblyName.GetType("System.ServiceModel.Security.TlsnegoTokenAuthenticator");
			}
			if ((object)type != null)
			{
				type.InvokeMember("IssuedTokenCache", SetPropertyFlags, null, sta, new object[1] { wrappedTokenCache }, CultureInfo.InvariantCulture);
				IIssuanceSecurityTokenAuthenticator issuanceSecurityTokenAuthenticator = sta as IIssuanceSecurityTokenAuthenticator;
				if (issuanceSecurityTokenAuthenticator != null)
				{
					issuanceSecurityTokenAuthenticator.IssuedSecurityTokenHandler = claimsHandler.OnTokenIssued;
					issuanceSecurityTokenAuthenticator.RenewedSecurityTokenHandler = claimsHandler.OnTokenRenewed;
				}
			}
		}

		public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
		{
			if (version == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("version");
			}
			TrustVersion trustVersion = null;
			SecureConversationVersion secureConversationVersion = null;
			foreach (string securitySpecification in version.GetSecuritySpecifications())
			{
				if (StringComparer.Ordinal.Equals(securitySpecification, "http://schemas.xmlsoap.org/ws/2005/02/trust"))
				{
					trustVersion = TrustVersion.WSTrustFeb2005;
				}
				else if (StringComparer.Ordinal.Equals(securitySpecification, "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
				{
					trustVersion = TrustVersion.WSTrust13;
				}
				else if (StringComparer.Ordinal.Equals(securitySpecification, "http://schemas.xmlsoap.org/ws/2005/02/sc"))
				{
					secureConversationVersion = SecureConversationVersion.WSSecureConversationFeb2005;
				}
				else if (StringComparer.Ordinal.Equals(securitySpecification, "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512"))
				{
					secureConversationVersion = SecureConversationVersion.WSSecureConversation13;
				}
				if (trustVersion != null && secureConversationVersion != null)
				{
					break;
				}
			}
			if (trustVersion == null)
			{
				trustVersion = TrustVersion.WSTrust13;
			}
			if (secureConversationVersion == null)
			{
				secureConversationVersion = SecureConversationVersion.WSSecureConversation13;
			}
			SecurityTokenSerializerAdapter securityTokenSerializerAdapter = new SecurityTokenSerializerAdapter(_securityTokenHandlerCollection, GetSecurityVersion(version), trustVersion, secureConversationVersion, emitBspAttributes: false, base.ServiceCredentials.IssuedTokenAuthentication.SamlSerializer, base.ServiceCredentials.SecureConversationAuthentication.SecurityStateEncoder, base.ServiceCredentials.SecureConversationAuthentication.SecurityContextClaimTypes);
			securityTokenSerializerAdapter.MapExceptionsToSoapFaults = true;
			securityTokenSerializerAdapter.ExceptionMapper = _exceptionMapper;
			return securityTokenSerializerAdapter;
		}

		protected SecurityTokenResolver GetDefaultOutOfBandTokenResolver()
		{
			if (_defaultTokenResolver == null)
			{
				lock (_syncObject)
				{
					if (_defaultTokenResolver == null)
					{
						List<SecurityToken> list = new List<SecurityToken>();
						if (base.ServiceCredentials.ServiceCertificate.Certificate != null)
						{
							list.Add(new X509SecurityToken(base.ServiceCredentials.ServiceCertificate.Certificate));
						}
						if (base.ServiceCredentials.IssuedTokenAuthentication.KnownCertificates != null && base.ServiceCredentials.IssuedTokenAuthentication.KnownCertificates.Count > 0)
						{
							for (int i = 0; i < base.ServiceCredentials.IssuedTokenAuthentication.KnownCertificates.Count; i++)
							{
								list.Add(new X509SecurityToken(base.ServiceCredentials.IssuedTokenAuthentication.KnownCertificates[i]));
							}
						}
						_defaultTokenResolver = SecurityTokenResolver.CreateDefaultSecurityTokenResolver(list.AsReadOnly(), canMatchLocalId: false);
					}
				}
			}
			return _defaultTokenResolver;
		}

		internal static SecurityVersion GetSecurityVersion(SecurityTokenVersion tokenVersion)
		{
			if (tokenVersion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenVersion");
			}
			if ((object)tokenVersion.GetType() == MessageSecurityTokenVersionType)
			{
				object obj = MessageSecurityTokenVersionType.InvokeMember("SecurityVersion", GetPropertyFlags, null, tokenVersion, null, CultureInfo.InvariantCulture);
				SecurityVersion securityVersion = obj as SecurityVersion;
				if (securityVersion != null)
				{
					return securityVersion;
				}
			}
			else
			{
				if (tokenVersion.GetSecuritySpecifications().Contains("http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd"))
				{
					return SecurityVersion.WSSecurity11;
				}
				if (tokenVersion.GetSecuritySpecifications().Contains("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
				{
					return SecurityVersion.WSSecurity10;
				}
			}
			return SecurityVersion.WSSecurity11;
		}

		private SecurityTokenAuthenticator CreateInnerSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
		{
			SecurityTokenAuthenticator securityTokenAuthenticator = base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
			SctClaimsHandler sctClaimsHandler = new SctClaimsHandler(_claimsAuthenticationManager, _securityTokenHandlerCollection, GetNormalizedEndpointId(tokenRequirement));
			bool isSessionMode = true;
			if (tokenRequirement.Properties[ServiceModelSecurityTokenRequirement.SupportSecurityContextCancellationProperty] != null)
			{
				isSessionMode = (bool)tokenRequirement.Properties[ServiceModelSecurityTokenRequirement.SupportSecurityContextCancellationProperty];
			}
			SetWrappedTokenCache(new WrappedTokenCache(_tokenCache, sctClaimsHandler, isSessionMode), securityTokenAuthenticator, null, sctClaimsHandler);
			return securityTokenAuthenticator;
		}

		private SecurityTokenAuthenticator CreateSamlSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
		{
			outOfBandTokenResolver = null;
			Saml11SecurityTokenHandler saml11SecurityTokenHandler = _securityTokenHandlerCollection["urn:oasis:names:tc:SAML:1.0:assertion"] as Saml11SecurityTokenHandler;
			Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler saml2SecurityTokenHandler = _securityTokenHandlerCollection["urn:oasis:names:tc:SAML:2.0:assertion"] as Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler;
			if (saml11SecurityTokenHandler != null && saml11SecurityTokenHandler.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			if (saml2SecurityTokenHandler != null && saml2SecurityTokenHandler.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			SecurityTokenAuthenticator result;
			if (saml11SecurityTokenHandler != null && saml2SecurityTokenHandler != null)
			{
				WrappedSaml11SecurityTokenAuthenticator wrappedSaml11SecurityTokenAuthenticator = new WrappedSaml11SecurityTokenAuthenticator(saml11SecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
				WrappedSaml2SecurityTokenAuthenticator wrappedSaml2SecurityTokenAuthenticator = new WrappedSaml2SecurityTokenAuthenticator(saml2SecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
				result = new WrappedSamlSecurityTokenAuthenticator(wrappedSaml11SecurityTokenAuthenticator, wrappedSaml2SecurityTokenAuthenticator);
				List<SecurityTokenResolver> list = new List<SecurityTokenResolver>();
				list.Add(saml11SecurityTokenHandler.Configuration.ServiceTokenResolver);
				list.Add(saml2SecurityTokenHandler.Configuration.ServiceTokenResolver);
				outOfBandTokenResolver = new AggregateTokenResolver(list);
			}
			else if (saml11SecurityTokenHandler == null && saml2SecurityTokenHandler != null)
			{
				result = new WrappedSaml2SecurityTokenAuthenticator(saml2SecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
				outOfBandTokenResolver = saml2SecurityTokenHandler.Configuration.ServiceTokenResolver;
			}
			else if (saml11SecurityTokenHandler != null && saml2SecurityTokenHandler == null)
			{
				result = new WrappedSaml11SecurityTokenAuthenticator(saml11SecurityTokenHandler, tokenRequirement, _claimsAuthenticationManager, _exceptionMapper);
				outOfBandTokenResolver = saml11SecurityTokenHandler.Configuration.ServiceTokenResolver;
			}
			else
			{
				result = CreateInnerSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
			}
			return result;
		}

		public static string GetNormalizedEndpointId(SecurityTokenRequirement tokenRequirement)
		{
			if (tokenRequirement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenRequirement");
			}
			Uri uri = null;
			if (tokenRequirement.Properties.ContainsKey(ListenUriProperty))
			{
				uri = tokenRequirement.Properties[ListenUriProperty] as Uri;
			}
			if (uri == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4287", tokenRequirement));
			}
			if (uri.IsDefaultPort)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}://NormalizedHostName{1}", new object[2] { uri.Scheme, uri.AbsolutePath });
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}://NormalizedHostName:{1}{2}", new object[3] { uri.Scheme, uri.Port, uri.AbsolutePath });
		}
	}
}
