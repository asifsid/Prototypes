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
	[EntityLogicalName("recommendationmodelversion")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class RecommendationModelVersion : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "recommendationmodelversion";

		public const int EntityTypeCode = 9935;

		public const int AttributeName_MaxLength = 125;

		public const int AttributeAzureBuildId_MaxLength = 100;

		public const int AttributeLogicAppRunId_MaxLength = 100;

		public const string AttributeAzureBuildId = "azurebuildid";

		public const string AttributeAzureModelBuildStatus = "azuremodelbuildstatus";

		public const string AttributeBasketDataSynchronizationStatus = "basketdatasynchronizationstatus";

		public const string AttributeBuildEndedOn = "buildendedon";

		public const string AttributeBuildStartedOn = "buildstartedon";

		public const string AttributeCatalogCoverage = "catalogcoverage";

		public const string AttributeCatalogSynchronizationStatus = "catalogsynchronizationstatus";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDescription = "description";

		public const string AttributeDuration = "duration";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeLogicAppRunId = "logicapprunid";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributePercentileRank = "percentilerank";

		public const string AttributeRecommendationModelId = "recommendationmodelid";

		public const string AttributeRecommendationModelVersionId = "recommendationmodelversionid";

		public const string AttributeId = "recommendationmodelversionid";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeRecommendationmodel_recommendationmodelversion = "recommendationmodelid";

		[AttributeLogicalName("azurebuildid")]
		[ExcludeFromCodeCoverage]
		public string AzureBuildId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("azurebuildid");
			}
			set
			{
				OnPropertyChanging("AzureBuildId");
				((Entity)this).SetAttributeValue("azurebuildid", (object)value);
				OnPropertyChanged("AzureBuildId");
			}
		}

		[AttributeLogicalName("azuremodelbuildstatus")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue AzureModelBuildStatus
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("azuremodelbuildstatus");
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
				OnPropertyChanging("AzureModelBuildStatus");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("azuremodelbuildstatus", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("azuremodelbuildstatus", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("AzureModelBuildStatus");
			}
		}

		[AttributeLogicalName("basketdatasynchronizationstatus")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue BasketDataSynchronizationStatus
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("basketdatasynchronizationstatus");
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
				OnPropertyChanging("BasketDataSynchronizationStatus");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("basketdatasynchronizationstatus", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("basketdatasynchronizationstatus", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("BasketDataSynchronizationStatus");
			}
		}

		[AttributeLogicalName("buildendedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? BuildEndedOn
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("buildendedon");
			}
			set
			{
				OnPropertyChanging("BuildEndedOn");
				((Entity)this).SetAttributeValue("buildendedon", (object)value);
				OnPropertyChanged("BuildEndedOn");
			}
		}

		[AttributeLogicalName("buildstartedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? BuildStartedOn
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("buildstartedon");
			}
			set
			{
				OnPropertyChanging("BuildStartedOn");
				((Entity)this).SetAttributeValue("buildstartedon", (object)value);
				OnPropertyChanged("BuildStartedOn");
			}
		}

		[AttributeLogicalName("catalogcoverage")]
		[ExcludeFromCodeCoverage]
		public int? CatalogCoverage
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("catalogcoverage");
			}
			set
			{
				OnPropertyChanging("CatalogCoverage");
				((Entity)this).SetAttributeValue("catalogcoverage", (object)value);
				OnPropertyChanged("CatalogCoverage");
			}
		}

		[AttributeLogicalName("catalogsynchronizationstatus")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue CatalogSynchronizationStatus
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("catalogsynchronizationstatus");
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
				OnPropertyChanging("CatalogSynchronizationStatus");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("catalogsynchronizationstatus", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("catalogsynchronizationstatus", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("CatalogSynchronizationStatus");
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

		[AttributeLogicalName("duration")]
		[ExcludeFromCodeCoverage]
		public int? Duration => ((Entity)this).GetAttributeValue<int?>("duration");

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

		[AttributeLogicalName("logicapprunid")]
		[ExcludeFromCodeCoverage]
		public string LogicAppRunId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("logicapprunid");
			}
			set
			{
				OnPropertyChanging("LogicAppRunId");
				((Entity)this).SetAttributeValue("logicapprunid", (object)value);
				OnPropertyChanged("LogicAppRunId");
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

		[AttributeLogicalName("percentilerank")]
		[ExcludeFromCodeCoverage]
		public int? PercentileRank
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("percentilerank");
			}
			set
			{
				OnPropertyChanging("PercentileRank");
				((Entity)this).SetAttributeValue("percentilerank", (object)value);
				OnPropertyChanged("PercentileRank");
			}
		}

		[AttributeLogicalName("recommendationmodelid")]
		[ExcludeFromCodeCoverage]
		public EntityReference RecommendationModelId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("recommendationmodelid");
			}
			set
			{
				OnPropertyChanging("RecommendationModelId");
				((Entity)this).SetAttributeValue("recommendationmodelid", (object)value);
				OnPropertyChanged("RecommendationModelId");
			}
		}

		[AttributeLogicalName("recommendationmodelversionid")]
		[ExcludeFromCodeCoverage]
		public Guid? RecommendationModelVersionId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("recommendationmodelversionid");
			}
			set
			{
				OnPropertyChanging("RecommendationModelVersionId");
				((Entity)this).SetAttributeValue("recommendationmodelversionid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("RecommendationModelVersionId");
			}
		}

		[AttributeLogicalName("recommendationmodelversionid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				RecommendationModelVersionId = value;
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

		[RelationshipSchemaName("recommendationmodelversion_recommendationmodel")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<ProductRecommendationModel> recommendationmodelversion_recommendationmodel
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<ProductRecommendationModel>("recommendationmodelversion_recommendationmodel", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("recommendationmodelversion_recommendationmodel");
				((Entity)this).SetRelatedEntities<ProductRecommendationModel>("recommendationmodelversion_recommendationmodel", (EntityRole?)null, value);
				OnPropertyChanged("recommendationmodelversion_recommendationmodel");
			}
		}

		[AttributeLogicalName("recommendationmodelid")]
		[RelationshipSchemaName("recommendationmodel_recommendationmodelversion")]
		[ExcludeFromCodeCoverage]
		public ProductRecommendationModel recommendationmodel_recommendationmodelversion
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<ProductRecommendationModel>("recommendationmodel_recommendationmodelversion", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("recommendationmodel_recommendationmodelversion");
				((Entity)this).SetRelatedEntity<ProductRecommendationModel>("recommendationmodel_recommendationmodelversion", (EntityRole?)null, value);
				OnPropertyChanged("recommendationmodel_recommendationmodelversion");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public RecommendationModelVersion()
			: this("recommendationmodelversion")
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
