using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedAlgorithmException : Exception
	{
		public UnsupportedAlgorithmException()
			: base(SR.GetString("ID3251"))
		{
		}

		public UnsupportedAlgorithmException(string message)
			: base(message)
		{
		}

		public UnsupportedAlgorithmException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UnsupportedAlgorithmException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
