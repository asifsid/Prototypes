using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class RstParameterNotAcceptedFaultException : RequestFaultException
	{
		public RstParameterNotAcceptedFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3239")))
		{
		}

		public RstParameterNotAcceptedFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("RstParameterNotAccepted", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
