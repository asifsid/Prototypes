using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidSecurityException : Exception
	{
		public InvalidSecurityException()
			: base(SR.GetString("ID3252"))
		{
		}

		public InvalidSecurityException(string message)
			: base(message)
		{
		}

		public InvalidSecurityException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InvalidSecurityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
