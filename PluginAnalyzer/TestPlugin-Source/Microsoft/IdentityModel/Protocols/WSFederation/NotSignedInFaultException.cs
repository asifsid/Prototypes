using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class NotSignedInFaultException : RequestFaultException
	{
		public NotSignedInFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3235")))
		{
		}

		public NotSignedInFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("NotSignedIn", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
