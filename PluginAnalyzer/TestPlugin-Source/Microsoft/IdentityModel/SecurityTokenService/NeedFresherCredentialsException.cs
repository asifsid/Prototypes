using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class NeedFresherCredentialsException : RequestException
	{
		public NeedFresherCredentialsException()
			: base(SR.GetString("ID2095"))
		{
		}

		public NeedFresherCredentialsException(string message)
			: base(message)
		{
		}

		public NeedFresherCredentialsException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected NeedFresherCredentialsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
