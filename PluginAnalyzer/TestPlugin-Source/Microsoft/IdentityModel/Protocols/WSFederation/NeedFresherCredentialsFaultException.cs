using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class NeedFresherCredentialsFaultException : RequestFaultException
	{
		public NeedFresherCredentialsFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3237")))
		{
		}

		public NeedFresherCredentialsFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("NeedFresherCredentials", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
