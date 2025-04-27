using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class UnknownInformationCardReferenceException : Exception
	{
		public UnknownInformationCardReferenceException()
			: base(SR.GetString("ID2045"))
		{
		}

		public UnknownInformationCardReferenceException(string message)
			: base(message)
		{
		}

		public UnknownInformationCardReferenceException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UnknownInformationCardReferenceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
