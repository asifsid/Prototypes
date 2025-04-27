using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSFederation
{
	[Serializable]
	[ComVisible(true)]
	public class UnsupportedClaimsDialectFaultException : RequestFaultException
	{
		public UnsupportedClaimsDialectFaultException(string soapNamespace)
			: this(soapNamespace, new FaultReason(SR.GetString("ID3238")))
		{
		}

		public UnsupportedClaimsDialectFaultException(string soapNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode("UnsupportedClaimsDialect", "http://docs.oasis-open.org/wsfed/federation/200706")))
		{
		}
	}
}
