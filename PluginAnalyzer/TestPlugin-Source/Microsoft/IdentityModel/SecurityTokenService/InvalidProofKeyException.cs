using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidProofKeyException : Exception
	{
		public InvalidProofKeyException()
			: base(SR.GetString("ID2044"))
		{
		}

		public InvalidProofKeyException(string message)
			: base(message)
		{
		}

		public InvalidProofKeyException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InvalidProofKeyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
