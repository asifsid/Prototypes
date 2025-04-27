using System.Runtime.InteropServices;

namespace Microsoft.Crm.Common.ObjectModel
{
	[ComVisible(true)]
	public enum ActivityState
	{
		Open,
		Closed,
		Cancelled,
		Scheduled
	}
}
