using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class RequestFailedException : RequestException
	{
		public RequestFailedException()
			: base(SR.GetString("ID2008"))
		{
		}

		public RequestFailedException(string message)
			: base(message)
		{
		}

		public RequestFailedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RequestFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
