using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xrm.Kernel.Contracts.ExternalIntegration;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class AdapterUtility
	{
		public const string SampleValueForTestHook = "SampleValueUsedByTestHook";

		public static string ReadResponseStream(Stream responseStream)
		{
			if (responseStream == null)
			{
				return null;
			}
			using StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
			return streamReader.ReadToEnd();
		}

		public static IExternalIntegrationConfigSettings RetrievePropertiesFromGeoDB(string keyName, IPluginContext context)
		{
			return context.ExternalIntegrationSettings.GetConfigSettings(keyName);
		}

		public static string GetAbsoluteCrmOrgUri(IPluginContext context)
		{
			return GetCrmOrgUri(context).AbsoluteUri.TrimEnd('/');
		}

		public static Uri GetCrmOrgUri(IPluginContext context)
		{
			return context.OrganizationEndPointService.get_WebApplicationUri();
		}

		public static bool IsOnPremTestHookEnabled(IPluginContext context)
		{
			int num = default(int);
			context.DeveloperTestSettings.TryGet<int>("AzureMLAnalyticsOnPremiseAllowed", ref num);
			return num == 1;
		}
	}
}
