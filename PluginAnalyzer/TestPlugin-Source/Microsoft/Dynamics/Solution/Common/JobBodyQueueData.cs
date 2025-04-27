using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class JobBodyQueueData
	{
		public ActionData action { get; set; }

		public RecurrenceData recurrence { get; set; }

		public string State { get; set; }

		public string startTime { get; set; }
	}
}
