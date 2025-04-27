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
	[EntityLogicalName("actioncardusersettings")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class ActionCardUserSettings : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "actioncardusersettings";

		public const int EntityTypeCode = 9973;

		public const string AttributeActionCardUserSettingsId = "actioncardusersettingsid";

		public const string AttributeId = "actioncardusersettingsid";

		public const string AttributeBoolCardOption = "boolcardoption";

		public const string AttributeCardType = "cardtype";

		public const string AttributeCardTypeId = "cardtypeid";

		public const string AttributeIntCardOption = "intcardoption";

		public const string AttributeIsEnabled = "isenabled";

		public const string AttributeOwnerId = "ownerid";

		public const string AttributeOwningBusinessUnit = "owningbusinessunit";

		public const string AttributeOwningTeam = "owningteam";

		public const string AttributeOwningUser = "owninguser";

		public const string AttributeStringCardOption = "stringcardoption";

		public const string AttributeVersionNumber = "versionnumber";

		[AttributeLogicalName("actioncardusersettingsid")]
		[ExcludeFromCodeCoverage]
		public Guid? ActionCardUserSettingsId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("actioncardusersettingsid");
			}
			set
			{
				OnPropertyChanging("ActionCardUserSettingsId");
				((Entity)this).SetAttributeValue("actioncardusersettingsid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("ActionCardUserSettingsId");
			}
		}

		[AttributeLogicalName("actioncardusersettingsid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				ActionCardUserSettingsId = value;
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

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public ActionCardUserSettings()
			: this("actioncardusersettings")
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
