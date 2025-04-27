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
	[EntityLogicalName("systemuser")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class User : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "systemuser";

		public const int EntityTypeCode = 8;

		public const int AttributeTraversedPath_MaxLength = 1250;

		public const int AttributeAddress1_Line1_MaxLength = 1024;

		public const int AttributeInternalEMailAddress_MaxLength = 100;

		public const int AttributeAddress1_Line3_MaxLength = 1024;

		public const int AttributeSkills_MaxLength = 100;

		public const int AttributeYomiFirstName_MaxLength = 64;

		public const int AttributeYomiFullName_MaxLength = 200;

		public const int AttributeAddress2_County_MaxLength = 128;

		public const int AttributeTitle_MaxLength = 128;

		public const int AttributeHomePhone_MaxLength = 50;

		public const int AttributeAddress2_StateOrProvince_MaxLength = 128;

		public const int AttributeAddress2_Country_MaxLength = 128;

		public const int AttributeAddress2_Name_MaxLength = 100;

		public const int AttributeDisabledReason_MaxLength = 500;

		public const int AttributeLastName_MaxLength = 64;

		public const int AttributeAddress1_City_MaxLength = 128;

		public const int AttributeAddress2_City_MaxLength = 128;

		public const int AttributeYomiMiddleName_MaxLength = 50;

		public const int AttributeFirstName_MaxLength = 64;

		public const int AttributeAddress2_PostalCode_MaxLength = 40;

		public const int AttributeEmployeeId_MaxLength = 100;

		public const int AttributeGovernmentId_MaxLength = 100;

		public const int AttributeEntityImage_URL_MaxLength = 200;

		public const int AttributeAddress2_Line3_MaxLength = 1024;

		public const int AttributeAddress1_County_MaxLength = 128;

		public const int AttributeAddress2_Telephone2_MaxLength = 50;

		public const int AttributeAddress2_PostOfficeBox_MaxLength = 40;

		public const int AttributeAddress2_Telephone1_MaxLength = 50;

		public const int AttributeYomiLastName_MaxLength = 64;

		public const int AttributeAddress2_Telephone3_MaxLength = 50;

		public const int AttributeMiddleName_MaxLength = 50;

		public const int AttributeAddress2_Line2_MaxLength = 1024;

		public const int AttributeApplicationIdUri_MaxLength = 1024;

		public const int AttributeMobilePhone_MaxLength = 64;

		public const int AttributeAddress1_Country_MaxLength = 128;

		public const int AttributeMobileAlertEMail_MaxLength = 100;

		public const int AttributeSharePointEmailAddress_MaxLength = 1024;

		public const int AttributeAddress1_StateOrProvince_MaxLength = 128;

		public const int AttributeJobTitle_MaxLength = 100;

		public const int AttributeNickName_MaxLength = 50;

		public const int AttributeAddress1_Telephone1_MaxLength = 64;

		public const int AttributeAddress1_Telephone2_MaxLength = 50;

		public const int AttributeAddress1_Telephone3_MaxLength = 50;

		public const int AttributeAddress1_PostOfficeBox_MaxLength = 40;

		public const int AttributeYammerEmailAddress_MaxLength = 200;

		public const int AttributePhotoUrl_MaxLength = 200;

		public const int AttributePersonalEMailAddress_MaxLength = 100;

		public const int AttributeDomainName_MaxLength = 1024;

		public const int AttributeAddress2_Fax_MaxLength = 50;

		public const int AttributeAddress1_Line2_MaxLength = 1024;

		public const int AttributeAddress2_UPSZone_MaxLength = 4;

		public const int AttributeWindowsLiveID_MaxLength = 1024;

		public const int AttributeSalutation_MaxLength = 20;

		public const int AttributeAddress1_PostalCode_MaxLength = 40;

		public const int AttributeDefaultOdbFolderName_MaxLength = 200;

		public const int AttributeYammerUserId_MaxLength = 128;

		public const int AttributeAddress1_Name_MaxLength = 100;

		public const int AttributeAddress1_Fax_MaxLength = 64;

		public const int AttributeAddress2_Line1_MaxLength = 1024;

		public const int AttributeAddress1_UPSZone_MaxLength = 4;

		public const int AttributeFullName_MaxLength = 200;

		public const string AttributeAccessMode = "accessmode";

		public const string AttributeAddress1_AddressId = "address1_addressid";

		public const string AttributeAddress1_AddressTypeCode = "address1_addresstypecode";

		public const string AttributeAddress1_City = "address1_city";

		public const string AttributeAddress1_Composite = "address1_composite";

		public const string AttributeAddress1_Country = "address1_country";

		public const string AttributeAddress1_County = "address1_county";

		public const string AttributeAddress1_Fax = "address1_fax";

		public const string AttributeAddress1_Latitude = "address1_latitude";

		public const string AttributeAddress1_Line1 = "address1_line1";

		public const string AttributeAddress1_Line2 = "address1_line2";

		public const string AttributeAddress1_Line3 = "address1_line3";

		public const string AttributeAddress1_Longitude = "address1_longitude";

		public const string AttributeAddress1_Name = "address1_name";

		public const string AttributeAddress1_PostalCode = "address1_postalcode";

		public const string AttributeAddress1_PostOfficeBox = "address1_postofficebox";

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

		public const string AttributeAddress2_Latitude = "address2_latitude";

		public const string AttributeAddress2_Line1 = "address2_line1";

		public const string AttributeAddress2_Line2 = "address2_line2";

		public const string AttributeAddress2_Line3 = "address2_line3";

		public const string AttributeAddress2_Longitude = "address2_longitude";

		public const string AttributeAddress2_Name = "address2_name";

		public const string AttributeAddress2_PostalCode = "address2_postalcode";

		public const string AttributeAddress2_PostOfficeBox = "address2_postofficebox";

		public const string AttributeAddress2_ShippingMethodCode = "address2_shippingmethodcode";

		public const string AttributeAddress2_StateOrProvince = "address2_stateorprovince";

		public const string AttributeAddress2_Telephone1 = "address2_telephone1";

		public const string AttributeAddress2_Telephone2 = "address2_telephone2";

		public const string AttributeAddress2_Telephone3 = "address2_telephone3";

		public const string AttributeAddress2_UPSZone = "address2_upszone";

		public const string AttributeAddress2_UTCOffset = "address2_utcoffset";

		public const string AttributeApplicationId = "applicationid";

		public const string AttributeApplicationIdUri = "applicationiduri";

		public const string AttributeAzureActiveDirectoryObjectId = "azureactivedirectoryobjectid";

		public const string AttributeBusinessUnitId = "businessunitid";

		public const string AttributeCalendarId = "calendarid";

		public const string AttributeCALType = "caltype";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDefaultFiltersPopulated = "defaultfilterspopulated";

		public const string AttributeDefaultMailbox = "defaultmailbox";

		public const string AttributeDefaultOdbFolderName = "defaultodbfoldername";

		public const string AttributeDisabledReason = "disabledreason";

		public const string AttributeDisplayInServiceViews = "displayinserviceviews";

		public const string AttributeDomainName = "domainname";

		public const string AttributeEmailRouterAccessApproval = "emailrouteraccessapproval";

		public const string AttributeEmployeeId = "employeeid";

		public const string AttributeEntityImage = "entityimage";

		public const string AttributeEntityImage_Timestamp = "entityimage_timestamp";

		public const string AttributeEntityImage_URL = "entityimage_url";

		public const string AttributeEntityImageId = "entityimageid";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeFirstName = "firstname";

		public const string AttributeFullName = "fullname";

		public const string AttributeGovernmentId = "governmentid";

		public const string AttributeHomePhone = "homephone";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIncomingEmailDeliveryMethod = "incomingemaildeliverymethod";

		public const string AttributeInternalEMailAddress = "internalemailaddress";

		public const string AttributeInviteStatusCode = "invitestatuscode";

		public const string AttributeIsDisabled = "isdisabled";

		public const string AttributeIsEmailAddressApprovedByO365Admin = "isemailaddressapprovedbyo365admin";

		public const string AttributeIsIntegrationUser = "isintegrationuser";

		public const string AttributeIsLicensed = "islicensed";

		public const string AttributeIsSyncWithDirectory = "issyncwithdirectory";

		public const string AttributeJobTitle = "jobtitle";

		public const string AttributeLastName = "lastname";

		public const string AttributeMiddleName = "middlename";

		public const string AttributeMobileAlertEMail = "mobilealertemail";

		public const string AttributeMobileOfflineProfileId = "mobileofflineprofileid";

		public const string AttributeMobilePhone = "mobilephone";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeNickName = "nickname";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOutgoingEmailDeliveryMethod = "outgoingemaildeliverymethod";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeParentSystemUserId = "parentsystemuserid";

		public const string AttributePassportHi = "passporthi";

		public const string AttributePassportLo = "passportlo";

		public const string AttributePersonalEMailAddress = "personalemailaddress";

		public const string AttributePhotoUrl = "photourl";

		public const string AttributePositionId = "positionid";

		public const string AttributePreferredAddressCode = "preferredaddresscode";

		public const string AttributePreferredEmailCode = "preferredemailcode";

		public const string AttributePreferredPhoneCode = "preferredphonecode";

		public const string AttributeProcessId = "processid";

		public const string AttributeQueueId = "queueid";

		public const string AttributeSalutation = "salutation";

		public const string AttributeSetupUser = "setupuser";

		public const string AttributeSharePointEmailAddress = "sharepointemailaddress";

		public const string AttributeSiteId = "siteid";

		public const string AttributeSkills = "skills";

		public const string AttributeStageId = "stageid";

		public const string AttributeSystemUserId = "systemuserid";

		public const string AttributeId = "systemuserid";

		public const string AttributeTerritoryId = "territoryid";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeTitle = "title";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeTraversedPath = "traversedpath";

		public const string AttributeUserLicenseType = "userlicensetype";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeWindowsLiveID = "windowsliveid";

		public const string AttributeYammerEmailAddress = "yammeremailaddress";

		public const string AttributeYammerUserId = "yammeruserid";

		public const string AttributeYomiFirstName = "yomifirstname";

		public const string AttributeYomiFullName = "yomifullname";

		public const string AttributeYomiLastName = "yomilastname";

		public const string AttributeYomiMiddleName = "yomimiddlename";

		public const string AttributeReferencinglk_systemuser_createdonbehalfby = "createdonbehalfby";

		public const string AttributeReferencinglk_systemuser_modifiedonbehalfby = "modifiedonbehalfby";

		public const string AttributeReferencinglk_systemuserbase_createdby = "createdby";

		public const string AttributeReferencinglk_systemuserbase_modifiedby = "modifiedby";

		public const string AttributeReferencinguser_parent_user = "parentsystemuserid";

		[AttributeLogicalName("accessmode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue AccessMode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("accessmode");
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
				OnPropertyChanging("AccessMode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("accessmode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("accessmode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("AccessMode");
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

		[AttributeLogicalName("applicationid")]
		[ExcludeFromCodeCoverage]
		public Guid? ApplicationId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("applicationid");
			}
			set
			{
				OnPropertyChanging("ApplicationId");
				((Entity)this).SetAttributeValue("applicationid", (object)value);
				OnPropertyChanged("ApplicationId");
			}
		}

		[AttributeLogicalName("applicationiduri")]
		[ExcludeFromCodeCoverage]
		public string ApplicationIdUri => ((Entity)this).GetAttributeValue<string>("applicationiduri");

		[AttributeLogicalName("azureactivedirectoryobjectid")]
		[ExcludeFromCodeCoverage]
		public Guid? AzureActiveDirectoryObjectId => ((Entity)this).GetAttributeValue<Guid?>("azureactivedirectoryobjectid");

		[AttributeLogicalName("businessunitid")]
		[ExcludeFromCodeCoverage]
		public EntityReference BusinessUnitId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("businessunitid");
			}
			set
			{
				OnPropertyChanging("BusinessUnitId");
				((Entity)this).SetAttributeValue("businessunitid", (object)value);
				OnPropertyChanged("BusinessUnitId");
			}
		}

		[AttributeLogicalName("calendarid")]
		[ExcludeFromCodeCoverage]
		public EntityReference CalendarId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("calendarid");
			}
			set
			{
				OnPropertyChanging("CalendarId");
				((Entity)this).SetAttributeValue("calendarid", (object)value);
				OnPropertyChanged("CalendarId");
			}
		}

		[AttributeLogicalName("caltype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue CALType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("caltype");
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
				OnPropertyChanging("CALType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("caltype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("caltype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("CALType");
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

		[AttributeLogicalName("defaultfilterspopulated")]
		[ExcludeFromCodeCoverage]
		public bool? DefaultFiltersPopulated => ((Entity)this).GetAttributeValue<bool?>("defaultfilterspopulated");

		[AttributeLogicalName("defaultmailbox")]
		[ExcludeFromCodeCoverage]
		public EntityReference DefaultMailbox => ((Entity)this).GetAttributeValue<EntityReference>("defaultmailbox");

		[AttributeLogicalName("defaultodbfoldername")]
		[ExcludeFromCodeCoverage]
		public string DefaultOdbFolderName => ((Entity)this).GetAttributeValue<string>("defaultodbfoldername");

		[AttributeLogicalName("disabledreason")]
		[ExcludeFromCodeCoverage]
		public string DisabledReason => ((Entity)this).GetAttributeValue<string>("disabledreason");

		[AttributeLogicalName("displayinserviceviews")]
		[ExcludeFromCodeCoverage]
		public bool? DisplayInServiceViews
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("displayinserviceviews");
			}
			set
			{
				OnPropertyChanging("DisplayInServiceViews");
				((Entity)this).SetAttributeValue("displayinserviceviews", (object)value);
				OnPropertyChanged("DisplayInServiceViews");
			}
		}

		[AttributeLogicalName("domainname")]
		[ExcludeFromCodeCoverage]
		public string DomainName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("domainname");
			}
			set
			{
				OnPropertyChanging("DomainName");
				((Entity)this).SetAttributeValue("domainname", (object)value);
				OnPropertyChanged("DomainName");
			}
		}

		[AttributeLogicalName("emailrouteraccessapproval")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue EmailRouterAccessApproval
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("emailrouteraccessapproval");
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
				OnPropertyChanging("EmailRouterAccessApproval");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("emailrouteraccessapproval", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("emailrouteraccessapproval", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("EmailRouterAccessApproval");
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

		[AttributeLogicalName("fullname")]
		[ExcludeFromCodeCoverage]
		public string FullName => ((Entity)this).GetAttributeValue<string>("fullname");

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

		[AttributeLogicalName("homephone")]
		[ExcludeFromCodeCoverage]
		public string HomePhone
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("homephone");
			}
			set
			{
				OnPropertyChanging("HomePhone");
				((Entity)this).SetAttributeValue("homephone", (object)value);
				OnPropertyChanged("HomePhone");
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

		[AttributeLogicalName("incomingemaildeliverymethod")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue IncomingEmailDeliveryMethod
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("incomingemaildeliverymethod");
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
				OnPropertyChanging("IncomingEmailDeliveryMethod");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("incomingemaildeliverymethod", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("incomingemaildeliverymethod", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("IncomingEmailDeliveryMethod");
			}
		}

		[AttributeLogicalName("internalemailaddress")]
		[ExcludeFromCodeCoverage]
		public string InternalEMailAddress
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("internalemailaddress");
			}
			set
			{
				OnPropertyChanging("InternalEMailAddress");
				((Entity)this).SetAttributeValue("internalemailaddress", (object)value);
				OnPropertyChanged("InternalEMailAddress");
			}
		}

		[AttributeLogicalName("invitestatuscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue InviteStatusCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("invitestatuscode");
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
				OnPropertyChanging("InviteStatusCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("invitestatuscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("invitestatuscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("InviteStatusCode");
			}
		}

		[AttributeLogicalName("isdisabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsDisabled => ((Entity)this).GetAttributeValue<bool?>("isdisabled");

		[AttributeLogicalName("isemailaddressapprovedbyo365admin")]
		[ExcludeFromCodeCoverage]
		public bool? IsEmailAddressApprovedByO365Admin => ((Entity)this).GetAttributeValue<bool?>("isemailaddressapprovedbyo365admin");

		[AttributeLogicalName("isintegrationuser")]
		[ExcludeFromCodeCoverage]
		public bool? IsIntegrationUser
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isintegrationuser");
			}
			set
			{
				OnPropertyChanging("IsIntegrationUser");
				((Entity)this).SetAttributeValue("isintegrationuser", (object)value);
				OnPropertyChanged("IsIntegrationUser");
			}
		}

		[AttributeLogicalName("islicensed")]
		[ExcludeFromCodeCoverage]
		public bool? IsLicensed
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("islicensed");
			}
			set
			{
				OnPropertyChanging("IsLicensed");
				((Entity)this).SetAttributeValue("islicensed", (object)value);
				OnPropertyChanged("IsLicensed");
			}
		}

		[AttributeLogicalName("issyncwithdirectory")]
		[ExcludeFromCodeCoverage]
		public bool? IsSyncWithDirectory
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("issyncwithdirectory");
			}
			set
			{
				OnPropertyChanging("IsSyncWithDirectory");
				((Entity)this).SetAttributeValue("issyncwithdirectory", (object)value);
				OnPropertyChanged("IsSyncWithDirectory");
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

		[AttributeLogicalName("mobilealertemail")]
		[ExcludeFromCodeCoverage]
		public string MobileAlertEMail
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("mobilealertemail");
			}
			set
			{
				OnPropertyChanging("MobileAlertEMail");
				((Entity)this).SetAttributeValue("mobilealertemail", (object)value);
				OnPropertyChanged("MobileAlertEMail");
			}
		}

		[AttributeLogicalName("mobileofflineprofileid")]
		[ExcludeFromCodeCoverage]
		public EntityReference MobileOfflineProfileId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("mobileofflineprofileid");
			}
			set
			{
				OnPropertyChanging("MobileOfflineProfileId");
				((Entity)this).SetAttributeValue("mobileofflineprofileid", (object)value);
				OnPropertyChanged("MobileOfflineProfileId");
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

		[AttributeLogicalName("organizationid")]
		[ExcludeFromCodeCoverage]
		public Guid? OrganizationId => ((Entity)this).GetAttributeValue<Guid?>("organizationid");

		[AttributeLogicalName("outgoingemaildeliverymethod")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue OutgoingEmailDeliveryMethod
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("outgoingemaildeliverymethod");
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
				OnPropertyChanging("OutgoingEmailDeliveryMethod");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("outgoingemaildeliverymethod", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("outgoingemaildeliverymethod", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("OutgoingEmailDeliveryMethod");
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

		[AttributeLogicalName("parentsystemuserid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ParentSystemUserId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("parentsystemuserid");
			}
			set
			{
				OnPropertyChanging("ParentSystemUserId");
				((Entity)this).SetAttributeValue("parentsystemuserid", (object)value);
				OnPropertyChanged("ParentSystemUserId");
			}
		}

		[AttributeLogicalName("passporthi")]
		[ExcludeFromCodeCoverage]
		public int? PassportHi
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("passporthi");
			}
			set
			{
				OnPropertyChanging("PassportHi");
				((Entity)this).SetAttributeValue("passporthi", (object)value);
				OnPropertyChanged("PassportHi");
			}
		}

		[AttributeLogicalName("passportlo")]
		[ExcludeFromCodeCoverage]
		public int? PassportLo
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("passportlo");
			}
			set
			{
				OnPropertyChanging("PassportLo");
				((Entity)this).SetAttributeValue("passportlo", (object)value);
				OnPropertyChanged("PassportLo");
			}
		}

		[AttributeLogicalName("personalemailaddress")]
		[ExcludeFromCodeCoverage]
		public string PersonalEMailAddress
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("personalemailaddress");
			}
			set
			{
				OnPropertyChanging("PersonalEMailAddress");
				((Entity)this).SetAttributeValue("personalemailaddress", (object)value);
				OnPropertyChanged("PersonalEMailAddress");
			}
		}

		[AttributeLogicalName("photourl")]
		[ExcludeFromCodeCoverage]
		public string PhotoUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("photourl");
			}
			set
			{
				OnPropertyChanging("PhotoUrl");
				((Entity)this).SetAttributeValue("photourl", (object)value);
				OnPropertyChanged("PhotoUrl");
			}
		}

		[AttributeLogicalName("positionid")]
		[ExcludeFromCodeCoverage]
		public EntityReference PositionId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("positionid");
			}
			set
			{
				OnPropertyChanging("PositionId");
				((Entity)this).SetAttributeValue("positionid", (object)value);
				OnPropertyChanged("PositionId");
			}
		}

		[AttributeLogicalName("preferredaddresscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PreferredAddressCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("preferredaddresscode");
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
				OnPropertyChanging("PreferredAddressCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("preferredaddresscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("preferredaddresscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PreferredAddressCode");
			}
		}

		[AttributeLogicalName("preferredemailcode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PreferredEmailCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("preferredemailcode");
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
				OnPropertyChanging("PreferredEmailCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("preferredemailcode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("preferredemailcode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PreferredEmailCode");
			}
		}

		[AttributeLogicalName("preferredphonecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PreferredPhoneCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("preferredphonecode");
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
				OnPropertyChanging("PreferredPhoneCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("preferredphonecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("preferredphonecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PreferredPhoneCode");
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

		[AttributeLogicalName("queueid")]
		[ExcludeFromCodeCoverage]
		public EntityReference QueueId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("queueid");
			}
			set
			{
				OnPropertyChanging("QueueId");
				((Entity)this).SetAttributeValue("queueid", (object)value);
				OnPropertyChanged("QueueId");
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

		[AttributeLogicalName("setupuser")]
		[ExcludeFromCodeCoverage]
		public bool? SetupUser
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("setupuser");
			}
			set
			{
				OnPropertyChanging("SetupUser");
				((Entity)this).SetAttributeValue("setupuser", (object)value);
				OnPropertyChanged("SetupUser");
			}
		}

		[AttributeLogicalName("sharepointemailaddress")]
		[ExcludeFromCodeCoverage]
		public string SharePointEmailAddress
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("sharepointemailaddress");
			}
			set
			{
				OnPropertyChanging("SharePointEmailAddress");
				((Entity)this).SetAttributeValue("sharepointemailaddress", (object)value);
				OnPropertyChanged("SharePointEmailAddress");
			}
		}

		[AttributeLogicalName("siteid")]
		[ExcludeFromCodeCoverage]
		public EntityReference SiteId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("siteid");
			}
			set
			{
				OnPropertyChanging("SiteId");
				((Entity)this).SetAttributeValue("siteid", (object)value);
				OnPropertyChanged("SiteId");
			}
		}

		[AttributeLogicalName("skills")]
		[ExcludeFromCodeCoverage]
		public string Skills
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("skills");
			}
			set
			{
				OnPropertyChanging("Skills");
				((Entity)this).SetAttributeValue("skills", (object)value);
				OnPropertyChanged("Skills");
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

		[AttributeLogicalName("title")]
		[ExcludeFromCodeCoverage]
		public string Title
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("title");
			}
			set
			{
				OnPropertyChanging("Title");
				((Entity)this).SetAttributeValue("title", (object)value);
				OnPropertyChanged("Title");
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

		[AttributeLogicalName("userlicensetype")]
		[ExcludeFromCodeCoverage]
		public int? UserLicenseType
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("userlicensetype");
			}
			set
			{
				OnPropertyChanging("UserLicenseType");
				((Entity)this).SetAttributeValue("userlicensetype", (object)value);
				OnPropertyChanged("UserLicenseType");
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

		[AttributeLogicalName("windowsliveid")]
		[ExcludeFromCodeCoverage]
		public string WindowsLiveID
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("windowsliveid");
			}
			set
			{
				OnPropertyChanging("WindowsLiveID");
				((Entity)this).SetAttributeValue("windowsliveid", (object)value);
				OnPropertyChanged("WindowsLiveID");
			}
		}

		[AttributeLogicalName("yammeremailaddress")]
		[ExcludeFromCodeCoverage]
		public string YammerEmailAddress
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("yammeremailaddress");
			}
			set
			{
				OnPropertyChanging("YammerEmailAddress");
				((Entity)this).SetAttributeValue("yammeremailaddress", (object)value);
				OnPropertyChanged("YammerEmailAddress");
			}
		}

		[AttributeLogicalName("yammeruserid")]
		[ExcludeFromCodeCoverage]
		public string YammerUserId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("yammeruserid");
			}
			set
			{
				OnPropertyChanging("YammerUserId");
				((Entity)this).SetAttributeValue("yammeruserid", (object)value);
				OnPropertyChanged("YammerUserId");
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

		[RelationshipSchemaName("lk_product_createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Product> lk_product_createdonbehalfby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Product>("lk_product_createdonbehalfby", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("lk_product_createdonbehalfby");
				((Entity)this).SetRelatedEntities<Product>("lk_product_createdonbehalfby", (EntityRole?)null, value);
				OnPropertyChanged("lk_product_createdonbehalfby");
			}
		}

		[RelationshipSchemaName("lk_product_modifiedonbehalfby")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Product> lk_product_modifiedonbehalfby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Product>("lk_product_modifiedonbehalfby", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("lk_product_modifiedonbehalfby");
				((Entity)this).SetRelatedEntities<Product>("lk_product_modifiedonbehalfby", (EntityRole?)null, value);
				OnPropertyChanged("lk_product_modifiedonbehalfby");
			}
		}

		[RelationshipSchemaName("lk_productbase_createdby")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Product> lk_productbase_createdby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Product>("lk_productbase_createdby", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("lk_productbase_createdby");
				((Entity)this).SetRelatedEntities<Product>("lk_productbase_createdby", (EntityRole?)null, value);
				OnPropertyChanged("lk_productbase_createdby");
			}
		}

		[RelationshipSchemaName("lk_productbase_modifiedby")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Product> lk_productbase_modifiedby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Product>("lk_productbase_modifiedby", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("lk_productbase_modifiedby");
				((Entity)this).SetRelatedEntities<Product>("lk_productbase_modifiedby", (EntityRole?)null, value);
				OnPropertyChanged("lk_productbase_modifiedby");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<User> Referencedlk_systemuser_modifiedonbehalfby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<User>("lk_systemuser_modifiedonbehalfby", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedlk_systemuser_modifiedonbehalfby");
				((Entity)this).SetRelatedEntities<User>("lk_systemuser_modifiedonbehalfby", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedlk_systemuser_modifiedonbehalfby");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<User> Referencedlk_systemuserbase_createdby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<User>("lk_systemuserbase_createdby", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedlk_systemuserbase_createdby");
				((Entity)this).SetRelatedEntities<User>("lk_systemuserbase_createdby", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedlk_systemuserbase_createdby");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<User> Referencedlk_systemuserbase_modifiedby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<User>("lk_systemuserbase_modifiedby", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedlk_systemuserbase_modifiedby");
				((Entity)this).SetRelatedEntities<User>("lk_systemuserbase_modifiedby", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedlk_systemuserbase_modifiedby");
			}
		}

		[RelationshipSchemaName("lk_uom_createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Unit> lk_uom_createdonbehalfby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Unit>("lk_uom_createdonbehalfby", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("lk_uom_createdonbehalfby");
				((Entity)this).SetRelatedEntities<Unit>("lk_uom_createdonbehalfby", (EntityRole?)null, value);
				OnPropertyChanged("lk_uom_createdonbehalfby");
			}
		}

		[RelationshipSchemaName("lk_uom_modifiedonbehalfby")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Unit> lk_uom_modifiedonbehalfby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Unit>("lk_uom_modifiedonbehalfby", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("lk_uom_modifiedonbehalfby");
				((Entity)this).SetRelatedEntities<Unit>("lk_uom_modifiedonbehalfby", (EntityRole?)null, value);
				OnPropertyChanged("lk_uom_modifiedonbehalfby");
			}
		}

		[RelationshipSchemaName("lk_uombase_createdby")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Unit> lk_uombase_createdby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Unit>("lk_uombase_createdby", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("lk_uombase_createdby");
				((Entity)this).SetRelatedEntities<Unit>("lk_uombase_createdby", (EntityRole?)null, value);
				OnPropertyChanged("lk_uombase_createdby");
			}
		}

		[RelationshipSchemaName("lk_uombase_modifiedby")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Unit> lk_uombase_modifiedby
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Unit>("lk_uombase_modifiedby", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("lk_uombase_modifiedby");
				((Entity)this).SetRelatedEntities<Unit>("lk_uombase_modifiedby", (EntityRole?)null, value);
				OnPropertyChanged("lk_uombase_modifiedby");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<User> Referenceduser_parent_user
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<User>("user_parent_user", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referenceduser_parent_user");
				((Entity)this).SetRelatedEntities<User>("user_parent_user", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referenceduser_parent_user");
			}
		}

		[AttributeLogicalName("createdonbehalfby")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public User Referencinglk_systemuser_createdonbehalfby => ((Entity)this).GetRelatedEntity<User>("lk_systemuser_createdonbehalfby", (EntityRole?)(EntityRole)0);

		[AttributeLogicalName("modifiedonbehalfby")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public User Referencinglk_systemuser_modifiedonbehalfby => ((Entity)this).GetRelatedEntity<User>("lk_systemuser_modifiedonbehalfby", (EntityRole?)(EntityRole)0);

		[AttributeLogicalName("createdby")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public User Referencinglk_systemuserbase_createdby => ((Entity)this).GetRelatedEntity<User>("lk_systemuserbase_createdby", (EntityRole?)(EntityRole)0);

		[AttributeLogicalName("modifiedby")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public User Referencinglk_systemuserbase_modifiedby => ((Entity)this).GetRelatedEntity<User>("lk_systemuserbase_modifiedby", (EntityRole?)(EntityRole)0);

		[AttributeLogicalName("parentsystemuserid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public User Referencinguser_parent_user
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<User>("user_parent_user", (EntityRole?)(EntityRole)0);
			}
			set
			{
				OnPropertyChanging("Referencinguser_parent_user");
				((Entity)this).SetRelatedEntity<User>("user_parent_user", (EntityRole?)(EntityRole)0, value);
				OnPropertyChanged("Referencinguser_parent_user");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public User()
			: this("systemuser")
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
