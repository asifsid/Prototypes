using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class UnableToRenewException : RequestException
	{
		public UnableToRenewException()
			: base(SR.GetString("ID2090"))
		{
		}

		public UnableToRenewException(string message)
			: base(message)
		{
		}

		public UnableToRenewException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UnableToRenewException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
