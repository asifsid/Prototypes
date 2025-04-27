using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidSecurityTokenException : Exception
	{
		public InvalidSecurityTokenException()
			: base(SR.GetString("ID3253"))
		{
		}

		public InvalidSecurityTokenException(string message)
			: base(message)
		{
		}

		public InvalidSecurityTokenException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InvalidSecurityTokenException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
