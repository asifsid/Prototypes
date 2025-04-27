using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[DataContract]
	[ComVisible(true)]
	public class LIAuthConfigStoreValue
	{
		[DataMember]
		public LIAppClientCredentials clientCredentials { get; set; }

		[DataMember]
		public LIAccessToken accessToken { get; set; }
	}
}
