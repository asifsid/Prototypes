using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class FailedCheckFaultException : FaultException
	{
		public FailedCheckFaultException()
			: this(new FaultReason(SR.GetString("ID3254")))
		{
		}

		public FailedCheckFaultException(FaultReason reason)
			: base(reason, new FaultCode("Sender", new FaultCode("FailedCheck", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
		{
		}
	}
}
