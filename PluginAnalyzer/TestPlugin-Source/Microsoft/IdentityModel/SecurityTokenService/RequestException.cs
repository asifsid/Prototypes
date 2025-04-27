using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public abstract class RequestException : Exception
	{
		protected RequestException()
		{
		}

		protected RequestException(string message)
			: base(message)
		{
		}

		protected RequestException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RequestException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
