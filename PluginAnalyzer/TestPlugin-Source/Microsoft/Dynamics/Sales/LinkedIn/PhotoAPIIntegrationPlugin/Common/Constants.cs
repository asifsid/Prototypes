using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common
{
	[ComVisible(true)]
	public static class Constants
	{
		public const string OrgIdentifier = "OrgUrl";

		public const string RecordIds = "RecordIds";

		public const string RecordType = "RecordType";

		public const string EntityImageUrlAttrName = "entityimage_url";

		public const string CrmPhotoOrigin = "CRM";

		public const string LinkedInPhotoOrigin = "LinkedIn";

		public const string LIParentAppClientSecretsVaultKey = "LIMRSParentAppSecrets";

		public const string ProfileAssociationsEntityLogicalName = "sales_linkedin_profileassociations";

		public const string ProfileAssociationsEntityObjId = "sales_linkedin_objectid";

		public const string ProfileAssociationsEntityRefreshTime = "sales_linkedin_profilephotorefreshtime";

		public const string ProfileAssociationsEntityPhotoUrl = "sales_linkedin_profilephotourl";

		public const string BusinessEntity = "BusinessEntity";

		public const double LinkedIn_Photo_TTL_Hrs = 24.0;

		public static readonly string[] CrmSyncErrorCodes = new string[2] { "40001", "40002" };

		public const string Error_CRM_Sync_Disabled_Msg = "ERROR_CRM_SYNC_DISABLED";

		public const string ConfigurationEntityLogicalName = "sales_linkedin_configuration";

		public const string ConfigurationEntityRecordKey = "sales_name";

		public const string ConfigurationEntityRecordName = "Active Sales Navigator Configuration";

		public const string ProfileFetchStatusAttributeName = "sales_linkedin_profileFetch_Status";

		public const string MediaTypeJsonContent = "application/json";

		public const string LiAppProvisioningEndPoint = "https://api.linkedin.com/v2/provisionedApplications";

		public const string AuthHeader = "Bearer";
	}
}
