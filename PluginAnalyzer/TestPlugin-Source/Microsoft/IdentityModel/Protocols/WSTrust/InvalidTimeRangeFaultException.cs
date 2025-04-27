using System;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[Serializable]
	[ComVisible(true)]
	public class InvalidTimeRangeFaultException : RequestFaultException
	{
		public InvalidTimeRangeFaultException(string soapNamespace, string trustNamespace)
			: this(soapNamespace, trustNamespace, new FaultReason(SR.GetString("ID3243")))
		{
		}

		public InvalidTimeRangeFaultException(string soapNamespace, string trustNamespace, FaultReason reason)
			: base(reason, new FaultCode("Sender", soapNamespace, new FaultCode(GetFaultSubCodeName(trustNamespace), trustNamespace)))
		{
		}

		private static string GetFaultSubCodeName(string trustNamespace)
		{
			WSTrustConstantsAdapter constantsAdapter = WSTrustConstantsAdapter.GetConstantsAdapter(trustNamespace);
			if (constantsAdapter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("trustNamespace", SR.GetString("ID3228"));
			}
			return constantsAdapter.FaultCodes.InvalidTimeRange;
		}
	}
}
