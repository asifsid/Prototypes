using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Microsoft.Dynamics.Solution.Common.Proxies
{
	[DataContract]
	[EntityLogicalName("account")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Account : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "account";

		public const int EntityTypeCode = 1;

		public const int AttributeEMailAddress3_MaxLength = 100;

		public const int AttributeEMailAddress2_MaxLength = 100;

		public const int AttributeEMailAddress1_MaxLength = 100;

		public const int AttributeAddress1_City_MaxLength = 80;

		public const int AttributeWebSiteURL_MaxLength = 200;

		public const int AttributeYomiName_MaxLength = 160;

		public const int AttributeAddress2_StateOrProvince_MaxLength = 50;

		public const int AttributeAddress2_Country_MaxLength = 80;

		public const int AttributeAddress2_Line2_MaxLength = 250;

		public const int AttributeAddress1_Line3_MaxLength = 250;

		public const int AttributeAddress2_County_MaxLength = 50;

		public const int AttributeTelephone3_MaxLength = 50;

		public const int AttributeAddress2_City_MaxLength = 80;

		public const int AttributeAddress2_PostalCode_MaxLength = 20;

		public const int AttributeEntityImage_URL_MaxLength = 200;

		public const int AttributeAddress2_Line3_MaxLength = 250;

		public const int AttributeAddress1_County_MaxLength = 50;

		public const int AttributeAddress1_Line1_MaxLength = 250;

		public const int AttributeAddress2_PostOfficeBox_MaxLength = 20;

		public const int AttributeAddress2_Telephone1_MaxLength = 50;

		public const int AttributeAddress2_Telephone2_MaxLength = 50;

		public const int AttributeAddress2_Telephone3_MaxLength = 50;

		public const int AttributeTraversedPath_MaxLength = 1250;

		public const int AttributeAddress1_Telephone2_MaxLength = 50;

		public const int AttributeAddress2_Name_MaxLength = 200;

		public const int AttributePrimarySatoriId_MaxLength = 200;

		public const int AttributeName_MaxLength = 160;

		public const int AttributeTimeSpentByMeOnEmailAndMeetings_MaxLength = 1250;

		public const int AttributePrimaryTwitterId_MaxLength = 20;

		public const int AttributeAddress1_Country_MaxLength = 80;

		public const int AttributeAddress1_StateOrProvince_MaxLength = 50;

		public const int AttributeAddress2_Line1_MaxLength = 250;

		public const int AttributeAddress1_Telephone1_MaxLength = 50;

		public const int AttributeAddress1_Telephone3_MaxLength = 50;

		public const int AttributeAddress1_PostOfficeBox_MaxLength = 20;

		public const int AttributeFax_MaxLength = 50;

		public const int AttributeSIC_MaxLength = 20;

		public const int AttributeAccountNumber_MaxLength = 20;

		public const int AttributeAddress2_Fax_MaxLength = 50;

		public const int AttributeFtpSiteURL_MaxLength = 200;

		public const int AttributeAddress1_PrimaryContactName_MaxLength = 100;

		public const int AttributeAddress1_Line2_MaxLength = 250;

		public const int AttributeAddress2_UPSZone_MaxLength = 4;

		public const int AttributeAddress1_PostalCode_MaxLength = 20;

		public const int AttributeTickerSymbol_MaxLength = 10;

		public const int AttributeStockExchange_MaxLength = 20;

		public const int AttributeTelephone2_MaxLength = 50;

		public const int AttributeTelephone1_MaxLength = 50;

		public const int AttributeAddress1_Name_MaxLength = 200;

		public const int AttributeAddress1_Fax_MaxLength = 50;

		public const int AttributeAddress1_UPSZone_MaxLength = 4;

		public const int AttributeAddress2_PrimaryContactName_MaxLength = 100;

		public const string AttributeAccountCategoryCode = "accountcategorycode";

		public const string AttributeAccountClassificationCode = "accountclassificationcode";

		public const string AttributeAccountId = "accountid";

		public const string AttributeId = "accountid";

		public const string AttributeAccountNumber = "accountnumber";

		public const string AttributeAccountRatingCode = "accountratingcode";

		public const string AttributeAddress1_AddressId = "address1_addressid";

		public const string AttributeAddress1_AddressTypeCode = "address1_addresstypecode";

		public const string AttributeAddress1_City = "address1_city";

		public const string AttributeAddress1_Composite = "address1_composite";

		public const string AttributeAddress1_Country = "address1_country";

		public const string AttributeAddress1_County = "address1_county";

		public const string AttributeAddress1_Fax = "address1_fax";

		public const string AttributeAddress1_FreightTermsCode = "address1_freighttermscode";

		public const string AttributeAddress1_Latitude = "address1_latitude";

		public const string AttributeAddress1_Line1 = "address1_line1";

		public const string AttributeAddress1_Line2 = "address1_line2";

		public const string AttributeAddress1_Line3 = "address1_line3";

		public const string AttributeAddress1_Longitude = "address1_longitude";

		public const string AttributeAddress1_Name = "address1_name";

		public const string AttributeAddress1_PostalCode = "address1_postalcode";

		public const string AttributeAddress1_PostOfficeBox = "address1_postofficebox";

		public const string AttributeAddress1_PrimaryContactName = "address1_primarycontactname";

		public const string AttributeAddress1_ShippingMethodCode = "address1_shippingmethodcode";

		public const string AttributeAddress1_StateOrProvince = "address1_stateorprovince";

		public const string AttributeAddress1_Telephone1 = "address1_telephone1";

		public const string AttributeAddress1_Telephone2 = "address1_telephone2";

		public const string AttributeAddress1_Telephone3 = "address1_telephone3";

		public const string AttributeAddress1_UPSZone = "address1_upszone";

		public const string AttributeAddress1_UTCOffset = "address1_utcoffset";

		public const string AttributeAddress2_AddressId = "address2_addressid";

		public const string AttributeAddress2_AddressTypeCode = "address2_addresstypecode";

		public const string AttributeAddress2_City = "address2_city";

		public const string AttributeAddress2_Composite = "address2_composite";

		public const string AttributeAddress2_Country = "address2_country";

		public const string AttributeAddress2_County = "address2_county";

		public const string AttributeAddress2_Fax = "address2_fax";

		public const string AttributeAddress2_FreightTermsCode = "address2_freighttermscode";

		public const string AttributeAddress2_Latitude = "address2_latitude";

		public const string AttributeAddress2_Line1 = "address2_line1";

		public const string AttributeAddress2_Line2 = "address2_line2";

		public const string AttributeAddress2_Line3 = "address2_line3";

		public const string AttributeAddress2_Longitude = "address2_longitude";

		public const string AttributeAddress2_Name = "address2_name";

		public const string AttributeAddress2_PostalCode = "address2_postalcode";

		public const string AttributeAddress2_PostOfficeBox = "address2_postofficebox";

		public const string AttributeAddress2_PrimaryContactName = "address2_primarycontactname";

		public const string AttributeAddress2_ShippingMethodCode = "address2_shippingmethodcode";

		public const string AttributeAddress2_StateOrProvince = "address2_stateorprovince";

		public const string AttributeAddress2_Telephone1 = "address2_telephone1";

		public const string AttributeAddress2_Telephone2 = "address2_telephone2";

		public const string AttributeAddress2_Telephone3 = "address2_telephone3";

		public const string AttributeAddress2_UPSZone = "address2_upszone";

		public const string AttributeAddress2_UTCOffset = "address2_utcoffset";

		public const string AttributeAging30 = "aging30";

		public const string AttributeAging30_Base = "aging30_base";

		public const string AttributeAging60 = "aging60";

		public const string AttributeAging60_Base = "aging60_base";

		public const string AttributeAging90 = "aging90";

		public const string AttributeAging90_Base = "aging90_base";

		public const string AttributeBusinessTypeCode = "businesstypecode";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedByExternalParty = "createdbyexternalparty";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeCreditLimit = "creditlimit";

		public const string AttributeCreditLimit_Base = "creditlimit_base";

		public const string AttributeCreditOnHold = "creditonhold";

		public const string AttributeCustomerSizeCode = "customersizecode";

		public const string AttributeCustomerTypeCode = "customertypecode";

		public const string AttributeDefaultPriceLevelId = "defaultpricelevelid";

		public const string AttributeDescription = "description";

		public const string AttributeDoNotBulkEMail = "donotbulkemail";

		public const string AttributeDoNotBulkPostalMail = "donotbulkpostalmail";

		public const string AttributeDoNotEMail = "donotemail";

		public const string AttributeDoNotFax = "donotfax";

		public const string AttributeDoNotPhone = "donotphone";

		public const string AttributeDoNotPostalMail = "donotpostalmail";

		public const string AttributeDoNotSendMM = "donotsendmm";

		public const string AttributeEMailAddress1 = "emailaddress1";

		public const string AttributeEMailAddress2 = "emailaddress2";

		public const string AttributeEMailAddress3 = "emailaddress3";

		public const string AttributeEntityImage = "entityimage";

		public const string AttributeEntityImage_Timestamp = "entityimage_timestamp";

		public const string AttributeEntityImage_URL = "entityimage_url";

		public const string AttributeEntityImageId = "entityimageid";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeFax = "fax";

		public const string AttributeFollowEmail = "followemail";

		public const string AttributeFtpSiteURL = "ftpsiteurl";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIndustryCode = "industrycode";

		public const string AttributeLastOnHoldTime = "lastonholdtime";

		public const string AttributeLastUsedInCampaign = "lastusedincampaign";

		public const string AttributeMarketCap = "marketcap";

		public const string AttributeMarketCap_Base = "marketcap_base";

		public const string AttributeMarketingOnly = "marketingonly";

		public const string AttributeMasterId = "masterid";

		public const string AttributeMerged = "merged";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedByExternalParty = "modifiedbyexternalparty";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeNumberOfEmployees = "numberofemployees";

		public const string AttributeOnHoldTime = "onholdtime";

		public const string AttributeOpenDeals = "opendeals";

		public const string AttributeOpenDeals_Date = "opendeals_date";

		public const string AttributeOpenDeals_State = "opendeals_state";

		public const string AttributeOpenRevenue = "openrevenue";

		public const string AttributeOpenRevenue_Base = "openrevenue_base";

		public const string AttributeOpenRevenue_Date = "openrevenue_date";

		public const string AttributeOpenRevenue_State = "openrevenue_state";

		public const string AttributeOriginatingLeadId = "originatingleadid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeOwnerId = "ownerid";

		public const string AttributeOwnershipCode = "ownershipcode";

		public const string AttributeOwningBusinessUnit = "owningbusinessunit";

		public const string AttributeOwningTeam = "owningteam";

		public const string AttributeOwningUser = "owninguser";

		public const string AttributeParentAccountId = "parentaccountid";

		public const string AttributeParticipatesInWorkflow = "participatesinworkflow";

		public const string AttributePaymentTermsCode = "paymenttermscode";

		public const string AttributePreferredAppointmentDayCode = "preferredappointmentdaycode";

		public const string AttributePreferredAppointmentTimeCode = "preferredappointmenttimecode";

		public const string AttributePreferredContactMethodCode = "preferredcontactmethodcode";

		public const string AttributePreferredSystemUserId = "preferredsystemuserid";

		public const string AttributePrimaryContactId = "primarycontactid";

		public const string AttributePrimarySatoriId = "primarysatoriid";

		public const string AttributePrimaryTwitterId = "primarytwitterid";

		public const string AttributeProcessId = "processid";

		public const string AttributeRevenue = "revenue";

		public const string AttributeRevenue_Base = "revenue_base";

		public const string AttributeSharesOutstanding = "sharesoutstanding";

		public const string AttributeShippingMethodCode = "shippingmethodcode";

		public const string AttributeSIC = "sic";

		public const string AttributeSLAId = "slaid";

		public const string AttributeSLAInvokedId = "slainvokedid";

		public const string AttributeStageId = "stageid";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeStockExchange = "stockexchange";

		public const string AttributeTelephone1 = "telephone1";

		public const string AttributeTelephone2 = "telephone2";

		public const string AttributeTelephone3 = "telephone3";

		public const string AttributeTerritoryCode = "territorycode";

		public const string AttributeTerritoryId = "territoryid";

		public const string AttributeTickerSymbol = "tickersymbol";

		public const string AttributeTimeSpentByMeOnEmailAndMeetings = "timespentbymeonemailandmeetings";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeTraversedPath = "traversedpath";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeWebSiteURL = "websiteurl";

		public const string AttributeYomiName = "yominame";

		public const string AttributeReferencingaccount_master_account = "masterid";

		public const string AttributeReferencingaccount_parent_account = "parentaccountid";

		public const string AttributeAccount_primary_contact = "primarycontactid";

		[AttributeLogicalName("accountcategorycode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue AccountCategoryCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("accountcategorycode");
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
				OnPropertyChanging("AccountCategoryCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("accountcategorycode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("accountcategorycode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("AccountCategoryCode");
			}
		}

		[AttributeLogicalName("accountclassificationcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue AccountClassificationCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("accountclassificationcode");
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
				OnPropertyChanging("AccountClassificationCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("accountclassificationcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("accountclassificationcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("AccountClassificationCode");
			}
		}

		[AttributeLogicalName("accountid")]
		[ExcludeFromCodeCoverage]
		public Guid? AccountId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("accountid");
			}
			set
			{
				OnPropertyChanging("AccountId");
				((Entity)this).SetAttributeValue("accountid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("AccountId");
			}
		}

		[AttributeLogicalName("accountid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				AccountId = value;
			}
		}

		[AttributeLogicalName("accountnumber")]
		[ExcludeFromCodeCoverage]
		public string AccountNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("accountnumber");
			}
			set
			{
				OnPropertyChanging("AccountNumber");
				((Entity)this).SetAttributeValue("accountnumber", (object)value);
				OnPropertyChanged("AccountNumber");
			}
		}

		[AttributeLogicalName("accountratingcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue AccountRatingCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("accountratingcode");
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
				OnPropertyChanging("AccountRatingCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("accountratingcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("accountratingcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("AccountRatingCode");
			}
		}

		[AttributeLogicalName("address1_addressid")]
		[ExcludeFromCodeCoverage]
		public Guid? Address1_AddressId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("address1_addressid");
			}
			set
			{
				OnPropertyChanging("Address1_AddressId");
				((Entity)this).SetAttributeValue("address1_addressid", (object)value);
				OnPropertyChanged("Address1_AddressId");
			}
		}

		[AttributeLogicalName("address1_addresstypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address1_AddressTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address1_addresstypecode");
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
				OnPropertyChanging("Address1_AddressTypeCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address1_addresstypecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address1_addresstypecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address1_AddressTypeCode");
			}
		}

		[AttributeLogicalName("address1_city")]
		[ExcludeFromCodeCoverage]
		public string Address1_City
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_city");
			}
			set
			{
				OnPropertyChanging("Address1_City");
				((Entity)this).SetAttributeValue("address1_city", (object)value);
				OnPropertyChanged("Address1_City");
			}
		}

		[AttributeLogicalName("address1_composite")]
		[ExcludeFromCodeCoverage]
		public string Address1_Composite => ((Entity)this).GetAttributeValue<string>("address1_composite");

		[AttributeLogicalName("address1_country")]
		[ExcludeFromCodeCoverage]
		public string Address1_Country
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_country");
			}
			set
			{
				OnPropertyChanging("Address1_Country");
				((Entity)this).SetAttributeValue("address1_country", (object)value);
				OnPropertyChanged("Address1_Country");
			}
		}

		[AttributeLogicalName("address1_county")]
		[ExcludeFromCodeCoverage]
		public string Address1_County
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_county");
			}
			set
			{
				OnPropertyChanging("Address1_County");
				((Entity)this).SetAttributeValue("address1_county", (object)value);
				OnPropertyChanged("Address1_County");
			}
		}

		[AttributeLogicalName("address1_fax")]
		[ExcludeFromCodeCoverage]
		public string Address1_Fax
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_fax");
			}
			set
			{
				OnPropertyChanging("Address1_Fax");
				((Entity)this).SetAttributeValue("address1_fax", (object)value);
				OnPropertyChanged("Address1_Fax");
			}
		}

		[AttributeLogicalName("address1_freighttermscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address1_FreightTermsCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address1_freighttermscode");
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
				OnPropertyChanging("Address1_FreightTermsCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address1_freighttermscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address1_freighttermscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address1_FreightTermsCode");
			}
		}

		[AttributeLogicalName("address1_latitude")]
		[ExcludeFromCodeCoverage]
		public double? Address1_Latitude
		{
			get
			{
				return ((Entity)this).GetAttributeValue<double?>("address1_latitude");
			}
			set
			{
				OnPropertyChanging("Address1_Latitude");
				((Entity)this).SetAttributeValue("address1_latitude", (object)value);
				OnPropertyChanged("Address1_Latitude");
			}
		}

		[AttributeLogicalName("address1_line1")]
		[ExcludeFromCodeCoverage]
		public string Address1_Line1
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_line1");
			}
			set
			{
				OnPropertyChanging("Address1_Line1");
				((Entity)this).SetAttributeValue("address1_line1", (object)value);
				OnPropertyChanged("Address1_Line1");
			}
		}

		[AttributeLogicalName("address1_line2")]
		[ExcludeFromCodeCoverage]
		public string Address1_Line2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_line2");
			}
			set
			{
				OnPropertyChanging("Address1_Line2");
				((Entity)this).SetAttributeValue("address1_line2", (object)value);
				OnPropertyChanged("Address1_Line2");
			}
		}

		[AttributeLogicalName("address1_line3")]
		[ExcludeFromCodeCoverage]
		public string Address1_Line3
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_line3");
			}
			set
			{
				OnPropertyChanging("Address1_Line3");
				((Entity)this).SetAttributeValue("address1_line3", (object)value);
				OnPropertyChanged("Address1_Line3");
			}
		}

		[AttributeLogicalName("address1_longitude")]
		[ExcludeFromCodeCoverage]
		public double? Address1_Longitude
		{
			get
			{
				return ((Entity)this).GetAttributeValue<double?>("address1_longitude");
			}
			set
			{
				OnPropertyChanging("Address1_Longitude");
				((Entity)this).SetAttributeValue("address1_longitude", (object)value);
				OnPropertyChanged("Address1_Longitude");
			}
		}

		[AttributeLogicalName("address1_name")]
		[ExcludeFromCodeCoverage]
		public string Address1_Name
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_name");
			}
			set
			{
				OnPropertyChanging("Address1_Name");
				((Entity)this).SetAttributeValue("address1_name", (object)value);
				OnPropertyChanged("Address1_Name");
			}
		}

		[AttributeLogicalName("address1_postalcode")]
		[ExcludeFromCodeCoverage]
		public string Address1_PostalCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_postalcode");
			}
			set
			{
				OnPropertyChanging("Address1_PostalCode");
				((Entity)this).SetAttributeValue("address1_postalcode", (object)value);
				OnPropertyChanged("Address1_PostalCode");
			}
		}

		[AttributeLogicalName("address1_postofficebox")]
		[ExcludeFromCodeCoverage]
		public string Address1_PostOfficeBox
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_postofficebox");
			}
			set
			{
				OnPropertyChanging("Address1_PostOfficeBox");
				((Entity)this).SetAttributeValue("address1_postofficebox", (object)value);
				OnPropertyChanged("Address1_PostOfficeBox");
			}
		}

		[AttributeLogicalName("address1_primarycontactname")]
		[ExcludeFromCodeCoverage]
		public string Address1_PrimaryContactName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_primarycontactname");
			}
			set
			{
				OnPropertyChanging("Address1_PrimaryContactName");
				((Entity)this).SetAttributeValue("address1_primarycontactname", (object)value);
				OnPropertyChanged("Address1_PrimaryContactName");
			}
		}

		[AttributeLogicalName("address1_shippingmethodcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address1_ShippingMethodCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address1_shippingmethodcode");
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
				OnPropertyChanging("Address1_ShippingMethodCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address1_shippingmethodcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address1_shippingmethodcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address1_ShippingMethodCode");
			}
		}

		[AttributeLogicalName("address1_stateorprovince")]
		[ExcludeFromCodeCoverage]
		public string Address1_StateOrProvince
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_stateorprovince");
			}
			set
			{
				OnPropertyChanging("Address1_StateOrProvince");
				((Entity)this).SetAttributeValue("address1_stateorprovince", (object)value);
				OnPropertyChanged("Address1_StateOrProvince");
			}
		}

		[AttributeLogicalName("address1_telephone1")]
		[ExcludeFromCodeCoverage]
		public string Address1_Telephone1
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_telephone1");
			}
			set
			{
				OnPropertyChanging("Address1_Telephone1");
				((Entity)this).SetAttributeValue("address1_telephone1", (object)value);
				OnPropertyChanged("Address1_Telephone1");
			}
		}

		[AttributeLogicalName("address1_telephone2")]
		[ExcludeFromCodeCoverage]
		public string Address1_Telephone2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_telephone2");
			}
			set
			{
				OnPropertyChanging("Address1_Telephone2");
				((Entity)this).SetAttributeValue("address1_telephone2", (object)value);
				OnPropertyChanged("Address1_Telephone2");
			}
		}

		[AttributeLogicalName("address1_telephone3")]
		[ExcludeFromCodeCoverage]
		public string Address1_Telephone3
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_telephone3");
			}
			set
			{
				OnPropertyChanging("Address1_Telephone3");
				((Entity)this).SetAttributeValue("address1_telephone3", (object)value);
				OnPropertyChanged("Address1_Telephone3");
			}
		}

		[AttributeLogicalName("address1_upszone")]
		[ExcludeFromCodeCoverage]
		public string Address1_UPSZone
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address1_upszone");
			}
			set
			{
				OnPropertyChanging("Address1_UPSZone");
				((Entity)this).SetAttributeValue("address1_upszone", (object)value);
				OnPropertyChanged("Address1_UPSZone");
			}
		}

		[AttributeLogicalName("address1_utcoffset")]
		[ExcludeFromCodeCoverage]
		public int? Address1_UTCOffset
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("address1_utcoffset");
			}
			set
			{
				OnPropertyChanging("Address1_UTCOffset");
				((Entity)this).SetAttributeValue("address1_utcoffset", (object)value);
				OnPropertyChanged("Address1_UTCOffset");
			}
		}

		[AttributeLogicalName("address2_addressid")]
		[ExcludeFromCodeCoverage]
		public Guid? Address2_AddressId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("address2_addressid");
			}
			set
			{
				OnPropertyChanging("Address2_AddressId");
				((Entity)this).SetAttributeValue("address2_addressid", (object)value);
				OnPropertyChanged("Address2_AddressId");
			}
		}

		[AttributeLogicalName("address2_addresstypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address2_AddressTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address2_addresstypecode");
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
				OnPropertyChanging("Address2_AddressTypeCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address2_addresstypecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address2_addresstypecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address2_AddressTypeCode");
			}
		}

		[AttributeLogicalName("address2_city")]
		[ExcludeFromCodeCoverage]
		public string Address2_City
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_city");
			}
			set
			{
				OnPropertyChanging("Address2_City");
				((Entity)this).SetAttributeValue("address2_city", (object)value);
				OnPropertyChanged("Address2_City");
			}
		}

		[AttributeLogicalName("address2_composite")]
		[ExcludeFromCodeCoverage]
		public string Address2_Composite => ((Entity)this).GetAttributeValue<string>("address2_composite");

		[AttributeLogicalName("address2_country")]
		[ExcludeFromCodeCoverage]
		public string Address2_Country
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_country");
			}
			set
			{
				OnPropertyChanging("Address2_Country");
				((Entity)this).SetAttributeValue("address2_country", (object)value);
				OnPropertyChanged("Address2_Country");
			}
		}

		[AttributeLogicalName("address2_county")]
		[ExcludeFromCodeCoverage]
		public string Address2_County
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_county");
			}
			set
			{
				OnPropertyChanging("Address2_County");
				((Entity)this).SetAttributeValue("address2_county", (object)value);
				OnPropertyChanged("Address2_County");
			}
		}

		[AttributeLogicalName("address2_fax")]
		[ExcludeFromCodeCoverage]
		public string Address2_Fax
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_fax");
			}
			set
			{
				OnPropertyChanging("Address2_Fax");
				((Entity)this).SetAttributeValue("address2_fax", (object)value);
				OnPropertyChanged("Address2_Fax");
			}
		}

		[AttributeLogicalName("address2_freighttermscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address2_FreightTermsCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address2_freighttermscode");
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
				OnPropertyChanging("Address2_FreightTermsCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address2_freighttermscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address2_freighttermscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address2_FreightTermsCode");
			}
		}

		[AttributeLogicalName("address2_latitude")]
		[ExcludeFromCodeCoverage]
		public double? Address2_Latitude
		{
			get
			{
				return ((Entity)this).GetAttributeValue<double?>("address2_latitude");
			}
			set
			{
				OnPropertyChanging("Address2_Latitude");
				((Entity)this).SetAttributeValue("address2_latitude", (object)value);
				OnPropertyChanged("Address2_Latitude");
			}
		}

		[AttributeLogicalName("address2_line1")]
		[ExcludeFromCodeCoverage]
		public string Address2_Line1
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_line1");
			}
			set
			{
				OnPropertyChanging("Address2_Line1");
				((Entity)this).SetAttributeValue("address2_line1", (object)value);
				OnPropertyChanged("Address2_Line1");
			}
		}

		[AttributeLogicalName("address2_line2")]
		[ExcludeFromCodeCoverage]
		public string Address2_Line2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_line2");
			}
			set
			{
				OnPropertyChanging("Address2_Line2");
				((Entity)this).SetAttributeValue("address2_line2", (object)value);
				OnPropertyChanged("Address2_Line2");
			}
		}

		[AttributeLogicalName("address2_line3")]
		[ExcludeFromCodeCoverage]
		public string Address2_Line3
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_line3");
			}
			set
			{
				OnPropertyChanging("Address2_Line3");
				((Entity)this).SetAttributeValue("address2_line3", (object)value);
				OnPropertyChanged("Address2_Line3");
			}
		}

		[AttributeLogicalName("address2_longitude")]
		[ExcludeFromCodeCoverage]
		public double? Address2_Longitude
		{
			get
			{
				return ((Entity)this).GetAttributeValue<double?>("address2_longitude");
			}
			set
			{
				OnPropertyChanging("Address2_Longitude");
				((Entity)this).SetAttributeValue("address2_longitude", (object)value);
				OnPropertyChanged("Address2_Longitude");
			}
		}

		[AttributeLogicalName("address2_name")]
		[ExcludeFromCodeCoverage]
		public string Address2_Name
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_name");
			}
			set
			{
				OnPropertyChanging("Address2_Name");
				((Entity)this).SetAttributeValue("address2_name", (object)value);
				OnPropertyChanged("Address2_Name");
			}
		}

		[AttributeLogicalName("address2_postalcode")]
		[ExcludeFromCodeCoverage]
		public string Address2_PostalCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_postalcode");
			}
			set
			{
				OnPropertyChanging("Address2_PostalCode");
				((Entity)this).SetAttributeValue("address2_postalcode", (object)value);
				OnPropertyChanged("Address2_PostalCode");
			}
		}

		[AttributeLogicalName("address2_postofficebox")]
		[ExcludeFromCodeCoverage]
		public string Address2_PostOfficeBox
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_postofficebox");
			}
			set
			{
				OnPropertyChanging("Address2_PostOfficeBox");
				((Entity)this).SetAttributeValue("address2_postofficebox", (object)value);
				OnPropertyChanged("Address2_PostOfficeBox");
			}
		}

		[AttributeLogicalName("address2_primarycontactname")]
		[ExcludeFromCodeCoverage]
		public string Address2_PrimaryContactName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_primarycontactname");
			}
			set
			{
				OnPropertyChanging("Address2_PrimaryContactName");
				((Entity)this).SetAttributeValue("address2_primarycontactname", (object)value);
				OnPropertyChanged("Address2_PrimaryContactName");
			}
		}

		[AttributeLogicalName("address2_shippingmethodcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address2_ShippingMethodCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address2_shippingmethodcode");
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
				OnPropertyChanging("Address2_ShippingMethodCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address2_shippingmethodcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address2_shippingmethodcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address2_ShippingMethodCode");
			}
		}

		[AttributeLogicalName("address2_stateorprovince")]
		[ExcludeFromCodeCoverage]
		public string Address2_StateOrProvince
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_stateorprovince");
			}
			set
			{
				OnPropertyChanging("Address2_StateOrProvince");
				((Entity)this).SetAttributeValue("address2_stateorprovince", (object)value);
				OnPropertyChanged("Address2_StateOrProvince");
			}
		}

		[AttributeLogicalName("address2_telephone1")]
		[ExcludeFromCodeCoverage]
		public string Address2_Telephone1
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_telephone1");
			}
			set
			{
				OnPropertyChanging("Address2_Telephone1");
				((Entity)this).SetAttributeValue("address2_telephone1", (object)value);
				OnPropertyChanged("Address2_Telephone1");
			}
		}

		[AttributeLogicalName("address2_telephone2")]
		[ExcludeFromCodeCoverage]
		public string Address2_Telephone2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_telephone2");
			}
			set
			{
				OnPropertyChanging("Address2_Telephone2");
				((Entity)this).SetAttributeValue("address2_telephone2", (object)value);
				OnPropertyChanged("Address2_Telephone2");
			}
		}

		[AttributeLogicalName("address2_telephone3")]
		[ExcludeFromCodeCoverage]
		public string Address2_Telephone3
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_telephone3");
			}
			set
			{
				OnPropertyChanging("Address2_Telephone3");
				((Entity)this).SetAttributeValue("address2_telephone3", (object)value);
				OnPropertyChanged("Address2_Telephone3");
			}
		}

		[AttributeLogicalName("address2_upszone")]
		[ExcludeFromCodeCoverage]
		public string Address2_UPSZone
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address2_upszone");
			}
			set
			{
				OnPropertyChanging("Address2_UPSZone");
				((Entity)this).SetAttributeValue("address2_upszone", (object)value);
				OnPropertyChanged("Address2_UPSZone");
			}
		}

		[AttributeLogicalName("address2_utcoffset")]
		[ExcludeFromCodeCoverage]
		public int? Address2_UTCOffset
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("address2_utcoffset");
			}
			set
			{
				OnPropertyChanging("Address2_UTCOffset");
				((Entity)this).SetAttributeValue("address2_utcoffset", (object)value);
				OnPropertyChanged("Address2_UTCOffset");
			}
		}

		[AttributeLogicalName("aging30")]
		[ExcludeFromCodeCoverage]
		public Money Aging30 => ((Entity)this).GetAttributeValue<Money>("aging30");

		[AttributeLogicalName("aging30_base")]
		[ExcludeFromCodeCoverage]
		public Money Aging30_Base => ((Entity)this).GetAttributeValue<Money>("aging30_base");

		[AttributeLogicalName("aging60")]
		[ExcludeFromCodeCoverage]
		public Money Aging60 => ((Entity)this).GetAttributeValue<Money>("aging60");

		[AttributeLogicalName("aging60_base")]
		[ExcludeFromCodeCoverage]
		public Money Aging60_Base => ((Entity)this).GetAttributeValue<Money>("aging60_base");

		[AttributeLogicalName("aging90")]
		[ExcludeFromCodeCoverage]
		public Money Aging90 => ((Entity)this).GetAttributeValue<Money>("aging90");

		[AttributeLogicalName("aging90_base")]
		[ExcludeFromCodeCoverage]
		public Money Aging90_Base => ((Entity)this).GetAttributeValue<Money>("aging90_base");

		[AttributeLogicalName("businesstypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue BusinessTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("businesstypecode");
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
				OnPropertyChanging("BusinessTypeCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("businesstypecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("businesstypecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("BusinessTypeCode");
			}
		}

		[AttributeLogicalName("createdby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedBy => ((Entity)this).GetAttributeValue<EntityReference>("createdby");

		[AttributeLogicalName("createdbyexternalparty")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedByExternalParty => ((Entity)this).GetAttributeValue<EntityReference>("createdbyexternalparty");

		[AttributeLogicalName("createdon")]
		[ExcludeFromCodeCoverage]
		public DateTime? CreatedOn => ((Entity)this).GetAttributeValue<DateTime?>("createdon");

		[AttributeLogicalName("createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("createdonbehalfby");

		[AttributeLogicalName("creditlimit")]
		[ExcludeFromCodeCoverage]
		public Money CreditLimit
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Money>("creditlimit");
			}
			set
			{
				OnPropertyChanging("CreditLimit");
				((Entity)this).SetAttributeValue("creditlimit", (object)value);
				OnPropertyChanged("CreditLimit");
			}
		}

		[AttributeLogicalName("creditlimit_base")]
		[ExcludeFromCodeCoverage]
		public Money CreditLimit_Base => ((Entity)this).GetAttributeValue<Money>("creditlimit_base");

		[AttributeLogicalName("creditonhold")]
		[ExcludeFromCodeCoverage]
		public bool? CreditOnHold
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("creditonhold");
			}
			set
			{
				OnPropertyChanging("CreditOnHold");
				((Entity)this).SetAttributeValue("creditonhold", (object)value);
				OnPropertyChanged("CreditOnHold");
			}
		}

		[AttributeLogicalName("customersizecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue CustomerSizeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("customersizecode");
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
				OnPropertyChanging("CustomerSizeCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("customersizecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("customersizecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("CustomerSizeCode");
			}
		}

		[AttributeLogicalName("customertypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue CustomerTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("customertypecode");
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
				OnPropertyChanging("CustomerTypeCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("customertypecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("customertypecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("CustomerTypeCode");
			}
		}

		[AttributeLogicalName("defaultpricelevelid")]
		[ExcludeFromCodeCoverage]
		public EntityReference DefaultPriceLevelId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("defaultpricelevelid");
			}
			set
			{
				OnPropertyChanging("DefaultPriceLevelId");
				((Entity)this).SetAttributeValue("defaultpricelevelid", (object)value);
				OnPropertyChanged("DefaultPriceLevelId");
			}
		}

		[AttributeLogicalName("description")]
		[ExcludeFromCodeCoverage]
		public string Description
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("description");
			}
			set
			{
				OnPropertyChanging("Description");
				((Entity)this).SetAttributeValue("description", (object)value);
				OnPropertyChanged("Description");
			}
		}

		[AttributeLogicalName("donotbulkemail")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotBulkEMail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("donotbulkemail");
			}
			set
			{
				OnPropertyChanging("DoNotBulkEMail");
				((Entity)this).SetAttributeValue("donotbulkemail", (object)value);
				OnPropertyChanged("DoNotBulkEMail");
			}
		}

		[AttributeLogicalName("donotbulkpostalmail")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotBulkPostalMail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("donotbulkpostalmail");
			}
			set
			{
				OnPropertyChanging("DoNotBulkPostalMail");
				((Entity)this).SetAttributeValue("donotbulkpostalmail", (object)value);
				OnPropertyChanged("DoNotBulkPostalMail");
			}
		}

		[AttributeLogicalName("donotemail")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotEMail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("donotemail");
			}
			set
			{
				OnPropertyChanging("DoNotEMail");
				((Entity)this).SetAttributeValue("donotemail", (object)value);
				OnPropertyChanged("DoNotEMail");
			}
		}

		[AttributeLogicalName("donotfax")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotFax
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("donotfax");
			}
			set
			{
				OnPropertyChanging("DoNotFax");
				((Entity)this).SetAttributeValue("donotfax", (object)value);
				OnPropertyChanged("DoNotFax");
			}
		}

		[AttributeLogicalName("donotphone")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotPhone
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("donotphone");
			}
			set
			{
				OnPropertyChanging("DoNotPhone");
				((Entity)this).SetAttributeValue("donotphone", (object)value);
				OnPropertyChanged("DoNotPhone");
			}
		}

		[AttributeLogicalName("donotpostalmail")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotPostalMail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("donotpostalmail");
			}
			set
			{
				OnPropertyChanging("DoNotPostalMail");
				((Entity)this).SetAttributeValue("donotpostalmail", (object)value);
				OnPropertyChanged("DoNotPostalMail");
			}
		}

		[AttributeLogicalName("donotsendmm")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotSendMM
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("donotsendmm");
			}
			set
			{
				OnPropertyChanging("DoNotSendMM");
				((Entity)this).SetAttributeValue("donotsendmm", (object)value);
				OnPropertyChanged("DoNotSendMM");
			}
		}

		[AttributeLogicalName("emailaddress1")]
		[ExcludeFromCodeCoverage]
		public string EMailAddress1
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("emailaddress1");
			}
			set
			{
				OnPropertyChanging("EMailAddress1");
				((Entity)this).SetAttributeValue("emailaddress1", (object)value);
				OnPropertyChanged("EMailAddress1");
			}
		}

		[AttributeLogicalName("emailaddress2")]
		[ExcludeFromCodeCoverage]
		public string EMailAddress2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("emailaddress2");
			}
			set
			{
				OnPropertyChanging("EMailAddress2");
				((Entity)this).SetAttributeValue("emailaddress2", (object)value);
				OnPropertyChanged("EMailAddress2");
			}
		}

		[AttributeLogicalName("emailaddress3")]
		[ExcludeFromCodeCoverage]
		public string EMailAddress3
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("emailaddress3");
			}
			set
			{
				OnPropertyChanging("EMailAddress3");
				((Entity)this).SetAttributeValue("emailaddress3", (object)value);
				OnPropertyChanged("EMailAddress3");
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

		[AttributeLogicalName("exchangerate")]
		[ExcludeFromCodeCoverage]
		public decimal? ExchangeRate => ((Entity)this).GetAttributeValue<decimal?>("exchangerate");

		[AttributeLogicalName("fax")]
		[ExcludeFromCodeCoverage]
		public string Fax
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("fax");
			}
			set
			{
				OnPropertyChanging("Fax");
				((Entity)this).SetAttributeValue("fax", (object)value);
				OnPropertyChanged("Fax");
			}
		}

		[AttributeLogicalName("followemail")]
		[ExcludeFromCodeCoverage]
		public bool? FollowEmail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("followemail");
			}
			set
			{
				OnPropertyChanging("FollowEmail");
				((Entity)this).SetAttributeValue("followemail", (object)value);
				OnPropertyChanged("FollowEmail");
			}
		}

		[AttributeLogicalName("ftpsiteurl")]
		[ExcludeFromCodeCoverage]
		public string FtpSiteURL
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("ftpsiteurl");
			}
			set
			{
				OnPropertyChanging("FtpSiteURL");
				((Entity)this).SetAttributeValue("ftpsiteurl", (object)value);
				OnPropertyChanged("FtpSiteURL");
			}
		}

		[AttributeLogicalName("importsequencenumber")]
		[ExcludeFromCodeCoverage]
		public int? ImportSequenceNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("importsequencenumber");
			}
			set
			{
				OnPropertyChanging("ImportSequenceNumber");
				((Entity)this).SetAttributeValue("importsequencenumber", (object)value);
				OnPropertyChanged("ImportSequenceNumber");
			}
		}

		[AttributeLogicalName("industrycode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue IndustryCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("industrycode");
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
				OnPropertyChanging("IndustryCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("industrycode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("industrycode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("IndustryCode");
			}
		}

		[AttributeLogicalName("lastonholdtime")]
		[ExcludeFromCodeCoverage]
		public DateTime? LastOnHoldTime
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("lastonholdtime");
			}
			set
			{
				OnPropertyChanging("LastOnHoldTime");
				((Entity)this).SetAttributeValue("lastonholdtime", (object)value);
				OnPropertyChanged("LastOnHoldTime");
			}
		}

		[AttributeLogicalName("lastusedincampaign")]
		[ExcludeFromCodeCoverage]
		public DateTime? LastUsedInCampaign
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("lastusedincampaign");
			}
			set
			{
				OnPropertyChanging("LastUsedInCampaign");
				((Entity)this).SetAttributeValue("lastusedincampaign", (object)value);
				OnPropertyChanged("LastUsedInCampaign");
			}
		}

		[AttributeLogicalName("marketcap")]
		[ExcludeFromCodeCoverage]
		public Money MarketCap
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Money>("marketcap");
			}
			set
			{
				OnPropertyChanging("MarketCap");
				((Entity)this).SetAttributeValue("marketcap", (object)value);
				OnPropertyChanged("MarketCap");
			}
		}

		[AttributeLogicalName("marketcap_base")]
		[ExcludeFromCodeCoverage]
		public Money MarketCap_Base => ((Entity)this).GetAttributeValue<Money>("marketcap_base");

		[AttributeLogicalName("marketingonly")]
		[ExcludeFromCodeCoverage]
		public bool? MarketingOnly
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("marketingonly");
			}
			set
			{
				OnPropertyChanging("MarketingOnly");
				((Entity)this).SetAttributeValue("marketingonly", (object)value);
				OnPropertyChanged("MarketingOnly");
			}
		}

		[AttributeLogicalName("masterid")]
		[ExcludeFromCodeCoverage]
		public EntityReference MasterId => ((Entity)this).GetAttributeValue<EntityReference>("masterid");

		[AttributeLogicalName("merged")]
		[ExcludeFromCodeCoverage]
		public bool? Merged => ((Entity)this).GetAttributeValue<bool?>("merged");

		[AttributeLogicalName("modifiedby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedby");

		[AttributeLogicalName("modifiedbyexternalparty")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedByExternalParty => ((Entity)this).GetAttributeValue<EntityReference>("modifiedbyexternalparty");

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

		[AttributeLogicalName("numberofemployees")]
		[ExcludeFromCodeCoverage]
		public int? NumberOfEmployees
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("numberofemployees");
			}
			set
			{
				OnPropertyChanging("NumberOfEmployees");
				((Entity)this).SetAttributeValue("numberofemployees", (object)value);
				OnPropertyChanged("NumberOfEmployees");
			}
		}

		[AttributeLogicalName("onholdtime")]
		[ExcludeFromCodeCoverage]
		public int? OnHoldTime => ((Entity)this).GetAttributeValue<int?>("onholdtime");

		[AttributeLogicalName("opendeals")]
		[ExcludeFromCodeCoverage]
		public int? OpenDeals => ((Entity)this).GetAttributeValue<int?>("opendeals");

		[AttributeLogicalName("opendeals_date")]
		[ExcludeFromCodeCoverage]
		public DateTime? OpenDeals_Date => ((Entity)this).GetAttributeValue<DateTime?>("opendeals_date");

		[AttributeLogicalName("opendeals_state")]
		[ExcludeFromCodeCoverage]
		public int? OpenDeals_State => ((Entity)this).GetAttributeValue<int?>("opendeals_state");

		[AttributeLogicalName("openrevenue")]
		[ExcludeFromCodeCoverage]
		public Money OpenRevenue => ((Entity)this).GetAttributeValue<Money>("openrevenue");

		[AttributeLogicalName("openrevenue_base")]
		[ExcludeFromCodeCoverage]
		public Money OpenRevenue_Base => ((Entity)this).GetAttributeValue<Money>("openrevenue_base");

		[AttributeLogicalName("openrevenue_date")]
		[ExcludeFromCodeCoverage]
		public DateTime? OpenRevenue_Date => ((Entity)this).GetAttributeValue<DateTime?>("openrevenue_date");

		[AttributeLogicalName("openrevenue_state")]
		[ExcludeFromCodeCoverage]
		public int? OpenRevenue_State => ((Entity)this).GetAttributeValue<int?>("openrevenue_state");

		[AttributeLogicalName("originatingleadid")]
		[ExcludeFromCodeCoverage]
		public EntityReference OriginatingLeadId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("originatingleadid");
			}
			set
			{
				OnPropertyChanging("OriginatingLeadId");
				((Entity)this).SetAttributeValue("originatingleadid", (object)value);
				OnPropertyChanged("OriginatingLeadId");
			}
		}

		[AttributeLogicalName("overriddencreatedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? OverriddenCreatedOn
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("overriddencreatedon");
			}
			set
			{
				OnPropertyChanging("OverriddenCreatedOn");
				((Entity)this).SetAttributeValue("overriddencreatedon", (object)value);
				OnPropertyChanged("OverriddenCreatedOn");
			}
		}

		[AttributeLogicalName("ownerid")]
		[ExcludeFromCodeCoverage]
		public EntityReference OwnerId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("ownerid");
			}
			set
			{
				OnPropertyChanging("OwnerId");
				((Entity)this).SetAttributeValue("ownerid", (object)value);
				OnPropertyChanged("OwnerId");
			}
		}

		[AttributeLogicalName("ownershipcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue OwnershipCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("ownershipcode");
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
				OnPropertyChanging("OwnershipCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("ownershipcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("ownershipcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("OwnershipCode");
			}
		}

		[AttributeLogicalName("owningbusinessunit")]
		[ExcludeFromCodeCoverage]
		public EntityReference OwningBusinessUnit => ((Entity)this).GetAttributeValue<EntityReference>("owningbusinessunit");

		[AttributeLogicalName("owningteam")]
		[ExcludeFromCodeCoverage]
		public EntityReference OwningTeam => ((Entity)this).GetAttributeValue<EntityReference>("owningteam");

		[AttributeLogicalName("owninguser")]
		[ExcludeFromCodeCoverage]
		public EntityReference OwningUser => ((Entity)this).GetAttributeValue<EntityReference>("owninguser");

		[AttributeLogicalName("parentaccountid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ParentAccountId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("parentaccountid");
			}
			set
			{
				OnPropertyChanging("ParentAccountId");
				((Entity)this).SetAttributeValue("parentaccountid", (object)value);
				OnPropertyChanged("ParentAccountId");
			}
		}

		[AttributeLogicalName("participatesinworkflow")]
		[ExcludeFromCodeCoverage]
		public bool? ParticipatesInWorkflow
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("participatesinworkflow");
			}
			set
			{
				OnPropertyChanging("ParticipatesInWorkflow");
				((Entity)this).SetAttributeValue("participatesinworkflow", (object)value);
				OnPropertyChanged("ParticipatesInWorkflow");
			}
		}

		[AttributeLogicalName("paymenttermscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PaymentTermsCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("paymenttermscode");
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
				OnPropertyChanging("PaymentTermsCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("paymenttermscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("paymenttermscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PaymentTermsCode");
			}
		}

		[AttributeLogicalName("preferredappointmentdaycode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PreferredAppointmentDayCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("preferredappointmentdaycode");
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
				OnPropertyChanging("PreferredAppointmentDayCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("preferredappointmentdaycode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("preferredappointmentdaycode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PreferredAppointmentDayCode");
			}
		}

		[AttributeLogicalName("preferredappointmenttimecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PreferredAppointmentTimeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("preferredappointmenttimecode");
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
				OnPropertyChanging("PreferredAppointmentTimeCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("preferredappointmenttimecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("preferredappointmenttimecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PreferredAppointmentTimeCode");
			}
		}

		[AttributeLogicalName("preferredcontactmethodcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PreferredContactMethodCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("preferredcontactmethodcode");
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
				OnPropertyChanging("PreferredContactMethodCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("preferredcontactmethodcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("preferredcontactmethodcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PreferredContactMethodCode");
			}
		}

		[AttributeLogicalName("preferredsystemuserid")]
		[ExcludeFromCodeCoverage]
		public EntityReference PreferredSystemUserId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("preferredsystemuserid");
			}
			set
			{
				OnPropertyChanging("PreferredSystemUserId");
				((Entity)this).SetAttributeValue("preferredsystemuserid", (object)value);
				OnPropertyChanged("PreferredSystemUserId");
			}
		}

		[AttributeLogicalName("primarycontactid")]
		[ExcludeFromCodeCoverage]
		public EntityReference PrimaryContactId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("primarycontactid");
			}
			set
			{
				OnPropertyChanging("PrimaryContactId");
				((Entity)this).SetAttributeValue("primarycontactid", (object)value);
				OnPropertyChanged("PrimaryContactId");
			}
		}

		[AttributeLogicalName("primarysatoriid")]
		[ExcludeFromCodeCoverage]
		public string PrimarySatoriId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("primarysatoriid");
			}
			set
			{
				OnPropertyChanging("PrimarySatoriId");
				((Entity)this).SetAttributeValue("primarysatoriid", (object)value);
				OnPropertyChanged("PrimarySatoriId");
			}
		}

		[AttributeLogicalName("primarytwitterid")]
		[ExcludeFromCodeCoverage]
		public string PrimaryTwitterId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("primarytwitterid");
			}
			set
			{
				OnPropertyChanging("PrimaryTwitterId");
				((Entity)this).SetAttributeValue("primarytwitterid", (object)value);
				OnPropertyChanged("PrimaryTwitterId");
			}
		}

		[AttributeLogicalName("processid")]
		[ExcludeFromCodeCoverage]
		public Guid? ProcessId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("processid");
			}
			set
			{
				OnPropertyChanging("ProcessId");
				((Entity)this).SetAttributeValue("processid", (object)value);
				OnPropertyChanged("ProcessId");
			}
		}

		[AttributeLogicalName("revenue")]
		[ExcludeFromCodeCoverage]
		public Money Revenue
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Money>("revenue");
			}
			set
			{
				OnPropertyChanging("Revenue");
				((Entity)this).SetAttributeValue("revenue", (object)value);
				OnPropertyChanged("Revenue");
			}
		}

		[AttributeLogicalName("revenue_base")]
		[ExcludeFromCodeCoverage]
		public Money Revenue_Base => ((Entity)this).GetAttributeValue<Money>("revenue_base");

		[AttributeLogicalName("sharesoutstanding")]
		[ExcludeFromCodeCoverage]
		public int? SharesOutstanding
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("sharesoutstanding");
			}
			set
			{
				OnPropertyChanging("SharesOutstanding");
				((Entity)this).SetAttributeValue("sharesoutstanding", (object)value);
				OnPropertyChanged("SharesOutstanding");
			}
		}

		[AttributeLogicalName("shippingmethodcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ShippingMethodCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("shippingmethodcode");
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
				OnPropertyChanging("ShippingMethodCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("shippingmethodcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("shippingmethodcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ShippingMethodCode");
			}
		}

		[AttributeLogicalName("sic")]
		[ExcludeFromCodeCoverage]
		public string SIC
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("sic");
			}
			set
			{
				OnPropertyChanging("SIC");
				((Entity)this).SetAttributeValue("sic", (object)value);
				OnPropertyChanged("SIC");
			}
		}

		[AttributeLogicalName("slaid")]
		[ExcludeFromCodeCoverage]
		public EntityReference SLAId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("slaid");
			}
			set
			{
				OnPropertyChanging("SLAId");
				((Entity)this).SetAttributeValue("slaid", (object)value);
				OnPropertyChanged("SLAId");
			}
		}

		[AttributeLogicalName("slainvokedid")]
		[ExcludeFromCodeCoverage]
		public EntityReference SLAInvokedId => ((Entity)this).GetAttributeValue<EntityReference>("slainvokedid");

		[AttributeLogicalName("stageid")]
		[ExcludeFromCodeCoverage]
		public Guid? StageId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("stageid");
			}
			set
			{
				OnPropertyChanging("StageId");
				((Entity)this).SetAttributeValue("stageid", (object)value);
				OnPropertyChanged("StageId");
			}
		}

		[AttributeLogicalName("statecode")]
		[ExcludeFromCodeCoverage]
		public AccountState? StateCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (AccountState)Enum.ToObject(typeof(AccountState), attributeValue.get_Value());
				}
				return null;
			}
			set
			{
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Expected O, but got Unknown
				OnPropertyChanging("StateCode");
				if (!value.HasValue)
				{
					((Entity)this).SetAttributeValue("statecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("statecode", (object)new OptionSetValue((int)value.Value));
				}
				OnPropertyChanged("StateCode");
			}
		}

		[AttributeLogicalName("statuscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue StatusCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statuscode");
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
				OnPropertyChanging("StatusCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("statuscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("statuscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("StatusCode");
			}
		}

		[AttributeLogicalName("stockexchange")]
		[ExcludeFromCodeCoverage]
		public string StockExchange
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("stockexchange");
			}
			set
			{
				OnPropertyChanging("StockExchange");
				((Entity)this).SetAttributeValue("stockexchange", (object)value);
				OnPropertyChanged("StockExchange");
			}
		}

		[AttributeLogicalName("telephone1")]
		[ExcludeFromCodeCoverage]
		public string Telephone1
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("telephone1");
			}
			set
			{
				OnPropertyChanging("Telephone1");
				((Entity)this).SetAttributeValue("telephone1", (object)value);
				OnPropertyChanged("Telephone1");
			}
		}

		[AttributeLogicalName("telephone2")]
		[ExcludeFromCodeCoverage]
		public string Telephone2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("telephone2");
			}
			set
			{
				OnPropertyChanging("Telephone2");
				((Entity)this).SetAttributeValue("telephone2", (object)value);
				OnPropertyChanged("Telephone2");
			}
		}

		[AttributeLogicalName("telephone3")]
		[ExcludeFromCodeCoverage]
		public string Telephone3
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("telephone3");
			}
			set
			{
				OnPropertyChanging("Telephone3");
				((Entity)this).SetAttributeValue("telephone3", (object)value);
				OnPropertyChanged("Telephone3");
			}
		}

		[AttributeLogicalName("territorycode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue TerritoryCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("territorycode");
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
				OnPropertyChanging("TerritoryCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("territorycode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("territorycode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("TerritoryCode");
			}
		}

		[AttributeLogicalName("territoryid")]
		[ExcludeFromCodeCoverage]
		public EntityReference TerritoryId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("territoryid");
			}
			set
			{
				OnPropertyChanging("TerritoryId");
				((Entity)this).SetAttributeValue("territoryid", (object)value);
				OnPropertyChanged("TerritoryId");
			}
		}

		[AttributeLogicalName("tickersymbol")]
		[ExcludeFromCodeCoverage]
		public string TickerSymbol
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("tickersymbol");
			}
			set
			{
				OnPropertyChanging("TickerSymbol");
				((Entity)this).SetAttributeValue("tickersymbol", (object)value);
				OnPropertyChanged("TickerSymbol");
			}
		}

		[AttributeLogicalName("timespentbymeonemailandmeetings")]
		[ExcludeFromCodeCoverage]
		public string TimeSpentByMeOnEmailAndMeetings => ((Entity)this).GetAttributeValue<string>("timespentbymeonemailandmeetings");

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

		[AttributeLogicalName("traversedpath")]
		[ExcludeFromCodeCoverage]
		public string TraversedPath
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("traversedpath");
			}
			set
			{
				OnPropertyChanging("TraversedPath");
				((Entity)this).SetAttributeValue("traversedpath", (object)value);
				OnPropertyChanged("TraversedPath");
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

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[AttributeLogicalName("websiteurl")]
		[ExcludeFromCodeCoverage]
		public string WebSiteURL
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("websiteurl");
			}
			set
			{
				OnPropertyChanging("WebSiteURL");
				((Entity)this).SetAttributeValue("websiteurl", (object)value);
				OnPropertyChanged("WebSiteURL");
			}
		}

		[AttributeLogicalName("yominame")]
		[ExcludeFromCodeCoverage]
		public string YomiName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("yominame");
			}
			set
			{
				OnPropertyChanging("YomiName");
				((Entity)this).SetAttributeValue("yominame", (object)value);
				OnPropertyChanged("YomiName");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Account> Referencedaccount_master_account
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Account>("account_master_account", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedaccount_master_account");
				((Entity)this).SetRelatedEntities<Account>("account_master_account", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedaccount_master_account");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Account> Referencedaccount_parent_account
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Account>("account_parent_account", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedaccount_parent_account");
				((Entity)this).SetRelatedEntities<Account>("account_parent_account", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedaccount_parent_account");
			}
		}

		[RelationshipSchemaName("contact_customer_accounts")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Contact> contact_customer_accounts
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Contact>("contact_customer_accounts", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("contact_customer_accounts");
				((Entity)this).SetRelatedEntities<Contact>("contact_customer_accounts", (EntityRole?)null, value);
				OnPropertyChanged("contact_customer_accounts");
			}
		}

		[AttributeLogicalName("masterid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public Account Referencingaccount_master_account => ((Entity)this).GetRelatedEntity<Account>("account_master_account", (EntityRole?)(EntityRole)0);

		[AttributeLogicalName("parentaccountid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public Account Referencingaccount_parent_account
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<Account>("account_parent_account", (EntityRole?)(EntityRole)0);
			}
			set
			{
				OnPropertyChanging("Referencingaccount_parent_account");
				((Entity)this).SetRelatedEntity<Account>("account_parent_account", (EntityRole?)(EntityRole)0, value);
				OnPropertyChanged("Referencingaccount_parent_account");
			}
		}

		[AttributeLogicalName("primarycontactid")]
		[RelationshipSchemaName("account_primary_contact")]
		[ExcludeFromCodeCoverage]
		public Contact account_primary_contact
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<Contact>("account_primary_contact", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("account_primary_contact");
				((Entity)this).SetRelatedEntity<Contact>("account_primary_contact", (EntityRole?)null, value);
				OnPropertyChanged("account_primary_contact");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Account()
			: this("account")
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
