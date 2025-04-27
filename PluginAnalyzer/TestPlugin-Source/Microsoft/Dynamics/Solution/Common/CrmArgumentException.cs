using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public class CrmArgumentException : CrmException
	{
		public CrmArgumentException(string message, string parameterName)
			: base(message, new ArgumentException(message, parameterName), -2147220989)
		{
		}

		public CrmArgumentException(string message, string parameterName, int errorCode)
			: base(message, new ArgumentException(message, parameterName), errorCode)
		{
		}

		public CrmArgumentException(string message, Exception innerException)
			: base(message, innerException, -2147220989)
		{
		}

		public CrmArgumentException(string message, Exception innerException, int errorCode)
			: base(message, innerException, errorCode)
		{
		}

		protected CrmArgumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
