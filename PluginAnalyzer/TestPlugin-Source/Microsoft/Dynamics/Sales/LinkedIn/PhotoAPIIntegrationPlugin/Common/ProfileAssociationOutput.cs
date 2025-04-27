using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[DataContract]
	[ComVisible(true)]
	public class ProfileAssociationOutput
	{
		[DataMember]
		public string profilePhoto { get; set; }

		[DataMember]
		public string member { get; set; }

		[DataMember]
		public string profile { get; set; }
	}
}
