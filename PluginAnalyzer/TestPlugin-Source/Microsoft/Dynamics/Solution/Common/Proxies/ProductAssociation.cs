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
	[EntityLogicalName("productassociation")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class ProductAssociation : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "productassociation";

		public const int EntityTypeCode = 1025;

		public const int AttributeName_MaxLength = 100;

		public const string AttributeAssociatedProduct = "associatedproduct";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDMTImportState = "dmtimportstate";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeProductAssociationId = "productassociationid";

		public const string AttributeId = "productassociationid";

		public const string AttributeProductId = "productid";

		public const string AttributeProductIsRequired = "productisrequired";

		public const string AttributePropertyCustomizationStatus = "propertycustomizationstatus";

		public const string AttributeQuantity = "quantity";

		public const string AttributeStatecode = "statecode";

		public const string AttributeStatuscode = "statuscode";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeUoMId = "uomid";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeUnit_of_measurement_productassociation = "uomid";

		[AttributeLogicalName("associatedproduct")]
		[ExcludeFromCodeCoverage]
		public EntityReference AssociatedProduct
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("associatedproduct");
			}
			set
			{
				OnPropertyChanging("AssociatedProduct");
				((Entity)this).SetAttributeValue("associatedproduct", (object)value);
				OnPropertyChanged("AssociatedProduct");
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

		[AttributeLogicalName("exchangerate")]
		[ExcludeFromCodeCoverage]
		public decimal? ExchangeRate => ((Entity)this).GetAttributeValue<decimal?>("exchangerate");

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

		[AttributeLogicalName("productassociationid")]
		[ExcludeFromCodeCoverage]
		public Guid? ProductAssociationId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("productassociationid");
			}
			set
			{
				OnPropertyChanging("ProductAssociationId");
				((Entity)this).SetAttributeValue("productassociationid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("ProductAssociationId");
			}
		}

		[AttributeLogicalName("productassociationid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				ProductAssociationId = value;
			}
		}

		[AttributeLogicalName("productid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ProductId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("productid");
			}
			set
			{
				OnPropertyChanging("ProductId");
				((Entity)this).SetAttributeValue("productid", (object)value);
				OnPropertyChanged("ProductId");
			}
		}

		[AttributeLogicalName("productisrequired")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ProductIsRequired
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("productisrequired");
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
				OnPropertyChanging("ProductIsRequired");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("productisrequired", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("productisrequired", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ProductIsRequired");
			}
		}

		[AttributeLogicalName("propertycustomizationstatus")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue PropertyCustomizationStatus
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("propertycustomizationstatus");
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
				OnPropertyChanging("PropertyCustomizationStatus");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("propertycustomizationstatus", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("propertycustomizationstatus", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("PropertyCustomizationStatus");
			}
		}

		[AttributeLogicalName("quantity")]
		[ExcludeFromCodeCoverage]
		public decimal? Quantity
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("quantity");
			}
			set
			{
				OnPropertyChanging("Quantity");
				((Entity)this).SetAttributeValue("quantity", (object)value);
				OnPropertyChanged("Quantity");
			}
		}

		[AttributeLogicalName("statecode")]
		[ExcludeFromCodeCoverage]
		public ProductAssociationState? statecode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (ProductAssociationState)Enum.ToObject(typeof(ProductAssociationState), attributeValue.get_Value());
				}
				return null;
			}
			set
			{
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Expected O, but got Unknown
				OnPropertyChanging("statecode");
				if (!value.HasValue)
				{
					((Entity)this).SetAttributeValue("statecode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("statecode", (object)new OptionSetValue((int)value.Value));
				}
				OnPropertyChanged("statecode");
			}
		}

		[AttributeLogicalName("statuscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue statuscode
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
				OnPropertyChanging("statuscode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("statuscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("statuscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("statuscode");
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

		[AttributeLogicalName("uomid")]
		[ExcludeFromCodeCoverage]
		public EntityReference UoMId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("uomid");
			}
			set
			{
				OnPropertyChanging("UoMId");
				((Entity)this).SetAttributeValue("uomid", (object)value);
				OnPropertyChanged("UoMId");
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

		[AttributeLogicalName("uomid")]
		[RelationshipSchemaName("unit_of_measurement_productassociation")]
		[ExcludeFromCodeCoverage]
		public Unit unit_of_measurement_productassociation
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<Unit>("unit_of_measurement_productassociation", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("unit_of_measurement_productassociation");
				((Entity)this).SetRelatedEntity<Unit>("unit_of_measurement_productassociation", (EntityRole?)null, value);
				OnPropertyChanged("unit_of_measurement_productassociation");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public ProductAssociation()
			: this("productassociation")
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
