using System.Runtime.InteropServices;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class BulkCreateActivityType : XrmTelemetrySingletonActivityType<BulkCreateActivityType>
	{
		public BulkCreateActivityType()
			: base(SolutionCommonExecutionTelemetryActivityTypesName.BulkCreate, true, true)
		{
		}
	}
}
