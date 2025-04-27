using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class InternalErrorFaultException : RequestFaultException
	{
		public InternalErrorFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID2101")))
		{
		}

		public InternalErrorFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("InternalError")))
		{
		}
	}
}
