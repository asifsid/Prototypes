using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class FailedAuthenticationException : Exception
	{
		public FailedAuthenticationException()
			: base(SR.GetString("ID3242"))
		{
		}

		public FailedAuthenticationException(string message)
			: base(message)
		{
		}

		public FailedAuthenticationException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected FailedAuthenticationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
