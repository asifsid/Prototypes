using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedClaimsDialectException : RequestException
	{
		public UnsupportedClaimsDialectException()
			: base(SR.GetString("ID2096"))
		{
		}

		public UnsupportedClaimsDialectException(string message)
			: base(message)
		{
		}

		public UnsupportedClaimsDialectException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected UnsupportedClaimsDialectException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
