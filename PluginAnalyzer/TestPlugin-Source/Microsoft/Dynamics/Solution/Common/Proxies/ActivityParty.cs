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
	[EntityLogicalName("activityparty")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class ActivityParty : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "activityparty";

		public const int EntityTypeCode = 135;

		public const int AttributeExchangeEntryId_MaxLength = 1024;

		public const int AttributeAddressUsed_MaxLength = 200;

		public const string AttributeActivityId = "activityid";

		public const string AttributeActivityPartyId = "activitypartyid";

		public const string AttributeId = "activitypartyid";

		public const string AttributeAddressUsed = "addressused";

		public const string AttributeAddressUsedEmailColumnNumber = "addressusedemailcolumnnumber";

		public const string AttributeDoNotEmail = "donotemail";

		public const string AttributeDoNotFax = "donotfax";

		public const string AttributeDoNotPhone = "donotphone";

		public const string AttributeDoNotPostalMail = "donotpostalmail";

		public const string AttributeEffort = "effort";

		public const string AttributeExchangeEntryId = "exchangeentryid";

		public const string AttributeInstanceTypeCode = "instancetypecode";

		public const string AttributeIsPartyDeleted = "ispartydeleted";

		public const string AttributeOwnerId = "ownerid";

		public const string AttributeParticipationTypeMask = "participationtypemask";

		public const string AttributePartyId = "partyid";

		public const string AttributeResourceSpecId = "resourcespecid";

		public const string AttributeScheduledEnd = "scheduledend";

		public const string AttributeScheduledStart = "scheduledstart";

		public const string AttributeVersionNumber = "versionnumber";

		[AttributeLogicalName("activityid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ActivityId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("activityid");
			}
			set
			{
				OnPropertyChanging("ActivityId");
				((Entity)this).SetAttributeValue("activityid", (object)value);
				OnPropertyChanged("ActivityId");
			}
		}

		[AttributeLogicalName("activitypartyid")]
		[ExcludeFromCodeCoverage]
		public Guid? ActivityPartyId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("activitypartyid");
			}
			set
			{
				OnPropertyChanging("ActivityPartyId");
				((Entity)this).SetAttributeValue("activitypartyid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("ActivityPartyId");
			}
		}

		[AttributeLogicalName("activitypartyid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				ActivityPartyId = value;
			}
		}

		[AttributeLogicalName("addressused")]
		[ExcludeFromCodeCoverage]
		public string AddressUsed
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("addressused");
			}
			set
			{
				OnPropertyChanging("AddressUsed");
				((Entity)this).SetAttributeValue("addressused", (object)value);
				OnPropertyChanged("AddressUsed");
			}
		}

		[AttributeLogicalName("addressusedemailcolumnnumber")]
		[ExcludeFromCodeCoverage]
		public int? AddressUsedEmailColumnNumber => ((Entity)this).GetAttributeValue<int?>("addressusedemailcolumnnumber");

		[AttributeLogicalName("donotemail")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotEmail => ((Entity)this).GetAttributeValue<bool?>("donotemail");

		[AttributeLogicalName("donotfax")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotFax => ((Entity)this).GetAttributeValue<bool?>("donotfax");

		[AttributeLogicalName("donotphone")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotPhone => ((Entity)this).GetAttributeValue<bool?>("donotphone");

		[AttributeLogicalName("donotpostalmail")]
		[ExcludeFromCodeCoverage]
		public bool? DoNotPostalMail => ((Entity)this).GetAttributeValue<bool?>("donotpostalmail");

		[AttributeLogicalName("effort")]
		[ExcludeFromCodeCoverage]
		public double? Effort
		{
			get
			{
				return ((Entity)this).GetAttributeValue<double?>("effort");
			}
			set
			{
				OnPropertyChanging("Effort");
				((Entity)this).SetAttributeValue("effort", (object)value);
				OnPropertyChanged("Effort");
			}
		}

		[AttributeLogicalName("exchangeentryid")]
		[ExcludeFromCodeCoverage]
		public string ExchangeEntryId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("exchangeentryid");
			}
			set
			{
				OnPropertyChanging("ExchangeEntryId");
				((Entity)this).SetAttributeValue("exchangeentryid", (object)value);
				OnPropertyChanged("ExchangeEntryId");
			}
		}

		[AttributeLogicalName("instancetypecode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue InstanceTypeCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("instancetypecode");
				if (attributeValue != null)
				{
					return attributeValue;
				}
				return null;
			}
		}

		[AttributeLogicalName("ispartydeleted")]
		[ExcludeFromCodeCoverage]
		public bool? IsPartyDeleted => ((Entity)this).GetAttributeValue<bool?>("ispartydeleted");

		[AttributeLogicalName("ownerid")]
		[ExcludeFromCodeCoverage]
		public EntityReference OwnerId => ((Entity)this).GetAttributeValue<EntityReference>("ownerid");

		[AttributeLogicalName("participationtypemask")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ParticipationTypeMask
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("participationtypemask");
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
				OnPropertyChanging("ParticipationTypeMask");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("participationtypemask", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("participationtypemask", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ParticipationTypeMask");
			}
		}

		[AttributeLogicalName("partyid")]
		[ExcludeFromCodeCoverage]
		public EntityReference PartyId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("partyid");
			}
			set
			{
				OnPropertyChanging("PartyId");
				((Entity)this).SetAttributeValue("partyid", (object)value);
				OnPropertyChanged("PartyId");
			}
		}

		[AttributeLogicalName("resourcespecid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ResourceSpecId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("resourcespecid");
			}
			set
			{
				OnPropertyChanging("ResourceSpecId");
				((Entity)this).SetAttributeValue("resourcespecid", (object)value);
				OnPropertyChanged("ResourceSpecId");
			}
		}

		[AttributeLogicalName("scheduledend")]
		[ExcludeFromCodeCoverage]
		public DateTime? ScheduledEnd => ((Entity)this).GetAttributeValue<DateTime?>("scheduledend");

		[AttributeLogicalName("scheduledstart")]
		[ExcludeFromCodeCoverage]
		public DateTime? ScheduledStart => ((Entity)this).GetAttributeValue<DateTime?>("scheduledstart");

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public ActivityParty()
			: this("activityparty")
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
