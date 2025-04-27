using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class UnsupportedAlgorithmFaultException : FaultException
	{
		public UnsupportedAlgorithmFaultException()
			: this(new FaultReason(SR.GetString("ID3251")))
		{
		}

		public UnsupportedAlgorithmFaultException(FaultReason reason)
			: base(reason, new FaultCode("Sender", new FaultCode("UnsupportedAlgorithm", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")))
		{
		}
	}
}
