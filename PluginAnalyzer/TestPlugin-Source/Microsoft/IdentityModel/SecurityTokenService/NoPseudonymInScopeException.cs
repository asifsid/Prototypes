using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class NoPseudonymInScopeException : RequestException
	{
		public NoPseudonymInScopeException()
			: base(SR.GetString("ID2091"))
		{
		}

		public NoPseudonymInScopeException(string message)
			: base(message)
		{
		}

		public NoPseudonymInScopeException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected NoPseudonymInScopeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
