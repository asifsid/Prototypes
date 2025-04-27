using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class FailedRequiredClaimsException : Exception
	{
		public FailedRequiredClaimsException()
			: base(SR.GetString("ID2046"))
		{
		}

		public FailedRequiredClaimsException(string message)
			: base(message)
		{
		}

		public FailedRequiredClaimsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected FailedRequiredClaimsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
