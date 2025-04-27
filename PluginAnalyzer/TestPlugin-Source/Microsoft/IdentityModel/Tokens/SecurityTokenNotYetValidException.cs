using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class SecurityTokenNotYetValidException : SecurityTokenValidationException
	{
		public SecurityTokenNotYetValidException()
			: base(SR.GetString("ID4182"))
		{
		}

		public SecurityTokenNotYetValidException(string message)
			: base(message)
		{
		}

		public SecurityTokenNotYetValidException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected SecurityTokenNotYetValidException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
