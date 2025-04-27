using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	[ComVisible(true)]
	public class DelegatingXmlDictionaryWriter : XmlDictionaryWriter
	{
		private XmlDictionaryWriter _innerWriter;

		private XmlWriter _tracingWriter;

		protected XmlDictionaryWriter InnerWriter => _innerWriter;

		public override WriteState WriteState => _innerWriter.WriteState;

		public override bool CanCanonicalize => _innerWriter.CanCanonicalize;

		protected DelegatingXmlDictionaryWriter()
		{
		}

		protected void InitializeInnerWriter(XmlDictionaryWriter innerWriter)
		{
			if (innerWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("innerWriter");
			}
			_innerWriter = innerWriter;
		}

		protected void InitializeTracingWriter(XmlWriter tracingWriter)
		{
			_tracingWriter = tracingWriter;
		}

		public override void Close()
		{
			_innerWriter.Close();
			if (_tracingWriter != null)
			{
				_tracingWriter.Close();
			}
		}

		public override void Flush()
		{
			_innerWriter.Flush();
			if (_tracingWriter != null)
			{
				_tracingWriter.Flush();
			}
		}

		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			_innerWriter.WriteBase64(buffer, index, count);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteBase64(buffer, index, count);
			}
		}

		public override void WriteCData(string text)
		{
			_innerWriter.WriteCData(text);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteCData(text);
			}
		}

		public override void WriteCharEntity(char ch)
		{
			_innerWriter.WriteCharEntity(ch);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteCharEntity(ch);
			}
		}

		public override void WriteChars(char[] buffer, int index, int count)
		{
			_innerWriter.WriteChars(buffer, index, count);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteChars(buffer, index, count);
			}
		}

		public override void WriteComment(string text)
		{
			_innerWriter.WriteComment(text);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteComment(text);
			}
		}

		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			_innerWriter.WriteDocType(name, pubid, sysid, subset);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteDocType(name, pubid, sysid, subset);
			}
		}

		public override void WriteEndAttribute()
		{
			_innerWriter.WriteEndAttribute();
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteEndAttribute();
			}
		}

		public override void WriteEndDocument()
		{
			_innerWriter.WriteEndDocument();
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteEndDocument();
			}
		}

		public override void WriteEndElement()
		{
			_innerWriter.WriteEndElement();
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteEndElement();
			}
		}

		public override void WriteEntityRef(string name)
		{
			_innerWriter.WriteEntityRef(name);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteEntityRef(name);
			}
		}

		public override void WriteFullEndElement()
		{
			_innerWriter.WriteFullEndElement();
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteFullEndElement();
			}
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			_innerWriter.WriteProcessingInstruction(name, text);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteProcessingInstruction(name, text);
			}
		}

		public override void WriteRaw(char[] buffer, int index, int count)
		{
			_innerWriter.WriteRaw(buffer, index, count);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteRaw(buffer, index, count);
			}
		}

		public override void WriteRaw(string data)
		{
			_innerWriter.WriteRaw(data);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteRaw(data);
			}
		}

		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			_innerWriter.WriteStartAttribute(prefix, localName, ns);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteStartAttribute(prefix, localName, ns);
			}
		}

		public override void WriteStartDocument()
		{
			_innerWriter.WriteStartDocument();
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteStartDocument();
			}
		}

		public override void WriteStartDocument(bool standalone)
		{
			_innerWriter.WriteStartDocument(standalone);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteStartDocument(standalone);
			}
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			_innerWriter.WriteStartElement(prefix, localName, ns);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteStartElement(prefix, localName, ns);
			}
		}

		public override void WriteString(string text)
		{
			_innerWriter.WriteString(text);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteString(text);
			}
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			_innerWriter.WriteSurrogateCharEntity(lowChar, highChar);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteSurrogateCharEntity(lowChar, highChar);
			}
		}

		public override void WriteWhitespace(string ws)
		{
			_innerWriter.WriteWhitespace(ws);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteWhitespace(ws);
			}
		}

		public override void WriteXmlAttribute(string localName, string value)
		{
			_innerWriter.WriteXmlAttribute(localName, value);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteAttributeString(localName, value);
			}
		}

		public override void WriteXmlnsAttribute(string prefix, string namespaceUri)
		{
			_innerWriter.WriteXmlnsAttribute(prefix, namespaceUri);
			if (_tracingWriter != null)
			{
				_tracingWriter.WriteAttributeString(prefix, string.Empty, namespaceUri, string.Empty);
			}
		}

		public override string LookupPrefix(string ns)
		{
			return _innerWriter.LookupPrefix(ns);
		}

		public override void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
		{
			_innerWriter.StartCanonicalization(stream, includeComments, inclusivePrefixes);
		}

		public override void EndCanonicalization()
		{
			_innerWriter.EndCanonicalization();
		}
	}
}
