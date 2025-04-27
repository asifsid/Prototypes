using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols
{
	[Serializable]
	[ComVisible(true)]
	public class FederationException : Exception
	{
		public FederationException()
			: this(SR.GetString("ID3205"))
		{
		}

		public FederationException(string message)
			: base(message)
		{
		}

		public FederationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected FederationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
