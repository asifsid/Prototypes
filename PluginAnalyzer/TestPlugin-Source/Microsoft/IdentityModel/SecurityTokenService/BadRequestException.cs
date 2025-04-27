using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class BadRequestException : RequestException
	{
		public BadRequestException()
			: base(SR.GetString("ID2009"))
		{
		}

		public BadRequestException(string message)
			: base(message)
		{
		}

		public BadRequestException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected BadRequestException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
