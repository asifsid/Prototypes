using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class AuthenticationBadElementsException : RequestException
	{
		public AuthenticationBadElementsException()
			: base(SR.GetString("ID2086"))
		{
		}

		public AuthenticationBadElementsException(string message)
			: base(message)
		{
		}

		public AuthenticationBadElementsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected AuthenticationBadElementsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
