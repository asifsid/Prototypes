using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class RequestInformationCardsSerializer
	{
		private WSTrustSerializationContext _trustSerializationContext = new WSTrustSerializationContext();

		public RequestInformationCardsSerializer()
		{
		}

		public RequestInformationCardsSerializer(WSTrustSerializationContext trustSerializationContext)
		{
			if (trustSerializationContext == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("trustSerializationContext");
			}
			_trustSerializationContext = trustSerializationContext;
		}

		public virtual RequestInformationCards ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			if (!xmlDictionaryReader.IsStartElement("RequestInformationCards", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				xmlDictionaryReader.ReadStartElement("RequestInformationCards", "http://docs.oasis-open.org/imi/ns/identity-200903");
			}
			RequestInformationCards requestInformationCards = new RequestInformationCards();
			if (xmlDictionaryReader.IsEmptyElement)
			{
				xmlDictionaryReader.Skip();
				return requestInformationCards;
			}
			xmlDictionaryReader.ReadStartElement();
			while (xmlDictionaryReader.IsStartElement())
			{
				if (xmlDictionaryReader.IsStartElement("Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
				{
					if (requestInformationCards.Issuer != null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(xmlDictionaryReader, SR.GetString("ID3280", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
					}
					requestInformationCards.Issuer = ReadElementUri(xmlDictionaryReader, "Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity");
					continue;
				}
				if (xmlDictionaryReader.IsStartElement("CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity"))
				{
					if (requestInformationCards.CardIdentifier != null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(xmlDictionaryReader, SR.GetString("ID3280", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
					}
					requestInformationCards.CardIdentifier = ReadElementUri(xmlDictionaryReader, "CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity");
					continue;
				}
				if (xmlDictionaryReader.IsStartElement("CardType", "http://docs.oasis-open.org/imi/ns/identity-200903"))
				{
					if (requestInformationCards.CardType != null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(xmlDictionaryReader, SR.GetString("ID3280", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
					}
					requestInformationCards.CardType = ReadElementUri(xmlDictionaryReader, "CardType", "http://docs.oasis-open.org/imi/ns/identity-200903");
					continue;
				}
				if (xmlDictionaryReader.IsStartElement("OnBehalfOf", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
				{
					if (requestInformationCards.OnBehalfOf != null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(xmlDictionaryReader, SR.GetString("ID3280", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
					}
					requestInformationCards.OnBehalfOf = new Microsoft.IdentityModel.Tokens.SecurityTokenElement(ReadInnerXml(xmlDictionaryReader), _trustSerializationContext.SecurityTokenHandlers);
					continue;
				}
				if (xmlDictionaryReader.IsStartElement("CardSignatureFormat", "http://docs.oasis-open.org/imi/ns/identity-200903"))
				{
					if (requestInformationCards.CardSignatureFormat != 0)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(xmlDictionaryReader, SR.GetString("ID3280", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
					}
					requestInformationCards.CardSignatureFormat = ReadCardSignatureFormat(xmlDictionaryReader);
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(xmlDictionaryReader, SR.GetString("ID3007", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
			}
			xmlDictionaryReader.ReadEndElement();
			return requestInformationCards;
		}

		public virtual void WriteXml(Stream stream, RequestInformationCards request)
		{
			if (stream == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("stream");
			}
			using XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, ownsStream: false);
			WriteXml(writer, request);
		}

		public virtual void WriteXml(XmlWriter writer, RequestInformationCards request)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (request == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(writer);
			xmlDictionaryWriter.WriteStartElement("ic09", "RequestInformationCards", "http://docs.oasis-open.org/imi/ns/identity-200903");
			if (request.Issuer != null)
			{
				xmlDictionaryWriter.WriteElementString("i", "Issuer", "http://schemas.xmlsoap.org/ws/2005/05/identity", request.Issuer.OriginalString);
			}
			if (request.CardIdentifier != null)
			{
				xmlDictionaryWriter.WriteElementString("i", "CardId", "http://schemas.xmlsoap.org/ws/2005/05/identity", request.CardIdentifier.OriginalString);
			}
			if (request.CardType != null)
			{
				xmlDictionaryWriter.WriteElementString("ic09", "CardType", "http://docs.oasis-open.org/imi/ns/identity-200903", request.CardType.OriginalString);
			}
			if (request.OnBehalfOf != null)
			{
				xmlDictionaryWriter.WriteStartElement("trust", "OnBehalfOf", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
				WriteTokenElement(request.OnBehalfOf, "OnBehalfOf", _trustSerializationContext, xmlDictionaryWriter);
				xmlDictionaryWriter.WriteEndElement();
			}
			xmlDictionaryWriter.WriteElementString("ic09", "CardSignatureFormat", "http://docs.oasis-open.org/imi/ns/identity-200903", request.CardSignatureFormat.ToString());
			xmlDictionaryWriter.WriteEndElement();
			writer.Flush();
		}

		private CardSignatureFormatType ReadCardSignatureFormat(XmlDictionaryReader reader)
		{
			string text = reader.ReadElementString("CardSignatureFormat", "http://docs.oasis-open.org/imi/ns/identity-200903");
			return text switch
			{
				"Enveloping" => CardSignatureFormatType.Enveloping, 
				"Enveloped" => CardSignatureFormatType.Enveloped, 
				"None" => CardSignatureFormatType.None, 
				_ => throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID3281", text)), 
			};
		}

		private static Uri ReadElementUri(XmlDictionaryReader reader, string localName, string ns)
		{
			string text = reader.ReadElementString(localName, ns);
			if (!UriUtil.CanCreateValidUri(text, UriKind.Absolute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID0014", text));
			}
			return new Uri(text);
		}

		private static XmlElement ReadInnerXml(XmlReader reader)
		{
			return ReadInnerXml(reader, onStartElement: false);
		}

		private static XmlElement ReadInnerXml(XmlReader reader, bool onStartElement)
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
	}
}
