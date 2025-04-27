using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	internal class LabelResource
	{
		private const string MainLabelFileName = "Labels";

		private const string LocalizedResourcesSuffix = "resx.xml";

		private const string ResourceUniqueNamePrefix = "PluginResources";

		private static readonly ConcurrentDictionary<int, XmlDocument> ResourcesPerLocale = new ConcurrentDictionary<int, XmlDocument>();

		private IPluginContext pluginContext;

		private string solutionName;

		internal LabelResource(IPluginContext pluginContext, string solutionName)
		{
			this.pluginContext = pluginContext;
			this.solutionName = solutionName;
		}

		internal XmlDocument GetResource(int cultureId)
		{
			XmlDocument xmlDocument;
			if (ResourcesPerLocale.ContainsKey(cultureId))
			{
				xmlDocument = ResourcesPerLocale[cultureId];
			}
			else
			{
				xmlDocument = GetResourceFromServer(cultureId);
				ResourcesPerLocale[cultureId] = xmlDocument;
			}
			return xmlDocument;
		}

		private string GetResourceUniqueName(int cultureId)
		{
			string resourceFileName = GetResourceFileName(cultureId);
			return string.Format("{0}/{1}/{2}", solutionName, "PluginResources", resourceFileName);
		}

		private string GetResourceFileName(int cultureId)
		{
			CultureInfo cultureInfo = new CultureInfo(cultureId);
			string name = cultureInfo.Name;
			name = name.Replace('-', '_');
			return string.Format("{0}.{1}.{2}", "Labels", name, "resx.xml");
		}

		private XmlDocument GetResourceFromServer(int cultureId)
		{
			byte[] array = RetrieveResourceContent(cultureId);
			if (array != null)
			{
				return BuildXmlDocumentFromResourceContent(array);
			}
			return null;
		}

		private static XmlDocument BuildXmlDocumentFromResourceContent(byte[] bytes)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.XmlResolver = null;
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using StreamReader txtReader = new StreamReader(stream);
				xmlDocument.Load(txtReader);
			}
			return xmlDocument;
		}

		private byte[] RetrieveResourceContent(int localeId)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			string resourceUniqueName = GetResourceUniqueName(localeId);
			QueryByAttribute val = new QueryByAttribute("webresource");
			val.set_ColumnSet(new ColumnSet(new string[1] { "content" }));
			val.AddAttributeValue("name", (object)resourceUniqueName);
			val.set_TopCount((int?)1);
			EntityCollection val2 = pluginContext.OrganizationService.RetrieveMultiple((QueryBase)(object)val);
			WebResource webResource = ((Collection<Entity>)(object)val2.get_Entities())[0].ToEntity<WebResource>();
			byte[] result = null;
			if (webResource != null)
			{
				result = Convert.FromBase64String(webResource.Content);
			}
			return result;
		}
	}
}
