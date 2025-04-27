using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Web
{
	[Serializable]
	[ComVisible(true)]
	public class FederatedAuthenticationSessionEndingException : Exception, ISerializable
	{
		public FederatedAuthenticationSessionEndingException()
			: base(SR.GetString("ID1003"))
		{
		}

		public FederatedAuthenticationSessionEndingException(string message)
			: base(message)
		{
		}

		public FederatedAuthenticationSessionEndingException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected FederatedAuthenticationSessionEndingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
