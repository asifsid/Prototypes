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
	[EntityLogicalName("product")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Product : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "product";

		public const int EntityTypeCode = 1024;

		public const int AttributeProductUrl_MaxLength = 255;

		public const int AttributeSize_MaxLength = 200;

		public const int AttributeTraversedPath_MaxLength = 1250;

		public const int AttributeVendorName_MaxLength = 100;

		public const int AttributeVendorPartNumber_MaxLength = 100;

		public const int AttributeVendorID_MaxLength = 100;

		public const int AttributeHierarchyPath_MaxLength = 450;

		public const int AttributeSupplierName_MaxLength = 100;

		public const int AttributeName_MaxLength = 100;

		public const int AttributeEntityImage_URL_MaxLength = 200;

		public const int AttributeProductNumber_MaxLength = 100;

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedByExternalParty = "createdbyexternalparty";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeCurrentCost = "currentcost";

		public const string AttributeCurrentCost_Base = "currentcost_base";

		public const string AttributeDefaultUoMId = "defaultuomid";

		public const string AttributeDefaultUoMScheduleId = "defaultuomscheduleid";

		public const string AttributeDescription = "description";

		public const string AttributeDMTImportState = "dmtimportstate";

		public const string AttributeEntityImage = "entityimage";

		public const string AttributeEntityImage_Timestamp = "entityimage_timestamp";

		public const string AttributeEntityImage_URL = "entityimage_url";

		public const string AttributeEntityImageId = "entityimageid";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeHierarchyPath = "hierarchypath";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIsKit = "iskit";

		public const string AttributeIsStockItem = "isstockitem";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedByExternalParty = "modifiedbyexternalparty";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeParentProductId = "parentproductid";

		public const string AttributePrice = "price";

		public const string AttributePrice_Base = "price_base";

		public const string AttributePriceLevelId = "pricelevelid";

		public const string AttributeProcessId = "processid";

		public const string AttributeProductId = "productid";

		public const string AttributeId = "productid";

		public const string AttributeProductNumber = "productnumber";

		public const string AttributeProductStructure = "productstructure";

		public const string AttributeProductTypeCode = "producttypecode";

		public const string AttributeProductUrl = "producturl";

		public const string AttributeQuantityDecimal = "quantitydecimal";

		public const string AttributeQuantityOnHand = "quantityonhand";

		public const string AttributeSize = "size";

		public const string AttributeStageId = "stageid";

		public const string AttributeStandardCost = "standardcost";

		public const string AttributeStandardCost_Base = "standardcost_base";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeStockVolume = "stockvolume";

		public const string AttributeStockWeight = "stockweight";

		public const string AttributeSubjectId = "subjectid";

		public const string AttributeSupplierName = "suppliername";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeTraversedPath = "traversedpath";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeValidFromDate = "validfromdate";

		public const string AttributeValidToDate = "validtodate";

		public const string AttributeVendorID = "vendorid";

		public const string AttributeVendorName = "vendorname";

		public const string AttributeVendorPartNumber = "vendorpartnumber";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeReferencingproduct_parent_product = "parentproductid";

		public const string AttributeUnit_of_measurement_products = "defaultuomid";

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

		[AttributeLogicalName("currentcost")]
		[ExcludeFromCodeCoverage]
		public Money CurrentCost
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Money>("currentcost");
			}
			set
			{
				OnPropertyChanging("CurrentCost");
				((Entity)this).SetAttributeValue("currentcost", (object)value);
				OnPropertyChanged("CurrentCost");
			}
		}

		[AttributeLogicalName("currentcost_base")]
		[ExcludeFromCodeCoverage]
		public Money CurrentCost_Base => ((Entity)this).GetAttributeValue<Money>("currentcost_base");

		[AttributeLogicalName("defaultuomid")]
		[ExcludeFromCodeCoverage]
		public EntityReference DefaultUoMId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("defaultuomid");
			}
			set
			{
				OnPropertyChanging("DefaultUoMId");
				((Entity)this).SetAttributeValue("defaultuomid", (object)value);
				OnPropertyChanged("DefaultUoMId");
			}
		}

		[AttributeLogicalName("defaultuomscheduleid")]
		[ExcludeFromCodeCoverage]
		public EntityReference DefaultUoMScheduleId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("defaultuomscheduleid");
			}
			set
			{
				OnPropertyChanging("DefaultUoMScheduleId");
				((Entity)this).SetAttributeValue("defaultuomscheduleid", (object)value);
				OnPropertyChanged("DefaultUoMScheduleId");
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

		[AttributeLogicalName("dmtimportstate")]
		[ExcludeFromCodeCoverage]
		public int? DMTImportState
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("dmtimportstate");
			}
			set
			{
				OnPropertyChanging("DMTImportState");
				((Entity)this).SetAttributeValue("dmtimportstate", (object)value);
				OnPropertyChanged("DMTImportState");
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

		[AttributeLogicalName("hierarchypath")]
		[ExcludeFromCodeCoverage]
		public string HierarchyPath => ((Entity)this).GetAttributeValue<string>("hierarchypath");

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

		[AttributeLogicalName("iskit")]
		[ExcludeFromCodeCoverage]
		public bool? IsKit
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("iskit");
			}
			set
			{
				OnPropertyChanging("IsKit");
				((Entity)this).SetAttributeValue("iskit", (object)value);
				OnPropertyChanged("IsKit");
			}
		}

		[AttributeLogicalName("isstockitem")]
		[ExcludeFromCodeCoverage]
		public bool? IsStockItem
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isstockitem");
			}
			set
			{
				OnPropertyChanging("IsStockItem");
				((Entity)this).SetAttributeValue("isstockitem", (object)value);
				OnPropertyChanged("IsStockItem");
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

		[AttributeLogicalName("parentproductid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ParentProductId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("parentproductid");
			}
			set
			{
				OnPropertyChanging("ParentProductId");
				((Entity)this).SetAttributeValue("parentproductid", (object)value);
				OnPropertyChanged("ParentProductId");
			}
		}

		[AttributeLogicalName("price")]
		[ExcludeFromCodeCoverage]
		public Money Price
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Money>("price");
			}
			set
			{
				OnPropertyChanging("Price");
				((Entity)this).SetAttributeValue("price", (object)value);
				OnPropertyChanged("Price");
			}
		}

		[AttributeLogicalName("price_base")]
		[ExcludeFromCodeCoverage]
		public Money Price_Base => ((Entity)this).GetAttributeValue<Money>("price_base");

		[AttributeLogicalName("pricelevelid")]
		[ExcludeFromCodeCoverage]
		public EntityReference PriceLevelId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("pricelevelid");
			}
			set
			{
				OnPropertyChanging("PriceLevelId");
				((Entity)this).SetAttributeValue("pricelevelid", (object)value);
				OnPropertyChanged("PriceLevelId");
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

		[AttributeLogicalName("productid")]
		[ExcludeFromCodeCoverage]
		public Guid? ProductId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("productid");
			}
			set
			{
				OnPropertyChanging("ProductId");
				((Entity)this).SetAttributeValue("productid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("ProductId");
			}
		}

		[AttributeLogicalName("productid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				ProductId = value;
			}
		}

		[AttributeLogicalName("productnumber")]
		[ExcludeFromCodeCoverage]
		public string ProductNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("productnumber");
			}
			set
			{
				OnPropertyChanging("ProductNumber");
				((Entity)this).SetAttributeValue("productnumber", (object)value);
				OnPropertyChanged("ProductNumber");
			}
		}

		[AttributeLogicalName("productstructure")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ProductStructure
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("productstructure");
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
				OnPropertyChanging("ProductStructure");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("productstructure", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("productstructure", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ProductStructure");
			}
		}

		[AttributeLogicalName("producttypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ProductTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("producttypecode");
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
				OnPropertyChanging("ProductTypeCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("producttypecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("producttypecode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ProductTypeCode");
			}
		}

		[AttributeLogicalName("producturl")]
		[ExcludeFromCodeCoverage]
		public string ProductUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("producturl");
			}
			set
			{
				OnPropertyChanging("ProductUrl");
				((Entity)this).SetAttributeValue("producturl", (object)value);
				OnPropertyChanged("ProductUrl");
			}
		}

		[AttributeLogicalName("quantitydecimal")]
		[ExcludeFromCodeCoverage]
		public int? QuantityDecimal
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("quantitydecimal");
			}
			set
			{
				OnPropertyChanging("QuantityDecimal");
				((Entity)this).SetAttributeValue("quantitydecimal", (object)value);
				OnPropertyChanged("QuantityDecimal");
			}
		}

		[AttributeLogicalName("quantityonhand")]
		[ExcludeFromCodeCoverage]
		public decimal? QuantityOnHand
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("quantityonhand");
			}
			set
			{
				OnPropertyChanging("QuantityOnHand");
				((Entity)this).SetAttributeValue("quantityonhand", (object)value);
				OnPropertyChanged("QuantityOnHand");
			}
		}

		[AttributeLogicalName("size")]
		[ExcludeFromCodeCoverage]
		public string Size
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("size");
			}
			set
			{
				OnPropertyChanging("Size");
				((Entity)this).SetAttributeValue("size", (object)value);
				OnPropertyChanged("Size");
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

		[AttributeLogicalName("standardcost")]
		[ExcludeFromCodeCoverage]
		public Money StandardCost
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Money>("standardcost");
			}
			set
			{
				OnPropertyChanging("StandardCost");
				((Entity)this).SetAttributeValue("standardcost", (object)value);
				OnPropertyChanged("StandardCost");
			}
		}

		[AttributeLogicalName("standardcost_base")]
		[ExcludeFromCodeCoverage]
		public Money StandardCost_Base => ((Entity)this).GetAttributeValue<Money>("standardcost_base");

		[AttributeLogicalName("statecode")]
		[ExcludeFromCodeCoverage]
		public ProductState? StateCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (ProductState)Enum.ToObject(typeof(ProductState), attributeValue.get_Value());
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

		[AttributeLogicalName("stockvolume")]
		[ExcludeFromCodeCoverage]
		public decimal? StockVolume
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("stockvolume");
			}
			set
			{
				OnPropertyChanging("StockVolume");
				((Entity)this).SetAttributeValue("stockvolume", (object)value);
				OnPropertyChanged("StockVolume");
			}
		}

		[AttributeLogicalName("stockweight")]
		[ExcludeFromCodeCoverage]
		public decimal? StockWeight
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("stockweight");
			}
			set
			{
				OnPropertyChanging("StockWeight");
				((Entity)this).SetAttributeValue("stockweight", (object)value);
				OnPropertyChanged("StockWeight");
			}
		}

		[AttributeLogicalName("subjectid")]
		[ExcludeFromCodeCoverage]
		public EntityReference SubjectId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("subjectid");
			}
			set
			{
				OnPropertyChanging("SubjectId");
				((Entity)this).SetAttributeValue("subjectid", (object)value);
				OnPropertyChanged("SubjectId");
			}
		}

		[AttributeLogicalName("suppliername")]
		[ExcludeFromCodeCoverage]
		public string SupplierName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("suppliername");
			}
			set
			{
				OnPropertyChanging("SupplierName");
				((Entity)this).SetAttributeValue("suppliername", (object)value);
				OnPropertyChanged("SupplierName");
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

		[AttributeLogicalName("validfromdate")]
		[ExcludeFromCodeCoverage]
		public DateTime? ValidFromDate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("validfromdate");
			}
			set
			{
				OnPropertyChanging("ValidFromDate");
				((Entity)this).SetAttributeValue("validfromdate", (object)value);
				OnPropertyChanged("ValidFromDate");
			}
		}

		[AttributeLogicalName("validtodate")]
		[ExcludeFromCodeCoverage]
		public DateTime? ValidToDate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("validtodate");
			}
			set
			{
				OnPropertyChanging("ValidToDate");
				((Entity)this).SetAttributeValue("validtodate", (object)value);
				OnPropertyChanged("ValidToDate");
			}
		}

		[AttributeLogicalName("vendorid")]
		[ExcludeFromCodeCoverage]
		public string VendorID
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("vendorid");
			}
			set
			{
				OnPropertyChanging("VendorID");
				((Entity)this).SetAttributeValue("vendorid", (object)value);
				OnPropertyChanged("VendorID");
			}
		}

		[AttributeLogicalName("vendorname")]
		[ExcludeFromCodeCoverage]
		public string VendorName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("vendorname");
			}
			set
			{
				OnPropertyChanging("VendorName");
				((Entity)this).SetAttributeValue("vendorname", (object)value);
				OnPropertyChanged("VendorName");
			}
		}

		[AttributeLogicalName("vendorpartnumber")]
		[ExcludeFromCodeCoverage]
		public string VendorPartNumber
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("vendorpartnumber");
			}
			set
			{
				OnPropertyChanging("VendorPartNumber");
				((Entity)this).SetAttributeValue("vendorpartnumber", (object)value);
				OnPropertyChanged("VendorPartNumber");
			}
		}

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Product> Referencedproduct_parent_product
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Product>("product_parent_product", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedproduct_parent_product");
				((Entity)this).SetRelatedEntities<Product>("product_parent_product", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedproduct_parent_product");
			}
		}

		[AttributeLogicalName("parentproductid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public Product Referencingproduct_parent_product
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<Product>("product_parent_product", (EntityRole?)(EntityRole)0);
			}
			set
			{
				OnPropertyChanging("Referencingproduct_parent_product");
				((Entity)this).SetRelatedEntity<Product>("product_parent_product", (EntityRole?)(EntityRole)0, value);
				OnPropertyChanged("Referencingproduct_parent_product");
			}
		}

		[AttributeLogicalName("defaultuomid")]
		[RelationshipSchemaName("unit_of_measurement_products")]
		[ExcludeFromCodeCoverage]
		public Unit unit_of_measurement_products
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<Unit>("unit_of_measurement_products", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("unit_of_measurement_products");
				((Entity)this).SetRelatedEntity<Unit>("unit_of_measurement_products", (EntityRole?)null, value);
				OnPropertyChanged("unit_of_measurement_products");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Product()
			: this("product")
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
