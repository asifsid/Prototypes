using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Diagnostics;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Tokens.Saml11
{
	[ComVisible(true)]
	public class Saml11SecurityTokenHandler : SecurityTokenHandler
	{
		private class WrappedSerializer : SecurityTokenSerializer
		{
			private Saml11SecurityTokenHandler _parent;

			private SamlAssertion _assertion;

			public WrappedSerializer(Saml11SecurityTokenHandler parent, SamlAssertion assertion)
			{
				if (parent == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("parent");
				}
				_parent = parent;
				_assertion = assertion;
			}

			protected override bool CanReadKeyIdentifierClauseCore(XmlReader reader)
			{
				return false;
			}

			protected override bool CanReadKeyIdentifierCore(XmlReader reader)
			{
				return true;
			}

			protected override bool CanReadTokenCore(XmlReader reader)
			{
				return false;
			}

			protected override bool CanWriteKeyIdentifierClauseCore(SecurityKeyIdentifierClause keyIdentifierClause)
			{
				return false;
			}

			protected override bool CanWriteKeyIdentifierCore(SecurityKeyIdentifier keyIdentifier)
			{
				return false;
			}

			protected override bool CanWriteTokenCore(SecurityToken token)
			{
				return false;
			}

			protected override SecurityKeyIdentifierClause ReadKeyIdentifierClauseCore(XmlReader reader)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
			}

			protected override SecurityKeyIdentifier ReadKeyIdentifierCore(XmlReader reader)
			{
				return _parent.ReadSigningKeyInfo(reader, _assertion);
			}

			protected override SecurityToken ReadTokenCore(XmlReader reader, SecurityTokenResolver tokenResolver)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
			}

			protected override void WriteKeyIdentifierClauseCore(XmlWriter writer, SecurityKeyIdentifierClause keyIdentifierClause)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
			}

			protected override void WriteKeyIdentifierCore(XmlWriter writer, SecurityKeyIdentifier keyIdentifier)
			{
				_parent.WriteSigningKeyInfo(writer, keyIdentifier);
			}

			protected override void WriteTokenCore(XmlWriter writer, SecurityToken token)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
			}
		}

		public const string Namespace = "urn:oasis:names:tc:SAML:1.0";

		public const string BearerConfirmationMethod = "urn:oasis:names:tc:SAML:1.0:cm:bearer";

		public const string UnspecifiedAuthenticationMethod = "urn:oasis:names:tc:SAML:1.0:am:unspecified";

		public const string Assertion = "urn:oasis:names:tc:SAML:1.0:assertion";

		private const string Attribute = "saml:Attribute";

		private const string Actor = "Actor";

		private static DateTime WCFMinValue = new DateTime(DateTime.MinValue.Ticks + 864000000000L, DateTimeKind.Utc);

		private static DateTime WCFMaxValue = new DateTime(DateTime.MaxValue.Ticks - 864000000000L, DateTimeKind.Utc);

		private static string[] _tokenTypeIdentifiers = new string[2] { "urn:oasis:names:tc:SAML:1.0:assertion", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1" };

		private SamlSecurityTokenRequirement _samlSecurityTokenRequirement;

		private SecurityTokenSerializer _keyInfoSerializer;

		private object _syncObject = new object();

		public override bool CanValidateToken => true;

		public X509CertificateValidator CertificateValidator
		{
			get
			{
				if (_samlSecurityTokenRequirement.CertificateValidator == null)
				{
					if (base.Configuration != null)
					{
						return base.Configuration.CertificateValidator;
					}
					return null;
				}
				return _samlSecurityTokenRequirement.CertificateValidator;
			}
			set
			{
				_samlSecurityTokenRequirement.CertificateValidator = value;
			}
		}

		public override bool CanWriteToken => true;

		public SecurityTokenSerializer KeyInfoSerializer
		{
			get
			{
				if (_keyInfoSerializer == null)
				{
					lock (_syncObject)
					{
						if (_keyInfoSerializer == null)
						{
							SecurityTokenHandlerCollection securityTokenHandlerCollection = ((base.ContainingCollection != null) ? base.ContainingCollection : SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection());
							_keyInfoSerializer = new SecurityTokenSerializerAdapter(securityTokenHandlerCollection, SecurityVersion.WSSecurity11, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, emitBspAttributes: false, null, null, null);
						}
					}
				}
				return _keyInfoSerializer;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_keyInfoSerializer = value;
			}
		}

		public override Type TokenType => typeof(SamlSecurityToken);

		public SamlSecurityTokenRequirement SamlSecurityTokenRequirement
		{
			get
			{
				return _samlSecurityTokenRequirement;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_samlSecurityTokenRequirement = value;
			}
		}

		public Saml11SecurityTokenHandler()
			: this(new SamlSecurityTokenRequirement())
		{
		}

		public Saml11SecurityTokenHandler(SamlSecurityTokenRequirement samlSecurityTokenRequirement)
		{
			if (samlSecurityTokenRequirement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlSecurityTokenRequirement");
			}
			_samlSecurityTokenRequirement = samlSecurityTokenRequirement;
		}

		public Saml11SecurityTokenHandler(XmlNodeList customConfigElements)
		{
			if (customConfigElements == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("customConfigElements");
			}
			List<XmlElement> xmlElements = XmlUtil.GetXmlElements(customConfigElements);
			bool flag = false;
			foreach (XmlElement item in xmlElements)
			{
				if (!(item.LocalName != "samlSecurityTokenRequirement"))
				{
					if (flag)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7026", "samlSecurityTokenRequirement"));
					}
					_samlSecurityTokenRequirement = new SamlSecurityTokenRequirement(item);
					flag = true;
				}
			}
			if (!flag)
			{
				_samlSecurityTokenRequirement = new SamlSecurityTokenRequirement();
			}
		}

		public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			IEnumerable<SamlStatement> statements = CreateStatements(tokenDescriptor);
			SamlConditions conditions = CreateConditions(tokenDescriptor.Lifetime, tokenDescriptor.AppliesToAddress, tokenDescriptor);
			SamlAdvice advice = CreateAdvice(tokenDescriptor);
			string tokenIssuerName = tokenDescriptor.TokenIssuerName;
			SamlAssertion samlAssertion = CreateAssertion(tokenIssuerName, conditions, advice, statements);
			if (samlAssertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4013")));
			}
			samlAssertion.SigningCredentials = GetSigningCredentials(tokenDescriptor);
			SecurityToken securityToken = new SamlSecurityToken(samlAssertion);
			Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = GetEncryptingCredentials(tokenDescriptor);
			if (encryptingCredentials != null)
			{
				securityToken = new EncryptedSecurityToken(securityToken, encryptingCredentials);
			}
			return securityToken;
		}

		protected virtual Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials GetEncryptingCredentials(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = null;
			if (tokenDescriptor.EncryptingCredentials != null)
			{
				encryptingCredentials = tokenDescriptor.EncryptingCredentials;
				if (encryptingCredentials.SecurityKey is AsymmetricSecurityKey)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4178")));
				}
			}
			return encryptingCredentials;
		}

		protected virtual SigningCredentials GetSigningCredentials(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			return tokenDescriptor.SigningCredentials;
		}

		protected virtual SamlAdvice CreateAdvice(SecurityTokenDescriptor tokenDescriptor)
		{
			return null;
		}

		protected virtual SamlAssertion CreateAssertion(string issuer, SamlConditions conditions, SamlAdvice advice, IEnumerable<SamlStatement> statements)
		{
			return new Saml11Assertion(UniqueId.CreateRandomId(), issuer, DateTime.UtcNow, conditions, advice, statements);
		}

		public override SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			return token.CreateKeyIdentifierClause<SamlAssertionKeyIdentifierClause>();
		}

		protected virtual SamlConditions CreateConditions(Lifetime tokenLifetime, string relyingPartyAddress, SecurityTokenDescriptor tokenDescriptor)
		{
			SamlConditions samlConditions = new SamlConditions();
			if (tokenLifetime != null)
			{
				if (tokenLifetime.Created.HasValue)
				{
					samlConditions.NotBefore = tokenLifetime.Created.Value;
				}
				if (tokenLifetime.Expires.HasValue)
				{
					samlConditions.NotOnOrAfter = tokenLifetime.Expires.Value;
				}
			}
			if (!string.IsNullOrEmpty(relyingPartyAddress))
			{
				samlConditions.Conditions.Add(new SamlAudienceRestrictionCondition(new Uri[1]
				{
					new Uri(relyingPartyAddress)
				}));
			}
			return samlConditions;
		}

		protected virtual IEnumerable<SamlStatement> CreateStatements(SecurityTokenDescriptor tokenDescriptor)
		{
			Collection<SamlStatement> collection = new Collection<SamlStatement>();
			SamlSubject samlSubject = CreateSamlSubject(tokenDescriptor);
			SamlAttributeStatement samlAttributeStatement = CreateAttributeStatement(samlSubject, tokenDescriptor.Subject, tokenDescriptor);
			if (samlAttributeStatement != null)
			{
				collection.Add(samlAttributeStatement);
			}
			SamlAuthenticationStatement samlAuthenticationStatement = CreateAuthenticationStatement(samlSubject, tokenDescriptor.AuthenticationInfo, tokenDescriptor);
			if (samlAuthenticationStatement != null)
			{
				collection.Add(samlAuthenticationStatement);
			}
			return collection;
		}

		protected virtual SamlAuthenticationStatement CreateAuthenticationStatement(SamlSubject samlSubject, AuthenticationInformation authInfo, SecurityTokenDescriptor tokenDescriptor)
		{
			if (samlSubject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlSubject");
			}
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			if (tokenDescriptor.Subject == null)
			{
				return null;
			}
			string text = null;
			string text2 = null;
			IEnumerable<Claim> source = tokenDescriptor.Subject.Claims.Where((Claim c) => c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod");
			if (source.Count() > 0)
			{
				text = source.First().Value;
			}
			source = tokenDescriptor.Subject.Claims.Where((Claim c) => c.ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant");
			if (source.Count() > 0)
			{
				text2 = source.First().Value;
			}
			if (text == null && text2 == null)
			{
				return null;
			}
			if (text == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4270", "AuthenticationMethod", "SAML11"));
			}
			if (text2 == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4270", "AuthenticationInstant", "SAML11"));
			}
			DateTime authenticationInstant = DateTime.ParseExact(text2, DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
			if (authInfo == null)
			{
				return new SamlAuthenticationStatement(samlSubject, DenormalizeAuthenticationType(text), authenticationInstant, null, null, null);
			}
			return new SamlAuthenticationStatement(samlSubject, DenormalizeAuthenticationType(text), authenticationInstant, authInfo.DnsName, authInfo.Address, null);
		}

		protected virtual SamlAttributeStatement CreateAttributeStatement(SamlSubject samlSubject, IClaimsIdentity subject, SecurityTokenDescriptor tokenDescriptor)
		{
			if (subject == null)
			{
				return null;
			}
			if (samlSubject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlSubject");
			}
			if (subject.Claims != null)
			{
				List<SamlAttribute> list = new List<SamlAttribute>(subject.Claims.Count);
				foreach (Claim claim in subject.Claims)
				{
					if (claim != null && claim.ClaimType != "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
					{
						switch (claim.ClaimType)
						{
						case "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant":
						case "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod":
							continue;
						}
						list.Add(CreateAttribute(claim, tokenDescriptor));
					}
				}
				AddDelegateToAttributes(subject, list, tokenDescriptor);
				ICollection<SamlAttribute> collection = CollectAttributeValues(list);
				if (collection.Count > 0)
				{
					return new SamlAttributeStatement(samlSubject, collection);
				}
			}
			return null;
		}

		protected virtual ICollection<SamlAttribute> CollectAttributeValues(ICollection<SamlAttribute> attributes)
		{
			Dictionary<SamlAttributeKeyComparer.AttributeKey, SamlAttribute> dictionary = new Dictionary<SamlAttributeKeyComparer.AttributeKey, SamlAttribute>(attributes.Count, new SamlAttributeKeyComparer());
			foreach (SamlAttribute attribute in attributes)
			{
				Saml11Attribute saml11Attribute = attribute as Saml11Attribute;
				if (saml11Attribute == null)
				{
					continue;
				}
				SamlAttributeKeyComparer.AttributeKey key = new SamlAttributeKeyComparer.AttributeKey(saml11Attribute);
				if (dictionary.ContainsKey(key))
				{
					foreach (string attributeValue in saml11Attribute.AttributeValues)
					{
						dictionary[key].AttributeValues.Add(attributeValue);
					}
				}
				else
				{
					dictionary.Add(key, saml11Attribute);
				}
			}
			return dictionary.Values;
		}

		protected virtual void AddDelegateToAttributes(IClaimsIdentity subject, ICollection<SamlAttribute> attributes, SecurityTokenDescriptor tokenDescriptor)
		{
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subject");
			}
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			if (subject.Actor == null)
			{
				return;
			}
			List<SamlAttribute> list = new List<SamlAttribute>(subject.Actor.Claims.Count);
			foreach (Claim claim in subject.Actor.Claims)
			{
				if (claim != null)
				{
					list.Add(CreateAttribute(claim, tokenDescriptor));
				}
			}
			AddDelegateToAttributes(subject.Actor, list, tokenDescriptor);
			ICollection<SamlAttribute> attributes2 = CollectAttributeValues(list);
			attributes.Add(CreateAttribute(new Claim("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor", CreateXmlStringFromAttributes(attributes2), "http://www.w3.org/2001/XMLSchema#string"), tokenDescriptor));
		}

		protected virtual SamlSubject CreateSamlSubject(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			SamlSubject samlSubject = new SamlSubject();
			Claim claim = null;
			if (tokenDescriptor.Subject != null && tokenDescriptor.Subject.Claims != null)
			{
				foreach (Claim claim2 in tokenDescriptor.Subject.Claims)
				{
					if (claim2.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
					{
						if (claim != null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4139")));
						}
						claim = claim2;
					}
				}
			}
			if (claim != null)
			{
				samlSubject.Name = claim.Value;
				if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"))
				{
					samlSubject.NameFormat = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"];
				}
				if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"))
				{
					samlSubject.NameQualifier = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"];
				}
			}
			if (tokenDescriptor.Proof != null)
			{
				samlSubject.KeyIdentifier = tokenDescriptor.Proof.KeyIdentifier;
				samlSubject.ConfirmationMethods.Add(SamlConstants.HolderOfKey);
			}
			else
			{
				samlSubject.ConfirmationMethods.Add("urn:oasis:names:tc:SAML:1.0:cm:bearer");
			}
			return samlSubject;
		}

		protected virtual string CreateXmlStringFromAttributes(IEnumerable<SamlAttribute> attributes)
		{
			bool flag = false;
			using MemoryStream memoryStream = new MemoryStream();
			using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false))
			{
				foreach (SamlAttribute attribute in attributes)
				{
					if (attribute != null)
					{
						if (!flag)
						{
							xmlDictionaryWriter.WriteStartElement("Actor");
							flag = true;
						}
						WriteAttribute(xmlDictionaryWriter, attribute);
					}
				}
				if (flag)
				{
					xmlDictionaryWriter.WriteEndElement();
				}
				xmlDictionaryWriter.Flush();
			}
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}

		protected virtual SamlAttribute CreateAttribute(Claim claim, SecurityTokenDescriptor tokenDescriptor)
		{
			if (claim == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claim");
			}
			int num = claim.ClaimType.LastIndexOf('/');
			if (num == 0 || num == -1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("claimType", SR.GetString("ID4216", claim.ClaimType));
			}
			if (num == claim.ClaimType.Length - 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("claimType", SR.GetString("ID4216", claim.ClaimType));
			}
			string text = claim.ClaimType.Substring(0, num);
			if (text.EndsWith("/", StringComparison.Ordinal))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("claim", SR.GetString("ID4213", claim.ClaimType));
			}
			string attributeName = claim.ClaimType.Substring(num + 1, claim.ClaimType.Length - (num + 1));
			Saml11Attribute saml11Attribute = new Saml11Attribute(text, attributeName, new string[1] { claim.Value });
			if (!StringComparer.Ordinal.Equals("LOCAL AUTHORITY", claim.OriginalIssuer))
			{
				saml11Attribute.OriginalIssuer = claim.OriginalIssuer;
			}
			saml11Attribute.AttributeValueXsiType = claim.ValueType;
			return saml11Attribute;
		}

		protected override void DetectReplayedTokens(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			SamlSecurityToken samlSecurityToken = token as SamlSecurityToken;
			if (samlSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID1067", token.GetType().ToString()));
			}
			if (samlSecurityToken.SecurityKeys.Count != 0)
			{
				return;
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			if (base.Configuration.TokenReplayCache == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4278"));
			}
			if (string.IsNullOrEmpty(samlSecurityToken.Assertion.AssertionId))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID1063")));
			}
			StringBuilder stringBuilder = new StringBuilder();
			string key;
			using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.NewSha256())
			{
				if (string.IsNullOrEmpty(samlSecurityToken.Assertion.Issuer))
				{
					stringBuilder.AppendFormat("{0}{1}", samlSecurityToken.Assertion.AssertionId, _tokenTypeIdentifiers[0]);
				}
				else
				{
					stringBuilder.AppendFormat("{0}{1}{2}", samlSecurityToken.Assertion.AssertionId, samlSecurityToken.Assertion.Issuer, _tokenTypeIdentifiers[0]);
				}
				key = Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
			}
			if (base.Configuration.TokenReplayCache.TryFind(key))
			{
				if (string.IsNullOrEmpty(samlSecurityToken.Assertion.Issuer))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenReplayDetectedException(SR.GetString("ID1062", typeof(SamlSecurityToken).ToString(), samlSecurityToken.Assertion.AssertionId, "")));
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenReplayDetectedException(SR.GetString("ID1062", typeof(SamlSecurityToken).ToString(), samlSecurityToken.Assertion.AssertionId, samlSecurityToken.Assertion.Issuer)));
			}
			base.Configuration.TokenReplayCache.TryAdd(key, null, DateTimeUtil.Add(GetCacheExpirationTime(samlSecurityToken), base.Configuration.MaxClockSkew));
		}

		protected virtual DateTime GetCacheExpirationTime(SamlSecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			DateTime dateTime = DateTimeUtil.Add(DateTime.UtcNow, base.Configuration.TokenReplayCacheExpirationPeriod);
			if (DateTime.Compare(dateTime, token.ValidTo) < 0)
			{
				return dateTime;
			}
			return token.ValidTo;
		}

		protected virtual void ValidateConditions(SamlConditions conditions, bool enforceAudienceRestriction)
		{
			if (conditions != null)
			{
				DateTime utcNow = DateTime.UtcNow;
				_ = conditions.NotBefore;
				if (DateTimeUtil.Add(utcNow, base.Configuration.MaxClockSkew) < conditions.NotBefore)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenNotYetValidException(SR.GetString("ID4222", conditions.NotBefore, utcNow)));
				}
				_ = conditions.NotOnOrAfter;
				if (DateTimeUtil.Add(utcNow, base.Configuration.MaxClockSkew.Negate()) >= conditions.NotOnOrAfter)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenExpiredException(SR.GetString("ID4223", conditions.NotOnOrAfter, utcNow)));
				}
			}
			if (!enforceAudienceRestriction)
			{
				return;
			}
			if (base.Configuration == null || base.Configuration.AudienceRestriction.AllowedAudienceUris.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1032")));
			}
			bool flag = false;
			if (conditions != null && conditions.Conditions != null)
			{
				foreach (SamlCondition condition in conditions.Conditions)
				{
					SamlAudienceRestrictionCondition samlAudienceRestrictionCondition = condition as SamlAudienceRestrictionCondition;
					if (samlAudienceRestrictionCondition != null)
					{
						_samlSecurityTokenRequirement.ValidateAudienceRestriction(base.Configuration.AudienceRestriction.AllowedAudienceUris, samlAudienceRestrictionCondition.Audiences);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new AudienceUriValidationFailedException(SR.GetString("ID1035")));
			}
		}

		public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			SamlSecurityToken samlSecurityToken = token as SamlSecurityToken;
			if (samlSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID1033", token.GetType().ToString()));
			}
			if (samlSecurityToken.Assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID1034"));
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, SR.GetString("TraceValidateToken"), new TokenTraceRecord(token), null);
			}
			Saml11Assertion saml11Assertion = samlSecurityToken.Assertion as Saml11Assertion;
			if (samlSecurityToken.Assertion.SigningToken == null && (saml11Assertion == null || saml11Assertion.IssuerToken == null))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4220")));
			}
			ValidateConditions(samlSecurityToken.Assertion.Conditions, _samlSecurityTokenRequirement.ShouldEnforceAudienceRestriction(base.Configuration.AudienceRestriction.AudienceMode, samlSecurityToken));
			X509SecurityToken x509SecurityToken = null;
			if (saml11Assertion != null && saml11Assertion.IssuerToken != null)
			{
				x509SecurityToken = saml11Assertion.IssuerToken as X509SecurityToken;
			}
			else if (samlSecurityToken.Assertion.SigningToken != null)
			{
				x509SecurityToken = samlSecurityToken.Assertion.SigningToken as X509SecurityToken;
			}
			if (x509SecurityToken != null)
			{
				try
				{
					CertificateValidator.Validate(x509SecurityToken.Certificate);
				}
				catch (SecurityTokenValidationException innerException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4257", X509Util.GetCertificateId(x509SecurityToken.Certificate)), innerException));
				}
			}
			IClaimsIdentity claimsIdentity = CreateClaims(samlSecurityToken);
			if (_samlSecurityTokenRequirement.MapToWindows)
			{
				WindowsClaimsIdentity windowsClaimsIdentity = WindowsClaimsIdentity.CreateFromUpn(FindUpn(claimsIdentity), "Federation", _samlSecurityTokenRequirement.UseWindowsTokenService, base.Configuration.IssuerNameRegistry.GetWindowsIssuerName());
				windowsClaimsIdentity.Claims.CopyRange(claimsIdentity.Claims);
				claimsIdentity = windowsClaimsIdentity;
			}
			if (base.Configuration.DetectReplayedTokens)
			{
				DetectReplayedTokens(samlSecurityToken);
			}
			if (base.Configuration.SaveBootstrapTokens)
			{
				claimsIdentity.BootstrapToken = token;
			}
			return new ClaimsIdentityCollection(new IClaimsIdentity[1] { claimsIdentity });
		}

		protected virtual string FindUpn(IClaimsIdentity claimsIdentity)
		{
			return WindowsMappingOperations.FindUpn(claimsIdentity);
		}

		protected virtual IClaimsIdentity CreateClaims(SamlSecurityToken samlSecurityToken)
		{
			if (samlSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlSecurityToken");
			}
			if (samlSecurityToken.Assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("samlSecurityToken", SR.GetString("ID1034"));
			}
			IClaimsIdentity claimsIdentity = new ClaimsIdentity("Federation", _samlSecurityTokenRequirement.NameClaimType, _samlSecurityTokenRequirement.RoleClaimType);
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			if (base.Configuration.IssuerNameRegistry == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4277"));
			}
			Saml11Assertion saml11Assertion = samlSecurityToken.Assertion as Saml11Assertion;
			string issuerName;
			if (saml11Assertion != null && saml11Assertion.IssuerToken != null)
			{
				issuerName = base.Configuration.IssuerNameRegistry.GetIssuerName(saml11Assertion.IssuerToken, saml11Assertion.Issuer);
				if (string.IsNullOrEmpty(issuerName))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4175")));
				}
			}
			else
			{
				issuerName = base.Configuration.IssuerNameRegistry.GetIssuerName(samlSecurityToken.Assertion.SigningToken, samlSecurityToken.Assertion.Issuer);
				if (string.IsNullOrEmpty(issuerName))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4175")));
				}
			}
			ProcessStatement(samlSecurityToken.Assertion.Statements, claimsIdentity, issuerName);
			return claimsIdentity;
		}

		protected virtual string DenormalizeAuthenticationType(string normalizedAuthenticationType)
		{
			return AuthenticationTypeMaps.Denormalize(normalizedAuthenticationType, AuthenticationTypeMaps.Saml11);
		}

		protected virtual string NormalizeAuthenticationType(string saml11AuthenticationMethod)
		{
			return AuthenticationTypeMaps.Normalize(saml11AuthenticationMethod, AuthenticationTypeMaps.Saml11);
		}

		protected virtual void ProcessStatement(IList<SamlStatement> statements, IClaimsIdentity subject, string issuer)
		{
			if (statements == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statements");
			}
			Collection<SamlAuthenticationStatement> collection = new Collection<SamlAuthenticationStatement>();
			ValidateStatements(statements);
			foreach (SamlStatement statement in statements)
			{
				SamlAttributeStatement samlAttributeStatement = statement as SamlAttributeStatement;
				if (samlAttributeStatement != null)
				{
					ProcessAttributeStatement(samlAttributeStatement, subject, issuer);
					continue;
				}
				SamlAuthenticationStatement samlAuthenticationStatement = statement as SamlAuthenticationStatement;
				if (samlAuthenticationStatement != null)
				{
					collection.Add(samlAuthenticationStatement);
					continue;
				}
				SamlAuthorizationDecisionStatement samlAuthorizationDecisionStatement = statement as SamlAuthorizationDecisionStatement;
				if (samlAuthorizationDecisionStatement != null)
				{
					ProcessAuthorizationDecisionStatement(samlAuthorizationDecisionStatement, subject, issuer);
				}
			}
			foreach (SamlAuthenticationStatement item in collection)
			{
				if (item != null)
				{
					ProcessAuthenticationStatement(item, subject, issuer);
				}
			}
		}

		protected virtual void ProcessAttributeStatement(SamlAttributeStatement samlStatement, IClaimsIdentity subject, string issuer)
		{
			if (samlStatement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlStatement");
			}
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subject");
			}
			ProcessSamlSubject(samlStatement.SamlSubject, subject, issuer);
			foreach (SamlAttribute attribute in samlStatement.Attributes)
			{
				string text;
				if (string.IsNullOrEmpty(attribute.Namespace))
				{
					text = attribute.Name;
				}
				else
				{
					if (StringComparer.Ordinal.Equals(attribute.Name, "NameIdentifier"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID4094")));
					}
					int num = attribute.Namespace.LastIndexOf('/');
					text = ((num != -1 && num == attribute.Namespace.Length - 1) ? (attribute.Namespace + attribute.Name) : (attribute.Namespace + "/" + attribute.Name));
				}
				if (text == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
				{
					if (subject.Actor != null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4034"));
					}
					SetDelegateFromAttribute(attribute, subject, issuer);
					continue;
				}
				for (int i = 0; i < attribute.AttributeValues.Count; i++)
				{
					if (!StringComparer.Ordinal.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", text) || GetClaim(subject, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") == null)
					{
						string originalIssuer = issuer;
						Saml11Attribute saml11Attribute = attribute as Saml11Attribute;
						if (saml11Attribute != null && saml11Attribute.OriginalIssuer != null)
						{
							originalIssuer = saml11Attribute.OriginalIssuer;
						}
						string valueType = "http://www.w3.org/2001/XMLSchema#string";
						if (saml11Attribute != null)
						{
							valueType = saml11Attribute.AttributeValueXsiType;
						}
						subject.Claims.Add(new Claim(text, attribute.AttributeValues[i], valueType, issuer, originalIssuer));
					}
				}
			}
		}

		private static Claim GetClaim(IClaimsIdentity subject, string claimType)
		{
			foreach (Claim claim in subject.Claims)
			{
				if (StringComparer.Ordinal.Equals(claimType, claim.ClaimType))
				{
					return claim;
				}
			}
			return null;
		}

		protected virtual void ProcessSamlSubject(SamlSubject samlSubject, IClaimsIdentity subject, string issuer)
		{
			if (samlSubject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlSubject");
			}
			Claim claim = GetClaim(subject, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
			if (claim == null && !string.IsNullOrEmpty(samlSubject.Name))
			{
				Claim claim2 = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", samlSubject.Name, "http://www.w3.org/2001/XMLSchema#string", issuer);
				if (samlSubject.NameFormat != null)
				{
					claim2.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"] = samlSubject.NameFormat;
				}
				if (samlSubject.NameQualifier != null)
				{
					claim2.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"] = samlSubject.NameQualifier;
				}
				subject.Claims.Add(claim2);
			}
		}

		protected virtual void ProcessAuthenticationStatement(SamlAuthenticationStatement samlStatement, IClaimsIdentity subject, string issuer)
		{
			if (samlStatement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlStatement");
			}
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subject");
			}
			ProcessSamlSubject(samlStatement.SamlSubject, subject, issuer);
			subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", NormalizeAuthenticationType(samlStatement.AuthenticationMethod), "http://www.w3.org/2001/XMLSchema#string", issuer));
			subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(samlStatement.AuthenticationInstant.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime", issuer));
		}

		protected virtual void ProcessAuthorizationDecisionStatement(SamlAuthorizationDecisionStatement samlStatement, IClaimsIdentity subject, string issuer)
		{
		}

		protected virtual void SetDelegateFromAttribute(SamlAttribute attribute, IClaimsIdentity subject, string issuer)
		{
			if (subject == null || attribute == null || attribute.AttributeValues == null || attribute.AttributeValues.Count < 1)
			{
				return;
			}
			Collection<Claim> collection = new Collection<Claim>();
			SamlAttribute samlAttribute = null;
			foreach (string attributeValue in attribute.AttributeValues)
			{
				if (attributeValue == null || attributeValue.Length <= 0)
				{
					continue;
				}
				using XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(attributeValue), XmlDictionaryReaderQuotas.Max);
				xmlDictionaryReader.MoveToContent();
				xmlDictionaryReader.ReadStartElement("Actor");
				while (xmlDictionaryReader.IsStartElement("saml:Attribute"))
				{
					SamlAttribute samlAttribute2 = ReadAttribute(xmlDictionaryReader);
					if (samlAttribute2 == null)
					{
						continue;
					}
					string text = (string.IsNullOrEmpty(samlAttribute2.Namespace) ? samlAttribute2.Name : (samlAttribute2.Namespace + "/" + samlAttribute2.Name));
					if (text == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
					{
						if (samlAttribute != null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4034"));
						}
						samlAttribute = samlAttribute2;
						continue;
					}
					string valueType = "http://www.w3.org/2001/XMLSchema#string";
					string text2 = null;
					Saml11Attribute saml11Attribute = samlAttribute2 as Saml11Attribute;
					if (saml11Attribute != null)
					{
						valueType = saml11Attribute.AttributeValueXsiType;
						text2 = saml11Attribute.OriginalIssuer;
					}
					for (int i = 0; i < samlAttribute2.AttributeValues.Count; i++)
					{
						Claim item = ((!string.IsNullOrEmpty(text2)) ? new Claim(text, samlAttribute2.AttributeValues[i], valueType, issuer, text2) : new Claim(text, samlAttribute2.AttributeValues[i], valueType, issuer));
						collection.Add(item);
					}
				}
				xmlDictionaryReader.ReadEndElement();
			}
			subject.Actor = new ClaimsIdentity(collection, "Federation");
			SetDelegateFromAttribute(samlAttribute, subject.Actor, issuer);
		}

		public override bool CanReadToken(XmlReader reader)
		{
			return reader?.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:1.0:assertion") ?? false;
		}

		public override SecurityToken ReadToken(XmlReader reader)
		{
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			if (base.Configuration.IssuerTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4275"));
			}
			Saml11Assertion saml11Assertion = ReadAssertion(reader);
			TryResolveIssuerToken(saml11Assertion, base.Configuration.IssuerTokenResolver, out var token);
			saml11Assertion.IssuerToken = token;
			return new SamlSecurityToken(saml11Assertion);
		}

		protected virtual SamlAction ReadAction(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (reader.IsStartElement("Action", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				string attribute = reader.GetAttribute("Namespace", null);
				reader.MoveToContent();
				string text = reader.ReadString();
				if (string.IsNullOrEmpty(text))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4073")));
				}
				reader.MoveToContent();
				reader.ReadEndElement();
				return new SamlAction(text, attribute);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4065", "Action", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
		}

		protected virtual void WriteAction(XmlWriter writer, SamlAction action)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (action == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("action");
			}
			writer.WriteStartElement("saml", "Action", "urn:oasis:names:tc:SAML:1.0:assertion");
			if (!string.IsNullOrEmpty(action.Namespace))
			{
				writer.WriteAttributeString("Namespace", null, action.Namespace);
			}
			writer.WriteString(action.Action);
			writer.WriteEndElement();
		}

		protected virtual SamlAdvice ReadAdvice(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Advice", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4065", "Advice", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			if (reader.IsEmptyElement)
			{
				reader.MoveToContent();
				reader.Read();
				return new SamlAdvice();
			}
			reader.MoveToContent();
			reader.Read();
			Collection<string> collection = new Collection<string>();
			Collection<SamlAssertion> collection2 = new Collection<SamlAssertion>();
			while (reader.IsStartElement())
			{
				if (reader.IsStartElement("AssertionIDReference", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					collection.Add(reader.ReadString());
					reader.ReadEndElement();
					continue;
				}
				if (reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					SamlAssertion item = ReadAssertion(reader);
					collection2.Add(item);
					continue;
				}
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8005", reader.LocalName, reader.NamespaceURI));
				}
				reader.Skip();
			}
			reader.MoveToContent();
			reader.ReadEndElement();
			return new SamlAdvice(collection, collection2);
		}

		protected virtual void WriteAdvice(XmlWriter writer, SamlAdvice advice)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (advice == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("advice");
			}
			writer.WriteStartElement("saml", "Advice", "urn:oasis:names:tc:SAML:1.0:assertion");
			if (advice.AssertionIdReferences.Count > 0)
			{
				foreach (string assertionIdReference in advice.AssertionIdReferences)
				{
					if (string.IsNullOrEmpty(assertionIdReference))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4079")));
					}
					writer.WriteElementString("saml", "AssertionIDReference", "urn:oasis:names:tc:SAML:1.0:assertion", assertionIdReference);
				}
			}
			if (advice.Assertions.Count > 0)
			{
				foreach (SamlAssertion assertion in advice.Assertions)
				{
					WriteAssertion(writer, assertion);
				}
			}
			writer.WriteEndElement();
		}

		protected virtual Saml11Assertion ReadAssertion(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			if (base.Configuration.IssuerTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4275"));
			}
			Saml11Assertion saml11Assertion = new Saml11Assertion();
			EnvelopedSignatureReader envelopedSignatureReader = new EnvelopedSignatureReader(reader, new WrappedSerializer(this, saml11Assertion), base.Configuration.IssuerTokenResolver, requireSignature: false, automaticallyReadSignature: true, resolveIntrinsicSigningKeys: false);
			if (!envelopedSignatureReader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4065", "Assertion", "urn:oasis:names:tc:SAML:1.0:assertion", envelopedSignatureReader.LocalName, envelopedSignatureReader.NamespaceURI)));
			}
			string attribute = envelopedSignatureReader.GetAttribute("MajorVersion", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4075", "MajorVersion")));
			}
			int num = XmlConvert.ToInt32(attribute);
			attribute = envelopedSignatureReader.GetAttribute("MinorVersion", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4075", "MinorVersion")));
			}
			int num2 = XmlConvert.ToInt32(attribute);
			if (num != 1 || num2 != 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4076", num, num2, 1, 1)));
			}
			attribute = envelopedSignatureReader.GetAttribute("AssertionID", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4075", "AssertionID")));
			}
			if (!XmlUtil.IsValidXmlIDValue(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4077", attribute)));
			}
			saml11Assertion.AssertionId = attribute;
			attribute = envelopedSignatureReader.GetAttribute("Issuer", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4075", "Issuer")));
			}
			saml11Assertion.Issuer = attribute;
			attribute = envelopedSignatureReader.GetAttribute("IssueInstant", null);
			if (!string.IsNullOrEmpty(attribute))
			{
				saml11Assertion.IssueInstant = DateTime.ParseExact(attribute, DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
			}
			envelopedSignatureReader.MoveToContent();
			envelopedSignatureReader.Read();
			if (envelopedSignatureReader.IsStartElement("Conditions", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				saml11Assertion.Conditions = ReadConditions(envelopedSignatureReader);
			}
			if (envelopedSignatureReader.IsStartElement("Advice", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				saml11Assertion.Advice = ReadAdvice(envelopedSignatureReader);
			}
			while (envelopedSignatureReader.IsStartElement())
			{
				saml11Assertion.Statements.Add(ReadStatement(envelopedSignatureReader));
			}
			if (saml11Assertion.Statements.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4078")));
			}
			envelopedSignatureReader.MoveToContent();
			envelopedSignatureReader.ReadEndElement();
			saml11Assertion.SigningCredentials = envelopedSignatureReader.SigningCredentials;
			saml11Assertion.CaptureSourceData(envelopedSignatureReader);
			return saml11Assertion;
		}

		protected virtual void WriteAssertion(XmlWriter writer, SamlAssertion assertion)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertion");
			}
			Saml11Assertion saml11Assertion = assertion as Saml11Assertion;
			if (saml11Assertion != null && saml11Assertion.CanWriteSourceData)
			{
				saml11Assertion.WriteSourceData(writer);
				return;
			}
			if (assertion.SigningCredentials != null)
			{
				writer = new EnvelopedSignatureWriter(writer, assertion.SigningCredentials, assertion.AssertionId, new WrappedSerializer(this, assertion));
			}
			writer.WriteStartElement("saml", "Assertion", "urn:oasis:names:tc:SAML:1.0:assertion");
			writer.WriteAttributeString("MajorVersion", null, Convert.ToString(1, CultureInfo.InvariantCulture));
			writer.WriteAttributeString("MinorVersion", null, Convert.ToString(1, CultureInfo.InvariantCulture));
			writer.WriteAttributeString("AssertionID", null, assertion.AssertionId);
			writer.WriteAttributeString("Issuer", null, assertion.Issuer);
			writer.WriteAttributeString("IssueInstant", null, assertion.IssueInstant.ToUniversalTime().ToString(DateTimeFormats.Generated, CultureInfo.InvariantCulture));
			if (assertion.Conditions != null)
			{
				WriteConditions(writer, assertion.Conditions);
			}
			if (assertion.Advice != null)
			{
				WriteAdvice(writer, assertion.Advice);
			}
			for (int i = 0; i < assertion.Statements.Count; i++)
			{
				WriteStatement(writer, assertion.Statements[i]);
			}
			writer.WriteEndElement();
		}

		protected virtual SamlConditions ReadConditions(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			SamlConditions samlConditions = new SamlConditions();
			string attribute = reader.GetAttribute("NotBefore", null);
			if (!string.IsNullOrEmpty(attribute))
			{
				samlConditions.NotBefore = DateTime.ParseExact(attribute, DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
			}
			attribute = reader.GetAttribute("NotOnOrAfter", null);
			if (!string.IsNullOrEmpty(attribute))
			{
				samlConditions.NotOnOrAfter = DateTime.ParseExact(attribute, DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
			}
			if (reader.IsEmptyElement)
			{
				reader.MoveToContent();
				reader.Read();
				return samlConditions;
			}
			reader.ReadStartElement();
			while (reader.IsStartElement())
			{
				samlConditions.Conditions.Add(ReadCondition(reader));
			}
			reader.ReadEndElement();
			return samlConditions;
		}

		protected virtual void WriteConditions(XmlWriter writer, SamlConditions conditions)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (conditions == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("conditions");
			}
			writer.WriteStartElement("saml", "Conditions", "urn:oasis:names:tc:SAML:1.0:assertion");
			if (conditions.NotBefore != DateTimeUtil.GetMinValue(DateTimeKind.Utc) && conditions.NotBefore != WCFMinValue)
			{
				writer.WriteAttributeString("NotBefore", null, conditions.NotBefore.ToUniversalTime().ToString(DateTimeFormats.Generated, DateTimeFormatInfo.InvariantInfo));
			}
			if (conditions.NotOnOrAfter != DateTimeUtil.GetMaxValue(DateTimeKind.Utc) && conditions.NotOnOrAfter != WCFMaxValue)
			{
				writer.WriteAttributeString("NotOnOrAfter", null, conditions.NotOnOrAfter.ToUniversalTime().ToString(DateTimeFormats.Generated, DateTimeFormatInfo.InvariantInfo));
			}
			for (int i = 0; i < conditions.Conditions.Count; i++)
			{
				WriteCondition(writer, conditions.Conditions[i]);
			}
			writer.WriteEndElement();
		}

		protected virtual SamlCondition ReadCondition(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (reader.IsStartElement("AudienceRestrictionCondition", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				return ReadAudienceRestrictionCondition(reader);
			}
			if (reader.IsStartElement("DoNotCacheCondition", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				return ReadDoNotCacheCondition(reader);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4080", reader.LocalName, reader.NamespaceURI)));
		}

		protected virtual void WriteCondition(XmlWriter writer, SamlCondition condition)
		{
			if (condition == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("condition");
			}
			SamlAudienceRestrictionCondition samlAudienceRestrictionCondition = condition as SamlAudienceRestrictionCondition;
			if (samlAudienceRestrictionCondition != null)
			{
				WriteAudienceRestrictionCondition(writer, samlAudienceRestrictionCondition);
				return;
			}
			SamlDoNotCacheCondition samlDoNotCacheCondition = condition as SamlDoNotCacheCondition;
			if (samlDoNotCacheCondition != null)
			{
				WriteDoNotCacheCondition(writer, samlDoNotCacheCondition);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4081", condition.GetType())));
		}

		protected virtual SamlAudienceRestrictionCondition ReadAudienceRestrictionCondition(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("AudienceRestrictionCondition", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "AudienceRestrictionCondition", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			reader.ReadStartElement();
			SamlAudienceRestrictionCondition samlAudienceRestrictionCondition = new SamlAudienceRestrictionCondition();
			while (reader.IsStartElement())
			{
				if (reader.IsStartElement("Audience", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					string text = reader.ReadString();
					if (string.IsNullOrEmpty(text))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4083")));
					}
					samlAudienceRestrictionCondition.Audiences.Add(new Uri(text, UriKind.RelativeOrAbsolute));
					reader.MoveToContent();
					reader.ReadEndElement();
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "Audience", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			if (samlAudienceRestrictionCondition.Audiences.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4084")));
			}
			reader.MoveToContent();
			reader.ReadEndElement();
			return samlAudienceRestrictionCondition;
		}

		protected virtual void WriteAudienceRestrictionCondition(XmlWriter writer, SamlAudienceRestrictionCondition condition)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (condition == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("condition");
			}
			if (condition.Audiences == null || condition.Audiences.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4269")));
			}
			writer.WriteStartElement("saml", "AudienceRestrictionCondition", "urn:oasis:names:tc:SAML:1.0:assertion");
			for (int i = 0; i < condition.Audiences.Count; i++)
			{
				writer.WriteElementString("Audience", "urn:oasis:names:tc:SAML:1.0:assertion", condition.Audiences[i].OriginalString);
			}
			writer.WriteEndElement();
		}

		protected virtual SamlDoNotCacheCondition ReadDoNotCacheCondition(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("DoNotCacheCondition", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "DoNotCacheCondition", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			SamlDoNotCacheCondition result = new SamlDoNotCacheCondition();
			if (reader.IsEmptyElement)
			{
				reader.MoveToContent();
				reader.Read();
				return result;
			}
			reader.MoveToContent();
			reader.ReadStartElement();
			reader.ReadEndElement();
			return result;
		}

		protected virtual void WriteDoNotCacheCondition(XmlWriter writer, SamlDoNotCacheCondition condition)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (condition == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("condition");
			}
			writer.WriteStartElement("saml", "DoNotCacheCondition", "urn:oasis:names:tc:SAML:1.0:assertion");
			writer.WriteEndElement();
		}

		protected virtual SamlStatement ReadStatement(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (reader.IsStartElement("AuthenticationStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				return ReadAuthenticationStatement(reader);
			}
			if (reader.IsStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				return ReadAttributeStatement(reader);
			}
			if (reader.IsStartElement("AuthorizationDecisionStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				return ReadAuthorizationDecisionStatement(reader);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4085", reader.LocalName, reader.NamespaceURI)));
		}

		protected virtual void WriteStatement(XmlWriter writer, SamlStatement statement)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (statement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statement");
			}
			SamlAuthenticationStatement samlAuthenticationStatement = statement as SamlAuthenticationStatement;
			if (samlAuthenticationStatement != null)
			{
				WriteAuthenticationStatement(writer, samlAuthenticationStatement);
				return;
			}
			SamlAuthorizationDecisionStatement samlAuthorizationDecisionStatement = statement as SamlAuthorizationDecisionStatement;
			if (samlAuthorizationDecisionStatement != null)
			{
				WriteAuthorizationDecisionStatement(writer, samlAuthorizationDecisionStatement);
				return;
			}
			SamlAttributeStatement samlAttributeStatement = statement as SamlAttributeStatement;
			if (samlAttributeStatement != null)
			{
				WriteAttributeStatement(writer, samlAttributeStatement);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4086", statement.GetType())));
		}

		protected virtual SamlSubject ReadSubject(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "Subject", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			SamlSubject samlSubject = new SamlSubject();
			reader.ReadStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion");
			if (reader.IsStartElement("NameIdentifier", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				samlSubject.NameFormat = reader.GetAttribute("Format", null);
				samlSubject.NameQualifier = reader.GetAttribute("NameQualifier", null);
				reader.MoveToContent();
				samlSubject.Name = reader.ReadElementString();
				if (string.IsNullOrEmpty(samlSubject.Name))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4087")));
				}
			}
			if (reader.IsStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				reader.ReadStartElement();
				while (reader.IsStartElement("ConfirmationMethod", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					string text = reader.ReadElementString();
					if (string.IsNullOrEmpty(text))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4088")));
					}
					samlSubject.ConfirmationMethods.Add(text);
				}
				if (samlSubject.ConfirmationMethods.Count == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4088")));
				}
				if (reader.IsStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					samlSubject.SubjectConfirmationData = reader.ReadElementString();
				}
				if (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
				{
					samlSubject.KeyIdentifier = ReadSubjectKeyInfo(reader);
					SecurityKey securityKey = ResolveSubjectKeyIdentifier(samlSubject.KeyIdentifier);
					if (securityKey != null)
					{
						samlSubject.Crypto = securityKey;
					}
					else
					{
						samlSubject.Crypto = new SecurityKeyElement(samlSubject.KeyIdentifier, base.Configuration.ServiceTokenResolver);
					}
				}
				if (samlSubject.ConfirmationMethods.Count == 0 && string.IsNullOrEmpty(samlSubject.Name))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4089")));
				}
				reader.MoveToContent();
				reader.ReadEndElement();
			}
			reader.MoveToContent();
			reader.ReadEndElement();
			return samlSubject;
		}

		protected virtual void WriteSubject(XmlWriter writer, SamlSubject subject)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subject");
			}
			writer.WriteStartElement("saml", "Subject", "urn:oasis:names:tc:SAML:1.0:assertion");
			if (!string.IsNullOrEmpty(subject.Name))
			{
				writer.WriteStartElement("saml", "NameIdentifier", "urn:oasis:names:tc:SAML:1.0:assertion");
				if (!string.IsNullOrEmpty(subject.NameFormat))
				{
					writer.WriteAttributeString("Format", null, subject.NameFormat);
				}
				if (subject.NameQualifier != null)
				{
					writer.WriteAttributeString("NameQualifier", null, subject.NameQualifier);
				}
				writer.WriteString(subject.Name);
				writer.WriteEndElement();
			}
			if (subject.ConfirmationMethods.Count > 0)
			{
				writer.WriteStartElement("saml", "SubjectConfirmation", "urn:oasis:names:tc:SAML:1.0:assertion");
				foreach (string confirmationMethod in subject.ConfirmationMethods)
				{
					writer.WriteElementString("ConfirmationMethod", "urn:oasis:names:tc:SAML:1.0:assertion", confirmationMethod);
				}
				if (!string.IsNullOrEmpty(subject.SubjectConfirmationData))
				{
					writer.WriteElementString("SubjectConfirmationData", "urn:oasis:names:tc:SAML:1.0:assertion", subject.SubjectConfirmationData);
				}
				if (subject.KeyIdentifier != null)
				{
					WriteSubjectKeyInfo(writer, subject.KeyIdentifier);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		protected virtual SecurityKeyIdentifier ReadSubjectKeyInfo(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (KeyInfoSerializer.CanReadKeyIdentifier(reader))
			{
				return KeyInfoSerializer.ReadKeyIdentifier(reader);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4090")));
		}

		protected virtual void WriteSubjectKeyInfo(XmlWriter writer, SecurityKeyIdentifier subjectSki)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (subjectSki == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subjectSki");
			}
			if (KeyInfoSerializer.CanWriteKeyIdentifier(subjectSki))
			{
				KeyInfoSerializer.WriteKeyIdentifier(writer, subjectSki);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("subjectSki", SR.GetString("ID4091", subjectSki.GetType()));
		}

		protected virtual SamlAttributeStatement ReadAttributeStatement(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "AttributeStatement", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			reader.ReadStartElement();
			SamlAttributeStatement samlAttributeStatement = new SamlAttributeStatement();
			if (reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				samlAttributeStatement.SamlSubject = ReadSubject(reader);
				while (reader.IsStartElement() && reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					samlAttributeStatement.Attributes.Add(ReadAttribute(reader));
				}
				if (samlAttributeStatement.Attributes.Count == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4093")));
				}
				reader.MoveToContent();
				reader.ReadEndElement();
				return samlAttributeStatement;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4092")));
		}

		protected virtual void WriteAttributeStatement(XmlWriter writer, SamlAttributeStatement statement)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (statement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statement");
			}
			writer.WriteStartElement("saml", "AttributeStatement", "urn:oasis:names:tc:SAML:1.0:assertion");
			WriteSubject(writer, statement.SamlSubject);
			for (int i = 0; i < statement.Attributes.Count; i++)
			{
				WriteAttribute(writer, statement.Attributes[i]);
			}
			writer.WriteEndElement();
		}

		protected virtual SamlAttribute ReadAttribute(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			Saml11Attribute saml11Attribute = new Saml11Attribute();
			saml11Attribute.Name = reader.GetAttribute("AttributeName", null);
			if (string.IsNullOrEmpty(saml11Attribute.Name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4094")));
			}
			saml11Attribute.Namespace = reader.GetAttribute("AttributeNamespace", null);
			if (string.IsNullOrEmpty(saml11Attribute.Namespace))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4095")));
			}
			string attribute = reader.GetAttribute("OriginalIssuer", "http://schemas.xmlsoap.org/ws/2009/09/identity/claims");
			if (attribute == null)
			{
				attribute = reader.GetAttribute("OriginalIssuer", "http://schemas.microsoft.com/ws/2008/06/identity");
			}
			if (attribute == string.Empty)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4252")));
			}
			saml11Attribute.OriginalIssuer = attribute;
			reader.MoveToContent();
			reader.Read();
			while (reader.IsStartElement("AttributeValue", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				string text = null;
				string text2 = null;
				string attribute2 = reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
				if (!string.IsNullOrEmpty(attribute2))
				{
					if (attribute2.IndexOf(":", StringComparison.Ordinal) == -1)
					{
						text = reader.LookupNamespace(string.Empty);
						text2 = attribute2;
					}
					else if (attribute2.IndexOf(":", StringComparison.Ordinal) > 0 && attribute2.IndexOf(":", StringComparison.Ordinal) < attribute2.Length - 1)
					{
						string prefix = attribute2.Substring(0, attribute2.IndexOf(":", StringComparison.Ordinal));
						text = reader.LookupNamespace(prefix);
						text2 = attribute2.Substring(attribute2.IndexOf(":", StringComparison.Ordinal) + 1);
					}
				}
				if (text != null && text2 != null)
				{
					saml11Attribute.AttributeValueXsiType = text + "#" + text2;
				}
				string item = ReadAttributeValue(reader, saml11Attribute);
				saml11Attribute.AttributeValues.Add(item);
			}
			if (saml11Attribute.AttributeValues.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4212")));
			}
			reader.MoveToContent();
			reader.ReadEndElement();
			return saml11Attribute;
		}

		protected virtual string ReadAttributeValue(XmlReader reader, SamlAttribute attribute)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.ReadElementString();
		}

		protected virtual void WriteAttribute(XmlWriter writer, SamlAttribute attribute)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (attribute == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("attribute");
			}
			writer.WriteStartElement("saml", "Attribute", "urn:oasis:names:tc:SAML:1.0:assertion");
			writer.WriteAttributeString("AttributeName", null, attribute.Name);
			writer.WriteAttributeString("AttributeNamespace", null, attribute.Namespace);
			Saml11Attribute saml11Attribute = attribute as Saml11Attribute;
			if (saml11Attribute != null && saml11Attribute.OriginalIssuer != null)
			{
				writer.WriteAttributeString("OriginalIssuer", "http://schemas.xmlsoap.org/ws/2009/09/identity/claims", saml11Attribute.OriginalIssuer);
			}
			string text = null;
			string text2 = null;
			if (saml11Attribute != null && !StringComparer.Ordinal.Equals(saml11Attribute.AttributeValueXsiType, "http://www.w3.org/2001/XMLSchema#string"))
			{
				int num = saml11Attribute.AttributeValueXsiType.IndexOf('#');
				text = saml11Attribute.AttributeValueXsiType.Substring(0, num);
				text2 = saml11Attribute.AttributeValueXsiType.Substring(num + 1);
			}
			for (int i = 0; i < attribute.AttributeValues.Count; i++)
			{
				if (attribute.AttributeValues[i] == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4096")));
				}
				writer.WriteStartElement("saml", "AttributeValue", "urn:oasis:names:tc:SAML:1.0:assertion");
				if (text != null && text2 != null)
				{
					writer.WriteAttributeString("xmlns", "tn", null, text);
					writer.WriteAttributeString("type", "http://www.w3.org/2001/XMLSchema-instance", "tn:" + text2);
				}
				WriteAttributeValue(writer, attribute.AttributeValues[i], attribute);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		protected virtual void WriteAttributeValue(XmlWriter writer, string value, SamlAttribute attribute)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			writer.WriteString(value);
		}

		protected virtual SamlAuthenticationStatement ReadAuthenticationStatement(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("AuthenticationStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "AuthenticationStatement", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			SamlAuthenticationStatement samlAuthenticationStatement = new SamlAuthenticationStatement();
			string attribute = reader.GetAttribute("AuthenticationInstant", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4097")));
			}
			samlAuthenticationStatement.AuthenticationInstant = DateTime.ParseExact(attribute, DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
			samlAuthenticationStatement.AuthenticationMethod = reader.GetAttribute("AuthenticationMethod", null);
			if (string.IsNullOrEmpty(samlAuthenticationStatement.AuthenticationMethod))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4098")));
			}
			reader.MoveToContent();
			reader.Read();
			if (reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				samlAuthenticationStatement.SamlSubject = ReadSubject(reader);
				if (reader.IsStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					samlAuthenticationStatement.DnsAddress = reader.GetAttribute("DNSAddress", null);
					samlAuthenticationStatement.IPAddress = reader.GetAttribute("IPAddress", null);
					if (reader.IsEmptyElement)
					{
						reader.MoveToContent();
						reader.Read();
					}
					else
					{
						reader.MoveToContent();
						reader.Read();
						reader.ReadEndElement();
					}
				}
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("AuthorityBinding", "urn:oasis:names:tc:SAML:1.0:assertion"))
					{
						samlAuthenticationStatement.AuthorityBindings.Add(ReadAuthorityBinding(reader));
						continue;
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "AuthorityBinding", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
				}
				reader.MoveToContent();
				reader.ReadEndElement();
				return samlAuthenticationStatement;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4099")));
		}

		protected virtual void WriteAuthenticationStatement(XmlWriter writer, SamlAuthenticationStatement statement)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (statement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statement");
			}
			writer.WriteStartElement("saml", "AuthenticationStatement", "urn:oasis:names:tc:SAML:1.0:assertion");
			writer.WriteAttributeString("AuthenticationMethod", null, statement.AuthenticationMethod);
			writer.WriteAttributeString("AuthenticationInstant", null, XmlConvert.ToString(statement.AuthenticationInstant.ToUniversalTime(), DateTimeFormats.Generated));
			WriteSubject(writer, statement.SamlSubject);
			if (statement.IPAddress != null || statement.DnsAddress != null)
			{
				writer.WriteStartElement("saml", "SubjectLocality", "urn:oasis:names:tc:SAML:1.0:assertion");
				if (statement.IPAddress != null)
				{
					writer.WriteAttributeString("IPAddress", null, statement.IPAddress);
				}
				if (statement.DnsAddress != null)
				{
					writer.WriteAttributeString("DNSAddress", null, statement.DnsAddress);
				}
				writer.WriteEndElement();
			}
			for (int i = 0; i < statement.AuthorityBindings.Count; i++)
			{
				WriteAuthorityBinding(writer, statement.AuthorityBindings[i]);
			}
			writer.WriteEndElement();
		}

		protected virtual SamlAuthorityBinding ReadAuthorityBinding(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			SamlAuthorityBinding samlAuthorityBinding = new SamlAuthorityBinding();
			string attribute = reader.GetAttribute("AuthorityKind", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4200")));
			}
			string[] array = attribute.Split(':');
			if (array.Length > 2)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4201", attribute)));
			}
			string prefix;
			string name;
			if (array.Length == 2)
			{
				prefix = array[0];
				name = array[1];
			}
			else
			{
				prefix = string.Empty;
				name = array[0];
			}
			string ns = reader.LookupNamespace(prefix);
			samlAuthorityBinding.AuthorityKind = new XmlQualifiedName(name, ns);
			samlAuthorityBinding.Binding = reader.GetAttribute("Binding", null);
			if (string.IsNullOrEmpty(samlAuthorityBinding.Binding))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4202")));
			}
			samlAuthorityBinding.Location = reader.GetAttribute("Location", null);
			if (string.IsNullOrEmpty(samlAuthorityBinding.Location))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4203")));
			}
			if (reader.IsEmptyElement)
			{
				reader.MoveToContent();
				reader.Read();
			}
			else
			{
				reader.MoveToContent();
				reader.Read();
				reader.ReadEndElement();
			}
			return samlAuthorityBinding;
		}

		protected virtual void WriteAuthorityBinding(XmlWriter writer, SamlAuthorityBinding authorityBinding)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (authorityBinding == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statement");
			}
			writer.WriteStartElement("saml", "AuthorityBinding", "urn:oasis:names:tc:SAML:1.0:assertion");
			string text = null;
			if (!string.IsNullOrEmpty(authorityBinding.AuthorityKind.Namespace))
			{
				writer.WriteAttributeString(string.Empty, "xmlns", null, authorityBinding.AuthorityKind.Namespace);
				text = writer.LookupPrefix(authorityBinding.AuthorityKind.Namespace);
			}
			writer.WriteStartAttribute("AuthorityKind", null);
			if (string.IsNullOrEmpty(text))
			{
				writer.WriteString(authorityBinding.AuthorityKind.Name);
			}
			else
			{
				writer.WriteString(text + ":" + authorityBinding.AuthorityKind.Name);
			}
			writer.WriteEndAttribute();
			writer.WriteAttributeString("Location", null, authorityBinding.Location);
			writer.WriteAttributeString("Binding", null, authorityBinding.Binding);
			writer.WriteEndElement();
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
			SamlSecurityToken samlSecurityToken = token as SamlSecurityToken;
			if (samlSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4217", token.GetType(), typeof(SamlSecurityToken))));
			}
			WriteAssertion(writer, samlSecurityToken.Assertion);
		}

		protected virtual SamlAuthorizationDecisionStatement ReadAuthorizationDecisionStatement(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("AuthorizationDecisionStatement", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "AuthorizationDecisionStatement", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			SamlAuthorizationDecisionStatement samlAuthorizationDecisionStatement = new SamlAuthorizationDecisionStatement();
			samlAuthorizationDecisionStatement.Resource = reader.GetAttribute("Resource", null);
			if (string.IsNullOrEmpty(samlAuthorizationDecisionStatement.Resource))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4205")));
			}
			string attribute = reader.GetAttribute("Decision", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4204")));
			}
			if (attribute.Equals(SamlAccessDecision.Deny.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				samlAuthorizationDecisionStatement.AccessDecision = SamlAccessDecision.Deny;
			}
			else if (attribute.Equals(SamlAccessDecision.Permit.ToString(), StringComparison.OrdinalIgnoreCase))
			{
				samlAuthorizationDecisionStatement.AccessDecision = SamlAccessDecision.Permit;
			}
			else
			{
				samlAuthorizationDecisionStatement.AccessDecision = SamlAccessDecision.Indeterminate;
			}
			reader.MoveToContent();
			reader.Read();
			if (reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				samlAuthorizationDecisionStatement.SamlSubject = ReadSubject(reader);
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("Action", "urn:oasis:names:tc:SAML:1.0:assertion"))
					{
						samlAuthorizationDecisionStatement.SamlActions.Add(ReadAction(reader));
						continue;
					}
					if (reader.IsStartElement("Evidence", "urn:oasis:names:tc:SAML:1.0:assertion"))
					{
						if (samlAuthorizationDecisionStatement.Evidence != null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4207")));
						}
						samlAuthorizationDecisionStatement.Evidence = ReadEvidence(reader);
						continue;
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4208", reader.LocalName, reader.NamespaceURI)));
				}
				if (samlAuthorizationDecisionStatement.SamlActions.Count == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4209")));
				}
				reader.MoveToContent();
				reader.ReadEndElement();
				return samlAuthorizationDecisionStatement;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4206")));
		}

		protected virtual void WriteAuthorizationDecisionStatement(XmlWriter writer, SamlAuthorizationDecisionStatement statement)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (statement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statement");
			}
			writer.WriteStartElement("saml", "AuthorizationDecisionStatement", "urn:oasis:names:tc:SAML:1.0:assertion");
			writer.WriteAttributeString("Decision", null, statement.AccessDecision.ToString());
			writer.WriteAttributeString("Resource", null, statement.Resource);
			WriteSubject(writer, statement.SamlSubject);
			foreach (SamlAction samlAction in statement.SamlActions)
			{
				WriteAction(writer, samlAction);
			}
			if (statement.Evidence != null)
			{
				WriteEvidence(writer, statement.Evidence);
			}
			writer.WriteEndElement();
		}

		protected virtual SamlEvidence ReadEvidence(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Evidence", "urn:oasis:names:tc:SAML:1.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4082", "Evidence", "urn:oasis:names:tc:SAML:1.0:assertion", reader.LocalName, reader.NamespaceURI)));
			}
			SamlEvidence samlEvidence = new SamlEvidence();
			reader.ReadStartElement();
			while (reader.IsStartElement())
			{
				if (reader.IsStartElement("AssertionIDReference", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					samlEvidence.AssertionIdReferences.Add(reader.ReadElementString());
					continue;
				}
				if (reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:1.0:assertion"))
				{
					samlEvidence.Assertions.Add(ReadAssertion(reader));
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4210", reader.LocalName, reader.NamespaceURI)));
			}
			if (samlEvidence.AssertionIdReferences.Count == 0 && samlEvidence.Assertions.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4211")));
			}
			reader.MoveToContent();
			reader.ReadEndElement();
			return samlEvidence;
		}

		protected virtual void WriteEvidence(XmlWriter writer, SamlEvidence evidence)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (evidence == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("evidence");
			}
			writer.WriteStartElement("saml", "Evidence", "urn:oasis:names:tc:SAML:1.0:assertion");
			for (int i = 0; i < evidence.AssertionIdReferences.Count; i++)
			{
				writer.WriteElementString("saml", "AssertionIDReference", "urn:oasis:names:tc:SAML:1.0:assertion", evidence.AssertionIdReferences[i]);
			}
			for (int j = 0; j < evidence.Assertions.Count; j++)
			{
				WriteAssertion(writer, evidence.Assertions[j]);
			}
			writer.WriteEndElement();
		}

		protected virtual SecurityKey ResolveSubjectKeyIdentifier(SecurityKeyIdentifier subjectKeyIdentifier)
		{
			if (subjectKeyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subjectKeyIdentifier");
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			if (base.Configuration.ServiceTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4276"));
			}
			SecurityKey key = null;
			foreach (SecurityKeyIdentifierClause item in subjectKeyIdentifier)
			{
				if (base.Configuration.ServiceTokenResolver.TryResolveSecurityKey(item, out key))
				{
					return key;
				}
			}
			if (subjectKeyIdentifier.CanCreateKey)
			{
				return subjectKeyIdentifier.CreateKey();
			}
			return null;
		}

		protected virtual SecurityToken ResolveIssuerToken(Saml11Assertion assertion, SecurityTokenResolver issuerResolver)
		{
			if (assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertion");
			}
			if (issuerResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("issuerResolver");
			}
			if (TryResolveIssuerToken(assertion, issuerResolver, out var token))
			{
				return token;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4220")));
		}

		protected virtual bool TryResolveIssuerToken(Saml11Assertion assertion, SecurityTokenResolver issuerResolver, out SecurityToken token)
		{
			if (assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertion");
			}
			if (assertion.SigningCredentials != null && assertion.SigningCredentials.SigningKeyIdentifier != null && issuerResolver != null)
			{
				SecurityKeyIdentifier signingKeyIdentifier = assertion.SigningCredentials.SigningKeyIdentifier;
				return issuerResolver.TryResolveToken(signingKeyIdentifier, out token);
			}
			token = null;
			return false;
		}

		protected virtual SecurityKeyIdentifier ReadSigningKeyInfo(XmlReader reader, SamlAssertion assertion)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			SecurityKeyIdentifier securityKeyIdentifier;
			if (KeyInfoSerializer.CanReadKeyIdentifier(reader))
			{
				securityKeyIdentifier = KeyInfoSerializer.ReadKeyIdentifier(reader);
			}
			else
			{
				KeyInfo keyInfo = new KeyInfo(KeyInfoSerializer);
				keyInfo.ReadXml(XmlDictionaryReader.CreateDictionaryReader(reader));
				securityKeyIdentifier = keyInfo.KeyIdentifier;
			}
			if (securityKeyIdentifier.Count == 0)
			{
				return new SecurityKeyIdentifier(new Saml11SecurityKeyIdentifierClause(assertion));
			}
			return securityKeyIdentifier;
		}

		protected virtual void WriteSigningKeyInfo(XmlWriter writer, SecurityKeyIdentifier signingKeyIdentifier)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (signingKeyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signingKeyIdentifier");
			}
			if (KeyInfoSerializer.CanWriteKeyIdentifier(signingKeyIdentifier))
			{
				KeyInfoSerializer.WriteKeyIdentifier(writer, signingKeyIdentifier);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4221", signingKeyIdentifier));
		}

		private void ValidateStatements(IList<SamlStatement> statements)
		{
			if (statements == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statements");
			}
			List<SamlSubject> list = new List<SamlSubject>();
			foreach (SamlStatement statement in statements)
			{
				if (statement is SamlAttributeStatement)
				{
					list.Add((statement as SamlAttributeStatement).SamlSubject);
				}
				if (statement is SamlAuthenticationStatement)
				{
					list.Add((statement as SamlAuthenticationStatement).SamlSubject);
				}
				if (statement is SamlAuthorizationDecisionStatement)
				{
					list.Add((statement as SamlAuthorizationDecisionStatement).SamlSubject);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			string name = list[0].Name;
			string nameFormat = list[0].NameFormat;
			string nameQualifier = list[0].NameQualifier;
			foreach (SamlSubject item in list)
			{
				if (!StringComparer.Ordinal.Equals(item.Name, name) || !StringComparer.Ordinal.Equals(item.NameFormat, nameFormat) || !StringComparer.Ordinal.Equals(item.NameQualifier, nameQualifier))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4225", item));
				}
			}
		}

		public override string[] GetTokenTypeIdentifiers()
		{
			return _tokenTypeIdentifiers;
		}
	}
}
