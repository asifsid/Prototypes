using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Dynamics.Solution.Common.Interfaces;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class TelemetryMessageBuilder
	{
		private const string DELIMITER = " ";

		private const string INFORMATION = "Info";

		private const string ERROR = "Error";

		private const string WARNING = "Warning";

		private const string DATETIMEFORMAT = "yyyy-MM-dd HH:mm:ss.fffffff";

		private const int MAXLOGCAPACITY = 30000;

		private StringBuilder messageLog = new StringBuilder();

		private IPluginTelemetryTraceEventLogger logger;

		private string currentTimestamp => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffffff") + " ";

		public TelemetryMessageBuilder(IPluginTelemetryTraceEventLogger logger)
		{
			this.logger = logger;
		}

		public void AppendError(string message)
		{
			AppendInternal(message, "Error");
		}

		public void AppendInformation(string message)
		{
			AppendInternal(message, "Info");
		}

		public void AppendWarning(string message)
		{
			AppendInternal(message, "Warning");
		}

		private void AppendInternal(string message, string eventType)
		{
			messageLog.Append(currentTimestamp);
			messageLog.Append(" ");
			messageLog.Append(eventType);
			messageLog.Append(" ");
			messageLog.Append(message);
			messageLog.AppendLine();
			TryAutoFlush();
		}

		public string Flush()
		{
			if (messageLog == null || messageLog.Length == 0)
			{
				return string.Empty;
			}
			string result = messageLog.ToString();
			messageLog.Clear();
			return result;
		}

		private void TryAutoFlush()
		{
			if (messageLog != null && messageLog.Length != 0 && messageLog.Length >= 30000)
			{
				logger.LogEvent(EventType.Info, messageLog.ToString());
				messageLog.Clear();
			}
		}
	}
}
