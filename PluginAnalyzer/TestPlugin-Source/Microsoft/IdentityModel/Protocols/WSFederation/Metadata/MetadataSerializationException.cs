using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[Serializable]
	[ComVisible(true)]
	public class MetadataSerializationException : Exception
	{
		public MetadataSerializationException()
			: this(SR.GetString("ID3198"))
		{
		}

		public MetadataSerializationException(string message)
			: base(message)
		{
		}

		public MetadataSerializationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected MetadataSerializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
