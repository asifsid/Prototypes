using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[Serializable]
	[ComVisible(true)]
	public class WSTrustSerializationException : Exception, ISerializable
	{
		public WSTrustSerializationException()
			: this(SR.GetString("ID3063"))
		{
		}

		public WSTrustSerializationException(string message)
			: base(message)
		{
		}

		public WSTrustSerializationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected WSTrustSerializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
