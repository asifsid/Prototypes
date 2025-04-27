using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class RecurrenceData
	{
		public string frequency { get; set; }

		public string endTime { get; set; }

		public int interval { get; set; }

		public ScheduleData schedule { get; set; }
	}
}
