using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class AlreadySignedInFaultException : RequestFaultException
	{
		public AlreadySignedInFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3234")))
		{
		}

		public AlreadySignedInFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("AlreadySignedIn", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
