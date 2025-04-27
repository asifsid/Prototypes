using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[DataContract]
	[ComVisible(true)]
	public class ChildAppProvisioningOutput
	{
		[DataMember]
		public string key { get; set; }

		[DataMember]
		public LIAppClientCredentials credentials { get; set; }
	}
}
