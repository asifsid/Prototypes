using System.Collections;
using System.Xml;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class DictionaryTraceRecord : TraceRecord
	{
		private IDictionary _dictionary;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/DictionaryTraceRecord";

		public DictionaryTraceRecord(IDictionary dictionary)
		{
			_dictionary = dictionary;
		}

		public override void WriteTo(XmlWriter xml)
		{
			if (_dictionary == null)
			{
				return;
			}
			foreach (object key in _dictionary.Keys)
			{
				xml.WriteElementString(key.ToString(), (_dictionary[key] == null) ? string.Empty : _dictionary[key].ToString());
			}
		}
	}
}
