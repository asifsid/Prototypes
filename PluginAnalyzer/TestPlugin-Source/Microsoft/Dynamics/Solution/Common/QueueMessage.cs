using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class QueueMessage
	{
		public string storageAccount { get; set; }

		public string queueName { get; set; }

		public string sasToken { get; set; }

		public string message { get; set; }
	}
}
