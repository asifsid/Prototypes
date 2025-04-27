using System;
using Microsoft.Dynamics.Solution.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common
{
	internal sealed class PluginTelemetryLogger : IPluginTelemetry
	{
		private const string logSourceName = "Microsoft.Dynamics.Solution.Common.PluginExecutionTelemetry";

		private IPluginTelemetryTraceEventLogger logger = null;

		private IPluginTelemetryOperationEventLogger operationLogger = null;

		private static readonly Lazy<PluginTelemetryLogger> pluginTelemetryLogger = new Lazy<PluginTelemetryLogger>(() => new PluginTelemetryLogger());

		private TelemetryMode mode;

		private PluginInternalLogger pluginInternalLogger;

		private ILogger innerlogger;

		public IPluginTelemetryTraceEventLogger Logger => logger;

		public IPluginTelemetryOperationEventLogger OperationEventLogger => operationLogger;

		public static PluginTelemetryLogger Instance => pluginTelemetryLogger.Value;

		public TelemetryMode LoggingMode
		{
			get
			{
				if (pluginInternalLogger != null)
				{
					return pluginInternalLogger.LoggingMode;
				}
				return TelemetryMode.Disabled;
			}
			set
			{
				if (pluginInternalLogger != null)
				{
					pluginInternalLogger.LoggingMode = value;
				}
			}
		}

		private PluginTelemetryLogger()
		{
			innerlogger = XrmLoggerFactory.get_LoggerFactory().CreateLogger("Microsoft.Dynamics.Solution.Common.PluginExecutionTelemetry");
			pluginInternalLogger = new PluginInternalLogger(innerlogger);
			logger = pluginInternalLogger;
			operationLogger = pluginInternalLogger;
		}
	}
}
