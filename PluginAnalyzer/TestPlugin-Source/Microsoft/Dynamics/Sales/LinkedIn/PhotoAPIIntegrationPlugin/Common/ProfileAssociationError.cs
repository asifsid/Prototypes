using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[DataContract]
	[ComVisible(true)]
	public class ProfileAssociationError
	{
		[DataMember]
		public string message { get; set; }

		[DataMember]
		public string status { get; set; }

		[DataMember]
		public string serviceErrorCode { get; set; }
	}
}
