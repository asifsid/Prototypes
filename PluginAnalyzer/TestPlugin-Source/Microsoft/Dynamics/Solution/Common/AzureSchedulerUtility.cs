using System;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Localization;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class AzureSchedulerUtility
	{
		public static AzureSchedulerType GetSchedulerType(string schedulerUrl)
		{
			if (Uri.TryCreate(schedulerUrl, UriKind.RelativeOrAbsolute, out var result))
			{
				return result.Host switch
				{
					"management.core.windows.net" => AzureSchedulerType.RDFEBased, 
					"management.azure.com" => AzureSchedulerType.ARMBased, 
					_ => throw new CrmException("scheduler type not supported"), 
				};
			}
			throw new CrmException($"schedulerUrl = {schedulerUrl} not valid URI");
		}

		public static void ProcessSchedulerError(Exception exception, IPluginContext context)
		{
			WebException ex = exception as WebException;
			string format = string.Empty;
			if (ex != null)
			{
				HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
				if (httpWebResponse != null)
				{
					format = AdapterUtility.ReadResponseStream(httpWebResponse.GetResponseStream());
				}
			}
			TelemetryUtilities.LogError(context, -2147084653, "Scheduler Error : {0}", format);
			TelemetryUtilities.LogErrorAndThrowException(context, -2147084653, Labels.TextAnalyticsAzureSchedulerError, exception);
		}

		public static bool IsIgnoreSchedulerInitTestHookEnabled(IPluginContext context)
		{
			int num = 0;
			context.DeveloperTestSettings.TryGet<int>("ignoreSchedulerInitialization", ref num);
			return num == 1;
		}
	}
}
