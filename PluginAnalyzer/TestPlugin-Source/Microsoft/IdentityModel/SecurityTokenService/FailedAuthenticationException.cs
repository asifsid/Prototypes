using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class FailedAuthenticationException : RequestException
	{
		public FailedAuthenticationException()
			: base(SR.GetString("ID2007"))
		{
		}

		public FailedAuthenticationException(string message)
			: base(message)
		{
		}

		public FailedAuthenticationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected FailedAuthenticationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
