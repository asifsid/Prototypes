using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.AzureRecommendations.Messages
{
	[DataContract(Namespace = "http://schemas.microsoft.com/crm/2011/Contracts")]
	[ComVisible(true)]
	public sealed class GetAzureServiceConnectionIdByTypeRequest : OrganizationRequest
	{
		public int ConnectionType
		{
			get
			{
				if (((DataCollection<string, object>)(object)((OrganizationRequest)this).get_Parameters()).Contains("ConnectionType"))
				{
					return (int)((DataCollection<string, object>)(object)((OrganizationRequest)this).get_Parameters()).get_Item("ConnectionType");
				}
				return 0;
			}
			set
			{
				((DataCollection<string, object>)(object)((OrganizationRequest)this).get_Parameters()).set_Item("ConnectionType", (object)value);
			}
		}

		public GetAzureServiceConnectionIdByTypeRequest()
			: this()
		{
			((OrganizationRequest)this).set_RequestName("GetAzureServiceConnectionIdByType");
			ConnectionType = 0;
		}
	}
}
