using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.Interfaces
{
	[ComVisible(true)]
	public interface IPluginTelemetry
	{
		IPluginTelemetryOperationEventLogger OperationEventLogger { get; }

		IPluginTelemetryTraceEventLogger Logger { get; }

		TelemetryMode LoggingMode { get; set; }
	}
}
