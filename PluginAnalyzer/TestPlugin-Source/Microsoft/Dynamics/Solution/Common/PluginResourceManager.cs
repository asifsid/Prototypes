using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class PluginResourceManager
	{
		private const int nativeLocaleId = 1033;

		private string labelResourceFullName;

		private Assembly labelResourceAssembly;

		private ResourceManager nativeResourceManager = null;

		private ResourceManager NativeResourceManager
		{
			get
			{
				if (nativeResourceManager == null)
				{
					ResourceManager resourceManager = (nativeResourceManager = new ResourceManager(labelResourceFullName, labelResourceAssembly));
				}
				return nativeResourceManager;
			}
		}

		public string SolutionName { get; set; }

		public PluginResourceManager(string nativeResourceBaseName, Assembly nativeResourceAssembly)
		{
			labelResourceFullName = nativeResourceBaseName;
			labelResourceAssembly = nativeResourceAssembly;
		}

		public string GetString(string labelId, CultureInfo cultureInfo)
		{
			IPluginContext currentContext = PluginContextManager.GetCurrentContext();
			_ = string.Empty;
			if (currentContext != null)
			{
				try
				{
					CultureProvider cultureProvider = new CultureProvider(currentContext);
					LabelResource labelResource = new LabelResource(currentContext, SolutionName);
					return GetLabelFromCultureHierarchy(labelId, cultureProvider, labelResource);
				}
				catch
				{
					return NativeResourceManager.GetString(labelId, cultureInfo);
				}
			}
			return NativeResourceManager.GetString(labelId, cultureInfo);
		}

		private string GetLabelFromCultureHierarchy(string labelId, CultureProvider cultureProvider, LabelResource labelResource)
		{
			string result = string.Empty;
			foreach (int applicableCulture in cultureProvider.GetApplicableCultures())
			{
				if (applicableCulture != 1033)
				{
					XmlDocument resource = labelResource.GetResource(applicableCulture);
					if (resource != null)
					{
						XmlNode xmlNode = resource.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "./root/data[@name='{0}']/value", labelId));
						if (xmlNode != null)
						{
							result = xmlNode.InnerText;
							break;
						}
					}
					continue;
				}
				result = NativeResourceManager.GetString(labelId, null);
				break;
			}
			return result;
		}
	}
}
