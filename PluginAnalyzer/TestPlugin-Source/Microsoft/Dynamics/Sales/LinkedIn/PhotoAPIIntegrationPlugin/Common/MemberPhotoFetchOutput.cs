using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[DataContract]
	[ComVisible(true)]
	public class MemberPhotoFetchOutput
	{
		[DataMember]
		public bool shouldRefresh;

		[DataMember]
		public string photoOrigin;

		[DataMember]
		public string photoUrl;

		[DataMember]
		public string profileAssociationId;

		[DataMember]
		public string errorMessage;

		public MemberPhotoFetchOutput()
		{
			shouldRefresh = false;
			photoOrigin = (photoUrl = (profileAssociationId = (errorMessage = string.Empty)));
		}

		public MemberPhotoFetchOutput(bool _shouldRefresh, string _photoOrigin = "", string _photoUrl = "", string _profileAssociationId = "", string _errorMsg = "")
		{
			shouldRefresh = _shouldRefresh;
			photoOrigin = _photoOrigin;
			photoUrl = _photoUrl;
			profileAssociationId = _profileAssociationId;
			errorMessage = _errorMsg;
		}
	}
}
