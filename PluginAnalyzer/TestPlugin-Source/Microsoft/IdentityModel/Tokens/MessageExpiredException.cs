using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class MessageExpiredException : Exception
	{
		public MessageExpiredException()
			: base(SR.GetString("ID3256"))
		{
		}

		public MessageExpiredException(string message)
			: base(message)
		{
		}

		public MessageExpiredException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected MessageExpiredException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
