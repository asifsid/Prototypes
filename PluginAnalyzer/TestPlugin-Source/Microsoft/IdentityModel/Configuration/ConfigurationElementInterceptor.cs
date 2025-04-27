using System.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class ConfigurationElementInterceptor : ConfigurationElement
	{
		private XmlDocument elementXml;

		public XmlElement ElementAsXml
		{
			get
			{
				if (elementXml != null)
				{
					return elementXml.DocumentElement;
				}
				return null;
			}
		}

		public XmlNodeList ChildNodes
		{
			get
			{
				if (elementXml != null && ElementAsXml.ChildNodes.Count != 0)
				{
					return ElementAsXml.ChildNodes;
				}
				return null;
			}
		}

		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			elementXml = new XmlDocument();
			elementXml.LoadXml(reader.ReadOuterXml());
			using XmlReader xmlReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(elementXml.DocumentElement.OuterXml), XmlDictionaryReaderQuotas.Max);
			xmlReader.Read();
			base.DeserializeElement(xmlReader, serializeCollectionKey);
		}

		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			return true;
		}

		protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
		{
			return true;
		}

		protected override void Reset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
			Reset((ConfigurationElementInterceptor)parentElement);
		}

		private void Reset(ConfigurationElementInterceptor parentElement)
		{
			elementXml = parentElement.elementXml;
		}
	}
}
