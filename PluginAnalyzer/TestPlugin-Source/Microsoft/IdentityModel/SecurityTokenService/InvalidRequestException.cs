using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidRequestException : RequestException
	{
		public InvalidRequestException()
			: base(SR.GetString("ID2005"))
		{
		}

		public InvalidRequestException(string message)
			: base(message)
		{
		}

		public InvalidRequestException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected InvalidRequestException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
