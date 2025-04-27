using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class UnsupportedSecurityTokenFaultException : FaultException
	{
		public UnsupportedSecurityTokenFaultException()
			: this(new FaultReason(SR.GetString("ID3250")))
		{
		}

		public UnsupportedSecurityTokenFaultException(FaultReason reason)
			: base(reason, new FaultCode("Sender", new FaultCode("UnsupportedSecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
		{
		}
	}
}
