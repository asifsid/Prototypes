using System;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[EventSource(Guid = "5ceeb2d9-858a-49fb-a0f1-e6d83c7f112d")]
	[ComVisible(true)]
	public sealed class MlEventSource : EventSource
	{
		private enum EventSourceEvent
		{
			Error = 1,
			Event,
			NumericEvent
		}

		private static readonly Lazy<MlEventSource> LazyInstance = new Lazy<MlEventSource>(() => new MlEventSource());

		public static MlEventSource Instance => LazyInstance.Value;

		[Event(2)]
		public void LogEvent(int eventId, string eventName, string orgId, string callingMethod, string eventDetails)
		{
			if (IsEnabled())
			{
				WriteEvent(2, eventName, orgId, callingMethod, eventDetails);
			}
		}

		[Event(3)]
		public void LogNumericEvent(int eventId, string eventName, string numericValue, string eventCorrelationId, string orgId, string callingMethod)
		{
			if (IsEnabled())
			{
				WriteEvent(3, eventName, numericValue, eventCorrelationId, orgId, callingMethod);
			}
		}

		[Event(1)]
		public void LogError(string orgId, string callingMethod, int crmErrorCode, string errorDetails)
		{
			if (IsEnabled())
			{
				WriteEvent(1, orgId, callingMethod, crmErrorCode, errorDetails);
			}
		}
	}
}
