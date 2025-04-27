using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class IssuerNameNotSupportedException : RequestException
	{
		public IssuerNameNotSupportedException()
			: base(SR.GetString("ID2098"))
		{
		}

		public IssuerNameNotSupportedException(string message)
			: base(message)
		{
		}

		public IssuerNameNotSupportedException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected IssuerNameNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
