using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public enum ActionCardPriority
	{
		Urgent = 1000,
		High = 800,
		Medium = 600,
		Low = 400,
		NotUrgent = 200
	}
}
