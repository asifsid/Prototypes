using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class CommonErrorCodes
	{
		public const int FirstPlatformErrorCode = -2147220992;

		public const int RequiredFieldMissing = -2147220992;

		public const int InvalidXml = -2147220991;

		public const int EmptyXml = -2147220990;

		public const int InvalidArgument = -2147220989;

		public const int InvalidParent = -2147220987;

		public const int InvalidSharee = -2147220980;

		public const int InvalidAccessRights = -2147220979;

		public const int InvalidAssigneeId = -2147220976;

		public const int CannotShareWithOwner = -2147220972;

		public const int MissingOwner = -2147220971;

		public const int UnExpected = -2147220970;

		public const int ObjectDoesNotExist = -2147220969;

		public const int NotImplemented = -2147220967;

		public const int InvalidPointer = -2147220968;

		public const int MissingBusinessId = -2147220966;

		public const int MissingUserId = -2147220965;

		public const int InvalidObjectTypes = -2147220961;

		public const int PrivilegeDenied = -2147220960;

		public const int OnlyOwnerCanRevoke = -2147220957;

		public const int CannotDeleteDueToAssociation = -2147220953;

		public const int CannotDeleteAsItIsReadOnly = -2147220952;

		public const int InvalidOwnerID = -2147220951;

		public const int CannotUpdateBecauseItIsReadOnly = -2147220946;

		public const int PrincipalPrivilegeDenied = -2147220943;

		public const int InvalidState = -2147220941;

		public const int InvalidRollupType = -2147220940;

		public const int MissingQueryType = -2147220939;

		public const int InvalidDateTime = -2147220935;

		public const int InvalidMetadata = -2147220934;

		public const int InvalidOperation = -2147220933;

		public const int InvalidVersion = -2147220932;

		public const int UnpopulatedPrimaryKey = -2147220931;

		public const int OpenCrmDBConnection = -2147220930;

		public const int InvalidConnectionString = -2147220929;

		public const int UserTimeZoneException = -2147220928;

		public const int UserTimeConvertException = -2147220927;

		public const int FirstGenericErrorCode = -2147220926;

		public const int EntityInstantiationFailed = -2147220925;

		public const int ServiceInstantiationFailed = -2147220924;

		public const int CrmImpersonationError = -2147220923;

		public const int InvalidRecurrenceRule = -2147220922;

		public const int InvalidXmlCollectionNameException = -2147220921;

		public const int InvalidXmlEntityNameException = -2147220920;

		public const int InvalidEntityClassException = -2147220919;

		public const int MetadataNotFound = -2147220918;

		public const int InvalidPriv = -2147220917;

		public const int InvalidRegistryKey = -2147220916;

		public const int CrmQueryExpressionNotInitialized = -2147220915;

		public const int FailedToLoadAssembly = -2147220914;

		public const int InvalidSingletonResults = -2147220913;

		public const int MultipleChildPicklist = -2147220912;

		public const int TransactionNotStarted = -2147220911;

		public const int TransactionNotCommited = -2147220910;

		public const int SessionTokenUnavailable = -2147220909;

		public const int CannotBindToSession = -2147220908;

		public const int TransactionAborted = -2147220907;

		public const int CrmSecurityError = -2147220906;

		public const int InvalidCaller = -2147220905;

		public const int InvalidRestore = -2147220904;

		public const int NumberFormatFailed = -2147220903;

		public const int DateTimeFormatFailed = -2147220902;

		public const int CalloutException = -2147220901;

		public const int CrmMalformedExpressionError = -2147220900;

		public const int CrmExpressionParsingError = -2147220899;

		public const int CrmExpressionBodyParsingError = -2147220898;

		public const int CrmExpressionParametersParsingError = -2147220897;

		public const int CrmExpressionEvaluationError = -2147220896;

		public const int CrmConstraintEvaluationError = -2147220895;

		public const int CrmConstraintParsingError = -2147220894;

		public const int CannotCreateOutlookFilters = -2147220893;

		public const int CannotAssignOutlookFilters = -2147220892;

		public const int IsvAborted = -2147220891;

		public const int InvalidPrimaryKey = -2147220890;

		public const int InvalidPrincipal = -2147220322;

		public const int ProductProductNumberExists = -2147157645;

		public const int InvalidQuantityDecimalCode = -2147206404;

		public const int DuplicateRecord = -2147220937;

		public const int AccessDenied = -2147024891;

		public const int FirstQueryBuilderError = -2147217152;

		public const int QueryBuilderInvalidUpdate = -2147217152;

		public const int QueryBuilderUnexpected = -2147217151;

		public const int QueryBuilderNoEntity = -2147217150;

		public const int QueryBuilderNoAttribute = -2147217149;

		public const int QueryBuilderNo_Primary_Key = -2147217148;

		public const int QueryBuilderMulti_Primary_Key = -2147217147;

		public const int QueryBuilderBad_Condition = -2147217146;

		public const int QueryBuilderAttribute_With_Aggregate = -2147217145;

		public const int QueryBuilderInvalid_Value = -2147217144;

		public const int QueryBuilderInvalid_Alias = -2147217143;

		public const int QueryBuilderAlias_Does_Not_Exist = -2147217142;

		public const int QueryBuilderNoAlias = -2147217141;

		public const int QueryBuilderValue_GreaterThanZero = -2147217140;

		public const int QueryBuilderReportView_Does_Not_Exist = -2147217139;

		public const int QueryBuilderMultipleIntersectEntities = -2147217138;

		public const int QueryBuilderByAttributeMismatch = -2147217137;

		public const int QueryBuilderByAttributeNonEmpty = -2147217136;

		public const int QueryBuilderAttributePairMismatch = -2147217135;

		public const int QueryBuilderInvalidColumnSetVersion = -2147217134;

		public const int QueryBuilderColumnSetVersionMissing = -2147217133;

		public const int QueryBuilderSerialzeLinkTopCriteria = -2147217132;

		public const int QueryBuilderDeserializeInvalidDistinct = -2147217131;

		public const int QueryBuilderDeserializeInvalidMapping = -2147217130;

		public const int QueryBuilderDeserializeInvalidLinkType = -2147217129;

		public const int QueryBuilderDeserializeLinkAttributes = -2147217128;

		public const int QueryBuilderDeserializeInvalidDescending = -2147217127;

		public const int QueryBuilderDeserializeInvalidAggregate = -2147217126;

		public const int QueryBuilderDeserializeInvalidAlias = -2147217125;

		public const int QueryBuilderDeserializeInvalidNode = -2147217124;

		public const int QueryBuilderDeserializeInvalidUtcOffset = -2147217123;

		public const int QueryBuilderAttributeNotFound = -2147217122;

		public const int QueryBuilderInvalidOrderType = -2147217121;

		public const int QueryBuilderInvalidConditionOperator = -2147217120;

		public const int QueryBuilderInvalidJoinOperator = -2147217119;

		public const int QueryBuilderInvalidFilterType = -2147217118;

		public const int QueryBuilderElementNotFound = -2147217117;

		public const int QueryBuilderDeserializeEmptyXml = -2147217116;

		public const int QueryBuilderDeserializeNoDocElemXml = -2147217115;

		public const int QueryBuilderLinkNodeForOrderNotFound = -2147217114;

		public const int QueryBuilderTopCountGreaterThanZero = -2147217113;

		public const int QueryBuilderEntitiesDontMatch = -2147217112;

		public const int FirstActivityErrorCode = -2147207936;

		public const int ActivityPartyObjectTypeNotAllowed = -2147207930;

		public const int MissingRecipient = -2147207923;

		public const int TooManyRecipients = -2147207922;

		public const int NoDialNumber = -2147207921;

		public const int FaxSendBlocked = -2147207920;

		public const int FaxServiceNotRunning = -2147207919;

		public const int ActivityInvalidSessionToken = -2147207918;

		public const int ActivityInvalidObjectTypeCode = -2147207917;

		public const int InvalidActivityXml = -2147207916;

		public const int InvalidPartyMapping = -2147207915;

		public const int FirstSdkErrorCode = -2147219456;

		public const int SdkEntityDoesNotSupportGenericMethod = -2147219456;

		public const int NotSupported = -2147220715;

		public const int UnitNotInSchedule = -2147206378;

		public const int ProductInvalidUnit = -2147206380;

		public const int CannotActivateRecord = -2146955241;

		public const int InvalidStateTransition = -2147160050;

		public const int CannotAddDraftFamilyProductBundleToCases = -2147157628;

		public const int AzureOperationResponseTimedOut = -2147084747;

		public const int InvalidParentId = -2147220986;

		public const int AdvancedSimilaritySearchFeatureNotEnabled = -2147084715;

		public const int AsyncOperationMissingId = -2147204764;

		public const int InvalidProduct = -2147088861;

		public const int SubjectLoopBeingCreated = -2147205631;

		public const int AccessTokenExpired = -2147094271;

		public const int BadRequest = -2147094272;

		public const int InvalidStateCodeStatusCode = -2147187704;

		public const int MissingUomScheduleId = -2147206390;

		public const int NetworkIssue = -2147094268;

		public const int ProductKitLoopExists = -2147206366;

		public const int ProductKitLoopBeingCreated = -2147206365;

		public const int ProductDoesNotExist = -2147206364;

		public const int TextAnalyticsFeatureNotEnabled = -2147084718;

		public const int TextAnalyticsAzureSchedulerError = -2147084653;

		public const int unManagedidsdataaccessunexpected = -2147212544;

		public const int Forbidden = -2147094270;

		public const int DecimalValueOutOfRange = -2147204304;

		public const int BaseCurrencyOverflow = -2147185428;

		public const int BaseCurrencyUnderflow = -2147185429;

		public const int ActiveStageIsNotOnLeadEntity = -2146435070;
	}
}
