using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedSignatureFormatFaultException : RequestFaultException
	{
		public UnsupportedSignatureFormatFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID2104")))
		{
		}

		public UnsupportedSignatureFormatFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("UnsupportedSignatureFormat")))
		{
		}
	}
}
