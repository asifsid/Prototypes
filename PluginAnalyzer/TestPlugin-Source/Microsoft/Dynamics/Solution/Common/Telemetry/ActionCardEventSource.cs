using System;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.Telemetry
{
	[EventSource(Guid = "1c08ece3-cd31-4961-a896-3997061fb744")]
	[ComVisible(true)]
	public class ActionCardEventSource : EventSource
	{
		internal enum EventSource
		{
			ActionCardInformational = 1,
			ActionCardWarning,
			ActionCardError,
			ActionCardTimeTaken
		}

		private static readonly Lazy<ActionCardEventSource> LazyInstance = new Lazy<ActionCardEventSource>(() => new ActionCardEventSource());

		public static ActionCardEventSource Instance => LazyInstance.Value;

		[Event(1, Level = EventLevel.Informational)]
		public void ActionCardInformational(string eventName, Guid organizationId, string callingMethod, string eventDetails, string EmailId, Guid userId)
		{
			if (IsEnabled())
			{
				WriteEvent(1, eventName, organizationId, callingMethod, eventDetails, EmailId, userId);
			}
		}

		[Event(2, Level = EventLevel.Warning)]
		public void ActionCardWarning(string eventName, Guid organizationId, string callingMethod, string eventDetails, Guid userId)
		{
			if (IsEnabled())
			{
				WriteEvent(2, eventName, organizationId, callingMethod, eventDetails, userId);
			}
		}

		[Event(3, Level = EventLevel.Error)]
		public void ActionCardError(Guid organizationId, string callingMethod, int errorCode, string eventDetails, string EmailId, Guid userId)
		{
			if (IsEnabled())
			{
				WriteEvent(3, organizationId, callingMethod, errorCode, eventDetails, EmailId, userId);
			}
		}

		[Event(4, Level = EventLevel.Informational)]
		public void ActionCardTimeTaken(string eventName, Guid organizationId, string callingMethod, long timeTaken, string eventDetails, string EmailId, Guid userId)
		{
			if (IsEnabled())
			{
				WriteEvent(4, eventName, organizationId, callingMethod, timeTaken, eventDetails, EmailId, userId);
			}
		}
	}
}
