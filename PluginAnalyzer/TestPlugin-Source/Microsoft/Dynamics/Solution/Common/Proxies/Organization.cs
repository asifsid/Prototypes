using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Microsoft.Dynamics.Solution.Common.Proxies
{
	[DataContract]
	[EntityLogicalName("organization")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Organization : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "organization";

		public const int EntityTypeCode = 1019;

		public const int AttributeOrgDbOrgSettings_MaxLength = 1073741823;

		public const int AttributeBulkOperationPrefix_MaxLength = 20;

		public const int AttributeInvoicePrefix_MaxLength = 20;

		public const int AttributeReferenceSiteMapXml_MaxLength = 1073741823;

		public const int AttributeParsedTableColumnPrefix_MaxLength = 20;

		public const int AttributeDisabledReason_MaxLength = 500;

		public const int AttributeDateFormatString_MaxLength = 255;

		public const int AttributeFiscalPeriodFormat_MaxLength = 25;

		public const int AttributeKbPrefix_MaxLength = 20;

		public const int AttributeHashFilterKeywords_MaxLength = 1073741823;

		public const int AttributeSlaPauseStates_MaxLength = 1073741823;

		public const int AttributeSchemaNamePrefix_MaxLength = 8;

		public const int AttributeNumberSeparator_MaxLength = 5;

		public const int AttributeOrderPrefix_MaxLength = 20;

		public const int AttributeTokenKey_MaxLength = 90;

		public const int AttributeEntityImage_URL_MaxLength = 200;

		public const int AttributeWebResourceHash_MaxLength = 100;

		public const int AttributeContractPrefix_MaxLength = 20;

		public const int AttributeBingMapsApiKey_MaxLength = 1024;

		public const int AttributeNumberGroupFormat_MaxLength = 50;

		public const int AttributeAzureSchedulerJobCollectionName_MaxLength = 100;

		public const int AttributeDecimalSymbol_MaxLength = 5;

		public const int AttributeSignupOutlookDownloadFWLink_MaxLength = 200;

		public const int AttributeCasePrefix_MaxLength = 20;

		public const int AttributeFiscalYearFormat_MaxLength = 25;

		public const int AttributeInitialVersion_MaxLength = 20;

		public const int AttributeTrackingPrefix_MaxLength = 256;

		public const int AttributeYammerNetworkPermalink_MaxLength = 100;

		public const int AttributePMDesignator_MaxLength = 25;

		public const int AttributeExternalPartyCorrelationKeys_MaxLength = 1073741823;

		public const int AttributeDefaultCountryCode_MaxLength = 30;

		public const int AttributeReportingGroupName_MaxLength = 256;

		public const int AttributeExternalPartyEntitySettings_MaxLength = 1073741823;

		public const int AttributePrivReportingGroupName_MaxLength = 256;

		public const int AttributeCategoryPrefix_MaxLength = 20;

		public const int AttributeFeatureSet_MaxLength = 1073741823;

		public const int AttributeSqlAccessGroupName_MaxLength = 256;

		public const int AttributeSocialInsightsInstance_MaxLength = 2048;

		public const int AttributeDefaultEmailSettings_MaxLength = 1073741823;

		public const int AttributeNumberFormat_MaxLength = 2;

		public const int AttributeOfficeGraphDelveUrl_MaxLength = 1000;

		public const int AttributeV3CalloutConfigHash_MaxLength = 256;

		public const int AttributeExternalBaseUrl_MaxLength = 500;

		public const int AttributeCurrencySymbol_MaxLength = 13;

		public const int AttributeBaseCurrencySymbol_MaxLength = 5;

		public const int AttributeKaPrefix_MaxLength = 20;

		public const int AttributeAMDesignator_MaxLength = 25;

		public const int AttributeName_MaxLength = 160;

		public const int AttributePrivacyStatementUrl_MaxLength = 500;

		public const int AttributeQuotePrefix_MaxLength = 20;

		public const int AttributeTimeSeparator_MaxLength = 5;

		public const int AttributeParsedTablePrefix_MaxLength = 20;

		public const int AttributeTimeFormatString_MaxLength = 255;

		public const int AttributeFiscalYearPeriodConnect_MaxLength = 5;

		public const int AttributeCampaignPrefix_MaxLength = 20;

		public const int AttributeDateSeparator_MaxLength = 5;

		public const int AttributeKMSettings_MaxLength = 1073741823;

		public const int AttributeGlobalHelpUrl_MaxLength = 500;

		public const int AttributeBlockedAttachments_MaxLength = 1073741823;

		public const string AttributeAcknowledgementTemplateId = "acknowledgementtemplateid";

		public const string AttributeAllowAddressBookSyncs = "allowaddressbooksyncs";

		public const string AttributeAllowAutoResponseCreation = "allowautoresponsecreation";

		public const string AttributeAllowAutoUnsubscribe = "allowautounsubscribe";

		public const string AttributeAllowAutoUnsubscribeAcknowledgement = "allowautounsubscribeacknowledgement";

		public const string AttributeAllowClientMessageBarAd = "allowclientmessagebarad";

		public const string AttributeAllowEntityOnlyAudit = "allowentityonlyaudit";

		public const string AttributeAllowMarketingEmailExecution = "allowmarketingemailexecution";

		public const string AttributeAllowOfflineScheduledSyncs = "allowofflinescheduledsyncs";

		public const string AttributeAllowOutlookScheduledSyncs = "allowoutlookscheduledsyncs";

		public const string AttributeAllowUnresolvedPartiesOnEmailSend = "allowunresolvedpartiesonemailsend";

		public const string AttributeAllowUserFormModePreference = "allowuserformmodepreference";

		public const string AttributeAllowUsersSeeAppdownloadMessage = "allowusersseeappdownloadmessage";

		public const string AttributeAllowWebExcelExport = "allowwebexcelexport";

		public const string AttributeAMDesignator = "amdesignator";

		public const string AttributeAutoApplyDefaultonCaseCreate = "autoapplydefaultoncasecreate";

		public const string AttributeAutoApplyDefaultonCaseUpdate = "autoapplydefaultoncaseupdate";

		public const string AttributeAutoApplySLA = "autoapplysla";

		public const string AttributeAzureSchedulerJobCollectionName = "azureschedulerjobcollectionname";

		public const string AttributeBaseCurrencyId = "basecurrencyid";

		public const string AttributeBaseCurrencyPrecision = "basecurrencyprecision";

		public const string AttributeBaseCurrencySymbol = "basecurrencysymbol";

		public const string AttributeBingMapsApiKey = "bingmapsapikey";

		public const string AttributeBlockedAttachments = "blockedattachments";

		public const string AttributeBulkOperationPrefix = "bulkoperationprefix";

		public const string AttributeBusinessClosureCalendarId = "businessclosurecalendarid";

		public const string AttributeCalendarType = "calendartype";

		public const string AttributeCampaignPrefix = "campaignprefix";

		public const string AttributeCascadeStatusUpdate = "cascadestatusupdate";

		public const string AttributeCasePrefix = "caseprefix";

		public const string AttributeCategoryPrefix = "categoryprefix";

		public const string AttributeContractPrefix = "contractprefix";

		public const string AttributeCortanaProactiveExperienceEnabled = "cortanaproactiveexperienceenabled";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeCreateProductsWithoutParentInActiveState = "createproductswithoutparentinactivestate";

		public const string AttributeCurrencyDecimalPrecision = "currencydecimalprecision";

		public const string AttributeCurrencyDisplayOption = "currencydisplayoption";

		public const string AttributeCurrencyFormatCode = "currencyformatcode";

		public const string AttributeCurrencySymbol = "currencysymbol";

		public const string AttributeCurrentBulkOperationNumber = "currentbulkoperationnumber";

		public const string AttributeCurrentCampaignNumber = "currentcampaignnumber";

		public const string AttributeCurrentCaseNumber = "currentcasenumber";

		public const string AttributeCurrentCategoryNumber = "currentcategorynumber";

		public const string AttributeCurrentContractNumber = "currentcontractnumber";

		public const string AttributeCurrentImportSequenceNumber = "currentimportsequencenumber";

		public const string AttributeCurrentInvoiceNumber = "currentinvoicenumber";

		public const string AttributeCurrentKaNumber = "currentkanumber";

		public const string AttributeCurrentKbNumber = "currentkbnumber";

		public const string AttributeCurrentOrderNumber = "currentordernumber";

		public const string AttributeCurrentParsedTableNumber = "currentparsedtablenumber";

		public const string AttributeCurrentQuoteNumber = "currentquotenumber";

		public const string AttributeDateFormatCode = "dateformatcode";

		public const string AttributeDateFormatString = "dateformatstring";

		public const string AttributeDateSeparator = "dateseparator";

		public const string AttributeDaysSinceRecordLastModifiedMaxValue = "dayssincerecordlastmodifiedmaxvalue";

		public const string AttributeDecimalSymbol = "decimalsymbol";

		public const string AttributeDefaultCountryCode = "defaultcountrycode";

		public const string AttributeDefaultEmailServerProfileId = "defaultemailserverprofileid";

		public const string AttributeDefaultEmailSettings = "defaultemailsettings";

		public const string AttributeDefaultMobileOfflineProfileId = "defaultmobileofflineprofileid";

		public const string AttributeDefaultRecurrenceEndRangeType = "defaultrecurrenceendrangetype";

		public const string AttributeDefaultThemeData = "defaultthemedata";

		public const string AttributeDelegatedAdminUserId = "delegatedadminuserid";

		public const string AttributeDisabledReason = "disabledreason";

		public const string AttributeDisableSocialCare = "disablesocialcare";

		public const string AttributeDiscountCalculationMethod = "discountcalculationmethod";

		public const string AttributeDisplayNavigationTour = "displaynavigationtour";

		public const string AttributeEmailConnectionChannel = "emailconnectionchannel";

		public const string AttributeEmailCorrelationEnabled = "emailcorrelationenabled";

		public const string AttributeEmailSendPollingPeriod = "emailsendpollingperiod";

		public const string AttributeEnableBingMapsIntegration = "enablebingmapsintegration";

		public const string AttributeEnablePricingOnCreate = "enablepricingoncreate";

		public const string AttributeEnableSmartMatching = "enablesmartmatching";

		public const string AttributeEnforceReadOnlyPlugins = "enforcereadonlyplugins";

		public const string AttributeEntityImage = "entityimage";

		public const string AttributeEntityImage_Timestamp = "entityimage_timestamp";

		public const string AttributeEntityImage_URL = "entityimage_url";

		public const string AttributeEntityImageId = "entityimageid";

		public const string AttributeExpireChangeTrackingInDays = "expirechangetrackingindays";

		public const string AttributeExpireSubscriptionsInDays = "expiresubscriptionsindays";

		public const string AttributeExternalBaseUrl = "externalbaseurl";

		public const string AttributeExternalPartyCorrelationKeys = "externalpartycorrelationkeys";

		public const string AttributeExternalPartyEntitySettings = "externalpartyentitysettings";

		public const string AttributeFeatureSet = "featureset";

		public const string AttributeFiscalCalendarStart = "fiscalcalendarstart";

		public const string AttributeFiscalPeriodFormat = "fiscalperiodformat";

		public const string AttributeFiscalPeriodFormatPeriod = "fiscalperiodformatperiod";

		public const string AttributeFiscalPeriodType = "fiscalperiodtype";

		public const string AttributeFiscalSettingsUpdated = "fiscalsettingsupdated";

		public const string AttributeFiscalYearDisplayCode = "fiscalyeardisplaycode";

		public const string AttributeFiscalYearFormat = "fiscalyearformat";

		public const string AttributeFiscalYearFormatPrefix = "fiscalyearformatprefix";

		public const string AttributeFiscalYearFormatSuffix = "fiscalyearformatsuffix";

		public const string AttributeFiscalYearFormatYear = "fiscalyearformatyear";

		public const string AttributeFiscalYearPeriodConnect = "fiscalyearperiodconnect";

		public const string AttributeFullNameConventionCode = "fullnameconventioncode";

		public const string AttributeFutureExpansionWindow = "futureexpansionwindow";

		public const string AttributeGenerateAlertsForErrors = "generatealertsforerrors";

		public const string AttributeGenerateAlertsForInformation = "generatealertsforinformation";

		public const string AttributeGenerateAlertsForWarnings = "generatealertsforwarnings";

		public const string AttributeGetStartedPaneContentEnabled = "getstartedpanecontentenabled";

		public const string AttributeGlobalAppendUrlParametersEnabled = "globalappendurlparametersenabled";

		public const string AttributeGlobalHelpUrl = "globalhelpurl";

		public const string AttributeGlobalHelpUrlEnabled = "globalhelpurlenabled";

		public const string AttributeGoalRollupExpiryTime = "goalrollupexpirytime";

		public const string AttributeGoalRollupFrequency = "goalrollupfrequency";

		public const string AttributeGrantAccessToNetworkService = "grantaccesstonetworkservice";

		public const string AttributeHashDeltaSubjectCount = "hashdeltasubjectcount";

		public const string AttributeHashFilterKeywords = "hashfilterkeywords";

		public const string AttributeHashMaxCount = "hashmaxcount";

		public const string AttributeHashMinAddressCount = "hashminaddresscount";

		public const string AttributeHighContrastThemeData = "highcontrastthemedata";

		public const string AttributeIgnoreInternalEmail = "ignoreinternalemail";

		public const string AttributeIncomingEmailExchangeEmailRetrievalBatchSize = "incomingemailexchangeemailretrievalbatchsize";

		public const string AttributeInitialVersion = "initialversion";

		public const string AttributeIntegrationUserId = "integrationuserid";

		public const string AttributeInvoicePrefix = "invoiceprefix";

		public const string AttributeIsActionCardEnabled = "isactioncardenabled";

		public const string AttributeIsActivityAnalysisEnabled = "isactivityanalysisenabled";

		public const string AttributeIsAppMode = "isappmode";

		public const string AttributeIsAppointmentAttachmentSyncEnabled = "isappointmentattachmentsyncenabled";

		public const string AttributeIsAssignedTasksSyncEnabled = "isassignedtaskssyncenabled";

		public const string AttributeIsAuditEnabled = "isauditenabled";

		public const string AttributeIsAutoDataCaptureEnabled = "isautodatacaptureenabled";

		public const string AttributeIsAutoSaveEnabled = "isautosaveenabled";

		public const string AttributeIsConflictDetectionEnabledForMobileClient = "isconflictdetectionenabledformobileclient";

		public const string AttributeIsContactMailingAddressSyncEnabled = "iscontactmailingaddresssyncenabled";

		public const string AttributeIsDefaultCountryCodeCheckEnabled = "isdefaultcountrycodecheckenabled";

		public const string AttributeIsDelegateAccessEnabled = "isdelegateaccessenabled";

		public const string AttributeIsDelveActionHubIntegrationEnabled = "isdelveactionhubintegrationenabled";

		public const string AttributeIsDisabled = "isdisabled";

		public const string AttributeIsDuplicateDetectionEnabled = "isduplicatedetectionenabled";

		public const string AttributeIsDuplicateDetectionEnabledForImport = "isduplicatedetectionenabledforimport";

		public const string AttributeIsDuplicateDetectionEnabledForOfflineSync = "isduplicatedetectionenabledforofflinesync";

		public const string AttributeIsDuplicateDetectionEnabledForOnlineCreateUpdate = "isduplicatedetectionenabledforonlinecreateupdate";

		public const string AttributeIsEmailMonitoringAllowed = "isemailmonitoringallowed";

		public const string AttributeIsEmailServerProfileContentFilteringEnabled = "isemailserverprofilecontentfilteringenabled";

		public const string AttributeIsExternalSearchIndexEnabled = "isexternalsearchindexenabled";

		public const string AttributeIsFiscalPeriodMonthBased = "isfiscalperiodmonthbased";

		public const string AttributeIsFolderAutoCreatedonSP = "isfolderautocreatedonsp";

		public const string AttributeIsFolderBasedTrackingEnabled = "isfolderbasedtrackingenabled";

		public const string AttributeIsFullTextSearchEnabled = "isfulltextsearchenabled";

		public const string AttributeIsHierarchicalSecurityModelEnabled = "ishierarchicalsecuritymodelenabled";

		public const string AttributeIsMailboxForcedUnlockingEnabled = "ismailboxforcedunlockingenabled";

		public const string AttributeIsMailboxInactiveBackoffEnabled = "ismailboxinactivebackoffenabled";

		public const string AttributeIsMobileOfflineEnabled = "ismobileofflineenabled";

		public const string AttributeIsOfficeGraphEnabled = "isofficegraphenabled";

		public const string AttributeIsOneDriveEnabled = "isonedriveenabled";

		public const string AttributeIsPresenceEnabled = "ispresenceenabled";

		public const string AttributeIsPreviewEnabledForActionCard = "ispreviewenabledforactioncard";

		public const string AttributeIsSOPIntegrationEnabled = "issopintegrationenabled";

		public const string AttributeIsUserAccessAuditEnabled = "isuseraccessauditenabled";

		public const string AttributeISVIntegrationCode = "isvintegrationcode";

		public const string AttributeKaPrefix = "kaprefix";

		public const string AttributeKbPrefix = "kbprefix";

		public const string AttributeKMSettings = "kmsettings";

		public const string AttributeLanguageCode = "languagecode";

		public const string AttributeLocaleId = "localeid";

		public const string AttributeLongDateFormatCode = "longdateformatcode";

		public const string AttributeMailboxIntermittentIssueMinRange = "mailboxintermittentissueminrange";

		public const string AttributeMailboxPermanentIssueMinRange = "mailboxpermanentissueminrange";

		public const string AttributeMaxAppointmentDurationDays = "maxappointmentdurationdays";

		public const string AttributeMaxConditionsForMobileOfflineFilters = "maxconditionsformobileofflinefilters";

		public const string AttributeMaxDepthForHierarchicalSecurityModel = "maxdepthforhierarchicalsecuritymodel";

		public const string AttributeMaxFolderBasedTrackingMappings = "maxfolderbasedtrackingmappings";

		public const string AttributeMaximumActiveBusinessProcessFlowsAllowedPerEntity = "maximumactivebusinessprocessflowsallowedperentity";

		public const string AttributeMaximumDynamicPropertiesAllowed = "maximumdynamicpropertiesallowed";

		public const string AttributeMaximumEntitiesWithActiveSLA = "maximumentitieswithactivesla";

		public const string AttributeMaximumSLAKPIPerEntityWithActiveSLA = "maximumslakpiperentitywithactivesla";

		public const string AttributeMaximumTrackingNumber = "maximumtrackingnumber";

		public const string AttributeMaxProductsInBundle = "maxproductsinbundle";

		public const string AttributeMaxRecordsForExportToExcel = "maxrecordsforexporttoexcel";

		public const string AttributeMaxRecordsForLookupFilters = "maxrecordsforlookupfilters";

		public const string AttributeMaxSupportedInternetExplorerVersion = "maxsupportedinternetexplorerversion";

		public const string AttributeMaxUploadFileSize = "maxuploadfilesize";

		public const string AttributeMaxVerboseLoggingMailbox = "maxverboseloggingmailbox";

		public const string AttributeMaxVerboseLoggingSyncCycles = "maxverboseloggingsynccycles";

		public const string AttributeMinAddressBookSyncInterval = "minaddressbooksyncinterval";

		public const string AttributeMinOfflineSyncInterval = "minofflinesyncinterval";

		public const string AttributeMinOutlookSyncInterval = "minoutlooksyncinterval";

		public const string AttributeMobileOfflineMinLicenseProd = "mobileofflineminlicenseprod";

		public const string AttributeMobileOfflineMinLicenseTrial = "mobileofflineminlicensetrial";

		public const string AttributeMobileOfflineSyncInterval = "mobileofflinesyncinterval";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeNegativeCurrencyFormatCode = "negativecurrencyformatcode";

		public const string AttributeNegativeFormatCode = "negativeformatcode";

		public const string AttributeNextTrackingNumber = "nexttrackingnumber";

		public const string AttributeNotifyMailboxOwnerOfEmailServerLevelAlerts = "notifymailboxownerofemailserverlevelalerts";

		public const string AttributeNumberFormat = "numberformat";

		public const string AttributeNumberGroupFormat = "numbergroupformat";

		public const string AttributeNumberSeparator = "numberseparator";

		public const string AttributeOfficeAppsAutoDeploymentEnabled = "officeappsautodeploymentenabled";

		public const string AttributeOfficeGraphDelveUrl = "officegraphdelveurl";

		public const string AttributeOOBPriceCalculationEnabled = "oobpricecalculationenabled";

		public const string AttributeOrderPrefix = "orderprefix";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeId = "organizationid";

		public const string AttributeOrgDbOrgSettings = "orgdborgsettings";

		public const string AttributeOrgInsightsEnabled = "orginsightsenabled";

		public const string AttributeParsedTableColumnPrefix = "parsedtablecolumnprefix";

		public const string AttributeParsedTablePrefix = "parsedtableprefix";

		public const string AttributePastExpansionWindow = "pastexpansionwindow";

		public const string AttributePicture = "picture";

		public const string AttributePinpointLanguageCode = "pinpointlanguagecode";

		public const string AttributePluginTraceLogSetting = "plugintracelogsetting";

		public const string AttributePMDesignator = "pmdesignator";

		public const string AttributePowerBiFeatureEnabled = "powerbifeatureenabled";

		public const string AttributePricingDecimalPrecision = "pricingdecimalprecision";

		public const string AttributePrivacyStatementUrl = "privacystatementurl";

		public const string AttributePrivilegeUserGroupId = "privilegeusergroupid";

		public const string AttributePrivReportingGroupId = "privreportinggroupid";

		public const string AttributePrivReportingGroupName = "privreportinggroupname";

		public const string AttributeProductRecommendationsEnabled = "productrecommendationsenabled";

		public const string AttributeQuickFindRecordLimitEnabled = "quickfindrecordlimitenabled";

		public const string AttributeQuotePrefix = "quoteprefix";

		public const string AttributeRecurrenceDefaultNumberOfOccurrences = "recurrencedefaultnumberofoccurrences";

		public const string AttributeRecurrenceExpansionJobBatchInterval = "recurrenceexpansionjobbatchinterval";

		public const string AttributeRecurrenceExpansionJobBatchSize = "recurrenceexpansionjobbatchsize";

		public const string AttributeRecurrenceExpansionSynchCreateMax = "recurrenceexpansionsynchcreatemax";

		public const string AttributeReferenceSiteMapXml = "referencesitemapxml";

		public const string AttributeRenderSecureIFrameForEmail = "rendersecureiframeforemail";

		public const string AttributeReportingGroupId = "reportinggroupid";

		public const string AttributeReportingGroupName = "reportinggroupname";

		public const string AttributeReportScriptErrors = "reportscripterrors";

		public const string AttributeRequireApprovalForQueueEmail = "requireapprovalforqueueemail";

		public const string AttributeRequireApprovalForUserEmail = "requireapprovalforuseremail";

		public const string AttributeRestrictStatusUpdate = "restrictstatusupdate";

		public const string AttributeSampleDataImportId = "sampledataimportid";

		public const string AttributeSchemaNamePrefix = "schemanameprefix";

		public const string AttributeSharePointDeploymentType = "sharepointdeploymenttype";

		public const string AttributeShareToPreviousOwnerOnAssign = "sharetopreviousowneronassign";

		public const string AttributeShowKBArticleDeprecationNotification = "showkbarticledeprecationnotification";

		public const string AttributeShowWeekNumber = "showweeknumber";

		public const string AttributeSignupOutlookDownloadFWLink = "signupoutlookdownloadfwlink";

		public const string AttributeSiteMapXml = "sitemapxml";

		public const string AttributeSlaPauseStates = "slapausestates";

		public const string AttributeSocialInsightsEnabled = "socialinsightsenabled";

		public const string AttributeSocialInsightsInstance = "socialinsightsinstance";

		public const string AttributeSocialInsightsTermsAccepted = "socialinsightstermsaccepted";

		public const string AttributeSortId = "sortid";

		public const string AttributeSqlAccessGroupId = "sqlaccessgroupid";

		public const string AttributeSqlAccessGroupName = "sqlaccessgroupname";

		public const string AttributeSQMEnabled = "sqmenabled";

		public const string AttributeSupportUserId = "supportuserid";

		public const string AttributeSuppressSLA = "suppresssla";

		public const string AttributeSystemUserId = "systemuserid";

		public const string AttributeTagMaxAggressiveCycles = "tagmaxaggressivecycles";

		public const string AttributeTagPollingPeriod = "tagpollingperiod";

		public const string AttributeTaskBasedFlowEnabled = "taskbasedflowenabled";

		public const string AttributeTextAnalyticsEnabled = "textanalyticsenabled";

		public const string AttributeTimeFormatCode = "timeformatcode";

		public const string AttributeTimeFormatString = "timeformatstring";

		public const string AttributeTimeSeparator = "timeseparator";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeTokenExpiry = "tokenexpiry";

		public const string AttributeTokenKey = "tokenkey";

		public const string AttributeTrackingPrefix = "trackingprefix";

		public const string AttributeTrackingTokenIdBase = "trackingtokenidbase";

		public const string AttributeTrackingTokenIdDigits = "trackingtokeniddigits";

		public const string AttributeUniqueSpecifierLength = "uniquespecifierlength";

		public const string AttributeUseInbuiltRuleForDefaultPricelistSelection = "useinbuiltrulefordefaultpricelistselection";

		public const string AttributeUseLegacyRendering = "uselegacyrendering";

		public const string AttributeUsePositionHierarchy = "usepositionhierarchy";

		public const string AttributeUserAccessAuditingInterval = "useraccessauditinginterval";

		public const string AttributeUseReadForm = "usereadform";

		public const string AttributeUserGroupId = "usergroupid";

		public const string AttributeUseSkypeProtocol = "useskypeprotocol";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeV3CalloutConfigHash = "v3calloutconfighash";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeWebResourceHash = "webresourcehash";

		public const string AttributeWeekStartDayCode = "weekstartdaycode";

		public const string AttributeYammerGroupId = "yammergroupid";

		public const string AttributeYammerNetworkPermalink = "yammernetworkpermalink";

		public const string AttributeYammerOAuthAccessTokenExpired = "yammeroauthaccesstokenexpired";

		public const string AttributeYammerPostMethod = "yammerpostmethod";

		public const string AttributeYearStartWeekCode = "yearstartweekcode";

		[AttributeLogicalName("acknowledgementtemplateid")]
		[ExcludeFromCodeCoverage]
		public EntityReference AcknowledgementTemplateId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("acknowledgementtemplateid");
			}
			set
			{
				OnPropertyChanging("AcknowledgementTemplateId");
				((Entity)this).SetAttributeValue("acknowledgementtemplateid", (object)value);
				OnPropertyChanged("AcknowledgementTemplateId");
			}
		}

		[AttributeLogicalName("allowaddressbooksyncs")]
		[ExcludeFromCodeCoverage]
		public bool? AllowAddressBookSyncs
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowaddressbooksyncs");
			}
			set
			{
				OnPropertyChanging("AllowAddressBookSyncs");
				((Entity)this).SetAttributeValue("allowaddressbooksyncs", (object)value);
				OnPropertyChanged("AllowAddressBookSyncs");
			}
		}

		[AttributeLogicalName("allowautoresponsecreation")]
		[ExcludeFromCodeCoverage]
		public bool? AllowAutoResponseCreation
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowautoresponsecreation");
			}
			set
			{
				OnPropertyChanging("AllowAutoResponseCreation");
				((Entity)this).SetAttributeValue("allowautoresponsecreation", (object)value);
				OnPropertyChanged("AllowAutoResponseCreation");
			}
		}

		[AttributeLogicalName("allowautounsubscribe")]
		[ExcludeFromCodeCoverage]
		public bool? AllowAutoUnsubscribe
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowautounsubscribe");
			}
			set
			{
				OnPropertyChanging("AllowAutoUnsubscribe");
				((Entity)this).SetAttributeValue("allowautounsubscribe", (object)value);
				OnPropertyChanged("AllowAutoUnsubscribe");
			}
		}

		[AttributeLogicalName("allowautounsubscribeacknowledgement")]
		[ExcludeFromCodeCoverage]
		public bool? AllowAutoUnsubscribeAcknowledgement
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowautounsubscribeacknowledgement");
			}
			set
			{
				OnPropertyChanging("AllowAutoUnsubscribeAcknowledgement");
				((Entity)this).SetAttributeValue("allowautounsubscribeacknowledgement", (object)value);
				OnPropertyChanged("AllowAutoUnsubscribeAcknowledgement");
			}
		}

		[AttributeLogicalName("allowclientmessagebarad")]
		[ExcludeFromCodeCoverage]
		public bool? AllowClientMessageBarAd
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowclientmessagebarad");
			}
			set
			{
				OnPropertyChanging("AllowClientMessageBarAd");
				((Entity)this).SetAttributeValue("allowclientmessagebarad", (object)value);
				OnPropertyChanged("AllowClientMessageBarAd");
			}
		}

		[AttributeLogicalName("allowentityonlyaudit")]
		[ExcludeFromCodeCoverage]
		public bool? AllowEntityOnlyAudit
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowentityonlyaudit");
			}
			set
			{
				OnPropertyChanging("AllowEntityOnlyAudit");
				((Entity)this).SetAttributeValue("allowentityonlyaudit", (object)value);
				OnPropertyChanged("AllowEntityOnlyAudit");
			}
		}

		[AttributeLogicalName("allowmarketingemailexecution")]
		[ExcludeFromCodeCoverage]
		public bool? AllowMarketingEmailExecution
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowmarketingemailexecution");
			}
			set
			{
				OnPropertyChanging("AllowMarketingEmailExecution");
				((Entity)this).SetAttributeValue("allowmarketingemailexecution", (object)value);
				OnPropertyChanged("AllowMarketingEmailExecution");
			}
		}

		[AttributeLogicalName("allowofflinescheduledsyncs")]
		[ExcludeFromCodeCoverage]
		public bool? AllowOfflineScheduledSyncs
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowofflinescheduledsyncs");
			}
			set
			{
				OnPropertyChanging("AllowOfflineScheduledSyncs");
				((Entity)this).SetAttributeValue("allowofflinescheduledsyncs", (object)value);
				OnPropertyChanged("AllowOfflineScheduledSyncs");
			}
		}

		[AttributeLogicalName("allowoutlookscheduledsyncs")]
		[ExcludeFromCodeCoverage]
		public bool? AllowOutlookScheduledSyncs
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowoutlookscheduledsyncs");
			}
			set
			{
				OnPropertyChanging("AllowOutlookScheduledSyncs");
				((Entity)this).SetAttributeValue("allowoutlookscheduledsyncs", (object)value);
				OnPropertyChanged("AllowOutlookScheduledSyncs");
			}
		}

		[AttributeLogicalName("allowunresolvedpartiesonemailsend")]
		[ExcludeFromCodeCoverage]
		public bool? AllowUnresolvedPartiesOnEmailSend
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowunresolvedpartiesonemailsend");
			}
			set
			{
				OnPropertyChanging("AllowUnresolvedPartiesOnEmailSend");
				((Entity)this).SetAttributeValue("allowunresolvedpartiesonemailsend", (object)value);
				OnPropertyChanged("AllowUnresolvedPartiesOnEmailSend");
			}
		}

		[AttributeLogicalName("allowuserformmodepreference")]
		[ExcludeFromCodeCoverage]
		public bool? AllowUserFormModePreference
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowuserformmodepreference");
			}
			set
			{
				OnPropertyChanging("AllowUserFormModePreference");
				((Entity)this).SetAttributeValue("allowuserformmodepreference", (object)value);
				OnPropertyChanged("AllowUserFormModePreference");
			}
		}

		[AttributeLogicalName("allowusersseeappdownloadmessage")]
		[ExcludeFromCodeCoverage]
		public bool? AllowUsersSeeAppdownloadMessage
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowusersseeappdownloadmessage");
			}
			set
			{
				OnPropertyChanging("AllowUsersSeeAppdownloadMessage");
				((Entity)this).SetAttributeValue("allowusersseeappdownloadmessage", (object)value);
				OnPropertyChanged("AllowUsersSeeAppdownloadMessage");
			}
		}

		[AttributeLogicalName("allowwebexcelexport")]
		[ExcludeFromCodeCoverage]
		public bool? AllowWebExcelExport
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("allowwebexcelexport");
			}
			set
			{
				OnPropertyChanging("AllowWebExcelExport");
				((Entity)this).SetAttributeValue("allowwebexcelexport", (object)value);
				OnPropertyChanged("AllowWebExcelExport");
			}
		}

		[AttributeLogicalName("amdesignator")]
		[ExcludeFromCodeCoverage]
		public string AMDesignator
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("amdesignator");
			}
			set
			{
				OnPropertyChanging("AMDesignator");
				((Entity)this).SetAttributeValue("amdesignator", (object)value);
				OnPropertyChanged("AMDesignator");
			}
		}

		[AttributeLogicalName("autoapplydefaultoncasecreate")]
		[ExcludeFromCodeCoverage]
		public bool? AutoApplyDefaultonCaseCreate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("autoapplydefaultoncasecreate");
			}
			set
			{
				OnPropertyChanging("AutoApplyDefaultonCaseCreate");
				((Entity)this).SetAttributeValue("autoapplydefaultoncasecreate", (object)value);
				OnPropertyChanged("AutoApplyDefaultonCaseCreate");
			}
		}

		[AttributeLogicalName("autoapplydefaultoncaseupdate")]
		[ExcludeFromCodeCoverage]
		public bool? AutoApplyDefaultonCaseUpdate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("autoapplydefaultoncaseupdate");
			}
			set
			{
				OnPropertyChanging("AutoApplyDefaultonCaseUpdate");
				((Entity)this).SetAttributeValue("autoapplydefaultoncaseupdate", (object)value);
				OnPropertyChanged("AutoApplyDefaultonCaseUpdate");
			}
		}

		[AttributeLogicalName("autoapplysla")]
		[ExcludeFromCodeCoverage]
		public bool? AutoApplySLA
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("autoapplysla");
			}
			set
			{
				OnPropertyChanging("AutoApplySLA");
				((Entity)this).SetAttributeValue("autoapplysla", (object)value);
				OnPropertyChanged("AutoApplySLA");
			}
		}

		[AttributeLogicalName("azureschedulerjobcollectionname")]
		[ExcludeFromCodeCoverage]
		public string AzureSchedulerJobCollectionName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("azureschedulerjobcollectionname");
			}
			set
			{
				OnPropertyChanging("AzureSchedulerJobCollectionName");
				((Entity)this).SetAttributeValue("azureschedulerjobcollectionname", (object)value);
				OnPropertyChanged("AzureSchedulerJobCollectionName");
			}
		}

		[AttributeLogicalName("basecurrencyid")]
		[ExcludeFromCodeCoverage]
		public EntityReference BaseCurrencyId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("basecurrencyid");
			}
			set
			{
				OnPropertyChanging("BaseCurrencyId");
				((Entity)this).SetAttributeValue("basecurrencyid", (object)value);
				OnPropertyChanged("BaseCurrencyId");
			}
		}

		[AttributeLogicalName("basecurrencyprecision")]
		[ExcludeFromCodeCoverage]
		public int? BaseCurrencyPrecision => ((Entity)this).GetAttributeValue<int?>("basecurrencyprecision");

		[AttributeLogicalName("basecurrencysymbol")]
		[ExcludeFromCodeCoverage]
		public string BaseCurrencySymbol => ((Entity)this).GetAttributeValue<string>("basecurrencysymbol");

		[AttributeLogicalName("bingmapsapikey")]
		[ExcludeFromCodeCoverage]
		public string BingMapsApiKey
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("bingmapsapikey");
			}
			set
			{
				OnPropertyChanging("BingMapsApiKey");
				((Entity)this).SetAttributeValue("bingmapsapikey", (object)value);
				OnPropertyChanged("BingMapsApiKey");
			}
		}

		[AttributeLogicalName("blockedattachments")]
		[ExcludeFromCodeCoverage]
		public string BlockedAttachments
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("blockedattachments");
			}
			set
			{
				OnPropertyChanging("BlockedAttachments");
				((Entity)this).SetAttributeValue("blockedattachments", (object)value);
				OnPropertyChanged("BlockedAttachments");
			}
		}

		[AttributeLogicalName("bulkoperationprefix")]
		[ExcludeFromCodeCoverage]
		public string BulkOperationPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("bulkoperationprefix");
			}
			set
			{
				OnPropertyChanging("BulkOperationPrefix");
				((Entity)this).SetAttributeValue("bulkoperationprefix", (object)value);
				OnPropertyChanged("BulkOperationPrefix");
			}
		}

		[AttributeLogicalName("businessclosurecalendarid")]
		[ExcludeFromCodeCoverage]
		public Guid? BusinessClosureCalendarId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("businessclosurecalendarid");
			}
			set
			{
				OnPropertyChanging("BusinessClosureCalendarId");
				((Entity)this).SetAttributeValue("businessclosurecalendarid", (object)value);
				OnPropertyChanged("BusinessClosureCalendarId");
			}
		}

		[AttributeLogicalName("calendartype")]
		[ExcludeFromCodeCoverage]
		public int? CalendarType
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("calendartype");
			}
			set
			{
				OnPropertyChanging("CalendarType");
				((Entity)this).SetAttributeValue("calendartype", (object)value);
				OnPropertyChanged("CalendarType");
			}
		}

		[AttributeLogicalName("campaignprefix")]
		[ExcludeFromCodeCoverage]
		public string CampaignPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("campaignprefix");
			}
			set
			{
				OnPropertyChanging("CampaignPrefix");
				((Entity)this).SetAttributeValue("campaignprefix", (object)value);
				OnPropertyChanged("CampaignPrefix");
			}
		}

		[AttributeLogicalName("cascadestatusupdate")]
		[ExcludeFromCodeCoverage]
		public bool? CascadeStatusUpdate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("cascadestatusupdate");
			}
			set
			{
				OnPropertyChanging("CascadeStatusUpdate");
				((Entity)this).SetAttributeValue("cascadestatusupdate", (object)value);
				OnPropertyChanged("CascadeStatusUpdate");
			}
		}

		[AttributeLogicalName("caseprefix")]
		[ExcludeFromCodeCoverage]
		public string CasePrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("caseprefix");
			}
			set
			{
				OnPropertyChanging("CasePrefix");
				((Entity)this).SetAttributeValue("caseprefix", (object)value);
				OnPropertyChanged("CasePrefix");
			}
		}

		[AttributeLogicalName("categoryprefix")]
		[ExcludeFromCodeCoverage]
		public string CategoryPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("categoryprefix");
			}
			set
			{
				OnPropertyChanging("CategoryPrefix");
				((Entity)this).SetAttributeValue("categoryprefix", (object)value);
				OnPropertyChanged("CategoryPrefix");
			}
		}

		[AttributeLogicalName("contractprefix")]
		[ExcludeFromCodeCoverage]
		public string ContractPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("contractprefix");
			}
			set
			{
				OnPropertyChanging("ContractPrefix");
				((Entity)this).SetAttributeValue("contractprefix", (object)value);
				OnPropertyChanged("ContractPrefix");
			}
		}

		[AttributeLogicalName("cortanaproactiveexperienceenabled")]
		[ExcludeFromCodeCoverage]
		public bool? CortanaProactiveExperienceEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("cortanaproactiveexperienceenabled");
			}
			set
			{
				OnPropertyChanging("CortanaProactiveExperienceEnabled");
				((Entity)this).SetAttributeValue("cortanaproactiveexperienceenabled", (object)value);
				OnPropertyChanged("CortanaProactiveExperienceEnabled");
			}
		}

		[AttributeLogicalName("createdby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedBy => ((Entity)this).GetAttributeValue<EntityReference>("createdby");

		[AttributeLogicalName("createdon")]
		[ExcludeFromCodeCoverage]
		public DateTime? CreatedOn => ((Entity)this).GetAttributeValue<DateTime?>("createdon");

		[AttributeLogicalName("createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("createdonbehalfby");

		[AttributeLogicalName("createproductswithoutparentinactivestate")]
		[ExcludeFromCodeCoverage]
		public bool? CreateProductsWithoutParentInActiveState
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("createproductswithoutparentinactivestate");
			}
			set
			{
				OnPropertyChanging("CreateProductsWithoutParentInActiveState");
				((Entity)this).SetAttributeValue("createproductswithoutparentinactivestate", (object)value);
				OnPropertyChanged("CreateProductsWithoutParentInActiveState");
			}
		}

		[AttributeLogicalName("currencydecimalprecision")]
		[ExcludeFromCodeCoverage]
		public int? CurrencyDecimalPrecision
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currencydecimalprecision");
			}
			set
			{
				OnPropertyChanging("CurrencyDecimalPrecision");
				((Entity)this).SetAttributeValue("currencydecimalprecision", (object)value);
				OnPropertyChanged("CurrencyDecimalPrecision");
			}
		}

		[AttributeLogicalName("currencydisplayoption")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue CurrencyDisplayOption
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("currencydisplayoption");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("CurrencyDisplayOption");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("currencydisplayoption", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("currencydisplayoption", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("CurrencyDisplayOption");
			}
		}

		[AttributeLogicalName("currencyformatcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue CurrencyFormatCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("currencyformatcode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("CurrencyFormatCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("currencyformatcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("currencyformatcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("CurrencyFormatCode");
			}
		}

		[AttributeLogicalName("currencysymbol")]
		[ExcludeFromCodeCoverage]
		public string CurrencySymbol
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("currencysymbol");
			}
			set
			{
				OnPropertyChanging("CurrencySymbol");
				((Entity)this).SetAttributeValue("currencysymbol", (object)value);
				OnPropertyChanged("CurrencySymbol");
			}
		}

		[AttributeLogicalName("currentbulkoperationnumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentBulkOperationNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentbulkoperationnumber");
			}
			set
			{
				OnPropertyChanging("CurrentBulkOperationNumber");
				((Entity)this).SetAttributeValue("currentbulkoperationnumber", (object)value);
				OnPropertyChanged("CurrentBulkOperationNumber");
			}
		}

		[AttributeLogicalName("currentcampaignnumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentCampaignNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentcampaignnumber");
			}
			set
			{
				OnPropertyChanging("CurrentCampaignNumber");
				((Entity)this).SetAttributeValue("currentcampaignnumber", (object)value);
				OnPropertyChanged("CurrentCampaignNumber");
			}
		}

		[AttributeLogicalName("currentcasenumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentCaseNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentcasenumber");
			}
			set
			{
				OnPropertyChanging("CurrentCaseNumber");
				((Entity)this).SetAttributeValue("currentcasenumber", (object)value);
				OnPropertyChanged("CurrentCaseNumber");
			}
		}

		[AttributeLogicalName("currentcategorynumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentCategoryNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentcategorynumber");
			}
			set
			{
				OnPropertyChanging("CurrentCategoryNumber");
				((Entity)this).SetAttributeValue("currentcategorynumber", (object)value);
				OnPropertyChanged("CurrentCategoryNumber");
			}
		}

		[AttributeLogicalName("currentcontractnumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentContractNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentcontractnumber");
			}
			set
			{
				OnPropertyChanging("CurrentContractNumber");
				((Entity)this).SetAttributeValue("currentcontractnumber", (object)value);
				OnPropertyChanged("CurrentContractNumber");
			}
		}

		[AttributeLogicalName("currentimportsequencenumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentImportSequenceNumber => ((Entity)this).GetAttributeValue<int?>("currentimportsequencenumber");

		[AttributeLogicalName("currentinvoicenumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentInvoiceNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentinvoicenumber");
			}
			set
			{
				OnPropertyChanging("CurrentInvoiceNumber");
				((Entity)this).SetAttributeValue("currentinvoicenumber", (object)value);
				OnPropertyChanged("CurrentInvoiceNumber");
			}
		}

		[AttributeLogicalName("currentkanumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentKaNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentkanumber");
			}
			set
			{
				OnPropertyChanging("CurrentKaNumber");
				((Entity)this).SetAttributeValue("currentkanumber", (object)value);
				OnPropertyChanged("CurrentKaNumber");
			}
		}

		[AttributeLogicalName("currentkbnumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentKbNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentkbnumber");
			}
			set
			{
				OnPropertyChanging("CurrentKbNumber");
				((Entity)this).SetAttributeValue("currentkbnumber", (object)value);
				OnPropertyChanged("CurrentKbNumber");
			}
		}

		[AttributeLogicalName("currentordernumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentOrderNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentordernumber");
			}
			set
			{
				OnPropertyChanging("CurrentOrderNumber");
				((Entity)this).SetAttributeValue("currentordernumber", (object)value);
				OnPropertyChanged("CurrentOrderNumber");
			}
		}

		[AttributeLogicalName("currentparsedtablenumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentParsedTableNumber => ((Entity)this).GetAttributeValue<int?>("currentparsedtablenumber");

		[AttributeLogicalName("currentquotenumber")]
		[ExcludeFromCodeCoverage]
		public int? CurrentQuoteNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currentquotenumber");
			}
			set
			{
				OnPropertyChanging("CurrentQuoteNumber");
				((Entity)this).SetAttributeValue("currentquotenumber", (object)value);
				OnPropertyChanged("CurrentQuoteNumber");
			}
		}

		[AttributeLogicalName("dateformatcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue DateFormatCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("dateformatcode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("DateFormatCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("dateformatcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("dateformatcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("DateFormatCode");
			}
		}

		[AttributeLogicalName("dateformatstring")]
		[ExcludeFromCodeCoverage]
		public string DateFormatString
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("dateformatstring");
			}
			set
			{
				OnPropertyChanging("DateFormatString");
				((Entity)this).SetAttributeValue("dateformatstring", (object)value);
				OnPropertyChanged("DateFormatString");
			}
		}

		[AttributeLogicalName("dateseparator")]
		[ExcludeFromCodeCoverage]
		public string DateSeparator
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("dateseparator");
			}
			set
			{
				OnPropertyChanging("DateSeparator");
				((Entity)this).SetAttributeValue("dateseparator", (object)value);
				OnPropertyChanged("DateSeparator");
			}
		}

		[AttributeLogicalName("dayssincerecordlastmodifiedmaxvalue")]
		[ExcludeFromCodeCoverage]
		public int? DaysSinceRecordLastModifiedMaxValue => ((Entity)this).GetAttributeValue<int?>("dayssincerecordlastmodifiedmaxvalue");

		[AttributeLogicalName("decimalsymbol")]
		[ExcludeFromCodeCoverage]
		public string DecimalSymbol
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("decimalsymbol");
			}
			set
			{
				OnPropertyChanging("DecimalSymbol");
				((Entity)this).SetAttributeValue("decimalsymbol", (object)value);
				OnPropertyChanged("DecimalSymbol");
			}
		}

		[AttributeLogicalName("defaultcountrycode")]
		[ExcludeFromCodeCoverage]
		public string DefaultCountryCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("defaultcountrycode");
			}
			set
			{
				OnPropertyChanging("DefaultCountryCode");
				((Entity)this).SetAttributeValue("defaultcountrycode", (object)value);
				OnPropertyChanged("DefaultCountryCode");
			}
		}

		[AttributeLogicalName("defaultemailserverprofileid")]
		[ExcludeFromCodeCoverage]
		public EntityReference DefaultEmailServerProfileId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("defaultemailserverprofileid");
			}
			set
			{
				OnPropertyChanging("DefaultEmailServerProfileId");
				((Entity)this).SetAttributeValue("defaultemailserverprofileid", (object)value);
				OnPropertyChanged("DefaultEmailServerProfileId");
			}
		}

		[AttributeLogicalName("defaultemailsettings")]
		[ExcludeFromCodeCoverage]
		public string DefaultEmailSettings
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("defaultemailsettings");
			}
			set
			{
				OnPropertyChanging("DefaultEmailSettings");
				((Entity)this).SetAttributeValue("defaultemailsettings", (object)value);
				OnPropertyChanged("DefaultEmailSettings");
			}
		}

		[AttributeLogicalName("defaultmobileofflineprofileid")]
		[ExcludeFromCodeCoverage]
		public EntityReference DefaultMobileOfflineProfileId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("defaultmobileofflineprofileid");
			}
			set
			{
				OnPropertyChanging("DefaultMobileOfflineProfileId");
				((Entity)this).SetAttributeValue("defaultmobileofflineprofileid", (object)value);
				OnPropertyChanged("DefaultMobileOfflineProfileId");
			}
		}

		[AttributeLogicalName("defaultrecurrenceendrangetype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue DefaultRecurrenceEndRangeType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("defaultrecurrenceendrangetype");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("DefaultRecurrenceEndRangeType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("defaultrecurrenceendrangetype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("defaultrecurrenceendrangetype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("DefaultRecurrenceEndRangeType");
			}
		}

		[AttributeLogicalName("defaultthemedata")]
		[ExcludeFromCodeCoverage]
		public string DefaultThemeData
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("defaultthemedata");
			}
			set
			{
				OnPropertyChanging("DefaultThemeData");
				((Entity)this).SetAttributeValue("defaultthemedata", (object)value);
				OnPropertyChanged("DefaultThemeData");
			}
		}

		[AttributeLogicalName("delegatedadminuserid")]
		[ExcludeFromCodeCoverage]
		public Guid? DelegatedAdminUserId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("delegatedadminuserid");
			}
			set
			{
				OnPropertyChanging("DelegatedAdminUserId");
				((Entity)this).SetAttributeValue("delegatedadminuserid", (object)value);
				OnPropertyChanged("DelegatedAdminUserId");
			}
		}

		[AttributeLogicalName("disabledreason")]
		[ExcludeFromCodeCoverage]
		public string DisabledReason => ((Entity)this).GetAttributeValue<string>("disabledreason");

		[AttributeLogicalName("disablesocialcare")]
		[ExcludeFromCodeCoverage]
		public bool? DisableSocialCare
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("disablesocialcare");
			}
			set
			{
				OnPropertyChanging("DisableSocialCare");
				((Entity)this).SetAttributeValue("disablesocialcare", (object)value);
				OnPropertyChanged("DisableSocialCare");
			}
		}

		[AttributeLogicalName("discountcalculationmethod")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue DiscountCalculationMethod
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("discountcalculationmethod");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("DiscountCalculationMethod");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("discountcalculationmethod", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("discountcalculationmethod", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("DiscountCalculationMethod");
			}
		}

		[AttributeLogicalName("displaynavigationtour")]
		[ExcludeFromCodeCoverage]
		public bool? DisplayNavigationTour
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("displaynavigationtour");
			}
			set
			{
				OnPropertyChanging("DisplayNavigationTour");
				((Entity)this).SetAttributeValue("displaynavigationtour", (object)value);
				OnPropertyChanged("DisplayNavigationTour");
			}
		}

		[AttributeLogicalName("emailconnectionchannel")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue EmailConnectionChannel
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("emailconnectionchannel");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("EmailConnectionChannel");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("emailconnectionchannel", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("emailconnectionchannel", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("EmailConnectionChannel");
			}
		}

		[AttributeLogicalName("emailcorrelationenabled")]
		[ExcludeFromCodeCoverage]
		public bool? EmailCorrelationEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("emailcorrelationenabled");
			}
			set
			{
				OnPropertyChanging("EmailCorrelationEnabled");
				((Entity)this).SetAttributeValue("emailcorrelationenabled", (object)value);
				OnPropertyChanged("EmailCorrelationEnabled");
			}
		}

		[AttributeLogicalName("emailsendpollingperiod")]
		[ExcludeFromCodeCoverage]
		public int? EmailSendPollingPeriod
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("emailsendpollingperiod");
			}
			set
			{
				OnPropertyChanging("EmailSendPollingPeriod");
				((Entity)this).SetAttributeValue("emailsendpollingperiod", (object)value);
				OnPropertyChanged("EmailSendPollingPeriod");
			}
		}

		[AttributeLogicalName("enablebingmapsintegration")]
		[ExcludeFromCodeCoverage]
		public bool? EnableBingMapsIntegration
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("enablebingmapsintegration");
			}
			set
			{
				OnPropertyChanging("EnableBingMapsIntegration");
				((Entity)this).SetAttributeValue("enablebingmapsintegration", (object)value);
				OnPropertyChanged("EnableBingMapsIntegration");
			}
		}

		[AttributeLogicalName("enablepricingoncreate")]
		[ExcludeFromCodeCoverage]
		public bool? EnablePricingOnCreate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("enablepricingoncreate");
			}
			set
			{
				OnPropertyChanging("EnablePricingOnCreate");
				((Entity)this).SetAttributeValue("enablepricingoncreate", (object)value);
				OnPropertyChanged("EnablePricingOnCreate");
			}
		}

		[AttributeLogicalName("enablesmartmatching")]
		[ExcludeFromCodeCoverage]
		public bool? EnableSmartMatching
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("enablesmartmatching");
			}
			set
			{
				OnPropertyChanging("EnableSmartMatching");
				((Entity)this).SetAttributeValue("enablesmartmatching", (object)value);
				OnPropertyChanged("EnableSmartMatching");
			}
		}

		[AttributeLogicalName("enforcereadonlyplugins")]
		[ExcludeFromCodeCoverage]
		public bool? EnforceReadOnlyPlugins
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("enforcereadonlyplugins");
			}
			set
			{
				OnPropertyChanging("EnforceReadOnlyPlugins");
				((Entity)this).SetAttributeValue("enforcereadonlyplugins", (object)value);
				OnPropertyChanged("EnforceReadOnlyPlugins");
			}
		}

		[AttributeLogicalName("entityimage")]
		[ExcludeFromCodeCoverage]
		public byte[] EntityImage
		{
			get
			{
				return ((Entity)this).GetAttributeValue<byte[]>("entityimage");
			}
			set
			{
				OnPropertyChanging("EntityImage");
				((Entity)this).SetAttributeValue("entityimage", (object)value);
				OnPropertyChanged("EntityImage");
			}
		}

		[AttributeLogicalName("entityimage_timestamp")]
		[ExcludeFromCodeCoverage]
		public long? EntityImage_Timestamp => ((Entity)this).GetAttributeValue<long?>("entityimage_timestamp");

		[AttributeLogicalName("entityimage_url")]
		[ExcludeFromCodeCoverage]
		public string EntityImage_URL => ((Entity)this).GetAttributeValue<string>("entityimage_url");

		[AttributeLogicalName("entityimageid")]
		[ExcludeFromCodeCoverage]
		public Guid? EntityImageId => ((Entity)this).GetAttributeValue<Guid?>("entityimageid");

		[AttributeLogicalName("expirechangetrackingindays")]
		[ExcludeFromCodeCoverage]
		public int? ExpireChangeTrackingInDays
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("expirechangetrackingindays");
			}
			set
			{
				OnPropertyChanging("ExpireChangeTrackingInDays");
				((Entity)this).SetAttributeValue("expirechangetrackingindays", (object)value);
				OnPropertyChanged("ExpireChangeTrackingInDays");
			}
		}

		[AttributeLogicalName("expiresubscriptionsindays")]
		[ExcludeFromCodeCoverage]
		public int? ExpireSubscriptionsInDays
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("expiresubscriptionsindays");
			}
			set
			{
				OnPropertyChanging("ExpireSubscriptionsInDays");
				((Entity)this).SetAttributeValue("expiresubscriptionsindays", (object)value);
				OnPropertyChanged("ExpireSubscriptionsInDays");
			}
		}

		[AttributeLogicalName("externalbaseurl")]
		[ExcludeFromCodeCoverage]
		public string ExternalBaseUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("externalbaseurl");
			}
			set
			{
				OnPropertyChanging("ExternalBaseUrl");
				((Entity)this).SetAttributeValue("externalbaseurl", (object)value);
				OnPropertyChanged("ExternalBaseUrl");
			}
		}

		[AttributeLogicalName("externalpartycorrelationkeys")]
		[ExcludeFromCodeCoverage]
		public string ExternalPartyCorrelationKeys
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("externalpartycorrelationkeys");
			}
			set
			{
				OnPropertyChanging("ExternalPartyCorrelationKeys");
				((Entity)this).SetAttributeValue("externalpartycorrelationkeys", (object)value);
				OnPropertyChanged("ExternalPartyCorrelationKeys");
			}
		}

		[AttributeLogicalName("externalpartyentitysettings")]
		[ExcludeFromCodeCoverage]
		public string ExternalPartyEntitySettings
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("externalpartyentitysettings");
			}
			set
			{
				OnPropertyChanging("ExternalPartyEntitySettings");
				((Entity)this).SetAttributeValue("externalpartyentitysettings", (object)value);
				OnPropertyChanged("ExternalPartyEntitySettings");
			}
		}

		[AttributeLogicalName("featureset")]
		[ExcludeFromCodeCoverage]
		public string FeatureSet
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("featureset");
			}
			set
			{
				OnPropertyChanging("FeatureSet");
				((Entity)this).SetAttributeValue("featureset", (object)value);
				OnPropertyChanged("FeatureSet");
			}
		}

		[AttributeLogicalName("fiscalcalendarstart")]
		[ExcludeFromCodeCoverage]
		public DateTime? FiscalCalendarStart
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("fiscalcalendarstart");
			}
			set
			{
				OnPropertyChanging("FiscalCalendarStart");
				((Entity)this).SetAttributeValue("fiscalcalendarstart", (object)value);
				OnPropertyChanged("FiscalCalendarStart");
			}
		}

		[AttributeLogicalName("fiscalperiodformat")]
		[ExcludeFromCodeCoverage]
		public string FiscalPeriodFormat
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("fiscalperiodformat");
			}
			set
			{
				OnPropertyChanging("FiscalPeriodFormat");
				((Entity)this).SetAttributeValue("fiscalperiodformat", (object)value);
				OnPropertyChanged("FiscalPeriodFormat");
			}
		}

		[AttributeLogicalName("fiscalperiodformatperiod")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue FiscalPeriodFormatPeriod
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("fiscalperiodformatperiod");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("FiscalPeriodFormatPeriod");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("fiscalperiodformatperiod", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("fiscalperiodformatperiod", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("FiscalPeriodFormatPeriod");
			}
		}

		[AttributeLogicalName("fiscalperiodtype")]
		[ExcludeFromCodeCoverage]
		public int? FiscalPeriodType
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("fiscalperiodtype");
			}
			set
			{
				OnPropertyChanging("FiscalPeriodType");
				((Entity)this).SetAttributeValue("fiscalperiodtype", (object)value);
				OnPropertyChanged("FiscalPeriodType");
			}
		}

		[AttributeLogicalName("fiscalsettingsupdated")]
		[Obsolete]
		[ExcludeFromCodeCoverage]
		public bool? FiscalSettingsUpdated => ((Entity)this).GetAttributeValue<bool?>("fiscalsettingsupdated");

		[AttributeLogicalName("fiscalyeardisplaycode")]
		[ExcludeFromCodeCoverage]
		public int? FiscalYearDisplayCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("fiscalyeardisplaycode");
			}
			set
			{
				OnPropertyChanging("FiscalYearDisplayCode");
				((Entity)this).SetAttributeValue("fiscalyeardisplaycode", (object)value);
				OnPropertyChanged("FiscalYearDisplayCode");
			}
		}

		[AttributeLogicalName("fiscalyearformat")]
		[ExcludeFromCodeCoverage]
		public string FiscalYearFormat
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("fiscalyearformat");
			}
			set
			{
				OnPropertyChanging("FiscalYearFormat");
				((Entity)this).SetAttributeValue("fiscalyearformat", (object)value);
				OnPropertyChanged("FiscalYearFormat");
			}
		}

		[AttributeLogicalName("fiscalyearformatprefix")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue FiscalYearFormatPrefix
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("fiscalyearformatprefix");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("FiscalYearFormatPrefix");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("fiscalyearformatprefix", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("fiscalyearformatprefix", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("FiscalYearFormatPrefix");
			}
		}

		[AttributeLogicalName("fiscalyearformatsuffix")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue FiscalYearFormatSuffix
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("fiscalyearformatsuffix");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("FiscalYearFormatSuffix");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("fiscalyearformatsuffix", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("fiscalyearformatsuffix", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("FiscalYearFormatSuffix");
			}
		}

		[AttributeLogicalName("fiscalyearformatyear")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue FiscalYearFormatYear
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("fiscalyearformatyear");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("FiscalYearFormatYear");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("fiscalyearformatyear", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("fiscalyearformatyear", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("FiscalYearFormatYear");
			}
		}

		[AttributeLogicalName("fiscalyearperiodconnect")]
		[ExcludeFromCodeCoverage]
		public string FiscalYearPeriodConnect
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("fiscalyearperiodconnect");
			}
			set
			{
				OnPropertyChanging("FiscalYearPeriodConnect");
				((Entity)this).SetAttributeValue("fiscalyearperiodconnect", (object)value);
				OnPropertyChanged("FiscalYearPeriodConnect");
			}
		}

		[AttributeLogicalName("fullnameconventioncode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue FullNameConventionCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("fullnameconventioncode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("FullNameConventionCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("fullnameconventioncode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("fullnameconventioncode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("FullNameConventionCode");
			}
		}

		[AttributeLogicalName("futureexpansionwindow")]
		[ExcludeFromCodeCoverage]
		public int? FutureExpansionWindow
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("futureexpansionwindow");
			}
			set
			{
				OnPropertyChanging("FutureExpansionWindow");
				((Entity)this).SetAttributeValue("futureexpansionwindow", (object)value);
				OnPropertyChanged("FutureExpansionWindow");
			}
		}

		[AttributeLogicalName("generatealertsforerrors")]
		[ExcludeFromCodeCoverage]
		public bool? GenerateAlertsForErrors
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("generatealertsforerrors");
			}
			set
			{
				OnPropertyChanging("GenerateAlertsForErrors");
				((Entity)this).SetAttributeValue("generatealertsforerrors", (object)value);
				OnPropertyChanged("GenerateAlertsForErrors");
			}
		}

		[AttributeLogicalName("generatealertsforinformation")]
		[ExcludeFromCodeCoverage]
		public bool? GenerateAlertsForInformation
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("generatealertsforinformation");
			}
			set
			{
				OnPropertyChanging("GenerateAlertsForInformation");
				((Entity)this).SetAttributeValue("generatealertsforinformation", (object)value);
				OnPropertyChanged("GenerateAlertsForInformation");
			}
		}

		[AttributeLogicalName("generatealertsforwarnings")]
		[ExcludeFromCodeCoverage]
		public bool? GenerateAlertsForWarnings
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("generatealertsforwarnings");
			}
			set
			{
				OnPropertyChanging("GenerateAlertsForWarnings");
				((Entity)this).SetAttributeValue("generatealertsforwarnings", (object)value);
				OnPropertyChanged("GenerateAlertsForWarnings");
			}
		}

		[AttributeLogicalName("getstartedpanecontentenabled")]
		[ExcludeFromCodeCoverage]
		public bool? GetStartedPaneContentEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("getstartedpanecontentenabled");
			}
			set
			{
				OnPropertyChanging("GetStartedPaneContentEnabled");
				((Entity)this).SetAttributeValue("getstartedpanecontentenabled", (object)value);
				OnPropertyChanged("GetStartedPaneContentEnabled");
			}
		}

		[AttributeLogicalName("globalappendurlparametersenabled")]
		[ExcludeFromCodeCoverage]
		public bool? GlobalAppendUrlParametersEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("globalappendurlparametersenabled");
			}
			set
			{
				OnPropertyChanging("GlobalAppendUrlParametersEnabled");
				((Entity)this).SetAttributeValue("globalappendurlparametersenabled", (object)value);
				OnPropertyChanged("GlobalAppendUrlParametersEnabled");
			}
		}

		[AttributeLogicalName("globalhelpurl")]
		[ExcludeFromCodeCoverage]
		public string GlobalHelpUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("globalhelpurl");
			}
			set
			{
				OnPropertyChanging("GlobalHelpUrl");
				((Entity)this).SetAttributeValue("globalhelpurl", (object)value);
				OnPropertyChanged("GlobalHelpUrl");
			}
		}

		[AttributeLogicalName("globalhelpurlenabled")]
		[ExcludeFromCodeCoverage]
		public bool? GlobalHelpUrlEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("globalhelpurlenabled");
			}
			set
			{
				OnPropertyChanging("GlobalHelpUrlEnabled");
				((Entity)this).SetAttributeValue("globalhelpurlenabled", (object)value);
				OnPropertyChanged("GlobalHelpUrlEnabled");
			}
		}

		[AttributeLogicalName("goalrollupexpirytime")]
		[ExcludeFromCodeCoverage]
		public int? GoalRollupExpiryTime
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("goalrollupexpirytime");
			}
			set
			{
				OnPropertyChanging("GoalRollupExpiryTime");
				((Entity)this).SetAttributeValue("goalrollupexpirytime", (object)value);
				OnPropertyChanged("GoalRollupExpiryTime");
			}
		}

		[AttributeLogicalName("goalrollupfrequency")]
		[ExcludeFromCodeCoverage]
		public int? GoalRollupFrequency
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("goalrollupfrequency");
			}
			set
			{
				OnPropertyChanging("GoalRollupFrequency");
				((Entity)this).SetAttributeValue("goalrollupfrequency", (object)value);
				OnPropertyChanged("GoalRollupFrequency");
			}
		}

		[AttributeLogicalName("grantaccesstonetworkservice")]
		[ExcludeFromCodeCoverage]
		public bool? GrantAccessToNetworkService
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("grantaccesstonetworkservice");
			}
			set
			{
				OnPropertyChanging("GrantAccessToNetworkService");
				((Entity)this).SetAttributeValue("grantaccesstonetworkservice", (object)value);
				OnPropertyChanged("GrantAccessToNetworkService");
			}
		}

		[AttributeLogicalName("hashdeltasubjectcount")]
		[ExcludeFromCodeCoverage]
		public int? HashDeltaSubjectCount
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("hashdeltasubjectcount");
			}
			set
			{
				OnPropertyChanging("HashDeltaSubjectCount");
				((Entity)this).SetAttributeValue("hashdeltasubjectcount", (object)value);
				OnPropertyChanged("HashDeltaSubjectCount");
			}
		}

		[AttributeLogicalName("hashfilterkeywords")]
		[ExcludeFromCodeCoverage]
		public string HashFilterKeywords
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("hashfilterkeywords");
			}
			set
			{
				OnPropertyChanging("HashFilterKeywords");
				((Entity)this).SetAttributeValue("hashfilterkeywords", (object)value);
				OnPropertyChanged("HashFilterKeywords");
			}
		}

		[AttributeLogicalName("hashmaxcount")]
		[ExcludeFromCodeCoverage]
		public int? HashMaxCount
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("hashmaxcount");
			}
			set
			{
				OnPropertyChanging("HashMaxCount");
				((Entity)this).SetAttributeValue("hashmaxcount", (object)value);
				OnPropertyChanged("HashMaxCount");
			}
		}

		[AttributeLogicalName("hashminaddresscount")]
		[ExcludeFromCodeCoverage]
		public int? HashMinAddressCount
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("hashminaddresscount");
			}
			set
			{
				OnPropertyChanging("HashMinAddressCount");
				((Entity)this).SetAttributeValue("hashminaddresscount", (object)value);
				OnPropertyChanged("HashMinAddressCount");
			}
		}

		[AttributeLogicalName("highcontrastthemedata")]
		[ExcludeFromCodeCoverage]
		public string HighContrastThemeData
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("highcontrastthemedata");
			}
			set
			{
				OnPropertyChanging("HighContrastThemeData");
				((Entity)this).SetAttributeValue("highcontrastthemedata", (object)value);
				OnPropertyChanged("HighContrastThemeData");
			}
		}

		[AttributeLogicalName("ignoreinternalemail")]
		[ExcludeFromCodeCoverage]
		public bool? IgnoreInternalEmail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ignoreinternalemail");
			}
			set
			{
				OnPropertyChanging("IgnoreInternalEmail");
				((Entity)this).SetAttributeValue("ignoreinternalemail", (object)value);
				OnPropertyChanged("IgnoreInternalEmail");
			}
		}

		[AttributeLogicalName("incomingemailexchangeemailretrievalbatchsize")]
		[ExcludeFromCodeCoverage]
		public int? IncomingEmailExchangeEmailRetrievalBatchSize
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("incomingemailexchangeemailretrievalbatchsize");
			}
			set
			{
				OnPropertyChanging("IncomingEmailExchangeEmailRetrievalBatchSize");
				((Entity)this).SetAttributeValue("incomingemailexchangeemailretrievalbatchsize", (object)value);
				OnPropertyChanged("IncomingEmailExchangeEmailRetrievalBatchSize");
			}
		}

		[AttributeLogicalName("initialversion")]
		[ExcludeFromCodeCoverage]
		public string InitialVersion
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("initialversion");
			}
			set
			{
				OnPropertyChanging("InitialVersion");
				((Entity)this).SetAttributeValue("initialversion", (object)value);
				OnPropertyChanged("InitialVersion");
			}
		}

		[AttributeLogicalName("integrationuserid")]
		[ExcludeFromCodeCoverage]
		public Guid? IntegrationUserId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("integrationuserid");
			}
			set
			{
				OnPropertyChanging("IntegrationUserId");
				((Entity)this).SetAttributeValue("integrationuserid", (object)value);
				OnPropertyChanged("IntegrationUserId");
			}
		}

		[AttributeLogicalName("invoiceprefix")]
		[ExcludeFromCodeCoverage]
		public string InvoicePrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("invoiceprefix");
			}
			set
			{
				OnPropertyChanging("InvoicePrefix");
				((Entity)this).SetAttributeValue("invoiceprefix", (object)value);
				OnPropertyChanged("InvoicePrefix");
			}
		}

		[AttributeLogicalName("isactioncardenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsActionCardEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isactioncardenabled");
			}
			set
			{
				OnPropertyChanging("IsActionCardEnabled");
				((Entity)this).SetAttributeValue("isactioncardenabled", (object)value);
				OnPropertyChanged("IsActionCardEnabled");
			}
		}

		[AttributeLogicalName("isactivityanalysisenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsActivityAnalysisEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isactivityanalysisenabled");
			}
			set
			{
				OnPropertyChanging("IsActivityAnalysisEnabled");
				((Entity)this).SetAttributeValue("isactivityanalysisenabled", (object)value);
				OnPropertyChanged("IsActivityAnalysisEnabled");
			}
		}

		[AttributeLogicalName("isappmode")]
		[ExcludeFromCodeCoverage]
		public bool? IsAppMode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isappmode");
			}
			set
			{
				OnPropertyChanging("IsAppMode");
				((Entity)this).SetAttributeValue("isappmode", (object)value);
				OnPropertyChanged("IsAppMode");
			}
		}

		[AttributeLogicalName("isappointmentattachmentsyncenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsAppointmentAttachmentSyncEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isappointmentattachmentsyncenabled");
			}
			set
			{
				OnPropertyChanging("IsAppointmentAttachmentSyncEnabled");
				((Entity)this).SetAttributeValue("isappointmentattachmentsyncenabled", (object)value);
				OnPropertyChanged("IsAppointmentAttachmentSyncEnabled");
			}
		}

		[AttributeLogicalName("isassignedtaskssyncenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsAssignedTasksSyncEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isassignedtaskssyncenabled");
			}
			set
			{
				OnPropertyChanging("IsAssignedTasksSyncEnabled");
				((Entity)this).SetAttributeValue("isassignedtaskssyncenabled", (object)value);
				OnPropertyChanged("IsAssignedTasksSyncEnabled");
			}
		}

		[AttributeLogicalName("isauditenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsAuditEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isauditenabled");
			}
			set
			{
				OnPropertyChanging("IsAuditEnabled");
				((Entity)this).SetAttributeValue("isauditenabled", (object)value);
				OnPropertyChanged("IsAuditEnabled");
			}
		}

		[AttributeLogicalName("isautodatacaptureenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsAutoDataCaptureEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isautodatacaptureenabled");
			}
			set
			{
				OnPropertyChanging("IsAutoDataCaptureEnabled");
				((Entity)this).SetAttributeValue("isautodatacaptureenabled", (object)value);
				OnPropertyChanged("IsAutoDataCaptureEnabled");
			}
		}

		[AttributeLogicalName("isautosaveenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsAutoSaveEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isautosaveenabled");
			}
			set
			{
				OnPropertyChanging("IsAutoSaveEnabled");
				((Entity)this).SetAttributeValue("isautosaveenabled", (object)value);
				OnPropertyChanged("IsAutoSaveEnabled");
			}
		}

		[AttributeLogicalName("isconflictdetectionenabledformobileclient")]
		[ExcludeFromCodeCoverage]
		public bool? IsConflictDetectionEnabledForMobileClient
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isconflictdetectionenabledformobileclient");
			}
			set
			{
				OnPropertyChanging("IsConflictDetectionEnabledForMobileClient");
				((Entity)this).SetAttributeValue("isconflictdetectionenabledformobileclient", (object)value);
				OnPropertyChanged("IsConflictDetectionEnabledForMobileClient");
			}
		}

		[AttributeLogicalName("iscontactmailingaddresssyncenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsContactMailingAddressSyncEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("iscontactmailingaddresssyncenabled");
			}
			set
			{
				OnPropertyChanging("IsContactMailingAddressSyncEnabled");
				((Entity)this).SetAttributeValue("iscontactmailingaddresssyncenabled", (object)value);
				OnPropertyChanged("IsContactMailingAddressSyncEnabled");
			}
		}

		[AttributeLogicalName("isdefaultcountrycodecheckenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsDefaultCountryCodeCheckEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isdefaultcountrycodecheckenabled");
			}
			set
			{
				OnPropertyChanging("IsDefaultCountryCodeCheckEnabled");
				((Entity)this).SetAttributeValue("isdefaultcountrycodecheckenabled", (object)value);
				OnPropertyChanged("IsDefaultCountryCodeCheckEnabled");
			}
		}

		[AttributeLogicalName("isdelegateaccessenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsDelegateAccessEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isdelegateaccessenabled");
			}
			set
			{
				OnPropertyChanging("IsDelegateAccessEnabled");
				((Entity)this).SetAttributeValue("isdelegateaccessenabled", (object)value);
				OnPropertyChanged("IsDelegateAccessEnabled");
			}
		}

		[AttributeLogicalName("isdelveactionhubintegrationenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsDelveActionHubIntegrationEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isdelveactionhubintegrationenabled");
			}
			set
			{
				OnPropertyChanging("IsDelveActionHubIntegrationEnabled");
				((Entity)this).SetAttributeValue("isdelveactionhubintegrationenabled", (object)value);
				OnPropertyChanged("IsDelveActionHubIntegrationEnabled");
			}
		}

		[AttributeLogicalName("isdisabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsDisabled => ((Entity)this).GetAttributeValue<bool?>("isdisabled");

		[AttributeLogicalName("isduplicatedetectionenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsDuplicateDetectionEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isduplicatedetectionenabled");
			}
			set
			{
				OnPropertyChanging("IsDuplicateDetectionEnabled");
				((Entity)this).SetAttributeValue("isduplicatedetectionenabled", (object)value);
				OnPropertyChanged("IsDuplicateDetectionEnabled");
			}
		}

		[AttributeLogicalName("isduplicatedetectionenabledforimport")]
		[ExcludeFromCodeCoverage]
		public bool? IsDuplicateDetectionEnabledForImport
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isduplicatedetectionenabledforimport");
			}
			set
			{
				OnPropertyChanging("IsDuplicateDetectionEnabledForImport");
				((Entity)this).SetAttributeValue("isduplicatedetectionenabledforimport", (object)value);
				OnPropertyChanged("IsDuplicateDetectionEnabledForImport");
			}
		}

		[AttributeLogicalName("isduplicatedetectionenabledforofflinesync")]
		[ExcludeFromCodeCoverage]
		public bool? IsDuplicateDetectionEnabledForOfflineSync
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isduplicatedetectionenabledforofflinesync");
			}
			set
			{
				OnPropertyChanging("IsDuplicateDetectionEnabledForOfflineSync");
				((Entity)this).SetAttributeValue("isduplicatedetectionenabledforofflinesync", (object)value);
				OnPropertyChanged("IsDuplicateDetectionEnabledForOfflineSync");
			}
		}

		[AttributeLogicalName("isduplicatedetectionenabledforonlinecreateupdate")]
		[ExcludeFromCodeCoverage]
		public bool? IsDuplicateDetectionEnabledForOnlineCreateUpdate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isduplicatedetectionenabledforonlinecreateupdate");
			}
			set
			{
				OnPropertyChanging("IsDuplicateDetectionEnabledForOnlineCreateUpdate");
				((Entity)this).SetAttributeValue("isduplicatedetectionenabledforonlinecreateupdate", (object)value);
				OnPropertyChanged("IsDuplicateDetectionEnabledForOnlineCreateUpdate");
			}
		}

		[AttributeLogicalName("isemailmonitoringallowed")]
		[ExcludeFromCodeCoverage]
		public bool? IsEmailMonitoringAllowed
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isemailmonitoringallowed");
			}
			set
			{
				OnPropertyChanging("IsEmailMonitoringAllowed");
				((Entity)this).SetAttributeValue("isemailmonitoringallowed", (object)value);
				OnPropertyChanged("IsEmailMonitoringAllowed");
			}
		}

		[AttributeLogicalName("isemailserverprofilecontentfilteringenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsEmailServerProfileContentFilteringEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isemailserverprofilecontentfilteringenabled");
			}
			set
			{
				OnPropertyChanging("IsEmailServerProfileContentFilteringEnabled");
				((Entity)this).SetAttributeValue("isemailserverprofilecontentfilteringenabled", (object)value);
				OnPropertyChanged("IsEmailServerProfileContentFilteringEnabled");
			}
		}

		[AttributeLogicalName("isexternalsearchindexenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsExternalSearchIndexEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isexternalsearchindexenabled");
			}
			set
			{
				OnPropertyChanging("IsExternalSearchIndexEnabled");
				((Entity)this).SetAttributeValue("isexternalsearchindexenabled", (object)value);
				OnPropertyChanged("IsExternalSearchIndexEnabled");
			}
		}

		[AttributeLogicalName("isfiscalperiodmonthbased")]
		[ExcludeFromCodeCoverage]
		public bool? IsFiscalPeriodMonthBased
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isfiscalperiodmonthbased");
			}
			set
			{
				OnPropertyChanging("IsFiscalPeriodMonthBased");
				((Entity)this).SetAttributeValue("isfiscalperiodmonthbased", (object)value);
				OnPropertyChanged("IsFiscalPeriodMonthBased");
			}
		}

		[AttributeLogicalName("isfolderautocreatedonsp")]
		[ExcludeFromCodeCoverage]
		public bool? IsFolderAutoCreatedonSP
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isfolderautocreatedonsp");
			}
			set
			{
				OnPropertyChanging("IsFolderAutoCreatedonSP");
				((Entity)this).SetAttributeValue("isfolderautocreatedonsp", (object)value);
				OnPropertyChanged("IsFolderAutoCreatedonSP");
			}
		}

		[AttributeLogicalName("isfolderbasedtrackingenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsFolderBasedTrackingEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isfolderbasedtrackingenabled");
			}
			set
			{
				OnPropertyChanging("IsFolderBasedTrackingEnabled");
				((Entity)this).SetAttributeValue("isfolderbasedtrackingenabled", (object)value);
				OnPropertyChanged("IsFolderBasedTrackingEnabled");
			}
		}

		[AttributeLogicalName("isfulltextsearchenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsFullTextSearchEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isfulltextsearchenabled");
			}
			set
			{
				OnPropertyChanging("IsFullTextSearchEnabled");
				((Entity)this).SetAttributeValue("isfulltextsearchenabled", (object)value);
				OnPropertyChanged("IsFullTextSearchEnabled");
			}
		}

		[AttributeLogicalName("ishierarchicalsecuritymodelenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsHierarchicalSecurityModelEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ishierarchicalsecuritymodelenabled");
			}
			set
			{
				OnPropertyChanging("IsHierarchicalSecurityModelEnabled");
				((Entity)this).SetAttributeValue("ishierarchicalsecuritymodelenabled", (object)value);
				OnPropertyChanged("IsHierarchicalSecurityModelEnabled");
			}
		}

		[AttributeLogicalName("ismailboxforcedunlockingenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsMailboxForcedUnlockingEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ismailboxforcedunlockingenabled");
			}
			set
			{
				OnPropertyChanging("IsMailboxForcedUnlockingEnabled");
				((Entity)this).SetAttributeValue("ismailboxforcedunlockingenabled", (object)value);
				OnPropertyChanged("IsMailboxForcedUnlockingEnabled");
			}
		}

		[AttributeLogicalName("ismailboxinactivebackoffenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsMailboxInactiveBackoffEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ismailboxinactivebackoffenabled");
			}
			set
			{
				OnPropertyChanging("IsMailboxInactiveBackoffEnabled");
				((Entity)this).SetAttributeValue("ismailboxinactivebackoffenabled", (object)value);
				OnPropertyChanged("IsMailboxInactiveBackoffEnabled");
			}
		}

		[AttributeLogicalName("ismobileofflineenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsMobileOfflineEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ismobileofflineenabled");
			}
			set
			{
				OnPropertyChanging("IsMobileOfflineEnabled");
				((Entity)this).SetAttributeValue("ismobileofflineenabled", (object)value);
				OnPropertyChanged("IsMobileOfflineEnabled");
			}
		}

		[AttributeLogicalName("isofficegraphenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsOfficeGraphEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isofficegraphenabled");
			}
			set
			{
				OnPropertyChanging("IsOfficeGraphEnabled");
				((Entity)this).SetAttributeValue("isofficegraphenabled", (object)value);
				OnPropertyChanged("IsOfficeGraphEnabled");
			}
		}

		[AttributeLogicalName("isonedriveenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsOneDriveEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isonedriveenabled");
			}
			set
			{
				OnPropertyChanging("IsOneDriveEnabled");
				((Entity)this).SetAttributeValue("isonedriveenabled", (object)value);
				OnPropertyChanged("IsOneDriveEnabled");
			}
		}

		[AttributeLogicalName("ispresenceenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsPresenceEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ispresenceenabled");
			}
			set
			{
				OnPropertyChanging("IsPresenceEnabled");
				((Entity)this).SetAttributeValue("ispresenceenabled", (object)value);
				OnPropertyChanged("IsPresenceEnabled");
			}
		}

		[AttributeLogicalName("ispreviewenabledforactioncard")]
		[ExcludeFromCodeCoverage]
		public bool? IsPreviewEnabledForActionCard
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ispreviewenabledforactioncard");
			}
			set
			{
				OnPropertyChanging("IsPreviewEnabledForActionCard");
				((Entity)this).SetAttributeValue("ispreviewenabledforactioncard", (object)value);
				OnPropertyChanged("IsPreviewEnabledForActionCard");
			}
		}

		[AttributeLogicalName("issopintegrationenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsSOPIntegrationEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("issopintegrationenabled");
			}
			set
			{
				OnPropertyChanging("IsSOPIntegrationEnabled");
				((Entity)this).SetAttributeValue("issopintegrationenabled", (object)value);
				OnPropertyChanged("IsSOPIntegrationEnabled");
			}
		}

		[AttributeLogicalName("isuseraccessauditenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsUserAccessAuditEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isuseraccessauditenabled");
			}
			set
			{
				OnPropertyChanging("IsUserAccessAuditEnabled");
				((Entity)this).SetAttributeValue("isuseraccessauditenabled", (object)value);
				OnPropertyChanged("IsUserAccessAuditEnabled");
			}
		}

		[AttributeLogicalName("isvintegrationcode")]
		[Obsolete]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ISVIntegrationCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("isvintegrationcode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("ISVIntegrationCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("isvintegrationcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("isvintegrationcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ISVIntegrationCode");
			}
		}

		[AttributeLogicalName("kaprefix")]
		[ExcludeFromCodeCoverage]
		public string KaPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("kaprefix");
			}
			set
			{
				OnPropertyChanging("KaPrefix");
				((Entity)this).SetAttributeValue("kaprefix", (object)value);
				OnPropertyChanged("KaPrefix");
			}
		}

		[AttributeLogicalName("kbprefix")]
		[ExcludeFromCodeCoverage]
		public string KbPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("kbprefix");
			}
			set
			{
				OnPropertyChanging("KbPrefix");
				((Entity)this).SetAttributeValue("kbprefix", (object)value);
				OnPropertyChanged("KbPrefix");
			}
		}

		[AttributeLogicalName("kmsettings")]
		[ExcludeFromCodeCoverage]
		public string KMSettings
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("kmsettings");
			}
			set
			{
				OnPropertyChanging("KMSettings");
				((Entity)this).SetAttributeValue("kmsettings", (object)value);
				OnPropertyChanged("KMSettings");
			}
		}

		[AttributeLogicalName("languagecode")]
		[ExcludeFromCodeCoverage]
		public int? LanguageCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("languagecode");
			}
			set
			{
				OnPropertyChanging("LanguageCode");
				((Entity)this).SetAttributeValue("languagecode", (object)value);
				OnPropertyChanged("LanguageCode");
			}
		}

		[AttributeLogicalName("localeid")]
		[ExcludeFromCodeCoverage]
		public int? LocaleId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("localeid");
			}
			set
			{
				OnPropertyChanging("LocaleId");
				((Entity)this).SetAttributeValue("localeid", (object)value);
				OnPropertyChanged("LocaleId");
			}
		}

		[AttributeLogicalName("longdateformatcode")]
		[ExcludeFromCodeCoverage]
		public int? LongDateFormatCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("longdateformatcode");
			}
			set
			{
				OnPropertyChanging("LongDateFormatCode");
				((Entity)this).SetAttributeValue("longdateformatcode", (object)value);
				OnPropertyChanged("LongDateFormatCode");
			}
		}

		[AttributeLogicalName("mailboxintermittentissueminrange")]
		[ExcludeFromCodeCoverage]
		public int? MailboxIntermittentIssueMinRange
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("mailboxintermittentissueminrange");
			}
			set
			{
				OnPropertyChanging("MailboxIntermittentIssueMinRange");
				((Entity)this).SetAttributeValue("mailboxintermittentissueminrange", (object)value);
				OnPropertyChanged("MailboxIntermittentIssueMinRange");
			}
		}

		[AttributeLogicalName("mailboxpermanentissueminrange")]
		[ExcludeFromCodeCoverage]
		public int? MailboxPermanentIssueMinRange
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("mailboxpermanentissueminrange");
			}
			set
			{
				OnPropertyChanging("MailboxPermanentIssueMinRange");
				((Entity)this).SetAttributeValue("mailboxpermanentissueminrange", (object)value);
				OnPropertyChanged("MailboxPermanentIssueMinRange");
			}
		}

		[AttributeLogicalName("maxappointmentdurationdays")]
		[ExcludeFromCodeCoverage]
		public int? MaxAppointmentDurationDays
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxappointmentdurationdays");
			}
			set
			{
				OnPropertyChanging("MaxAppointmentDurationDays");
				((Entity)this).SetAttributeValue("maxappointmentdurationdays", (object)value);
				OnPropertyChanged("MaxAppointmentDurationDays");
			}
		}

		[AttributeLogicalName("maxconditionsformobileofflinefilters")]
		[ExcludeFromCodeCoverage]
		public int? MaxConditionsForMobileOfflineFilters
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxconditionsformobileofflinefilters");
			}
			set
			{
				OnPropertyChanging("MaxConditionsForMobileOfflineFilters");
				((Entity)this).SetAttributeValue("maxconditionsformobileofflinefilters", (object)value);
				OnPropertyChanged("MaxConditionsForMobileOfflineFilters");
			}
		}

		[AttributeLogicalName("maxdepthforhierarchicalsecuritymodel")]
		[ExcludeFromCodeCoverage]
		public int? MaxDepthForHierarchicalSecurityModel
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxdepthforhierarchicalsecuritymodel");
			}
			set
			{
				OnPropertyChanging("MaxDepthForHierarchicalSecurityModel");
				((Entity)this).SetAttributeValue("maxdepthforhierarchicalsecuritymodel", (object)value);
				OnPropertyChanged("MaxDepthForHierarchicalSecurityModel");
			}
		}

		[AttributeLogicalName("maxfolderbasedtrackingmappings")]
		[ExcludeFromCodeCoverage]
		public int? MaxFolderBasedTrackingMappings
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxfolderbasedtrackingmappings");
			}
			set
			{
				OnPropertyChanging("MaxFolderBasedTrackingMappings");
				((Entity)this).SetAttributeValue("maxfolderbasedtrackingmappings", (object)value);
				OnPropertyChanged("MaxFolderBasedTrackingMappings");
			}
		}

		[AttributeLogicalName("maximumactivebusinessprocessflowsallowedperentity")]
		[ExcludeFromCodeCoverage]
		public int? MaximumActiveBusinessProcessFlowsAllowedPerEntity
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maximumactivebusinessprocessflowsallowedperentity");
			}
			set
			{
				OnPropertyChanging("MaximumActiveBusinessProcessFlowsAllowedPerEntity");
				((Entity)this).SetAttributeValue("maximumactivebusinessprocessflowsallowedperentity", (object)value);
				OnPropertyChanged("MaximumActiveBusinessProcessFlowsAllowedPerEntity");
			}
		}

		[AttributeLogicalName("maximumdynamicpropertiesallowed")]
		[ExcludeFromCodeCoverage]
		public int? MaximumDynamicPropertiesAllowed
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maximumdynamicpropertiesallowed");
			}
			set
			{
				OnPropertyChanging("MaximumDynamicPropertiesAllowed");
				((Entity)this).SetAttributeValue("maximumdynamicpropertiesallowed", (object)value);
				OnPropertyChanged("MaximumDynamicPropertiesAllowed");
			}
		}

		[AttributeLogicalName("maximumentitieswithactivesla")]
		[ExcludeFromCodeCoverage]
		public int? MaximumEntitiesWithActiveSLA
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maximumentitieswithactivesla");
			}
			set
			{
				OnPropertyChanging("MaximumEntitiesWithActiveSLA");
				((Entity)this).SetAttributeValue("maximumentitieswithactivesla", (object)value);
				OnPropertyChanged("MaximumEntitiesWithActiveSLA");
			}
		}

		[AttributeLogicalName("maximumslakpiperentitywithactivesla")]
		[ExcludeFromCodeCoverage]
		public int? MaximumSLAKPIPerEntityWithActiveSLA
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maximumslakpiperentitywithactivesla");
			}
			set
			{
				OnPropertyChanging("MaximumSLAKPIPerEntityWithActiveSLA");
				((Entity)this).SetAttributeValue("maximumslakpiperentitywithactivesla", (object)value);
				OnPropertyChanged("MaximumSLAKPIPerEntityWithActiveSLA");
			}
		}

		[AttributeLogicalName("maximumtrackingnumber")]
		[ExcludeFromCodeCoverage]
		public int? MaximumTrackingNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maximumtrackingnumber");
			}
			set
			{
				OnPropertyChanging("MaximumTrackingNumber");
				((Entity)this).SetAttributeValue("maximumtrackingnumber", (object)value);
				OnPropertyChanged("MaximumTrackingNumber");
			}
		}

		[AttributeLogicalName("maxproductsinbundle")]
		[ExcludeFromCodeCoverage]
		public int? MaxProductsInBundle
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxproductsinbundle");
			}
			set
			{
				OnPropertyChanging("MaxProductsInBundle");
				((Entity)this).SetAttributeValue("maxproductsinbundle", (object)value);
				OnPropertyChanged("MaxProductsInBundle");
			}
		}

		[AttributeLogicalName("maxrecordsforexporttoexcel")]
		[ExcludeFromCodeCoverage]
		public int? MaxRecordsForExportToExcel
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxrecordsforexporttoexcel");
			}
			set
			{
				OnPropertyChanging("MaxRecordsForExportToExcel");
				((Entity)this).SetAttributeValue("maxrecordsforexporttoexcel", (object)value);
				OnPropertyChanged("MaxRecordsForExportToExcel");
			}
		}

		[AttributeLogicalName("maxrecordsforlookupfilters")]
		[ExcludeFromCodeCoverage]
		public int? MaxRecordsForLookupFilters
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxrecordsforlookupfilters");
			}
			set
			{
				OnPropertyChanging("MaxRecordsForLookupFilters");
				((Entity)this).SetAttributeValue("maxrecordsforlookupfilters", (object)value);
				OnPropertyChanged("MaxRecordsForLookupFilters");
			}
		}

		[AttributeLogicalName("maxsupportedinternetexplorerversion")]
		[ExcludeFromCodeCoverage]
		public int? MaxSupportedInternetExplorerVersion => ((Entity)this).GetAttributeValue<int?>("maxsupportedinternetexplorerversion");

		[AttributeLogicalName("maxuploadfilesize")]
		[ExcludeFromCodeCoverage]
		public int? MaxUploadFileSize
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxuploadfilesize");
			}
			set
			{
				OnPropertyChanging("MaxUploadFileSize");
				((Entity)this).SetAttributeValue("maxuploadfilesize", (object)value);
				OnPropertyChanged("MaxUploadFileSize");
			}
		}

		[AttributeLogicalName("maxverboseloggingmailbox")]
		[ExcludeFromCodeCoverage]
		public int? MaxVerboseLoggingMailbox => ((Entity)this).GetAttributeValue<int?>("maxverboseloggingmailbox");

		[AttributeLogicalName("maxverboseloggingsynccycles")]
		[ExcludeFromCodeCoverage]
		public int? MaxVerboseLoggingSyncCycles => ((Entity)this).GetAttributeValue<int?>("maxverboseloggingsynccycles");

		[AttributeLogicalName("minaddressbooksyncinterval")]
		[ExcludeFromCodeCoverage]
		public int? MinAddressBookSyncInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("minaddressbooksyncinterval");
			}
			set
			{
				OnPropertyChanging("MinAddressBookSyncInterval");
				((Entity)this).SetAttributeValue("minaddressbooksyncinterval", (object)value);
				OnPropertyChanged("MinAddressBookSyncInterval");
			}
		}

		[AttributeLogicalName("minofflinesyncinterval")]
		[ExcludeFromCodeCoverage]
		public int? MinOfflineSyncInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("minofflinesyncinterval");
			}
			set
			{
				OnPropertyChanging("MinOfflineSyncInterval");
				((Entity)this).SetAttributeValue("minofflinesyncinterval", (object)value);
				OnPropertyChanged("MinOfflineSyncInterval");
			}
		}

		[AttributeLogicalName("minoutlooksyncinterval")]
		[ExcludeFromCodeCoverage]
		public int? MinOutlookSyncInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("minoutlooksyncinterval");
			}
			set
			{
				OnPropertyChanging("MinOutlookSyncInterval");
				((Entity)this).SetAttributeValue("minoutlooksyncinterval", (object)value);
				OnPropertyChanged("MinOutlookSyncInterval");
			}
		}

		[AttributeLogicalName("mobileofflineminlicenseprod")]
		[ExcludeFromCodeCoverage]
		public int? MobileOfflineMinLicenseProd => ((Entity)this).GetAttributeValue<int?>("mobileofflineminlicenseprod");

		[AttributeLogicalName("mobileofflineminlicensetrial")]
		[ExcludeFromCodeCoverage]
		public int? MobileOfflineMinLicenseTrial => ((Entity)this).GetAttributeValue<int?>("mobileofflineminlicensetrial");

		[AttributeLogicalName("mobileofflinesyncinterval")]
		[ExcludeFromCodeCoverage]
		public int? MobileOfflineSyncInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("mobileofflinesyncinterval");
			}
			set
			{
				OnPropertyChanging("MobileOfflineSyncInterval");
				((Entity)this).SetAttributeValue("mobileofflinesyncinterval", (object)value);
				OnPropertyChanged("MobileOfflineSyncInterval");
			}
		}

		[AttributeLogicalName("modifiedby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedby");

		[AttributeLogicalName("modifiedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? ModifiedOn => ((Entity)this).GetAttributeValue<DateTime?>("modifiedon");

		[AttributeLogicalName("modifiedonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedonbehalfby");

		[AttributeLogicalName("name")]
		[ExcludeFromCodeCoverage]
		public string Name
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("name");
			}
			set
			{
				OnPropertyChanging("Name");
				((Entity)this).SetAttributeValue("name", (object)value);
				OnPropertyChanged("Name");
			}
		}

		[AttributeLogicalName("negativecurrencyformatcode")]
		[ExcludeFromCodeCoverage]
		public int? NegativeCurrencyFormatCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("negativecurrencyformatcode");
			}
			set
			{
				OnPropertyChanging("NegativeCurrencyFormatCode");
				((Entity)this).SetAttributeValue("negativecurrencyformatcode", (object)value);
				OnPropertyChanged("NegativeCurrencyFormatCode");
			}
		}

		[AttributeLogicalName("negativeformatcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue NegativeFormatCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("negativeformatcode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("NegativeFormatCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("negativeformatcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("negativeformatcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("NegativeFormatCode");
			}
		}

		[AttributeLogicalName("nexttrackingnumber")]
		[ExcludeFromCodeCoverage]
		public int? NextTrackingNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("nexttrackingnumber");
			}
			set
			{
				OnPropertyChanging("NextTrackingNumber");
				((Entity)this).SetAttributeValue("nexttrackingnumber", (object)value);
				OnPropertyChanged("NextTrackingNumber");
			}
		}

		[AttributeLogicalName("notifymailboxownerofemailserverlevelalerts")]
		[ExcludeFromCodeCoverage]
		public bool? NotifyMailboxOwnerOfEmailServerLevelAlerts
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("notifymailboxownerofemailserverlevelalerts");
			}
			set
			{
				OnPropertyChanging("NotifyMailboxOwnerOfEmailServerLevelAlerts");
				((Entity)this).SetAttributeValue("notifymailboxownerofemailserverlevelalerts", (object)value);
				OnPropertyChanged("NotifyMailboxOwnerOfEmailServerLevelAlerts");
			}
		}

		[AttributeLogicalName("numberformat")]
		[ExcludeFromCodeCoverage]
		public string NumberFormat
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("numberformat");
			}
			set
			{
				OnPropertyChanging("NumberFormat");
				((Entity)this).SetAttributeValue("numberformat", (object)value);
				OnPropertyChanged("NumberFormat");
			}
		}

		[AttributeLogicalName("numbergroupformat")]
		[ExcludeFromCodeCoverage]
		public string NumberGroupFormat
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("numbergroupformat");
			}
			set
			{
				OnPropertyChanging("NumberGroupFormat");
				((Entity)this).SetAttributeValue("numbergroupformat", (object)value);
				OnPropertyChanged("NumberGroupFormat");
			}
		}

		[AttributeLogicalName("numberseparator")]
		[ExcludeFromCodeCoverage]
		public string NumberSeparator
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("numberseparator");
			}
			set
			{
				OnPropertyChanging("NumberSeparator");
				((Entity)this).SetAttributeValue("numberseparator", (object)value);
				OnPropertyChanged("NumberSeparator");
			}
		}

		[AttributeLogicalName("officeappsautodeploymentenabled")]
		[ExcludeFromCodeCoverage]
		public bool? OfficeAppsAutoDeploymentEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("officeappsautodeploymentenabled");
			}
			set
			{
				OnPropertyChanging("OfficeAppsAutoDeploymentEnabled");
				((Entity)this).SetAttributeValue("officeappsautodeploymentenabled", (object)value);
				OnPropertyChanged("OfficeAppsAutoDeploymentEnabled");
			}
		}

		[AttributeLogicalName("officegraphdelveurl")]
		[ExcludeFromCodeCoverage]
		public string OfficeGraphDelveUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("officegraphdelveurl");
			}
			set
			{
				OnPropertyChanging("OfficeGraphDelveUrl");
				((Entity)this).SetAttributeValue("officegraphdelveurl", (object)value);
				OnPropertyChanged("OfficeGraphDelveUrl");
			}
		}

		[AttributeLogicalName("oobpricecalculationenabled")]
		[ExcludeFromCodeCoverage]
		public bool? OOBPriceCalculationEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("oobpricecalculationenabled");
			}
			set
			{
				OnPropertyChanging("OOBPriceCalculationEnabled");
				((Entity)this).SetAttributeValue("oobpricecalculationenabled", (object)value);
				OnPropertyChanged("OOBPriceCalculationEnabled");
			}
		}

		[AttributeLogicalName("orderprefix")]
		[ExcludeFromCodeCoverage]
		public string OrderPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("orderprefix");
			}
			set
			{
				OnPropertyChanging("OrderPrefix");
				((Entity)this).SetAttributeValue("orderprefix", (object)value);
				OnPropertyChanged("OrderPrefix");
			}
		}

		[AttributeLogicalName("organizationid")]
		[ExcludeFromCodeCoverage]
		public Guid? OrganizationId => ((Entity)this).GetAttributeValue<Guid?>("organizationid");

		[AttributeLogicalName("organizationid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				((Entity)this).set_Id(value);
			}
		}

		[AttributeLogicalName("orgdborgsettings")]
		[ExcludeFromCodeCoverage]
		public string OrgDbOrgSettings
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("orgdborgsettings");
			}
			set
			{
				OnPropertyChanging("OrgDbOrgSettings");
				((Entity)this).SetAttributeValue("orgdborgsettings", (object)value);
				OnPropertyChanged("OrgDbOrgSettings");
			}
		}

		[AttributeLogicalName("orginsightsenabled")]
		[ExcludeFromCodeCoverage]
		public bool? OrgInsightsEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("orginsightsenabled");
			}
			set
			{
				OnPropertyChanging("OrgInsightsEnabled");
				((Entity)this).SetAttributeValue("orginsightsenabled", (object)value);
				OnPropertyChanged("OrgInsightsEnabled");
			}
		}

		[AttributeLogicalName("parsedtablecolumnprefix")]
		[ExcludeFromCodeCoverage]
		public string ParsedTableColumnPrefix => ((Entity)this).GetAttributeValue<string>("parsedtablecolumnprefix");

		[AttributeLogicalName("parsedtableprefix")]
		[ExcludeFromCodeCoverage]
		public string ParsedTablePrefix => ((Entity)this).GetAttributeValue<string>("parsedtableprefix");

		[AttributeLogicalName("pastexpansionwindow")]
		[ExcludeFromCodeCoverage]
		public int? PastExpansionWindow
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("pastexpansionwindow");
			}
			set
			{
				OnPropertyChanging("PastExpansionWindow");
				((Entity)this).SetAttributeValue("pastexpansionwindow", (object)value);
				OnPropertyChanged("PastExpansionWindow");
			}
		}

		[AttributeLogicalName("picture")]
		[ExcludeFromCodeCoverage]
		public string Picture
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("picture");
			}
			set
			{
				OnPropertyChanging("Picture");
				((Entity)this).SetAttributeValue("picture", (object)value);
				OnPropertyChanged("Picture");
			}
		}

		[AttributeLogicalName("pinpointlanguagecode")]
		[ExcludeFromCodeCoverage]
		public int? PinpointLanguageCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("pinpointlanguagecode");
			}
			set
			{
				OnPropertyChanging("PinpointLanguageCode");
				((Entity)this).SetAttributeValue("pinpointlanguagecode", (object)value);
				OnPropertyChanged("PinpointLanguageCode");
			}
		}

		[AttributeLogicalName("plugintracelogsetting")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PluginTraceLogSetting
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("plugintracelogsetting");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("PluginTraceLogSetting");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("plugintracelogsetting", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("plugintracelogsetting", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PluginTraceLogSetting");
			}
		}

		[AttributeLogicalName("pmdesignator")]
		[ExcludeFromCodeCoverage]
		public string PMDesignator
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("pmdesignator");
			}
			set
			{
				OnPropertyChanging("PMDesignator");
				((Entity)this).SetAttributeValue("pmdesignator", (object)value);
				OnPropertyChanged("PMDesignator");
			}
		}

		[AttributeLogicalName("powerbifeatureenabled")]
		[ExcludeFromCodeCoverage]
		public bool? PowerBiFeatureEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("powerbifeatureenabled");
			}
			set
			{
				OnPropertyChanging("PowerBiFeatureEnabled");
				((Entity)this).SetAttributeValue("powerbifeatureenabled", (object)value);
				OnPropertyChanged("PowerBiFeatureEnabled");
			}
		}

		[AttributeLogicalName("pricingdecimalprecision")]
		[ExcludeFromCodeCoverage]
		public int? PricingDecimalPrecision
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("pricingdecimalprecision");
			}
			set
			{
				OnPropertyChanging("PricingDecimalPrecision");
				((Entity)this).SetAttributeValue("pricingdecimalprecision", (object)value);
				OnPropertyChanged("PricingDecimalPrecision");
			}
		}

		[AttributeLogicalName("privacystatementurl")]
		[ExcludeFromCodeCoverage]
		public string PrivacyStatementUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("privacystatementurl");
			}
			set
			{
				OnPropertyChanging("PrivacyStatementUrl");
				((Entity)this).SetAttributeValue("privacystatementurl", (object)value);
				OnPropertyChanged("PrivacyStatementUrl");
			}
		}

		[AttributeLogicalName("privilegeusergroupid")]
		[ExcludeFromCodeCoverage]
		public Guid? PrivilegeUserGroupId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("privilegeusergroupid");
			}
			set
			{
				OnPropertyChanging("PrivilegeUserGroupId");
				((Entity)this).SetAttributeValue("privilegeusergroupid", (object)value);
				OnPropertyChanged("PrivilegeUserGroupId");
			}
		}

		[AttributeLogicalName("privreportinggroupid")]
		[ExcludeFromCodeCoverage]
		public Guid? PrivReportingGroupId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("privreportinggroupid");
			}
			set
			{
				OnPropertyChanging("PrivReportingGroupId");
				((Entity)this).SetAttributeValue("privreportinggroupid", (object)value);
				OnPropertyChanged("PrivReportingGroupId");
			}
		}

		[AttributeLogicalName("privreportinggroupname")]
		[ExcludeFromCodeCoverage]
		public string PrivReportingGroupName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("privreportinggroupname");
			}
			set
			{
				OnPropertyChanging("PrivReportingGroupName");
				((Entity)this).SetAttributeValue("privreportinggroupname", (object)value);
				OnPropertyChanged("PrivReportingGroupName");
			}
		}

		[AttributeLogicalName("productrecommendationsenabled")]
		[ExcludeFromCodeCoverage]
		public bool? ProductRecommendationsEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("productrecommendationsenabled");
			}
			set
			{
				OnPropertyChanging("ProductRecommendationsEnabled");
				((Entity)this).SetAttributeValue("productrecommendationsenabled", (object)value);
				OnPropertyChanged("ProductRecommendationsEnabled");
			}
		}

		[AttributeLogicalName("quickfindrecordlimitenabled")]
		[ExcludeFromCodeCoverage]
		public bool? QuickFindRecordLimitEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("quickfindrecordlimitenabled");
			}
			set
			{
				OnPropertyChanging("QuickFindRecordLimitEnabled");
				((Entity)this).SetAttributeValue("quickfindrecordlimitenabled", (object)value);
				OnPropertyChanged("QuickFindRecordLimitEnabled");
			}
		}

		[AttributeLogicalName("quoteprefix")]
		[ExcludeFromCodeCoverage]
		public string QuotePrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("quoteprefix");
			}
			set
			{
				OnPropertyChanging("QuotePrefix");
				((Entity)this).SetAttributeValue("quoteprefix", (object)value);
				OnPropertyChanged("QuotePrefix");
			}
		}

		[AttributeLogicalName("recurrencedefaultnumberofoccurrences")]
		[ExcludeFromCodeCoverage]
		public int? RecurrenceDefaultNumberOfOccurrences
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("recurrencedefaultnumberofoccurrences");
			}
			set
			{
				OnPropertyChanging("RecurrenceDefaultNumberOfOccurrences");
				((Entity)this).SetAttributeValue("recurrencedefaultnumberofoccurrences", (object)value);
				OnPropertyChanged("RecurrenceDefaultNumberOfOccurrences");
			}
		}

		[AttributeLogicalName("recurrenceexpansionjobbatchinterval")]
		[ExcludeFromCodeCoverage]
		public int? RecurrenceExpansionJobBatchInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("recurrenceexpansionjobbatchinterval");
			}
			set
			{
				OnPropertyChanging("RecurrenceExpansionJobBatchInterval");
				((Entity)this).SetAttributeValue("recurrenceexpansionjobbatchinterval", (object)value);
				OnPropertyChanged("RecurrenceExpansionJobBatchInterval");
			}
		}

		[AttributeLogicalName("recurrenceexpansionjobbatchsize")]
		[ExcludeFromCodeCoverage]
		public int? RecurrenceExpansionJobBatchSize
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("recurrenceexpansionjobbatchsize");
			}
			set
			{
				OnPropertyChanging("RecurrenceExpansionJobBatchSize");
				((Entity)this).SetAttributeValue("recurrenceexpansionjobbatchsize", (object)value);
				OnPropertyChanged("RecurrenceExpansionJobBatchSize");
			}
		}

		[AttributeLogicalName("recurrenceexpansionsynchcreatemax")]
		[ExcludeFromCodeCoverage]
		public int? RecurrenceExpansionSynchCreateMax
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("recurrenceexpansionsynchcreatemax");
			}
			set
			{
				OnPropertyChanging("RecurrenceExpansionSynchCreateMax");
				((Entity)this).SetAttributeValue("recurrenceexpansionsynchcreatemax", (object)value);
				OnPropertyChanged("RecurrenceExpansionSynchCreateMax");
			}
		}

		[AttributeLogicalName("referencesitemapxml")]
		[Obsolete]
		[ExcludeFromCodeCoverage]
		public string ReferenceSiteMapXml
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("referencesitemapxml");
			}
			set
			{
				OnPropertyChanging("ReferenceSiteMapXml");
				((Entity)this).SetAttributeValue("referencesitemapxml", (object)value);
				OnPropertyChanged("ReferenceSiteMapXml");
			}
		}

		[AttributeLogicalName("rendersecureiframeforemail")]
		[ExcludeFromCodeCoverage]
		public bool? RenderSecureIFrameForEmail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("rendersecureiframeforemail");
			}
			set
			{
				OnPropertyChanging("RenderSecureIFrameForEmail");
				((Entity)this).SetAttributeValue("rendersecureiframeforemail", (object)value);
				OnPropertyChanged("RenderSecureIFrameForEmail");
			}
		}

		[AttributeLogicalName("reportinggroupid")]
		[ExcludeFromCodeCoverage]
		public Guid? ReportingGroupId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("reportinggroupid");
			}
			set
			{
				OnPropertyChanging("ReportingGroupId");
				((Entity)this).SetAttributeValue("reportinggroupid", (object)value);
				OnPropertyChanged("ReportingGroupId");
			}
		}

		[AttributeLogicalName("reportinggroupname")]
		[ExcludeFromCodeCoverage]
		public string ReportingGroupName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("reportinggroupname");
			}
			set
			{
				OnPropertyChanging("ReportingGroupName");
				((Entity)this).SetAttributeValue("reportinggroupname", (object)value);
				OnPropertyChanged("ReportingGroupName");
			}
		}

		[AttributeLogicalName("reportscripterrors")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ReportScriptErrors
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("reportscripterrors");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("ReportScriptErrors");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("reportscripterrors", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("reportscripterrors", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ReportScriptErrors");
			}
		}

		[AttributeLogicalName("requireapprovalforqueueemail")]
		[ExcludeFromCodeCoverage]
		public bool? RequireApprovalForQueueEmail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("requireapprovalforqueueemail");
			}
			set
			{
				OnPropertyChanging("RequireApprovalForQueueEmail");
				((Entity)this).SetAttributeValue("requireapprovalforqueueemail", (object)value);
				OnPropertyChanged("RequireApprovalForQueueEmail");
			}
		}

		[AttributeLogicalName("requireapprovalforuseremail")]
		[ExcludeFromCodeCoverage]
		public bool? RequireApprovalForUserEmail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("requireapprovalforuseremail");
			}
			set
			{
				OnPropertyChanging("RequireApprovalForUserEmail");
				((Entity)this).SetAttributeValue("requireapprovalforuseremail", (object)value);
				OnPropertyChanged("RequireApprovalForUserEmail");
			}
		}

		[AttributeLogicalName("restrictstatusupdate")]
		[ExcludeFromCodeCoverage]
		public bool? RestrictStatusUpdate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("restrictstatusupdate");
			}
			set
			{
				OnPropertyChanging("RestrictStatusUpdate");
				((Entity)this).SetAttributeValue("restrictstatusupdate", (object)value);
				OnPropertyChanged("RestrictStatusUpdate");
			}
		}

		[AttributeLogicalName("sampledataimportid")]
		[ExcludeFromCodeCoverage]
		public Guid? SampleDataImportId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("sampledataimportid");
			}
			set
			{
				OnPropertyChanging("SampleDataImportId");
				((Entity)this).SetAttributeValue("sampledataimportid", (object)value);
				OnPropertyChanged("SampleDataImportId");
			}
		}

		[AttributeLogicalName("schemanameprefix")]
		[ExcludeFromCodeCoverage]
		public string SchemaNamePrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("schemanameprefix");
			}
			set
			{
				OnPropertyChanging("SchemaNamePrefix");
				((Entity)this).SetAttributeValue("schemanameprefix", (object)value);
				OnPropertyChanged("SchemaNamePrefix");
			}
		}

		[AttributeLogicalName("sharepointdeploymenttype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue SharePointDeploymentType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("sharepointdeploymenttype");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("SharePointDeploymentType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("sharepointdeploymenttype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("sharepointdeploymenttype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("SharePointDeploymentType");
			}
		}

		[AttributeLogicalName("sharetopreviousowneronassign")]
		[ExcludeFromCodeCoverage]
		public bool? ShareToPreviousOwnerOnAssign
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("sharetopreviousowneronassign");
			}
			set
			{
				OnPropertyChanging("ShareToPreviousOwnerOnAssign");
				((Entity)this).SetAttributeValue("sharetopreviousowneronassign", (object)value);
				OnPropertyChanged("ShareToPreviousOwnerOnAssign");
			}
		}

		[AttributeLogicalName("showkbarticledeprecationnotification")]
		[ExcludeFromCodeCoverage]
		public bool? ShowKBArticleDeprecationNotification
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("showkbarticledeprecationnotification");
			}
			set
			{
				OnPropertyChanging("ShowKBArticleDeprecationNotification");
				((Entity)this).SetAttributeValue("showkbarticledeprecationnotification", (object)value);
				OnPropertyChanged("ShowKBArticleDeprecationNotification");
			}
		}

		[AttributeLogicalName("showweeknumber")]
		[ExcludeFromCodeCoverage]
		public bool? ShowWeekNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("showweeknumber");
			}
			set
			{
				OnPropertyChanging("ShowWeekNumber");
				((Entity)this).SetAttributeValue("showweeknumber", (object)value);
				OnPropertyChanged("ShowWeekNumber");
			}
		}

		[AttributeLogicalName("signupoutlookdownloadfwlink")]
		[ExcludeFromCodeCoverage]
		public string SignupOutlookDownloadFWLink
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("signupoutlookdownloadfwlink");
			}
			set
			{
				OnPropertyChanging("SignupOutlookDownloadFWLink");
				((Entity)this).SetAttributeValue("signupoutlookdownloadfwlink", (object)value);
				OnPropertyChanged("SignupOutlookDownloadFWLink");
			}
		}

		[AttributeLogicalName("sitemapxml")]
		[Obsolete]
		[ExcludeFromCodeCoverage]
		public string SiteMapXml
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("sitemapxml");
			}
			set
			{
				OnPropertyChanging("SiteMapXml");
				((Entity)this).SetAttributeValue("sitemapxml", (object)value);
				OnPropertyChanged("SiteMapXml");
			}
		}

		[AttributeLogicalName("slapausestates")]
		[ExcludeFromCodeCoverage]
		public string SlaPauseStates
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("slapausestates");
			}
			set
			{
				OnPropertyChanging("SlaPauseStates");
				((Entity)this).SetAttributeValue("slapausestates", (object)value);
				OnPropertyChanged("SlaPauseStates");
			}
		}

		[AttributeLogicalName("socialinsightsenabled")]
		[ExcludeFromCodeCoverage]
		public bool? SocialInsightsEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("socialinsightsenabled");
			}
			set
			{
				OnPropertyChanging("SocialInsightsEnabled");
				((Entity)this).SetAttributeValue("socialinsightsenabled", (object)value);
				OnPropertyChanged("SocialInsightsEnabled");
			}
		}

		[AttributeLogicalName("socialinsightsinstance")]
		[ExcludeFromCodeCoverage]
		public string SocialInsightsInstance
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("socialinsightsinstance");
			}
			set
			{
				OnPropertyChanging("SocialInsightsInstance");
				((Entity)this).SetAttributeValue("socialinsightsinstance", (object)value);
				OnPropertyChanged("SocialInsightsInstance");
			}
		}

		[AttributeLogicalName("socialinsightstermsaccepted")]
		[ExcludeFromCodeCoverage]
		public bool? SocialInsightsTermsAccepted
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("socialinsightstermsaccepted");
			}
			set
			{
				OnPropertyChanging("SocialInsightsTermsAccepted");
				((Entity)this).SetAttributeValue("socialinsightstermsaccepted", (object)value);
				OnPropertyChanged("SocialInsightsTermsAccepted");
			}
		}

		[AttributeLogicalName("sortid")]
		[ExcludeFromCodeCoverage]
		public int? SortId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("sortid");
			}
			set
			{
				OnPropertyChanging("SortId");
				((Entity)this).SetAttributeValue("sortid", (object)value);
				OnPropertyChanged("SortId");
			}
		}

		[AttributeLogicalName("sqlaccessgroupid")]
		[ExcludeFromCodeCoverage]
		public Guid? SqlAccessGroupId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("sqlaccessgroupid");
			}
			set
			{
				OnPropertyChanging("SqlAccessGroupId");
				((Entity)this).SetAttributeValue("sqlaccessgroupid", (object)value);
				OnPropertyChanged("SqlAccessGroupId");
			}
		}

		[AttributeLogicalName("sqlaccessgroupname")]
		[ExcludeFromCodeCoverage]
		public string SqlAccessGroupName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("sqlaccessgroupname");
			}
			set
			{
				OnPropertyChanging("SqlAccessGroupName");
				((Entity)this).SetAttributeValue("sqlaccessgroupname", (object)value);
				OnPropertyChanged("SqlAccessGroupName");
			}
		}

		[AttributeLogicalName("sqmenabled")]
		[ExcludeFromCodeCoverage]
		public bool? SQMEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("sqmenabled");
			}
			set
			{
				OnPropertyChanging("SQMEnabled");
				((Entity)this).SetAttributeValue("sqmenabled", (object)value);
				OnPropertyChanged("SQMEnabled");
			}
		}

		[AttributeLogicalName("supportuserid")]
		[ExcludeFromCodeCoverage]
		public Guid? SupportUserId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("supportuserid");
			}
			set
			{
				OnPropertyChanging("SupportUserId");
				((Entity)this).SetAttributeValue("supportuserid", (object)value);
				OnPropertyChanged("SupportUserId");
			}
		}

		[AttributeLogicalName("suppresssla")]
		[ExcludeFromCodeCoverage]
		public bool? SuppressSLA
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("suppresssla");
			}
			set
			{
				OnPropertyChanging("SuppressSLA");
				((Entity)this).SetAttributeValue("suppresssla", (object)value);
				OnPropertyChanged("SuppressSLA");
			}
		}

		[AttributeLogicalName("systemuserid")]
		[ExcludeFromCodeCoverage]
		public Guid? SystemUserId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("systemuserid");
			}
			set
			{
				OnPropertyChanging("SystemUserId");
				((Entity)this).SetAttributeValue("systemuserid", (object)value);
				OnPropertyChanged("SystemUserId");
			}
		}

		[AttributeLogicalName("tagmaxaggressivecycles")]
		[ExcludeFromCodeCoverage]
		public int? TagMaxAggressiveCycles
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("tagmaxaggressivecycles");
			}
			set
			{
				OnPropertyChanging("TagMaxAggressiveCycles");
				((Entity)this).SetAttributeValue("tagmaxaggressivecycles", (object)value);
				OnPropertyChanged("TagMaxAggressiveCycles");
			}
		}

		[AttributeLogicalName("tagpollingperiod")]
		[ExcludeFromCodeCoverage]
		public int? TagPollingPeriod
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("tagpollingperiod");
			}
			set
			{
				OnPropertyChanging("TagPollingPeriod");
				((Entity)this).SetAttributeValue("tagpollingperiod", (object)value);
				OnPropertyChanged("TagPollingPeriod");
			}
		}

		[AttributeLogicalName("taskbasedflowenabled")]
		[ExcludeFromCodeCoverage]
		public bool? TaskBasedFlowEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("taskbasedflowenabled");
			}
			set
			{
				OnPropertyChanging("TaskBasedFlowEnabled");
				((Entity)this).SetAttributeValue("taskbasedflowenabled", (object)value);
				OnPropertyChanged("TaskBasedFlowEnabled");
			}
		}

		[AttributeLogicalName("textanalyticsenabled")]
		[ExcludeFromCodeCoverage]
		public bool? TextAnalyticsEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("textanalyticsenabled");
			}
			set
			{
				OnPropertyChanging("TextAnalyticsEnabled");
				((Entity)this).SetAttributeValue("textanalyticsenabled", (object)value);
				OnPropertyChanged("TextAnalyticsEnabled");
			}
		}

		[AttributeLogicalName("timeformatcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue TimeFormatCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("timeformatcode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("TimeFormatCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("timeformatcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("timeformatcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("TimeFormatCode");
			}
		}

		[AttributeLogicalName("timeformatstring")]
		[ExcludeFromCodeCoverage]
		public string TimeFormatString
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("timeformatstring");
			}
			set
			{
				OnPropertyChanging("TimeFormatString");
				((Entity)this).SetAttributeValue("timeformatstring", (object)value);
				OnPropertyChanged("TimeFormatString");
			}
		}

		[AttributeLogicalName("timeseparator")]
		[ExcludeFromCodeCoverage]
		public string TimeSeparator
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("timeseparator");
			}
			set
			{
				OnPropertyChanging("TimeSeparator");
				((Entity)this).SetAttributeValue("timeseparator", (object)value);
				OnPropertyChanged("TimeSeparator");
			}
		}

		[AttributeLogicalName("timezoneruleversionnumber")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneRuleVersionNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezoneruleversionnumber");
			}
			set
			{
				OnPropertyChanging("TimeZoneRuleVersionNumber");
				((Entity)this).SetAttributeValue("timezoneruleversionnumber", (object)value);
				OnPropertyChanged("TimeZoneRuleVersionNumber");
			}
		}

		[AttributeLogicalName("tokenexpiry")]
		[ExcludeFromCodeCoverage]
		public int? TokenExpiry
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("tokenexpiry");
			}
			set
			{
				OnPropertyChanging("TokenExpiry");
				((Entity)this).SetAttributeValue("tokenexpiry", (object)value);
				OnPropertyChanged("TokenExpiry");
			}
		}

		[AttributeLogicalName("tokenkey")]
		[ExcludeFromCodeCoverage]
		public string TokenKey
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("tokenkey");
			}
			set
			{
				OnPropertyChanging("TokenKey");
				((Entity)this).SetAttributeValue("tokenkey", (object)value);
				OnPropertyChanged("TokenKey");
			}
		}

		[AttributeLogicalName("trackingprefix")]
		[ExcludeFromCodeCoverage]
		public string TrackingPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("trackingprefix");
			}
			set
			{
				OnPropertyChanging("TrackingPrefix");
				((Entity)this).SetAttributeValue("trackingprefix", (object)value);
				OnPropertyChanged("TrackingPrefix");
			}
		}

		[AttributeLogicalName("trackingtokenidbase")]
		[ExcludeFromCodeCoverage]
		public int? TrackingTokenIdBase
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("trackingtokenidbase");
			}
			set
			{
				OnPropertyChanging("TrackingTokenIdBase");
				((Entity)this).SetAttributeValue("trackingtokenidbase", (object)value);
				OnPropertyChanged("TrackingTokenIdBase");
			}
		}

		[AttributeLogicalName("trackingtokeniddigits")]
		[ExcludeFromCodeCoverage]
		public int? TrackingTokenIdDigits
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("trackingtokeniddigits");
			}
			set
			{
				OnPropertyChanging("TrackingTokenIdDigits");
				((Entity)this).SetAttributeValue("trackingtokeniddigits", (object)value);
				OnPropertyChanged("TrackingTokenIdDigits");
			}
		}

		[AttributeLogicalName("uniquespecifierlength")]
		[ExcludeFromCodeCoverage]
		public int? UniqueSpecifierLength
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("uniquespecifierlength");
			}
			set
			{
				OnPropertyChanging("UniqueSpecifierLength");
				((Entity)this).SetAttributeValue("uniquespecifierlength", (object)value);
				OnPropertyChanged("UniqueSpecifierLength");
			}
		}

		[AttributeLogicalName("useinbuiltrulefordefaultpricelistselection")]
		[ExcludeFromCodeCoverage]
		public bool? UseInbuiltRuleForDefaultPricelistSelection
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("useinbuiltrulefordefaultpricelistselection");
			}
			set
			{
				OnPropertyChanging("UseInbuiltRuleForDefaultPricelistSelection");
				((Entity)this).SetAttributeValue("useinbuiltrulefordefaultpricelistselection", (object)value);
				OnPropertyChanged("UseInbuiltRuleForDefaultPricelistSelection");
			}
		}

		[AttributeLogicalName("uselegacyrendering")]
		[ExcludeFromCodeCoverage]
		public bool? UseLegacyRendering
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("uselegacyrendering");
			}
			set
			{
				OnPropertyChanging("UseLegacyRendering");
				((Entity)this).SetAttributeValue("uselegacyrendering", (object)value);
				OnPropertyChanged("UseLegacyRendering");
			}
		}

		[AttributeLogicalName("usepositionhierarchy")]
		[ExcludeFromCodeCoverage]
		public bool? UsePositionHierarchy
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("usepositionhierarchy");
			}
			set
			{
				OnPropertyChanging("UsePositionHierarchy");
				((Entity)this).SetAttributeValue("usepositionhierarchy", (object)value);
				OnPropertyChanged("UsePositionHierarchy");
			}
		}

		[AttributeLogicalName("useraccessauditinginterval")]
		[ExcludeFromCodeCoverage]
		public int? UserAccessAuditingInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("useraccessauditinginterval");
			}
			set
			{
				OnPropertyChanging("UserAccessAuditingInterval");
				((Entity)this).SetAttributeValue("useraccessauditinginterval", (object)value);
				OnPropertyChanged("UserAccessAuditingInterval");
			}
		}

		[AttributeLogicalName("usereadform")]
		[ExcludeFromCodeCoverage]
		public bool? UseReadForm
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("usereadform");
			}
			set
			{
				OnPropertyChanging("UseReadForm");
				((Entity)this).SetAttributeValue("usereadform", (object)value);
				OnPropertyChanged("UseReadForm");
			}
		}

		[AttributeLogicalName("usergroupid")]
		[ExcludeFromCodeCoverage]
		public Guid? UserGroupId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("usergroupid");
			}
			set
			{
				OnPropertyChanging("UserGroupId");
				((Entity)this).SetAttributeValue("usergroupid", (object)value);
				OnPropertyChanged("UserGroupId");
			}
		}

		[AttributeLogicalName("useskypeprotocol")]
		[ExcludeFromCodeCoverage]
		public bool? UseSkypeProtocol
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("useskypeprotocol");
			}
			set
			{
				OnPropertyChanging("UseSkypeProtocol");
				((Entity)this).SetAttributeValue("useskypeprotocol", (object)value);
				OnPropertyChanged("UseSkypeProtocol");
			}
		}

		[AttributeLogicalName("utcconversiontimezonecode")]
		[ExcludeFromCodeCoverage]
		public int? UTCConversionTimeZoneCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("utcconversiontimezonecode");
			}
			set
			{
				OnPropertyChanging("UTCConversionTimeZoneCode");
				((Entity)this).SetAttributeValue("utcconversiontimezonecode", (object)value);
				OnPropertyChanged("UTCConversionTimeZoneCode");
			}
		}

		[AttributeLogicalName("v3calloutconfighash")]
		[ExcludeFromCodeCoverage]
		public string V3CalloutConfigHash => ((Entity)this).GetAttributeValue<string>("v3calloutconfighash");

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[AttributeLogicalName("webresourcehash")]
		[ExcludeFromCodeCoverage]
		public string WebResourceHash
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("webresourcehash");
			}
			set
			{
				OnPropertyChanging("WebResourceHash");
				((Entity)this).SetAttributeValue("webresourcehash", (object)value);
				OnPropertyChanged("WebResourceHash");
			}
		}

		[AttributeLogicalName("weekstartdaycode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue WeekStartDayCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("weekstartdaycode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("WeekStartDayCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("weekstartdaycode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("weekstartdaycode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("WeekStartDayCode");
			}
		}

		[AttributeLogicalName("yammergroupid")]
		[ExcludeFromCodeCoverage]
		public int? YammerGroupId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("yammergroupid");
			}
			set
			{
				OnPropertyChanging("YammerGroupId");
				((Entity)this).SetAttributeValue("yammergroupid", (object)value);
				OnPropertyChanged("YammerGroupId");
			}
		}

		[AttributeLogicalName("yammernetworkpermalink")]
		[ExcludeFromCodeCoverage]
		public string YammerNetworkPermalink
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("yammernetworkpermalink");
			}
			set
			{
				OnPropertyChanging("YammerNetworkPermalink");
				((Entity)this).SetAttributeValue("yammernetworkpermalink", (object)value);
				OnPropertyChanged("YammerNetworkPermalink");
			}
		}

		[AttributeLogicalName("yammeroauthaccesstokenexpired")]
		[ExcludeFromCodeCoverage]
		public bool? YammerOAuthAccessTokenExpired
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("yammeroauthaccesstokenexpired");
			}
			set
			{
				OnPropertyChanging("YammerOAuthAccessTokenExpired");
				((Entity)this).SetAttributeValue("yammeroauthaccesstokenexpired", (object)value);
				OnPropertyChanged("YammerOAuthAccessTokenExpired");
			}
		}

		[AttributeLogicalName("yammerpostmethod")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue YammerPostMethod
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("yammerpostmethod");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
			set
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected O, but got Unknown
				OnPropertyChanging("YammerPostMethod");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("yammerpostmethod", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("yammerpostmethod", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("YammerPostMethod");
			}
		}

		[AttributeLogicalName("yearstartweekcode")]
		[ExcludeFromCodeCoverage]
		public int? YearStartWeekCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("yearstartweekcode");
			}
			set
			{
				OnPropertyChanging("YearStartWeekCode");
				((Entity)this).SetAttributeValue("yearstartweekcode", (object)value);
				OnPropertyChanged("YearStartWeekCode");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Organization()
			: this("organization")
		{
		}

		[ExcludeFromCodeCoverage]
		private void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		[ExcludeFromCodeCoverage]
		private void OnPropertyChanging(string propertyName)
		{
			if (this.PropertyChanging != null)
			{
				this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
			}
		}
	}
}
