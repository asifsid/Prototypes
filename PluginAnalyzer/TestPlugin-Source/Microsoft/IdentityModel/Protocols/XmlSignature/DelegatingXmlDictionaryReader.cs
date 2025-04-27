using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	[ComVisible(true)]
	public class DelegatingXmlDictionaryReader : XmlDictionaryReader
	{
		private XmlDictionaryReader _innerReader;

		protected XmlDictionaryReader InnerReader => _innerReader;

		public override string this[int i] => _innerReader[i];

		public override string this[string name] => _innerReader[name];

		public override string this[string name, string namespaceURI] => _innerReader[name, namespaceURI];

		public override int AttributeCount => _innerReader.AttributeCount;

		public override string BaseURI => _innerReader.BaseURI;

		public override int Depth => _innerReader.Depth;

		public override bool EOF => _innerReader.EOF;

		public override bool HasValue => _innerReader.HasValue;

		public override bool IsDefault => _innerReader.IsDefault;

		public override bool IsEmptyElement => _innerReader.IsEmptyElement;

		public override string LocalName => _innerReader.LocalName;

		public override string Name => _innerReader.Name;

		public override string NamespaceURI => _innerReader.NamespaceURI;

		public override XmlNameTable NameTable => _innerReader.NameTable;

		public override XmlNodeType NodeType => _innerReader.NodeType;

		public override string Prefix => _innerReader.Prefix;

		public override char QuoteChar => _innerReader.QuoteChar;

		public override ReadState ReadState => _innerReader.ReadState;

		public override string Value => _innerReader.Value;

		public override Type ValueType => _innerReader.ValueType;

		public override string XmlLang => _innerReader.XmlLang;

		public override XmlSpace XmlSpace => _innerReader.XmlSpace;

		protected DelegatingXmlDictionaryReader()
		{
		}

		protected void InitializeInnerReader(XmlDictionaryReader innerReader)
		{
			if (innerReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("innerReader");
			}
			_innerReader = innerReader;
		}

		public override void Close()
		{
			_innerReader.Close();
		}

		public override string GetAttribute(int i)
		{
			return _innerReader.GetAttribute(i);
		}

		public override string GetAttribute(string name)
		{
			return _innerReader.GetAttribute(name);
		}

		public override string GetAttribute(string name, string namespaceURI)
		{
			return _innerReader.GetAttribute(name, namespaceURI);
		}

		public override string LookupNamespace(string prefix)
		{
			return _innerReader.LookupNamespace(prefix);
		}

		public override void MoveToAttribute(int i)
		{
			_innerReader.MoveToAttribute(i);
		}

		public override bool MoveToAttribute(string name)
		{
			return _innerReader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			return _innerReader.MoveToAttribute(name, ns);
		}

		public override bool MoveToElement()
		{
			return _innerReader.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			return _innerReader.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			return _innerReader.MoveToNextAttribute();
		}

		public override bool Read()
		{
			return _innerReader.Read();
		}

		public override bool ReadAttributeValue()
		{
			return _innerReader.ReadAttributeValue();
		}

		public override int ReadContentAsBase64(byte[] buffer, int index, int count)
		{
			return _innerReader.ReadContentAsBase64(buffer, index, count);
		}

		public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
		{
			return _innerReader.ReadContentAsBinHex(buffer, index, count);
		}

		public override System.Xml.UniqueId ReadContentAsUniqueId()
		{
			return _innerReader.ReadContentAsUniqueId();
		}

		public override int ReadValueChunk(char[] buffer, int index, int count)
		{
			return _innerReader.ReadValueChunk(buffer, index, count);
		}

		public override void ResolveEntity()
		{
			_innerReader.ResolveEntity();
		}
	}
}
