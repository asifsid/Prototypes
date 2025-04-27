using System.Globalization;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class HashTraceRecord : TraceRecord
	{
		internal new const string ElementName = "HashTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/HashTraceRecord";

		private static readonly char[] hexDigits = new char[16]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};

		private byte[] _octets;

		private byte[] _untranslatedOctets;

		private string _hash;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/HashTraceRecord";

		public HashTraceRecord(string hash, byte[] octets, byte[] untranslatedOctets)
		{
			_hash = hash;
			_octets = octets;
			_untranslatedOctets = untranslatedOctets;
		}

		public override void WriteTo(XmlWriter writer)
		{
			writer.WriteStartElement("HashTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			if (_untranslatedOctets != null)
			{
				WriteBytes(_untranslatedOctets, "PreCanonicalBytes", writer);
			}
			WriteBytes(_octets, "CanonicalBytes", writer);
			writer.WriteStartElement("Hash");
			writer.WriteElementString("Length", _hash.Length.ToString(CultureInfo.InvariantCulture));
			writer.WriteElementString("Value", _hash);
			writer.WriteEndElement();
			writer.WriteEndElement();
		}

		private void WriteBytes(byte[] bytes, string startElementName, XmlWriter writer)
		{
			writer.WriteStartElement(startElementName);
			writer.WriteElementString("Length", bytes.Length.ToString(CultureInfo.InvariantCulture));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in bytes)
			{
				stringBuilder.Append(hexDigits[((int)b / 16) & 0xF]);
				stringBuilder.Append(hexDigits[b & 0xF]);
			}
			writer.WriteElementString("HexBytes", stringBuilder.ToString());
			writer.WriteElementString("Encoding.UTF8", Encoding.UTF8.GetString(bytes));
			writer.WriteEndElement();
		}
	}
}
