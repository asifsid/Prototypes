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
	[EntityLogicalName("contact")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Contact : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "contact";

		public const int EntityTypeCode = 2;

		public const int AttributeEMailAddress3_MaxLength = 100;

		public const int AttributeEMailAddress2_MaxLength = 100;

		public const int AttributeEMailAddress1_MaxLength = 100;

		public const int AttributeAddress1_City_MaxLength = 80;

		public const int AttributeAddress3_Line1_MaxLength = 250;

		public const int AttributeAddress1_Line1_MaxLength = 250;

		public const int AttributeYomiLastName_MaxLength = 150;

		public const int AttributeYomiFirstName_MaxLength = 150;

		public const int AttributeYomiFullName_MaxLength = 450;

		public const int AttributeAddress3_Line2_MaxLength = 250;

		public const int AttributeAddress3_Telephone1_MaxLength = 50;

		public const int AttributeAddress3_Line3_MaxLength = 250;

		public const int AttributeExternalUserIdentifier_MaxLength = 50;

		public const int AttributeAddress3_PrimaryContactName_MaxLength = 100;

		public const int AttributeEntityImage_URL_MaxLength = 200;

		public const int AttributeAddress2_StateOrProvince_MaxLength = 50;

		public const int AttributeMobilePhone_MaxLength = 50;

		public const int AttributeAddress2_Country_MaxLength = 80;

		public const int AttributeAddress2_Line2_MaxLength = 250;

		public const int AttributeAddress3_StateOrProvince_MaxLength = 50;

		public const int AttributeAssistantPhone_MaxLength = 50;

		public const int AttributeAddress3_Fax_MaxLength = 50;

		public const int AttributeManagerPhone_MaxLength = 50;

		public const int AttributeEmployeeId_MaxLength = 50;

		public const int AttributeAddress3_UPSZone_MaxLength = 4;

		public const int AttributeTelephone1_MaxLength = 50;

		public const int AttributeAddress2_County_MaxLength = 50;

		public const int AttributeTelephone3_MaxLength = 50;

		public const int AttributeChildrensNames_MaxLength = 255;

		public const int AttributeFirstName_MaxLength = 50;

		public const int AttributeCallback_MaxLength = 50;

		public const int AttributeAddress2_PostalCode_MaxLength = 20;

		public const int AttributeGovernmentId_MaxLength = 50;

		public const int AttributeAddress2_Line3_MaxLength = 250;

		public const int AttributeSpousesName_MaxLength = 100;

		public const int AttributeAddress1_County_MaxLength = 50;

		public const int AttributeHome2_MaxLength = 50;

		public const int AttributeAddress3_County_MaxLength = 50;

		public const int AttributePager_MaxLength = 50;

		public const int AttributeAddress2_PostOfficeBox_MaxLength = 20;

		public const int AttributeAddress2_Telephone1_MaxLength = 50;

		public const int AttributeAddress2_Telephone2_MaxLength = 50;

		public const int AttributeAddress2_Telephone3_MaxLength = 50;

		public const int AttributeTraversedPath_MaxLength = 1250;

		public const int AttributeWebSiteUrl_MaxLength = 200;

		public const int AttributeAddress2_Name_MaxLength = 100;

		public const int AttributeMiddleName_MaxLength = 50;

		public const int AttributeAddress3_Telephone2_MaxLength = 50;

		public const int AttributeTimeSpentByMeOnEmailAndMeetings_MaxLength = 1250;

		public const int AttributeAddress1_Country_MaxLength = 80;

		public const int AttributeAddress1_StateOrProvince_MaxLength = 50;

		public const int AttributeAddress1_Line3_MaxLength = 250;

		public const int AttributeJobTitle_MaxLength = 100;

		public const int AttributeNickName_MaxLength = 50;

		public const int AttributeManagerName_MaxLength = 100;

		public const int AttributeAddress1_Telephone1_MaxLength = 50;

		public const int AttributeAddress1_Telephone2_MaxLength = 50;

		public const int AttributeAddress1_Telephone3_MaxLength = 50;

		public const int AttributeSuffix_MaxLength = 10;

		public const int AttributeFax_MaxLength = 50;

		public const int AttributeAssistantName_MaxLength = 100;

		public const int AttributeYomiMiddleName_MaxLength = 150;

		public const int AttributeAddress3_Country_MaxLength = 80;

		public const int AttributeAddress2_Fax_MaxLength = 50;

		public const int AttributeAddress1_PrimaryContactName_MaxLength = 100;

		public const int AttributeAddress1_Line2_MaxLength = 250;

		public const int AttributeAddress2_UPSZone_MaxLength = 4;

		public const int AttributeAddress3_Name_MaxLength = 200;

		public const int AttributeSalutation_MaxLength = 100;

		public const int AttributeAddress1_PostalCode_MaxLength = 20;

		public const int AttributeCompany_MaxLength = 50;

		public const int AttributeAddress3_Telephone3_MaxLength = 50;

		public const int AttributeAddress3_PostalCode_MaxLength = 20;

		public const int AttributeAddress2_City_MaxLength = 80;

		public const int AttributeTelephone2_MaxLength = 50;

		public const int AttributeDepartment_MaxLength = 100;

		public const int AttributeAddress3_City_MaxLength = 80;

		public const int AttributeBusiness2_MaxLength = 50;

		public const int AttributeAddress1_Name_MaxLength = 200;

		public const int AttributeAddress1_Fax_MaxLength = 50;

		public const int AttributeAddress3_PostOfficeBox_MaxLength = 20;

		public const int AttributeAddress2_Line1_MaxLength = 250;

		public const int AttributeAddress1_UPSZone_MaxLength = 4;

		public const int AttributeLastName_MaxLength = 50;

		public const int AttributeFtpSiteUrl_MaxLength = 200;

		public const int AttributeAddress1_PostOfficeBox_MaxLength = 20;

		public const int AttributeAddress2_PrimaryContactName_MaxLength = 100;

		public const int AttributeFullName_MaxLength = 160;

		public const string AttributeAccountId = "accountid";

		public const string AttributeAccountRoleCode = "accountrolecode";

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

		public const string AttributeAddress3_AddressId = "address3_addressid";

		public const string AttributeAddress3_AddressTypeCode = "address3_addresstypecode";

		public const string AttributeAddress3_City = "address3_city";

		public const string AttributeAddress3_Composite = "address3_composite";

		public const string AttributeAddress3_Country = "address3_country";

		public const string AttributeAddress3_County = "address3_county";

		public const string AttributeAddress3_Fax = "address3_fax";

		public const string AttributeAddress3_FreightTermsCode = "address3_freighttermscode";

		public const string AttributeAddress3_Latitude = "address3_latitude";

		public const string AttributeAddress3_Line1 = "address3_line1";

		public const string AttributeAddress3_Line2 = "address3_line2";

		public const string AttributeAddress3_Line3 = "address3_line3";

		public const string AttributeAddress3_Longitude = "address3_longitude";

		public const string AttributeAddress3_Name = "address3_name";

		public const string AttributeAddress3_PostalCode = "address3_postalcode";

		public const string AttributeAddress3_PostOfficeBox = "address3_postofficebox";

		public const string AttributeAddress3_PrimaryContactName = "address3_primarycontactname";

		public const string AttributeAddress3_ShippingMethodCode = "address3_shippingmethodcode";

		public const string AttributeAddress3_StateOrProvince = "address3_stateorprovince";

		public const string AttributeAddress3_Telephone1 = "address3_telephone1";

		public const string AttributeAddress3_Telephone2 = "address3_telephone2";

		public const string AttributeAddress3_Telephone3 = "address3_telephone3";

		public const string AttributeAddress3_UPSZone = "address3_upszone";

		public const string AttributeAddress3_UTCOffset = "address3_utcoffset";

		public const string AttributeAging30 = "aging30";

		public const string AttributeAging30_Base = "aging30_base";

		public const string AttributeAging60 = "aging60";

		public const string AttributeAging60_Base = "aging60_base";

		public const string AttributeAging90 = "aging90";

		public const string AttributeAging90_Base = "aging90_base";

		public const string AttributeAnniversary = "anniversary";

		public const string AttributeAnnualIncome = "annualincome";

		public const string AttributeAnnualIncome_Base = "annualincome_base";

		public const string AttributeAssistantName = "assistantname";

		public const string AttributeAssistantPhone = "assistantphone";

		public const string AttributeBirthDate = "birthdate";

		public const string AttributeBusiness2 = "business2";

		public const string AttributeCallback = "callback";

		public const string AttributeChildrensNames = "childrensnames";

		public const string AttributeCompany = "company";

		public const string AttributeContactId = "contactid";

		public const string AttributeId = "contactid";

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

		public const string AttributeDepartment = "department";

		public const string AttributeDescription = "description";

		public const string AttributeDoNotBulkEMail = "donotbulkemail";

		public const string AttributeDoNotBulkPostalMail = "donotbulkpostalmail";

		public const string AttributeDoNotEMail = "donotemail";

		public const string AttributeDoNotFax = "donotfax";

		public const string AttributeDoNotPhone = "donotphone";

		public const string AttributeDoNotPostalMail = "donotpostalmail";

		public const string AttributeDoNotSendMM = "donotsendmm";

		public const string AttributeEducationCode = "educationcode";

		public const string AttributeEMailAddress1 = "emailaddress1";

		public const string AttributeEMailAddress2 = "emailaddress2";

		public const string AttributeEMailAddress3 = "emailaddress3";

		public const string AttributeEmployeeId = "employeeid";

		public const string AttributeEntityImage = "entityimage";

		public const string AttributeEntityImage_Timestamp = "entityimage_timestamp";

		public const string AttributeEntityImage_URL = "entityimage_url";

		public const string AttributeEntityImageId = "entityimageid";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeExternalUserIdentifier = "externaluseridentifier";

		public const string AttributeFamilyStatusCode = "familystatuscode";

		public const string AttributeFax = "fax";

		public const string AttributeFirstName = "firstname";

		public const string AttributeFollowEmail = "followemail";

		public const string AttributeFtpSiteUrl = "ftpsiteurl";

		public const string AttributeFullName = "fullname";

		public const string AttributeGenderCode = "gendercode";

		public const string AttributeGovernmentId = "governmentid";

		public const string AttributeHasChildrenCode = "haschildrencode";

		public const string AttributeHome2 = "home2";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIsBackofficeCustomer = "isbackofficecustomer";

		public const string AttributeJobTitle = "jobtitle";

		public const string AttributeLastName = "lastname";

		public const string AttributeLastOnHoldTime = "lastonholdtime";

		public const string AttributeLastUsedInCampaign = "lastusedincampaign";

		public const string AttributeLeadSourceCode = "leadsourcecode";

		public const string AttributeManagerName = "managername";

		public const string AttributeManagerPhone = "managerphone";

		public const string AttributeMarketingOnly = "marketingonly";

		public const string AttributeMasterId = "masterid";

		public const string AttributeMerged = "merged";

		public const string AttributeMiddleName = "middlename";

		public const string AttributeMobilePhone = "mobilephone";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedByExternalParty = "modifiedbyexternalparty";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeNickName = "nickname";

		public const string AttributeNumberOfChildren = "numberofchildren";

		public const string AttributeOnHoldTime = "onholdtime";

		public const string AttributeOriginatingLeadId = "originatingleadid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeOwnerId = "ownerid";

		public const string AttributeOwningBusinessUnit = "owningbusinessunit";

		public const string AttributeOwningTeam = "owningteam";

		public const string AttributeOwningUser = "owninguser";

		public const string AttributePager = "pager";

		public const string AttributeParentContactId = "parentcontactid";

		public const string AttributeParentCustomerId = "parentcustomerid";

		public const string AttributeParticipatesInWorkflow = "participatesinworkflow";

		public const string AttributePaymentTermsCode = "paymenttermscode";

		public const string AttributePreferredAppointmentDayCode = "preferredappointmentdaycode";

		public const string AttributePreferredAppointmentTimeCode = "preferredappointmenttimecode";

		public const string AttributePreferredContactMethodCode = "preferredcontactmethodcode";

		public const string AttributePreferredSystemUserId = "preferredsystemuserid";

		public const string AttributeProcessId = "processid";

		public const string AttributeSalutation = "salutation";

		public const string AttributeShippingMethodCode = "shippingmethodcode";

		public const string AttributeSLAId = "slaid";

		public const string AttributeSLAInvokedId = "slainvokedid";

		public const string AttributeSpousesName = "spousesname";

		public const string AttributeStageId = "stageid";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeSubscriptionId = "subscriptionid";

		public const string AttributeSuffix = "suffix";

		public const string AttributeTelephone1 = "telephone1";

		public const string AttributeTelephone2 = "telephone2";

		public const string AttributeTelephone3 = "telephone3";

		public const string AttributeTerritoryCode = "territorycode";

		public const string AttributeTimeSpentByMeOnEmailAndMeetings = "timespentbymeonemailandmeetings";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeTraversedPath = "traversedpath";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeWebSiteUrl = "websiteurl";

		public const string AttributeYomiFirstName = "yomifirstname";

		public const string AttributeYomiFullName = "yomifullname";

		public const string AttributeYomiLastName = "yomilastname";

		public const string AttributeYomiMiddleName = "yomimiddlename";

		public const string AttributeContact_customer_accounts = "parentcustomerid";

		public const string AttributeReferencingcontact_customer_contacts = "parentcustomerid";

		public const string AttributeReferencingcontact_master_contact = "masterid";

		[AttributeLogicalName("accountid")]
		[ExcludeFromCodeCoverage]
		public EntityReference AccountId => ((Entity)this).GetAttributeValue<EntityReference>("accountid");

		[AttributeLogicalName("accountrolecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue AccountRoleCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("accountrolecode");
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
				OnPropertyChanging("AccountRoleCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("accountrolecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("accountrolecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("AccountRoleCode");
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

		[AttributeLogicalName("address3_addressid")]
		[ExcludeFromCodeCoverage]
		public Guid? Address3_AddressId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("address3_addressid");
			}
			set
			{
				OnPropertyChanging("Address3_AddressId");
				((Entity)this).SetAttributeValue("address3_addressid", (object)value);
				OnPropertyChanged("Address3_AddressId");
			}
		}

		[AttributeLogicalName("address3_addresstypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address3_AddressTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address3_addresstypecode");
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
				OnPropertyChanging("Address3_AddressTypeCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address3_addresstypecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address3_addresstypecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address3_AddressTypeCode");
			}
		}

		[AttributeLogicalName("address3_city")]
		[ExcludeFromCodeCoverage]
		public string Address3_City
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_city");
			}
			set
			{
				OnPropertyChanging("Address3_City");
				((Entity)this).SetAttributeValue("address3_city", (object)value);
				OnPropertyChanged("Address3_City");
			}
		}

		[AttributeLogicalName("address3_composite")]
		[ExcludeFromCodeCoverage]
		public string Address3_Composite => ((Entity)this).GetAttributeValue<string>("address3_composite");

		[AttributeLogicalName("address3_country")]
		[ExcludeFromCodeCoverage]
		public string Address3_Country
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_country");
			}
			set
			{
				OnPropertyChanging("Address3_Country");
				((Entity)this).SetAttributeValue("address3_country", (object)value);
				OnPropertyChanged("Address3_Country");
			}
		}

		[AttributeLogicalName("address3_county")]
		[ExcludeFromCodeCoverage]
		public string Address3_County
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_county");
			}
			set
			{
				OnPropertyChanging("Address3_County");
				((Entity)this).SetAttributeValue("address3_county", (object)value);
				OnPropertyChanged("Address3_County");
			}
		}

		[AttributeLogicalName("address3_fax")]
		[ExcludeFromCodeCoverage]
		public string Address3_Fax
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_fax");
			}
			set
			{
				OnPropertyChanging("Address3_Fax");
				((Entity)this).SetAttributeValue("address3_fax", (object)value);
				OnPropertyChanged("Address3_Fax");
			}
		}

		[AttributeLogicalName("address3_freighttermscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address3_FreightTermsCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address3_freighttermscode");
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
				OnPropertyChanging("Address3_FreightTermsCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address3_freighttermscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address3_freighttermscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address3_FreightTermsCode");
			}
		}

		[AttributeLogicalName("address3_latitude")]
		[ExcludeFromCodeCoverage]
		public double? Address3_Latitude
		{
			get
			{
				return ((Entity)this).GetAttributeValue<double?>("address3_latitude");
			}
			set
			{
				OnPropertyChanging("Address3_Latitude");
				((Entity)this).SetAttributeValue("address3_latitude", (object)value);
				OnPropertyChanged("Address3_Latitude");
			}
		}

		[AttributeLogicalName("address3_line1")]
		[ExcludeFromCodeCoverage]
		public string Address3_Line1
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_line1");
			}
			set
			{
				OnPropertyChanging("Address3_Line1");
				((Entity)this).SetAttributeValue("address3_line1", (object)value);
				OnPropertyChanged("Address3_Line1");
			}
		}

		[AttributeLogicalName("address3_line2")]
		[ExcludeFromCodeCoverage]
		public string Address3_Line2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_line2");
			}
			set
			{
				OnPropertyChanging("Address3_Line2");
				((Entity)this).SetAttributeValue("address3_line2", (object)value);
				OnPropertyChanged("Address3_Line2");
			}
		}

		[AttributeLogicalName("address3_line3")]
		[ExcludeFromCodeCoverage]
		public string Address3_Line3
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_line3");
			}
			set
			{
				OnPropertyChanging("Address3_Line3");
				((Entity)this).SetAttributeValue("address3_line3", (object)value);
				OnPropertyChanged("Address3_Line3");
			}
		}

		[AttributeLogicalName("address3_longitude")]
		[ExcludeFromCodeCoverage]
		public double? Address3_Longitude
		{
			get
			{
				return ((Entity)this).GetAttributeValue<double?>("address3_longitude");
			}
			set
			{
				OnPropertyChanging("Address3_Longitude");
				((Entity)this).SetAttributeValue("address3_longitude", (object)value);
				OnPropertyChanged("Address3_Longitude");
			}
		}

		[AttributeLogicalName("address3_name")]
		[ExcludeFromCodeCoverage]
		public string Address3_Name
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_name");
			}
			set
			{
				OnPropertyChanging("Address3_Name");
				((Entity)this).SetAttributeValue("address3_name", (object)value);
				OnPropertyChanged("Address3_Name");
			}
		}

		[AttributeLogicalName("address3_postalcode")]
		[ExcludeFromCodeCoverage]
		public string Address3_PostalCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_postalcode");
			}
			set
			{
				OnPropertyChanging("Address3_PostalCode");
				((Entity)this).SetAttributeValue("address3_postalcode", (object)value);
				OnPropertyChanged("Address3_PostalCode");
			}
		}

		[AttributeLogicalName("address3_postofficebox")]
		[ExcludeFromCodeCoverage]
		public string Address3_PostOfficeBox
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_postofficebox");
			}
			set
			{
				OnPropertyChanging("Address3_PostOfficeBox");
				((Entity)this).SetAttributeValue("address3_postofficebox", (object)value);
				OnPropertyChanged("Address3_PostOfficeBox");
			}
		}

		[AttributeLogicalName("address3_primarycontactname")]
		[ExcludeFromCodeCoverage]
		public string Address3_PrimaryContactName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_primarycontactname");
			}
			set
			{
				OnPropertyChanging("Address3_PrimaryContactName");
				((Entity)this).SetAttributeValue("address3_primarycontactname", (object)value);
				OnPropertyChanged("Address3_PrimaryContactName");
			}
		}

		[AttributeLogicalName("address3_shippingmethodcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Address3_ShippingMethodCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("address3_shippingmethodcode");
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
				OnPropertyChanging("Address3_ShippingMethodCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("address3_shippingmethodcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("address3_shippingmethodcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Address3_ShippingMethodCode");
			}
		}

		[AttributeLogicalName("address3_stateorprovince")]
		[ExcludeFromCodeCoverage]
		public string Address3_StateOrProvince
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_stateorprovince");
			}
			set
			{
				OnPropertyChanging("Address3_StateOrProvince");
				((Entity)this).SetAttributeValue("address3_stateorprovince", (object)value);
				OnPropertyChanged("Address3_StateOrProvince");
			}
		}

		[AttributeLogicalName("address3_telephone1")]
		[ExcludeFromCodeCoverage]
		public string Address3_Telephone1
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_telephone1");
			}
			set
			{
				OnPropertyChanging("Address3_Telephone1");
				((Entity)this).SetAttributeValue("address3_telephone1", (object)value);
				OnPropertyChanged("Address3_Telephone1");
			}
		}

		[AttributeLogicalName("address3_telephone2")]
		[ExcludeFromCodeCoverage]
		public string Address3_Telephone2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_telephone2");
			}
			set
			{
				OnPropertyChanging("Address3_Telephone2");
				((Entity)this).SetAttributeValue("address3_telephone2", (object)value);
				OnPropertyChanged("Address3_Telephone2");
			}
		}

		[AttributeLogicalName("address3_telephone3")]
		[ExcludeFromCodeCoverage]
		public string Address3_Telephone3
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_telephone3");
			}
			set
			{
				OnPropertyChanging("Address3_Telephone3");
				((Entity)this).SetAttributeValue("address3_telephone3", (object)value);
				OnPropertyChanged("Address3_Telephone3");
			}
		}

		[AttributeLogicalName("address3_upszone")]
		[ExcludeFromCodeCoverage]
		public string Address3_UPSZone
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("address3_upszone");
			}
			set
			{
				OnPropertyChanging("Address3_UPSZone");
				((Entity)this).SetAttributeValue("address3_upszone", (object)value);
				OnPropertyChanged("Address3_UPSZone");
			}
		}

		[AttributeLogicalName("address3_utcoffset")]
		[ExcludeFromCodeCoverage]
		public int? Address3_UTCOffset
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("address3_utcoffset");
			}
			set
			{
				OnPropertyChanging("Address3_UTCOffset");
				((Entity)this).SetAttributeValue("address3_utcoffset", (object)value);
				OnPropertyChanged("Address3_UTCOffset");
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

		[AttributeLogicalName("anniversary")]
		[ExcludeFromCodeCoverage]
		public DateTime? Anniversary
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("anniversary");
			}
			set
			{
				OnPropertyChanging("Anniversary");
				((Entity)this).SetAttributeValue("anniversary", (object)value);
				OnPropertyChanged("Anniversary");
			}
		}

		[AttributeLogicalName("annualincome")]
		[ExcludeFromCodeCoverage]
		public Money AnnualIncome
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Money>("annualincome");
			}
			set
			{
				OnPropertyChanging("AnnualIncome");
				((Entity)this).SetAttributeValue("annualincome", (object)value);
				OnPropertyChanged("AnnualIncome");
			}
		}

		[AttributeLogicalName("annualincome_base")]
		[ExcludeFromCodeCoverage]
		public Money AnnualIncome_Base => ((Entity)this).GetAttributeValue<Money>("annualincome_base");

		[AttributeLogicalName("assistantname")]
		[ExcludeFromCodeCoverage]
		public string AssistantName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("assistantname");
			}
			set
			{
				OnPropertyChanging("AssistantName");
				((Entity)this).SetAttributeValue("assistantname", (object)value);
				OnPropertyChanged("AssistantName");
			}
		}

		[AttributeLogicalName("assistantphone")]
		[ExcludeFromCodeCoverage]
		public string AssistantPhone
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("assistantphone");
			}
			set
			{
				OnPropertyChanging("AssistantPhone");
				((Entity)this).SetAttributeValue("assistantphone", (object)value);
				OnPropertyChanged("AssistantPhone");
			}
		}

		[AttributeLogicalName("birthdate")]
		[ExcludeFromCodeCoverage]
		public DateTime? BirthDate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("birthdate");
			}
			set
			{
				OnPropertyChanging("BirthDate");
				((Entity)this).SetAttributeValue("birthdate", (object)value);
				OnPropertyChanged("BirthDate");
			}
		}

		[AttributeLogicalName("business2")]
		[ExcludeFromCodeCoverage]
		public string Business2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("business2");
			}
			set
			{
				OnPropertyChanging("Business2");
				((Entity)this).SetAttributeValue("business2", (object)value);
				OnPropertyChanged("Business2");
			}
		}

		[AttributeLogicalName("callback")]
		[ExcludeFromCodeCoverage]
		public string Callback
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("callback");
			}
			set
			{
				OnPropertyChanging("Callback");
				((Entity)this).SetAttributeValue("callback", (object)value);
				OnPropertyChanged("Callback");
			}
		}

		[AttributeLogicalName("childrensnames")]
		[ExcludeFromCodeCoverage]
		public string ChildrensNames
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("childrensnames");
			}
			set
			{
				OnPropertyChanging("ChildrensNames");
				((Entity)this).SetAttributeValue("childrensnames", (object)value);
				OnPropertyChanged("ChildrensNames");
			}
		}

		[AttributeLogicalName("company")]
		[ExcludeFromCodeCoverage]
		public string Company
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("company");
			}
			set
			{
				OnPropertyChanging("Company");
				((Entity)this).SetAttributeValue("company", (object)value);
				OnPropertyChanged("Company");
			}
		}

		[AttributeLogicalName("contactid")]
		[ExcludeFromCodeCoverage]
		public Guid? ContactId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("contactid");
			}
			set
			{
				OnPropertyChanging("ContactId");
				((Entity)this).SetAttributeValue("contactid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("ContactId");
			}
		}

		[AttributeLogicalName("contactid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				ContactId = value;
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

		[AttributeLogicalName("department")]
		[ExcludeFromCodeCoverage]
		public string Department
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("department");
			}
			set
			{
				OnPropertyChanging("Department");
				((Entity)this).SetAttributeValue("department", (object)value);
				OnPropertyChanged("Department");
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

		[AttributeLogicalName("educationcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue EducationCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("educationcode");
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
				OnPropertyChanging("EducationCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("educationcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("educationcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("EducationCode");
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

		[AttributeLogicalName("employeeid")]
		[ExcludeFromCodeCoverage]
		public string EmployeeId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("employeeid");
			}
			set
			{
				OnPropertyChanging("EmployeeId");
				((Entity)this).SetAttributeValue("employeeid", (object)value);
				OnPropertyChanged("EmployeeId");
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

		[AttributeLogicalName("externaluseridentifier")]
		[ExcludeFromCodeCoverage]
		public string ExternalUserIdentifier
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("externaluseridentifier");
			}
			set
			{
				OnPropertyChanging("ExternalUserIdentifier");
				((Entity)this).SetAttributeValue("externaluseridentifier", (object)value);
				OnPropertyChanged("ExternalUserIdentifier");
			}
		}

		[AttributeLogicalName("familystatuscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue FamilyStatusCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("familystatuscode");
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
				OnPropertyChanging("FamilyStatusCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("familystatuscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("familystatuscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("FamilyStatusCode");
			}
		}

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

		[AttributeLogicalName("firstname")]
		[ExcludeFromCodeCoverage]
		public string FirstName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("firstname");
			}
			set
			{
				OnPropertyChanging("FirstName");
				((Entity)this).SetAttributeValue("firstname", (object)value);
				OnPropertyChanged("FirstName");
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
		public string FtpSiteUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("ftpsiteurl");
			}
			set
			{
				OnPropertyChanging("FtpSiteUrl");
				((Entity)this).SetAttributeValue("ftpsiteurl", (object)value);
				OnPropertyChanged("FtpSiteUrl");
			}
		}

		[AttributeLogicalName("fullname")]
		[ExcludeFromCodeCoverage]
		public string FullName => ((Entity)this).GetAttributeValue<string>("fullname");

		[AttributeLogicalName("gendercode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue GenderCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("gendercode");
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
				OnPropertyChanging("GenderCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("gendercode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("gendercode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("GenderCode");
			}
		}

		[AttributeLogicalName("governmentid")]
		[ExcludeFromCodeCoverage]
		public string GovernmentId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("governmentid");
			}
			set
			{
				OnPropertyChanging("GovernmentId");
				((Entity)this).SetAttributeValue("governmentid", (object)value);
				OnPropertyChanged("GovernmentId");
			}
		}

		[AttributeLogicalName("haschildrencode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue HasChildrenCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("haschildrencode");
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
				OnPropertyChanging("HasChildrenCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("haschildrencode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("haschildrencode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("HasChildrenCode");
			}
		}

		[AttributeLogicalName("home2")]
		[ExcludeFromCodeCoverage]
		public string Home2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("home2");
			}
			set
			{
				OnPropertyChanging("Home2");
				((Entity)this).SetAttributeValue("home2", (object)value);
				OnPropertyChanged("Home2");
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

		[AttributeLogicalName("isbackofficecustomer")]
		[ExcludeFromCodeCoverage]
		public bool? IsBackofficeCustomer
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isbackofficecustomer");
			}
			set
			{
				OnPropertyChanging("IsBackofficeCustomer");
				((Entity)this).SetAttributeValue("isbackofficecustomer", (object)value);
				OnPropertyChanged("IsBackofficeCustomer");
			}
		}

		[AttributeLogicalName("jobtitle")]
		[ExcludeFromCodeCoverage]
		public string JobTitle
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("jobtitle");
			}
			set
			{
				OnPropertyChanging("JobTitle");
				((Entity)this).SetAttributeValue("jobtitle", (object)value);
				OnPropertyChanged("JobTitle");
			}
		}

		[AttributeLogicalName("lastname")]
		[ExcludeFromCodeCoverage]
		public string LastName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("lastname");
			}
			set
			{
				OnPropertyChanging("LastName");
				((Entity)this).SetAttributeValue("lastname", (object)value);
				OnPropertyChanged("LastName");
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

		[AttributeLogicalName("leadsourcecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue LeadSourceCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("leadsourcecode");
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
				OnPropertyChanging("LeadSourceCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("leadsourcecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("leadsourcecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("LeadSourceCode");
			}
		}

		[AttributeLogicalName("managername")]
		[ExcludeFromCodeCoverage]
		public string ManagerName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("managername");
			}
			set
			{
				OnPropertyChanging("ManagerName");
				((Entity)this).SetAttributeValue("managername", (object)value);
				OnPropertyChanged("ManagerName");
			}
		}

		[AttributeLogicalName("managerphone")]
		[ExcludeFromCodeCoverage]
		public string ManagerPhone
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("managerphone");
			}
			set
			{
				OnPropertyChanging("ManagerPhone");
				((Entity)this).SetAttributeValue("managerphone", (object)value);
				OnPropertyChanged("ManagerPhone");
			}
		}

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

		[AttributeLogicalName("middlename")]
		[ExcludeFromCodeCoverage]
		public string MiddleName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("middlename");
			}
			set
			{
				OnPropertyChanging("MiddleName");
				((Entity)this).SetAttributeValue("middlename", (object)value);
				OnPropertyChanged("MiddleName");
			}
		}

		[AttributeLogicalName("mobilephone")]
		[ExcludeFromCodeCoverage]
		public string MobilePhone
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("mobilephone");
			}
			set
			{
				OnPropertyChanging("MobilePhone");
				((Entity)this).SetAttributeValue("mobilephone", (object)value);
				OnPropertyChanged("MobilePhone");
			}
		}

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

		[AttributeLogicalName("nickname")]
		[ExcludeFromCodeCoverage]
		public string NickName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("nickname");
			}
			set
			{
				OnPropertyChanging("NickName");
				((Entity)this).SetAttributeValue("nickname", (object)value);
				OnPropertyChanged("NickName");
			}
		}

		[AttributeLogicalName("numberofchildren")]
		[ExcludeFromCodeCoverage]
		public int? NumberOfChildren
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("numberofchildren");
			}
			set
			{
				OnPropertyChanging("NumberOfChildren");
				((Entity)this).SetAttributeValue("numberofchildren", (object)value);
				OnPropertyChanged("NumberOfChildren");
			}
		}

		[AttributeLogicalName("onholdtime")]
		[ExcludeFromCodeCoverage]
		public int? OnHoldTime => ((Entity)this).GetAttributeValue<int?>("onholdtime");

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

		[AttributeLogicalName("owningbusinessunit")]
		[ExcludeFromCodeCoverage]
		public EntityReference OwningBusinessUnit => ((Entity)this).GetAttributeValue<EntityReference>("owningbusinessunit");

		[AttributeLogicalName("owningteam")]
		[ExcludeFromCodeCoverage]
		public EntityReference OwningTeam => ((Entity)this).GetAttributeValue<EntityReference>("owningteam");

		[AttributeLogicalName("owninguser")]
		[ExcludeFromCodeCoverage]
		public EntityReference OwningUser => ((Entity)this).GetAttributeValue<EntityReference>("owninguser");

		[AttributeLogicalName("pager")]
		[ExcludeFromCodeCoverage]
		public string Pager
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("pager");
			}
			set
			{
				OnPropertyChanging("Pager");
				((Entity)this).SetAttributeValue("pager", (object)value);
				OnPropertyChanged("Pager");
			}
		}

		[AttributeLogicalName("parentcontactid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ParentContactId => ((Entity)this).GetAttributeValue<EntityReference>("parentcontactid");

		[AttributeLogicalName("parentcustomerid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ParentCustomerId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("parentcustomerid");
			}
			set
			{
				OnPropertyChanging("ParentCustomerId");
				((Entity)this).SetAttributeValue("parentcustomerid", (object)value);
				OnPropertyChanged("ParentCustomerId");
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

		[AttributeLogicalName("salutation")]
		[ExcludeFromCodeCoverage]
		public string Salutation
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("salutation");
			}
			set
			{
				OnPropertyChanging("Salutation");
				((Entity)this).SetAttributeValue("salutation", (object)value);
				OnPropertyChanged("Salutation");
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

		[AttributeLogicalName("spousesname")]
		[ExcludeFromCodeCoverage]
		public string SpousesName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("spousesname");
			}
			set
			{
				OnPropertyChanging("SpousesName");
				((Entity)this).SetAttributeValue("spousesname", (object)value);
				OnPropertyChanged("SpousesName");
			}
		}

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
		public ContactState? StateCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (ContactState)Enum.ToObject(typeof(ContactState), attributeValue.get_Value());
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

		[AttributeLogicalName("subscriptionid")]
		[ExcludeFromCodeCoverage]
		public Guid? SubscriptionId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("subscriptionid");
			}
			set
			{
				OnPropertyChanging("SubscriptionId");
				((Entity)this).SetAttributeValue("subscriptionid", (object)value);
				OnPropertyChanged("SubscriptionId");
			}
		}

		[AttributeLogicalName("suffix")]
		[ExcludeFromCodeCoverage]
		public string Suffix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("suffix");
			}
			set
			{
				OnPropertyChanging("Suffix");
				((Entity)this).SetAttributeValue("suffix", (object)value);
				OnPropertyChanged("Suffix");
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
		public string WebSiteUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("websiteurl");
			}
			set
			{
				OnPropertyChanging("WebSiteUrl");
				((Entity)this).SetAttributeValue("websiteurl", (object)value);
				OnPropertyChanged("WebSiteUrl");
			}
		}

		[AttributeLogicalName("yomifirstname")]
		[ExcludeFromCodeCoverage]
		public string YomiFirstName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("yomifirstname");
			}
			set
			{
				OnPropertyChanging("YomiFirstName");
				((Entity)this).SetAttributeValue("yomifirstname", (object)value);
				OnPropertyChanged("YomiFirstName");
			}
		}

		[AttributeLogicalName("yomifullname")]
		[ExcludeFromCodeCoverage]
		public string YomiFullName => ((Entity)this).GetAttributeValue<string>("yomifullname");

		[AttributeLogicalName("yomilastname")]
		[ExcludeFromCodeCoverage]
		public string YomiLastName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("yomilastname");
			}
			set
			{
				OnPropertyChanging("YomiLastName");
				((Entity)this).SetAttributeValue("yomilastname", (object)value);
				OnPropertyChanged("YomiLastName");
			}
		}

		[AttributeLogicalName("yomimiddlename")]
		[ExcludeFromCodeCoverage]
		public string YomiMiddleName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("yomimiddlename");
			}
			set
			{
				OnPropertyChanging("YomiMiddleName");
				((Entity)this).SetAttributeValue("yomimiddlename", (object)value);
				OnPropertyChanged("YomiMiddleName");
			}
		}

		[RelationshipSchemaName("account_primary_contact")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Account> account_primary_contact
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Account>("account_primary_contact", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("account_primary_contact");
				((Entity)this).SetRelatedEntities<Account>("account_primary_contact", (EntityRole?)null, value);
				OnPropertyChanged("account_primary_contact");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Contact> Referencedcontact_customer_contacts
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Contact>("contact_customer_contacts", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedcontact_customer_contacts");
				((Entity)this).SetRelatedEntities<Contact>("contact_customer_contacts", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedcontact_customer_contacts");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Contact> Referencedcontact_master_contact
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Contact>("contact_master_contact", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedcontact_master_contact");
				((Entity)this).SetRelatedEntities<Contact>("contact_master_contact", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedcontact_master_contact");
			}
		}

		[AttributeLogicalName("parentcustomerid")]
		[RelationshipSchemaName("contact_customer_accounts")]
		[ExcludeFromCodeCoverage]
		public Account contact_customer_accounts
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<Account>("contact_customer_accounts", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("contact_customer_accounts");
				((Entity)this).SetRelatedEntity<Account>("contact_customer_accounts", (EntityRole?)null, value);
				OnPropertyChanged("contact_customer_accounts");
			}
		}

		[AttributeLogicalName("parentcustomerid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public Contact Referencingcontact_customer_contacts
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<Contact>("contact_customer_contacts", (EntityRole?)(EntityRole)0);
			}
			set
			{
				OnPropertyChanging("Referencingcontact_customer_contacts");
				((Entity)this).SetRelatedEntity<Contact>("contact_customer_contacts", (EntityRole?)(EntityRole)0, value);
				OnPropertyChanged("Referencingcontact_customer_contacts");
			}
		}

		[AttributeLogicalName("masterid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public Contact Referencingcontact_master_contact => ((Entity)this).GetRelatedEntity<Contact>("contact_master_contact", (EntityRole?)(EntityRole)0);

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Contact()
			: this("contact")
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
