using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.Dynamics.CRM.Common
{
	[ComVisible(true)]
	public static class XmlUtility
	{
		public static XmlDocument CreateXmlDocument(string xml)
		{
			if (string.IsNullOrEmpty(xml))
			{
				return new XmlDocument
				{
					XmlResolver = null
				};
			}
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreWhitespace = true;
			using StringReader input = new StringReader(xml);
			using XmlReader reader = XmlReader.Create(input, xmlReaderSettings);
			return CreateXmlDocument(reader);
		}

		public static XmlDocument CreateXmlDocument(XmlReader reader)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.XmlResolver = null;
			xmlDocument.Load(reader);
			reader.Close();
			return xmlDocument;
		}
	}
}
