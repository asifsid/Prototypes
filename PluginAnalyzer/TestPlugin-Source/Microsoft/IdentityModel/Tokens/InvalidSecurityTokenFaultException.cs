using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class InvalidSecurityTokenFaultException : FaultException
	{
		public InvalidSecurityTokenFaultException()
			: this(new FaultReason(SR.GetString("ID3253")))
		{
		}

		public InvalidSecurityTokenFaultException(FaultReason reason)
			: base(reason, new FaultCode("Sender", new FaultCode("InvalidSecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
		{
		}
	}
}
