using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[DataContract]
	[ComVisible(true)]
	public class ChildAppProvisioningInputs
	{
		[DataMember]
		public string uniqueForeignId { get; set; }

		[DataMember]
		public string name { get; set; }

		[DataMember]
		public string description { get; set; }
	}
}
