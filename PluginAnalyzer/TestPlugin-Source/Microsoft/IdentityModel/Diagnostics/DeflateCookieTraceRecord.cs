using System.Globalization;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class DeflateCookieTraceRecord : TraceRecord
	{
		internal new const string ElementName = "DeflateCookieTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/DeflateCookieTraceRecord";

		private int _originalSize;

		private int _deflatedSize;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/DeflateCookieTraceRecord";

		public DeflateCookieTraceRecord(int originalSize, int deflatedSize)
		{
			_originalSize = originalSize;
			_deflatedSize = deflatedSize;
		}

		public override void WriteTo(XmlWriter writer)
		{
			writer.WriteStartElement("DeflateCookieTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			writer.WriteElementString("OriginalSize", _originalSize.ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("AfterDeflating", _deflatedSize.ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}
	}
}
