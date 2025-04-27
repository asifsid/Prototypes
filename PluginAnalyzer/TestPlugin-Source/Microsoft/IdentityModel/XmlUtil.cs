using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel
{
	internal static class XmlUtil
	{
		public const string LanguageNamespaceUri = "http://www.w3.org/XML/1998/namespace";

		public const string LanguagePrefix = "xml";

		public const string LanguageLocalname = "lang";

		public const string LanguageAttribute = "xml:lang";

		public static void WriteLanguageAttribute(XmlWriter writer, string value)
		{
			writer.WriteAttributeString("xml", "lang", null, value);
		}

		public static XmlQualifiedName GetXsiType(XmlReader reader)
		{
			string attribute = reader.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
			reader.MoveToElement();
			if (string.IsNullOrEmpty(attribute))
			{
				return null;
			}
			return ResolveQName(reader, attribute);
		}

		public static bool EqualsQName(XmlQualifiedName qname, string localName, string namespaceUri)
		{
			if (null != qname && StringComparer.Ordinal.Equals(localName, qname.Name))
			{
				return StringComparer.Ordinal.Equals(namespaceUri, qname.Namespace);
			}
			return false;
		}

		public static bool IsNil(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (!string.IsNullOrEmpty(attribute))
			{
				return XmlConvert.ToBoolean(attribute);
			}
			return false;
		}

		public static string NormalizeEmptyString(string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				return s;
			}
			return null;
		}

		public static XmlQualifiedName ResolveQName(XmlReader reader, string qstring)
		{
			string name = qstring;
			string prefix = string.Empty;
			int num = qstring.IndexOf(':');
			if (num > -1)
			{
				prefix = qstring.Substring(0, num);
				name = qstring.Substring(num + 1, qstring.Length - (num + 1));
			}
			string ns = reader.LookupNamespace(prefix);
			return new XmlQualifiedName(name, ns);
		}

		public static void ValidateXsiType(XmlReader reader, string expectedTypeName, string expectedTypeNamespace)
		{
			ValidateXsiType(reader, expectedTypeName, expectedTypeNamespace, requireDeclaration: false);
		}

		public static void ValidateXsiType(XmlReader reader, string expectedTypeName, string expectedTypeNamespace, bool requireDeclaration)
		{
			XmlQualifiedName xsiType = GetXsiType(reader);
			if (null == xsiType)
			{
				if (requireDeclaration)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4104", reader.LocalName, reader.NamespaceURI));
				}
			}
			else if (!StringComparer.Ordinal.Equals(expectedTypeNamespace, xsiType.Namespace) || !StringComparer.Ordinal.Equals(expectedTypeName, xsiType.Name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4102", expectedTypeName, expectedTypeNamespace, xsiType.Name, xsiType.Namespace));
			}
		}

		public static string SerializeSecurityKeyIdentifier(SecurityKeyIdentifier ski, SecurityTokenSerializer securityTokenSerializer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			using (StringWriter output = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
			{
				XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
				xmlWriterSettings.OmitXmlDeclaration = true;
				using XmlWriter writer = XmlWriter.Create(output, xmlWriterSettings);
				securityTokenSerializer.WriteKeyIdentifier(writer, ski);
			}
			return stringBuilder.ToString();
		}

		public static bool IsValidXmlIDValue(string val)
		{
			if (string.IsNullOrEmpty(val))
			{
				return false;
			}
			if ((val[0] < 'A' || val[0] > 'Z') && (val[0] < 'a' || val[0] > 'z') && val[0] != '_')
			{
				return val[0] == ':';
			}
			return true;
		}

		public static void WriteElementStringAsUniqueId(XmlDictionaryWriter writer, XmlDictionaryString localName, XmlDictionaryString ns, string id)
		{
			writer.WriteStartElement(localName, ns);
			writer.WriteValue(id);
			writer.WriteEndElement();
		}

		public static void WriteElementContentAsInt64(XmlDictionaryWriter writer, XmlDictionaryString localName, XmlDictionaryString ns, long value)
		{
			writer.WriteStartElement(localName, ns);
			writer.WriteValue(value);
			writer.WriteEndElement();
		}

		public static long ReadElementContentAsInt64(XmlDictionaryReader reader)
		{
			reader.ReadFullStartElement();
			long result = reader.ReadContentAsLong();
			reader.ReadEndElement();
			return result;
		}

		public static List<XmlElement> GetXmlElements(XmlNodeList nodeList)
		{
			if (nodeList == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("nodeList");
			}
			List<XmlElement> list = new List<XmlElement>();
			foreach (XmlNode node in nodeList)
			{
				XmlElement xmlElement = node as XmlElement;
				if (xmlElement != null)
				{
					list.Add(xmlElement);
				}
			}
			return list;
		}
	}
}
