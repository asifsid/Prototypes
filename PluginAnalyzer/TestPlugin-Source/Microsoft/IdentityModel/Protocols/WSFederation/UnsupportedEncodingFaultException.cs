using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedEncodingFaultException : RequestFaultException
	{
		public UnsupportedEncodingFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3241")))
		{
		}

		public UnsupportedEncodingFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("UnsupportedEncoding", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
