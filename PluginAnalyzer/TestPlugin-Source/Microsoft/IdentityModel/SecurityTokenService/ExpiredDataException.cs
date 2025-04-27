using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class ExpiredDataException : RequestException
	{
		public ExpiredDataException()
			: base(SR.GetString("ID2087"))
		{
		}

		public ExpiredDataException(string message)
			: base(message)
		{
		}

		public ExpiredDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected ExpiredDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
