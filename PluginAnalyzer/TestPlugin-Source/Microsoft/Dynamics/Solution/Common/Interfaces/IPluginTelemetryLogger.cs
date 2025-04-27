using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.Interfaces
{
	[ComVisible(true)]
	public interface IPluginTelemetryLogger : IPluginTelemetryOperationEventLogger, IPluginTelemetryTraceEventLogger
	{
		TelemetryMode LoggingMode { get; set; }
	}
}
