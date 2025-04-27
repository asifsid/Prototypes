using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class EncryptedTokenDecryptionFailedException : SecurityTokenException
	{
		public EncryptedTokenDecryptionFailedException()
			: base(SR.GetString("ID4022"))
		{
		}

		public EncryptedTokenDecryptionFailedException(string message)
			: base(message)
		{
		}

		public EncryptedTokenDecryptionFailedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected EncryptedTokenDecryptionFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
