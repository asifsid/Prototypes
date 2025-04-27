using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class FailedAuthenticationFaultException : FaultException
	{
		public FailedAuthenticationFaultException()
			: this(new FaultReason(SR.GetString("ID3242")))
		{
		}

		public FailedAuthenticationFaultException(FaultReason reason)
			: base(reason, new FaultCode("Sender", new FaultCode("FailedAuthentication", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
		{
		}
	}
}
