using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Dynamics.Solution.Localization;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public class CrmInvalidOperationException : CrmException
	{
		public CrmInvalidOperationException()
			: base(Labels.InvalidOperation, -2147220933)
		{
		}

		public CrmInvalidOperationException(string message)
			: base(message, new InvalidOperationException(message))
		{
		}

		public CrmInvalidOperationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected CrmInvalidOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
