using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Model;
using Microsoft.Xrm.Sdk.Messages;

namespace Microsoft.Dynamics.Solution.Common.Interfaces
{
	[ComVisible(true)]
	public interface IAttributeUtility
	{
		AttributeMetadataWrapper GetAttributeMetadataForSetDefaultPriceLevel(RetrieveAttributeResponse retrieveAttributeResponse);

		int GetOptionsCountForHasSalesStages(RetrieveAttributeResponse retrieveAttributeResponse);
	}
}
