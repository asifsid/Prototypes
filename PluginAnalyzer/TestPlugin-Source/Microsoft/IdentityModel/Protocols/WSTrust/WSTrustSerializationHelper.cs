using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	internal class WSTrustSerializationHelper
	{
		public static RequestSecurityToken CreateRequest(XmlReader reader, WSTrustSerializationContext context, WSTrustRequestSerializer requestSerializer, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (requestSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestSerializer");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (!reader.IsStartElement(trustConstants.Elements.RequestSecurityToken, trustConstants.NamespaceURI))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, trustConstants.Elements.RequestSecurityToken, trustConstants.NamespaceURI)));
			}
			bool isEmptyElement = reader.IsEmptyElement;
			RequestSecurityToken requestSecurityToken = requestSerializer.CreateRequestSecurityToken();
			requestSecurityToken.Context = reader.GetAttribute(trustConstants.Attributes.Context);
			reader.Read();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					requestSerializer.ReadXmlElement(reader, requestSecurityToken, context);
				}
				reader.ReadEndElement();
			}
			requestSerializer.Validate(requestSecurityToken);
			return requestSecurityToken;
		}

		public static void ReadRSTXml(XmlReader reader, RequestSecurityToken rst, WSTrustSerializationContext context, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (rst == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rst");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (reader.IsStartElement(trustConstants.Elements.TokenType, trustConstants.NamespaceURI))
			{
				rst.TokenType = reader.ReadElementContentAsString();
				if (!UriUtil.CanCreateValidUri(rst.TokenType, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.TokenType, trustConstants.NamespaceURI, rst.TokenType)));
				}
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.RequestType, trustConstants.NamespaceURI))
			{
				rst.RequestType = ReadRequestType(reader, trustConstants);
				return;
			}
			if (reader.IsStartElement("AppliesTo", "http://schemas.xmlsoap.org/ws/2004/09/policy"))
			{
				rst.AppliesTo = ReadAppliesTo(reader, trustConstants);
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.Issuer, trustConstants.NamespaceURI))
			{
				rst.Issuer = ReadOnBehalfOfIssuer(reader, trustConstants);
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.ProofEncryption, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					rst.ProofEncryption = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(ReadInnerXml(reader), context.SecurityTokenHandlers);
				}
				if (rst.ProofEncryption != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3218")));
			}
			if (reader.IsStartElement(trustConstants.Elements.Encryption, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					rst.Encryption = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(ReadInnerXml(reader), context.SecurityTokenHandlers);
				}
				if (rst.Encryption != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3268")));
			}
			if (reader.IsStartElement(trustConstants.Elements.DelegateTo, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					rst.DelegateTo = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(ReadInnerXml(reader), context.SecurityTokenHandlers);
				}
				if (rst.DelegateTo != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3219")));
			}
			if (reader.IsStartElement(trustConstants.Elements.Claims, trustConstants.NamespaceURI))
			{
				rst.Claims.Dialect = reader.GetAttribute(trustConstants.Attributes.Dialect);
				if (rst.Claims.Dialect != null && !UriUtil.CanCreateValidUri(rst.Claims.Dialect, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3136", trustConstants.Attributes.Dialect, reader.LocalName, reader.NamespaceURI, rst.Claims.Dialect)));
				}
				string requestClaimNamespace = GetRequestClaimNamespace(rst.Claims.Dialect);
				bool isEmptyElement = reader.IsEmptyElement;
				reader.ReadStartElement(trustConstants.Elements.Claims, trustConstants.NamespaceURI);
				if (isEmptyElement)
				{
					return;
				}
				while (reader.IsStartElement("ClaimType", requestClaimNamespace))
				{
					isEmptyElement = reader.IsEmptyElement;
					string attribute = reader.GetAttribute("Uri");
					if (string.IsNullOrEmpty(attribute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3009")));
					}
					bool isOptional = false;
					string attribute2 = reader.GetAttribute("Optional");
					if (!string.IsNullOrEmpty(attribute2))
					{
						isOptional = XmlConvert.ToBoolean(attribute2);
					}
					reader.Read();
					reader.MoveToContent();
					string value = null;
					if (!isEmptyElement)
					{
						if (reader.IsStartElement("Value", requestClaimNamespace))
						{
							if (!StringComparer.Ordinal.Equals(rst.Claims.Dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims"))
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3258", rst.Claims.Dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims")));
							}
							value = reader.ReadElementContentAsString("Value", requestClaimNamespace);
						}
						reader.ReadEndElement();
					}
					rst.Claims.Add(new RequestClaim(attribute, isOptional, value));
				}
				reader.ReadEndElement();
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI))
			{
				bool isEmptyElement = reader.IsEmptyElement;
				reader.ReadStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI);
				if (!isEmptyElement)
				{
					ProtectedKey protectedKey = ReadProtectedKey(reader, context, trustConstants);
					if (protectedKey == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3026")));
					}
					rst.Entropy = new Entropy(protectedKey);
					reader.ReadEndElement();
				}
				if (rst.Entropy != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3026")));
			}
			if (reader.IsStartElement(trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI))
			{
				rst.BinaryExchange = ReadBinaryExchange(reader, trustConstants);
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.Lifetime, trustConstants.NamespaceURI))
			{
				rst.Lifetime = ReadLifetime(reader, trustConstants);
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.RenewTarget, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					rst.RenewTarget = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(ReadInnerXml(reader), context.SecurityTokenHandlers);
				}
				if (rst.RenewTarget != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3151")));
			}
			if (reader.IsStartElement("RequestDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				rst.RequestDisplayToken = true;
				while (reader.MoveToNextAttribute())
				{
					if (StringComparer.Ordinal.Equals("xml:lang", reader.Name))
					{
						rst.DisplayTokenLanguage = reader.Value;
						break;
					}
				}
				reader.Skip();
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.OnBehalfOf, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					if (!context.SecurityTokenHandlerCollectionManager.ContainsKey("OnBehalfOf"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3264")));
					}
					rst.OnBehalfOf = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(ReadInnerXml(reader), context.SecurityTokenHandlerCollectionManager["OnBehalfOf"]);
				}
				if (rst.OnBehalfOf != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3152")));
			}
			if (reader.IsStartElement("ActAs", "http://docs.oasis-open.org/ws-sx/ws-trust/200802"))
			{
				if (!reader.IsEmptyElement)
				{
					if (!context.SecurityTokenHandlerCollectionManager.ContainsKey("ActAs"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3265")));
					}
					rst.ActAs = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(ReadInnerXml(reader), context.SecurityTokenHandlerCollectionManager["ActAs"]);
				}
				if (rst.ActAs != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3153")));
			}
			if (reader.IsStartElement(trustConstants.Elements.KeyType, trustConstants.NamespaceURI))
			{
				rst.KeyType = ReadKeyType(reader, trustConstants);
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.KeySize, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					rst.KeySizeInBits = int.Parse(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);
				}
				if (rst.KeySizeInBits.HasValue)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3154")));
			}
			if (reader.IsStartElement(trustConstants.Elements.UseKey, trustConstants.NamespaceURI))
			{
				bool isEmptyElement = reader.IsEmptyElement;
				reader.ReadStartElement();
				if (!isEmptyElement)
				{
					if (!context.SecurityTokenHandlers.CanReadToken(reader))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3165")));
					}
					SecurityToken securityToken = context.SecurityTokenHandlers.ReadToken(reader);
					SecurityKeyIdentifier securityKeyIdentifier = new SecurityKeyIdentifier();
					if (securityToken.CanCreateKeyIdentifierClause<RsaKeyIdentifierClause>())
					{
						securityKeyIdentifier.Add(securityToken.CreateKeyIdentifierClause<RsaKeyIdentifierClause>());
					}
					else
					{
						if (!securityToken.CanCreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>())
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3166")));
						}
						securityKeyIdentifier.Add(securityToken.CreateKeyIdentifierClause<X509RawDataKeyIdentifierClause>());
					}
					if (!context.UseKeyTokenResolver.TryResolveToken(securityKeyIdentifier, out var token))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidRequestException(SR.GetString("ID3092", securityKeyIdentifier)));
					}
					rst.UseKey = new UseKey(securityKeyIdentifier, token);
					reader.ReadEndElement();
				}
				if (rst.UseKey != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3155")));
			}
			if (reader.IsStartElement(trustConstants.Elements.SignWith, trustConstants.NamespaceURI))
			{
				rst.SignWith = reader.ReadElementContentAsString();
				if (UriUtil.CanCreateValidUri(rst.SignWith, UriKind.Absolute))
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.SignWith, trustConstants.NamespaceURI, rst.SignWith)));
			}
			if (reader.IsStartElement(trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI))
			{
				rst.EncryptWith = reader.ReadElementContentAsString();
				if (UriUtil.CanCreateValidUri(rst.EncryptWith, UriKind.Absolute))
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI, rst.EncryptWith)));
			}
			if (reader.IsStartElement(trustConstants.Elements.ComputedKeyAlgorithm, trustConstants.NamespaceURI))
			{
				rst.ComputedKeyAlgorithm = ReadComputedKeyAlgorithm(reader, trustConstants);
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI))
			{
				rst.AuthenticationType = reader.ReadElementContentAsString(trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI);
				if (UriUtil.CanCreateValidUri(rst.AuthenticationType, UriKind.Absolute))
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI, rst.AuthenticationType)));
			}
			if (reader.IsStartElement(trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI))
			{
				rst.EncryptionAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI);
				if (UriUtil.CanCreateValidUri(rst.EncryptionAlgorithm, UriKind.Absolute))
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI, rst.EncryptionAlgorithm)));
			}
			if (reader.IsStartElement(trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI))
			{
				rst.CanonicalizationAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI);
				if (UriUtil.CanCreateValidUri(rst.CanonicalizationAlgorithm, UriKind.Absolute))
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI, rst.CanonicalizationAlgorithm)));
			}
			if (reader.IsStartElement(trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI))
			{
				rst.SignatureAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI);
				if (UriUtil.CanCreateValidUri(rst.SignatureAlgorithm, UriKind.Absolute))
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI, rst.SignatureAlgorithm)));
			}
			if (reader.IsStartElement("InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				if (!reader.IsEmptyElement)
				{
					reader.ReadStartElement();
					string cardId = reader.ReadElementContentAsString("CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity");
					long num = reader.ReadElementContentAsLong("CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity");
					if (num < 1 || num > uint.MaxValue)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3263", 4294967295L)));
					}
					rst.InformationCardReference = new InformationCardReference(cardId, num);
					reader.ReadEndElement();
				}
				if (rst.InformationCardReference != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3156")));
			}
			if (reader.IsStartElement("ClientPseudonym", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				if (!reader.IsEmptyElement)
				{
					reader.ReadStartElement();
					rst.ClientPseudonym = reader.ReadElementString("PPID", "http://schemas.xmlsoap.org/ws/2005/05/identity");
					reader.ReadEndElement();
				}
				if (!string.IsNullOrEmpty(rst.ClientPseudonym))
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3157")));
			}
			if (reader.IsStartElement(trustConstants.Elements.Forwardable, trustConstants.NamespaceURI))
			{
				rst.Forwardable = reader.ReadElementContentAsBoolean();
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.Delegatable, trustConstants.NamespaceURI))
			{
				rst.Delegatable = reader.ReadElementContentAsBoolean();
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.AllowPostdating, trustConstants.NamespaceURI))
			{
				rst.AllowPostdating = true;
				bool isEmptyElement = reader.IsEmptyElement;
				reader.Read();
				reader.MoveToContent();
				if (!isEmptyElement)
				{
					reader.ReadEndElement();
				}
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.Renewing, trustConstants.NamespaceURI))
			{
				bool isEmptyElement = reader.IsEmptyElement;
				string attribute3 = reader.GetAttribute(trustConstants.Attributes.Allow);
				bool allowRenewal = true;
				bool okForRenewalAfterExpiration = false;
				if (!string.IsNullOrEmpty(attribute3))
				{
					allowRenewal = XmlConvert.ToBoolean(attribute3);
				}
				attribute3 = reader.GetAttribute(trustConstants.Attributes.OK);
				if (!string.IsNullOrEmpty(attribute3))
				{
					okForRenewalAfterExpiration = XmlConvert.ToBoolean(attribute3);
				}
				rst.Renewing = new Renewing(allowRenewal, okForRenewalAfterExpiration);
				reader.Read();
				reader.MoveToContent();
				if (!isEmptyElement)
				{
					reader.ReadEndElement();
				}
				return;
			}
			if (reader.IsStartElement(trustConstants.Elements.CancelTarget, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					rst.CancelTarget = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(ReadInnerXml(reader), context.SecurityTokenHandlers);
				}
				if (rst.CancelTarget != null)
				{
					return;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3220")));
			}
			if (reader.IsStartElement(trustConstants.Elements.Participants, trustConstants.NamespaceURI))
			{
				EndpointAddress primary = null;
				List<EndpointAddress> list = new List<EndpointAddress>();
				bool isEmptyElement = reader.IsEmptyElement;
				reader.Read();
				reader.MoveToContent();
				if (!isEmptyElement)
				{
					if (reader.IsStartElement(trustConstants.Elements.Primary, trustConstants.NamespaceURI))
					{
						reader.ReadStartElement(trustConstants.Elements.Primary, trustConstants.NamespaceURI);
						primary = EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader));
						reader.ReadEndElement();
					}
					while (reader.IsStartElement(trustConstants.Elements.Participant, trustConstants.NamespaceURI))
					{
						reader.ReadStartElement(trustConstants.Elements.Participant, trustConstants.NamespaceURI);
						list.Add(EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader)));
						reader.ReadEndElement();
					}
					if (reader.IsStartElement())
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3223", trustConstants.Elements.Participants, trustConstants.NamespaceURI, reader.LocalName, reader.NamespaceURI)));
					}
					rst.Participants = new Participants();
					rst.Participants.Primary = primary;
					rst.Participants.Participant.AddRange(list);
					reader.ReadEndElement();
				}
				return;
			}
			if (reader.IsStartElement("AdditionalContext", "http://docs.oasis-open.org/wsfed/authorization/200706"))
			{
				rst.AdditionalContext = new AdditionalContext();
				bool isEmptyElement = reader.IsEmptyElement;
				reader.Read();
				reader.MoveToContent();
				if (isEmptyElement)
				{
					return;
				}
				while (reader.IsStartElement("ContextItem", "http://docs.oasis-open.org/wsfed/authorization/200706"))
				{
					Uri result = null;
					Uri result2 = null;
					string value2 = null;
					string attribute4 = reader.GetAttribute("Name");
					if (string.IsNullOrEmpty(attribute4) || !UriUtil.TryCreateValidUri(attribute4, UriKind.Absolute, out result))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3136", "Name", reader.LocalName, reader.NamespaceURI, attribute4)));
					}
					attribute4 = reader.GetAttribute("Scope");
					if (!string.IsNullOrEmpty(attribute4) && !UriUtil.TryCreateValidUri(attribute4, UriKind.Absolute, out result2))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3136", "Scope", reader.LocalName, reader.NamespaceURI, attribute4)));
					}
					if (reader.IsEmptyElement)
					{
						reader.Read();
					}
					else
					{
						reader.Read();
						if (reader.IsStartElement("Value", "http://docs.oasis-open.org/wsfed/authorization/200706"))
						{
							value2 = reader.ReadElementContentAsString("Value", "http://docs.oasis-open.org/wsfed/authorization/200706");
						}
						reader.ReadEndElement();
					}
					rst.AdditionalContext.Items.Add(new ContextItem(result, value2, result2));
				}
				if (reader.IsStartElement())
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3223", "AdditionalContext", "http://docs.oasis-open.org/wsfed/authorization/200706", reader.LocalName, reader.NamespaceURI)));
				}
				reader.ReadEndElement();
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3007", reader.LocalName, reader.NamespaceURI)));
		}

		public static void WriteRequest(RequestSecurityToken rst, XmlWriter writer, WSTrustSerializationContext context, WSTrustRequestSerializer requestSerializer, WSTrustConstantsAdapter trustConstants)
		{
			if (rst == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rst");
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (requestSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestSerializer");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			requestSerializer.Validate(rst);
			writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestSecurityToken, trustConstants.NamespaceURI);
			if (rst.Context != null)
			{
				writer.WriteAttributeString(trustConstants.Attributes.Context, rst.Context);
			}
			requestSerializer.WriteKnownRequestElement(rst, writer, context);
			foreach (KeyValuePair<string, object> property in rst.Properties)
			{
				requestSerializer.WriteXmlElement(writer, property.Key, property.Value, rst, context);
			}
			writer.WriteEndElement();
		}

		public static void WriteKnownRequestElement(RequestSecurityToken rst, XmlWriter writer, WSTrustSerializationContext context, WSTrustRequestSerializer requestSerializer, WSTrustConstantsAdapter trustConstants)
		{
			if (rst == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rst");
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (requestSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestSerializer");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (rst.AppliesTo != null)
			{
				requestSerializer.WriteXmlElement(writer, "AppliesTo", rst.AppliesTo, rst, context);
			}
			if (rst.Claims.Count > 0)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Claims, rst.Claims, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.ComputedKeyAlgorithm))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.ComputedKeyAlgorithm, rst.ComputedKeyAlgorithm, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.SignWith))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.SignWith, rst.SignWith, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.EncryptWith))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.EncryptWith, rst.EncryptWith, rst, context);
			}
			if (rst.Entropy != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Entropy, rst.Entropy, rst, context);
			}
			if (rst.KeySizeInBits.HasValue)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.KeySize, rst.KeySizeInBits, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.KeyType))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.KeyType, rst.KeyType, rst, context);
			}
			if (rst.Lifetime != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Lifetime, rst.Lifetime, rst, context);
			}
			if (rst.RenewTarget != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.RenewTarget, rst.RenewTarget, rst, context);
			}
			if (rst.RequestDisplayToken)
			{
				requestSerializer.WriteXmlElement(writer, "RequestDisplayToken", rst.DisplayTokenLanguage, rst, context);
			}
			if (rst.OnBehalfOf != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.OnBehalfOf, rst.OnBehalfOf, rst, context);
			}
			if (rst.ActAs != null)
			{
				requestSerializer.WriteXmlElement(writer, "ActAs", rst.ActAs, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.RequestType))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestType, rst.RequestType, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.TokenType))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.TokenType, rst.TokenType, rst, context);
			}
			if (rst.UseKey != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.UseKey, rst.UseKey, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.AuthenticationType))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.AuthenticationType, rst.AuthenticationType, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.EncryptionAlgorithm))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.EncryptionAlgorithm, rst.EncryptionAlgorithm, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.CanonicalizationAlgorithm))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.CanonicalizationAlgorithm, rst.CanonicalizationAlgorithm, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.SignatureAlgorithm))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.SignatureAlgorithm, rst.SignatureAlgorithm, rst, context);
			}
			if (rst.InformationCardReference != null)
			{
				requestSerializer.WriteXmlElement(writer, "InformationCardReference", rst.InformationCardReference, rst, context);
			}
			if (!string.IsNullOrEmpty(rst.ClientPseudonym))
			{
				requestSerializer.WriteXmlElement(writer, "ClientPseudonym", rst.ClientPseudonym, rst, context);
			}
			if (rst.BinaryExchange != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.BinaryExchange, rst.BinaryExchange, rst, context);
			}
			if (rst.Issuer != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Issuer, rst.Issuer, rst, context);
			}
			if (rst.ProofEncryption != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.ProofEncryption, rst.ProofEncryption, rst, context);
			}
			if (rst.Encryption != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Encryption, rst.Encryption, rst, context);
			}
			if (rst.DelegateTo != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.DelegateTo, rst.DelegateTo, rst, context);
			}
			if (rst.Forwardable.HasValue)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Forwardable, rst.Forwardable.Value, rst, context);
			}
			if (rst.Delegatable.HasValue)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Delegatable, rst.Delegatable.Value, rst, context);
			}
			if (rst.AllowPostdating)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.AllowPostdating, rst.AllowPostdating, rst, context);
			}
			if (rst.Renewing != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Renewing, rst.Renewing, rst, context);
			}
			if (rst.CancelTarget != null)
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.CancelTarget, rst.CancelTarget, rst, context);
			}
			if (rst.Participants != null && (rst.Participants.Primary != null || rst.Participants.Participant.Count > 0))
			{
				requestSerializer.WriteXmlElement(writer, trustConstants.Elements.Participants, rst.Participants, rst, context);
			}
			if (rst.AdditionalContext != null)
			{
				requestSerializer.WriteXmlElement(writer, "AdditionalContext", rst.AdditionalContext, rst, context);
			}
		}

		public static void WriteRSTXml(XmlWriter writer, string elementName, object elementValue, WSTrustSerializationContext context, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (string.IsNullOrEmpty(elementName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("elementName");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (StringComparer.Ordinal.Equals(elementName, "AppliesTo"))
			{
				EndpointAddress appliesTo = elementValue as EndpointAddress;
				WriteAppliesTo(writer, appliesTo, trustConstants);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Claims))
			{
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Claims, trustConstants.NamespaceURI);
				RequestClaimCollection requestClaimCollection = (RequestClaimCollection)elementValue;
				if (requestClaimCollection.Dialect != null && !UriUtil.CanCreateValidUri(requestClaimCollection.Dialect, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3136", trustConstants.Attributes.Dialect, trustConstants.Elements.Claims, trustConstants.NamespaceURI, requestClaimCollection.Dialect)));
				}
				string requestClaimNamespace = GetRequestClaimNamespace(requestClaimCollection.Dialect);
				string text = writer.LookupPrefix(requestClaimNamespace);
				if (string.IsNullOrEmpty(text))
				{
					text = GetRequestClaimPrefix(requestClaimCollection.Dialect);
					writer.WriteAttributeString("xmlns", text, null, requestClaimNamespace);
				}
				writer.WriteAttributeString(trustConstants.Attributes.Dialect, (!string.IsNullOrEmpty(requestClaimCollection.Dialect)) ? requestClaimCollection.Dialect : "http://schemas.xmlsoap.org/ws/2005/05/identity");
				foreach (RequestClaim item in requestClaimCollection)
				{
					writer.WriteStartElement(text, "ClaimType", requestClaimNamespace);
					writer.WriteAttributeString("Uri", item.ClaimType);
					writer.WriteAttributeString("Optional", item.IsOptional ? "true" : "false");
					if (item.Value != null)
					{
						if (!StringComparer.Ordinal.Equals(requestClaimCollection.Dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims"))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3257", requestClaimCollection.Dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims")));
						}
						writer.WriteElementString(text, "Value", requestClaimNamespace, item.Value);
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.ComputedKeyAlgorithm))
			{
				WriteComputedKeyAlgorithm(writer, trustConstants.Elements.ComputedKeyAlgorithm, (string)elementValue, trustConstants);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.BinaryExchange))
			{
				WriteBinaryExchange(writer, elementValue as BinaryExchange, trustConstants);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Issuer))
			{
				WriteOnBehalfOfIssuer(writer, elementValue as EndpointAddress, trustConstants);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.SignWith))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.SignWith, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.SignWith, trustConstants.NamespaceURI, (string)elementValue);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.EncryptWith))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI, (string)elementValue);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Entropy))
			{
				Entropy entropy = elementValue as Entropy;
				if (entropy != null)
				{
					writer.WriteStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI);
					WriteProtectedKey(writer, entropy, context, trustConstants);
					writer.WriteEndElement();
				}
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.KeySize))
			{
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.KeySize, trustConstants.NamespaceURI, Convert.ToString((int)elementValue, CultureInfo.InvariantCulture));
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.KeyType))
			{
				WriteKeyType(writer, (string)elementValue, trustConstants);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Lifetime))
			{
				Lifetime lifetime = (Lifetime)elementValue;
				WriteLifetime(writer, lifetime, trustConstants);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RenewTarget))
			{
				Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement = elementValue as Microsoft.IdentityModel.Tokens.SecurityTokenElement;
				if (securityTokenElement == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("elementValue", SR.GetString("ID3222", trustConstants.Elements.RenewTarget, trustConstants.NamespaceURI, typeof(Microsoft.IdentityModel.Tokens.SecurityTokenElement), elementValue));
				}
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RenewTarget, trustConstants.NamespaceURI);
				if (securityTokenElement.SecurityTokenXml != null)
				{
					securityTokenElement.SecurityTokenXml.WriteTo(writer);
				}
				else
				{
					context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement.GetSecurityToken());
				}
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, "RequestDisplayToken"))
			{
				writer.WriteStartElement("i", "RequestDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				if (!string.IsNullOrEmpty((string)elementValue))
				{
					XmlUtil.WriteLanguageAttribute(writer, (string)elementValue);
				}
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.OnBehalfOf))
			{
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.OnBehalfOf, trustConstants.NamespaceURI);
				WriteTokenElement((Microsoft.IdentityModel.Tokens.SecurityTokenElement)elementValue, "OnBehalfOf", context, writer);
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, "ActAs"))
			{
				writer.WriteStartElement("tr", "ActAs", "http://docs.oasis-open.org/ws-sx/ws-trust/200802");
				WriteTokenElement((Microsoft.IdentityModel.Tokens.SecurityTokenElement)elementValue, "ActAs", context, writer);
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestType))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.RequestType, trustConstants.NamespaceURI, (string)elementValue)));
				}
				WriteRequestType(writer, (string)elementValue, trustConstants);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.TokenType))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.TokenType, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.TokenType, trustConstants.NamespaceURI, (string)elementValue);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.UseKey))
			{
				UseKey useKey = (UseKey)elementValue;
				if (useKey.Token == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3012")));
				}
				if (!context.SecurityTokenSerializer.CanWriteToken(useKey.Token))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3017")));
				}
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.UseKey, trustConstants.NamespaceURI);
				context.SecurityTokenSerializer.WriteToken(writer, useKey.Token);
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.AuthenticationType))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI, (string)elementValue);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.EncryptionAlgorithm))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI, (string)elementValue);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.CanonicalizationAlgorithm))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI, (string)elementValue);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.SignatureAlgorithm))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI, (string)elementValue);
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, "InformationCardReference"))
			{
				InformationCardReference informationCardReference = (InformationCardReference)elementValue;
				writer.WriteStartElement("i", "InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				writer.WriteElementString("i", "CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity", informationCardReference.CardId);
				writer.WriteElementString("i", "CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity", Convert.ToString(informationCardReference.CardVersion, CultureInfo.InvariantCulture));
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, "ClientPseudonym"))
			{
				string value = (string)elementValue;
				writer.WriteStartElement("i", "ClientPseudonym", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				writer.WriteElementString("i", "PPID", "http://schemas.xmlsoap.org/ws/2005/05/identity", value);
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Encryption))
			{
				Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement2 = (Microsoft.IdentityModel.Tokens.SecurityTokenElement)elementValue;
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Encryption, trustConstants.NamespaceURI);
				if (securityTokenElement2.SecurityTokenXml != null)
				{
					securityTokenElement2.SecurityTokenXml.WriteTo(writer);
				}
				else
				{
					context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement2.GetSecurityToken());
				}
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.ProofEncryption))
			{
				Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement3 = (Microsoft.IdentityModel.Tokens.SecurityTokenElement)elementValue;
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.ProofEncryption, trustConstants.NamespaceURI);
				if (securityTokenElement3.SecurityTokenXml != null)
				{
					securityTokenElement3.SecurityTokenXml.WriteTo(writer);
				}
				else
				{
					context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement3.GetSecurityToken());
				}
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.DelegateTo))
			{
				Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement4 = (Microsoft.IdentityModel.Tokens.SecurityTokenElement)elementValue;
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.DelegateTo, trustConstants.NamespaceURI);
				if (securityTokenElement4.SecurityTokenXml != null)
				{
					securityTokenElement4.SecurityTokenXml.WriteTo(writer);
				}
				else
				{
					context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement4.GetSecurityToken());
				}
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Forwardable))
			{
				if (!(elementValue is bool))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("elementValue", SR.GetString("ID3222", trustConstants.Elements.Forwardable, trustConstants.NamespaceURI, typeof(bool), elementValue));
				}
				writer.WriteStartElement(trustConstants.Elements.Forwardable, trustConstants.NamespaceURI);
				writer.WriteString(XmlConvert.ToString((bool)elementValue));
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Delegatable))
			{
				if (!(elementValue is bool))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("elementValue", SR.GetString("ID3222", trustConstants.Elements.Delegatable, trustConstants.NamespaceURI, typeof(bool), elementValue));
				}
				writer.WriteStartElement(trustConstants.Elements.Delegatable, trustConstants.NamespaceURI);
				writer.WriteString(XmlConvert.ToString((bool)elementValue));
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.AllowPostdating))
			{
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.AllowPostdating, trustConstants.NamespaceURI);
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Renewing))
			{
				Renewing renewing = elementValue as Renewing;
				if (renewing == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("elementValue", SR.GetString("ID3222", trustConstants.Elements.Renewing, trustConstants.NamespaceURI, typeof(Renewing), elementValue));
				}
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Renewing, trustConstants.NamespaceURI);
				writer.WriteAttributeString(trustConstants.Attributes.Allow, XmlConvert.ToString(renewing.AllowRenewal));
				writer.WriteAttributeString(trustConstants.Attributes.OK, XmlConvert.ToString(renewing.OkForRenewalAfterExpiration));
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.CancelTarget))
			{
				Microsoft.IdentityModel.Tokens.SecurityTokenElement securityTokenElement5 = elementValue as Microsoft.IdentityModel.Tokens.SecurityTokenElement;
				if (securityTokenElement5 == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("elementValue", SR.GetString("ID3222", trustConstants.Elements.CancelTarget, trustConstants.NamespaceURI, typeof(Microsoft.IdentityModel.Tokens.SecurityTokenElement), elementValue));
				}
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.CancelTarget, trustConstants.NamespaceURI);
				if (securityTokenElement5.SecurityTokenXml != null)
				{
					securityTokenElement5.SecurityTokenXml.WriteTo(writer);
				}
				else
				{
					context.SecurityTokenSerializer.WriteToken(writer, securityTokenElement5.GetSecurityToken());
				}
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Participants))
			{
				Participants participants = elementValue as Participants;
				if (participants == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("elementValue", SR.GetString("ID3222", trustConstants.Elements.Participant, trustConstants.NamespaceURI, typeof(Participants), elementValue));
				}
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Participants, trustConstants.NamespaceURI);
				if (participants.Primary != null)
				{
					writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Primary, trustConstants.NamespaceURI);
					participants.Primary.WriteTo(AddressingVersion.WSAddressing10, writer);
					writer.WriteEndElement();
				}
				foreach (EndpointAddress item2 in participants.Participant)
				{
					writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Participant, trustConstants.NamespaceURI);
					item2.WriteTo(AddressingVersion.WSAddressing10, writer);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				return;
			}
			if (StringComparer.Ordinal.Equals(elementName, "AdditionalContext"))
			{
				AdditionalContext additionalContext = elementValue as AdditionalContext;
				if (additionalContext == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("elementValue", SR.GetString("ID3222", "AdditionalContext", "http://docs.oasis-open.org/wsfed/authorization/200706", typeof(AdditionalContext), elementValue));
				}
				writer.WriteStartElement("auth", "AdditionalContext", "http://docs.oasis-open.org/wsfed/authorization/200706");
				foreach (ContextItem item3 in additionalContext.Items)
				{
					writer.WriteStartElement("auth", "ContextItem", "http://docs.oasis-open.org/wsfed/authorization/200706");
					writer.WriteAttributeString("Name", item3.Name.AbsoluteUri);
					if (item3.Scope != null)
					{
						writer.WriteAttributeString("Scope", item3.Scope.AbsoluteUri);
					}
					if (item3.Value != null)
					{
						writer.WriteElementString("Value", "http://docs.oasis-open.org/wsfed/authorization/200706", item3.Value);
					}
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3013", elementName, elementValue.GetType())));
		}

		private static void WriteTokenElement(Microsoft.IdentityModel.Tokens.SecurityTokenElement tokenElement, string usage, WSTrustSerializationContext context, XmlWriter writer)
		{
			if (tokenElement.SecurityTokenXml != null)
			{
				tokenElement.SecurityTokenXml.WriteTo(writer);
				return;
			}
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlerCollection = ((!context.SecurityTokenHandlerCollectionManager.ContainsKey(usage)) ? context.SecurityTokenHandlers : context.SecurityTokenHandlerCollectionManager[usage]);
			SecurityToken securityToken = tokenElement.GetSecurityToken();
			bool flag = false;
			if (securityTokenHandlerCollection != null && securityTokenHandlerCollection.CanWriteToken(securityToken))
			{
				securityTokenHandlerCollection.WriteToken(writer, securityToken);
				flag = true;
			}
			if (!flag)
			{
				context.SecurityTokenSerializer.WriteToken(writer, securityToken);
			}
		}

		public static RequestSecurityTokenResponse CreateResponse(XmlReader reader, WSTrustSerializationContext context, WSTrustResponseSerializer responseSerializer, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (responseSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseSerializer");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (!reader.IsStartElement(trustConstants.Elements.RequestSecurityTokenResponse, trustConstants.NamespaceURI))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, trustConstants.Elements.RequestSecurityTokenResponse, trustConstants.NamespaceURI)));
			}
			RequestSecurityTokenResponse requestSecurityTokenResponse = responseSerializer.CreateInstance();
			bool isEmptyElement = reader.IsEmptyElement;
			requestSecurityTokenResponse.Context = reader.GetAttribute(trustConstants.Attributes.Context);
			reader.Read();
			if (!isEmptyElement)
			{
				while (reader.IsStartElement())
				{
					responseSerializer.ReadXmlElement(reader, requestSecurityTokenResponse, context);
				}
				reader.ReadEndElement();
			}
			responseSerializer.Validate(requestSecurityTokenResponse);
			return requestSecurityTokenResponse;
		}

		public static void ReadRSTRXml(XmlReader reader, RequestSecurityTokenResponse rstr, WSTrustSerializationContext context, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (rstr == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rstr");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (reader.IsStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					reader.ReadStartElement(trustConstants.Elements.Entropy, trustConstants.NamespaceURI);
					ProtectedKey protectedKey = ReadProtectedKey(reader, context, trustConstants);
					if (protectedKey == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3026")));
					}
					rstr.Entropy = new Entropy(protectedKey);
					reader.ReadEndElement();
				}
				if (rstr.Entropy == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3026")));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.KeySize, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					rstr.KeySizeInBits = Convert.ToInt32(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);
				}
				if (!rstr.KeySizeInBits.HasValue)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3154")));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.RequestType, trustConstants.NamespaceURI))
			{
				rstr.RequestType = ReadRequestType(reader, trustConstants);
			}
			else if (reader.IsStartElement(trustConstants.Elements.Lifetime, trustConstants.NamespaceURI))
			{
				rstr.Lifetime = ReadLifetime(reader, trustConstants);
			}
			else if (reader.IsStartElement(trustConstants.Elements.RequestedSecurityToken, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					rstr.RequestedSecurityToken = new RequestedSecurityToken(ReadInnerXml(reader));
				}
				if (rstr.RequestedSecurityToken == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3158")));
				}
			}
			else if (reader.IsStartElement("RequestedDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				rstr.RequestedDisplayToken = ReadRequestedDisplayToken(reader, trustConstants);
			}
			else if (reader.IsStartElement("AppliesTo", "http://schemas.xmlsoap.org/ws/2004/09/policy"))
			{
				rstr.AppliesTo = ReadAppliesTo(reader, trustConstants);
			}
			else if (reader.IsStartElement(trustConstants.Elements.RequestedProofToken, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					reader.ReadStartElement();
					if (reader.LocalName == trustConstants.Elements.ComputedKey && reader.NamespaceURI == trustConstants.NamespaceURI)
					{
						rstr.RequestedProofToken = new RequestedProofToken(ReadComputedKeyAlgorithm(reader, trustConstants));
					}
					else
					{
						ProtectedKey protectedKey2 = ReadProtectedKey(reader, context, trustConstants);
						if (protectedKey2 == null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3025")));
						}
						rstr.RequestedProofToken = new RequestedProofToken(protectedKey2);
					}
					reader.ReadEndElement();
				}
				if (rstr.RequestedProofToken == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3025")));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.RequestedAttachedReference, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					reader.ReadStartElement();
					rstr.RequestedAttachedReference = context.SecurityTokenSerializer.ReadKeyIdentifierClause(reader);
					reader.ReadEndElement();
				}
				if (rstr.RequestedAttachedReference == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3159")));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.RequestedUnattachedReference, trustConstants.NamespaceURI))
			{
				if (!reader.IsEmptyElement)
				{
					reader.ReadStartElement();
					rstr.RequestedUnattachedReference = context.SecurityTokenSerializer.ReadKeyIdentifierClause(reader);
					reader.ReadEndElement();
				}
				if (rstr.RequestedUnattachedReference == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3160")));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.TokenType, trustConstants.NamespaceURI))
			{
				rstr.TokenType = reader.ReadElementContentAsString();
				if (!UriUtil.CanCreateValidUri(rstr.TokenType, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.TokenType, trustConstants.NamespaceURI, rstr.TokenType)));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.KeyType, trustConstants.NamespaceURI))
			{
				rstr.KeyType = ReadKeyType(reader, trustConstants);
			}
			else if (reader.IsStartElement(trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI))
			{
				rstr.AuthenticationType = reader.ReadElementContentAsString(trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI);
				if (!UriUtil.CanCreateValidUri(rstr.AuthenticationType, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI, rstr.AuthenticationType)));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI))
			{
				rstr.EncryptionAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI);
				if (!UriUtil.CanCreateValidUri(rstr.EncryptionAlgorithm, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI, rstr.EncryptionAlgorithm)));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI))
			{
				rstr.CanonicalizationAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI);
				if (!UriUtil.CanCreateValidUri(rstr.CanonicalizationAlgorithm, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI, rstr.CanonicalizationAlgorithm)));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI))
			{
				rstr.SignatureAlgorithm = reader.ReadElementContentAsString(trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI);
				if (!UriUtil.CanCreateValidUri(rstr.SignatureAlgorithm, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI, rstr.SignatureAlgorithm)));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.SignWith, trustConstants.NamespaceURI))
			{
				rstr.SignWith = reader.ReadElementContentAsString();
				if (!UriUtil.CanCreateValidUri(rstr.SignWith, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.SignWith, trustConstants.NamespaceURI, rstr.SignWith)));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI))
			{
				rstr.EncryptWith = reader.ReadElementContentAsString();
				if (!UriUtil.CanCreateValidUri(rstr.EncryptWith, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI, rstr.EncryptWith)));
				}
			}
			else if (reader.IsStartElement(trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI))
			{
				rstr.BinaryExchange = ReadBinaryExchange(reader, trustConstants);
			}
			else if (reader.IsStartElement(trustConstants.Elements.Status, trustConstants.NamespaceURI))
			{
				rstr.Status = ReadStatus(reader, trustConstants);
			}
			else
			{
				if (!reader.IsStartElement(trustConstants.Elements.RequestedTokenCancelled, trustConstants.NamespaceURI))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3007", reader.LocalName, reader.NamespaceURI)));
				}
				rstr.RequestedTokenCancelled = true;
				reader.ReadStartElement();
			}
		}

		public static void WriteResponse(RequestSecurityTokenResponse response, XmlWriter writer, WSTrustSerializationContext context, WSTrustResponseSerializer responseSerializer, WSTrustConstantsAdapter trustConstants)
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (responseSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseSerializer");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			responseSerializer.Validate(response);
			writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestSecurityTokenResponse, trustConstants.NamespaceURI);
			if (!string.IsNullOrEmpty(response.Context))
			{
				writer.WriteAttributeString(trustConstants.Attributes.Context, response.Context);
			}
			responseSerializer.WriteKnownResponseElement(response, writer, context);
			foreach (KeyValuePair<string, object> property in response.Properties)
			{
				responseSerializer.WriteXmlElement(writer, property.Key, property.Value, response, context);
			}
			writer.WriteEndElement();
		}

		public static void WriteKnownResponseElement(RequestSecurityTokenResponse rstr, XmlWriter writer, WSTrustSerializationContext context, WSTrustResponseSerializer responseSerializer, WSTrustConstantsAdapter trustConstants)
		{
			if (rstr == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rstr");
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (responseSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("responseSerializer");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (rstr.Entropy != null)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.Entropy, rstr.Entropy, rstr, context);
			}
			if (rstr.KeySizeInBits.HasValue)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.KeySize, rstr.KeySizeInBits, rstr, context);
			}
			if (rstr.Lifetime != null)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.Lifetime, rstr.Lifetime, rstr, context);
			}
			if (rstr.AppliesTo != null)
			{
				responseSerializer.WriteXmlElement(writer, "AppliesTo", rstr.AppliesTo, rstr, context);
			}
			if (rstr.RequestedSecurityToken != null)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedSecurityToken, rstr.RequestedSecurityToken, rstr, context);
			}
			if (rstr.RequestedDisplayToken != null)
			{
				responseSerializer.WriteXmlElement(writer, "RequestedDisplayToken", rstr.RequestedDisplayToken, rstr, context);
			}
			if (rstr.RequestedProofToken != null)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedProofToken, rstr.RequestedProofToken, rstr, context);
			}
			if (rstr.RequestedAttachedReference != null)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedAttachedReference, rstr.RequestedAttachedReference, rstr, context);
			}
			if (rstr.RequestedUnattachedReference != null)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedUnattachedReference, rstr.RequestedUnattachedReference, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.SignWith))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.SignWith, rstr.SignWith, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.EncryptWith))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.EncryptWith, rstr.EncryptWith, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.TokenType))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.TokenType, rstr.TokenType, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.RequestType))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestType, rstr.RequestType, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.KeyType))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.KeyType, rstr.KeyType, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.AuthenticationType))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.AuthenticationType, rstr.AuthenticationType, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.EncryptionAlgorithm))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.EncryptionAlgorithm, rstr.EncryptionAlgorithm, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.CanonicalizationAlgorithm))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.CanonicalizationAlgorithm, rstr.CanonicalizationAlgorithm, rstr, context);
			}
			if (!string.IsNullOrEmpty(rstr.SignatureAlgorithm))
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.SignatureAlgorithm, rstr.SignatureAlgorithm, rstr, context);
			}
			if (rstr.BinaryExchange != null)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.BinaryExchange, rstr.BinaryExchange, rstr, context);
			}
			if (rstr.Status != null)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.Status, rstr.Status, rstr, context);
			}
			if (rstr.RequestedTokenCancelled)
			{
				responseSerializer.WriteXmlElement(writer, trustConstants.Elements.RequestedTokenCancelled, rstr.RequestedTokenCancelled, rstr, context);
			}
		}

		public static void WriteRSTRXml(XmlWriter writer, string elementName, object elementValue, WSTrustSerializationContext context, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("context");
			}
			if (string.IsNullOrEmpty(elementName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("elementName");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Entropy))
			{
				Entropy entropy = elementValue as Entropy;
				if (entropy != null)
				{
					writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Entropy, trustConstants.NamespaceURI);
					WriteProtectedKey(writer, entropy, context, trustConstants);
					writer.WriteEndElement();
				}
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.KeySize))
			{
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.KeySize, trustConstants.NamespaceURI, Convert.ToString((int)elementValue, CultureInfo.InvariantCulture));
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Lifetime))
			{
				Lifetime lifetime = (Lifetime)elementValue;
				WriteLifetime(writer, lifetime, trustConstants);
			}
			else if (StringComparer.Ordinal.Equals(elementName, "AppliesTo"))
			{
				EndpointAddress appliesTo = elementValue as EndpointAddress;
				WriteAppliesTo(writer, appliesTo, trustConstants);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedSecurityToken))
			{
				RequestedSecurityToken requestedSecurityToken = (RequestedSecurityToken)elementValue;
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedSecurityToken, trustConstants.NamespaceURI);
				if (requestedSecurityToken.SecurityTokenXml != null)
				{
					requestedSecurityToken.SecurityTokenXml.WriteTo(writer);
				}
				else
				{
					context.SecurityTokenSerializer.WriteToken(writer, requestedSecurityToken.SecurityToken);
				}
				writer.WriteEndElement();
			}
			else if (StringComparer.Ordinal.Equals(elementName, "RequestedDisplayToken"))
			{
				WriteRequestedDisplayToken(writer, (DisplayToken)elementValue, trustConstants);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedProofToken))
			{
				RequestedProofToken requestedProofToken = (RequestedProofToken)elementValue;
				if (string.IsNullOrEmpty(requestedProofToken.ComputedKeyAlgorithm) && requestedProofToken.ProtectedKey == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3021")));
				}
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedProofToken, trustConstants.NamespaceURI);
				if (!string.IsNullOrEmpty(requestedProofToken.ComputedKeyAlgorithm))
				{
					WriteComputedKeyAlgorithm(writer, trustConstants.Elements.ComputedKey, requestedProofToken.ComputedKeyAlgorithm, trustConstants);
				}
				else
				{
					WriteProtectedKey(writer, requestedProofToken.ProtectedKey, context, trustConstants);
				}
				writer.WriteEndElement();
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedAttachedReference))
			{
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedAttachedReference, trustConstants.NamespaceURI);
				context.SecurityTokenSerializer.WriteKeyIdentifierClause(writer, (SecurityKeyIdentifierClause)elementValue);
				writer.WriteEndElement();
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedUnattachedReference))
			{
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedUnattachedReference, trustConstants.NamespaceURI);
				context.SecurityTokenSerializer.WriteKeyIdentifierClause(writer, (SecurityKeyIdentifierClause)elementValue);
				writer.WriteEndElement();
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.TokenType))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.TokenType, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.TokenType, trustConstants.NamespaceURI, (string)elementValue);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestType))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.RequestType, trustConstants.NamespaceURI, (string)elementValue)));
				}
				WriteRequestType(writer, (string)elementValue, trustConstants);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.KeyType))
			{
				WriteKeyType(writer, (string)elementValue, trustConstants);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.AuthenticationType))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.AuthenticationType, trustConstants.NamespaceURI, (string)elementValue);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.EncryptionAlgorithm))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.EncryptionAlgorithm, trustConstants.NamespaceURI, (string)elementValue);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.CanonicalizationAlgorithm))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.CanonicalizationAlgorithm, trustConstants.NamespaceURI, (string)elementValue);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.SignatureAlgorithm))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.SignatureAlgorithm, trustConstants.NamespaceURI, (string)elementValue);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.SignWith))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.SignWith, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.SignWith, trustConstants.NamespaceURI, (string)elementValue);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.EncryptWith))
			{
				if (!UriUtil.CanCreateValidUri((string)elementValue, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI, (string)elementValue)));
				}
				writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.EncryptWith, trustConstants.NamespaceURI, (string)elementValue);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.BinaryExchange))
			{
				WriteBinaryExchange(writer, elementValue as BinaryExchange, trustConstants);
			}
			else if (StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.Status))
			{
				WriteStatus(writer, elementValue as Status, trustConstants);
			}
			else
			{
				if (!StringComparer.Ordinal.Equals(elementName, trustConstants.Elements.RequestedTokenCancelled))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3013", elementName, elementValue.GetType())));
				}
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.RequestedTokenCancelled, trustConstants.NamespaceURI);
				writer.WriteEndElement();
			}
		}

		public static string ReadComputedKeyAlgorithm(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			string text = reader.ReadElementContentAsString();
			if (string.IsNullOrEmpty(text))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3006")));
			}
			if (!UriUtil.CanCreateValidUri(text, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.ComputedKeyAlgorithm, trustConstants.NamespaceURI, text)));
			}
			if (StringComparer.Ordinal.Equals(text, trustConstants.ComputedKeyAlgorithm.Psha1))
			{
				text = "http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1";
			}
			return text;
		}

		public static void WriteComputedKeyAlgorithm(XmlWriter writer, string elementName, string computedKeyAlgorithm, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (string.IsNullOrEmpty(computedKeyAlgorithm))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("computedKeyAlgorithm");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (!UriUtil.CanCreateValidUri(computedKeyAlgorithm, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", elementName, trustConstants.NamespaceURI, computedKeyAlgorithm)));
			}
			string text = ((!StringComparer.Ordinal.Equals(computedKeyAlgorithm, "http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1")) ? computedKeyAlgorithm : trustConstants.ComputedKeyAlgorithm.Psha1);
			if (!UriUtil.CanCreateValidUri(text, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", elementName, trustConstants.NamespaceURI, text)));
			}
			writer.WriteElementString(trustConstants.Prefix, elementName, trustConstants.NamespaceURI, text);
		}

		public static Status ReadStatus(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (!reader.IsStartElement(trustConstants.Elements.Status, trustConstants.NamespaceURI))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, trustConstants.Elements.Status, trustConstants.NamespaceURI)));
			}
			string reason = null;
			reader.ReadStartElement();
			if (!reader.IsStartElement(trustConstants.Elements.Code, trustConstants.NamespaceURI))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, trustConstants.Elements.Code, trustConstants.NamespaceURI)));
			}
			string code = reader.ReadElementContentAsString(trustConstants.Elements.Code, trustConstants.NamespaceURI);
			if (reader.IsStartElement(trustConstants.Elements.Reason, trustConstants.NamespaceURI))
			{
				reason = reader.ReadElementContentAsString(trustConstants.Elements.Reason, trustConstants.NamespaceURI);
			}
			reader.ReadEndElement();
			return new Status(code, reason);
		}

		public static BinaryExchange ReadBinaryExchange(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (!reader.IsStartElement(trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI)));
			}
			string attribute = reader.GetAttribute(trustConstants.Attributes.ValueType);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID0001", trustConstants.Attributes.ValueType, reader.Name)));
			}
			if (!UriUtil.TryCreateValidUri(attribute, UriKind.Absolute, out var result))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3136", trustConstants.Attributes.ValueType, reader.LocalName, reader.NamespaceURI, attribute)));
			}
			attribute = reader.GetAttribute(trustConstants.Attributes.EncodingType);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID0001", trustConstants.Attributes.EncodingType, reader.Name)));
			}
			if (!UriUtil.TryCreateValidUri(attribute, UriKind.Absolute, out var result2))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3136", trustConstants.Attributes.EncodingType, reader.LocalName, reader.NamespaceURI, attribute)));
			}
			return new BinaryExchange(result2.AbsoluteUri switch
			{
				"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary" => Convert.FromBase64String(reader.ReadElementContentAsString()), 
				"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary" => SoapHexBinary.Parse(reader.ReadElementContentAsString()).Value, 
				_ => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3215", result2, reader.LocalName, reader.NamespaceURI, string.Format("({0}, {1})", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary")))), 
			}, result, result2);
		}

		public static void WriteBinaryExchange(XmlWriter writer, BinaryExchange binaryExchange, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (binaryExchange == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("binaryExchange");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			string text;
			switch (binaryExchange.EncodingType.AbsoluteUri)
			{
			case "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary":
				text = Convert.ToBase64String(binaryExchange.BinaryData);
				break;
			case "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary":
			{
				SoapHexBinary soapHexBinary = new SoapHexBinary(binaryExchange.BinaryData);
				text = soapHexBinary.ToString();
				break;
			}
			default:
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3217", binaryExchange.EncodingType.AbsoluteUri, string.Format(CultureInfo.InvariantCulture, "({0}, {1})", new object[2] { "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary" }))));
			}
			writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.BinaryExchange, trustConstants.NamespaceURI);
			writer.WriteAttributeString(trustConstants.Attributes.ValueType, binaryExchange.ValueType.AbsoluteUri);
			writer.WriteAttributeString(trustConstants.Attributes.EncodingType, binaryExchange.EncodingType.AbsoluteUri);
			writer.WriteString(text);
			writer.WriteEndElement();
		}

		public static void WriteStatus(XmlWriter writer, Status status, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (status == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("status");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (status.Code == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("status code");
			}
			writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Status, trustConstants.NamespaceURI);
			writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Code, trustConstants.NamespaceURI);
			writer.WriteString(status.Code);
			writer.WriteEndElement();
			if (status.Reason != null)
			{
				writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Reason, trustConstants.NamespaceURI);
				writer.WriteString(status.Reason);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		public static ProtectedKey ReadProtectedKey(XmlReader reader, WSTrustSerializationContext context, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			ProtectedKey result = null;
			if (!reader.IsEmptyElement)
			{
				if (reader.IsStartElement(trustConstants.Elements.BinarySecret, trustConstants.NamespaceURI))
				{
					BinarySecretSecurityToken binarySecretSecurityToken = ReadBinarySecretSecurityToken(reader, trustConstants);
					byte[] keyBytes = binarySecretSecurityToken.GetKeyBytes();
					result = new ProtectedKey(keyBytes);
				}
				else if (context.SecurityTokenSerializer.CanReadKeyIdentifierClause(reader))
				{
					EncryptedKeyIdentifierClause encryptedKeyIdentifierClause = context.SecurityTokenSerializer.ReadKeyIdentifierClause(reader) as EncryptedKeyIdentifierClause;
					if (encryptedKeyIdentifierClause != null)
					{
						SecurityKey key = null;
						foreach (SecurityKeyIdentifierClause item in encryptedKeyIdentifierClause.EncryptingKeyIdentifier)
						{
							if (context.TokenResolver.TryResolveSecurityKey(item, out key))
							{
								break;
							}
						}
						if (key == null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3027", "the SecurityHeaderTokenResolver or OutOfBandTokenResolver")));
						}
						byte[] secret = key.DecryptKey(encryptedKeyIdentifierClause.EncryptionMethod, encryptedKeyIdentifierClause.GetEncryptedKey());
						Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials wrappingCredentials = new Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials(key, encryptedKeyIdentifierClause.EncryptingKeyIdentifier, encryptedKeyIdentifierClause.EncryptionMethod);
						result = new ProtectedKey(secret, wrappingCredentials);
					}
				}
			}
			return result;
		}

		public static void WriteProtectedKey(XmlWriter writer, ProtectedKey protectedKey, WSTrustSerializationContext context, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (protectedKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("protectedKey");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (protectedKey.WrappingCredentials != null)
			{
				byte[] encryptedKey = protectedKey.WrappingCredentials.SecurityKey.EncryptKey(protectedKey.WrappingCredentials.Algorithm, protectedKey.GetKeyBytes());
				EncryptedKeyIdentifierClause keyIdentifierClause = new EncryptedKeyIdentifierClause(encryptedKey, protectedKey.WrappingCredentials.Algorithm, protectedKey.WrappingCredentials.SecurityKeyIdentifier);
				context.SecurityTokenSerializer.WriteKeyIdentifierClause(writer, keyIdentifierClause);
			}
			else
			{
				BinarySecretSecurityToken token = new BinarySecretSecurityToken(protectedKey.GetKeyBytes());
				WriteBinarySecretSecurityToken(writer, token, trustConstants);
			}
		}

		public static string ReadRequestType(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			string text = reader.ReadElementContentAsString();
			if (!UriUtil.CanCreateValidUri(text, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.RequestType, trustConstants.NamespaceURI, text)));
			}
			if (trustConstants.RequestTypes.Issue.Equals(text))
			{
				return "http://schemas.microsoft.com/idfx/requesttype/issue";
			}
			if (trustConstants.RequestTypes.Cancel.Equals(text))
			{
				return "http://schemas.microsoft.com/idfx/requesttype/cancel";
			}
			if (trustConstants.RequestTypes.Renew.Equals(text))
			{
				return "http://schemas.microsoft.com/idfx/requesttype/renew";
			}
			if (trustConstants.RequestTypes.Validate.Equals(text))
			{
				return "http://schemas.microsoft.com/idfx/requesttype/validate";
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3011", text)));
		}

		public static void WriteRequestType(XmlWriter writer, string requestType, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (requestType == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestType");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			string value;
			if (StringComparer.Ordinal.Equals(requestType, "http://schemas.microsoft.com/idfx/requesttype/issue") || StringComparer.Ordinal.Equals(requestType, trustConstants.RequestTypes.Issue))
			{
				value = trustConstants.RequestTypes.Issue;
			}
			else if (StringComparer.Ordinal.Equals(requestType, "http://schemas.microsoft.com/idfx/requesttype/renew") || StringComparer.Ordinal.Equals(requestType, trustConstants.RequestTypes.Renew))
			{
				value = trustConstants.RequestTypes.Renew;
			}
			else if (StringComparer.Ordinal.Equals(requestType, "http://schemas.microsoft.com/idfx/requesttype/cancel") || StringComparer.Ordinal.Equals(requestType, trustConstants.RequestTypes.Cancel))
			{
				value = trustConstants.RequestTypes.Cancel;
			}
			else
			{
				if (!StringComparer.Ordinal.Equals(requestType, "http://schemas.microsoft.com/idfx/requesttype/validate") && !StringComparer.Ordinal.Equals(requestType, trustConstants.RequestTypes.Validate))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3011", requestType)));
				}
				value = trustConstants.RequestTypes.Validate;
			}
			writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.RequestType, trustConstants.NamespaceURI, value);
		}

		public static Lifetime ReadLifetime(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			DateTime? created = null;
			DateTime? expires = null;
			Lifetime lifetime = null;
			bool isEmptyElement = reader.IsEmptyElement;
			reader.ReadStartElement();
			if (!isEmptyElement)
			{
				if (reader.IsStartElement("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"))
				{
					reader.ReadStartElement("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
					created = DateTime.ParseExact(reader.ReadString(), DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
					reader.ReadEndElement();
				}
				if (reader.IsStartElement("Expires", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"))
				{
					reader.ReadStartElement("Expires", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
					expires = DateTime.ParseExact(reader.ReadString(), DateTimeFormats.Accepted, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).ToUniversalTime();
					reader.ReadEndElement();
				}
				reader.ReadEndElement();
				lifetime = new Lifetime(created, expires);
			}
			if (lifetime == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3161")));
			}
			return lifetime;
		}

		public static void WriteLifetime(XmlWriter writer, Lifetime lifetime, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (lifetime == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("lifetime");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Lifetime, trustConstants.NamespaceURI);
			if (lifetime.Created.HasValue)
			{
				writer.WriteElementString("wsu", "Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", lifetime.Created.Value.ToString(DateTimeFormats.Generated, CultureInfo.InvariantCulture));
			}
			if (lifetime.Expires.HasValue)
			{
				writer.WriteElementString("wsu", "Expires", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", lifetime.Expires.Value.ToString(DateTimeFormats.Generated, CultureInfo.InvariantCulture));
			}
			writer.WriteEndElement();
		}

		public static EndpointAddress ReadOnBehalfOfIssuer(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (!reader.IsStartElement(trustConstants.Elements.Issuer, trustConstants.NamespaceURI))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, trustConstants.Elements.Issuer, trustConstants.NamespaceURI)));
			}
			EndpointAddress endpointAddress = null;
			if (!reader.IsEmptyElement)
			{
				reader.ReadStartElement();
				endpointAddress = EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader));
				reader.ReadEndElement();
			}
			if (endpointAddress == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3216")));
			}
			return endpointAddress;
		}

		public static void WriteOnBehalfOfIssuer(XmlWriter writer, EndpointAddress issuer, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (issuer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("issuer");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			writer.WriteStartElement(trustConstants.Prefix, trustConstants.Elements.Issuer, trustConstants.NamespaceURI);
			issuer.WriteTo(AddressingVersion.WSAddressing10, writer);
			writer.WriteEndElement();
		}

		public static EndpointAddress ReadAppliesTo(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			EndpointAddress endpointAddress = null;
			if (!reader.IsEmptyElement)
			{
				reader.ReadStartElement();
				endpointAddress = EndpointAddress.ReadFrom(XmlDictionaryReader.CreateDictionaryReader(reader));
				reader.ReadEndElement();
			}
			if (endpointAddress == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3162")));
			}
			return endpointAddress;
		}

		public static void WriteAppliesTo(XmlWriter writer, EndpointAddress appliesTo, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (appliesTo == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("appliesTo");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			writer.WriteStartElement("wsp", "AppliesTo", "http://schemas.xmlsoap.org/ws/2004/09/policy");
			appliesTo.WriteTo(AddressingVersion.WSAddressing10, writer);
			writer.WriteEndElement();
		}

		public static string ReadKeyType(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			string text = reader.ReadElementContentAsString();
			if (!UriUtil.CanCreateValidUri(text, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.KeyType, trustConstants.NamespaceURI, text)));
			}
			if (trustConstants.KeyTypes.Symmetric.Equals(text))
			{
				return "http://schemas.microsoft.com/idfx/keytype/symmetric";
			}
			if (trustConstants.KeyTypes.Asymmetric.Equals(text))
			{
				return "http://schemas.microsoft.com/idfx/keytype/asymmetric";
			}
			if (trustConstants.KeyTypes.Bearer.Equals(text))
			{
				return "http://schemas.microsoft.com/idfx/keytype/bearer";
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3020", text)));
		}

		public static void WriteKeyType(XmlWriter writer, string keyType, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (string.IsNullOrEmpty(keyType))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("keyType");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			if (!UriUtil.CanCreateValidUri(keyType, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3135", trustConstants.Elements.KeyType, trustConstants.NamespaceURI, keyType)));
			}
			string value;
			if (StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/asymmetric") || StringComparer.Ordinal.Equals(keyType, trustConstants.KeyTypes.Asymmetric))
			{
				value = trustConstants.KeyTypes.Asymmetric;
			}
			else if (StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/symmetric") || StringComparer.Ordinal.Equals(keyType, trustConstants.KeyTypes.Symmetric))
			{
				value = trustConstants.KeyTypes.Symmetric;
			}
			else
			{
				if (!StringComparer.Ordinal.Equals(keyType, "http://schemas.microsoft.com/idfx/keytype/bearer") && !StringComparer.Ordinal.Equals(keyType, trustConstants.KeyTypes.Bearer))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3010", keyType)));
				}
				value = trustConstants.KeyTypes.Bearer;
			}
			writer.WriteElementString(trustConstants.Prefix, trustConstants.Elements.KeyType, trustConstants.NamespaceURI, value);
		}

		public static DisplayToken ReadRequestedDisplayToken(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			DisplayToken displayToken = null;
			if (!reader.IsEmptyElement)
			{
				reader.ReadStartElement("RequestedDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				if (!reader.IsStartElement("DisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3030", "DisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity")));
				}
				string text = null;
				while (reader.MoveToNextAttribute())
				{
					if (StringComparer.Ordinal.Equals("xml:lang", reader.Name))
					{
						text = reader.Value;
						break;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3031", "DisplayToken", "xml:lang")));
				}
				reader.Read();
				IList<DisplayClaim> list = new List<DisplayClaim>();
				while (reader.IsStartElement("DisplayClaim", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
				{
					bool isEmptyElement = reader.IsEmptyElement;
					string text2 = null;
					while (reader.MoveToNextAttribute())
					{
						if (StringComparer.Ordinal.Equals("Uri", reader.Name))
						{
							text2 = reader.Value;
							break;
						}
					}
					if (text2 == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3031", "DisplayClaim", "Uri")));
					}
					reader.Read();
					reader.MoveToContent();
					DisplayClaim displayClaim = new DisplayClaim(text2);
					if (!isEmptyElement)
					{
						if (reader.IsStartElement("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
						{
							displayClaim.DisplayTag = reader.ReadElementContentAsString("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity");
						}
						if (reader.IsStartElement("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
						{
							displayClaim.Description = reader.ReadElementContentAsString("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity");
						}
						if (reader.IsStartElement("DisplayValue", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
						{
							displayClaim.DisplayValue = reader.ReadElementContentAsString("DisplayValue", "http://schemas.xmlsoap.org/ws/2005/05/identity");
						}
						reader.ReadEndElement();
					}
					list.Add(displayClaim);
				}
				if (list.Count == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3030", "DisplayClaim", "http://schemas.xmlsoap.org/ws/2005/05/identity")));
				}
				reader.ReadEndElement();
				displayToken = new DisplayToken(text, list);
				reader.ReadEndElement();
			}
			if (displayToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3163")));
			}
			return displayToken;
		}

		public static void WriteRequestedDisplayToken(XmlWriter writer, DisplayToken requestedDisplayToken, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (requestedDisplayToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("requestedDisplayToken");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			writer.WriteStartElement("i", "RequestedDisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			writer.WriteStartElement("i", "DisplayToken", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			XmlUtil.WriteLanguageAttribute(writer, requestedDisplayToken.Language);
			for (int i = 0; i < requestedDisplayToken.DisplayClaims.Count; i++)
			{
				DisplayClaim displayClaim = requestedDisplayToken.DisplayClaims[i];
				writer.WriteStartElement("i", "DisplayClaim", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				writer.WriteAttributeString("Uri", displayClaim.ClaimType);
				if (!string.IsNullOrEmpty(displayClaim.DisplayTag))
				{
					writer.WriteElementString("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity", displayClaim.DisplayTag);
				}
				if (!string.IsNullOrEmpty(displayClaim.Description))
				{
					writer.WriteElementString("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity", displayClaim.Description);
				}
				if (!string.IsNullOrEmpty(displayClaim.DisplayValue))
				{
					writer.WriteElementString("DisplayValue", "http://schemas.xmlsoap.org/ws/2005/05/identity", displayClaim.DisplayValue);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		public static XmlElement ReadInnerXml(XmlReader reader)
		{
			return ReadInnerXml(reader, onStartElement: false);
		}

		public static XmlElement ReadInnerXml(XmlReader reader, bool onStartElement)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			string localName = reader.LocalName;
			string namespaceURI = reader.NamespaceURI;
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3061", localName, namespaceURI)));
			}
			if (!onStartElement)
			{
				reader.ReadStartElement();
			}
			reader.MoveToContent();
			XmlElement documentElement;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (XmlWriter xmlWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false))
				{
					xmlWriter.WriteNode(reader, defattr: true);
					xmlWriter.Flush();
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
				if (memoryStream.Length == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3061", localName, namespaceURI)));
				}
				XmlDictionaryReader reader2 = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8, XmlDictionaryReaderQuotas.Max, null);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.PreserveWhitespace = true;
				xmlDocument.Load(reader2);
				documentElement = xmlDocument.DocumentElement;
			}
			if (!onStartElement)
			{
				reader.ReadEndElement();
			}
			return documentElement;
		}

		public static BinarySecretSecurityToken ReadBinarySecretSecurityToken(XmlReader reader, WSTrustConstantsAdapter trustConstants)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			string text = reader.ReadElementContentAsString(trustConstants.Elements.BinarySecret, trustConstants.NamespaceURI);
			if (string.IsNullOrEmpty(text))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new WSTrustSerializationException(SR.GetString("ID3164")));
			}
			return new BinarySecretSecurityToken(Convert.FromBase64String(text));
		}

		public static void WriteBinarySecretSecurityToken(XmlWriter writer, BinarySecretSecurityToken token, WSTrustConstantsAdapter trustConstants)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if (trustConstants == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustConstants");
			}
			byte[] keyBytes = token.GetKeyBytes();
			writer.WriteStartElement(trustConstants.Elements.BinarySecret, trustConstants.NamespaceURI);
			writer.WriteBase64(keyBytes, 0, keyBytes.Length);
			writer.WriteEndElement();
		}

		private static string GetRequestClaimNamespace(string dialect)
		{
			if (StringComparer.Ordinal.Equals(dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims"))
			{
				return "http://docs.oasis-open.org/wsfed/authorization/200706";
			}
			return "http://schemas.xmlsoap.org/ws/2005/05/identity";
		}

		private static string GetRequestClaimPrefix(string dialect)
		{
			if (StringComparer.Ordinal.Equals(dialect, "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims"))
			{
				return "auth";
			}
			return "i";
		}
	}
}
