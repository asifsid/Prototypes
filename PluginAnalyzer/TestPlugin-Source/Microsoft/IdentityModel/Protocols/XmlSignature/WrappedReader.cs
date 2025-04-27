using System;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class WrappedReader : DelegatingXmlDictionaryReader, IXmlLineInfo
	{
		private XmlTokenStream _xmlTokens;

		private MemoryStream _contentStream;

		private TextReader _contentReader;

		private bool _recordDone;

		private int _depth;

		private bool _disposed;

		public int LineNumber => (base.InnerReader as IXmlLineInfo)?.LineNumber ?? 1;

		public int LinePosition => (base.InnerReader as IXmlLineInfo)?.LinePosition ?? 1;

		public XmlTokenStream XmlTokens => _xmlTokens;

		public WrappedReader(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!reader.IsStartElement())
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6023")));
			}
			_xmlTokens = new XmlTokenStream(32);
			InitializeInnerReader(reader);
			Record();
		}

		public override void Close()
		{
			OnEndOfContent();
			base.InnerReader.Close();
		}

		public bool HasLineInfo()
		{
			return (base.InnerReader as IXmlLineInfo)?.HasLineInfo() ?? false;
		}

		public override void MoveToAttribute(int index)
		{
			OnEndOfContent();
			base.InnerReader.MoveToAttribute(index);
		}

		public override bool MoveToAttribute(string name)
		{
			OnEndOfContent();
			return base.InnerReader.MoveToAttribute(name);
		}

		public override bool MoveToAttribute(string name, string ns)
		{
			OnEndOfContent();
			return base.InnerReader.MoveToAttribute(name, ns);
		}

		public override bool MoveToElement()
		{
			OnEndOfContent();
			return base.MoveToElement();
		}

		public override bool MoveToFirstAttribute()
		{
			OnEndOfContent();
			return base.MoveToFirstAttribute();
		}

		public override bool MoveToNextAttribute()
		{
			OnEndOfContent();
			return base.MoveToNextAttribute();
		}

		private void OnEndOfContent()
		{
			if (_contentReader != null)
			{
				_contentReader.Close();
				_contentReader = null;
			}
			if (_contentStream != null)
			{
				_contentStream.Close();
				_contentStream = null;
			}
		}

		public override bool Read()
		{
			OnEndOfContent();
			if (!base.Read())
			{
				return false;
			}
			if (!_recordDone)
			{
				Record();
			}
			return true;
		}

		private int ReadBinaryContent(byte[] buffer, int offset, int count, bool isBase64)
		{
			CryptoUtil.ValidateBufferBounds(buffer, offset, count);
			if (_contentStream == null)
			{
				string text;
				if (NodeType == XmlNodeType.Attribute)
				{
					text = Value;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(1000);
					while (NodeType != XmlNodeType.Element && NodeType != XmlNodeType.EndElement)
					{
						switch (NodeType)
						{
						case XmlNodeType.Text:
							stringBuilder.Append(Value);
							break;
						}
						Read();
					}
					text = stringBuilder.ToString();
				}
				byte[] buffer2 = (isBase64 ? Convert.FromBase64String(text) : SoapHexBinary.Parse(text).Value);
				_contentStream = new MemoryStream(buffer2);
			}
			int num = _contentStream.Read(buffer, offset, count);
			if (num == 0)
			{
				_contentStream.Close();
				_contentStream = null;
			}
			return num;
		}

		public override int ReadContentAsBase64(byte[] buffer, int offset, int count)
		{
			return ReadBinaryContent(buffer, offset, count, isBase64: true);
		}

		public override int ReadContentAsBinHex(byte[] buffer, int offset, int count)
		{
			return ReadBinaryContent(buffer, offset, count, isBase64: false);
		}

		public override int ReadValueChunk(char[] chars, int offset, int count)
		{
			if (_contentReader == null)
			{
				_contentReader = new StringReader(Value);
			}
			return _contentReader.Read(chars, offset, count);
		}

		private void Record()
		{
			switch (NodeType)
			{
			case XmlNodeType.Element:
			{
				bool isEmptyElement = base.InnerReader.IsEmptyElement;
				_xmlTokens.AddElement(base.InnerReader.Prefix, base.InnerReader.LocalName, base.InnerReader.NamespaceURI, isEmptyElement);
				if (base.InnerReader.MoveToFirstAttribute())
				{
					do
					{
						_xmlTokens.AddAttribute(base.InnerReader.Prefix, base.InnerReader.LocalName, base.InnerReader.NamespaceURI, base.InnerReader.Value);
					}
					while (base.InnerReader.MoveToNextAttribute());
					base.InnerReader.MoveToElement();
				}
				if (!isEmptyElement)
				{
					_depth++;
				}
				else if (_depth == 0)
				{
					_recordDone = true;
				}
				break;
			}
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.EntityReference:
			case XmlNodeType.Comment:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
			case XmlNodeType.EndEntity:
				_xmlTokens.Add(NodeType, Value);
				break;
			case XmlNodeType.EndElement:
				_xmlTokens.Add(NodeType, Value);
				if (--_depth == 0)
				{
					_recordDone = true;
				}
				break;
			default:
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID6024", base.InnerReader.NodeType, base.InnerReader.Name)));
			case XmlNodeType.DocumentType:
			case XmlNodeType.XmlDeclaration:
				break;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				if (_contentReader != null)
				{
					_contentReader.Dispose();
					_contentReader = null;
				}
				if (_contentStream != null)
				{
					_contentStream.Dispose();
					_contentStream = null;
				}
			}
			_disposed = true;
		}
	}
}
