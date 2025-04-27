using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public class CrmArgumentOutOfRangeException : CrmException
	{
		public CrmArgumentOutOfRangeException(string parameterName, string message)
			: base(message, new ArgumentOutOfRangeException(parameterName, message), -2147220989)
		{
		}

		public CrmArgumentOutOfRangeException(string parameterName, string message, int errorCode)
			: base(message, new ArgumentOutOfRangeException(parameterName, message), errorCode)
		{
		}

		public CrmArgumentOutOfRangeException(string parameterName, object actualValue, string message)
			: base(message, new ArgumentOutOfRangeException(parameterName, actualValue, message), -2147220989)
		{
		}

		public CrmArgumentOutOfRangeException(string parameterName, object actualValue, string message, int errorCode)
			: base(message, new ArgumentOutOfRangeException(parameterName, actualValue, message), errorCode)
		{
		}

		public CrmArgumentOutOfRangeException(string message, Exception innerException)
			: base(message, innerException, -2147220989)
		{
		}

		public CrmArgumentOutOfRangeException(string message, Exception innerException, int errorCode)
			: base(message, innerException, errorCode)
		{
		}

		protected CrmArgumentOutOfRangeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
