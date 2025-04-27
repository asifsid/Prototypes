using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class IssuerNameNotSupportedFaultException : RequestFaultException
	{
		public IssuerNameNotSupportedFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3240")))
		{
		}

		public IssuerNameNotSupportedFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("IssuerNameNotSupported", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
