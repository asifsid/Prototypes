using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class InformationCardSerializer
	{
		private bool _allowUnknownElements = true;

		private SecurityTokenSerializer _tokenSerializer;

		private WSTrustConstantsAdapter _trustConstants = WSTrustConstantsAdapter.TrustFeb2005;

		public bool AllowUnknownElements
		{
			get
			{
				return _allowUnknownElements;
			}
			set
			{
				_allowUnknownElements = value;
			}
		}

		public SecurityTokenSerializer SecurityTokenSerializer => _tokenSerializer;

		public InformationCardSerializer()
			: this(new WSSecurityTokenSerializer(SecurityVersion.WSSecurity11, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, emitBspRequiredAttributes: false, null, null, null, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationOffset, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationLabelLength, WSSecurityTokenSerializer.DefaultInstance.MaximumKeyDerivationNonceLength))
		{
		}

		public InformationCardSerializer(SecurityTokenSerializer securityTokenSerializer)
		{
			if (securityTokenSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenSerializer");
			}
			_tokenSerializer = securityTokenSerializer;
		}

		public void WriteCard(Stream stream, InformationCard card)
		{
			if (stream == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("stream");
			}
			using XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, ownsStream: false);
			WriteCard(writer, card);
		}

		public void WriteCard(XmlWriter writer, InformationCard card)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (card == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("card");
			}
			XmlDictionaryWriter writer2 = ((card.SigningCredentials == null) ? XmlDictionaryWriter.CreateDictionaryWriter(writer) : new EnvelopingSignatureWriter(writer, card.SigningCredentials, "_Object_InformationCard", _tokenSerializer));
			WriteCardProperties(writer2, card);
			writer.Flush();
		}

		public InformationCard ReadCard(Stream stream)
		{
			if (stream == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("stream");
			}
			XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max);
			return ReadCard(reader);
		}

		public InformationCard ReadCard(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return ReadCard(reader, EmptySecurityTokenResolver.Instance);
		}

		public InformationCard ReadCard(XmlReader reader, SecurityTokenResolver signingTokenResolver)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (signingTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signingTokenResolver");
			}
			SigningCredentials signingCredentials = null;
			XmlDictionaryReader xmlDictionaryReader;
			if (reader.IsStartElement("Signature", "http://www.w3.org/2000/09/xmldsig#"))
			{
				xmlDictionaryReader = new EnvelopingSignatureReader(reader, _tokenSerializer, signingTokenResolver);
				signingCredentials = ((EnvelopingSignatureReader)xmlDictionaryReader).SigningCredentials;
			}
			else
			{
				xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			}
			return ReadCardProperties(xmlDictionaryReader, signingCredentials);
		}

		private static void VerifyPPIDValidBase64String(string ppid)
		{
			if (string.IsNullOrEmpty(ppid))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("ppid");
			}
			try
			{
				Convert.FromBase64String(ppid);
			}
			catch (FormatException inner)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3126", ppid), inner));
			}
		}

		private static T GetTypedValue<T>(object value, string contextString)
		{
			if (value == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
			}
			if (string.IsNullOrEmpty(contextString))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException("contextString"));
			}
			if (!(value is T))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3065", value.GetType(), contextString, typeof(T))));
			}
			return (T)value;
		}

		protected virtual InformationCard CreateCard(string issuer, SigningCredentials signingCredentials)
		{
			if (issuer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("issuer");
			}
			if (!UriUtil.CanCreateValidUri(issuer, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3042", issuer)));
			}
			if (signingCredentials != null)
			{
				return new InformationCard(signingCredentials, issuer);
			}
			return new InformationCard(issuer);
		}

		protected virtual void ReadAdditionalElements(XmlDictionaryReader reader, InformationCard card)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			while (reader.IsStartElement())
			{
				if (AllowUnknownElements && !StringComparer.Ordinal.Equals(reader.NamespaceURI, "http://schemas.xmlsoap.org/ws/2005/05/identity"))
				{
					reader.Skip();
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3208", reader.Name)));
			}
		}

		protected virtual PrivacyNotice ReadPrivacyNotice(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			PrivacyNotice result = null;
			if (reader.IsStartElement("PrivacyNotice", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				string attribute = reader.GetAttribute("Version", null);
				long num = 1L;
				if (!string.IsNullOrEmpty(attribute))
				{
					num = long.Parse(attribute, CultureInfo.InvariantCulture);
					if (num < 1 || num > uint.MaxValue)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3054", 4294967295L)));
					}
				}
				reader.Read();
				reader.MoveToContent();
				string text = reader.ReadString();
				if (!UriUtil.CanCreateValidUri(text, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3070", text)));
				}
				reader.MoveToContent();
				reader.ReadEndElement();
				result = new PrivacyNotice(text, num);
			}
			return result;
		}

		protected virtual DisplayClaimCollection ReadSupportedClaimList(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			DisplayClaimCollection displayClaimCollection = new DisplayClaimCollection();
			if (reader.IsStartElement("SupportedClaimTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				bool isEmptyElement = reader.IsEmptyElement;
				reader.ReadStartElement("SupportedClaimTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				while (reader.IsStartElement("SupportedClaimType", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
				{
					if (displayClaimCollection.Count == 128)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3050", 128)));
					}
					string attribute = reader.GetAttribute("Uri", null);
					if (!UriUtil.CanCreateValidUri(attribute, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3051", attribute)));
					}
					DisplayClaim displayClaim = new DisplayClaim(attribute);
					bool isEmptyElement2 = reader.IsEmptyElement;
					reader.Read();
					reader.MoveToContent();
					if (!isEmptyElement2)
					{
						if (reader.IsStartElement("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
						{
							string text = reader.ReadElementContentAsString("DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity");
							if (text.Length < 1 || text.Length > 255)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3052", text.Length)));
							}
							displayClaim.DisplayTag = text;
						}
						if (reader.IsStartElement("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
						{
							string text2 = reader.ReadElementContentAsString("Description", "http://schemas.xmlsoap.org/ws/2005/05/identity");
							if (text2.Length < 1 || text2.Length > 255)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3053", text2.Length)));
							}
							displayClaim.Description = text2;
						}
						reader.ReadEndElement();
					}
					displayClaimCollection.Add(displayClaim);
				}
				if (!isEmptyElement)
				{
					reader.ReadEndElement();
				}
			}
			return displayClaimCollection;
		}

		protected virtual TokenServiceCollection ReadTokenServiceList(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			TokenServiceCollection tokenServiceCollection = new TokenServiceCollection();
			if (reader.IsStartElement("TokenServiceList", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3176")));
				}
				reader.ReadStartElement();
				int num = 0;
				while (reader.IsStartElement("TokenService", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
				{
					num++;
					if (num > 128)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3045", 128)));
					}
					if (reader.IsEmptyElement)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3178")));
					}
					reader.ReadStartElement("TokenService", "http://schemas.xmlsoap.org/ws/2005/05/identity");
					EndpointAddress endpointAddress = EndpointAddress.ReadFrom(reader);
					if (!reader.IsStartElement("UserCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3177")));
					}
					reader.ReadStartElement("UserCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
					string text = null;
					if (reader.IsStartElement("DisplayCredentialHint", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
					{
						text = reader.ReadElementContentAsString("DisplayCredentialHint", "http://schemas.xmlsoap.org/ws/2005/05/identity");
						if (text.Length <= 0 || text.Length > 64)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3068", text)));
						}
					}
					IUserCredential userCredential = ReadUserCredential(reader);
					reader.ReadEndElement();
					reader.ReadEndElement();
					tokenServiceCollection.Add(new TokenService(endpointAddress, userCredential, text));
				}
				if (num == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3067")));
				}
				reader.ReadEndElement();
			}
			return tokenServiceCollection;
		}

		protected virtual IUserCredential ReadUnrecognizedUserCredential(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3046", reader.LocalName, reader.NamespaceURI)));
		}

		protected virtual IUserCredential ReadX509UserCredential(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("X509V3Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				reader.ReadStartElement("X509V3Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3181")));
			}
			reader.ReadStartElement();
			if (!reader.IsStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#"))
			{
				reader.ReadStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#");
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3186")));
			}
			reader.ReadStartElement();
			IUserCredential result;
			if (reader.IsStartElement("X509Principal", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				X509Principal x509Principal = ReadX509Principal(reader);
				result = new X509CertificateCredential(x509Principal);
			}
			else if (reader.IsStartElement("X509SubjectAndIssuer", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				X509SubjectAndIssuer x509SubjectAndIssuer = ReadX509SubjectAndIssuer(reader);
				result = new X509CertificateCredential(x509SubjectAndIssuer);
			}
			else if (reader.IsStartElement("X509SubjectName", "http://www.w3.org/2000/09/xmldsig#"))
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3279")));
				}
				string x509SubjectName = reader.ReadElementString("X509SubjectName", "http://www.w3.org/2000/09/xmldsig#");
				result = new X509CertificateCredential(x509SubjectName);
			}
			else
			{
				SecurityKeyIdentifierClause x509IdentifierClause = ReadX509SecurityKeyIdentifierClause(reader);
				result = new X509CertificateCredential(x509IdentifierClause);
			}
			reader.ReadEndElement();
			reader.ReadEndElement();
			return result;
		}

		protected virtual void WriteAdditionalElements(XmlDictionaryWriter writer, InformationCard card)
		{
		}

		protected virtual void WritePrivacyNotice(XmlDictionaryWriter writer, InformationCard card)
		{
			if (card.PrivacyNotice != null)
			{
				if (card.PrivacyNotice.Version < 1 || card.PrivacyNotice.Version > uint.MaxValue)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3054", 4294967295L)));
				}
				if (!UriUtil.CanCreateValidUri(card.PrivacyNotice.Location, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3070", card.PrivacyNotice.Location)));
				}
				writer.WriteStartElement("i", "PrivacyNotice", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				writer.WriteAttributeString("Version", Convert.ToString(card.PrivacyNotice.Version, CultureInfo.InvariantCulture));
				writer.WriteString(card.PrivacyNotice.Location);
				writer.WriteEndElement();
			}
		}

		protected virtual void WriteSupportedClaimTypeList(XmlDictionaryWriter writer, InformationCard card)
		{
			if (card.SupportedClaimTypeList.Count > 128)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3050", 128)));
			}
			if (card.SupportedClaimTypeList.Count <= 0)
			{
				return;
			}
			writer.WriteStartElement("i", "SupportedClaimTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			for (int i = 0; i < card.SupportedClaimTypeList.Count; i++)
			{
				DisplayClaim displayClaim = card.SupportedClaimTypeList[i];
				if (!UriUtil.CanCreateValidUri(displayClaim.ClaimType, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3051", displayClaim.ClaimType)));
				}
				writer.WriteStartElement("i", "SupportedClaimType", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				writer.WriteAttributeString("Uri", displayClaim.ClaimType);
				if (!string.IsNullOrEmpty(displayClaim.DisplayTag))
				{
					if (displayClaim.DisplayTag.Length < 1 || displayClaim.DisplayTag.Length > 255)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3052", displayClaim.DisplayTag.Length)));
					}
					writer.WriteElementString("i", "DisplayTag", "http://schemas.xmlsoap.org/ws/2005/05/identity", displayClaim.DisplayTag);
				}
				if (!string.IsNullOrEmpty(displayClaim.Description))
				{
					if (displayClaim.Description.Length < 1 || displayClaim.Description.Length > 255)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3053", displayClaim.Description.Length)));
					}
					writer.WriteElementString("i", "Description", "http://schemas.xmlsoap.org/ws/2005/05/identity", displayClaim.Description);
				}
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		protected virtual void WriteUnrecognizedUserCredential(XmlDictionaryWriter writer, IUserCredential credential)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3062")));
		}

		protected virtual void WriteX509UserCredential(XmlDictionaryWriter writer, IUserCredential credential)
		{
			writer.WriteStartElement("i", "X509V3Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			X509CertificateCredential typedValue = GetTypedValue<X509CertificateCredential>(credential, "X509V3Credential");
			writer.WriteStartElement("ds", "X509Data", "http://www.w3.org/2000/09/xmldsig#");
			if (typedValue.X509SecurityTokenIdentifierClause != null)
			{
				WriteX509SecurityKeyIdentifierClause(writer, typedValue.X509SecurityTokenIdentifierClause);
			}
			else if (!string.IsNullOrEmpty(typedValue.X509SubjectName))
			{
				writer.WriteElementString("ds", "X509SubjectName", "http://www.w3.org/2000/09/xmldsig#", typedValue.X509SubjectName);
			}
			else if (typedValue.X509Principal != null)
			{
				WriteX509Principal(writer, typedValue.X509Principal);
			}
			else if (typedValue.X509SubjectAndIssuer != null)
			{
				WriteX509SubjectAndIssuer(writer, typedValue.X509SubjectAndIssuer);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		protected virtual void WriteTokenServiceList(XmlDictionaryWriter writer, InformationCard card)
		{
			if (card.TokenServiceList.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3067")));
			}
			if (card.TokenServiceList.Count > 128)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3045", 128)));
			}
			writer.WriteStartElement("i", "TokenServiceList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			for (int i = 0; i < card.TokenServiceList.Count; i++)
			{
				TokenService tokenService = card.TokenServiceList[i];
				writer.WriteStartElement("i", "TokenService", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				tokenService.Address.WriteTo(AddressingVersion.WSAddressing10, writer);
				writer.WriteStartElement("i", "UserCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				if (!string.IsNullOrEmpty(tokenService.DisplayCredentialHint))
				{
					if (tokenService.DisplayCredentialHint.Length > 64)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3068", tokenService.DisplayCredentialHint)));
					}
					writer.WriteElementString("i", "DisplayCredentialHint", "http://schemas.xmlsoap.org/ws/2005/05/identity", tokenService.DisplayCredentialHint);
				}
				WriteUserCredential(writer, tokenService.UserCredential);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		private void WriteCardProperties(XmlDictionaryWriter writer, InformationCard card)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (card == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("card");
			}
			writer.WriteStartElement("i", "InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			if (string.IsNullOrEmpty(card.Language))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3029", "xml:lang")));
			}
			XmlUtil.WriteLanguageAttribute(writer, card.Language);
			if (card.InformationCardReference != null)
			{
				writer.WriteStartElement("i", "InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				if (!UriUtil.CanCreateValidUri(card.InformationCardReference.CardId, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3040")));
				}
				writer.WriteElementString("i", "CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.InformationCardReference.CardId);
				if (card.InformationCardReference.CardVersion < 1 || card.InformationCardReference.CardVersion > uint.MaxValue)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3066", 4294967295L)));
				}
				writer.WriteElementString("i", "CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity", Convert.ToString(card.InformationCardReference.CardVersion, CultureInfo.InvariantCulture));
				writer.WriteEndElement();
				if (!string.IsNullOrEmpty(card.CardName))
				{
					if (card.CardName.Length < 1 || card.CardName.Length > 255)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3041")));
					}
					writer.WriteElementString("i", "CardName", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.CardName);
				}
				if (card.CardImage != null)
				{
					if (!CardImage.IsValidMimeType(card.CardImage.MimeType))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3043", card.CardImage.MimeType)));
					}
					if (card.CardImage.GetImage() == null || card.CardImage.GetImage().Length == 0)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3044")));
					}
					string text = Convert.ToBase64String(card.CardImage.GetImage());
					if (text.Length > 1048576)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3123", 1048576)));
					}
					writer.WriteStartElement("i", "CardImage", "http://schemas.xmlsoap.org/ws/2005/05/identity");
					writer.WriteAttributeString("MimeType", card.CardImage.MimeType);
					writer.WriteString(text);
					writer.WriteEndElement();
				}
				if (card.Issuer != null)
				{
					if (!UriUtil.CanCreateValidUri(card.Issuer, UriKind.Absolute))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3042", card.Issuer)));
					}
					writer.WriteElementString("i", "Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.Issuer);
					if (card.TimeIssued.HasValue)
					{
						writer.WriteElementString("i", "TimeIssued", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.TimeIssued.Value.ToString(DateTimeFormats.Generated, CultureInfo.InvariantCulture));
						if (card.TimeExpires.HasValue)
						{
							if (card.TimeIssued.Value.CompareTo(card.TimeExpires.Value) > 0)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3125")));
							}
							writer.WriteElementString("i", "TimeExpires", "http://schemas.xmlsoap.org/ws/2005/05/identity", card.TimeExpires.Value.ToString(DateTimeFormats.Generated, CultureInfo.InvariantCulture));
						}
						WriteTokenServiceList(writer, card);
						if (card.SupportedTokenTypeList.Count == 0)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3047")));
						}
						if (card.SupportedTokenTypeList.Count > 32)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3049", 32)));
						}
						writer.WriteStartElement("i", "SupportedTokenTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
						for (int i = 0; i < card.SupportedTokenTypeList.Count; i++)
						{
							string text2 = card.SupportedTokenTypeList[i];
							if (!UriUtil.CanCreateValidUri(text2, UriKind.Absolute))
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3209", text2)));
							}
							writer.WriteElementString(_trustConstants.Prefix, _trustConstants.Elements.TokenType, _trustConstants.NamespaceURI, text2);
						}
						writer.WriteEndElement();
						WriteSupportedClaimTypeList(writer, card);
						AppliesToOption? appliesToOption = card.AppliesToOption;
						if (appliesToOption.GetValueOrDefault() != 0 || !appliesToOption.HasValue)
						{
							writer.WriteStartElement("i", "RequireAppliesTo", "http://schemas.xmlsoap.org/ws/2005/05/identity");
							writer.WriteAttributeString("Optional", (card.AppliesToOption == AppliesToOption.Optional) ? "true" : "false");
							writer.WriteEndElement();
						}
						WritePrivacyNotice(writer, card);
						if (card.IssuerInformation.Count > 0)
						{
							writer.WriteStartElement("ic07", "IssuerInformation", "http://schemas.xmlsoap.org/ws/2007/01/identity");
							foreach (IssuerInformation item in card.IssuerInformation)
							{
								if (item.Key.Length < 1 || item.Key.Length > 255)
								{
									throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3115", item.Key, 1, 255)));
								}
								if (item.Value.Length < 1 || item.Value.Length > 255)
								{
									throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3116", item.Value, 1, 255)));
								}
								writer.WriteStartElement("ic07", "IssuerInformationEntry", "http://schemas.xmlsoap.org/ws/2007/01/identity");
								writer.WriteElementString("ic07", "EntryName", "http://schemas.xmlsoap.org/ws/2007/01/identity", item.Key);
								writer.WriteElementString("ic07", "EntryValue", "http://schemas.xmlsoap.org/ws/2007/01/identity", item.Value);
								writer.WriteEndElement();
							}
							writer.WriteEndElement();
						}
						if (card.RequireStrongRecipientIdentity.HasValue && card.RequireStrongRecipientIdentity.Value)
						{
							writer.WriteStartElement("ic07", "RequireStrongRecipientIdentity", "http://schemas.xmlsoap.org/ws/2007/01/identity");
							writer.WriteEndElement();
						}
						if (card.CardType != null)
						{
							writer.WriteElementString("ic09", "CardType", "http://docs.oasis-open.org/imi/ns/identity-200903", card.CardType.OriginalString);
						}
						if (!string.IsNullOrEmpty(card.IssuerName))
						{
							if (card.IssuerName.Length < 1 || card.IssuerName.Length > 64)
							{
								throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3259")));
							}
							writer.WriteElementString("ic09", "IssuerName", "http://docs.oasis-open.org/imi/ns/identity-200903", card.IssuerName);
						}
						WriteAdditionalElements(writer, card);
						writer.WriteEndElement();
						return;
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3029", "TimeIssued")));
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3029", "Issuer")));
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3029", "InformationCardReference")));
		}

		private void WriteEkuPolicy(XmlDictionaryWriter writer, EkuPolicy ekuPolicy)
		{
			if (ekuPolicy == null)
			{
				return;
			}
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			writer.WriteStartElement("ic09", "EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903");
			foreach (Oid oid in ekuPolicy.Oids)
			{
				writer.WriteElementString("ic09", "OID", "http://docs.oasis-open.org/imi/ns/identity-200903", oid.Value);
			}
			writer.WriteEndElement();
		}

		private void WriteUserCredential(XmlDictionaryWriter writer, IUserCredential credential)
		{
			switch (credential.CredentialType)
			{
			case UserCredentialType.KerberosV5Credential:
				writer.WriteStartElement("i", "KerberosV5Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				writer.WriteEndElement();
				break;
			case UserCredentialType.SelfIssuedCredential:
			{
				string pPID = ((SelfIssuedCredentials)credential).PPID;
				if (string.IsNullOrEmpty(pPID))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3129")));
				}
				VerifyPPIDValidBase64String(pPID);
				if (pPID.Length > 1024)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3127", pPID, 1024)));
				}
				writer.WriteStartElement("i", "SelfIssuedCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				writer.WriteElementString("i", "PrivatePersonalIdentifier", "http://schemas.xmlsoap.org/ws/2005/05/identity", pPID);
				writer.WriteEndElement();
				break;
			}
			case UserCredentialType.UserNamePasswordCredential:
			{
				writer.WriteStartElement("i", "UsernamePasswordCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				UserNamePasswordCredential typedValue = GetTypedValue<UserNamePasswordCredential>(credential, "UsernamePasswordCredential");
				if (!string.IsNullOrEmpty(typedValue.UserName))
				{
					if (typedValue.UserName.Length > 255)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3069", typedValue.UserName)));
					}
					writer.WriteElementString("i", "Username", "http://schemas.xmlsoap.org/ws/2005/05/identity", typedValue.UserName);
				}
				writer.WriteEndElement();
				break;
			}
			case UserCredentialType.X509V3Credential:
				WriteX509UserCredential(writer, credential);
				break;
			default:
				WriteUnrecognizedUserCredential(writer, credential);
				break;
			}
		}

		private void WriteX509Principal(XmlDictionaryWriter writer, X509Principal x509Principal)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (x509Principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("x509Principal");
			}
			writer.WriteStartElement("ic09", "X509Principal", "http://docs.oasis-open.org/imi/ns/identity-200903");
			writer.WriteElementString("ic09", "PrincipalName", "http://docs.oasis-open.org/imi/ns/identity-200903", x509Principal.PrincipalName);
			WriteEkuPolicy(writer, x509Principal.EkuPolicy);
			writer.WriteEndElement();
		}

		private void WriteX509SecurityKeyIdentifierClause(XmlDictionaryWriter writer, SecurityKeyIdentifierClause keyIdentifierClause)
		{
			X509ThumbprintKeyIdentifierClause x509ThumbprintKeyIdentifierClause = keyIdentifierClause as X509ThumbprintKeyIdentifierClause;
			if (x509ThumbprintKeyIdentifierClause != null)
			{
				writer.WriteStartElement("wsse", "KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
				writer.WriteAttributeString("ValueType", "http://docs.oasis-open.org/wss/2004/xx/oasis-2004xx-wss-soap-message-security-1.1#ThumbprintSHA1");
				writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
				byte[] x509Thumbprint = x509ThumbprintKeyIdentifierClause.GetX509Thumbprint();
				writer.WriteBase64(x509Thumbprint, 0, x509Thumbprint.Length);
				writer.WriteEndElement();
				return;
			}
			X509IssuerSerialKeyIdentifierClause x509IssuerSerialKeyIdentifierClause = keyIdentifierClause as X509IssuerSerialKeyIdentifierClause;
			if (x509IssuerSerialKeyIdentifierClause != null)
			{
				writer.WriteStartElement("ds", "X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
				writer.WriteElementString("ds", "X509IssuerName", "http://www.w3.org/2000/09/xmldsig#", x509IssuerSerialKeyIdentifierClause.IssuerName);
				writer.WriteElementString("ds", "X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#", x509IssuerSerialKeyIdentifierClause.IssuerSerialNumber);
				writer.WriteEndElement();
				return;
			}
			X509RawDataKeyIdentifierClause x509RawDataKeyIdentifierClause = keyIdentifierClause as X509RawDataKeyIdentifierClause;
			if (x509RawDataKeyIdentifierClause != null)
			{
				string value = Convert.ToBase64String(x509RawDataKeyIdentifierClause.GetX509RawData());
				writer.WriteElementString("ds", "X509Certificate", "http://www.w3.org/2000/09/xmldsig#", value);
				return;
			}
			X509SubjectKeyIdentifierClause x509SubjectKeyIdentifierClause = keyIdentifierClause as X509SubjectKeyIdentifierClause;
			if (x509SubjectKeyIdentifierClause != null)
			{
				string value2 = Convert.ToBase64String(x509SubjectKeyIdentifierClause.GetX509SubjectKeyIdentifier());
				writer.WriteElementString("ds", "X509SKI", "http://www.w3.org/2000/09/xmldsig#", value2);
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3060", keyIdentifierClause.GetType())));
		}

		private void WriteX509SubjectAndIssuer(XmlDictionaryWriter writer, X509SubjectAndIssuer x509SubjectAndIssuer)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (x509SubjectAndIssuer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("x509SubjectAndIssuer");
			}
			writer.WriteStartElement("ic09", "X509SubjectAndIssuer", "http://docs.oasis-open.org/imi/ns/identity-200903");
			writer.WriteElementString("ic09", "X509Subject", "http://docs.oasis-open.org/imi/ns/identity-200903", x509SubjectAndIssuer.X509Subject);
			writer.WriteElementString("ic09", "X509Issuer", "http://docs.oasis-open.org/imi/ns/identity-200903", x509SubjectAndIssuer.X509Issuer);
			WriteEkuPolicy(writer, x509SubjectAndIssuer.EkuPolicy);
			writer.WriteEndElement();
		}

		private InformationCard ReadCardProperties(XmlDictionaryReader reader, SigningCredentials signingCredentials)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3128", "InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity")));
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3171", reader.LocalName, reader.NamespaceURI)));
			}
			reader.MoveToStartElement("InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			string attribute = reader.GetAttribute("xml:lang");
			reader.Read();
			if (!reader.IsStartElement("InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3172")));
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3173")));
			}
			reader.ReadStartElement("InformationCardReference", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			if (!reader.IsStartElement("CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3174")));
			}
			string text = reader.ReadElementString("CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			if (!UriUtil.CanCreateValidUri(text, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3040")));
			}
			if (!reader.IsStartElement("CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3175")));
			}
			long num = Convert.ToInt64(reader.ReadElementString("CardVersion", "http://schemas.xmlsoap.org/ws/2005/05/identity"), CultureInfo.InvariantCulture);
			if (num < 1 || num > uint.MaxValue)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3262", 4294967295L)));
			}
			reader.ReadEndElement();
			string text2 = null;
			if (reader.IsStartElement("CardName", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				text2 = reader.ReadElementString("CardName", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				if (string.IsNullOrEmpty(text2) || text2.Length > 255)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3041")));
				}
			}
			CardImage cardImage = null;
			if (reader.IsStartElement("CardImage", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				string attribute2 = reader.GetAttribute("MimeType", null);
				if (!CardImage.IsValidMimeType(attribute2))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3043", attribute2)));
				}
				string text3 = reader.ReadElementContentAsString();
				if (string.IsNullOrEmpty(text3))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3044")));
				}
				byte[] array = Convert.FromBase64String(text3);
				if (array.Length > 1048576)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3123", 1048576)));
				}
				cardImage = new CardImage(array, attribute2);
			}
			if (!reader.IsStartElement("Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3128", "Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity")));
			}
			string issuer = reader.ReadElementString("Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			InformationCard informationCard = CreateCard(issuer, signingCredentials);
			informationCard.InformationCardReference = new InformationCardReference(text, num);
			informationCard.CardName = text2;
			informationCard.CardImage = cardImage;
			informationCard.Language = attribute;
			if (!reader.IsStartElement("TimeIssued", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3128", "TimeIssued", "http://schemas.xmlsoap.org/ws/2005/05/identity")));
			}
			informationCard.TimeIssued = XmlConvert.ToDateTime(reader.ReadElementString("TimeIssued", "http://schemas.xmlsoap.org/ws/2005/05/identity"), XmlDateTimeSerializationMode.Utc);
			if (reader.IsStartElement("TimeExpires", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				informationCard.TimeExpires = XmlConvert.ToDateTime(reader.ReadElementString("TimeExpires", "http://schemas.xmlsoap.org/ws/2005/05/identity"), XmlDateTimeSerializationMode.Utc);
				if (informationCard.TimeIssued.Value.CompareTo(informationCard.TimeExpires.Value) > 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3125")));
				}
			}
			TokenServiceCollection tokenServiceCollection = ReadTokenServiceList(reader);
			if (tokenServiceCollection != null)
			{
				foreach (TokenService item in tokenServiceCollection)
				{
					informationCard.TokenServiceList.Add(item);
				}
			}
			if (!reader.IsStartElement("SupportedTokenTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3128", "SupportedTokenTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity")));
			}
			reader.ReadStartElement("SupportedTokenTypeList", "http://schemas.xmlsoap.org/ws/2005/05/identity");
			while (reader.IsStartElement(_trustConstants.Elements.TokenType, _trustConstants.NamespaceURI))
			{
				if (informationCard.SupportedTokenTypeList.Count == 32)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3049", 32)));
				}
				string text4 = reader.ReadElementContentAsString(_trustConstants.Elements.TokenType, _trustConstants.NamespaceURI);
				if (!UriUtil.CanCreateValidUri(text4, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3135", _trustConstants.Elements.TokenType, _trustConstants.NamespaceURI, text4)));
				}
				informationCard.SupportedTokenTypeList.Add(text4);
			}
			if (informationCard.SupportedTokenTypeList.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3047")));
			}
			reader.ReadEndElement();
			DisplayClaimCollection displayClaimCollection = ReadSupportedClaimList(reader);
			if (displayClaimCollection != null)
			{
				foreach (DisplayClaim item2 in displayClaimCollection)
				{
					informationCard.SupportedClaimTypeList.Add(item2);
				}
			}
			if (reader.IsStartElement("RequireAppliesTo", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				bool isEmptyElement = reader.IsEmptyElement;
				string attribute3 = reader.GetAttribute("Optional", null);
				if (!string.IsNullOrEmpty(attribute3))
				{
					informationCard.AppliesToOption = ((!XmlConvert.ToBoolean(attribute3)) ? AppliesToOption.Required : AppliesToOption.Optional);
				}
				else
				{
					informationCard.AppliesToOption = AppliesToOption.Required;
				}
				reader.Read();
				reader.MoveToContent();
				if (!isEmptyElement)
				{
					reader.ReadEndElement();
				}
			}
			else
			{
				informationCard.AppliesToOption = AppliesToOption.NotAllowed;
			}
			informationCard.PrivacyNotice = ReadPrivacyNotice(reader);
			if (reader.IsStartElement("IssuerInformation", "http://schemas.xmlsoap.org/ws/2007/01/identity"))
			{
				reader.ReadStartElement();
				int num2 = 0;
				while (reader.IsStartElement("IssuerInformationEntry", "http://schemas.xmlsoap.org/ws/2007/01/identity"))
				{
					num2++;
					reader.ReadStartElement("IssuerInformationEntry", "http://schemas.xmlsoap.org/ws/2007/01/identity");
					if (!reader.IsStartElement("EntryName", "http://schemas.xmlsoap.org/ws/2007/01/identity"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3189", "IssuerInformationEntry", "EntryName")));
					}
					string text5 = reader.ReadElementContentAsString("EntryName", "http://schemas.xmlsoap.org/ws/2007/01/identity");
					if (text5.Length < 1 || text5.Length > 255)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3115", text5, 1, 255)));
					}
					if (!reader.IsStartElement("EntryValue", "http://schemas.xmlsoap.org/ws/2007/01/identity"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3189", "IssuerInformationEntry", "EntryValue")));
					}
					string text6 = reader.ReadElementContentAsString("EntryValue", "http://schemas.xmlsoap.org/ws/2007/01/identity");
					if (text6.Length < 1 || text6.Length > 255)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3116", text6, 1, 255)));
					}
					reader.ReadEndElement();
					informationCard.IssuerInformation.Add(new IssuerInformation(text5, text6));
				}
				if (num2 == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3117")));
				}
				reader.ReadEndElement();
			}
			if (reader.IsStartElement("RequireStrongRecipientIdentity", "http://schemas.xmlsoap.org/ws/2007/01/identity"))
			{
				bool isEmptyElement2 = reader.IsEmptyElement;
				reader.ReadStartElement();
				informationCard.RequireStrongRecipientIdentity = true;
				if (!isEmptyElement2)
				{
					reader.ReadEndElement();
				}
			}
			if (reader.IsStartElement("CardType", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				string text7 = reader.ReadElementContentAsString();
				if (!UriUtil.CanCreateValidUri(text7, UriKind.Absolute))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3185", text7)));
				}
				informationCard.CardType = new Uri(text7);
			}
			if (reader.IsStartElement("IssuerName", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				string text8 = reader.ReadElementString();
				if (string.IsNullOrEmpty(text8) || text8.Length < 1 || text8.Length > 64)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3259")));
				}
				informationCard.IssuerName = text8;
			}
			ReadAdditionalElements(reader, informationCard);
			reader.ReadEndElement();
			return informationCard;
		}

		private EkuPolicy ReadEkuPolicy(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			Collection<Oid> collection = new Collection<Oid>();
			if (!reader.IsStartElement("EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				reader.ReadStartElement("EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903");
			}
			if (reader.IsEmptyElement)
			{
				reader.Skip();
				return new EkuPolicy();
			}
			reader.ReadStartElement();
			while (reader.IsStartElement())
			{
				string oid = reader.ReadElementContentAsString("OID", "http://docs.oasis-open.org/imi/ns/identity-200903");
				collection.Add(new Oid(oid));
			}
			reader.ReadEndElement();
			return new EkuPolicy(collection);
		}

		private IUserCredential ReadUserCredential(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			IUserCredential userCredential = null;
			if (reader.IsStartElement("SelfIssuedCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3179")));
				}
				reader.ReadStartElement();
				if (!reader.IsStartElement("PrivatePersonalIdentifier", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3180")));
				}
				string text = reader.ReadElementString("PrivatePersonalIdentifier", "http://schemas.xmlsoap.org/ws/2005/05/identity");
				if (string.IsNullOrEmpty(text))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3129")));
				}
				VerifyPPIDValidBase64String(text);
				if (text.Length > 1024)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3127", text, 1024)));
				}
				reader.ReadEndElement();
				userCredential = new SelfIssuedCredentials(text);
			}
			else if (reader.IsStartElement("X509V3Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				userCredential = ReadX509UserCredential(reader);
			}
			else if (reader.IsStartElement("KerberosV5Credential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				bool isEmptyElement = reader.IsEmptyElement;
				reader.Read();
				reader.MoveToContent();
				if (!isEmptyElement)
				{
					reader.ReadEndElement();
				}
				userCredential = TokenService.DefaultUserCredential;
			}
			else if (reader.IsStartElement("UsernamePasswordCredential", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
			{
				bool isEmptyElement2 = reader.IsEmptyElement;
				reader.Read();
				if (!isEmptyElement2)
				{
					if (reader.IsStartElement("Username", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
					{
						string text2 = reader.ReadElementContentAsString("Username", "http://schemas.xmlsoap.org/ws/2005/05/identity");
						if (string.IsNullOrEmpty(text2))
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3124")));
						}
						if (text2.Length > 255)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3069", text2)));
						}
						userCredential = new UserNamePasswordCredential(text2);
					}
					reader.ReadEndElement();
				}
				if (userCredential == null)
				{
					userCredential = new UserNamePasswordCredential();
				}
			}
			else
			{
				userCredential = ReadUnrecognizedUserCredential(reader);
			}
			return userCredential;
		}

		private X509Principal ReadX509Principal(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("X509Principal", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				reader.ReadStartElement("X509Principal", "http://docs.oasis-open.org/imi/ns/identity-200903");
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3171", reader.LocalName, reader.NamespaceURI)));
			}
			reader.ReadStartElement();
			string principalName = reader.ReadElementContentAsString("PrincipalName", "http://docs.oasis-open.org/imi/ns/identity-200903");
			EkuPolicy ekuPolicy = null;
			if (reader.IsStartElement("EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				ekuPolicy = ReadEkuPolicy(reader);
			}
			reader.ReadEndElement();
			return new X509Principal(principalName, ekuPolicy);
		}

		private SecurityKeyIdentifierClause ReadX509SecurityKeyIdentifierClause(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			SecurityKeyIdentifierClause securityKeyIdentifierClause = null;
			if (reader.IsStartElement("KeyIdentifier", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
			{
				string attribute = reader.GetAttribute("ValueType", null);
				if (string.IsNullOrEmpty(attribute) || (!StringComparer.Ordinal.Equals(attribute, "http://docs.oasis-open.org/wss/2004/xx/oasis-2004xx-wss-soap-message-security-1.1#ThumbprintSHA1") && !StringComparer.Ordinal.Equals(attribute, "http://docs.oasis-open.org/wss/oasis-wss-soap-message-security-1.1#ThumbprintSHA1")))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3079", attribute)));
				}
				string attribute2 = reader.GetAttribute("EncodingType", null);
				byte[] thumbprint;
				if (string.IsNullOrEmpty(attribute2) || StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"))
				{
					thumbprint = reader.ReadElementContentAsBase64();
				}
				else if (StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary"))
				{
					thumbprint = SoapHexBinary.Parse(reader.ReadElementContentAsString()).Value;
				}
				else
				{
					if (!StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Text"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3080", attribute2)));
					}
					thumbprint = new UTF8Encoding().GetBytes(reader.ReadElementContentAsString());
				}
				securityKeyIdentifierClause = new X509ThumbprintKeyIdentifierClause(thumbprint);
			}
			else if (reader.IsStartElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#"))
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3182")));
				}
				reader.ReadStartElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
				if (!reader.IsStartElement("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3183")));
				}
				string text = reader.ReadElementContentAsString("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#");
				if (string.IsNullOrEmpty(text))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3187")));
				}
				if (!reader.IsStartElement("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3184")));
				}
				string text2 = reader.ReadElementContentAsString("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#");
				if (string.IsNullOrEmpty(text2))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3188")));
				}
				reader.ReadEndElement();
				securityKeyIdentifierClause = new X509IssuerSerialKeyIdentifierClause(text, text2);
			}
			else if (reader.IsStartElement("X509Certificate", "http://www.w3.org/2000/09/xmldsig#"))
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3277")));
				}
				string s = reader.ReadElementString("X509Certificate", "http://www.w3.org/2000/09/xmldsig#");
				byte[] certificateRawData = Convert.FromBase64String(s);
				securityKeyIdentifierClause = new X509RawDataKeyIdentifierClause(certificateRawData);
			}
			else if (reader.IsStartElement("X509SKI", "http://www.w3.org/2000/09/xmldsig#"))
			{
				if (reader.IsEmptyElement)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3278")));
				}
				string s2 = reader.ReadElementString("X509SKI", "http://www.w3.org/2000/09/xmldsig#");
				byte[] ski = Convert.FromBase64String(s2);
				securityKeyIdentifierClause = new X509SubjectKeyIdentifierClause(ski);
			}
			if (securityKeyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3186")));
			}
			return securityKeyIdentifierClause;
		}

		private X509SubjectAndIssuer ReadX509SubjectAndIssuer(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement("X509SubjectAndIssuer", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				reader.ReadStartElement("X509SubjectAndIssuer", "http://docs.oasis-open.org/imi/ns/identity-200903");
			}
			if (reader.IsEmptyElement)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InformationCardException(SR.GetString("ID3171", reader.LocalName, reader.NamespaceURI)));
			}
			reader.ReadStartElement();
			string x509Subject = reader.ReadElementContentAsString("X509Subject", "http://docs.oasis-open.org/imi/ns/identity-200903");
			string x509Issuer = reader.ReadElementContentAsString("X509Issuer", "http://docs.oasis-open.org/imi/ns/identity-200903");
			EkuPolicy ekuPolicy = null;
			if (reader.IsStartElement("EKUPolicy", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				ekuPolicy = ReadEkuPolicy(reader);
			}
			reader.ReadEndElement();
			return new X509SubjectAndIssuer(x509Subject, x509Issuer, ekuPolicy);
		}
	}
}
