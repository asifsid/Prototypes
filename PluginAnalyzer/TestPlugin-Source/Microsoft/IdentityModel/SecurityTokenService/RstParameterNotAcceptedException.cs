using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class RstParameterNotAcceptedException : RequestException
	{
		public RstParameterNotAcceptedException()
			: base(SR.GetString("ID2097"))
		{
		}

		public RstParameterNotAcceptedException(string message)
			: base(message)
		{
		}

		public RstParameterNotAcceptedException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected RstParameterNotAcceptedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
