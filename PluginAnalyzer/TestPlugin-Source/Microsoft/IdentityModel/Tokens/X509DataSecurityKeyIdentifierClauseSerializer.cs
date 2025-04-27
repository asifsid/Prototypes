using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class X509DataSecurityKeyIdentifierClauseSerializer : SecurityKeyIdentifierClauseSerializer
	{
		public override bool CanReadKeyIdentifierClause(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.IsStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#");
		}

		public override bool CanWriteKeyIdentifierClause(SecurityKeyIdentifierClause securityKeyIdentifierClause)
		{
			if (securityKeyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityKeyIdentifierClause");
			}
			if (!(securityKeyIdentifierClause is X509IssuerSerialKeyIdentifierClause) && !(securityKeyIdentifierClause is X509RawDataKeyIdentifierClause))
			{
				return securityKeyIdentifierClause is X509SubjectKeyIdentifierClause;
			}
			return true;
		}

		public override SecurityKeyIdentifierClause ReadKeyIdentifierClause(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			if (!xmlDictionaryReader.IsStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, "X509Data", "http://www.w3.org/2000/09/xmldsig#"));
			}
			xmlDictionaryReader.ReadStartElement("X509Data", "http://www.w3.org/2000/09/xmldsig#");
			SecurityKeyIdentifierClause result;
			if (xmlDictionaryReader.IsStartElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#"))
			{
				xmlDictionaryReader.ReadStartElement("X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
				if (!xmlDictionaryReader.IsStartElement("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, "X509IssuerName", "http://www.w3.org/2000/09/xmldsig#"));
				}
				string issuerName = xmlDictionaryReader.ReadElementContentAsString("X509IssuerName", "http://www.w3.org/2000/09/xmldsig#");
				if (!xmlDictionaryReader.IsStartElement("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3032", reader.LocalName, reader.NamespaceURI, "X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#"));
				}
				string issuerSerialNumber = xmlDictionaryReader.ReadElementContentAsString("X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#");
				xmlDictionaryReader.ReadEndElement();
				result = new X509IssuerSerialKeyIdentifierClause(issuerName, issuerSerialNumber);
			}
			else if (xmlDictionaryReader.IsStartElement("X509SKI", "http://www.w3.org/2000/09/xmldsig#"))
			{
				byte[] array = xmlDictionaryReader.ReadElementContentAsBase64();
				if (array == null || array.Length == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4258", "X509SKI", "http://www.w3.org/2000/09/xmldsig#"));
				}
				result = new X509SubjectKeyIdentifierClause(array);
			}
			else
			{
				if (!xmlDictionaryReader.IsStartElement("X509Certificate", "http://www.w3.org/2000/09/xmldsig#"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4260", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI));
				}
				byte[] array2 = null;
				while (reader.IsStartElement("X509Certificate", "http://www.w3.org/2000/09/xmldsig#"))
				{
					if (array2 == null)
					{
						array2 = xmlDictionaryReader.ReadElementContentAsBase64();
						if (array2 == null || array2.Length == 0)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4258", "X509Certificate", "http://www.w3.org/2000/09/xmldsig#"));
						}
					}
					else
					{
						reader.Skip();
					}
				}
				result = new X509RawDataKeyIdentifierClause(array2);
			}
			xmlDictionaryReader.ReadEndElement();
			return result;
		}

		public override void WriteKeyIdentifierClause(XmlWriter writer, SecurityKeyIdentifierClause securityKeyIdentifierClause)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (securityKeyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityKeyIdentifierClause");
			}
			X509IssuerSerialKeyIdentifierClause x509IssuerSerialKeyIdentifierClause = securityKeyIdentifierClause as X509IssuerSerialKeyIdentifierClause;
			if (x509IssuerSerialKeyIdentifierClause != null)
			{
				writer.WriteStartElement("ds", "X509Data", "http://www.w3.org/2000/09/xmldsig#");
				writer.WriteStartElement("ds", "X509IssuerSerial", "http://www.w3.org/2000/09/xmldsig#");
				writer.WriteElementString("ds", "X509IssuerName", "http://www.w3.org/2000/09/xmldsig#", x509IssuerSerialKeyIdentifierClause.IssuerName);
				writer.WriteElementString("ds", "X509SerialNumber", "http://www.w3.org/2000/09/xmldsig#", x509IssuerSerialKeyIdentifierClause.IssuerSerialNumber);
				writer.WriteEndElement();
				writer.WriteEndElement();
				return;
			}
			X509SubjectKeyIdentifierClause x509SubjectKeyIdentifierClause = securityKeyIdentifierClause as X509SubjectKeyIdentifierClause;
			if (x509SubjectKeyIdentifierClause != null)
			{
				writer.WriteStartElement("ds", "X509Data", "http://www.w3.org/2000/09/xmldsig#");
				writer.WriteStartElement("ds", "X509SKI", "http://www.w3.org/2000/09/xmldsig#");
				byte[] x509SubjectKeyIdentifier = x509SubjectKeyIdentifierClause.GetX509SubjectKeyIdentifier();
				writer.WriteBase64(x509SubjectKeyIdentifier, 0, x509SubjectKeyIdentifier.Length);
				writer.WriteEndElement();
				writer.WriteEndElement();
				return;
			}
			X509RawDataKeyIdentifierClause x509RawDataKeyIdentifierClause = securityKeyIdentifierClause as X509RawDataKeyIdentifierClause;
			if (x509RawDataKeyIdentifierClause != null)
			{
				writer.WriteStartElement("ds", "X509Data", "http://www.w3.org/2000/09/xmldsig#");
				writer.WriteStartElement("ds", "X509Certificate", "http://www.w3.org/2000/09/xmldsig#");
				byte[] x509RawData = x509RawDataKeyIdentifierClause.GetX509RawData();
				writer.WriteBase64(x509RawData, 0, x509RawData.Length);
				writer.WriteEndElement();
				writer.WriteEndElement();
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("securityKeyIdentifierClause", SR.GetString("ID4259", securityKeyIdentifierClause.GetType()));
		}
	}
}
