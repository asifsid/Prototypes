using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class RenewNeededException : RequestException
	{
		public RenewNeededException()
			: base(SR.GetString("ID2089"))
		{
		}

		public RenewNeededException(string message)
			: base(message)
		{
		}

		public RenewNeededException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected RenewNeededException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
