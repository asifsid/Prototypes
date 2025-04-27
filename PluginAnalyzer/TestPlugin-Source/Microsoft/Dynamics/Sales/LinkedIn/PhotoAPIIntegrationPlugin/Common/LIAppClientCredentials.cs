using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[DataContract]
	[ComVisible(true)]
	public class LIAppClientCredentials
	{
		[DataMember]
		public string client_id { get; set; }

		[DataMember]
		public string client_secret { get; set; }
	}
}
