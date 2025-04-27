using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedSignatureFormatException : Exception
	{
		public UnsupportedSignatureFormatException()
			: base(SR.GetString("ID2104"))
		{
		}

		public UnsupportedSignatureFormatException(string message)
			: base(message)
		{
		}

		public UnsupportedSignatureFormatException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UnsupportedSignatureFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
