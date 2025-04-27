using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class SecurityTokenReplayDetectedException : SecurityTokenValidationException
	{
		public SecurityTokenReplayDetectedException()
			: base(SR.GetString("ID1070"))
		{
		}

		public SecurityTokenReplayDetectedException(string message)
			: base(message)
		{
		}

		public SecurityTokenReplayDetectedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected SecurityTokenReplayDetectedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
