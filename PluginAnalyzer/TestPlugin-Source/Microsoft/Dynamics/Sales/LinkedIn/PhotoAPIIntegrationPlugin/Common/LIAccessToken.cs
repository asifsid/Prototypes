using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[DataContract]
	[ComVisible(true)]
	public class LIAccessToken
	{
		[DataMember]
		public string access_token { get; set; }

		[DataMember]
		public long expires_in { get; set; }
	}
}
