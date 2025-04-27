using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class FailedCheckException : Exception
	{
		public FailedCheckException()
			: base(SR.GetString("ID3254"))
		{
		}

		public FailedCheckException(string message)
			: base(message)
		{
		}

		public FailedCheckException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected FailedCheckException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
