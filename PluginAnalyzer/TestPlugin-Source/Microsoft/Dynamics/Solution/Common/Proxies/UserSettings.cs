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
	[EntityLogicalName("usersettings")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class UserSettings : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "usersettings";

		public const int EntityTypeCode = 150;

		public const int AttributeWorkdayStartTime_MaxLength = 5;

		public const int AttributeDefaultCountryCode_MaxLength = 30;

		public const int AttributeDateSeparator_MaxLength = 5;

		public const int AttributeEmailPassword_MaxLength = 200;

		public const int AttributeUserProfile_MaxLength = 1024;

		public const int AttributeDateFormatString_MaxLength = 255;

		public const int AttributePMDesignator_MaxLength = 25;

		public const int AttributeTimeSeparator_MaxLength = 5;

		public const int AttributeCurrencySymbol_MaxLength = 13;

		public const int AttributeNumberSeparator_MaxLength = 5;

		public const int AttributeNumberGroupFormat_MaxLength = 25;

		public const int AttributeHomepageArea_MaxLength = 200;

		public const int AttributeDecimalSymbol_MaxLength = 5;

		public const int AttributeEmailUsername_MaxLength = 200;

		public const int AttributeAMDesignator_MaxLength = 25;

		public const int AttributeHomepageLayout_MaxLength = 2000;

		public const int AttributeTimeFormatString_MaxLength = 255;

		public const int AttributeWorkdayStopTime_MaxLength = 5;

		public const int AttributeHomepageSubarea_MaxLength = 200;

		public const string AttributeAddressBookSyncInterval = "addressbooksyncinterval";

		public const string AttributeAdvancedFindStartupMode = "advancedfindstartupmode";

		public const string AttributeAllowEmailCredentials = "allowemailcredentials";

		public const string AttributeAMDesignator = "amdesignator";

		public const string AttributeAutoCreateContactOnPromote = "autocreatecontactonpromote";

		public const string AttributeBusinessUnitId = "businessunitid";

		public const string AttributeCalendarType = "calendartype";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeCurrencyDecimalPrecision = "currencydecimalprecision";

		public const string AttributeCurrencyFormatCode = "currencyformatcode";

		public const string AttributeCurrencySymbol = "currencysymbol";

		public const string AttributeDataValidationModeForExportToExcel = "datavalidationmodeforexporttoexcel";

		public const string AttributeDateFormatCode = "dateformatcode";

		public const string AttributeDateFormatString = "dateformatstring";

		public const string AttributeDateSeparator = "dateseparator";

		public const string AttributeDecimalSymbol = "decimalsymbol";

		public const string AttributeDefaultCalendarView = "defaultcalendarview";

		public const string AttributeDefaultCountryCode = "defaultcountrycode";

		public const string AttributeDefaultDashboardId = "defaultdashboardid";

		public const string AttributeDefaultSearchExperience = "defaultsearchexperience";

		public const string AttributeEmailPassword = "emailpassword";

		public const string AttributeEmailUsername = "emailusername";

		public const string AttributeEntityFormMode = "entityformmode";

		public const string AttributeFullNameConventionCode = "fullnameconventioncode";

		public const string AttributeGetStartedPaneContentEnabled = "getstartedpanecontentenabled";

		public const string AttributeHelpLanguageId = "helplanguageid";

		public const string AttributeHomepageArea = "homepagearea";

		public const string AttributeHomepageLayout = "homepagelayout";

		public const string AttributeHomepageSubarea = "homepagesubarea";

		public const string AttributeIgnoreUnsolicitedEmail = "ignoreunsolicitedemail";

		public const string AttributeIncomingEmailFilteringMethod = "incomingemailfilteringmethod";

		public const string AttributeIsAppsForCrmAlertDismissed = "isappsforcrmalertdismissed";

		public const string AttributeIsAutoDataCaptureEnabled = "isautodatacaptureenabled";

		public const string AttributeIsDefaultCountryCodeCheckEnabled = "isdefaultcountrycodecheckenabled";

		public const string AttributeIsDuplicateDetectionEnabledWhenGoingOnline = "isduplicatedetectionenabledwhengoingonline";

		public const string AttributeIsGuidedHelpEnabled = "isguidedhelpenabled";

		public const string AttributeIsResourceBookingExchangeSyncEnabled = "isresourcebookingexchangesyncenabled";

		public const string AttributeIsSendAsAllowed = "issendasallowed";

		public const string AttributeLastAlertsViewedTime = "lastalertsviewedtime";

		public const string AttributeLocaleId = "localeid";

		public const string AttributeLongDateFormatCode = "longdateformatcode";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeNegativeCurrencyFormatCode = "negativecurrencyformatcode";

		public const string AttributeNegativeFormatCode = "negativeformatcode";

		public const string AttributeNextTrackingNumber = "nexttrackingnumber";

		public const string AttributeNumberGroupFormat = "numbergroupformat";

		public const string AttributeNumberSeparator = "numberseparator";

		public const string AttributeOfflineSyncInterval = "offlinesyncinterval";

		public const string AttributeOutlookSyncInterval = "outlooksyncinterval";

		public const string AttributePagingLimit = "paginglimit";

		public const string AttributePersonalizationSettings = "personalizationsettings";

		public const string AttributePMDesignator = "pmdesignator";

		public const string AttributePricingDecimalPrecision = "pricingdecimalprecision";

		public const string AttributeReportScriptErrors = "reportscripterrors";

		public const string AttributeResourceBookingExchangeSyncVersion = "resourcebookingexchangesyncversion";

		public const string AttributeShowWeekNumber = "showweeknumber";

		public const string AttributeSplitViewState = "splitviewstate";

		public const string AttributeSyncContactCompany = "synccontactcompany";

		public const string AttributeSystemUserId = "systemuserid";

		public const string AttributeId = "systemuserid";

		public const string AttributeTimeFormatCode = "timeformatcode";

		public const string AttributeTimeFormatString = "timeformatstring";

		public const string AttributeTimeSeparator = "timeseparator";

		public const string AttributeTimeZoneBias = "timezonebias";

		public const string AttributeTimeZoneCode = "timezonecode";

		public const string AttributeTimeZoneDaylightBias = "timezonedaylightbias";

		public const string AttributeTimeZoneDaylightDay = "timezonedaylightday";

		public const string AttributeTimeZoneDaylightDayOfWeek = "timezonedaylightdayofweek";

		public const string AttributeTimeZoneDaylightHour = "timezonedaylighthour";

		public const string AttributeTimeZoneDaylightMinute = "timezonedaylightminute";

		public const string AttributeTimeZoneDaylightMonth = "timezonedaylightmonth";

		public const string AttributeTimeZoneDaylightSecond = "timezonedaylightsecond";

		public const string AttributeTimeZoneDaylightYear = "timezonedaylightyear";

		public const string AttributeTimeZoneStandardBias = "timezonestandardbias";

		public const string AttributeTimeZoneStandardDay = "timezonestandardday";

		public const string AttributeTimeZoneStandardDayOfWeek = "timezonestandarddayofweek";

		public const string AttributeTimeZoneStandardHour = "timezonestandardhour";

		public const string AttributeTimeZoneStandardMinute = "timezonestandardminute";

		public const string AttributeTimeZoneStandardMonth = "timezonestandardmonth";

		public const string AttributeTimeZoneStandardSecond = "timezonestandardsecond";

		public const string AttributeTimeZoneStandardYear = "timezonestandardyear";

		public const string AttributeTrackingTokenId = "trackingtokenid";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeUILanguageId = "uilanguageid";

		public const string AttributeUseCrmFormForAppointment = "usecrmformforappointment";

		public const string AttributeUseCrmFormForContact = "usecrmformforcontact";

		public const string AttributeUseCrmFormForEmail = "usecrmformforemail";

		public const string AttributeUseCrmFormForTask = "usecrmformfortask";

		public const string AttributeUseImageStrips = "useimagestrips";

		public const string AttributeUserProfile = "userprofile";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeVisualizationPaneLayout = "visualizationpanelayout";

		public const string AttributeWorkdayStartTime = "workdaystarttime";

		public const string AttributeWorkdayStopTime = "workdaystoptime";

		[AttributeLogicalName("addressbooksyncinterval")]
		[ExcludeFromCodeCoverage]
		public int? AddressBookSyncInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("addressbooksyncinterval");
			}
			set
			{
				OnPropertyChanging("AddressBookSyncInterval");
				((Entity)this).SetAttributeValue("addressbooksyncinterval", (object)value);
				OnPropertyChanged("AddressBookSyncInterval");
			}
		}

		[AttributeLogicalName("advancedfindstartupmode")]
		[ExcludeFromCodeCoverage]
		public int? AdvancedFindStartupMode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("advancedfindstartupmode");
			}
			set
			{
				OnPropertyChanging("AdvancedFindStartupMode");
				((Entity)this).SetAttributeValue("advancedfindstartupmode", (object)value);
				OnPropertyChanged("AdvancedFindStartupMode");
			}
		}

		[AttributeLogicalName("allowemailcredentials")]
		[Obsolete]
		[ExcludeFromCodeCoverage]
		public bool? AllowEmailCredentials => ((Entity)this).GetAttributeValue<bool?>("allowemailcredentials");

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

		[AttributeLogicalName("autocreatecontactonpromote")]
		[ExcludeFromCodeCoverage]
		public int? AutoCreateContactOnPromote
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("autocreatecontactonpromote");
			}
			set
			{
				OnPropertyChanging("AutoCreateContactOnPromote");
				((Entity)this).SetAttributeValue("autocreatecontactonpromote", (object)value);
				OnPropertyChanged("AutoCreateContactOnPromote");
			}
		}

		[AttributeLogicalName("businessunitid")]
		[ExcludeFromCodeCoverage]
		public Guid? BusinessUnitId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("businessunitid");
			}
			set
			{
				OnPropertyChanging("BusinessUnitId");
				((Entity)this).SetAttributeValue("businessunitid", (object)value);
				OnPropertyChanged("BusinessUnitId");
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

		[AttributeLogicalName("createdby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedBy => ((Entity)this).GetAttributeValue<EntityReference>("createdby");

		[AttributeLogicalName("createdon")]
		[ExcludeFromCodeCoverage]
		public DateTime? CreatedOn => ((Entity)this).GetAttributeValue<DateTime?>("createdon");

		[AttributeLogicalName("createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("createdonbehalfby");

		[AttributeLogicalName("currencydecimalprecision")]
		[Obsolete]
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

		[AttributeLogicalName("currencyformatcode")]
		[ExcludeFromCodeCoverage]
		public int? CurrencyFormatCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currencyformatcode");
			}
			set
			{
				OnPropertyChanging("CurrencyFormatCode");
				((Entity)this).SetAttributeValue("currencyformatcode", (object)value);
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

		[AttributeLogicalName("datavalidationmodeforexporttoexcel")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue DataValidationModeForExportToExcel
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("datavalidationmodeforexporttoexcel");
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
				OnPropertyChanging("DataValidationModeForExportToExcel");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("datavalidationmodeforexporttoexcel", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("datavalidationmodeforexporttoexcel", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("DataValidationModeForExportToExcel");
			}
		}

		[AttributeLogicalName("dateformatcode")]
		[ExcludeFromCodeCoverage]
		public int? DateFormatCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("dateformatcode");
			}
			set
			{
				OnPropertyChanging("DateFormatCode");
				((Entity)this).SetAttributeValue("dateformatcode", (object)value);
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

		[AttributeLogicalName("defaultcalendarview")]
		[ExcludeFromCodeCoverage]
		public int? DefaultCalendarView
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("defaultcalendarview");
			}
			set
			{
				OnPropertyChanging("DefaultCalendarView");
				((Entity)this).SetAttributeValue("defaultcalendarview", (object)value);
				OnPropertyChanged("DefaultCalendarView");
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

		[AttributeLogicalName("defaultdashboardid")]
		[ExcludeFromCodeCoverage]
		public Guid? DefaultDashboardId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("defaultdashboardid");
			}
			set
			{
				OnPropertyChanging("DefaultDashboardId");
				((Entity)this).SetAttributeValue("defaultdashboardid", (object)value);
				OnPropertyChanged("DefaultDashboardId");
			}
		}

		[AttributeLogicalName("defaultsearchexperience")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue DefaultSearchExperience
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("defaultsearchexperience");
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
				OnPropertyChanging("DefaultSearchExperience");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("defaultsearchexperience", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("defaultsearchexperience", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("DefaultSearchExperience");
			}
		}

		[AttributeLogicalName("emailpassword")]
		[Obsolete]
		[ExcludeFromCodeCoverage]
		public string EmailPassword => ((Entity)this).GetAttributeValue<string>("emailpassword");

		[AttributeLogicalName("emailusername")]
		[Obsolete]
		[ExcludeFromCodeCoverage]
		public string EmailUsername => ((Entity)this).GetAttributeValue<string>("emailusername");

		[AttributeLogicalName("entityformmode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue EntityFormMode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("entityformmode");
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
				OnPropertyChanging("EntityFormMode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("entityformmode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("entityformmode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("EntityFormMode");
			}
		}

		[AttributeLogicalName("fullnameconventioncode")]
		[ExcludeFromCodeCoverage]
		public int? FullNameConventionCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("fullnameconventioncode");
			}
			set
			{
				OnPropertyChanging("FullNameConventionCode");
				((Entity)this).SetAttributeValue("fullnameconventioncode", (object)value);
				OnPropertyChanged("FullNameConventionCode");
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

		[AttributeLogicalName("helplanguageid")]
		[ExcludeFromCodeCoverage]
		public int? HelpLanguageId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("helplanguageid");
			}
			set
			{
				OnPropertyChanging("HelpLanguageId");
				((Entity)this).SetAttributeValue("helplanguageid", (object)value);
				OnPropertyChanged("HelpLanguageId");
			}
		}

		[AttributeLogicalName("homepagearea")]
		[ExcludeFromCodeCoverage]
		public string HomepageArea
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("homepagearea");
			}
			set
			{
				OnPropertyChanging("HomepageArea");
				((Entity)this).SetAttributeValue("homepagearea", (object)value);
				OnPropertyChanged("HomepageArea");
			}
		}

		[AttributeLogicalName("homepagelayout")]
		[ExcludeFromCodeCoverage]
		public string HomepageLayout
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("homepagelayout");
			}
			set
			{
				OnPropertyChanging("HomepageLayout");
				((Entity)this).SetAttributeValue("homepagelayout", (object)value);
				OnPropertyChanged("HomepageLayout");
			}
		}

		[AttributeLogicalName("homepagesubarea")]
		[ExcludeFromCodeCoverage]
		public string HomepageSubarea
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("homepagesubarea");
			}
			set
			{
				OnPropertyChanging("HomepageSubarea");
				((Entity)this).SetAttributeValue("homepagesubarea", (object)value);
				OnPropertyChanged("HomepageSubarea");
			}
		}

		[AttributeLogicalName("ignoreunsolicitedemail")]
		[ExcludeFromCodeCoverage]
		public bool? IgnoreUnsolicitedEmail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ignoreunsolicitedemail");
			}
			set
			{
				OnPropertyChanging("IgnoreUnsolicitedEmail");
				((Entity)this).SetAttributeValue("ignoreunsolicitedemail", (object)value);
				OnPropertyChanged("IgnoreUnsolicitedEmail");
			}
		}

		[AttributeLogicalName("incomingemailfilteringmethod")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue IncomingEmailFilteringMethod
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("incomingemailfilteringmethod");
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
				OnPropertyChanging("IncomingEmailFilteringMethod");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("incomingemailfilteringmethod", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("incomingemailfilteringmethod", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("IncomingEmailFilteringMethod");
			}
		}

		[AttributeLogicalName("isappsforcrmalertdismissed")]
		[ExcludeFromCodeCoverage]
		public bool? IsAppsForCrmAlertDismissed
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isappsforcrmalertdismissed");
			}
			set
			{
				OnPropertyChanging("IsAppsForCrmAlertDismissed");
				((Entity)this).SetAttributeValue("isappsforcrmalertdismissed", (object)value);
				OnPropertyChanged("IsAppsForCrmAlertDismissed");
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

		[AttributeLogicalName("isduplicatedetectionenabledwhengoingonline")]
		[ExcludeFromCodeCoverage]
		public bool? IsDuplicateDetectionEnabledWhenGoingOnline
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isduplicatedetectionenabledwhengoingonline");
			}
			set
			{
				OnPropertyChanging("IsDuplicateDetectionEnabledWhenGoingOnline");
				((Entity)this).SetAttributeValue("isduplicatedetectionenabledwhengoingonline", (object)value);
				OnPropertyChanged("IsDuplicateDetectionEnabledWhenGoingOnline");
			}
		}

		[AttributeLogicalName("isguidedhelpenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsGuidedHelpEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isguidedhelpenabled");
			}
			set
			{
				OnPropertyChanging("IsGuidedHelpEnabled");
				((Entity)this).SetAttributeValue("isguidedhelpenabled", (object)value);
				OnPropertyChanged("IsGuidedHelpEnabled");
			}
		}

		[AttributeLogicalName("isresourcebookingexchangesyncenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsResourceBookingExchangeSyncEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isresourcebookingexchangesyncenabled");
			}
			set
			{
				OnPropertyChanging("IsResourceBookingExchangeSyncEnabled");
				((Entity)this).SetAttributeValue("isresourcebookingexchangesyncenabled", (object)value);
				OnPropertyChanged("IsResourceBookingExchangeSyncEnabled");
			}
		}

		[AttributeLogicalName("issendasallowed")]
		[ExcludeFromCodeCoverage]
		public bool? IsSendAsAllowed
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("issendasallowed");
			}
			set
			{
				OnPropertyChanging("IsSendAsAllowed");
				((Entity)this).SetAttributeValue("issendasallowed", (object)value);
				OnPropertyChanged("IsSendAsAllowed");
			}
		}

		[AttributeLogicalName("lastalertsviewedtime")]
		[ExcludeFromCodeCoverage]
		public DateTime? LastAlertsViewedTime
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("lastalertsviewedtime");
			}
			set
			{
				OnPropertyChanging("LastAlertsViewedTime");
				((Entity)this).SetAttributeValue("lastalertsviewedtime", (object)value);
				OnPropertyChanged("LastAlertsViewedTime");
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

		[AttributeLogicalName("modifiedby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedby");

		[AttributeLogicalName("modifiedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? ModifiedOn => ((Entity)this).GetAttributeValue<DateTime?>("modifiedon");

		[AttributeLogicalName("modifiedonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedonbehalfby");

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
		public int? NegativeFormatCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("negativeformatcode");
			}
			set
			{
				OnPropertyChanging("NegativeFormatCode");
				((Entity)this).SetAttributeValue("negativeformatcode", (object)value);
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

		[AttributeLogicalName("offlinesyncinterval")]
		[ExcludeFromCodeCoverage]
		public int? OfflineSyncInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("offlinesyncinterval");
			}
			set
			{
				OnPropertyChanging("OfflineSyncInterval");
				((Entity)this).SetAttributeValue("offlinesyncinterval", (object)value);
				OnPropertyChanged("OfflineSyncInterval");
			}
		}

		[AttributeLogicalName("outlooksyncinterval")]
		[ExcludeFromCodeCoverage]
		public int? OutlookSyncInterval
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("outlooksyncinterval");
			}
			set
			{
				OnPropertyChanging("OutlookSyncInterval");
				((Entity)this).SetAttributeValue("outlooksyncinterval", (object)value);
				OnPropertyChanged("OutlookSyncInterval");
			}
		}

		[AttributeLogicalName("paginglimit")]
		[ExcludeFromCodeCoverage]
		public int? PagingLimit
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("paginglimit");
			}
			set
			{
				OnPropertyChanging("PagingLimit");
				((Entity)this).SetAttributeValue("paginglimit", (object)value);
				OnPropertyChanged("PagingLimit");
			}
		}

		[AttributeLogicalName("personalizationsettings")]
		[ExcludeFromCodeCoverage]
		public string PersonalizationSettings
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("personalizationsettings");
			}
			set
			{
				OnPropertyChanging("PersonalizationSettings");
				((Entity)this).SetAttributeValue("personalizationsettings", (object)value);
				OnPropertyChanged("PersonalizationSettings");
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

		[AttributeLogicalName("pricingdecimalprecision")]
		[Obsolete]
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

		[AttributeLogicalName("resourcebookingexchangesyncversion")]
		[ExcludeFromCodeCoverage]
		public long? ResourceBookingExchangeSyncVersion
		{
			get
			{
				return ((Entity)this).GetAttributeValue<long?>("resourcebookingexchangesyncversion");
			}
			set
			{
				OnPropertyChanging("ResourceBookingExchangeSyncVersion");
				((Entity)this).SetAttributeValue("resourcebookingexchangesyncversion", (object)value);
				OnPropertyChanged("ResourceBookingExchangeSyncVersion");
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

		[AttributeLogicalName("splitviewstate")]
		[ExcludeFromCodeCoverage]
		public bool? SplitViewState
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("splitviewstate");
			}
			set
			{
				OnPropertyChanging("SplitViewState");
				((Entity)this).SetAttributeValue("splitviewstate", (object)value);
				OnPropertyChanged("SplitViewState");
			}
		}

		[AttributeLogicalName("synccontactcompany")]
		[ExcludeFromCodeCoverage]
		public bool? SyncContactCompany
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("synccontactcompany");
			}
			set
			{
				OnPropertyChanging("SyncContactCompany");
				((Entity)this).SetAttributeValue("synccontactcompany", (object)value);
				OnPropertyChanged("SyncContactCompany");
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
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("SystemUserId");
			}
		}

		[AttributeLogicalName("systemuserid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				SystemUserId = value;
			}
		}

		[AttributeLogicalName("timeformatcode")]
		[ExcludeFromCodeCoverage]
		public int? TimeFormatCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timeformatcode");
			}
			set
			{
				OnPropertyChanging("TimeFormatCode");
				((Entity)this).SetAttributeValue("timeformatcode", (object)value);
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

		[AttributeLogicalName("timezonebias")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneBias
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonebias");
			}
			set
			{
				OnPropertyChanging("TimeZoneBias");
				((Entity)this).SetAttributeValue("timezonebias", (object)value);
				OnPropertyChanged("TimeZoneBias");
			}
		}

		[AttributeLogicalName("timezonecode")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonecode");
			}
			set
			{
				OnPropertyChanging("TimeZoneCode");
				((Entity)this).SetAttributeValue("timezonecode", (object)value);
				OnPropertyChanged("TimeZoneCode");
			}
		}

		[AttributeLogicalName("timezonedaylightbias")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneDaylightBias
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonedaylightbias");
			}
			set
			{
				OnPropertyChanging("TimeZoneDaylightBias");
				((Entity)this).SetAttributeValue("timezonedaylightbias", (object)value);
				OnPropertyChanged("TimeZoneDaylightBias");
			}
		}

		[AttributeLogicalName("timezonedaylightday")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneDaylightDay
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonedaylightday");
			}
			set
			{
				OnPropertyChanging("TimeZoneDaylightDay");
				((Entity)this).SetAttributeValue("timezonedaylightday", (object)value);
				OnPropertyChanged("TimeZoneDaylightDay");
			}
		}

		[AttributeLogicalName("timezonedaylightdayofweek")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneDaylightDayOfWeek
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonedaylightdayofweek");
			}
			set
			{
				OnPropertyChanging("TimeZoneDaylightDayOfWeek");
				((Entity)this).SetAttributeValue("timezonedaylightdayofweek", (object)value);
				OnPropertyChanged("TimeZoneDaylightDayOfWeek");
			}
		}

		[AttributeLogicalName("timezonedaylighthour")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneDaylightHour
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonedaylighthour");
			}
			set
			{
				OnPropertyChanging("TimeZoneDaylightHour");
				((Entity)this).SetAttributeValue("timezonedaylighthour", (object)value);
				OnPropertyChanged("TimeZoneDaylightHour");
			}
		}

		[AttributeLogicalName("timezonedaylightminute")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneDaylightMinute
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonedaylightminute");
			}
			set
			{
				OnPropertyChanging("TimeZoneDaylightMinute");
				((Entity)this).SetAttributeValue("timezonedaylightminute", (object)value);
				OnPropertyChanged("TimeZoneDaylightMinute");
			}
		}

		[AttributeLogicalName("timezonedaylightmonth")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneDaylightMonth
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonedaylightmonth");
			}
			set
			{
				OnPropertyChanging("TimeZoneDaylightMonth");
				((Entity)this).SetAttributeValue("timezonedaylightmonth", (object)value);
				OnPropertyChanged("TimeZoneDaylightMonth");
			}
		}

		[AttributeLogicalName("timezonedaylightsecond")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneDaylightSecond
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonedaylightsecond");
			}
			set
			{
				OnPropertyChanging("TimeZoneDaylightSecond");
				((Entity)this).SetAttributeValue("timezonedaylightsecond", (object)value);
				OnPropertyChanged("TimeZoneDaylightSecond");
			}
		}

		[AttributeLogicalName("timezonedaylightyear")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneDaylightYear
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonedaylightyear");
			}
			set
			{
				OnPropertyChanging("TimeZoneDaylightYear");
				((Entity)this).SetAttributeValue("timezonedaylightyear", (object)value);
				OnPropertyChanged("TimeZoneDaylightYear");
			}
		}

		[AttributeLogicalName("timezonestandardbias")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneStandardBias
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonestandardbias");
			}
			set
			{
				OnPropertyChanging("TimeZoneStandardBias");
				((Entity)this).SetAttributeValue("timezonestandardbias", (object)value);
				OnPropertyChanged("TimeZoneStandardBias");
			}
		}

		[AttributeLogicalName("timezonestandardday")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneStandardDay
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonestandardday");
			}
			set
			{
				OnPropertyChanging("TimeZoneStandardDay");
				((Entity)this).SetAttributeValue("timezonestandardday", (object)value);
				OnPropertyChanged("TimeZoneStandardDay");
			}
		}

		[AttributeLogicalName("timezonestandarddayofweek")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneStandardDayOfWeek
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonestandarddayofweek");
			}
			set
			{
				OnPropertyChanging("TimeZoneStandardDayOfWeek");
				((Entity)this).SetAttributeValue("timezonestandarddayofweek", (object)value);
				OnPropertyChanged("TimeZoneStandardDayOfWeek");
			}
		}

		[AttributeLogicalName("timezonestandardhour")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneStandardHour
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonestandardhour");
			}
			set
			{
				OnPropertyChanging("TimeZoneStandardHour");
				((Entity)this).SetAttributeValue("timezonestandardhour", (object)value);
				OnPropertyChanged("TimeZoneStandardHour");
			}
		}

		[AttributeLogicalName("timezonestandardminute")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneStandardMinute
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonestandardminute");
			}
			set
			{
				OnPropertyChanging("TimeZoneStandardMinute");
				((Entity)this).SetAttributeValue("timezonestandardminute", (object)value);
				OnPropertyChanged("TimeZoneStandardMinute");
			}
		}

		[AttributeLogicalName("timezonestandardmonth")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneStandardMonth
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonestandardmonth");
			}
			set
			{
				OnPropertyChanging("TimeZoneStandardMonth");
				((Entity)this).SetAttributeValue("timezonestandardmonth", (object)value);
				OnPropertyChanged("TimeZoneStandardMonth");
			}
		}

		[AttributeLogicalName("timezonestandardsecond")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneStandardSecond
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonestandardsecond");
			}
			set
			{
				OnPropertyChanging("TimeZoneStandardSecond");
				((Entity)this).SetAttributeValue("timezonestandardsecond", (object)value);
				OnPropertyChanged("TimeZoneStandardSecond");
			}
		}

		[AttributeLogicalName("timezonestandardyear")]
		[ExcludeFromCodeCoverage]
		public int? TimeZoneStandardYear
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("timezonestandardyear");
			}
			set
			{
				OnPropertyChanging("TimeZoneStandardYear");
				((Entity)this).SetAttributeValue("timezonestandardyear", (object)value);
				OnPropertyChanged("TimeZoneStandardYear");
			}
		}

		[AttributeLogicalName("trackingtokenid")]
		[ExcludeFromCodeCoverage]
		public int? TrackingTokenId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("trackingtokenid");
			}
			set
			{
				OnPropertyChanging("TrackingTokenId");
				((Entity)this).SetAttributeValue("trackingtokenid", (object)value);
				OnPropertyChanged("TrackingTokenId");
			}
		}

		[AttributeLogicalName("transactioncurrencyid")]
		[ExcludeFromCodeCoverage]
		public EntityReference TransactionCurrencyId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("transactioncurrencyid");
			}
			set
			{
				OnPropertyChanging("TransactionCurrencyId");
				((Entity)this).SetAttributeValue("transactioncurrencyid", (object)value);
				OnPropertyChanged("TransactionCurrencyId");
			}
		}

		[AttributeLogicalName("uilanguageid")]
		[ExcludeFromCodeCoverage]
		public int? UILanguageId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("uilanguageid");
			}
			set
			{
				OnPropertyChanging("UILanguageId");
				((Entity)this).SetAttributeValue("uilanguageid", (object)value);
				OnPropertyChanged("UILanguageId");
			}
		}

		[AttributeLogicalName("usecrmformforappointment")]
		[ExcludeFromCodeCoverage]
		public bool? UseCrmFormForAppointment
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("usecrmformforappointment");
			}
			set
			{
				OnPropertyChanging("UseCrmFormForAppointment");
				((Entity)this).SetAttributeValue("usecrmformforappointment", (object)value);
				OnPropertyChanged("UseCrmFormForAppointment");
			}
		}

		[AttributeLogicalName("usecrmformforcontact")]
		[ExcludeFromCodeCoverage]
		public bool? UseCrmFormForContact
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("usecrmformforcontact");
			}
			set
			{
				OnPropertyChanging("UseCrmFormForContact");
				((Entity)this).SetAttributeValue("usecrmformforcontact", (object)value);
				OnPropertyChanged("UseCrmFormForContact");
			}
		}

		[AttributeLogicalName("usecrmformforemail")]
		[ExcludeFromCodeCoverage]
		public bool? UseCrmFormForEmail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("usecrmformforemail");
			}
			set
			{
				OnPropertyChanging("UseCrmFormForEmail");
				((Entity)this).SetAttributeValue("usecrmformforemail", (object)value);
				OnPropertyChanged("UseCrmFormForEmail");
			}
		}

		[AttributeLogicalName("usecrmformfortask")]
		[ExcludeFromCodeCoverage]
		public bool? UseCrmFormForTask
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("usecrmformfortask");
			}
			set
			{
				OnPropertyChanging("UseCrmFormForTask");
				((Entity)this).SetAttributeValue("usecrmformfortask", (object)value);
				OnPropertyChanged("UseCrmFormForTask");
			}
		}

		[AttributeLogicalName("useimagestrips")]
		[ExcludeFromCodeCoverage]
		public bool? UseImageStrips
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("useimagestrips");
			}
			set
			{
				OnPropertyChanging("UseImageStrips");
				((Entity)this).SetAttributeValue("useimagestrips", (object)value);
				OnPropertyChanged("UseImageStrips");
			}
		}

		[AttributeLogicalName("userprofile")]
		[ExcludeFromCodeCoverage]
		public string UserProfile
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("userprofile");
			}
			set
			{
				OnPropertyChanging("UserProfile");
				((Entity)this).SetAttributeValue("userprofile", (object)value);
				OnPropertyChanged("UserProfile");
			}
		}

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[AttributeLogicalName("visualizationpanelayout")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue VisualizationPaneLayout
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("visualizationpanelayout");
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
				OnPropertyChanging("VisualizationPaneLayout");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("visualizationpanelayout", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("visualizationpanelayout", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("VisualizationPaneLayout");
			}
		}

		[AttributeLogicalName("workdaystarttime")]
		[ExcludeFromCodeCoverage]
		public string WorkdayStartTime
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("workdaystarttime");
			}
			set
			{
				OnPropertyChanging("WorkdayStartTime");
				((Entity)this).SetAttributeValue("workdaystarttime", (object)value);
				OnPropertyChanged("WorkdayStartTime");
			}
		}

		[AttributeLogicalName("workdaystoptime")]
		[ExcludeFromCodeCoverage]
		public string WorkdayStopTime
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("workdaystoptime");
			}
			set
			{
				OnPropertyChanging("WorkdayStopTime");
				((Entity)this).SetAttributeValue("workdaystoptime", (object)value);
				OnPropertyChanged("WorkdayStopTime");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public UserSettings()
			: this("usersettings")
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
