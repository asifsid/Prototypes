using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidProofKeyFaultException : RequestFaultException
	{
		public InvalidProofKeyFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID2044")))
		{
		}

		public InvalidProofKeyFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("InvalidProofKey")))
		{
		}
	}
}
