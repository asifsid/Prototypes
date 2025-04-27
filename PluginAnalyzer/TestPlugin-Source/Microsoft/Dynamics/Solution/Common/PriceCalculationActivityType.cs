using System.Runtime.InteropServices;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class PriceCalculationActivityType : XrmTelemetrySingletonActivityType<PriceCalculationActivityType>
	{
		public PriceCalculationActivityType()
			: base(SolutionCommonExecutionTelemetryActivityTypesName.PriceCalculation, true, true)
		{
		}
	}
}
