using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Threading
{
	[Serializable]
	[ComVisible(true)]
	public class AsynchronousOperationException : Exception
	{
		public AsynchronousOperationException()
			: base(SR.GetString("ID4004"))
		{
		}

		public AsynchronousOperationException(string message)
			: base(message)
		{
		}

		public AsynchronousOperationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public AsynchronousOperationException(Exception innerException)
			: base(SR.GetString("ID4004"), innerException)
		{
		}

		protected AsynchronousOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
