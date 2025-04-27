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
	[EntityLogicalName("azureserviceconnection")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class AzureServiceConnection : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "azureserviceconnection";

		public const int EntityTypeCode = 9936;

		public const int AttributeAccountKey_MaxLength = 100;

		public const int AttributeName_MaxLength = 160;

		public const int AttributeServiceUri_MaxLength = 500;

		public const string AttributeAccountKey = "accountkey";

		public const string AttributeAzureServiceConnectionId = "azureserviceconnectionid";

		public const string AttributeId = "azureserviceconnectionid";

		public const string AttributeConnectionType = "connectiontype";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDescription = "description";

		public const string AttributeLastConnectionStatusCode = "lastconnectionstatuscode";

		public const string AttributeLastConnectionTime = "lastconnectiontime";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeServiceUri = "serviceuri";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		[AttributeLogicalName("accountkey")]
		[ExcludeFromCodeCoverage]
		public string AccountKey
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("accountkey");
			}
			set
			{
				OnPropertyChanging("AccountKey");
				((Entity)this).SetAttributeValue("accountkey", (object)value);
				OnPropertyChanged("AccountKey");
			}
		}

		[AttributeLogicalName("azureserviceconnectionid")]
		[ExcludeFromCodeCoverage]
		public Guid? AzureServiceConnectionId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("azureserviceconnectionid");
			}
			set
			{
				OnPropertyChanging("AzureServiceConnectionId");
				((Entity)this).SetAttributeValue("azureserviceconnectionid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("AzureServiceConnectionId");
			}
		}

		[AttributeLogicalName("azureserviceconnectionid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				AzureServiceConnectionId = value;
			}
		}

		[AttributeLogicalName("connectiontype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ConnectionType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("connectiontype");
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
				OnPropertyChanging("ConnectionType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("connectiontype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("connectiontype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ConnectionType");
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

		[AttributeLogicalName("lastconnectionstatuscode")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue LastConnectionStatusCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("lastconnectionstatuscode");
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
				OnPropertyChanging("LastConnectionStatusCode");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("lastconnectionstatuscode", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("lastconnectionstatuscode", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("LastConnectionStatusCode");
			}
		}

		[AttributeLogicalName("lastconnectiontime")]
		[ExcludeFromCodeCoverage]
		public DateTime? LastConnectionTime
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("lastconnectiontime");
			}
			set
			{
				OnPropertyChanging("LastConnectionTime");
				((Entity)this).SetAttributeValue("lastconnectiontime", (object)value);
				OnPropertyChanged("LastConnectionTime");
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

		[AttributeLogicalName("serviceuri")]
		[ExcludeFromCodeCoverage]
		public string ServiceUri
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("serviceuri");
			}
			set
			{
				OnPropertyChanging("ServiceUri");
				((Entity)this).SetAttributeValue("serviceuri", (object)value);
				OnPropertyChanged("ServiceUri");
			}
		}

		[AttributeLogicalName("statecode")]
		[ExcludeFromCodeCoverage]
		public AzureServiceConnectionState? StateCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (AzureServiceConnectionState)Enum.ToObject(typeof(AzureServiceConnectionState), attributeValue.get_Value());
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

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public AzureServiceConnection()
			: this("azureserviceconnection")
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
