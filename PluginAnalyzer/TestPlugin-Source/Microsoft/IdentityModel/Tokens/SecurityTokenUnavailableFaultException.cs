using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityTokenUnavailableFaultException : FaultException
	{
		public SecurityTokenUnavailableFaultException()
			: this(new FaultReason(SR.GetString("ID3255")))
		{
		}

		public SecurityTokenUnavailableFaultException(FaultReason reason)
			: base(reason, new FaultCode("Sender", new FaultCode("SecurityTokenUnavailable", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
		{
		}
	}
}
