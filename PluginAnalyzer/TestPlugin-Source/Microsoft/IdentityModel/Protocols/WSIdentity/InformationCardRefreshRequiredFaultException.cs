using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class InformationCardRefreshRequiredFaultException : RequestFaultException
	{
		public InformationCardRefreshRequiredFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID2047")))
		{
		}

		public InformationCardRefreshRequiredFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("InformationCardRefreshRequired")))
		{
		}
	}
}
