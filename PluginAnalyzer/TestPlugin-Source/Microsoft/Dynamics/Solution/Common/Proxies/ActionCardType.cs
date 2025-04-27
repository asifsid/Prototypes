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
	[EntityLogicalName("cardtype")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class ActionCardType : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "cardtype";

		public const int EntityTypeCode = 9983;

		public const int AttributeCardName_MaxLength = 100;

		public const int AttributeSoftTitle_MaxLength = 200;

		public const int AttributeGroupType_MaxLength = 200;

		public const int AttributeCardTypeIcon_MaxLength = 500;

		public const int AttributeSummaryText_MaxLength = 500;

		public const string AttributeActions = "actions";

		public const string AttributeBoolCardOption = "boolcardoption";

		public const string AttributeCardName = "cardname";

		public const string AttributeCardType = "cardtype";

		public const string AttributeCardTypeIcon = "cardtypeicon";

		public const string AttributeCardTypeId = "cardtypeid";

		public const string AttributeId = "cardtypeid";

		public const string AttributeClientAvailability = "clientavailability";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeGroupType = "grouptype";

		public const string AttributeHasSnoozeDismiss = "hassnoozedismiss";

		public const string AttributeIntCardOption = "intcardoption";

		public const string AttributeIsBaseCard = "isbasecard";

		public const string AttributeIsEnabled = "isenabled";

		public const string AttributeIsLiveOnly = "isliveonly";

		public const string AttributeIsPreviewCard = "ispreviewcard";

		public const string AttributeLastSyncTime = "lastsynctime";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeScheduleTime = "scheduletime";

		public const string AttributeSoftTitle = "softtitle";

		public const string AttributeStringCardOption = "stringcardoption";

		public const string AttributeSummaryText = "summarytext";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeVersionNumber = "versionnumber";

		[AttributeLogicalName("actions")]
		[ExcludeFromCodeCoverage]
		public string Actions
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("actions");
			}
			set
			{
				OnPropertyChanging("Actions");
				((Entity)this).SetAttributeValue("actions", (object)value);
				OnPropertyChanged("Actions");
			}
		}

		[AttributeLogicalName("boolcardoption")]
		[ExcludeFromCodeCoverage]
		public bool? BoolCardOption
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("boolcardoption");
			}
			set
			{
				OnPropertyChanging("BoolCardOption");
				((Entity)this).SetAttributeValue("boolcardoption", (object)value);
				OnPropertyChanged("BoolCardOption");
			}
		}

		[AttributeLogicalName("cardname")]
		[ExcludeFromCodeCoverage]
		public string CardName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("cardname");
			}
			set
			{
				OnPropertyChanging("CardName");
				((Entity)this).SetAttributeValue("cardname", (object)value);
				OnPropertyChanged("CardName");
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

		[AttributeLogicalName("cardtypeicon")]
		[ExcludeFromCodeCoverage]
		public string CardTypeIcon
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("cardtypeicon");
			}
			set
			{
				OnPropertyChanging("CardTypeIcon");
				((Entity)this).SetAttributeValue("cardtypeicon", (object)value);
				OnPropertyChanged("CardTypeIcon");
			}
		}

		[AttributeLogicalName("cardtypeid")]
		[ExcludeFromCodeCoverage]
		public Guid? CardTypeId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("cardtypeid");
			}
			set
			{
				OnPropertyChanging("CardTypeId");
				((Entity)this).SetAttributeValue("cardtypeid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("CardTypeId");
			}
		}

		[AttributeLogicalName("cardtypeid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				CardTypeId = value;
			}
		}

		[AttributeLogicalName("clientavailability")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ClientAvailability
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("clientavailability");
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
				OnPropertyChanging("ClientAvailability");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("clientavailability", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("clientavailability", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ClientAvailability");
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

		[AttributeLogicalName("exchangerate")]
		[ExcludeFromCodeCoverage]
		public decimal? ExchangeRate => ((Entity)this).GetAttributeValue<decimal?>("exchangerate");

		[AttributeLogicalName("grouptype")]
		[ExcludeFromCodeCoverage]
		public string GroupType
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("grouptype");
			}
			set
			{
				OnPropertyChanging("GroupType");
				((Entity)this).SetAttributeValue("grouptype", (object)value);
				OnPropertyChanged("GroupType");
			}
		}

		[AttributeLogicalName("hassnoozedismiss")]
		[ExcludeFromCodeCoverage]
		public bool? HasSnoozeDismiss
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("hassnoozedismiss");
			}
			set
			{
				OnPropertyChanging("HasSnoozeDismiss");
				((Entity)this).SetAttributeValue("hassnoozedismiss", (object)value);
				OnPropertyChanged("HasSnoozeDismiss");
			}
		}

		[AttributeLogicalName("intcardoption")]
		[ExcludeFromCodeCoverage]
		public int? IntCardOption
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("intcardoption");
			}
			set
			{
				OnPropertyChanging("IntCardOption");
				((Entity)this).SetAttributeValue("intcardoption", (object)value);
				OnPropertyChanged("IntCardOption");
			}
		}

		[AttributeLogicalName("isbasecard")]
		[ExcludeFromCodeCoverage]
		public bool? IsBaseCard
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isbasecard");
			}
			set
			{
				OnPropertyChanging("IsBaseCard");
				((Entity)this).SetAttributeValue("isbasecard", (object)value);
				OnPropertyChanged("IsBaseCard");
			}
		}

		[AttributeLogicalName("isenabled")]
		[ExcludeFromCodeCoverage]
		public bool? IsEnabled
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isenabled");
			}
			set
			{
				OnPropertyChanging("IsEnabled");
				((Entity)this).SetAttributeValue("isenabled", (object)value);
				OnPropertyChanged("IsEnabled");
			}
		}

		[AttributeLogicalName("isliveonly")]
		[ExcludeFromCodeCoverage]
		public bool? IsLiveOnly
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isliveonly");
			}
			set
			{
				OnPropertyChanging("IsLiveOnly");
				((Entity)this).SetAttributeValue("isliveonly", (object)value);
				OnPropertyChanged("IsLiveOnly");
			}
		}

		[AttributeLogicalName("ispreviewcard")]
		[ExcludeFromCodeCoverage]
		public bool? IsPreviewCard
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("ispreviewcard");
			}
			set
			{
				OnPropertyChanging("IsPreviewCard");
				((Entity)this).SetAttributeValue("ispreviewcard", (object)value);
				OnPropertyChanged("IsPreviewCard");
			}
		}

		[AttributeLogicalName("lastsynctime")]
		[ExcludeFromCodeCoverage]
		public DateTime? LastSyncTime
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("lastsynctime");
			}
			set
			{
				OnPropertyChanging("LastSyncTime");
				((Entity)this).SetAttributeValue("lastsynctime", (object)value);
				OnPropertyChanged("LastSyncTime");
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

		[AttributeLogicalName("scheduletime")]
		[ExcludeFromCodeCoverage]
		public DateTime? ScheduleTime
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("scheduletime");
			}
			set
			{
				OnPropertyChanging("ScheduleTime");
				((Entity)this).SetAttributeValue("scheduletime", (object)value);
				OnPropertyChanged("ScheduleTime");
			}
		}

		[AttributeLogicalName("softtitle")]
		[ExcludeFromCodeCoverage]
		public string SoftTitle
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("softtitle");
			}
			set
			{
				OnPropertyChanging("SoftTitle");
				((Entity)this).SetAttributeValue("softtitle", (object)value);
				OnPropertyChanged("SoftTitle");
			}
		}

		[AttributeLogicalName("stringcardoption")]
		[ExcludeFromCodeCoverage]
		public string StringCardOption
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("stringcardoption");
			}
			set
			{
				OnPropertyChanging("StringCardOption");
				((Entity)this).SetAttributeValue("stringcardoption", (object)value);
				OnPropertyChanged("StringCardOption");
			}
		}

		[AttributeLogicalName("summarytext")]
		[ExcludeFromCodeCoverage]
		public string SummaryText
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("summarytext");
			}
			set
			{
				OnPropertyChanging("SummaryText");
				((Entity)this).SetAttributeValue("summarytext", (object)value);
				OnPropertyChanged("SummaryText");
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

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public ActionCardType()
			: this("cardtype")
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
