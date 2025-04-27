using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class InformationCardRefreshRequiredException : Exception
	{
		public InformationCardRefreshRequiredException()
			: base(SR.GetString("ID2047"))
		{
		}

		public InformationCardRefreshRequiredException(string message)
			: base(message)
		{
		}

		public InformationCardRefreshRequiredException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InformationCardRefreshRequiredException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
