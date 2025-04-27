using System.Diagnostics;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
	internal class EncryptionMethodElement
	{
		private string _algorithm;

		private string _parameters;

		public string Algorithm
		{
			get
			{
				return _algorithm;
			}
			set
			{
				_algorithm = value;
			}
		}

		public string Parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters = value;
			}
		}

		public void ReadXml(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			if (!reader.IsStartElement("EncryptionMethod", "http://www.w3.org/2001/04/xmlenc#"))
			{
				return;
			}
			_algorithm = reader.GetAttribute("Algorithm", null);
			if (!reader.IsEmptyElement)
			{
				string text = reader.ReadOuterXml();
				if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
				{
					DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8024", reader.Name, reader.NamespaceURI, text));
				}
			}
			else
			{
				reader.Read();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("xenc", "EncryptionMethod", "http://www.w3.org/2001/04/xmlenc#");
			writer.WriteAttributeString("Algorithm", null, _algorithm);
			writer.WriteEndElement();
		}
	}
}
