using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class NoPseudonymInScopeFaultException : RequestFaultException
	{
		public NoPseudonymInScopeFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3233")))
		{
		}

		public NoPseudonymInScopeFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("NoPseudonymInScope", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
