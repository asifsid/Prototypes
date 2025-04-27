using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidInputException : Exception
	{
		public InvalidInputException()
			: base(SR.GetString("ID2103"))
		{
		}

		public InvalidInputException(string message)
			: base(message)
		{
		}

		public InvalidInputException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InvalidInputException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
