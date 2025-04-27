using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class AlreadySignedInException : RequestException
	{
		public AlreadySignedInException()
			: base(SR.GetString("ID2092"))
		{
		}

		public AlreadySignedInException(string message)
			: base(message)
		{
		}

		public AlreadySignedInException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected AlreadySignedInException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
