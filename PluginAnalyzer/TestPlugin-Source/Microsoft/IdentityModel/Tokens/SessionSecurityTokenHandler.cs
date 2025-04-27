using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Web.Compilation;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Web;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SessionSecurityTokenHandler : SecurityTokenHandler
	{
		private const string DefaultCookieElementName = "Cookie";

		private const string DefaultCookieNamespace = "http://schemas.microsoft.com/ws/2006/05/security";

		public static readonly TimeSpan DefaultLifetime = TimeSpan.FromHours(10.0);

		public static readonly ReadOnlyCollection<CookieTransform> DefaultCookieTransforms = new List<CookieTransform>(new CookieTransform[2]
		{
			new DeflateCookieTransform(),
			new ProtectedDataCookieTransform()
		}).AsReadOnly();

		private static SecureConversationVersion DefaultVersion = SecureConversationVersion.WSSecureConversation13;

		private bool _useWindowsTokenService;

		private SecurityTokenCache _tokenCache;

		private TimeSpan _tokenLifetime = DefaultLifetime;

		private ReadOnlyCollection<CookieTransform> _transforms = DefaultCookieTransforms;

		public virtual string CookieElementName => "Cookie";

		public virtual string CookieNamespace => "http://schemas.microsoft.com/ws/2006/05/security";

		public override bool CanValidateToken => true;

		public override bool CanWriteToken => true;

		public static SecureConversationVersion DefaultSecureConversationVersion => DefaultVersion;

		public static TimeSpan DefaultTokenLifetime => DefaultLifetime;

		public static ReadOnlyCollection<CookieTransform> DefaultTransforms => DefaultCookieTransforms;

		public bool UseWindowsTokenService
		{
			get
			{
				return _useWindowsTokenService;
			}
			set
			{
				_useWindowsTokenService = value;
			}
		}

		public virtual TimeSpan TokenLifetime
		{
			get
			{
				return _tokenLifetime;
			}
			set
			{
				if (value <= TimeSpan.Zero)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID0016"));
				}
				_tokenLifetime = value;
			}
		}

		public SecurityTokenCache TokenCache
		{
			get
			{
				return _tokenCache;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_tokenCache = value;
				_tokenCache.Owner = this;
			}
		}

		public override Type TokenType => typeof(SessionSecurityToken);

		public ReadOnlyCollection<CookieTransform> Transforms => _transforms;

		public SessionSecurityTokenHandler()
			: this(DefaultCookieTransforms)
		{
		}

		public SessionSecurityTokenHandler(ReadOnlyCollection<CookieTransform> transforms)
			: this(transforms, new MruSecurityTokenCache(), DefaultLifetime)
		{
		}

		public SessionSecurityTokenHandler(ReadOnlyCollection<CookieTransform> transforms, SecurityTokenCache tokenCache, TimeSpan tokenLifetime)
		{
			if (transforms == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("transforms");
			}
			if (tokenCache == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenCache");
			}
			if (tokenLifetime <= TimeSpan.Zero)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID0016"));
			}
			_transforms = transforms;
			_tokenCache = tokenCache;
			_tokenLifetime = tokenLifetime;
			_tokenCache.Owner = this;
		}

		public SessionSecurityTokenHandler(XmlNodeList customConfigElements)
			: this()
		{
			if (customConfigElements == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("customConfigElements");
			}
			List<XmlElement> xmlElements = XmlUtil.GetXmlElements(customConfigElements);
			bool flag = false;
			foreach (XmlElement item in xmlElements)
			{
				if (!StringComparer.Ordinal.Equals(item.LocalName, "sessionTokenRequirement"))
				{
					continue;
				}
				if (flag)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7026", "sessionTokenRequirement"));
				}
				_tokenLifetime = DefaultLifetime;
				SecurityTokenCache securityTokenCache = null;
				int num = -1;
				foreach (XmlAttribute attribute in item.Attributes)
				{
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "securityTokenCacheType"))
					{
						string value = attribute.Value;
						if (string.IsNullOrEmpty(value))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7015"));
						}
						CustomTypeElement customTypeElement = new CustomTypeElement(BuildManager.GetType(value, throwOnError: true));
						SecurityTokenCache securityTokenCache2 = CustomTypeElement.Resolve<SecurityTokenCache>(customTypeElement, new object[0]);
						if (securityTokenCache2 == null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7015"));
						}
						securityTokenCache = securityTokenCache2;
						continue;
					}
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "useWindowsTokenService"))
					{
						bool result = false;
						if (!bool.TryParse(attribute.Value, out result))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7021", attribute.Value));
						}
						_useWindowsTokenService = result;
						continue;
					}
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "lifetime"))
					{
						TimeSpan result2 = DefaultLifetime;
						if (!TimeSpan.TryParse(attribute.Value, out result2))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7017", attribute.Value));
						}
						if (result2 < TimeSpan.Zero)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7018"));
						}
						_tokenLifetime = result2;
						continue;
					}
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "securityTokenCacheSize"))
					{
						int result3 = -1;
						if (!int.TryParse(attribute.Value, out result3))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7024", attribute.Value));
						}
						if (result3 < 0)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7025"));
						}
						num = result3;
						continue;
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7004", attribute.LocalName, item.LocalName)));
				}
				if (securityTokenCache != null)
				{
					_tokenCache = securityTokenCache;
				}
				else if (num != -1)
				{
					_tokenCache = new MruSecurityTokenCache(num);
				}
				else
				{
					_tokenCache = new MruSecurityTokenCache();
				}
				_transforms = DefaultCookieTransforms;
				flag = true;
			}
		}

		protected virtual byte[] ApplyTransforms(byte[] cookie, bool outbound)
		{
			byte[] array = cookie;
			if (outbound)
			{
				for (int i = 0; i < _transforms.Count; i++)
				{
					array = _transforms[i].Encode(array);
				}
			}
			else
			{
				for (int num = _transforms.Count; num > 0; num--)
				{
					array = _transforms[num - 1].Decode(array);
				}
			}
			return array;
		}

		public override bool CanReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("SecurityContextToken", "http://schemas.xmlsoap.org/ws/2005/02/sc"))
			{
				return reader.IsStartElement("SecurityContextToken", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512");
			}
			return true;
		}

		public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4272"));
			}
			IClaimsPrincipal claimsPrincipal = ClaimsPrincipal.CreateFromIdentity(tokenDescriptor.Subject);
			if (base.Configuration.SaveBootstrapTokens)
			{
				claimsPrincipal.Identities[0].BootstrapToken = tokenDescriptor.Token;
			}
			DateTime value = (tokenDescriptor.Lifetime.Created.HasValue ? tokenDescriptor.Lifetime.Created.Value : DateTime.UtcNow);
			DateTime value2 = (tokenDescriptor.Lifetime.Expires.HasValue ? tokenDescriptor.Lifetime.Expires.Value : (DateTime.UtcNow + DefaultTokenLifetime));
			return new SessionSecurityToken(claimsPrincipal, null, value, value2);
		}

		public virtual SessionSecurityToken CreateSessionSecurityToken(IClaimsPrincipal principal, string context, string endpointId, DateTime validFrom, DateTime validTo)
		{
			if (principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("principal");
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4272"));
			}
			return new SessionSecurityToken(principal, context, endpointId, validFrom, validTo);
		}

		public virtual SecurityToken ReadToken(byte[] token)
		{
			return null;
		}

		public virtual SecurityToken ReadToken(byte[] token, SecurityTokenResolver tokenResolver)
		{
			SecurityToken securityToken = ReadToken(token);
			if (securityToken != null)
			{
				return securityToken;
			}
			using XmlReader reader = XmlDictionaryReader.CreateTextReader(token, XmlDictionaryReaderQuotas.Max);
			return ReadToken(reader, tokenResolver);
		}

		public override SecurityToken ReadToken(XmlReader reader)
		{
			return ReadToken(reader, EmptySecurityTokenResolver.Instance);
		}

		public override SecurityToken ReadToken(XmlReader reader, SecurityTokenResolver tokenResolver)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (tokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenResolver");
			}
			System.Xml.UniqueId uniqueId = null;
			SessionSecurityToken sessionSecurityToken = null;
			_ = SessionDictionary.Instance;
			XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			SecureConversationVersion version;
			string ns;
			string localname;
			string localname2;
			if (xmlDictionaryReader.IsStartElement("SecurityContextToken", "http://schemas.xmlsoap.org/ws/2005/02/sc"))
			{
				version = SecureConversationVersion.WSSecureConversationFeb2005;
				ns = "http://schemas.xmlsoap.org/ws/2005/02/sc";
				localname = "Identifier";
				localname2 = "Instance";
			}
			else
			{
				if (!xmlDictionaryReader.IsStartElement("SecurityContextToken", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", "SecurityContextToken", xmlDictionaryReader.Name)));
				}
				version = SecureConversationVersion.WSSecureConversation13;
				ns = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512";
				localname = "Identifier";
				localname2 = "Instance";
			}
			string attribute = xmlDictionaryReader.GetAttribute("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
			xmlDictionaryReader.ReadFullStartElement();
			if (!xmlDictionaryReader.IsStartElement(localname, ns))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4230", "Identifier", xmlDictionaryReader.Name)));
			}
			System.Xml.UniqueId uniqueId2 = xmlDictionaryReader.ReadElementContentAsUniqueId();
			if (uniqueId2 == null || string.IsNullOrEmpty(uniqueId2.ToString()))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4242")));
			}
			if (xmlDictionaryReader.IsStartElement(localname2, ns))
			{
				uniqueId = xmlDictionaryReader.ReadElementContentAsUniqueId();
			}
			if (xmlDictionaryReader.IsStartElement(CookieElementName, CookieNamespace))
			{
				SecurityToken token = null;
				SecurityContextKeyIdentifierClause keyIdentifierClause = ((!(uniqueId == null)) ? new SecurityContextKeyIdentifierClause(uniqueId2, uniqueId) : new SecurityContextKeyIdentifierClause(uniqueId2));
				tokenResolver.TryResolveToken(keyIdentifierClause, out token);
				if (token != null)
				{
					SecurityContextSecurityToken securityContextSecurityToken = token as SecurityContextSecurityToken;
					sessionSecurityToken = ((securityContextSecurityToken == null) ? (token as SessionSecurityToken) : new SessionSecurityToken(securityContextSecurityToken, version));
					xmlDictionaryReader.Skip();
				}
				else
				{
					byte[] array = xmlDictionaryReader.ReadElementContentAsBase64();
					if (array == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4237")));
					}
					byte[] cookie = ApplyTransforms(array, outbound: false);
					sessionSecurityToken = CreateCookieSerializer().Deserialize(cookie);
					if (sessionSecurityToken != null && sessionSecurityToken.ContextId != uniqueId2)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4229", sessionSecurityToken.ContextId, uniqueId2)));
					}
					if (sessionSecurityToken != null && sessionSecurityToken.Id != attribute)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4227", sessionSecurityToken.Id, attribute)));
					}
				}
			}
			else
			{
				SecurityToken token2 = null;
				SecurityContextKeyIdentifierClause keyIdentifierClause2 = ((!(uniqueId == null)) ? new SecurityContextKeyIdentifierClause(uniqueId2, uniqueId) : new SecurityContextKeyIdentifierClause(uniqueId2));
				tokenResolver.TryResolveToken(keyIdentifierClause2, out token2);
				if (token2 != null)
				{
					SecurityContextSecurityToken securityContextSecurityToken2 = token2 as SecurityContextSecurityToken;
					sessionSecurityToken = ((securityContextSecurityToken2 == null) ? (token2 as SessionSecurityToken) : new SessionSecurityToken(securityContextSecurityToken2, version));
				}
			}
			xmlDictionaryReader.ReadEndElement();
			if (sessionSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4243")));
			}
			return sessionSecurityToken;
		}

		public virtual SessionSecurityTokenCookieSerializer CreateCookieSerializer()
		{
			SecurityTokenHandlerCollection bootstrapTokenHandlers = base.ContainingCollection ?? SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
			bool saveBootstrapTokens = false;
			string windowsIssuerName = "LOCAL AUTHORITY";
			if (base.Configuration != null)
			{
				saveBootstrapTokens = base.Configuration.SaveBootstrapTokens;
				windowsIssuerName = base.Configuration.IssuerNameRegistry.GetWindowsIssuerName();
			}
			return new SessionSecurityTokenCookieSerializer(bootstrapTokenHandlers, saveBootstrapTokens, _useWindowsTokenService, windowsIssuerName);
		}

		public override string[] GetTokenTypeIdentifiers()
		{
			return new string[3]
			{
				ServiceModelSecurityTokenTypes.SecureConversation,
				"http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512/sct",
				"http://schemas.xmlsoap.org/ws/2005/02/sc/sct"
			};
		}

		protected void SetTransforms(IEnumerable<CookieTransform> transforms)
		{
			_transforms = new List<CookieTransform>(transforms).AsReadOnly();
		}

		public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			SessionSecurityToken sessionSecurityToken = token as SessionSecurityToken;
			if (sessionSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, null, new TokenTraceRecord(token), null);
			}
			ValidateSession(sessionSecurityToken);
			return sessionSecurityToken.ClaimsPrincipal.Identities;
		}

		public virtual ClaimsIdentityCollection ValidateToken(SessionSecurityToken token, string endpointId)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if (endpointId == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("endpointId");
			}
			if (!string.IsNullOrEmpty(token.EndpointId) && token.EndpointId != endpointId)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4291", token)));
			}
			return ValidateToken(token);
		}

		protected virtual void ValidateSession(SessionSecurityToken securityToken)
		{
			if (securityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityToken");
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = DateTimeUtil.Add(utcNow, base.Configuration.MaxClockSkew);
			DateTime dateTime2 = DateTimeUtil.Add(utcNow, -base.Configuration.MaxClockSkew);
			if (securityToken.ValidFrom > dateTime)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenNotYetValidException(SR.GetString("ID4255", securityToken.ValidTo, securityToken.ValidFrom, utcNow)));
			}
			if (securityToken.ValidTo < dateTime2)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenExpiredException(SR.GetString("ID4255", securityToken.ValidTo, securityToken.ValidFrom, utcNow)));
			}
		}

		public virtual byte[] WriteToken(SessionSecurityToken sessionToken)
		{
			if (sessionToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sessionToken");
			}
			using MemoryStream memoryStream = new MemoryStream();
			using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream))
			{
				WriteToken(xmlWriter, sessionToken);
				xmlWriter.Flush();
			}
			return memoryStream.ToArray();
		}

		public override void WriteToken(XmlWriter writer, SecurityToken token)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			SessionSecurityToken sessionSecurityToken = token as SessionSecurityToken;
			if (sessionSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4046", token, TokenType));
			}
			string ns;
			string localName;
			string localName2;
			string localName3;
			if (sessionSecurityToken.SecureConversationVersion == SecureConversationVersion.WSSecureConversationFeb2005)
			{
				ns = "http://schemas.xmlsoap.org/ws/2005/02/sc";
				localName = "SecurityContextToken";
				localName2 = "Identifier";
				localName3 = "Instance";
			}
			else
			{
				if (sessionSecurityToken.SecureConversationVersion != SecureConversationVersion.WSSecureConversation13)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4050"));
				}
				ns = "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512";
				localName = "SecurityContextToken";
				localName2 = "Identifier";
				localName3 = "Instance";
			}
			_ = SessionDictionary.Instance;
			XmlDictionaryWriter xmlDictionaryWriter = ((!(writer is XmlDictionaryWriter)) ? XmlDictionaryWriter.CreateDictionaryWriter(writer) : ((XmlDictionaryWriter)writer));
			xmlDictionaryWriter.WriteStartElement(localName, ns);
			if (sessionSecurityToken.Id != null)
			{
				xmlDictionaryWriter.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", sessionSecurityToken.Id);
			}
			xmlDictionaryWriter.WriteElementString(localName2, ns, sessionSecurityToken.ContextId.ToString());
			if (sessionSecurityToken.KeyGeneration != null)
			{
				xmlDictionaryWriter.WriteStartElement(localName3, ns);
				xmlDictionaryWriter.WriteValue(sessionSecurityToken.KeyGeneration);
				xmlDictionaryWriter.WriteEndElement();
			}
			if (!sessionSecurityToken.IsSessionMode)
			{
				xmlDictionaryWriter.WriteStartElement(CookieElementName, CookieNamespace);
				byte[] cookie = CreateCookieSerializer().Serialize(sessionSecurityToken);
				cookie = ApplyTransforms(cookie, outbound: true);
				xmlDictionaryWriter.WriteBase64(cookie, 0, cookie.Length);
				xmlDictionaryWriter.WriteEndElement();
			}
			xmlDictionaryWriter.WriteEndElement();
			xmlDictionaryWriter.Flush();
		}
	}
}
