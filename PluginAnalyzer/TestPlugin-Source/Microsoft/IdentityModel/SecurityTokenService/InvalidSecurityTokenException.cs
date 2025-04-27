using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidSecurityTokenException : RequestException
	{
		public InvalidSecurityTokenException()
			: base(SR.GetString("ID2085"))
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
