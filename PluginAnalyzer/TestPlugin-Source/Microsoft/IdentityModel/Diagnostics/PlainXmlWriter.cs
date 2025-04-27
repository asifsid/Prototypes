using System;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class PlainXmlWriter : XmlWriter
	{
		private TraceXPathNavigator _navigator;

		private bool _writingAttribute;

		private string _currentAttributeName;

		private string _currentAttributePrefix;

		private string _currentAttributeNs;

		private string _currentAttributeText = string.Empty;

		public TraceXPathNavigator Navigator => _navigator;

		public override WriteState WriteState => _navigator.WriteState;

		public override string XmlLang => string.Empty;

		public override XmlSpace XmlSpace => XmlSpace.Default;

		public PlainXmlWriter()
		{
			_navigator = new TraceXPathNavigator();
		}

		public override void Close()
		{
		}

		public override void Flush()
		{
		}

		public override string LookupPrefix(string ns)
		{
			return _navigator.LookupPrefix(ns);
		}

		public override void WriteBase64(byte[] buffer, int offset, int count)
		{
			_navigator.AddText(Convert.ToBase64String(buffer, offset, count));
		}

		public override void WriteCData(string text)
		{
			WriteRaw("<![CDATA[" + text + "]]>");
		}

		public override void WriteCharEntity(char ch)
		{
		}

		public override void WriteChars(char[] buffer, int index, int count)
		{
			if (buffer != null && index >= 0 && count >= 0)
			{
				WriteString(new string(buffer, index, count));
			}
		}

		public override void WriteComment(string text)
		{
			_navigator.AddComment(text);
		}

		public static void WriteDecoded(string startElement, string wresult, XmlWriter writer)
		{
			writer.WriteStartElement(startElement);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(wresult);
			XPathNavigator xPathNavigator = xmlDocument.CreateNavigator();
			xPathNavigator.WriteSubtree(writer);
			writer.WriteEndElement();
		}

		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		public override void WriteEndAttribute()
		{
			if (_writingAttribute)
			{
				_navigator.AddAttribute(_currentAttributeName, _currentAttributeText, _currentAttributeNs, _currentAttributePrefix);
				_writingAttribute = false;
			}
		}

		public override void WriteEndElement()
		{
			_navigator.CloseElement();
		}

		public override void WriteEndDocument()
		{
		}

		public override void WriteEntityRef(string name)
		{
		}

		public override void WriteFullEndElement()
		{
			WriteEndElement();
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			_navigator.AddProcessingInstruction(name, text);
		}

		public override void WriteRaw(string data)
		{
			WriteString(data);
		}

		public override void WriteRaw(char[] buffer, int index, int count)
		{
			WriteChars(buffer, index, count);
		}

		public override void WriteStartAttribute(string prefix, string localName, string ns)
		{
			if (!_writingAttribute)
			{
				_currentAttributeName = localName;
				_currentAttributePrefix = prefix;
				_currentAttributeNs = ns;
				_currentAttributeText = string.Empty;
				_writingAttribute = true;
			}
		}

		public override void WriteStartDocument()
		{
		}

		public override void WriteStartDocument(bool standalone)
		{
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (string.IsNullOrEmpty(localName))
			{
				_navigator.AddElement(prefix, "localName", ns);
			}
			else
			{
				_navigator.AddElement(prefix, localName, ns);
			}
		}

		public override void WriteString(string text)
		{
			if (_writingAttribute)
			{
				_currentAttributeText += text;
			}
			else
			{
				WriteValue(text);
			}
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
		}

		public override void WriteValue(object value)
		{
			_navigator.AddText(value.ToString());
		}

		public override void WriteValue(string value)
		{
			_navigator.AddText(value);
		}

		public override void WriteWhitespace(string ws)
		{
		}
	}
}
