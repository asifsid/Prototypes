using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class AzureSchedulerAdapterConstants
	{
		public const string schedulerJobType = "storageQueue";

		public const string TextAnalyticsKeyInGeoDB = "AzureMLTextAnalytics1";

		public const string TextAnalyticsKeyInGeoDBV3 = "AzureMLTextAnalyticsV3";

		public static readonly string[] TextAnalyticsPropsInGeoDB = new string[4] { "ApplicationId", "SubscriptionId", "ApplicationSecret", "ApplicationInfo" };

		public const string RDFESchedulerHostName = "management.core.windows.net";

		public const string ARMSchedulerHostName = "management.azure.com";
	}
}
