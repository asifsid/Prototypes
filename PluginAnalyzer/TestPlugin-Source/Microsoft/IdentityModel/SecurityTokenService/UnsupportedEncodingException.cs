using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedEncodingException : RequestException
	{
		public UnsupportedEncodingException()
			: base(SR.GetString("ID2099"))
		{
		}

		public UnsupportedEncodingException(string message)
			: base(message)
		{
		}

		public UnsupportedEncodingException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UnsupportedEncodingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
