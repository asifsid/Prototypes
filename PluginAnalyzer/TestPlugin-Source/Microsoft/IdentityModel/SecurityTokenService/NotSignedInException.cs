using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class NotSignedInException : RequestException
	{
		public NotSignedInException()
			: base(SR.GetString("ID2093"))
		{
		}

		public NotSignedInException(string message)
			: base(message)
		{
		}

		public NotSignedInException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected NotSignedInException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
