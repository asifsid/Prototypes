using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public class CrmArgumentNullException : CrmException
	{
		public CrmArgumentNullException(string message)
			: base(message, new ArgumentNullException(message), -2147220989)
		{
		}

		public CrmArgumentNullException(string message, int errorCode)
			: base(message, new ArgumentNullException(message), errorCode)
		{
		}

		public CrmArgumentNullException(string parameterName, string message)
			: base(message, new ArgumentNullException(parameterName, message), -2147220989)
		{
		}

		public CrmArgumentNullException(string parameterName, string message, int errorCode)
			: base(message, new ArgumentNullException(parameterName, message), errorCode)
		{
		}

		public CrmArgumentNullException(string message, Exception innerException)
			: base(message, innerException, -2147220989)
		{
		}

		public CrmArgumentNullException(string message, Exception innerException, int errorCode)
			: base(message, innerException, errorCode)
		{
		}

		protected CrmArgumentNullException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
