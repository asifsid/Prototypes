using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class RequestInformationCardsResponseSerializer
	{
		public virtual RequestInformationCardsResponse ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			if (!xmlDictionaryReader.IsStartElement("RequestInformationCardsResponse", "http://docs.oasis-open.org/imi/ns/identity-200903"))
			{
				xmlDictionaryReader.ReadStartElement("RequestInformationCardsResponse", "http://docs.oasis-open.org/imi/ns/identity-200903");
			}
			RequestInformationCardsResponse requestInformationCardsResponse = new RequestInformationCardsResponse();
			if (xmlDictionaryReader.IsEmptyElement)
			{
				xmlDictionaryReader.Skip();
				return requestInformationCardsResponse;
			}
			xmlDictionaryReader.ReadStartElement();
			while (xmlDictionaryReader.IsStartElement())
			{
				if (xmlDictionaryReader.IsStartElement("InformationCard", "http://schemas.xmlsoap.org/ws/2005/05/identity") || xmlDictionaryReader.IsStartElement("Signature", "http://www.w3.org/2000/09/xmldsig#"))
				{
					string item = xmlDictionaryReader.ReadOuterXml();
					requestInformationCardsResponse.InformationCards.Add(item);
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(xmlDictionaryReader, SR.GetString("ID3007", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
			}
			xmlDictionaryReader.ReadEndElement();
			return requestInformationCardsResponse;
		}

		public virtual void WriteXml(Stream stream, RequestInformationCardsResponse response)
		{
			if (stream == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("stream");
			}
			using XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, ownsStream: false);
			WriteXml(writer, response);
		}

		public virtual void WriteXml(XmlWriter writer, RequestInformationCardsResponse response)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("request");
			}
			XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(writer);
			xmlDictionaryWriter.WriteStartElement("ic09", "RequestInformationCardsResponse", "http://docs.oasis-open.org/imi/ns/identity-200903");
			foreach (string informationCard in response.InformationCards)
			{
				xmlDictionaryWriter.WriteRaw(informationCard);
			}
			xmlDictionaryWriter.WriteEndElement();
			writer.Flush();
		}
	}
}
