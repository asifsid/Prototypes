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
	[EntityLogicalName("actioncard")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class ActionCard : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "actioncard";

		public const int EntityTypeCode = 9962;

		public const int AttributeTitle_MaxLength = 200;

		public const string AttributeActionCardId = "actioncardid";

		public const string AttributeId = "actioncardid";

		public const string AttributeCardType = "cardtype";

		public const string AttributeCardTypeId = "cardtypeid";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeData = "data";

		public const string AttributeDescription = "description";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeExpiryDate = "expirydate";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeOwnerId = "ownerid";

		public const string AttributeOwningBusinessUnit = "owningbusinessunit";

		public const string AttributeOwningTeam = "owningteam";

		public const string AttributeOwningUser = "owninguser";

		public const string AttributePriority = "priority";

		public const string AttributeRecordId = "recordid";

		public const string AttributeRecordIdObjectTypeCode2 = "recordidobjecttypecode2";

		public const string AttributeReferenceTokens = "referencetokens";

		public const string AttributeRegardingObjectId = "regardingobjectid";

		public const string AttributeSource = "source";

		public const string AttributeStartDate = "startdate";

		public const string AttributeState = "state";

		public const string AttributeTitle = "title";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeVisibility = "visibility";

		[AttributeLogicalName("actioncardid")]
		[ExcludeFromCodeCoverage]
		public Guid? ActionCardId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("actioncardid");
			}
			set
			{
				OnPropertyChanging("ActionCardId");
				((Entity)this).SetAttributeValue("actioncardid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("ActionCardId");
			}
		}

		[AttributeLogicalName("actioncardid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				ActionCardId = value;
			}
		}

		[AttributeLogicalName("cardtype")]
		[ExcludeFromCodeCoverage]
		public int? CardType
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("cardtype");
			}
			set
			{
				OnPropertyChanging("CardType");
				((Entity)this).SetAttributeValue("cardtype", (object)value);
				OnPropertyChanged("CardType");
			}
		}

		[AttributeLogicalName("cardtypeid")]
		[ExcludeFromCodeCoverage]
		public EntityReference CardTypeId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("cardtypeid");
			}
			set
			{
				OnPropertyChanging("CardTypeId");
				((Entity)this).SetAttributeValue("cardtypeid", (object)value);
				OnPropertyChanged("CardTypeId");
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

		[AttributeLogicalName("data")]
		[ExcludeFromCodeCoverage]
		public string Data
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("data");
			}
			set
			{
				OnPropertyChanging("Data");
				((Entity)this).SetAttributeValue("data", (object)value);
				OnPropertyChanged("Data");
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

		[AttributeLogicalName("exchangerate")]
		[ExcludeFromCodeCoverage]
		public decimal? ExchangeRate => ((Entity)this).GetAttributeValue<decimal?>("exchangerate");

		[AttributeLogicalName("expirydate")]
		[ExcludeFromCodeCoverage]
		public DateTime? ExpiryDate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("expirydate");
			}
			set
			{
				OnPropertyChanging("ExpiryDate");
				((Entity)this).SetAttributeValue("expirydate", (object)value);
				OnPropertyChanged("ExpiryDate");
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

		[AttributeLogicalName("modifiedby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedby");

		[AttributeLogicalName("modifiedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? ModifiedOn => ((Entity)this).GetAttributeValue<DateTime?>("modifiedon");

		[AttributeLogicalName("modifiedonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedonbehalfby");

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

		[AttributeLogicalName("priority")]
		[ExcludeFromCodeCoverage]
		public int? Priority
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("priority");
			}
			set
			{
				OnPropertyChanging("Priority");
				((Entity)this).SetAttributeValue("priority", (object)value);
				OnPropertyChanged("Priority");
			}
		}

		[AttributeLogicalName("recordid")]
		[ExcludeFromCodeCoverage]
		public EntityReference RecordId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("recordid");
			}
			set
			{
				OnPropertyChanging("RecordId");
				((Entity)this).SetAttributeValue("recordid", (object)value);
				OnPropertyChanged("RecordId");
			}
		}

		[AttributeLogicalName("recordidobjecttypecode2")]
		[ExcludeFromCodeCoverage]
		public int? RecordIdObjectTypeCode2
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("recordidobjecttypecode2");
			}
			set
			{
				OnPropertyChanging("RecordIdObjectTypeCode2");
				((Entity)this).SetAttributeValue("recordidobjecttypecode2", (object)value);
				OnPropertyChanged("RecordIdObjectTypeCode2");
			}
		}

		[AttributeLogicalName("referencetokens")]
		[ExcludeFromCodeCoverage]
		public string ReferenceTokens
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("referencetokens");
			}
			set
			{
				OnPropertyChanging("ReferenceTokens");
				((Entity)this).SetAttributeValue("referencetokens", (object)value);
				OnPropertyChanged("ReferenceTokens");
			}
		}

		[AttributeLogicalName("regardingobjectid")]
		[ExcludeFromCodeCoverage]
		public EntityReference RegardingObjectId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("regardingobjectid");
			}
			set
			{
				OnPropertyChanging("RegardingObjectId");
				((Entity)this).SetAttributeValue("regardingobjectid", (object)value);
				OnPropertyChanged("RegardingObjectId");
			}
		}

		[AttributeLogicalName("source")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue Source
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("source");
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
				OnPropertyChanging("Source");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("source", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("source", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("Source");
			}
		}

		[AttributeLogicalName("startdate")]
		[ExcludeFromCodeCoverage]
		public DateTime? StartDate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("startdate");
			}
			set
			{
				OnPropertyChanging("StartDate");
				((Entity)this).SetAttributeValue("startdate", (object)value);
				OnPropertyChanged("StartDate");
			}
		}

		[AttributeLogicalName("state")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue State
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("state");
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
				OnPropertyChanging("State");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("state", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("state", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("State");
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

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[AttributeLogicalName("visibility")]
		[ExcludeFromCodeCoverage]
		public bool? Visibility
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("visibility");
			}
			set
			{
				OnPropertyChanging("Visibility");
				((Entity)this).SetAttributeValue("visibility", (object)value);
				OnPropertyChanged("Visibility");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public ActionCard()
			: this("actioncard")
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
