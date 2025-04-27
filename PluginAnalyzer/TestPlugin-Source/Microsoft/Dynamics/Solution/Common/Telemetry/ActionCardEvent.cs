using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.Telemetry
{
	[ComVisible(true)]
	public enum ActionCardEvent
	{
		CreateCard,
		UpdateCard,
		SkippedActionCardGeneration
	}
}
