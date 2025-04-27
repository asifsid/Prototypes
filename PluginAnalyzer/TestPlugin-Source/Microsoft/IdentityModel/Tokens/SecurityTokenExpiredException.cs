using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class SecurityTokenExpiredException : SecurityTokenValidationException
	{
		public SecurityTokenExpiredException()
			: base(SR.GetString("ID4181"))
		{
		}

		public SecurityTokenExpiredException(string message)
			: base(message)
		{
		}

		public SecurityTokenExpiredException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected SecurityTokenExpiredException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
