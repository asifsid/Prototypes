using System;
using System.Globalization;
using System.Web;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class ChunkedCookieHandlerTraceRecord : TraceRecord
	{
		public enum Action
		{
			Reading,
			Writing,
			Deleting
		}

		internal new const string ElementName = "ChunkedCookieHandlerTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/ChunkedCookieHandlerTraceRecord";

		private Action _action;

		private HttpCookie _cookie;

		private string _cookiePath;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/DeflateCookieTraceRecord";

		public ChunkedCookieHandlerTraceRecord(Action action, HttpCookie cookie, string cookiePath)
		{
			_action = action;
			_cookie = cookie;
			_cookiePath = cookiePath;
		}

		public override void WriteTo(XmlWriter writer)
		{
			string value = "unknown";
			if (_action == Action.Writing)
			{
				value = SR.GetString("TraceChunkedCookieHandlerWriting");
			}
			else if (_action == Action.Reading)
			{
				value = SR.GetString("TraceChunkedCookieHandlerReading");
			}
			else if (_action == Action.Deleting)
			{
				value = SR.GetString("TraceChunkedCookieHandlerDeleting");
			}
			writer.WriteStartElement("ChunkedCookieHandlerTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			writer.WriteAttributeString("Action", value);
			if (!string.IsNullOrEmpty(_cookie.Name))
			{
				writer.WriteElementString("Name", _cookie.Name);
			}
			if (_action == Action.Writing || _action == Action.Deleting)
			{
				if (!string.IsNullOrEmpty(_cookie.Path))
				{
					writer.WriteElementString("Path", _cookiePath);
				}
				if (!string.IsNullOrEmpty(_cookie.Domain))
				{
					writer.WriteElementString("Domain", _cookie.Domain);
				}
				if (_action == Action.Writing)
				{
					if (_cookie.Expires == DateTime.MinValue)
					{
						writer.WriteElementString("Expires", "Session");
					}
					else
					{
						writer.WriteElementString("Expires", _cookie.Expires.ToString(DateTimeFormatInfo.InvariantInfo));
					}
					writer.WriteElementString("Secure", _cookie.Secure.ToString(DateTimeFormatInfo.InvariantInfo));
					writer.WriteElementString("HttpOnly", _cookie.HttpOnly.ToString(DateTimeFormatInfo.InvariantInfo));
				}
			}
			writer.WriteEndElement();
		}
	}
}
