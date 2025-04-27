using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Dynamics.Solution.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class PluginInternalLogger : IPluginTelemetryLogger, IPluginTelemetryOperationEventLogger, IPluginTelemetryTraceEventLogger
	{
		private TelemetryMode mode;

		private ILogger logger = null;

		private const string separator = ": ";

		public TelemetryMode LoggingMode
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;
			}
		}

		public PluginInternalLogger(ILogger loggerInstance)
		{
			logger = loggerInstance;
		}

		public TResult Execute<TResult>(XrmTelemetryActivityType activityType, Func<TResult> func)
		{
			if (mode == TelemetryMode.OperationEvents || mode == TelemetryMode.All)
			{
				return XrmTelemetryExtensions.Execute<TResult>(logger, activityType, func);
			}
			return func();
		}

		public void Execute(XrmTelemetryActivityType activityType, Action action)
		{
			if (mode == TelemetryMode.OperationEvents || mode == TelemetryMode.All)
			{
				XrmTelemetryExtensions.Execute(logger, activityType, action);
			}
			else
			{
				action();
			}
		}

		public void LogEvent(EventType eventType, string className, string methodName, string messagePayload, Dictionary<string, string> customProperties)
		{
			LogEvent(eventType, className, methodName, messagePayload);
			if (customProperties == null && customProperties.Keys.Count > 0)
			{
				XrmTelemetryContext.AddCustomProperties(customProperties);
			}
		}

		public void LogEvent(EventType eventType, string className, string methodName, string messagePayload, string customPropertyKey, string customPropertyValue)
		{
			LogEvent(eventType, className, methodName, messagePayload);
			if (!string.IsNullOrWhiteSpace(customPropertyKey) && !string.IsNullOrWhiteSpace(customPropertyValue))
			{
				XrmTelemetryContext.AddCustomProperty(customPropertyKey, customPropertyValue);
			}
		}

		public void LogEvent(EventType eventType, string className, string methodName, string messagePayload)
		{
			if (mode == TelemetryMode.Disabled || mode == TelemetryMode.OperationEvents || mode == TelemetryMode.Usage)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(className))
			{
				stringBuilder.Append(className).Append(": ");
			}
			if (!string.IsNullOrWhiteSpace(methodName))
			{
				stringBuilder.Append(methodName).Append(": ");
			}
			if (!string.IsNullOrWhiteSpace(messagePayload))
			{
				stringBuilder.Append(messagePayload).Append(": ");
			}
			switch (eventType)
			{
			case EventType.Info:
				if (mode == TelemetryMode.Info || mode == TelemetryMode.All)
				{
					LoggerExtensions.LogInformation(logger, stringBuilder.ToString(), Array.Empty<object>());
				}
				break;
			case EventType.Error:
				if (mode == TelemetryMode.Error || mode == TelemetryMode.All)
				{
					LoggerExtensions.LogError(logger, stringBuilder.ToString(), Array.Empty<object>());
				}
				break;
			case EventType.Warning:
				if (mode == TelemetryMode.Warning || mode == TelemetryMode.All)
				{
					LoggerExtensions.LogWarning(logger, stringBuilder.ToString(), Array.Empty<object>());
				}
				break;
			case EventType.Debug:
				if (mode == TelemetryMode.Debug || mode == TelemetryMode.All)
				{
					LoggerExtensions.LogDebug(logger, stringBuilder.ToString(), Array.Empty<object>());
				}
				break;
			case EventType.Trace:
				if (mode == TelemetryMode.Trace || mode == TelemetryMode.All)
				{
					LoggerExtensions.LogTrace(logger, stringBuilder.ToString(), Array.Empty<object>());
				}
				break;
			case EventType.Critical:
				if (mode == TelemetryMode.Critical || mode == TelemetryMode.All)
				{
					LoggerExtensions.LogCritical(logger, stringBuilder.ToString(), Array.Empty<object>());
				}
				break;
			}
		}

		public void LogEvent(EventType eventType, string message)
		{
			LogEvent(eventType, string.Empty, string.Empty, message);
		}
	}
}
