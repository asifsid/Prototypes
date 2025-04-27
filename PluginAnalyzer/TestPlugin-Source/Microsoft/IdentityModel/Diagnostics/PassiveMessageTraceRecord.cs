using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class PassiveMessageTraceRecord : TraceRecord
	{
		internal new const string ElementName = "PassiveMessageTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/PassiveMessageTraceRecord";

		private IDictionary<string, string> _dictionary;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/PassiveMessageTraceRecord";

		public PassiveMessageTraceRecord(IDictionary<string, string> dictionary)
		{
			_dictionary = dictionary;
		}

		public override void WriteTo(XmlWriter writer)
		{
			writer.WriteStartElement("PassiveMessageTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			writer.WriteStartElement("Request");
			foreach (string key in _dictionary.Keys)
			{
				if (StringComparer.OrdinalIgnoreCase.Equals(key, "wresult"))
				{
					PlainXmlWriter.WriteDecoded(key, (_dictionary[key] == null) ? string.Empty : _dictionary[key], writer);
				}
				else
				{
					writer.WriteElementString(key, (_dictionary[key] == null) ? string.Empty : _dictionary[key]);
				}
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
	}
}
