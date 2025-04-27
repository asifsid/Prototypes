using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class MissingAppliesToFaultException : RequestFaultException
	{
		public MissingAppliesToFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID2043")))
		{
		}

		public MissingAppliesToFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("MissingAppliesTo")))
		{
		}
	}
}
