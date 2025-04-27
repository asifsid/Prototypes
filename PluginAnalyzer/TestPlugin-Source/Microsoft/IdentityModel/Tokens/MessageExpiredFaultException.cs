using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class MessageExpiredFaultException : FaultException
	{
		public MessageExpiredFaultException()
			: this(new FaultReason(SR.GetString("ID3256")))
		{
		}

		public MessageExpiredFaultException(FaultReason reason)
			: base(reason, new FaultCode("Sender", new FaultCode("MessageExpired", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
		{
		}
	}
}
