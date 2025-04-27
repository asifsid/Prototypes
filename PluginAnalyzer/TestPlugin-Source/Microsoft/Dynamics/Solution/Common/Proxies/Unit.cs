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
	[EntityLogicalName("uom")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Unit : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "uom";

		public const int EntityTypeCode = 1055;

		public const int AttributeName_MaxLength = 100;

		public const string AttributeBaseUoM = "baseuom";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedByExternalParty = "createdbyexternalparty";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIsScheduleBaseUoM = "isschedulebaseuom";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedByExternalParty = "modifiedbyexternalparty";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeQuantity = "quantity";

		public const string AttributeUoMId = "uomid";

		public const string AttributeId = "uomid";

		public const string AttributeUoMScheduleId = "uomscheduleid";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeReferencingunit_of_measurement_base_unit = "baseuom";

		[AttributeLogicalName("baseuom")]
		[ExcludeFromCodeCoverage]
		public EntityReference BaseUoM
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("baseuom");
			}
			set
			{
				OnPropertyChanging("BaseUoM");
				((Entity)this).SetAttributeValue("baseuom", (object)value);
				OnPropertyChanged("BaseUoM");
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

		[AttributeLogicalName("isschedulebaseuom")]
		[ExcludeFromCodeCoverage]
		public bool? IsScheduleBaseUoM
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isschedulebaseuom");
			}
			set
			{
				OnPropertyChanging("isschedulebaseuom");
				((Entity)this).SetAttributeValue("isschedulebaseuom", (object)value);
				OnPropertyChanged("isschedulebaseuom");
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
		public Guid? OrganizationId => ((Entity)this).GetAttributeValue<Guid?>("organizationid");

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

		[AttributeLogicalName("uomid")]
		[ExcludeFromCodeCoverage]
		public Guid? UoMId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("uomid");
			}
			set
			{
				OnPropertyChanging("UoMId");
				((Entity)this).SetAttributeValue("uomid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("UoMId");
			}
		}

		[AttributeLogicalName("uomid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				UoMId = value;
			}
		}

		[AttributeLogicalName("uomscheduleid")]
		[ExcludeFromCodeCoverage]
		public EntityReference UoMScheduleId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("uomscheduleid");
			}
			set
			{
				OnPropertyChanging("UoMScheduleId");
				((Entity)this).SetAttributeValue("uomscheduleid", (object)value);
				OnPropertyChanged("UoMScheduleId");
			}
		}

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Unit> Referencedunit_of_measurement_base_unit
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Unit>("unit_of_measurement_base_unit", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedunit_of_measurement_base_unit");
				((Entity)this).SetRelatedEntities<Unit>("unit_of_measurement_base_unit", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedunit_of_measurement_base_unit");
			}
		}

		[RelationshipSchemaName("unit_of_measurement_products")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Product> unit_of_measurement_products
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Product>("unit_of_measurement_products", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("unit_of_measurement_products");
				((Entity)this).SetRelatedEntities<Product>("unit_of_measurement_products", (EntityRole?)null, value);
				OnPropertyChanged("unit_of_measurement_products");
			}
		}

		[AttributeLogicalName("baseuom")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public Unit Referencingunit_of_measurement_base_unit
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<Unit>("unit_of_measurement_base_unit", (EntityRole?)(EntityRole)0);
			}
			set
			{
				OnPropertyChanging("Referencingunit_of_measurement_base_unit");
				((Entity)this).SetRelatedEntity<Unit>("unit_of_measurement_base_unit", (EntityRole?)(EntityRole)0, value);
				OnPropertyChanged("Referencingunit_of_measurement_base_unit");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Unit()
			: this("uom")
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
