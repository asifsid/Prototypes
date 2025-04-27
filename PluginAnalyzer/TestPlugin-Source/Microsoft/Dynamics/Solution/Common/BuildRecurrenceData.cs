using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class BuildRecurrenceData
	{
		public string startTime { get; set; }

		public RecurrenceData recurrence { get; set; }
	}
}
