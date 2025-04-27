using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Interfaces;
using Microsoft.Dynamics.Solution.Common.Model;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Microsoft.Dynamics.Solution.Common.Services
{
	[ComVisible(true)]
	public class AttributeUtility : IAttributeUtility
	{
		public static IAttributeUtility Instance = new AttributeUtility();

		public AttributeMetadataWrapper GetAttributeMetadataForSetDefaultPriceLevel(RetrieveAttributeResponse retrieveAttributeResponse)
		{
			AttributeMetadata attributeMetadata = retrieveAttributeResponse.get_AttributeMetadata();
			return new AttributeMetadataWrapper
			{
				IsSecured = attributeMetadata.get_IsSecured(),
				CanBeSecuredForCreate = attributeMetadata.get_CanBeSecuredForCreate(),
				IsValidForCreate = attributeMetadata.get_IsValidForCreate()
			};
		}

		public int GetOptionsCountForHasSalesStages(RetrieveAttributeResponse retrieveAttributeResponse)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			EnumAttributeMetadata val = (EnumAttributeMetadata)retrieveAttributeResponse.get_AttributeMetadata();
			return ((Collection<OptionMetadata>)(object)val.get_OptionSet().get_Options()).Count;
		}
	}
}
