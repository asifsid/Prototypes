using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Dynamics.Solution.Localization;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public sealed class CrmInvalidRecurrenceRuleException : CrmException
	{
		public CrmInvalidRecurrenceRuleException()
			: base(Labels.InvalidRecurrenceRule, -2147220922)
		{
		}

		public CrmInvalidRecurrenceRuleException(string message)
			: base(message, -2147220922)
		{
		}

		public CrmInvalidRecurrenceRuleException(string message, Exception exception)
			: base(message, exception, -2147220922)
		{
		}

		private CrmInvalidRecurrenceRuleException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
