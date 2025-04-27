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
	[EntityLogicalName("publisher")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Publisher : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "publisher";

		public const int EntityTypeCode = 7101;

		public const int AttributeAddress2_Line2_MaxLength = 50;

		public const int AttributeUniqueName_MaxLength = 256;

		public const int AttributeAddress2_Telephone1_MaxLength = 50;

		public const int AttributeAddress2_Telephone2_MaxLength = 50;

		public const int AttributeAddress2_Telephone3_MaxLength = 50;

		public const int AttributeAddress2_Fax_MaxLength = 50;

		public const int AttributeAddress2_Name_MaxLength = 100;

		public const int AttributeAddress2_Country_MaxLength = 80;

		public const int AttributeAddress1_StateOrProvince_MaxLength = 50;

		public const int AttributeAddress2_PostalCode_MaxLength = 20;

		public const int AttributeAddress2_County_MaxLength = 50;

		public const int AttributeAddress1_Country_MaxLength = 80;

		public const int AttributeAddress1_PostalCode_MaxLength = 20;

		public const int AttributeAddress1_Telephone2_MaxLength = 50;

		public const int AttributeFriendlyName_MaxLength = 256;

		public const int AttributeAddress1_Line3_MaxLength = 50;

		public const int AttributeEMailAddress_MaxLength = 100;

		public const int AttributeAddress2_Line1_MaxLength = 50;

		public const int AttributeAddress1_Fax_MaxLength = 50;

		public const int AttributeAddress1_Line1_MaxLength = 50;

		public const int AttributeAddress2_PostOfficeBox_MaxLength = 20;

		public const int AttributeAddress1_City_MaxLength = 80;

		public const int AttributeAddress1_County_MaxLength = 50;

		public const int AttributeAddress1_Telephone1_MaxLength = 50;

		public const int AttributeAddress1_Telephone3_MaxLength = 50;

		public const int AttributeAddress2_StateOrProvince_MaxLength = 50;

		public const int AttributeSupportingWebsiteUrl_MaxLength = 200;

		public const int AttributeDescription_MaxLength = 2000;

		public const int AttributeCustomizationPrefix_MaxLength = 8;

		public const int AttributeAddress1_Name_MaxLength = 100;

		public const int AttributeAddress1_Line2_MaxLength = 50;

		public const int AttributeAddress2_City_MaxLength = 80;

		public const int AttributeAddress2_UPSZone_MaxLength = 4;

		public const int AttributeEntityImage_URL_MaxLength = 200;

		public const int AttributeAddress1_UPSZone_MaxLength = 4;

		public const int AttributeAddress2_Line3_MaxLength = 50;

		public const int AttributePinpointPublisherDefaultLocale_MaxLength = 16;

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

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeCustomizationOptionValuePrefix = "customizationoptionvalueprefix";

		public const string AttributeCustomizationPrefix = "customizationprefix";

		public const string AttributeDescription = "description";

		public const string AttributeEMailAddress = "emailaddress";

		public const string AttributeEntityImage = "entityimage";

		public const string AttributeEntityImage_Timestamp = "entityimage_timestamp";

		public const string AttributeEntityImage_URL = "entityimage_url";

		public const string AttributeEntityImageId = "entityimageid";

		public const string AttributeFriendlyName = "friendlyname";

		public const string AttributeIsReadonly = "isreadonly";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributePinpointPublisherDefaultLocale = "pinpointpublisherdefaultlocale";

		public const string AttributePinpointPublisherId = "pinpointpublisherid";

		public const string AttributePublisherId = "publisherid";

		public const string AttributeId = "publisherid";

		public const string AttributeSupportingWebsiteUrl = "supportingwebsiteurl";

		public const string AttributeUniqueName = "uniquename";

		public const string AttributeVersionNumber = "versionnumber";

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

		[AttributeLogicalName("createdby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedBy => ((Entity)this).GetAttributeValue<EntityReference>("createdby");

		[AttributeLogicalName("createdon")]
		[ExcludeFromCodeCoverage]
		public DateTime? CreatedOn => ((Entity)this).GetAttributeValue<DateTime?>("createdon");

		[AttributeLogicalName("createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("createdonbehalfby");

		[AttributeLogicalName("customizationoptionvalueprefix")]
		[ExcludeFromCodeCoverage]
		public int? CustomizationOptionValuePrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("customizationoptionvalueprefix");
			}
			set
			{
				OnPropertyChanging("CustomizationOptionValuePrefix");
				((Entity)this).SetAttributeValue("customizationoptionvalueprefix", (object)value);
				OnPropertyChanged("CustomizationOptionValuePrefix");
			}
		}

		[AttributeLogicalName("customizationprefix")]
		[ExcludeFromCodeCoverage]
		public string CustomizationPrefix
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("customizationprefix");
			}
			set
			{
				OnPropertyChanging("CustomizationPrefix");
				((Entity)this).SetAttributeValue("customizationprefix", (object)value);
				OnPropertyChanged("CustomizationPrefix");
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

		[AttributeLogicalName("friendlyname")]
		[ExcludeFromCodeCoverage]
		public string FriendlyName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("friendlyname");
			}
			set
			{
				OnPropertyChanging("FriendlyName");
				((Entity)this).SetAttributeValue("friendlyname", (object)value);
				OnPropertyChanged("FriendlyName");
			}
		}

		[AttributeLogicalName("isreadonly")]
		[ExcludeFromCodeCoverage]
		public bool? IsReadonly => ((Entity)this).GetAttributeValue<bool?>("isreadonly");

		[AttributeLogicalName("modifiedby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedby");

		[AttributeLogicalName("modifiedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? ModifiedOn => ((Entity)this).GetAttributeValue<DateTime?>("modifiedon");

		[AttributeLogicalName("modifiedonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedonbehalfby");

		[AttributeLogicalName("organizationid")]
		[ExcludeFromCodeCoverage]
		public EntityReference OrganizationId => ((Entity)this).GetAttributeValue<EntityReference>("organizationid");

		[AttributeLogicalName("pinpointpublisherdefaultlocale")]
		[ExcludeFromCodeCoverage]
		public string PinpointPublisherDefaultLocale => ((Entity)this).GetAttributeValue<string>("pinpointpublisherdefaultlocale");

		[AttributeLogicalName("pinpointpublisherid")]
		[ExcludeFromCodeCoverage]
		public long? PinpointPublisherId => ((Entity)this).GetAttributeValue<long?>("pinpointpublisherid");

		[AttributeLogicalName("publisherid")]
		[ExcludeFromCodeCoverage]
		public Guid? PublisherId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("publisherid");
			}
			set
			{
				OnPropertyChanging("PublisherId");
				((Entity)this).SetAttributeValue("publisherid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("PublisherId");
			}
		}

		[AttributeLogicalName("publisherid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				PublisherId = value;
			}
		}

		[AttributeLogicalName("supportingwebsiteurl")]
		[ExcludeFromCodeCoverage]
		public string SupportingWebsiteUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("supportingwebsiteurl");
			}
			set
			{
				OnPropertyChanging("SupportingWebsiteUrl");
				((Entity)this).SetAttributeValue("supportingwebsiteurl", (object)value);
				OnPropertyChanged("SupportingWebsiteUrl");
			}
		}

		[AttributeLogicalName("uniquename")]
		[ExcludeFromCodeCoverage]
		public string UniqueName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("uniquename");
			}
			set
			{
				OnPropertyChanging("UniqueName");
				((Entity)this).SetAttributeValue("uniquename", (object)value);
				OnPropertyChanged("UniqueName");
			}
		}

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Publisher()
			: this("publisher")
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
