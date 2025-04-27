using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class InformationCardException : Exception, ISerializable
	{
		public InformationCardException()
			: this(SR.GetString("ID3059"))
		{
		}

		public InformationCardException(string message)
			: base(message)
		{
		}

		public InformationCardException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InformationCardException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
