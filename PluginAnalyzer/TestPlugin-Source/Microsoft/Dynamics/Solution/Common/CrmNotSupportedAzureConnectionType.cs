using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Localization;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class CrmNotSupportedAzureConnectionType : CrmException
	{
		public CrmNotSupportedAzureConnectionType(AzureServiceConnectionType azureConnectionType)
			: base(string.Format(Labels.AzureServiceConnectionTypeNotSupported, azureConnectionType.ToInt32()), -2147220715)
		{
		}
	}
}
