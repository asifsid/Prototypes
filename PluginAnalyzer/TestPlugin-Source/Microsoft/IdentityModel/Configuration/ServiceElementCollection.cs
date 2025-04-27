using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ConfigurationCollection(typeof(ServiceElement), AddItemName = "service", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[ComVisible(true)]
	public class ServiceElementCollection : ConfigurationElementCollection
	{
		protected override bool ThrowOnDuplicate => false;

		public bool IsConfigured => base.Count > 0;

		protected override ConfigurationElement CreateNewElement()
		{
			return new ServiceElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("element");
			}
			ServiceElement serviceElement = element as ServiceElement;
			if (serviceElement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7013"));
			}
			return serviceElement.Name;
		}

		public ServiceElement GetElement(string name)
		{
			if (name == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("name");
			}
			ServiceElement serviceElement = BaseGet(name) as ServiceElement;
			if (!StringComparer.Ordinal.Equals(name, "") && serviceElement == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7012", name));
			}
			return serviceElement;
		}
	}
}
