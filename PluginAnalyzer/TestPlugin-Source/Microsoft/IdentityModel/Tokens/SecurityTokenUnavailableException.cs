using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class SecurityTokenUnavailableException : Exception
	{
		public SecurityTokenUnavailableException()
			: base(SR.GetString("ID3255"))
		{
		}

		public SecurityTokenUnavailableException(string message)
			: base(message)
		{
		}

		public SecurityTokenUnavailableException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected SecurityTokenUnavailableException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
