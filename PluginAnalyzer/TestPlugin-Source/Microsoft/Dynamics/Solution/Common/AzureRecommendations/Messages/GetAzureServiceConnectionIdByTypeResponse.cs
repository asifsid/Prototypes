using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.AzureRecommendations.Messages
{
	[DataContract(Namespace = "http://schemas.microsoft.com/crm/2011/Contracts")]
	[ComVisible(true)]
	public sealed class GetAzureServiceConnectionIdByTypeResponse : OrganizationResponse
	{
		public Guid ConnectionId
		{
			get
			{
				if (((DataCollection<string, object>)(object)((OrganizationResponse)this).get_Results()).Contains("ConnectionId"))
				{
					return (Guid)((DataCollection<string, object>)(object)((OrganizationResponse)this).get_Results()).get_Item("ConnectionId");
				}
				return default(Guid);
			}
		}

		public GetAzureServiceConnectionIdByTypeResponse()
			: this()
		{
		}
	}
}
