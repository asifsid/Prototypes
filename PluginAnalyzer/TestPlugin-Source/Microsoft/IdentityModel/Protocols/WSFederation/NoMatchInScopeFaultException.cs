using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class NoMatchInScopeFaultException : RequestFaultException
	{
		public NoMatchInScopeFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3236")))
		{
		}

		public NoMatchInScopeFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("NoMatchInScope", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
