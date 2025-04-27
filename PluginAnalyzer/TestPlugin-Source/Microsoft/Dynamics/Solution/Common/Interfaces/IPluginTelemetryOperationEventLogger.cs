using System;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common.Interfaces
{
	[ComVisible(true)]
	public interface IPluginTelemetryOperationEventLogger
	{
		void Execute(XrmTelemetryActivityType activityType, Action action);

		TResult Execute<TResult>(XrmTelemetryActivityType activityType, Func<TResult> func);
	}
}
