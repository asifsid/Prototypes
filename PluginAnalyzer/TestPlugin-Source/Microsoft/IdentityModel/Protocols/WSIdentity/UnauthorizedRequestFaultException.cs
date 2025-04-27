using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class UnauthorizedRequestFaultException : RequestFaultException
	{
		public UnauthorizedRequestFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID2102")))
		{
		}

		public UnauthorizedRequestFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("UnauthorizedRequest")))
		{
		}
	}
}
