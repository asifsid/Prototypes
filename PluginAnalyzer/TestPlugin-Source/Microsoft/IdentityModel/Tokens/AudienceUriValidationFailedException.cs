using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Tokens
{
	[Serializable]
	[ComVisible(true)]
	public class AudienceUriValidationFailedException : SecurityTokenValidationException
	{
		public AudienceUriValidationFailedException()
			: base(SR.GetString("ID4183"))
		{
		}

		public AudienceUriValidationFailedException(string message)
			: base(message)
		{
		}

		public AudienceUriValidationFailedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected AudienceUriValidationFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
