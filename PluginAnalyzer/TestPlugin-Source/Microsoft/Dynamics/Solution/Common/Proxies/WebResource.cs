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
	[EntityLogicalName("webresource")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class WebResource : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "webresource";

		public const int EntityTypeCode = 9333;

		public const int AttributeIntroducedVersion_MaxLength = 48;

		public const int AttributeName_MaxLength = 256;

		public const int AttributeDisplayName_MaxLength = 200;

		public const int AttributeSilverlightVersion_MaxLength = 20;

		public const int AttributeContent_MaxLength = 1073741823;

		public const string AttributeCanBeDeleted = "canbedeleted";

		public const string AttributeComponentState = "componentstate";

		public const string AttributeContent = "content";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDependencyXml = "dependencyxml";

		public const string AttributeDescription = "description";

		public const string AttributeDisplayName = "displayname";

		public const string AttributeIntroducedVersion = "introducedversion";

		public const string AttributeIsAvailableForMobileOffline = "isavailableformobileoffline";

		public const string AttributeIsCustomizable = "iscustomizable";

		public const string AttributeIsEnabledForMobileClient = "isenabledformobileclient";

		public const string AttributeIsHidden = "ishidden";

		public const string AttributeIsManaged = "ismanaged";

		public const string AttributeLanguageCode = "languagecode";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverwriteTime = "overwritetime";

		public const string AttributeSilverlightVersion = "silverlightversion";

		public const string AttributeSolutionId = "solutionid";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeWebResourceId = "webresourceid";

		public const string AttributeId = "webresourceid";

		public const string AttributeWebResourceIdUnique = "webresourceidunique";

		public const string AttributeWebResourceType = "webresourcetype";

		public const string AttributeLk_webresourcebase_createdonbehalfby = "createdonbehalfby";

		public const string AttributeLk_webresourcebase_modifiedonbehalfby = "modifiedonbehalfby";

		public const string AttributeWebresource_createdby = "createdby";

		public const string AttributeWebresource_modifiedby = "modifiedby";

		public const string AttributeWebresource_organization = "organizationid";

		[AttributeLogicalName("canbedeleted")]
		[ExcludeFromCodeCoverage]
		public BooleanManagedProperty CanBeDeleted
		{
			get
			{
				return ((Entity)this).GetAttributeValue<BooleanManagedProperty>("canbedeleted");
			}
			set
			{
				OnPropertyChanging("CanBeDeleted");
				((Entity)this).SetAttributeValue("canbedeleted", (object)value);
				OnPropertyChanged("CanBeDeleted");
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

		[AttributeLogicalName("content")]
		[ExcludeFromCodeCoverage]
		public string Content
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("content");
			}
			set
			{
				OnPropertyChanging("Content");
				((Entity)this).SetAttributeValue("content", (object)value);
				OnPropertyChanged("Content");
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

		[AttributeLogicalName("dependencyxml")]
		[ExcludeFromCodeCoverage]
		public string DependencyXml
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("dependencyxml");
			}
			set
			{
				OnPropertyChanging("DependencyXml");
				((Entity)this).SetAttributeValue("dependencyxml", (object)value);
				OnPropertyChanged("DependencyXml");
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

		[AttributeLogicalName("displayname")]
		[ExcludeFromCodeCoverage]
		public string DisplayName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("displayname");
			}
			set
			{
				OnPropertyChanging("DisplayName");
				((Entity)this).SetAttributeValue("displayname", (object)value);
				OnPropertyChanged("DisplayName");
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

		[AttributeLogicalName("isavailableformobileoffline")]
		[ExcludeFromCodeCoverage]
		public bool? IsAvailableForMobileOffline
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isavailableformobileoffline");
			}
			set
			{
				OnPropertyChanging("IsAvailableForMobileOffline");
				((Entity)this).SetAttributeValue("isavailableformobileoffline", (object)value);
				OnPropertyChanged("IsAvailableForMobileOffline");
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

		[AttributeLogicalName("isenabledformobileclient")]
		[ExcludeFromCodeCoverage]
		public bool? IsEnabledForMobileClient
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isenabledformobileclient");
			}
			set
			{
				OnPropertyChanging("IsEnabledForMobileClient");
				((Entity)this).SetAttributeValue("isenabledformobileclient", (object)value);
				OnPropertyChanged("IsEnabledForMobileClient");
			}
		}

		[AttributeLogicalName("ishidden")]
		[ExcludeFromCodeCoverage]
		public BooleanManagedProperty IsHidden
		{
			get
			{
				return ((Entity)this).GetAttributeValue<BooleanManagedProperty>("ishidden");
			}
			set
			{
				OnPropertyChanging("IsHidden");
				((Entity)this).SetAttributeValue("ishidden", (object)value);
				OnPropertyChanged("IsHidden");
			}
		}

		[AttributeLogicalName("ismanaged")]
		[ExcludeFromCodeCoverage]
		public bool? IsManaged => ((Entity)this).GetAttributeValue<bool?>("ismanaged");

		[AttributeLogicalName("languagecode")]
		[ExcludeFromCodeCoverage]
		public int? LanguageCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("languagecode");
			}
			set
			{
				OnPropertyChanging("LanguageCode");
				((Entity)this).SetAttributeValue("languagecode", (object)value);
				OnPropertyChanged("LanguageCode");
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

		[AttributeLogicalName("overwritetime")]
		[ExcludeFromCodeCoverage]
		public DateTime? OverwriteTime => ((Entity)this).GetAttributeValue<DateTime?>("overwritetime");

		[AttributeLogicalName("silverlightversion")]
		[ExcludeFromCodeCoverage]
		public string SilverlightVersion
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("silverlightversion");
			}
			set
			{
				OnPropertyChanging("SilverlightVersion");
				((Entity)this).SetAttributeValue("silverlightversion", (object)value);
				OnPropertyChanged("SilverlightVersion");
			}
		}

		[AttributeLogicalName("solutionid")]
		[ExcludeFromCodeCoverage]
		public Guid? SolutionId => ((Entity)this).GetAttributeValue<Guid?>("solutionid");

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[AttributeLogicalName("webresourceid")]
		[ExcludeFromCodeCoverage]
		public Guid? WebResourceId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("webresourceid");
			}
			set
			{
				OnPropertyChanging("WebResourceId");
				((Entity)this).SetAttributeValue("webresourceid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("WebResourceId");
			}
		}

		[AttributeLogicalName("webresourceid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				WebResourceId = value;
			}
		}

		[AttributeLogicalName("webresourceidunique")]
		[ExcludeFromCodeCoverage]
		public Guid? WebResourceIdUnique => ((Entity)this).GetAttributeValue<Guid?>("webresourceidunique");

		[AttributeLogicalName("webresourcetype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue WebResourceType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("webresourcetype");
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
				OnPropertyChanging("WebResourceType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("webresourcetype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("webresourcetype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("WebResourceType");
			}
		}

		[AttributeLogicalName("createdonbehalfby")]
		[RelationshipSchemaName("lk_webresourcebase_createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public User lk_webresourcebase_createdonbehalfby => ((Entity)this).GetRelatedEntity<User>("lk_webresourcebase_createdonbehalfby", (EntityRole?)null);

		[AttributeLogicalName("modifiedonbehalfby")]
		[RelationshipSchemaName("lk_webresourcebase_modifiedonbehalfby")]
		[ExcludeFromCodeCoverage]
		public User lk_webresourcebase_modifiedonbehalfby => ((Entity)this).GetRelatedEntity<User>("lk_webresourcebase_modifiedonbehalfby", (EntityRole?)null);

		[AttributeLogicalName("createdby")]
		[RelationshipSchemaName("webresource_createdby")]
		[ExcludeFromCodeCoverage]
		public User webresource_createdby => ((Entity)this).GetRelatedEntity<User>("webresource_createdby", (EntityRole?)null);

		[AttributeLogicalName("modifiedby")]
		[RelationshipSchemaName("webresource_modifiedby")]
		[ExcludeFromCodeCoverage]
		public User webresource_modifiedby => ((Entity)this).GetRelatedEntity<User>("webresource_modifiedby", (EntityRole?)null);

		[AttributeLogicalName("organizationid")]
		[RelationshipSchemaName("webresource_organization")]
		[ExcludeFromCodeCoverage]
		public Organization webresource_organization => ((Entity)this).GetRelatedEntity<Organization>("webresource_organization", (EntityRole?)null);

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public WebResource()
			: this("webresource")
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
