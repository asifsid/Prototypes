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
	[EntityLogicalName("businessunit")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class BusinessUnit : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "businessunit";

		public const int EntityTypeCode = 10;

		public const int AttributeAddress2_Line2_MaxLength = 250;

		public const int AttributeAddress1_County_MaxLength = 50;

		public const int AttributeAddress2_Telephone1_MaxLength = 50;

		public const int AttributeAddress2_Telephone2_MaxLength = 50;

		public const int AttributeAddress2_Telephone3_MaxLength = 50;

		public const int AttributeFtpSiteUrl_MaxLength = 200;

		public const int AttributeAddress2_Name_MaxLength = 100;

		public const int AttributeWebSiteUrl_MaxLength = 200;

		public const int AttributeAddress2_Country_MaxLength = 80;

		public const int AttributeAddress1_Line1_MaxLength = 250;

		public const int AttributeAddress1_StateOrProvince_MaxLength = 50;

		public const int AttributeAddress2_PostalCode_MaxLength = 20;

		public const int AttributeAddress2_County_MaxLength = 50;

		public const int AttributeAddress1_Country_MaxLength = 80;

		public const int AttributeAddress1_PostalCode_MaxLength = 20;

		public const int AttributeAddress1_Fax_MaxLength = 50;

		public const int AttributeAddress1_Line3_MaxLength = 250;

		public const int AttributeEMailAddress_MaxLength = 100;

		public const int AttributeAddress2_Line1_MaxLength = 250;

		public const int AttributeAddress2_UPSZone_MaxLength = 4;

		public const int AttributeAddress2_PostOfficeBox_MaxLength = 20;

		public const int AttributeAddress1_City_MaxLength = 80;

		public const int AttributeTickerSymbol_MaxLength = 10;

		public const int AttributeAddress1_Telephone1_MaxLength = 50;

		public const int AttributeAddress1_Telephone2_MaxLength = 50;

		public const int AttributeAddress1_Telephone3_MaxLength = 50;

		public const int AttributeAddress2_StateOrProvince_MaxLength = 50;

		public const int AttributeAddress1_Name_MaxLength = 100;

		public const int AttributeDisabledReason_MaxLength = 500;

		public const int AttributeAddress1_Line2_MaxLength = 250;

		public const int AttributeAddress2_City_MaxLength = 80;

		public const int AttributeAddress2_Fax_MaxLength = 50;

		public const int AttributeStockExchange_MaxLength = 20;

		public const int AttributeDivisionName_MaxLength = 100;

		public const int AttributeName_MaxLength = 160;

		public const int AttributeAddress1_UPSZone_MaxLength = 4;

		public const int AttributeAddress2_Line3_MaxLength = 250;

		public const int AttributeFileAsName_MaxLength = 100;

		public const int AttributeCostCenter_MaxLength = 100;

		public const int AttributeAddress1_PostOfficeBox_MaxLength = 20;

		public const string AttributeAddress1_AddressId = "address1_addressid";

		public const string AttributeAddress1_AddressTypeCode = "address1_addresstypecode";

		public const string AttributeAddress1_City = "address1_city";

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

		public const string AttributeBusinessUnitId = "businessunitid";

		public const string AttributeId = "businessunitid";

		public const string AttributeCalendarId = "calendarid";

		public const string AttributeCostCenter = "costcenter";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeCreditLimit = "creditlimit";

		public const string AttributeDescription = "description";

		public const string AttributeDisabledReason = "disabledreason";

		public const string AttributeDivisionName = "divisionname";

		public const string AttributeEMailAddress = "emailaddress";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeFileAsName = "fileasname";

		public const string AttributeFtpSiteUrl = "ftpsiteurl";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeInheritanceMask = "inheritancemask";

		public const string AttributeIsDisabled = "isdisabled";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeParentBusinessUnitId = "parentbusinessunitid";

		public const string AttributePicture = "picture";

		public const string AttributeStockExchange = "stockexchange";

		public const string AttributeTickerSymbol = "tickersymbol";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeUTCOffset = "utcoffset";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeWebSiteUrl = "websiteurl";

		public const string AttributeWorkflowSuspended = "workflowsuspended";

		public const string AttributeReferencingbusiness_unit_parent_business_unit = "parentbusinessunitid";

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
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("BusinessUnitId");
			}
		}

		[AttributeLogicalName("businessunitid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				BusinessUnitId = value;
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

		[AttributeLogicalName("costcenter")]
		[ExcludeFromCodeCoverage]
		public string CostCenter
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("costcenter");
			}
			set
			{
				OnPropertyChanging("CostCenter");
				((Entity)this).SetAttributeValue("costcenter", (object)value);
				OnPropertyChanged("CostCenter");
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

		[AttributeLogicalName("creditlimit")]
		[ExcludeFromCodeCoverage]
		public double? CreditLimit
		{
			get
			{
				return ((Entity)this).GetAttributeValue<double?>("creditlimit");
			}
			set
			{
				OnPropertyChanging("CreditLimit");
				((Entity)this).SetAttributeValue("creditlimit", (object)value);
				OnPropertyChanged("CreditLimit");
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

		[AttributeLogicalName("disabledreason")]
		[ExcludeFromCodeCoverage]
		public string DisabledReason => ((Entity)this).GetAttributeValue<string>("disabledreason");

		[AttributeLogicalName("divisionname")]
		[ExcludeFromCodeCoverage]
		public string DivisionName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("divisionname");
			}
			set
			{
				OnPropertyChanging("DivisionName");
				((Entity)this).SetAttributeValue("divisionname", (object)value);
				OnPropertyChanged("DivisionName");
			}
		}

		[AttributeLogicalName("emailaddress")]
		[ExcludeFromCodeCoverage]
		public string EMailAddress
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("emailaddress");
			}
			set
			{
				OnPropertyChanging("EMailAddress");
				((Entity)this).SetAttributeValue("emailaddress", (object)value);
				OnPropertyChanged("EMailAddress");
			}
		}

		[AttributeLogicalName("exchangerate")]
		[ExcludeFromCodeCoverage]
		public decimal? ExchangeRate => ((Entity)this).GetAttributeValue<decimal?>("exchangerate");

		[AttributeLogicalName("fileasname")]
		[ExcludeFromCodeCoverage]
		public string FileAsName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("fileasname");
			}
			set
			{
				OnPropertyChanging("FileAsName");
				((Entity)this).SetAttributeValue("fileasname", (object)value);
				OnPropertyChanged("FileAsName");
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

		[AttributeLogicalName("inheritancemask")]
		[ExcludeFromCodeCoverage]
		public int? InheritanceMask
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("inheritancemask");
			}
			set
			{
				OnPropertyChanging("InheritanceMask");
				((Entity)this).SetAttributeValue("inheritancemask", (object)value);
				OnPropertyChanged("InheritanceMask");
			}
		}

		[AttributeLogicalName("isdisabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsDisabled => ((Entity)this).GetAttributeValue<bool?>("isdisabled");

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

		[AttributeLogicalName("organizationid")]
		[ExcludeFromCodeCoverage]
		public EntityReference OrganizationId => ((Entity)this).GetAttributeValue<EntityReference>("organizationid");

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

		[AttributeLogicalName("parentbusinessunitid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ParentBusinessUnitId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("parentbusinessunitid");
			}
			set
			{
				OnPropertyChanging("ParentBusinessUnitId");
				((Entity)this).SetAttributeValue("parentbusinessunitid", (object)value);
				OnPropertyChanged("ParentBusinessUnitId");
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

		[AttributeLogicalName("utcoffset")]
		[ExcludeFromCodeCoverage]
		public int? UTCOffset
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("utcoffset");
			}
			set
			{
				OnPropertyChanging("UTCOffset");
				((Entity)this).SetAttributeValue("utcoffset", (object)value);
				OnPropertyChanged("UTCOffset");
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

		[AttributeLogicalName("workflowsuspended")]
		[ExcludeFromCodeCoverage]
		public bool? WorkflowSuspended
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("workflowsuspended");
			}
			set
			{
				OnPropertyChanging("WorkflowSuspended");
				((Entity)this).SetAttributeValue("workflowsuspended", (object)value);
				OnPropertyChanged("WorkflowSuspended");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<BusinessUnit> Referencedbusiness_unit_parent_business_unit
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<BusinessUnit>("business_unit_parent_business_unit", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedbusiness_unit_parent_business_unit");
				((Entity)this).SetRelatedEntities<BusinessUnit>("business_unit_parent_business_unit", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedbusiness_unit_parent_business_unit");
			}
		}

		[AttributeLogicalName("parentbusinessunitid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public BusinessUnit Referencingbusiness_unit_parent_business_unit
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<BusinessUnit>("business_unit_parent_business_unit", (EntityRole?)(EntityRole)0);
			}
			set
			{
				OnPropertyChanging("Referencingbusiness_unit_parent_business_unit");
				((Entity)this).SetRelatedEntity<BusinessUnit>("business_unit_parent_business_unit", (EntityRole?)(EntityRole)0, value);
				OnPropertyChanged("Referencingbusiness_unit_parent_business_unit");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public BusinessUnit()
			: this("businessunit")
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
