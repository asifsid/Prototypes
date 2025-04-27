using System;
using System.Xml;

namespace Microsoft.IdentityModel.Web
{
	internal class IdentityModelWrappedXmlDictionaryReader : XmlDictionaryReader, IXmlLineInfo
	{
		private XmlReader _reader;

		private XmlDictionaryReaderQuotas _xmlDictionaryReaderQuotas;

		public override int AttributeCount => _reader.AttributeCount;

		public override string BaseURI => _reader.BaseURI;

		public override bool CanReadBinaryContent => _reader.CanReadBinaryContent;

		public override bool CanReadValueChunk => _reader.CanReadValueChunk;

		public override int Depth => _reader.Depth;

		public override bool EOF => _reader.EOF;

		public override bool HasValue => _reader.HasValue;

		public override bool IsDefault => _reader.IsDefault;

		public override bool IsEmptyElement => _reader.IsEmptyElement;

		public override string LocalName => _reader.LocalName;

		public override string Name => _reader.Name;

		public override string NamespaceURI => _reader.NamespaceURI;

		public override XmlNameTable NameTable => _reader.NameTable;

		public override XmlNodeType NodeType => _reader.NodeType;

		public override string Prefix => _reader.Prefix;

		public override char QuoteChar => _reader.QuoteChar;

		public override ReadState ReadState => _reader.ReadState;

		public override string this[int index] => _reader[index];

		public override string this[string name] => _reader[name];

		public override string this[string name, string namespaceUri] => _reader[name, namespaceUri];

		public override string Value => _reader.Value;

		public override string XmlLang => _reader.XmlLang;

		public override XmlSpace XmlSpace => _reader.XmlSpace;

		public override Type ValueType => _reader.ValueType;

		public int LineNumber => (_reader as IXmlLineInfo)?.LineNumber ?? 1;

		public int LinePosition => (_reader as IXmlLineInfo)?.LinePosition ?? 1;

		public override XmlDictionaryReaderQuotas Quotas => _xmlDictionaryReaderQuotas;

		public IdentityModelWrappedXmlDictionaryReader(XmlReader reader, XmlDictionaryReaderQuotas xmlDictionaryReaderQuotas)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (xmlDictionaryReaderQuotas == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("xmlDictionaryReaderQuotas");
			}
			_reader = reader;
			_xmlDictionaryReaderQuotas = xmlDictionaryReaderQuotas;
		}

		public override void Close()
		{
			_reader.Close();
		}

		public override string GetAttribute(int index)
		{
			return _reader.GetAttribute(index);
		}

		public override string GetAttribute(string name)
		{
			return _reader.GetAttribute(name);
		}

		public override string GetAttribute(string name, string namespaceUri)
		{
			return _reader.GetAttribute(name, namespaceUri);
		}

		public override bool IsStartElement(string name)
		{
			return _reader.IsStartElement(name);
		}

		public override bool IsStartElement(string localName, string namespaceUri)
		{
			return _reader.IsStartElement(localName, namespaceUri);
		}

		public override string LookupNamespace(string namespaceUri)
		{
			return _reader.LookupNamespace(namespaceUri);
		}

		public override void MoveToAttribute(int index)
		{
			_reader.MoveToAttribute(index);
		}

		public override bool MoveToAttribute(string name)
		{
			return _reader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string name, string namespaceUri)
		{
			return _reader.MoveToAttribute(name, namespaceUri);
		}

		public override bool MoveToElement()
		{
			return _reader.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			return _reader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return _reader.MoveToNextAttribute();
		}

		public override bool Read()
		{
			return _reader.Read();
		}

		public override bool ReadAttributeValue()
		{
			return _reader.ReadAttributeValue();
		}

		public override string ReadElementString(string name)
		{
			return _reader.ReadElementString(name);
		}

		public override string ReadElementString(string localName, string namespaceUri)
		{
			return _reader.ReadElementString(localName, namespaceUri);
		}

		public override string ReadInnerXml()
		{
			return _reader.ReadInnerXml();
		}

		public override string ReadOuterXml()
		{
			return _reader.ReadOuterXml();
		}

		public override void ReadStartElement(string name)
		{
			_reader.ReadStartElement(name);
		}

		public override void ReadStartElement(string localName, string namespaceUri)
		{
			_reader.ReadStartElement(localName, namespaceUri);
		}

		public override void ReadEndElement()
		{
			_reader.ReadEndElement();
		}

		public override string ReadString()
		{
			return _reader.ReadString();
		}

		public override void ResolveEntity()
		{
			_reader.ResolveEntity();
		}

		public override int ReadElementContentAsBase64(byte[] buffer, int offset, int count)
		{
			return _reader.ReadElementContentAsBase64(buffer, offset, count);
		}

		public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
		{
			return _reader.ReadContentAsBase64(buffer, offset, count);
		}

		public override int ReadElementContentAsBinHex(byte[] buffer, int offset, int count)
		{
			return _reader.ReadElementContentAsBinHex(buffer, offset, count);
		}

		public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
		{
			return _reader.ReadContentAsBinHex(buffer, offset, count);
		}

		public override int ReadValueChunk(char[] chars, int offset, int count)
		{
			return _reader.ReadValueChunk(chars, offset, count);
		}

		public override bool ReadContentAsBoolean()
		{
			return _reader.ReadContentAsBoolean();
		}

		public override DateTime ReadContentAsDateTime()
		{
			return _reader.ReadContentAsDateTime();
		}

		public override decimal ReadContentAsDecimal()
		{
			return (decimal)_reader.ReadContentAs(typeof(decimal), null);
		}

		public override double ReadContentAsDouble()
		{
			return _reader.ReadContentAsDouble();
		}

		public override int ReadContentAsInt()
		{
			return _reader.ReadContentAsInt();
		}

		public override long ReadContentAsLong()
		{
			return _reader.ReadContentAsLong();
		}

		public override float ReadContentAsFloat()
		{
			return _reader.ReadContentAsFloat();
		}

		public override string ReadContentAsString()
		{
			return _reader.ReadContentAsString();
		}

		public override object ReadContentAs(Type valueType, IXmlNamespaceResolver namespaceResolver)
		{
			return _reader.ReadContentAs(valueType, namespaceResolver);
		}

		public bool HasLineInfo()
		{
			return (_reader as IXmlLineInfo)?.HasLineInfo() ?? false;
		}
	}
}
