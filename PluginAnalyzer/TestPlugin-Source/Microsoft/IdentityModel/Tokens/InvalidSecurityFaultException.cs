using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class InvalidSecurityFaultException : FaultException
	{
		public InvalidSecurityFaultException()
			: this(new FaultReason(SR.GetString("ID3252")))
		{
		}

		public InvalidSecurityFaultException(FaultReason reason)
			: base(reason, new FaultCode("Sender", new FaultCode("InvalidSecurity", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
		{
		}
	}
}
