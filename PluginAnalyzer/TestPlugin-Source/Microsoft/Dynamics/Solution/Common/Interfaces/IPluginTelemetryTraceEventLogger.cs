using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.Interfaces
{
	[ComVisible(true)]
	public interface IPluginTelemetryTraceEventLogger
	{
		void LogEvent(EventType eventType, string className, string methodName, string messagePayload, string customPropertyKey, string customPropertyValue);

		void LogEvent(EventType eventType, string className, string methodName, string messagePayload, Dictionary<string, string> customProperties);

		void LogEvent(EventType eventType, string className, string methodName, string messagePayload);

		void LogEvent(EventType info, string message);
	}
}
