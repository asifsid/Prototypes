using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidInputFaultException : RequestFaultException
	{
		public InvalidInputFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID2103")))
		{
		}

		public InvalidInputFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("InvalidInput")))
		{
		}
	}
}
