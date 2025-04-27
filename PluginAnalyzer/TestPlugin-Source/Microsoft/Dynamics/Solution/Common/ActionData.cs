using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class ActionData
	{
		public string Type { get; set; }

		public QueueMessage queueMessage { get; set; }
	}
}
