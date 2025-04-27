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
	[EntityLogicalName("connection")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Connection : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "connection";

		public const int EntityTypeCode = 3234;

		public const int AttributeName_MaxLength = 500;

		public const int AttributeEntityImage_URL_MaxLength = 200;

		public const string AttributeConnectionId = "connectionid";

		public const string AttributeId = "connectionid";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDescription = "description";

		public const string AttributeEffectiveEnd = "effectiveend";

		public const string AttributeEffectiveStart = "effectivestart";

		public const string AttributeEntityImage = "entityimage";

		public const string AttributeEntityImage_Timestamp = "entityimage_timestamp";

		public const string AttributeEntityImage_URL = "entityimage_url";

		public const string AttributeEntityImageId = "entityimageid";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIsMaster = "ismaster";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeOwnerId = "ownerid";

		public const string AttributeOwningBusinessUnit = "owningbusinessunit";

		public const string AttributeOwningTeam = "owningteam";

		public const string AttributeOwningUser = "owninguser";

		public const string AttributeRecord1Id = "record1id";

		public const string AttributeRecord1ObjectTypeCode = "record1objecttypecode";

		public const string AttributeRecord1RoleId = "record1roleid";

		public const string AttributeRecord2Id = "record2id";

		public const string AttributeRecord2ObjectTypeCode = "record2objecttypecode";

		public const string AttributeRecord2RoleId = "record2roleid";

		public const string AttributeRelatedConnectionId = "relatedconnectionid";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeReferencingconnection_related_connection = "relatedconnectionid";

		public const string AttributeConnection_role_connections1 = "record1roleid";

		public const string AttributeConnection_role_connections2 = "record2roleid";

		[AttributeLogicalName("connectionid")]
		[ExcludeFromCodeCoverage]
		public Guid? ConnectionId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("connectionid");
			}
			set
			{
				OnPropertyChanging("ConnectionId");
				((Entity)this).SetAttributeValue("connectionid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("ConnectionId");
			}
		}

		[AttributeLogicalName("connectionid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				ConnectionId = value;
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

		[AttributeLogicalName("effectiveend")]
		[ExcludeFromCodeCoverage]
		public DateTime? EffectiveEnd
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("effectiveend");
			}
			set
			{
				OnPropertyChanging("EffectiveEnd");
				((Entity)this).SetAttributeValue("effectiveend", (object)value);
				OnPropertyChanged("EffectiveEnd");
			}
		}

		[AttributeLogicalName("effectivestart")]
		[ExcludeFromCodeCoverage]
		public DateTime? EffectiveStart
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("effectivestart");
			}
			set
			{
				OnPropertyChanging("EffectiveStart");
				((Entity)this).SetAttributeValue("effectivestart", (object)value);
				OnPropertyChanged("EffectiveStart");
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

		[AttributeLogicalName("ismaster")]
		[ExcludeFromCodeCoverage]
		public bool? IsMaster => ((Entity)this).GetAttributeValue<bool?>("ismaster");

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
		public string Name => ((Entity)this).GetAttributeValue<string>("name");

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

		[AttributeLogicalName("record1id")]
		[ExcludeFromCodeCoverage]
		public EntityReference Record1Id
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("record1id");
			}
			set
			{
				OnPropertyChanging("Record1Id");
				((Entity)this).SetAttributeValue("record1id", (object)value);
				OnPropertyChanged("Record1Id");
			}
		}

		[AttributeLogicalName("record1objecttypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Record1ObjectTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("record1objecttypecode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
		}

		[AttributeLogicalName("record1roleid")]
		[ExcludeFromCodeCoverage]
		public EntityReference Record1RoleId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("record1roleid");
			}
			set
			{
				OnPropertyChanging("Record1RoleId");
				((Entity)this).SetAttributeValue("record1roleid", (object)value);
				OnPropertyChanged("Record1RoleId");
			}
		}

		[AttributeLogicalName("record2id")]
		[ExcludeFromCodeCoverage]
		public EntityReference Record2Id
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("record2id");
			}
			set
			{
				OnPropertyChanging("Record2Id");
				((Entity)this).SetAttributeValue("record2id", (object)value);
				OnPropertyChanged("Record2Id");
			}
		}

		[AttributeLogicalName("record2objecttypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Record2ObjectTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("record2objecttypecode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
		}

		[AttributeLogicalName("record2roleid")]
		[ExcludeFromCodeCoverage]
		public EntityReference Record2RoleId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("record2roleid");
			}
			set
			{
				OnPropertyChanging("Record2RoleId");
				((Entity)this).SetAttributeValue("record2roleid", (object)value);
				OnPropertyChanged("Record2RoleId");
			}
		}

		[AttributeLogicalName("relatedconnectionid")]
		[ExcludeFromCodeCoverage]
		public EntityReference RelatedConnectionId => ((Entity)this).GetAttributeValue<EntityReference>("relatedconnectionid");

		[AttributeLogicalName("statecode")]
		[ExcludeFromCodeCoverage]
		public ConnectionState? StateCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (ConnectionState)Enum.ToObject(typeof(ConnectionState), attributeValue.get_Value());
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

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Connection> Referencedconnection_related_connection
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Connection>("connection_related_connection", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedconnection_related_connection");
				((Entity)this).SetRelatedEntities<Connection>("connection_related_connection", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedconnection_related_connection");
			}
		}

		[AttributeLogicalName("relatedconnectionid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public Connection Referencingconnection_related_connection => ((Entity)this).GetRelatedEntity<Connection>("connection_related_connection", (EntityRole?)(EntityRole)0);

		[AttributeLogicalName("record1roleid")]
		[RelationshipSchemaName("connection_role_connections1")]
		[ExcludeFromCodeCoverage]
		public ConnectionRole connection_role_connections1
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<ConnectionRole>("connection_role_connections1", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("connection_role_connections1");
				((Entity)this).SetRelatedEntity<ConnectionRole>("connection_role_connections1", (EntityRole?)null, value);
				OnPropertyChanged("connection_role_connections1");
			}
		}

		[AttributeLogicalName("record2roleid")]
		[RelationshipSchemaName("connection_role_connections2")]
		[ExcludeFromCodeCoverage]
		public ConnectionRole connection_role_connections2
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<ConnectionRole>("connection_role_connections2", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("connection_role_connections2");
				((Entity)this).SetRelatedEntity<ConnectionRole>("connection_role_connections2", (EntityRole?)null, value);
				OnPropertyChanged("connection_role_connections2");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Connection()
			: this("connection")
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
