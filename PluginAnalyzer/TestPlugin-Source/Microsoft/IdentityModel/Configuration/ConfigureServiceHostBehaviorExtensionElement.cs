using System;
using System.Runtime.InteropServices;
using System.ServiceModel.Configuration;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public class ConfigureServiceHostBehaviorExtensionElement : BehaviorExtensionElement
	{
		private string _serviceName = "";

		public override Type BehaviorType => typeof(ConfigureServiceHostServiceBehavior);

		protected override object CreateBehavior()
		{
			return new ConfigureServiceHostServiceBehavior(_serviceName);
		}

		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(reader.ReadOuterXml());
			XmlAttribute xmlAttribute = xmlDocument.DocumentElement.Attributes["name"];
			if (xmlAttribute != null)
			{
				_serviceName = xmlAttribute.Value;
			}
			using XmlReader xmlReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(xmlDocument.DocumentElement.OuterXml), XmlDictionaryReaderQuotas.Max);
			xmlReader.Read();
			base.DeserializeElement(xmlReader, serializeCollectionKey);
		}

		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			return true;
		}
	}
}
