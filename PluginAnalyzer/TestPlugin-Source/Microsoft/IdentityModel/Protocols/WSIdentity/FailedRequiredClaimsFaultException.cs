using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class FailedRequiredClaimsFaultException : RequestFaultException
	{
		public FailedRequiredClaimsFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID2046")))
		{
		}

		public FailedRequiredClaimsFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("FailedRequiredClaims")))
		{
		}
	}
}
