using System;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WSFederation;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class WSFedMessageTraceRecord : TraceRecord
	{
		internal new const string ElementName = "WSFederationMessageTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/WSFederationMessageTraceRecord";

		private WSFederationMessage _wsFederationMessage;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/WSFederationMessageTraceRecord";

		public WSFedMessageTraceRecord(WSFederationMessage wsFederationMessage)
		{
			_wsFederationMessage = wsFederationMessage;
		}

		public override void WriteTo(XmlWriter writer)
		{
			writer.WriteStartElement("WSFederationMessageTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			writer.WriteStartElement("WSFederationMessage");
			writer.WriteElementString("BaseUri", _wsFederationMessage.BaseUri.AbsoluteUri);
			foreach (string key in _wsFederationMessage.Parameters.Keys)
			{
				if (StringComparer.OrdinalIgnoreCase.Equals(key, "wresult"))
				{
					PlainXmlWriter.WriteDecoded(key, (_wsFederationMessage.Parameters[key] == null) ? string.Empty : _wsFederationMessage.Parameters[key], writer);
				}
				else
				{
					writer.WriteElementString(key, (_wsFederationMessage.Parameters[key] == null) ? string.Empty : _wsFederationMessage.Parameters[key]);
				}
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
	}
}
