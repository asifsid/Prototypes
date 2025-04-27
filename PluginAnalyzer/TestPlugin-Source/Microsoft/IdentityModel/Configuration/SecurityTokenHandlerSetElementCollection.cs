using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ConfigurationCollection(typeof(SecurityTokenHandlerElementCollection), AddItemName = "securityTokenHandlers", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[ComVisible(true)]
	public class SecurityTokenHandlerSetElementCollection : ConfigurationElementCollection
	{
		public bool IsConfigured => base.Count > 0;

		protected override ConfigurationElement CreateNewElement()
		{
			return new SecurityTokenHandlerElementCollection();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SecurityTokenHandlerElementCollection)element).Name;
		}
	}
}
