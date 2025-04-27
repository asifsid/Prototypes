using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class WSFederationMessageException : Exception
	{
		public WSFederationMessageException()
			: this(SR.GetString("ID3074"))
		{
		}

		public WSFederationMessageException(string message)
			: base(message)
		{
		}

		public WSFederationMessageException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected WSFederationMessageException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
