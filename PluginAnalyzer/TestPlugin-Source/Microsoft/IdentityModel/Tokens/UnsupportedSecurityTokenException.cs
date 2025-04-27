using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedSecurityTokenException : Exception
	{
		public UnsupportedSecurityTokenException()
			: base(SR.GetString("ID3250"))
		{
		}

		public UnsupportedSecurityTokenException(string message)
			: base(message)
		{
		}

		public UnsupportedSecurityTokenException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UnsupportedSecurityTokenException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
