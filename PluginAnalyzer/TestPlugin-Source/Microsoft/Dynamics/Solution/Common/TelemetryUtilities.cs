using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class TelemetryUtilities
	{
		public static void LogInvalidParameterAndThrowException(IPluginContext context, string parameterName)
		{
			LogErrorAndThrowException(context, -2147220989, Labels.InvalidArgument, string.Format(CultureInfo.InvariantCulture, "Invalid parameter '{0}'", parameterName), null, throwException: true, 2);
		}

		public static void LogError(IPluginContext context, int errorCode, string errorMessage, string format, params object[] args)
		{
			LogErrorAndThrowException(context, errorCode, errorMessage, format, null, throwException: false, 2);
		}

		public static void LogErrorAndThrowException(IPluginContext context, int crmErrorCode, string crmErrorMessage, Exception innerException)
		{
			LogErrorAndThrowException(context, crmErrorCode, crmErrorMessage, null, innerException, throwException: true, 2);
		}

		public static void LogErrorAndThrowException(IPluginContext context, int crmErrorCode, string crmErrorMessage, string format, params object[] args)
		{
			LogErrorAndThrowException(context, crmErrorCode, crmErrorMessage, string.Format(CultureInfo.InvariantCulture, format, args), null, throwException: true, 2);
		}

		public static void LogEvent(CrmMlSyncEvent crmMlSyncEvent, IPluginContext context, string message = "")
		{
			TelemetryRuntimeContext runtimeContext = GetRuntimeContext(context, 2);
			MlEventSource.Instance.LogEvent((int)crmMlSyncEvent, crmMlSyncEvent.ToString(), runtimeContext.OrgId.ToString(), runtimeContext.CallingMethod, message);
		}

		public static void LogNumericEvent(CrmMlSyncEvent crmMlSyncEvent, int numericValue, Guid? eventCorrelationId, IPluginContext context)
		{
			TelemetryRuntimeContext runtimeContext = GetRuntimeContext(context, 2);
			MlEventSource.Instance.LogNumericEvent((int)crmMlSyncEvent, crmMlSyncEvent.ToString(), numericValue.ToString(), (!eventCorrelationId.HasValue) ? string.Empty : eventCorrelationId.ToString(), runtimeContext.OrgId.ToString(), runtimeContext.CallingMethod);
		}

		public static void LogNumericEvent(CrmMlSyncEvent crmMlSyncEvent, long numericValue, Guid? eventCorrelationId, IPluginContext context)
		{
			TelemetryRuntimeContext runtimeContext = GetRuntimeContext(context, 2);
			MlEventSource.Instance.LogNumericEvent((int)crmMlSyncEvent, crmMlSyncEvent.ToString(), numericValue.ToString(), (!eventCorrelationId.HasValue) ? string.Empty : eventCorrelationId.ToString(), runtimeContext.OrgId.ToString(), runtimeContext.CallingMethod);
		}

		public static void LogNumericEvent(CrmMlSyncEvent crmMlSyncEvent, double numericValue, Guid? eventCorrelationId, IPluginContext context)
		{
			TelemetryRuntimeContext runtimeContext = GetRuntimeContext(context, 2);
			MlEventSource.Instance.LogNumericEvent((int)crmMlSyncEvent, crmMlSyncEvent.ToString(), numericValue.ToString(), (!eventCorrelationId.HasValue) ? string.Empty : eventCorrelationId.ToString(), runtimeContext.OrgId.ToString(), runtimeContext.CallingMethod);
		}

		public static void LogNumericEvent(CrmMlSyncEvent crmMlSyncEvent, decimal numericValue, Guid? eventCorrelationId, IPluginContext context)
		{
			TelemetryRuntimeContext runtimeContext = GetRuntimeContext(context, 2);
			MlEventSource.Instance.LogNumericEvent((int)crmMlSyncEvent, crmMlSyncEvent.ToString(), numericValue.ToString(), (!eventCorrelationId.HasValue) ? string.Empty : eventCorrelationId.ToString(), runtimeContext.OrgId.ToString(), runtimeContext.CallingMethod);
		}

		public static void LogErrorAndThrowException(IPluginContext context, int crmErrorCode, string crmErrorMessage, string message = null, Exception innerException = null, bool throwException = true, int skipFrames = 1)
		{
			Exceptions.ThrowIfNull(context, "context");
			TelemetryRuntimeContext runtimeContext = GetRuntimeContext(context, ++skipFrames);
			StringBuilder stringBuilder = new StringBuilder();
			if (message != null)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "Message: {0}", message);
			}
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, ", Organization Id: {0}, Calling method: {1}, Error details: '{2}'", runtimeContext.OrgId, runtimeContext.CallingMethod, crmErrorMessage);
			Exception ex = innerException;
			string message2 = stringBuilder.ToString();
			while (ex != null)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, ", InnerException: '{0}'", ex);
				ex = ex.InnerException;
			}
			string errorDetails = stringBuilder.ToString();
			MlEventSource.Instance.LogError(runtimeContext.OrgId.ToString(), runtimeContext.CallingMethod, crmErrorCode, errorDetails);
			if (!throwException)
			{
				return;
			}
			if (innerException != null)
			{
				throw new CrmException(message2, innerException, crmErrorCode);
			}
			throw new CrmException(message2, crmErrorCode);
		}

		private static TelemetryRuntimeContext GetRuntimeContext(IPluginContext context, int skipFrames)
		{
			TelemetryRuntimeContext telemetryRuntimeContext = new TelemetryRuntimeContext();
			try
			{
				telemetryRuntimeContext.OrgId = ((context != null) ? ((IExecutionContext)context.PluginExecutionContext).get_OrganizationId() : Guid.Empty);
				telemetryRuntimeContext.CallingMethod = string.Empty;
				MethodBase method = new StackTrace().GetFrame(skipFrames).GetMethod();
				string text2 = (telemetryRuntimeContext.CallingMethod = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", method.DeclaringType, method.Name));
			}
			catch
			{
			}
			return telemetryRuntimeContext;
		}
	}
}
