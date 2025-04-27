using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Solution.Common
{
	[Serializable]
	[ComVisible(true)]
	public class CrmSecurityException : CrmException
	{
		public CrmSecurityException(string message)
			: base(message, -2147220906)
		{
		}

		public CrmSecurityException(string message, int errorCode)
			: base(message, errorCode)
		{
		}

		public CrmSecurityException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public CrmSecurityException(string message, Exception innerException, int errorCode)
			: base(message, innerException, errorCode)
		{
		}

		protected CrmSecurityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
