using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class AzureSchedulerProperties
	{
		public string Url { get; set; }

		public string AADAppTenantId { get; set; }

		public string AADAppId { get; set; }

		public string AADAppPassword { get; set; }

		public string Sku { get; set; }

		public string ApiVersion { get; set; }

		public string Location { get; set; }

		public string SAS { get; set; }

		public string QueueName { get; set; }

		public string StorageAccount { get; set; }
	}
}
