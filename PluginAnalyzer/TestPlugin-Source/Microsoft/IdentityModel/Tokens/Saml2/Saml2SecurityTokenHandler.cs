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
using Microsoft.IdentityModel.Protocols.XmlEncryption;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Web;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2SecurityTokenHandler : SecurityTokenHandler
	{
		private class WrappedSerializer : SecurityTokenSerializer
		{
			private Saml2SecurityTokenHandler _parent;

			private Saml2Assertion _assertion;

			public WrappedSerializer(Saml2SecurityTokenHandler parent, Saml2Assertion assertion)
			{
				_assertion = assertion;
				_parent = parent;
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

		private class ReceivedEncryptingCredentials : Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials
		{
			public ReceivedEncryptingCredentials(SecurityKey key, SecurityKeyIdentifier keyIdentifier, string algorithm)
				: base(key, keyIdentifier, algorithm)
			{
			}
		}

		public const string TokenProfile11ValueType = "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID";

		private const string Actor = "Actor";

		private const string Attribute = "Attribute";

		private static string[] _tokenTypeIdentifiers = new string[2] { "urn:oasis:names:tc:SAML:2.0:assertion", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0" };

		private SamlSecurityTokenRequirement _samlSecurityTokenRequirement;

		private SecurityTokenSerializer _keyInfoSerializer;

		private object _syncObject = new object();

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

		public override bool CanValidateToken => true;

		public override Type TokenType => typeof(Saml2SecurityToken);

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

		public override bool CanWriteToken => true;

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

		public Saml2SecurityTokenHandler()
			: this(new SamlSecurityTokenRequirement())
		{
		}

		public Saml2SecurityTokenHandler(SamlSecurityTokenRequirement samlSecurityTokenRequirement)
		{
			if (samlSecurityTokenRequirement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlSecurityTokenRequirement");
			}
			_samlSecurityTokenRequirement = samlSecurityTokenRequirement;
		}

		public Saml2SecurityTokenHandler(XmlNodeList customConfigElements)
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

		public override SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			return token.CreateKeyIdentifierClause<Saml2AssertionKeyIdentifierClause>();
		}

		protected virtual Saml2Conditions CreateConditions(Lifetime tokenLifetime, string relyingPartyAddress, SecurityTokenDescriptor tokenDescriptor)
		{
			bool flag = null != tokenLifetime;
			bool flag2 = !string.IsNullOrEmpty(relyingPartyAddress);
			if (!flag && !flag2)
			{
				return null;
			}
			Saml2Conditions saml2Conditions = new Saml2Conditions();
			if (flag)
			{
				saml2Conditions.NotBefore = tokenLifetime.Created;
				saml2Conditions.NotOnOrAfter = tokenLifetime.Expires;
			}
			if (flag2)
			{
				saml2Conditions.AudienceRestrictions.Add(new Saml2AudienceRestriction(new Uri(relyingPartyAddress)));
			}
			return saml2Conditions;
		}

		protected virtual Saml2Advice CreateAdvice(SecurityTokenDescriptor tokenDescriptor)
		{
			return null;
		}

		protected virtual Saml2NameIdentifier CreateIssuerNameIdentifier(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			string tokenIssuerName = tokenDescriptor.TokenIssuerName;
			if (string.IsNullOrEmpty(tokenIssuerName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4138")));
			}
			return new Saml2NameIdentifier(tokenIssuerName);
		}

		protected virtual Saml2Attribute CreateAttribute(Claim claim, SecurityTokenDescriptor tokenDescriptor)
		{
			if (claim == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("claim");
			}
			Saml2Attribute saml2Attribute = new Saml2Attribute(claim.ClaimType, claim.Value);
			if (!StringComparer.Ordinal.Equals("LOCAL AUTHORITY", claim.OriginalIssuer))
			{
				saml2Attribute.OriginalIssuer = claim.OriginalIssuer;
			}
			saml2Attribute.AttributeValueXsiType = claim.ValueType;
			if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename"))
			{
				string uriString = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename"];
				if (!UriUtil.CanCreateValidUri(uriString, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("nameFormat", SR.GetString("ID0013"));
				}
				saml2Attribute.NameFormat = new Uri(uriString);
			}
			if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname"))
			{
				saml2Attribute.FriendlyName = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname"];
			}
			return saml2Attribute;
		}

		protected virtual Saml2AttributeStatement CreateAttributeStatement(IClaimsIdentity subject, SecurityTokenDescriptor tokenDescriptor)
		{
			if (subject == null)
			{
				return null;
			}
			if (subject.Claims != null)
			{
				List<Saml2Attribute> list = new List<Saml2Attribute>(subject.Claims.Count);
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
				ICollection<Saml2Attribute> collection = CollectAttributeValues(list);
				if (collection.Count > 0)
				{
					return new Saml2AttributeStatement(collection);
				}
			}
			return null;
		}

		protected virtual ICollection<Saml2Attribute> CollectAttributeValues(ICollection<Saml2Attribute> attributes)
		{
			Dictionary<SamlAttributeKeyComparer.AttributeKey, Saml2Attribute> dictionary = new Dictionary<SamlAttributeKeyComparer.AttributeKey, Saml2Attribute>(attributes.Count, new SamlAttributeKeyComparer());
			foreach (Saml2Attribute attribute in attributes)
			{
				if (attribute == null)
				{
					continue;
				}
				SamlAttributeKeyComparer.AttributeKey key = new SamlAttributeKeyComparer.AttributeKey(attribute);
				if (dictionary.ContainsKey(key))
				{
					foreach (string value in attribute.Values)
					{
						dictionary[key].Values.Add(value);
					}
				}
				else
				{
					dictionary.Add(key, attribute);
				}
			}
			return dictionary.Values;
		}

		protected virtual void AddDelegateToAttributes(IClaimsIdentity subject, ICollection<Saml2Attribute> attributes, SecurityTokenDescriptor tokenDescriptor)
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
			List<Saml2Attribute> list = new List<Saml2Attribute>(subject.Actor.Claims.Count);
			foreach (Claim claim in subject.Actor.Claims)
			{
				if (claim != null)
				{
					list.Add(CreateAttribute(claim, tokenDescriptor));
				}
			}
			AddDelegateToAttributes(subject.Actor, list, tokenDescriptor);
			ICollection<Saml2Attribute> attributes2 = CollectAttributeValues(list);
			attributes.Add(CreateAttribute(new Claim("http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor", CreateXmlStringFromAttributes(attributes2), "http://www.w3.org/2001/XMLSchema#string"), tokenDescriptor));
		}

		protected virtual string CreateXmlStringFromAttributes(IEnumerable<Saml2Attribute> attributes)
		{
			bool flag = false;
			using MemoryStream memoryStream = new MemoryStream();
			using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false))
			{
				foreach (Saml2Attribute attribute in attributes)
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

		protected virtual IEnumerable<Saml2Statement> CreateStatements(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			Collection<Saml2Statement> collection = new Collection<Saml2Statement>();
			Saml2AttributeStatement saml2AttributeStatement = CreateAttributeStatement(tokenDescriptor.Subject, tokenDescriptor);
			if (saml2AttributeStatement != null)
			{
				collection.Add(saml2AttributeStatement);
			}
			Saml2AuthenticationStatement saml2AuthenticationStatement = CreateAuthenticationStatement(tokenDescriptor.AuthenticationInfo, tokenDescriptor);
			if (saml2AuthenticationStatement != null)
			{
				collection.Add(saml2AuthenticationStatement);
			}
			return collection;
		}

		protected virtual Saml2AuthenticationStatement CreateAuthenticationStatement(AuthenticationInformation authInfo, SecurityTokenDescriptor tokenDescriptor)
		{
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
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4270", "AuthenticationMethod", "SAML2"));
			}
			if (text2 == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4270", "AuthenticationInstant", "SAML2"));
			}
			if (!UriUtil.TryCreateValidUri(DenormalizeAuthenticationType(text), UriKind.Absolute, out var result))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4185", text));
			}
			Saml2AuthenticationContext authenticationContext = new Saml2AuthenticationContext(result);
			DateTime authenticationInstant = DateTime.ParseExact(text2, DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
			Saml2AuthenticationStatement saml2AuthenticationStatement = new Saml2AuthenticationStatement(authenticationContext, authenticationInstant);
			if (authInfo != null)
			{
				if (!string.IsNullOrEmpty(authInfo.DnsName) || !string.IsNullOrEmpty(authInfo.Address))
				{
					saml2AuthenticationStatement.SubjectLocality = new Saml2SubjectLocality(authInfo.Address, authInfo.DnsName);
				}
				if (!string.IsNullOrEmpty(authInfo.Session))
				{
					saml2AuthenticationStatement.SessionIndex = authInfo.Session;
				}
				saml2AuthenticationStatement.SessionNotOnOrAfter = authInfo.NotOnOrAfter;
			}
			return saml2AuthenticationStatement;
		}

		protected virtual Saml2Subject CreateSamlSubject(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			Saml2Subject saml2Subject = new Saml2Subject();
			string text = null;
			string text2 = null;
			string nameQualifier = null;
			string sPProvidedId = null;
			string sPNameQualifier = null;
			if (tokenDescriptor.Subject != null && tokenDescriptor.Subject.Claims != null)
			{
				foreach (Claim claim in tokenDescriptor.Subject.Claims)
				{
					if (claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
					{
						if (text != null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4139")));
						}
						text = claim.Value;
						if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"))
						{
							text2 = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"];
						}
						if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"))
						{
							nameQualifier = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"];
						}
						if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spnamequalifier"))
						{
							sPNameQualifier = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spnamequalifier"];
						}
						if (claim.Properties.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spprovidedid"))
						{
							sPProvidedId = claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spprovidedid"];
						}
					}
				}
			}
			if (text != null)
			{
				Saml2NameIdentifier saml2NameIdentifier = new Saml2NameIdentifier(text);
				if (text2 != null && UriUtil.CanCreateValidUri(text2, UriKind.Absolute))
				{
					saml2NameIdentifier.Format = new Uri(text2);
				}
				saml2NameIdentifier.NameQualifier = nameQualifier;
				saml2NameIdentifier.SPNameQualifier = sPNameQualifier;
				saml2NameIdentifier.SPProvidedId = sPProvidedId;
				saml2Subject.NameId = saml2NameIdentifier;
			}
			Saml2SubjectConfirmation saml2SubjectConfirmation;
			if (tokenDescriptor.Proof == null)
			{
				saml2SubjectConfirmation = new Saml2SubjectConfirmation(Saml2Constants.ConfirmationMethods.Bearer);
			}
			else
			{
				saml2SubjectConfirmation = new Saml2SubjectConfirmation(Saml2Constants.ConfirmationMethods.HolderOfKey, new Saml2SubjectConfirmationData());
				saml2SubjectConfirmation.SubjectConfirmationData.KeyIdentifiers.Add(tokenDescriptor.Proof.KeyIdentifier);
			}
			saml2Subject.SubjectConfirmations.Add(saml2SubjectConfirmation);
			return saml2Subject;
		}

		public override SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			Saml2Assertion saml2Assertion = new Saml2Assertion(CreateIssuerNameIdentifier(tokenDescriptor));
			saml2Assertion.Subject = CreateSamlSubject(tokenDescriptor);
			saml2Assertion.SigningCredentials = GetSigningCredentials(tokenDescriptor);
			saml2Assertion.Conditions = CreateConditions(tokenDescriptor.Lifetime, tokenDescriptor.AppliesToAddress, tokenDescriptor);
			saml2Assertion.Advice = CreateAdvice(tokenDescriptor);
			IEnumerable<Saml2Statement> enumerable = CreateStatements(tokenDescriptor);
			if (enumerable != null)
			{
				foreach (Saml2Statement item in enumerable)
				{
					saml2Assertion.Statements.Add(item);
				}
			}
			saml2Assertion.EncryptingCredentials = GetEncryptingCredentials(tokenDescriptor);
			return new Saml2SecurityToken(saml2Assertion);
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

		public override string[] GetTokenTypeIdentifiers()
		{
			return _tokenTypeIdentifiers;
		}

		protected virtual void ValidateConditions(Saml2Conditions conditions, bool enforceAudienceRestriction)
		{
			if (conditions != null)
			{
				DateTime utcNow = DateTime.UtcNow;
				if (conditions.NotBefore.HasValue && DateTimeUtil.Add(utcNow, base.Configuration.MaxClockSkew) < conditions.NotBefore.Value)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenNotYetValidException(SR.GetString("ID4147", conditions.NotBefore.Value, utcNow)));
				}
				if (conditions.NotOnOrAfter.HasValue && DateTimeUtil.Add(utcNow, base.Configuration.MaxClockSkew.Negate()) >= conditions.NotOnOrAfter.Value)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenExpiredException(SR.GetString("ID4148", conditions.NotOnOrAfter.Value, utcNow)));
				}
				if (conditions.OneTimeUse)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4149")));
				}
				if (conditions.ProxyRestriction != null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4150")));
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
			if (conditions == null || conditions.AudienceRestrictions.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new AudienceUriValidationFailedException(SR.GetString("ID1035")));
			}
			foreach (Saml2AudienceRestriction audienceRestriction in conditions.AudienceRestrictions)
			{
				SamlSecurityTokenRequirement.ValidateAudienceRestriction(base.Configuration.AudienceRestriction.AllowedAudienceUris, audienceRestriction.Audiences);
			}
		}

		public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			Saml2SecurityToken saml2SecurityToken = token as Saml2SecurityToken;
			if (saml2SecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID4151"));
			}
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, SR.GetString("TraceValidateToken"), new TokenTraceRecord(token), null);
			}
			if (saml2SecurityToken.IssuerToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4152")));
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			Saml2Assertion assertion = saml2SecurityToken.Assertion;
			ValidateConditions(assertion.Conditions, SamlSecurityTokenRequirement.ShouldEnforceAudienceRestriction(base.Configuration.AudienceRestriction.AudienceMode, saml2SecurityToken));
			Saml2SubjectConfirmation saml2SubjectConfirmation = assertion.Subject.SubjectConfirmations[0];
			if (saml2SubjectConfirmation.SubjectConfirmationData != null)
			{
				ValidateConfirmationData(saml2SubjectConfirmation.SubjectConfirmationData);
			}
			X509SecurityToken x509SecurityToken = saml2SecurityToken.IssuerToken as X509SecurityToken;
			if (x509SecurityToken != null)
			{
				CertificateValidator.Validate(x509SecurityToken.Certificate);
			}
			IClaimsIdentity claimsIdentity = CreateClaims(saml2SecurityToken);
			if (_samlSecurityTokenRequirement.MapToWindows)
			{
				WindowsClaimsIdentity windowsClaimsIdentity = WindowsClaimsIdentity.CreateFromUpn(FindUpn(claimsIdentity), "Federation", _samlSecurityTokenRequirement.UseWindowsTokenService, base.Configuration.IssuerNameRegistry.GetWindowsIssuerName());
				windowsClaimsIdentity.Claims.CopyRange(claimsIdentity.Claims);
				claimsIdentity = windowsClaimsIdentity;
			}
			if (base.Configuration.DetectReplayedTokens)
			{
				DetectReplayedTokens(saml2SecurityToken);
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

		protected virtual string DenormalizeAuthenticationType(string normalizedAuthenticationType)
		{
			return AuthenticationTypeMaps.Denormalize(normalizedAuthenticationType, AuthenticationTypeMaps.Saml2);
		}

		protected override void DetectReplayedTokens(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			Saml2SecurityToken saml2SecurityToken = token as Saml2SecurityToken;
			if (saml2SecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID1064", token.GetType().ToString()));
			}
			if (saml2SecurityToken.SecurityKeys.Count != 0)
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
			if (string.IsNullOrEmpty(saml2SecurityToken.Assertion.Id.Value))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID1065")));
			}
			StringBuilder stringBuilder = new StringBuilder();
			string key;
			using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.NewSha256())
			{
				if (string.IsNullOrEmpty(saml2SecurityToken.Assertion.Issuer.Value))
				{
					stringBuilder.AppendFormat("{0}{1}", saml2SecurityToken.Assertion.Id.Value, _tokenTypeIdentifiers[0]);
				}
				else
				{
					stringBuilder.AppendFormat("{0}{1}{2}", saml2SecurityToken.Assertion.Id.Value, saml2SecurityToken.Assertion.Issuer.Value, _tokenTypeIdentifiers[0]);
				}
				key = Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
			}
			if (base.Configuration.TokenReplayCache.TryFind(key))
			{
				string text = ((saml2SecurityToken.Assertion.Issuer.Value != null) ? saml2SecurityToken.Assertion.Issuer.Value : string.Empty);
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenReplayDetectedException(SR.GetString("ID1066", typeof(Saml2SecurityToken).ToString(), saml2SecurityToken.Assertion.Id.Value, text)));
			}
			base.Configuration.TokenReplayCache.TryAdd(key, null, DateTimeUtil.Add(GetCacheExpirationTime(saml2SecurityToken), base.Configuration.MaxClockSkew));
		}

		protected virtual DateTime GetCacheExpirationTime(Saml2SecurityToken token)
		{
			DateTime dateTime = DateTime.MaxValue;
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			Saml2Assertion assertion = token.Assertion;
			if (assertion != null)
			{
				if (assertion.Conditions != null && assertion.Conditions.NotOnOrAfter.HasValue)
				{
					dateTime = assertion.Conditions.NotOnOrAfter.Value;
				}
				else if (assertion.Subject != null && assertion.Subject.SubjectConfirmations != null && assertion.Subject.SubjectConfirmations.Count != 0 && assertion.Subject.SubjectConfirmations[0].SubjectConfirmationData != null && assertion.Subject.SubjectConfirmations[0].SubjectConfirmationData.NotOnOrAfter.HasValue)
				{
					dateTime = assertion.Subject.SubjectConfirmations[0].SubjectConfirmationData.NotOnOrAfter.Value;
				}
			}
			DateTime dateTime2 = DateTimeUtil.Add(DateTime.UtcNow, base.Configuration.TokenReplayCacheExpirationPeriod);
			if (DateTime.Compare(dateTime2, dateTime) < 0)
			{
				dateTime = dateTime2;
			}
			return dateTime;
		}

		protected virtual string NormalizeAuthenticationContextClassReference(string saml2AuthenticationContextClassReference)
		{
			return AuthenticationTypeMaps.Normalize(saml2AuthenticationContextClassReference, AuthenticationTypeMaps.Saml2);
		}

		protected virtual void ProcessSamlSubject(Saml2Subject assertionSubject, IClaimsIdentity subject, string issuer)
		{
			if (assertionSubject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertionSubject");
			}
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subject");
			}
			Saml2NameIdentifier nameId = assertionSubject.NameId;
			if (nameId != null)
			{
				Claim claim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", nameId.Value, "http://www.w3.org/2001/XMLSchema#string", issuer);
				if (nameId.Format != null)
				{
					claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/format"] = nameId.Format.AbsoluteUri;
				}
				if (nameId.NameQualifier != null)
				{
					claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/namequalifier"] = nameId.NameQualifier;
				}
				if (nameId.SPNameQualifier != null)
				{
					claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spnamequalifier"] = nameId.SPNameQualifier;
				}
				if (nameId.SPProvidedId != null)
				{
					claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/spprovidedid"] = nameId.SPProvidedId;
				}
				subject.Claims.Add(claim);
			}
		}

		protected virtual void ProcessAttributeStatement(Saml2AttributeStatement statement, IClaimsIdentity subject, string issuer)
		{
			if (statement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("statement");
			}
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subject");
			}
			foreach (Saml2Attribute attribute in statement.Attributes)
			{
				if (StringComparer.Ordinal.Equals(attribute.Name, "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor"))
				{
					if (subject.Actor != null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4218"));
					}
					SetDelegateFromAttribute(attribute, subject, issuer);
					continue;
				}
				foreach (string value in attribute.Values)
				{
					if (value != null)
					{
						string originalIssuer = issuer;
						if (attribute.OriginalIssuer != null)
						{
							originalIssuer = attribute.OriginalIssuer;
						}
						Claim claim = new Claim(attribute.Name, value, attribute.AttributeValueXsiType, issuer, originalIssuer);
						if (attribute.NameFormat != null)
						{
							claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename"] = attribute.NameFormat.AbsoluteUri;
						}
						if (attribute.FriendlyName != null)
						{
							claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname"] = attribute.FriendlyName;
						}
						subject.Claims.Add(claim);
					}
				}
			}
		}

		protected virtual void SetDelegateFromAttribute(Saml2Attribute attribute, IClaimsIdentity subject, string issuer)
		{
			if (subject == null || attribute == null || attribute.Values == null || attribute.Values.Count < 1)
			{
				return;
			}
			Saml2Attribute saml2Attribute = null;
			Collection<Claim> collection = new Collection<Claim>();
			foreach (string value in attribute.Values)
			{
				if (value == null)
				{
					continue;
				}
				using XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(value), XmlDictionaryReaderQuotas.Max);
				xmlDictionaryReader.MoveToContent();
				xmlDictionaryReader.ReadStartElement("Actor");
				while (xmlDictionaryReader.IsStartElement("Attribute"))
				{
					Saml2Attribute saml2Attribute2 = ReadAttribute(xmlDictionaryReader);
					if (saml2Attribute2 == null)
					{
						continue;
					}
					if (saml2Attribute2.Name == "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor")
					{
						if (saml2Attribute != null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4218"));
						}
						saml2Attribute = saml2Attribute2;
						continue;
					}
					string originalIssuer = saml2Attribute2.OriginalIssuer;
					for (int i = 0; i < saml2Attribute2.Values.Count; i++)
					{
						Claim claim = ((!string.IsNullOrEmpty(originalIssuer)) ? new Claim(saml2Attribute2.Name, saml2Attribute2.Values[i], saml2Attribute2.AttributeValueXsiType, issuer, originalIssuer) : new Claim(saml2Attribute2.Name, saml2Attribute2.Values[i], saml2Attribute2.AttributeValueXsiType, issuer));
						if (saml2Attribute2.NameFormat != null)
						{
							claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/attributename"] = saml2Attribute2.NameFormat.AbsoluteUri;
						}
						if (saml2Attribute2.FriendlyName != null)
						{
							claim.Properties["http://schemas.xmlsoap.org/ws/2005/05/identity/claimproperties/displayname"] = saml2Attribute2.FriendlyName;
						}
						collection.Add(claim);
					}
				}
				xmlDictionaryReader.ReadEndElement();
			}
			subject.Actor = new ClaimsIdentity(collection, "Federation");
			SetDelegateFromAttribute(saml2Attribute, subject.Actor, issuer);
		}

		protected virtual void ProcessAuthenticationStatement(Saml2AuthenticationStatement statement, IClaimsIdentity subject, string issuer)
		{
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subject");
			}
			if (statement.AuthenticationContext.DeclarationReference != null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4180"));
			}
			if (statement.AuthenticationContext.ClassReference != null)
			{
				subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", NormalizeAuthenticationContextClassReference(statement.AuthenticationContext.ClassReference.AbsoluteUri), "http://www.w3.org/2001/XMLSchema#string", issuer));
			}
			subject.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(statement.AuthenticationInstant.ToUniversalTime(), DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime", issuer));
		}

		protected virtual void ProcessAuthorizationDecisionStatement(Saml2AuthorizationDecisionStatement statement, IClaimsIdentity subject, string issuer)
		{
		}

		protected virtual void ProcessStatement(Collection<Saml2Statement> statements, IClaimsIdentity subject, string issuer)
		{
			Collection<Saml2AuthenticationStatement> collection = new Collection<Saml2AuthenticationStatement>();
			foreach (Saml2Statement statement in statements)
			{
				Saml2AttributeStatement saml2AttributeStatement = statement as Saml2AttributeStatement;
				if (saml2AttributeStatement != null)
				{
					ProcessAttributeStatement(saml2AttributeStatement, subject, issuer);
					continue;
				}
				Saml2AuthenticationStatement saml2AuthenticationStatement = statement as Saml2AuthenticationStatement;
				if (saml2AuthenticationStatement != null)
				{
					collection.Add(saml2AuthenticationStatement);
					continue;
				}
				Saml2AuthorizationDecisionStatement saml2AuthorizationDecisionStatement = statement as Saml2AuthorizationDecisionStatement;
				if (saml2AuthorizationDecisionStatement != null)
				{
					ProcessAuthorizationDecisionStatement(saml2AuthorizationDecisionStatement, subject, issuer);
				}
			}
			foreach (Saml2AuthenticationStatement item in collection)
			{
				if (item != null)
				{
					ProcessAuthenticationStatement(item, subject, issuer);
				}
			}
		}

		protected virtual IClaimsIdentity CreateClaims(Saml2SecurityToken samlToken)
		{
			if (samlToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("samlToken");
			}
			IClaimsIdentity claimsIdentity = new ClaimsIdentity("Federation", SamlSecurityTokenRequirement.NameClaimType, SamlSecurityTokenRequirement.RoleClaimType);
			Saml2Assertion assertion = samlToken.Assertion;
			if (assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("samlToken", SR.GetString("ID1034"));
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			if (base.Configuration.IssuerNameRegistry == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4277"));
			}
			string issuerName = base.Configuration.IssuerNameRegistry.GetIssuerName(samlToken.IssuerToken, assertion.Issuer.Value);
			if (string.IsNullOrEmpty(issuerName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4175")));
			}
			ProcessSamlSubject(assertion.Subject, claimsIdentity, issuerName);
			ProcessStatement(assertion.Statements, claimsIdentity, issuerName);
			return claimsIdentity;
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
			Saml2SecurityToken saml2SecurityToken = token as Saml2SecurityToken;
			if (saml2SecurityToken != null)
			{
				WriteAssertion(writer, saml2SecurityToken.Assertion);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID4160"));
		}

		public override bool CanReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				return false;
			}
			if (!reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				return reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			return true;
		}

		public override bool CanReadKeyIdentifierClause(XmlReader reader)
		{
			return IsSaml2KeyIdentifierClause(reader);
		}

		public override bool CanWriteKeyIdentifierClause(SecurityKeyIdentifierClause securityKeyIdentifierClause)
		{
			if (!(securityKeyIdentifierClause is Saml2AssertionKeyIdentifierClause))
			{
				return securityKeyIdentifierClause is WrappedSaml2AssertionKeyIdentifierClause;
			}
			return true;
		}

		internal static bool IsSaml2KeyIdentifierClause(XmlReader reader)
		{
			if (!reader.IsStartElement("SecurityTokenReference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
			{
				return false;
			}
			string attribute = reader.GetAttribute("TokenType", "http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd");
			return _tokenTypeIdentifiers.Contains(attribute);
		}

		internal static bool IsSaml2Assertion(XmlReader reader)
		{
			if (!reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				return reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			return true;
		}

		public override SecurityKeyIdentifierClause ReadKeyIdentifierClause(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!IsSaml2KeyIdentifierClause(reader))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4161"));
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", "SecurityTokenReference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"));
			}
			try
			{
				byte[] array = null;
				int derivationLength = 0;
				string attribute = reader.GetAttribute("Nonce", "http://schemas.xmlsoap.org/ws/2005/02/sc");
				if (!string.IsNullOrEmpty(attribute))
				{
					array = Convert.FromBase64String(attribute);
					attribute = reader.GetAttribute("Length", "http://schemas.xmlsoap.org/ws/2005/02/sc");
					derivationLength = (string.IsNullOrEmpty(attribute) ? 32 : XmlConvert.ToInt32(attribute));
				}
				if (array == null)
				{
					attribute = reader.GetAttribute("Nonce", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512");
					if (!string.IsNullOrEmpty(attribute))
					{
						array = Convert.FromBase64String(attribute);
						attribute = reader.GetAttribute("Length", "http://docs.oasis-open.org/ws-sx/ws-secureconversation/200512");
						derivationLength = (string.IsNullOrEmpty(attribute) ? 32 : XmlConvert.ToInt32(attribute));
					}
				}
				reader.Read();
				if (reader.IsStartElement("Reference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4126"));
				}
				if (!reader.IsStartElement("KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
				{
					reader.ReadStartElement("KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
				}
				attribute = reader.GetAttribute("ValueType");
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0001", "ValueType", "KeyIdentifier"));
				}
				if (!StringComparer.Ordinal.Equals("http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID", attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4127", attribute));
				}
				string id = reader.ReadElementString();
				reader.ReadEndElement();
				return new Saml2AssertionKeyIdentifierClause(id, array, derivationLength);
			}
			catch (Exception ex)
			{
				if (ex is FormatException || ex is ArgumentException || ex is InvalidOperationException || ex is OverflowException)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4125"), ex);
				}
				throw;
			}
		}

		protected virtual void ValidateConfirmationData(Saml2SubjectConfirmationData confirmationData)
		{
			if (confirmationData == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("confirmationData");
			}
			if (confirmationData.Address != null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4153")));
			}
			if (confirmationData.InResponseTo != null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4154")));
			}
			if (null != confirmationData.Recipient)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4157")));
			}
			DateTime utcNow = DateTime.UtcNow;
			if (confirmationData.NotBefore.HasValue && DateTimeUtil.Add(utcNow, base.Configuration.MaxClockSkew) < confirmationData.NotBefore.Value)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4176", confirmationData.NotBefore.Value, utcNow)));
			}
			if (confirmationData.NotOnOrAfter.HasValue && DateTimeUtil.Add(utcNow, base.Configuration.MaxClockSkew.Negate()) >= confirmationData.NotOnOrAfter.Value)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4177", confirmationData.NotOnOrAfter.Value, utcNow)));
			}
		}

		protected virtual ReadOnlyCollection<SecurityKey> ResolveSecurityKeys(Saml2Assertion assertion, SecurityTokenResolver resolver)
		{
			if (assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertion");
			}
			Saml2Subject subject = assertion.Subject;
			if (subject == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4130")));
			}
			if (subject.SubjectConfirmations.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4131")));
			}
			if (subject.SubjectConfirmations.Count > 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4132")));
			}
			Saml2SubjectConfirmation saml2SubjectConfirmation = subject.SubjectConfirmations[0];
			if (Saml2Constants.ConfirmationMethods.Bearer == saml2SubjectConfirmation.Method)
			{
				if (saml2SubjectConfirmation.SubjectConfirmationData != null && saml2SubjectConfirmation.SubjectConfirmationData.KeyIdentifiers.Count != 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4133")));
				}
				return EmptyReadOnlyCollection<SecurityKey>.Instance;
			}
			if (Saml2Constants.ConfirmationMethods.HolderOfKey == saml2SubjectConfirmation.Method)
			{
				if (saml2SubjectConfirmation.SubjectConfirmationData == null || saml2SubjectConfirmation.SubjectConfirmationData.KeyIdentifiers.Count == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4134")));
				}
				List<SecurityKey> list = new List<SecurityKey>();
				foreach (SecurityKeyIdentifier keyIdentifier in saml2SubjectConfirmation.SubjectConfirmationData.KeyIdentifiers)
				{
					SecurityKey key = null;
					foreach (SecurityKeyIdentifierClause item in keyIdentifier)
					{
						if (resolver != null && resolver.TryResolveSecurityKey(item, out key))
						{
							list.Add(key);
							break;
						}
					}
					if (key == null)
					{
						if (keyIdentifier.CanCreateKey)
						{
							key = keyIdentifier.CreateKey();
							list.Add(key);
						}
						else
						{
							list.Add(new SecurityKeyElement(keyIdentifier, resolver));
						}
					}
				}
				return list.AsReadOnly();
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4136", saml2SubjectConfirmation.Method)));
		}

		protected virtual SecurityToken ResolveIssuerToken(Saml2Assertion assertion, SecurityTokenResolver issuerResolver)
		{
			if (assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertion");
			}
			if (TryResolveIssuerToken(assertion, issuerResolver, out var token))
			{
				return token;
			}
			string @string = SR.GetString((assertion.SigningCredentials == null) ? "ID4141" : "ID4142");
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(@string));
		}

		protected virtual bool TryResolveIssuerToken(Saml2Assertion assertion, SecurityTokenResolver issuerResolver, out SecurityToken token)
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
			if (base.Configuration.ServiceTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4276"));
			}
			Saml2Assertion assertion = ReadAssertion(reader);
			ReadOnlyCollection<SecurityKey> keys = ResolveSecurityKeys(assertion, base.Configuration.ServiceTokenResolver);
			TryResolveIssuerToken(assertion, base.Configuration.IssuerTokenResolver, out var token);
			return new Saml2SecurityToken(assertion, keys, token);
		}

		public override void WriteKeyIdentifierClause(XmlWriter writer, SecurityKeyIdentifierClause securityKeyIdentifierClause)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (securityKeyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
			}
			Saml2AssertionKeyIdentifierClause saml2AssertionKeyIdentifierClause = securityKeyIdentifierClause as Saml2AssertionKeyIdentifierClause;
			WrappedSaml2AssertionKeyIdentifierClause wrappedSaml2AssertionKeyIdentifierClause = securityKeyIdentifierClause as WrappedSaml2AssertionKeyIdentifierClause;
			if (wrappedSaml2AssertionKeyIdentifierClause != null)
			{
				saml2AssertionKeyIdentifierClause = wrappedSaml2AssertionKeyIdentifierClause.WrappedClause;
			}
			if (saml2AssertionKeyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("keyIdentifierClause", SR.GetString("ID4162"));
			}
			writer.WriteStartElement("SecurityTokenReference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
			byte[] derivationNonce = saml2AssertionKeyIdentifierClause.GetDerivationNonce();
			if (derivationNonce != null)
			{
				writer.WriteAttributeString("Nonce", "http://schemas.xmlsoap.org/ws/2005/02/sc", Convert.ToBase64String(derivationNonce));
				int derivationLength = saml2AssertionKeyIdentifierClause.DerivationLength;
				if (derivationLength != 0 && derivationLength != 32)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4129")));
				}
			}
			writer.WriteAttributeString("TokenType", "http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV2.0");
			writer.WriteStartElement("KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
			writer.WriteAttributeString("ValueType", "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLID");
			writer.WriteString(saml2AssertionKeyIdentifierClause.Id);
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		private static void ReadEmptyContentElement(XmlReader reader)
		{
			bool isEmptyElement = reader.IsEmptyElement;
			reader.Read();
			if (!isEmptyElement)
			{
				reader.ReadEndElement();
			}
		}

		private static Saml2Id ReadSimpleNCNameElement(XmlReader reader)
		{
			try
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", reader.LocalName, reader.NamespaceURI));
				}
				XmlUtil.ValidateXsiType(reader, "NCName", "http://www.w3.org/2001/XMLSchema");
				reader.MoveToElement();
				string value = reader.ReadElementContentAsString();
				return new Saml2Id(value);
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		private static Uri ReadSimpleUriElement(XmlReader reader)
		{
			return ReadSimpleUriElement(reader, UriKind.Absolute);
		}

		private static Uri ReadSimpleUriElement(XmlReader reader, UriKind kind)
		{
			return ReadSimpleUriElement(reader, kind, allowLaxReading: false);
		}

		private static Uri ReadSimpleUriElement(XmlReader reader, UriKind kind, bool allowLaxReading)
		{
			try
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", reader.LocalName, reader.NamespaceURI));
				}
				XmlUtil.ValidateXsiType(reader, "anyURI", "http://www.w3.org/2001/XMLSchema");
				reader.MoveToElement();
				string text = reader.ReadElementContentAsString();
				if (string.IsNullOrEmpty(text))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0022"));
				}
				if (!allowLaxReading && !UriUtil.CanCreateValidUri(text, kind))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString((kind == UriKind.RelativeOrAbsolute) ? "ID0019" : "ID0013"));
				}
				return new Uri(text, kind);
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual Saml2NameIdentifier ReadSubjectID(XmlReader reader, string parentElement)
		{
			if (reader.IsStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				return ReadNameID(reader);
			}
			if (reader.IsStartElement("EncryptedID", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				return ReadEncryptedId(reader);
			}
			if (reader.IsStartElement("BaseID", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
				if (null == xsiType || XmlUtil.EqualsQName(xsiType, "BaseIDAbstractType", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4104", reader.LocalName, reader.NamespaceURI));
				}
				if (XmlUtil.EqualsQName(xsiType, "NameIDType", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					return ReadNameIDType(reader);
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4110", parentElement, xsiType.Name, xsiType.Namespace));
			}
			return null;
		}

		private static Exception TryWrapReadException(XmlReader reader, Exception inner)
		{
			if (inner is FormatException || inner is ArgumentException || inner is InvalidOperationException || inner is OverflowException)
			{
				return DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4125"), inner);
			}
			return null;
		}

		protected virtual Saml2Action ReadAction(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Action", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Action", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", "Action", "urn:oasis:names:tc:SAML:2.0:assertion"));
			}
			try
			{
				XmlUtil.ValidateXsiType(reader, "ActionType", "urn:oasis:names:tc:SAML:2.0:assertion");
				string attribute = reader.GetAttribute("Namespace");
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0001", "Namespace", "Action"));
				}
				if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0011", "Namespace", "Action"));
				}
				Uri actionNamespace = new Uri(attribute);
				return new Saml2Action(reader.ReadElementString(), actionNamespace);
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAction(XmlWriter writer, Saml2Action data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (null == data.Namespace)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data.Namespace");
			}
			if (string.IsNullOrEmpty(data.Namespace.ToString()))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("data.Namespace");
			}
			writer.WriteStartElement("Action", "urn:oasis:names:tc:SAML:2.0:assertion");
			writer.WriteAttributeString("Namespace", data.Namespace.AbsoluteUri);
			writer.WriteString(data.Value);
			writer.WriteEndElement();
		}

		protected virtual Saml2Advice ReadAdvice(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Advice", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Advice", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				Saml2Advice saml2Advice = new Saml2Advice();
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "AdviceType", "urn:oasis:names:tc:SAML:2.0:assertion");
				reader.Read();
				if (!isEmptyElement)
				{
					while (reader.IsStartElement())
					{
						if (reader.IsStartElement("AssertionIDRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
						{
							saml2Advice.AssertionIdReferences.Add(ReadSimpleNCNameElement(reader));
							continue;
						}
						if (reader.IsStartElement("AssertionURIRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
						{
							saml2Advice.AssertionUriReferences.Add(ReadSimpleUriElement(reader));
							continue;
						}
						if (reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
						{
							saml2Advice.Assertions.Add(ReadAssertion(reader));
							continue;
						}
						if (reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
						{
							saml2Advice.Assertions.Add(ReadAssertion(reader));
							continue;
						}
						if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
						{
							DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8006", reader.LocalName, reader.NamespaceURI));
						}
						reader.Skip();
					}
					reader.ReadEndElement();
				}
				return saml2Advice;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAdvice(XmlWriter writer, Saml2Advice data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("Advice", "urn:oasis:names:tc:SAML:2.0:assertion");
			foreach (Saml2Id assertionIdReference in data.AssertionIdReferences)
			{
				writer.WriteElementString("AssertionIDRef", "urn:oasis:names:tc:SAML:2.0:assertion", assertionIdReference.Value);
			}
			foreach (Uri assertionUriReference in data.AssertionUriReferences)
			{
				writer.WriteElementString("AssertionURIRef", "urn:oasis:names:tc:SAML:2.0:assertion", assertionUriReference.AbsoluteUri);
			}
			foreach (Saml2Assertion assertion in data.Assertions)
			{
				WriteAssertion(writer, assertion);
			}
			writer.WriteEndElement();
		}

		private static XmlDictionaryReader CreatePlaintextReaderFromEncryptedData(XmlDictionaryReader reader, SecurityTokenResolver serviceTokenResolver, SecurityTokenSerializer keyInfoSerializer, Collection<EncryptedKeyIdentifierClause> clauses, out Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", reader.LocalName, reader.NamespaceURI));
			}
			encryptingCredentials = null;
			XmlUtil.ValidateXsiType(reader, "EncryptedElementType", "urn:oasis:names:tc:SAML:2.0:assertion");
			reader.ReadStartElement();
			EncryptedDataElement encryptedDataElement = new EncryptedDataElement(keyInfoSerializer);
			encryptedDataElement.ReadXml(reader);
			reader.MoveToContent();
			while (reader.IsStartElement("EncryptedKey", "http://www.w3.org/2001/04/xmlenc#"))
			{
				SecurityKeyIdentifierClause securityKeyIdentifierClause;
				if (keyInfoSerializer.CanReadKeyIdentifierClause(reader))
				{
					securityKeyIdentifierClause = keyInfoSerializer.ReadKeyIdentifierClause(reader);
				}
				else
				{
					EncryptedKeyElement encryptedKeyElement = new EncryptedKeyElement(keyInfoSerializer);
					encryptedKeyElement.ReadXml(reader);
					securityKeyIdentifierClause = encryptedKeyElement.GetClause();
				}
				EncryptedKeyIdentifierClause encryptedKeyIdentifierClause = securityKeyIdentifierClause as EncryptedKeyIdentifierClause;
				if (encryptedKeyIdentifierClause == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4172"));
				}
				clauses.Add(encryptedKeyIdentifierClause);
			}
			reader.ReadEndElement();
			SecurityKey key = null;
			SecurityKeyIdentifierClause securityKeyIdentifierClause2 = null;
			foreach (SecurityKeyIdentifierClause item in encryptedDataElement.KeyIdentifier)
			{
				if (serviceTokenResolver.TryResolveSecurityKey(item, out key))
				{
					securityKeyIdentifierClause2 = item;
					break;
				}
			}
			if (key == null)
			{
				foreach (EncryptedKeyIdentifierClause clause in clauses)
				{
					if (serviceTokenResolver.TryResolveSecurityKey(clause, out key))
					{
						securityKeyIdentifierClause2 = clause;
						break;
					}
				}
			}
			if (key == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new EncryptedTokenDecryptionFailedException());
			}
			SymmetricSecurityKey symmetricSecurityKey = key as SymmetricSecurityKey;
			if (symmetricSecurityKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4023")));
			}
			SymmetricAlgorithm symmetricAlgorithm = symmetricSecurityKey.GetSymmetricAlgorithm(encryptedDataElement.Algorithm);
			byte[] buffer = encryptedDataElement.Decrypt(symmetricAlgorithm);
			encryptingCredentials = new ReceivedEncryptingCredentials(key, new SecurityKeyIdentifier(securityKeyIdentifierClause2), encryptedDataElement.Algorithm);
			return XmlDictionaryReader.CreateTextReader(buffer, reader.Quotas);
		}

		protected virtual Saml2Assertion ReadAssertion(XmlReader reader)
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
			if (base.Configuration.ServiceTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4276"));
			}
			XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			Saml2Assertion saml2Assertion = new Saml2Assertion(new Saml2NameIdentifier("__TemporaryIssuer__"));
			if (reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = null;
				xmlDictionaryReader = CreatePlaintextReaderFromEncryptedData(xmlDictionaryReader, base.Configuration.ServiceTokenResolver, KeyInfoSerializer, saml2Assertion.ExternalEncryptedKeys, out encryptingCredentials);
				saml2Assertion.EncryptingCredentials = encryptingCredentials;
			}
			if (!xmlDictionaryReader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				xmlDictionaryReader.ReadStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			if (xmlDictionaryReader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(xmlDictionaryReader, SR.GetString("ID3061", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
			}
			WrappedSerializer securityTokenSerializer = new WrappedSerializer(this, saml2Assertion);
			EnvelopedSignatureReader envelopedSignatureReader = new EnvelopedSignatureReader(xmlDictionaryReader, securityTokenSerializer, base.Configuration.IssuerTokenResolver, requireSignature: false, automaticallyReadSignature: false, resolveIntrinsicSigningKeys: false);
			try
			{
				XmlUtil.ValidateXsiType(envelopedSignatureReader, "AssertionType", "urn:oasis:names:tc:SAML:2.0:assertion");
				string attribute = envelopedSignatureReader.GetAttribute("Version");
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(envelopedSignatureReader, SR.GetString("ID0001", "Version", "Assertion"));
				}
				if (!StringComparer.Ordinal.Equals(saml2Assertion.Version, attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(envelopedSignatureReader, SR.GetString("ID4100", attribute));
				}
				string attribute2 = envelopedSignatureReader.GetAttribute("ID");
				if (string.IsNullOrEmpty(attribute2))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(envelopedSignatureReader, SR.GetString("ID0001", "ID", "Assertion"));
				}
				saml2Assertion.Id = new Saml2Id(attribute2);
				attribute2 = envelopedSignatureReader.GetAttribute("IssueInstant");
				if (string.IsNullOrEmpty(attribute2))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(envelopedSignatureReader, SR.GetString("ID0001", "IssueInstant", "Assertion"));
				}
				saml2Assertion.IssueInstant = XmlConvert.ToDateTime(attribute2, DateTimeFormats.Accepted);
				envelopedSignatureReader.Read();
				saml2Assertion.Issuer = ReadIssuer(envelopedSignatureReader);
				envelopedSignatureReader.TryReadSignature();
				if (envelopedSignatureReader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					saml2Assertion.Subject = ReadSubject(envelopedSignatureReader);
				}
				if (envelopedSignatureReader.IsStartElement("Conditions", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					saml2Assertion.Conditions = ReadConditions(envelopedSignatureReader);
				}
				if (envelopedSignatureReader.IsStartElement("Advice", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					saml2Assertion.Advice = ReadAdvice(envelopedSignatureReader);
				}
				while (envelopedSignatureReader.IsStartElement())
				{
					Saml2Statement item;
					if (envelopedSignatureReader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						item = ReadStatement(envelopedSignatureReader);
					}
					else if (envelopedSignatureReader.IsStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						item = ReadAttributeStatement(envelopedSignatureReader);
					}
					else if (envelopedSignatureReader.IsStartElement("AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						item = ReadAuthenticationStatement(envelopedSignatureReader);
					}
					else
					{
						if (!envelopedSignatureReader.IsStartElement("AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
						{
							break;
						}
						item = ReadAuthorizationDecisionStatement(envelopedSignatureReader);
					}
					saml2Assertion.Statements.Add(item);
				}
				envelopedSignatureReader.ReadEndElement();
				if (saml2Assertion.Subject == null)
				{
					if (saml2Assertion.Statements.Count == 0)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4106"));
					}
					foreach (Saml2Statement statement in saml2Assertion.Statements)
					{
						if (statement is Saml2AuthenticationStatement || statement is Saml2AttributeStatement || statement is Saml2AuthorizationDecisionStatement)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4119"));
						}
					}
				}
				saml2Assertion.SigningCredentials = envelopedSignatureReader.SigningCredentials;
				saml2Assertion.CaptureSourceData(envelopedSignatureReader);
				return saml2Assertion;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(envelopedSignatureReader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAssertion(XmlWriter writer, Saml2Assertion data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			XmlWriter xmlWriter = writer;
			MemoryStream memoryStream = null;
			XmlDictionaryWriter xmlDictionaryWriter = null;
			if (data.EncryptingCredentials != null && !(data.EncryptingCredentials is ReceivedEncryptingCredentials))
			{
				memoryStream = new MemoryStream();
				writer = (xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false));
			}
			else if (data.ExternalEncryptedKeys == null || data.ExternalEncryptedKeys.Count > 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4173")));
			}
			if (data.CanWriteSourceData)
			{
				data.WriteSourceData(writer);
			}
			else
			{
				EnvelopedSignatureWriter envelopedSignatureWriter = null;
				if (data.SigningCredentials != null)
				{
					writer = (envelopedSignatureWriter = new EnvelopedSignatureWriter(writer, data.SigningCredentials, data.Id.Value, new WrappedSerializer(this, data)));
				}
				if (data.Subject == null)
				{
					if (data.Statements == null || data.Statements.Count == 0)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4106")));
					}
					foreach (Saml2Statement statement in data.Statements)
					{
						if (statement is Saml2AuthenticationStatement || statement is Saml2AttributeStatement || statement is Saml2AuthorizationDecisionStatement)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4119")));
						}
					}
				}
				writer.WriteStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion");
				writer.WriteAttributeString("ID", data.Id.Value);
				writer.WriteAttributeString("IssueInstant", XmlConvert.ToString(data.IssueInstant.ToUniversalTime(), DateTimeFormats.Generated));
				writer.WriteAttributeString("Version", data.Version);
				WriteIssuer(writer, data.Issuer);
				envelopedSignatureWriter?.WriteSignature();
				if (data.Subject != null)
				{
					WriteSubject(writer, data.Subject);
				}
				if (data.Conditions != null)
				{
					WriteConditions(writer, data.Conditions);
				}
				if (data.Advice != null)
				{
					WriteAdvice(writer, data.Advice);
				}
				foreach (Saml2Statement statement2 in data.Statements)
				{
					WriteStatement(writer, statement2);
				}
				writer.WriteEndElement();
			}
			if (xmlDictionaryWriter == null)
			{
				return;
			}
			((IDisposable)xmlDictionaryWriter).Dispose();
			EncryptedDataElement encryptedDataElement = new EncryptedDataElement();
			encryptedDataElement.Type = "http://www.w3.org/2001/04/xmlenc#Element";
			encryptedDataElement.Algorithm = data.EncryptingCredentials.Algorithm;
			encryptedDataElement.KeyIdentifier = data.EncryptingCredentials.SecurityKeyIdentifier;
			SymmetricSecurityKey symmetricSecurityKey = data.EncryptingCredentials.SecurityKey as SymmetricSecurityKey;
			if (symmetricSecurityKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID3064")));
			}
			SymmetricAlgorithm symmetricAlgorithm = symmetricSecurityKey.GetSymmetricAlgorithm(data.EncryptingCredentials.Algorithm);
			encryptedDataElement.Encrypt(symmetricAlgorithm, memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			((IDisposable)memoryStream).Dispose();
			xmlWriter.WriteStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion");
			encryptedDataElement.WriteXml(xmlWriter, KeyInfoSerializer);
			foreach (EncryptedKeyIdentifierClause externalEncryptedKey in data.ExternalEncryptedKeys)
			{
				KeyInfoSerializer.WriteKeyIdentifierClause(xmlWriter, externalEncryptedKey);
			}
			xmlWriter.WriteEndElement();
		}

		protected virtual Saml2Attribute ReadAttribute(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "AttributeType", "urn:oasis:names:tc:SAML:2.0:assertion");
				string attribute = reader.GetAttribute("Name");
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0001", "Name", "Attribute"));
				}
				Saml2Attribute saml2Attribute = new Saml2Attribute(attribute);
				attribute = reader.GetAttribute("NameFormat");
				if (!string.IsNullOrEmpty(attribute))
				{
					if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0011", "Namespace", "Action"));
					}
					saml2Attribute.NameFormat = new Uri(attribute);
				}
				saml2Attribute.FriendlyName = reader.GetAttribute("FriendlyName");
				string attribute2 = reader.GetAttribute("OriginalIssuer", "http://schemas.xmlsoap.org/ws/2009/09/identity/claims");
				if (attribute2 == null)
				{
					attribute2 = reader.GetAttribute("OriginalIssuer", "http://schemas.microsoft.com/ws/2008/06/identity");
				}
				if (attribute2 == string.Empty)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4252")));
				}
				saml2Attribute.OriginalIssuer = attribute2;
				reader.Read();
				if (!isEmptyElement)
				{
					while (reader.IsStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						bool isEmptyElement2 = reader.IsEmptyElement;
						bool flag = XmlUtil.IsNil(reader);
						string text = null;
						string text2 = null;
						string attribute3 = reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
						if (!string.IsNullOrEmpty(attribute3))
						{
							if (attribute3.IndexOf(":", StringComparison.Ordinal) == -1)
							{
								text = reader.LookupNamespace(string.Empty);
								text2 = attribute3;
							}
							else if (attribute3.IndexOf(":", StringComparison.Ordinal) > 0 && attribute3.IndexOf(":", StringComparison.Ordinal) < attribute3.Length - 1)
							{
								string prefix = attribute3.Substring(0, attribute3.IndexOf(":", StringComparison.Ordinal));
								text = reader.LookupNamespace(prefix);
								text2 = attribute3.Substring(attribute3.IndexOf(":", StringComparison.Ordinal) + 1);
							}
						}
						if (text != null && text2 != null)
						{
							saml2Attribute.AttributeValueXsiType = text + "#" + text2;
						}
						if (flag)
						{
							reader.Read();
							if (!isEmptyElement2)
							{
								reader.ReadEndElement();
							}
							saml2Attribute.Values.Add(null);
						}
						else if (isEmptyElement2)
						{
							reader.Read();
							saml2Attribute.Values.Add("");
						}
						else
						{
							saml2Attribute.Values.Add(ReadAttributeValue(reader, saml2Attribute));
						}
					}
					reader.ReadEndElement();
				}
				return saml2Attribute;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual string ReadAttributeValue(XmlReader reader, Saml2Attribute attribute)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.ReadElementString();
		}

		protected virtual void WriteAttribute(XmlWriter writer, Saml2Attribute data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
			writer.WriteAttributeString("Name", data.Name);
			if (null != data.NameFormat)
			{
				writer.WriteAttributeString("NameFormat", data.NameFormat.AbsoluteUri);
			}
			if (data.FriendlyName != null)
			{
				writer.WriteAttributeString("FriendlyName", data.FriendlyName);
			}
			if (data.OriginalIssuer != null)
			{
				writer.WriteAttributeString("OriginalIssuer", "http://schemas.xmlsoap.org/ws/2009/09/identity/claims", data.OriginalIssuer);
			}
			string text = null;
			string text2 = null;
			if (!StringComparer.Ordinal.Equals(data.AttributeValueXsiType, "http://www.w3.org/2001/XMLSchema#string"))
			{
				int num = data.AttributeValueXsiType.IndexOf('#');
				text = data.AttributeValueXsiType.Substring(0, num);
				text2 = data.AttributeValueXsiType.Substring(num + 1);
			}
			foreach (string value in data.Values)
			{
				writer.WriteStartElement("AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
				if (value == null)
				{
					writer.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", XmlConvert.ToString(value: true));
				}
				else if (value.Length > 0)
				{
					if (text != null && text2 != null)
					{
						writer.WriteAttributeString("xmlns", "tn", null, text);
						writer.WriteAttributeString("type", "http://www.w3.org/2001/XMLSchema-instance", "tn:" + text2);
					}
					WriteAttributeValue(writer, value, data);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		protected virtual void WriteAttributeValue(XmlWriter writer, string value, Saml2Attribute attribute)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			writer.WriteString(value);
		}

		protected virtual Saml2AttributeStatement ReadAttributeStatement(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			bool requireDeclaration = false;
			if (reader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				requireDeclaration = true;
			}
			else if (!reader.IsStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "AttributeStatementType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
				if (isEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", "AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion"));
				}
				Saml2AttributeStatement saml2AttributeStatement = new Saml2AttributeStatement();
				reader.Read();
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("EncryptedAttribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4158"));
					}
					if (!reader.IsStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						break;
					}
					saml2AttributeStatement.Attributes.Add(ReadAttribute(reader));
				}
				if (saml2AttributeStatement.Attributes.Count == 0)
				{
					reader.ReadStartElement("Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
				}
				reader.ReadEndElement();
				return saml2AttributeStatement;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAttributeStatement(XmlWriter writer, Saml2AttributeStatement data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (data.Attributes == null || data.Attributes.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4124")));
			}
			writer.WriteStartElement("AttributeStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
			foreach (Saml2Attribute attribute in data.Attributes)
			{
				WriteAttribute(writer, attribute);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2AudienceRestriction ReadAudienceRestriction(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			bool requireDeclaration = false;
			if (reader.IsStartElement("Condition", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				requireDeclaration = true;
			}
			else if (!reader.IsStartElement("AudienceRestriction", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("AudienceRestriction", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "AudienceRestrictionType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
				if (isEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", reader.LocalName, reader.NamespaceURI));
				}
				reader.Read();
				if (!reader.IsStartElement("Audience", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					reader.ReadStartElement("Audience", "urn:oasis:names:tc:SAML:2.0:assertion");
				}
				Saml2AudienceRestriction saml2AudienceRestriction = new Saml2AudienceRestriction(ReadSimpleUriElement(reader, UriKind.RelativeOrAbsolute, allowLaxReading: true));
				while (reader.IsStartElement("Audience", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					saml2AudienceRestriction.Audiences.Add(ReadSimpleUriElement(reader, UriKind.RelativeOrAbsolute, allowLaxReading: true));
				}
				reader.ReadEndElement();
				return saml2AudienceRestriction;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAudienceRestriction(XmlWriter writer, Saml2AudienceRestriction data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (data.Audiences == null || data.Audiences.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4159")));
			}
			writer.WriteStartElement("AudienceRestriction", "urn:oasis:names:tc:SAML:2.0:assertion");
			foreach (Uri audience in data.Audiences)
			{
				writer.WriteElementString("Audience", "urn:oasis:names:tc:SAML:2.0:assertion", audience.OriginalString);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2AuthenticationContext ReadAuthenticationContext(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("AuthnContext", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("AuthnContext", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", "AuthnContext", "urn:oasis:names:tc:SAML:2.0:assertion"));
				}
				XmlUtil.ValidateXsiType(reader, "AuthnContextType", "urn:oasis:names:tc:SAML:2.0:assertion");
				reader.ReadStartElement();
				Uri uri = null;
				Uri declarationReference = null;
				if (reader.IsStartElement("AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					uri = ReadSimpleUriElement(reader);
				}
				if (reader.IsStartElement("AuthnContextDecl", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4118"));
				}
				if (reader.IsStartElement("AuthnContextDeclRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					declarationReference = ReadSimpleUriElement(reader);
				}
				else if (null == uri)
				{
					reader.ReadStartElement("AuthnContextDeclRef", "urn:oasis:names:tc:SAML:2.0:assertion");
				}
				Saml2AuthenticationContext saml2AuthenticationContext = new Saml2AuthenticationContext(uri, declarationReference);
				while (reader.IsStartElement("AuthenticatingAuthority", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					saml2AuthenticationContext.AuthenticatingAuthorities.Add(ReadSimpleUriElement(reader));
				}
				reader.ReadEndElement();
				return saml2AuthenticationContext;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAuthenticationContext(XmlWriter writer, Saml2AuthenticationContext data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (null == data.ClassReference && null == data.DeclarationReference)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4117")));
			}
			writer.WriteStartElement("AuthnContext", "urn:oasis:names:tc:SAML:2.0:assertion");
			if (null != data.ClassReference)
			{
				writer.WriteElementString("AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion", data.ClassReference.AbsoluteUri);
			}
			if (null != data.DeclarationReference)
			{
				writer.WriteElementString("AuthnContextDeclRef", "urn:oasis:names:tc:SAML:2.0:assertion", data.DeclarationReference.AbsoluteUri);
			}
			foreach (Uri authenticatingAuthority in data.AuthenticatingAuthorities)
			{
				writer.WriteElementString("AuthenticatingAuthority", "urn:oasis:names:tc:SAML:2.0:assertion", authenticatingAuthority.AbsoluteUri);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2AuthenticationStatement ReadAuthenticationStatement(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			bool requireDeclaration = false;
			if (reader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				requireDeclaration = true;
			}
			else if (!reader.IsStartElement("AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				DateTime? sessionNotOnOrAfter = null;
				Saml2SubjectLocality subjectLocality = null;
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "AuthnStatementType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
				if (isEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", "AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion"));
				}
				string attribute = reader.GetAttribute("AuthnInstant");
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0001", "AuthnInstant", "AuthnStatement"));
				}
				DateTime authenticationInstant = XmlConvert.ToDateTime(attribute, DateTimeFormats.Accepted);
				string attribute2 = reader.GetAttribute("SessionIndex");
				attribute = reader.GetAttribute("SessionNotOnOrAfter");
				if (!string.IsNullOrEmpty(attribute))
				{
					sessionNotOnOrAfter = XmlConvert.ToDateTime(attribute, DateTimeFormats.Accepted);
				}
				reader.Read();
				if (reader.IsStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					subjectLocality = ReadSubjectLocality(reader);
				}
				Saml2AuthenticationContext authenticationContext = ReadAuthenticationContext(reader);
				reader.ReadEndElement();
				Saml2AuthenticationStatement saml2AuthenticationStatement = new Saml2AuthenticationStatement(authenticationContext, authenticationInstant);
				saml2AuthenticationStatement.SessionIndex = attribute2;
				saml2AuthenticationStatement.SessionNotOnOrAfter = sessionNotOnOrAfter;
				saml2AuthenticationStatement.SubjectLocality = subjectLocality;
				return saml2AuthenticationStatement;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAuthenticationStatement(XmlWriter writer, Saml2AuthenticationStatement data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("AuthnStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
			writer.WriteAttributeString("AuthnInstant", XmlConvert.ToString(data.AuthenticationInstant.ToUniversalTime(), DateTimeFormats.Generated));
			if (data.SessionIndex != null)
			{
				writer.WriteAttributeString("SessionIndex", data.SessionIndex);
			}
			if (data.SessionNotOnOrAfter.HasValue)
			{
				writer.WriteAttributeString("SessionNotOnOrAfter", XmlConvert.ToString(data.SessionNotOnOrAfter.Value.ToUniversalTime(), DateTimeFormats.Generated));
			}
			if (data.SubjectLocality != null)
			{
				WriteSubjectLocality(writer, data.SubjectLocality);
			}
			WriteAuthenticationContext(writer, data.AuthenticationContext);
			writer.WriteEndElement();
		}

		protected virtual Saml2AuthorizationDecisionStatement ReadAuthorizationDecisionStatement(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			bool requireDeclaration = false;
			if (reader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				requireDeclaration = true;
			}
			else if (!reader.IsStartElement("AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "AuthzDecisionStatementType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
				if (isEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", "AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion"));
				}
				string attribute = reader.GetAttribute("Decision");
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0001", "Decision", "AuthzDecisionStatement"));
				}
				SamlAccessDecision decision;
				if (StringComparer.Ordinal.Equals(SamlAccessDecision.Permit.ToString(), attribute))
				{
					decision = SamlAccessDecision.Permit;
				}
				else if (StringComparer.Ordinal.Equals(SamlAccessDecision.Deny.ToString(), attribute))
				{
					decision = SamlAccessDecision.Deny;
				}
				else
				{
					if (!StringComparer.Ordinal.Equals(SamlAccessDecision.Indeterminate.ToString(), attribute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4123", attribute));
					}
					decision = SamlAccessDecision.Indeterminate;
				}
				attribute = reader.GetAttribute("Resource");
				if (attribute == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0001", "Resource", "AuthzDecisionStatement"));
				}
				Uri resource;
				if (attribute.Length == 0)
				{
					resource = Saml2AuthorizationDecisionStatement.EmptyResource;
				}
				else
				{
					if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4121"));
					}
					resource = new Uri(attribute);
				}
				Saml2AuthorizationDecisionStatement saml2AuthorizationDecisionStatement = new Saml2AuthorizationDecisionStatement(resource, decision);
				reader.Read();
				do
				{
					saml2AuthorizationDecisionStatement.Actions.Add(ReadAction(reader));
				}
				while (reader.IsStartElement("Action", "urn:oasis:names:tc:SAML:2.0:assertion"));
				if (reader.IsStartElement("Evidence", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					saml2AuthorizationDecisionStatement.Evidence = ReadEvidence(reader);
				}
				reader.ReadEndElement();
				return saml2AuthorizationDecisionStatement;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteAuthorizationDecisionStatement(XmlWriter writer, Saml2AuthorizationDecisionStatement data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (data.Actions.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4122")));
			}
			writer.WriteStartElement("AuthzDecisionStatement", "urn:oasis:names:tc:SAML:2.0:assertion");
			writer.WriteAttributeString("Decision", data.Decision.ToString());
			writer.WriteAttributeString("Resource", data.Resource.Equals(Saml2AuthorizationDecisionStatement.EmptyResource) ? data.Resource.ToString() : data.Resource.AbsoluteUri);
			foreach (Saml2Action action in data.Actions)
			{
				WriteAction(writer, action);
			}
			if (data.Evidence != null)
			{
				WriteEvidence(writer, data.Evidence);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2Conditions ReadConditions(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Conditions", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Conditions", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				Saml2Conditions saml2Conditions = new Saml2Conditions();
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "ConditionsType", "urn:oasis:names:tc:SAML:2.0:assertion");
				string attribute = reader.GetAttribute("NotBefore");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2Conditions.NotBefore = XmlConvert.ToDateTime(attribute, DateTimeFormats.Accepted);
				}
				attribute = reader.GetAttribute("NotOnOrAfter");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2Conditions.NotOnOrAfter = XmlConvert.ToDateTime(attribute, DateTimeFormats.Accepted);
				}
				reader.ReadStartElement();
				if (!isEmptyElement)
				{
					while (reader.IsStartElement())
					{
						if (reader.IsStartElement("Condition", "urn:oasis:names:tc:SAML:2.0:assertion"))
						{
							XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
							if (null == xsiType || XmlUtil.EqualsQName(xsiType, "ConditionAbstractType", "urn:oasis:names:tc:SAML:2.0:assertion"))
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4104", reader.LocalName, reader.NamespaceURI));
							}
							if (XmlUtil.EqualsQName(xsiType, "AudienceRestrictionType", "urn:oasis:names:tc:SAML:2.0:assertion"))
							{
								saml2Conditions.AudienceRestrictions.Add(ReadAudienceRestriction(reader));
								continue;
							}
							if (XmlUtil.EqualsQName(xsiType, "OneTimeUseType", "urn:oasis:names:tc:SAML:2.0:assertion"))
							{
								if (saml2Conditions.OneTimeUse)
								{
									throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4115", "OneTimeUse"));
								}
								ReadEmptyContentElement(reader);
								saml2Conditions.OneTimeUse = true;
								continue;
							}
							if (!XmlUtil.EqualsQName(xsiType, "ProxyRestrictionType", "urn:oasis:names:tc:SAML:2.0:assertion"))
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4113"));
							}
							if (saml2Conditions.ProxyRestriction != null)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4115", "ProxyRestriction"));
							}
							saml2Conditions.ProxyRestriction = ReadProxyRestriction(reader);
						}
						else if (reader.IsStartElement("AudienceRestriction", "urn:oasis:names:tc:SAML:2.0:assertion"))
						{
							saml2Conditions.AudienceRestrictions.Add(ReadAudienceRestriction(reader));
						}
						else if (reader.IsStartElement("OneTimeUse", "urn:oasis:names:tc:SAML:2.0:assertion"))
						{
							if (saml2Conditions.OneTimeUse)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4115", "OneTimeUse"));
							}
							ReadEmptyContentElement(reader);
							saml2Conditions.OneTimeUse = true;
						}
						else
						{
							if (!reader.IsStartElement("ProxyRestriction", "urn:oasis:names:tc:SAML:2.0:assertion"))
							{
								break;
							}
							if (saml2Conditions.ProxyRestriction != null)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4115", "ProxyRestriction"));
							}
							saml2Conditions.ProxyRestriction = ReadProxyRestriction(reader);
						}
					}
					reader.ReadEndElement();
				}
				return saml2Conditions;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteConditions(XmlWriter writer, Saml2Conditions data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("Conditions", "urn:oasis:names:tc:SAML:2.0:assertion");
			if (data.NotBefore.HasValue)
			{
				writer.WriteAttributeString("NotBefore", XmlConvert.ToString(data.NotBefore.Value.ToUniversalTime(), DateTimeFormats.Generated));
			}
			if (data.NotOnOrAfter.HasValue)
			{
				writer.WriteAttributeString("NotOnOrAfter", XmlConvert.ToString(data.NotOnOrAfter.Value.ToUniversalTime(), DateTimeFormats.Generated));
			}
			foreach (Saml2AudienceRestriction audienceRestriction in data.AudienceRestrictions)
			{
				WriteAudienceRestriction(writer, audienceRestriction);
			}
			if (data.OneTimeUse)
			{
				writer.WriteStartElement("OneTimeUse", "urn:oasis:names:tc:SAML:2.0:assertion");
				writer.WriteEndElement();
			}
			if (data.ProxyRestriction != null)
			{
				WriteProxyRestriction(writer, data.ProxyRestriction);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2Evidence ReadEvidence(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Evidence", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Evidence", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", "Evidence", "urn:oasis:names:tc:SAML:2.0:assertion"));
			}
			try
			{
				Saml2Evidence saml2Evidence = new Saml2Evidence();
				XmlUtil.ValidateXsiType(reader, "EvidenceType", "urn:oasis:names:tc:SAML:2.0:assertion");
				reader.Read();
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("AssertionIDRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						saml2Evidence.AssertionIdReferences.Add(ReadSimpleNCNameElement(reader));
					}
					else if (reader.IsStartElement("AssertionURIRef", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						saml2Evidence.AssertionUriReferences.Add(ReadSimpleUriElement(reader));
					}
					else if (reader.IsStartElement("Assertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						saml2Evidence.Assertions.Add(ReadAssertion(reader));
					}
					else if (reader.IsStartElement("EncryptedAssertion", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						saml2Evidence.Assertions.Add(ReadAssertion(reader));
					}
				}
				if (saml2Evidence.AssertionIdReferences.Count == 0 && saml2Evidence.Assertions.Count == 0 && saml2Evidence.AssertionUriReferences.Count == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4120"));
				}
				reader.ReadEndElement();
				return saml2Evidence;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteEvidence(XmlWriter writer, Saml2Evidence data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if ((data.AssertionIdReferences == null || data.AssertionIdReferences.Count == 0) && (data.Assertions == null || data.Assertions.Count == 0) && (data.AssertionUriReferences == null || data.AssertionUriReferences.Count == 0))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4120")));
			}
			writer.WriteStartElement("Evidence", "urn:oasis:names:tc:SAML:2.0:assertion");
			foreach (Saml2Id assertionIdReference in data.AssertionIdReferences)
			{
				writer.WriteElementString("AssertionIDRef", "urn:oasis:names:tc:SAML:2.0:assertion", assertionIdReference.Value);
			}
			foreach (Uri assertionUriReference in data.AssertionUriReferences)
			{
				writer.WriteElementString("AssertionURIRef", "urn:oasis:names:tc:SAML:2.0:assertion", assertionUriReference.AbsoluteUri);
			}
			foreach (Saml2Assertion assertion in data.Assertions)
			{
				WriteAssertion(writer, assertion);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2NameIdentifier ReadIssuer(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			return ReadNameIDType(reader);
		}

		protected virtual void WriteIssuer(XmlWriter writer, Saml2NameIdentifier data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
			WriteNameIDType(writer, data);
			writer.WriteEndElement();
		}

		protected virtual SecurityKeyIdentifier ReadSubjectKeyInfo(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return KeyInfoSerializer.ReadKeyIdentifier(reader);
		}

		protected virtual SecurityKeyIdentifier ReadSigningKeyInfo(XmlReader reader, Saml2Assertion assertion)
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
				return new SecurityKeyIdentifier(new Saml2SecurityKeyIdentifierClause(assertion));
			}
			return securityKeyIdentifier;
		}

		protected virtual void WriteSubjectKeyInfo(XmlWriter writer, SecurityKeyIdentifier data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			KeyInfoSerializer.WriteKeyIdentifier(writer, data);
		}

		protected virtual void WriteSigningKeyInfo(XmlWriter writer, SecurityKeyIdentifier signingKeyIdentifier)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (signingKeyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (KeyInfoSerializer.CanWriteKeyIdentifier(signingKeyIdentifier))
			{
				KeyInfoSerializer.WriteKeyIdentifier(writer, signingKeyIdentifier);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4221", signingKeyIdentifier));
		}

		protected virtual Saml2NameIdentifier ReadNameID(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			return ReadNameIDType(reader);
		}

		protected virtual void WriteNameID(XmlWriter writer, Saml2NameIdentifier data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (data.EncryptingCredentials != null)
			{
				Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = data.EncryptingCredentials;
				SymmetricSecurityKey symmetricSecurityKey = encryptingCredentials.SecurityKey as SymmetricSecurityKey;
				if (symmetricSecurityKey == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID3284")));
				}
				MemoryStream memoryStream = null;
				try
				{
					memoryStream = new MemoryStream();
					using (XmlWriter xmlWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false))
					{
						xmlWriter.WriteStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion");
						WriteNameIDType(xmlWriter, data);
						xmlWriter.WriteEndElement();
					}
					EncryptedDataElement encryptedDataElement = new EncryptedDataElement();
					encryptedDataElement.Type = "http://www.w3.org/2001/04/xmlenc#Element";
					encryptedDataElement.Algorithm = encryptingCredentials.Algorithm;
					encryptedDataElement.KeyIdentifier = encryptingCredentials.SecurityKeyIdentifier;
					SymmetricAlgorithm symmetricAlgorithm = symmetricSecurityKey.GetSymmetricAlgorithm(encryptingCredentials.Algorithm);
					encryptedDataElement.Encrypt(symmetricAlgorithm, memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
					((IDisposable)memoryStream).Dispose();
					writer.WriteStartElement("EncryptedID", "urn:oasis:names:tc:SAML:2.0:assertion");
					encryptedDataElement.WriteXml(writer, KeyInfoSerializer);
					foreach (EncryptedKeyIdentifierClause externalEncryptedKey in data.ExternalEncryptedKeys)
					{
						KeyInfoSerializer.WriteKeyIdentifierClause(writer, externalEncryptedKey);
					}
					writer.WriteEndElement();
				}
				finally
				{
					memoryStream?.Dispose();
				}
			}
			else
			{
				writer.WriteStartElement("NameID", "urn:oasis:names:tc:SAML:2.0:assertion");
				WriteNameIDType(writer, data);
				writer.WriteEndElement();
			}
		}

		protected virtual Saml2NameIdentifier ReadNameIDType(XmlReader reader)
		{
			try
			{
				reader.MoveToContent();
				Saml2NameIdentifier saml2NameIdentifier = new Saml2NameIdentifier("__TemporaryName__");
				XmlUtil.ValidateXsiType(reader, "NameIDType", "urn:oasis:names:tc:SAML:2.0:assertion");
				string attribute = reader.GetAttribute("Format");
				if (!string.IsNullOrEmpty(attribute))
				{
					if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0011", "Format", "NameID"));
					}
					saml2NameIdentifier.Format = new Uri(attribute);
				}
				attribute = reader.GetAttribute("NameQualifier");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2NameIdentifier.NameQualifier = attribute;
				}
				attribute = reader.GetAttribute("SPNameQualifier");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2NameIdentifier.SPNameQualifier = attribute;
				}
				attribute = reader.GetAttribute("SPProvidedID");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2NameIdentifier.SPProvidedId = attribute;
				}
				saml2NameIdentifier.Value = reader.ReadElementString();
				if (saml2NameIdentifier.Format != null && StringComparer.Ordinal.Equals(saml2NameIdentifier.Format.AbsoluteUri, Saml2Constants.NameIdentifierFormats.Entity.AbsoluteUri))
				{
					if (!UriUtil.CanCreateValidUri(saml2NameIdentifier.Value, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4262", saml2NameIdentifier.Value, Saml2Constants.NameIdentifierFormats.Entity.AbsoluteUri));
					}
					if (!string.IsNullOrEmpty(saml2NameIdentifier.NameQualifier) || !string.IsNullOrEmpty(saml2NameIdentifier.SPNameQualifier) || !string.IsNullOrEmpty(saml2NameIdentifier.SPProvidedId))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4263", saml2NameIdentifier.Value, Saml2Constants.NameIdentifierFormats.Entity.AbsoluteUri));
					}
				}
				return saml2NameIdentifier;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual Saml2NameIdentifier ReadEncryptedId(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			if (!reader.IsStartElement("EncryptedID", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("EncryptedID", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			Collection<EncryptedKeyIdentifierClause> collection = new Collection<EncryptedKeyIdentifierClause>();
			Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = null;
			using StringReader input = new StringReader(reader.ReadOuterXml());
			using XmlDictionaryReader reader2 = new IdentityModelWrappedXmlDictionaryReader(XmlReader.Create(input), XmlDictionaryReaderQuotas.Max);
			XmlReader reader3 = CreatePlaintextReaderFromEncryptedData(reader2, base.Configuration.ServiceTokenResolver, KeyInfoSerializer, collection, out encryptingCredentials);
			Saml2NameIdentifier saml2NameIdentifier = ReadNameIDType(reader3);
			saml2NameIdentifier.EncryptingCredentials = encryptingCredentials;
			foreach (EncryptedKeyIdentifierClause item in collection)
			{
				saml2NameIdentifier.ExternalEncryptedKeys.Add(item);
			}
			return saml2NameIdentifier;
		}

		protected virtual void WriteNameIDType(XmlWriter writer, Saml2NameIdentifier data)
		{
			if (null != data.Format)
			{
				writer.WriteAttributeString("Format", data.Format.AbsoluteUri);
			}
			if (!string.IsNullOrEmpty(data.NameQualifier))
			{
				writer.WriteAttributeString("NameQualifier", data.NameQualifier);
			}
			if (!string.IsNullOrEmpty(data.SPNameQualifier))
			{
				writer.WriteAttributeString("SPNameQualifier", data.SPNameQualifier);
			}
			if (!string.IsNullOrEmpty(data.SPProvidedId))
			{
				writer.WriteAttributeString("SPProvidedID", data.SPProvidedId);
			}
			writer.WriteString(data.Value);
		}

		protected virtual Saml2ProxyRestriction ReadProxyRestriction(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			bool requireDeclaration = false;
			if (reader.IsStartElement("Condition", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				requireDeclaration = true;
			}
			else if (!reader.IsStartElement("ProxyRestriction", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("ProxyRestriction", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				Saml2ProxyRestriction saml2ProxyRestriction = new Saml2ProxyRestriction();
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "ProxyRestrictionType", "urn:oasis:names:tc:SAML:2.0:assertion", requireDeclaration);
				string attribute = reader.GetAttribute("Count");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2ProxyRestriction.Count = XmlConvert.ToInt32(attribute);
				}
				reader.Read();
				if (!isEmptyElement)
				{
					while (reader.IsStartElement("Audience", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						saml2ProxyRestriction.Audiences.Add(ReadSimpleUriElement(reader));
					}
					reader.ReadEndElement();
				}
				return saml2ProxyRestriction;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteProxyRestriction(XmlWriter writer, Saml2ProxyRestriction data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("ProxyRestriction", "urn:oasis:names:tc:SAML:2.0:assertion");
			if (data.Count.HasValue)
			{
				writer.WriteAttributeString("Count", XmlConvert.ToString(data.Count.Value));
			}
			foreach (Uri audience in data.Audiences)
			{
				writer.WriteElementString("Audience", audience.AbsoluteUri);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2Statement ReadStatement(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Statement", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
			if (null == xsiType || XmlUtil.EqualsQName(xsiType, "StatementAbstractType", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4104", reader.LocalName, reader.NamespaceURI));
			}
			if (XmlUtil.EqualsQName(xsiType, "AttributeStatementType", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				return ReadAttributeStatement(reader);
			}
			if (XmlUtil.EqualsQName(xsiType, "AuthnStatementType", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				return ReadAuthenticationStatement(reader);
			}
			if (XmlUtil.EqualsQName(xsiType, "AuthzDecisionStatementType", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				return ReadAuthorizationDecisionStatement(reader);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4105", xsiType.Name, xsiType.Namespace));
		}

		protected virtual void WriteStatement(XmlWriter writer, Saml2Statement data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			Saml2AttributeStatement saml2AttributeStatement = data as Saml2AttributeStatement;
			if (saml2AttributeStatement != null)
			{
				WriteAttributeStatement(writer, saml2AttributeStatement);
				return;
			}
			Saml2AuthenticationStatement saml2AuthenticationStatement = data as Saml2AuthenticationStatement;
			if (saml2AuthenticationStatement != null)
			{
				WriteAuthenticationStatement(writer, saml2AuthenticationStatement);
				return;
			}
			Saml2AuthorizationDecisionStatement saml2AuthorizationDecisionStatement = data as Saml2AuthorizationDecisionStatement;
			if (saml2AuthorizationDecisionStatement != null)
			{
				WriteAuthorizationDecisionStatement(writer, saml2AuthorizationDecisionStatement);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4107", data.GetType().AssemblyQualifiedName)));
		}

		protected virtual Saml2Subject ReadSubject(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("Subject", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("Subject", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3061", reader.LocalName, reader.NamespaceURI));
				}
				XmlUtil.ValidateXsiType(reader, "SubjectType", "urn:oasis:names:tc:SAML:2.0:assertion");
				Saml2Subject saml2Subject = new Saml2Subject();
				reader.Read();
				saml2Subject.NameId = ReadSubjectID(reader, "Subject");
				while (reader.IsStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:2.0:assertion"))
				{
					saml2Subject.SubjectConfirmations.Add(ReadSubjectConfirmation(reader));
				}
				reader.ReadEndElement();
				if (saml2Subject.NameId == null && saml2Subject.SubjectConfirmations.Count == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4108"));
				}
				return saml2Subject;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteSubject(XmlWriter writer, Saml2Subject data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (data.NameId == null && data.SubjectConfirmations.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4108")));
			}
			writer.WriteStartElement("Subject", "urn:oasis:names:tc:SAML:2.0:assertion");
			if (data.NameId != null)
			{
				WriteNameID(writer, data.NameId);
			}
			foreach (Saml2SubjectConfirmation subjectConfirmation in data.SubjectConfirmations)
			{
				WriteSubjectConfirmation(writer, subjectConfirmation);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2SubjectConfirmation ReadSubjectConfirmation(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "SubjectConfirmationType", "urn:oasis:names:tc:SAML:2.0:assertion");
				string attribute = reader.GetAttribute("Method");
				if (string.IsNullOrEmpty(attribute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0001", "Method", "SubjectConfirmation"));
				}
				if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0011", "Method", "SubjectConfirmation"));
				}
				Saml2SubjectConfirmation saml2SubjectConfirmation = new Saml2SubjectConfirmation(new Uri(attribute));
				reader.Read();
				if (!isEmptyElement)
				{
					saml2SubjectConfirmation.NameIdentifier = ReadSubjectID(reader, "SubjectConfirmation");
					if (reader.IsStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						saml2SubjectConfirmation.SubjectConfirmationData = ReadSubjectConfirmationData(reader);
					}
					reader.ReadEndElement();
				}
				return saml2SubjectConfirmation;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteSubjectConfirmation(XmlWriter writer, Saml2SubjectConfirmation data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			if (null == data.Method)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data.Method");
			}
			if (string.IsNullOrEmpty(data.Method.ToString()))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("data.Method");
			}
			writer.WriteStartElement("SubjectConfirmation", "urn:oasis:names:tc:SAML:2.0:assertion");
			writer.WriteAttributeString("Method", data.Method.AbsoluteUri);
			if (data.NameIdentifier != null)
			{
				WriteNameID(writer, data.NameIdentifier);
			}
			if (data.SubjectConfirmationData != null)
			{
				WriteSubjectConfirmationData(writer, data.SubjectConfirmationData);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2SubjectConfirmationData ReadSubjectConfirmationData(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				Saml2SubjectConfirmationData saml2SubjectConfirmationData = new Saml2SubjectConfirmationData();
				bool isEmptyElement = reader.IsEmptyElement;
				bool flag = false;
				XmlQualifiedName xsiType = XmlUtil.GetXsiType(reader);
				if (null != xsiType)
				{
					if (XmlUtil.EqualsQName(xsiType, "KeyInfoConfirmationDataType", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						flag = true;
					}
					else if (!XmlUtil.EqualsQName(xsiType, "SubjectConfirmationDataType", "urn:oasis:names:tc:SAML:2.0:assertion"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4112", xsiType.Name, xsiType.Namespace));
					}
				}
				if (flag && isEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString(SR.GetString("ID4111")));
				}
				string attribute = reader.GetAttribute("Address");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2SubjectConfirmationData.Address = attribute;
				}
				attribute = reader.GetAttribute("InResponseTo");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2SubjectConfirmationData.InResponseTo = new Saml2Id(attribute);
				}
				attribute = reader.GetAttribute("NotBefore");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2SubjectConfirmationData.NotBefore = XmlConvert.ToDateTime(attribute, DateTimeFormats.Accepted);
				}
				attribute = reader.GetAttribute("NotOnOrAfter");
				if (!string.IsNullOrEmpty(attribute))
				{
					saml2SubjectConfirmationData.NotOnOrAfter = XmlConvert.ToDateTime(attribute, DateTimeFormats.Accepted);
				}
				attribute = reader.GetAttribute("Recipient");
				if (!string.IsNullOrEmpty(attribute))
				{
					if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0011", "Recipient", "SubjectConfirmationData"));
					}
					saml2SubjectConfirmationData.Recipient = new Uri(attribute);
				}
				reader.Read();
				if (!isEmptyElement)
				{
					if (flag)
					{
						saml2SubjectConfirmationData.KeyIdentifiers.Add(ReadSubjectKeyInfo(reader));
					}
					while (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
					{
						saml2SubjectConfirmationData.KeyIdentifiers.Add(ReadSubjectKeyInfo(reader));
					}
					if (!flag && XmlNodeType.EndElement != reader.NodeType)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4114", "SubjectConfirmationData"));
					}
					reader.ReadEndElement();
				}
				return saml2SubjectConfirmationData;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteSubjectConfirmationData(XmlWriter writer, Saml2SubjectConfirmationData data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("SubjectConfirmationData", "urn:oasis:names:tc:SAML:2.0:assertion");
			if (data.KeyIdentifiers != null && data.KeyIdentifiers.Count > 0)
			{
				writer.WriteAttributeString("type", "http://www.w3.org/2001/XMLSchema-instance", "KeyInfoConfirmationDataType");
			}
			if (!string.IsNullOrEmpty(data.Address))
			{
				writer.WriteAttributeString("Address", data.Address);
			}
			if (data.InResponseTo != null)
			{
				writer.WriteAttributeString("InResponseTo", data.InResponseTo.Value);
			}
			if (data.NotBefore.HasValue)
			{
				writer.WriteAttributeString("NotBefore", XmlConvert.ToString(data.NotBefore.Value.ToUniversalTime(), DateTimeFormats.Generated));
			}
			if (data.NotOnOrAfter.HasValue)
			{
				writer.WriteAttributeString("NotOnOrAfter", XmlConvert.ToString(data.NotOnOrAfter.Value.ToUniversalTime(), DateTimeFormats.Generated));
			}
			if (null != data.Recipient)
			{
				writer.WriteAttributeString("Recipient", data.Recipient.OriginalString);
			}
			foreach (SecurityKeyIdentifier keyIdentifier in data.KeyIdentifiers)
			{
				WriteSubjectKeyInfo(writer, keyIdentifier);
			}
			writer.WriteEndElement();
		}

		protected virtual Saml2SubjectLocality ReadSubjectLocality(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:2.0:assertion"))
			{
				reader.ReadStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:2.0:assertion");
			}
			try
			{
				Saml2SubjectLocality saml2SubjectLocality = new Saml2SubjectLocality();
				bool isEmptyElement = reader.IsEmptyElement;
				XmlUtil.ValidateXsiType(reader, "SubjectLocalityType", "urn:oasis:names:tc:SAML:2.0:assertion");
				saml2SubjectLocality.Address = reader.GetAttribute("Address");
				saml2SubjectLocality.DnsName = reader.GetAttribute("DNSName");
				reader.Read();
				if (!isEmptyElement)
				{
					reader.ReadEndElement();
				}
				return saml2SubjectLocality;
			}
			catch (Exception inner)
			{
				Exception ex = TryWrapReadException(reader, inner);
				if (ex == null)
				{
					throw;
				}
				throw ex;
			}
		}

		protected virtual void WriteSubjectLocality(XmlWriter writer, Saml2SubjectLocality data)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (data == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("data");
			}
			writer.WriteStartElement("SubjectLocality", "urn:oasis:names:tc:SAML:2.0:assertion");
			if (data.Address != null)
			{
				writer.WriteAttributeString("Address", data.Address);
			}
			if (data.DnsName != null)
			{
				writer.WriteAttributeString("DNSName", data.DnsName);
			}
			writer.WriteEndElement();
		}
	}
}
