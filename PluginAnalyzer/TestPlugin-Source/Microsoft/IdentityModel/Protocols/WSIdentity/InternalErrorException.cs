using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class InternalErrorException : Exception
	{
		public InternalErrorException()
			: base(SR.GetString("ID2101"))
		{
		}

		public InternalErrorException(string message)
			: base(message)
		{
		}

		public InternalErrorException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InternalErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
