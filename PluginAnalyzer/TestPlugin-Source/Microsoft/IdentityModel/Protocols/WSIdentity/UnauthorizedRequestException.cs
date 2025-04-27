using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class UnauthorizedRequestException : Exception
	{
		public UnauthorizedRequestException()
			: base(SR.GetString("ID2102"))
		{
		}

		public UnauthorizedRequestException(string message)
			: base(message)
		{
		}

		public UnauthorizedRequestException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UnauthorizedRequestException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
