using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[Serializable]
	[ComVisible(true)]
	public class MissingAppliesToException : Exception
	{
		public MissingAppliesToException()
			: base(SR.GetString("ID2043"))
		{
		}

		public MissingAppliesToException(string message)
			: base(message)
		{
		}

		public MissingAppliesToException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected MissingAppliesToException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
