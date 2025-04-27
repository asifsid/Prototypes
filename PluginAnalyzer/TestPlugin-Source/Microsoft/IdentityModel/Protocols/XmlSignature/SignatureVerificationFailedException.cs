using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	[Serializable]
	[ComVisible(true)]
	public class SignatureVerificationFailedException : SecurityTokenException
	{
		public SignatureVerificationFailedException()
			: base(SR.GetString("ID4038"))
		{
		}

		public SignatureVerificationFailedException(string message)
			: base(message)
		{
		}

		public SignatureVerificationFailedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected SignatureVerificationFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
