using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class NoMatchInScopeException : RequestException
	{
		public NoMatchInScopeException()
			: base(SR.GetString("ID2094"))
		{
		}

		public NoMatchInScopeException(string message)
			: base(message)
		{
		}

		public NoMatchInScopeException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected NoMatchInScopeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
