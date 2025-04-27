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
	[EntityLogicalName("connectionrole")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class ConnectionRole : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "connectionrole";

		public const int EntityTypeCode = 3231;

		public const int AttributeIntroducedVersion_MaxLength = 48;

		public const int AttributeDescription_MaxLength = 1000;

		public const int AttributeName_MaxLength = 100;

		public const string AttributeCategory = "category";

		public const string AttributeComponentState = "componentstate";

		public const string AttributeConnectionRoleId = "connectionroleid";

		public const string AttributeId = "connectionroleid";

		public const string AttributeConnectionRoleIdUnique = "connectionroleidunique";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDescription = "description";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIntroducedVersion = "introducedversion";

		public const string AttributeIsCustomizable = "iscustomizable";

		public const string AttributeIsManaged = "ismanaged";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverwriteTime = "overwritetime";

		public const string AttributeSolutionId = "solutionid";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeVersionNumber = "versionnumber";

		[AttributeLogicalName("category")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Category
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("category");
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
				OnPropertyChanging("Category");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("category", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("category", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Category");
			}
		}

		[AttributeLogicalName("componentstate")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ComponentState
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("componentstate");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
		}

		[AttributeLogicalName("connectionroleid")]
		[ExcludeFromCodeCoverage]
		public Guid? ConnectionRoleId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("connectionroleid");
			}
			set
			{
				OnPropertyChanging("ConnectionRoleId");
				((Entity)this).SetAttributeValue("connectionroleid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("ConnectionRoleId");
			}
		}

		[AttributeLogicalName("connectionroleid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				ConnectionRoleId = value;
			}
		}

		[AttributeLogicalName("connectionroleidunique")]
		[ExcludeFromCodeCoverage]
		public Guid? ConnectionRoleIdUnique => ((Entity)this).GetAttributeValue<Guid?>("connectionroleidunique");

		[AttributeLogicalName("createdby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedBy => ((Entity)this).GetAttributeValue<EntityReference>("createdby");

		[AttributeLogicalName("createdon")]
		[ExcludeFromCodeCoverage]
		public DateTime? CreatedOn => ((Entity)this).GetAttributeValue<DateTime?>("createdon");

		[AttributeLogicalName("createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("createdonbehalfby");

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

		[AttributeLogicalName("introducedversion")]
		[ExcludeFromCodeCoverage]
		public string IntroducedVersion
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("introducedversion");
			}
			set
			{
				OnPropertyChanging("IntroducedVersion");
				((Entity)this).SetAttributeValue("introducedversion", (object)value);
				OnPropertyChanged("IntroducedVersion");
			}
		}

		[AttributeLogicalName("iscustomizable")]
		[ExcludeFromCodeCoverage]
		public BooleanManagedProperty IsCustomizable
		{
			get
			{
				return ((Entity)this).GetAttributeValue<BooleanManagedProperty>("iscustomizable");
			}
			set
			{
				OnPropertyChanging("IsCustomizable");
				((Entity)this).SetAttributeValue("iscustomizable", (object)value);
				OnPropertyChanged("IsCustomizable");
			}
		}

		[AttributeLogicalName("ismanaged")]
		[ExcludeFromCodeCoverage]
		public bool? IsManaged => ((Entity)this).GetAttributeValue<bool?>("ismanaged");

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

		[AttributeLogicalName("overwritetime")]
		[ExcludeFromCodeCoverage]
		public DateTime? OverwriteTime => ((Entity)this).GetAttributeValue<DateTime?>("overwritetime");

		[AttributeLogicalName("solutionid")]
		[ExcludeFromCodeCoverage]
		public Guid? SolutionId => ((Entity)this).GetAttributeValue<Guid?>("solutionid");

		[AttributeLogicalName("statecode")]
		[ExcludeFromCodeCoverage]
		public ConnectionRoleState? StateCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (ConnectionRoleState)Enum.ToObject(typeof(ConnectionRoleState), attributeValue.get_Value());
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

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[RelationshipSchemaName("connection_role_connections1")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Connection> connection_role_connections1
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Connection>("connection_role_connections1", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("connection_role_connections1");
				((Entity)this).SetRelatedEntities<Connection>("connection_role_connections1", (EntityRole?)null, value);
				OnPropertyChanged("connection_role_connections1");
			}
		}

		[RelationshipSchemaName("connection_role_connections2")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Connection> connection_role_connections2
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Connection>("connection_role_connections2", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("connection_role_connections2");
				((Entity)this).SetRelatedEntities<Connection>("connection_role_connections2", (EntityRole?)null, value);
				OnPropertyChanged("connection_role_connections2");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<ConnectionRole> Referencingconnectionroleassociation_association
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<ConnectionRole>("connectionroleassociation_association", (EntityRole?)(EntityRole)0);
			}
			set
			{
				OnPropertyChanging("Referencingconnectionroleassociation_association");
				((Entity)this).SetRelatedEntities<ConnectionRole>("connectionroleassociation_association", (EntityRole?)(EntityRole)0, value);
				OnPropertyChanged("Referencingconnectionroleassociation_association");
			}
		}

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<ConnectionRole> Referencedconnectionroleassociation_association
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<ConnectionRole>("connectionroleassociation_association", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedconnectionroleassociation_association");
				((Entity)this).SetRelatedEntities<ConnectionRole>("connectionroleassociation_association", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedconnectionroleassociation_association");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public ConnectionRole()
			: this("connectionrole")
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
