using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidTimeRangeException : RequestException
	{
		public InvalidTimeRangeException()
			: base(SR.GetString("ID2088"))
		{
		}

		public InvalidTimeRangeException(string message)
			: base(message)
		{
		}

		public InvalidTimeRangeException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InvalidTimeRangeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
