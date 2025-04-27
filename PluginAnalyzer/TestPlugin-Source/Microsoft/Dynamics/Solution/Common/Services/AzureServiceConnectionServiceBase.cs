using System.Runtime.InteropServices;
using Microsoft.Crm.Common.ObjectModel;
using Microsoft.Dynamics.Solution.Common.AzureRecommendations.Messages;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.Services
{
	[ComVisible(true)]
	public class AzureServiceConnectionServiceBase : IAzureConnectionService
	{
		public EntityReference GetAzureServiceConnectionIdByType(int connectionType, IPluginContext context)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			GetAzureServiceConnectionIdByTypeRequest getAzureServiceConnectionIdByTypeRequest = new GetAzureServiceConnectionIdByTypeRequest
			{
				ConnectionType = connectionType
			};
			GetAzureServiceConnectionIdByTypeResponse getAzureServiceConnectionIdByTypeResponse = (GetAzureServiceConnectionIdByTypeResponse)(object)context.OrganizationService.Execute((OrganizationRequest)(object)getAzureServiceConnectionIdByTypeRequest);
			return new EntityReference("azureserviceconnection", getAzureServiceConnectionIdByTypeResponse.ConnectionId);
		}
	}
}
