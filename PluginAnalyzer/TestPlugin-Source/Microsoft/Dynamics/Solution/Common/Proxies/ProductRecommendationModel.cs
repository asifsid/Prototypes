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
	[EntityLogicalName("recommendationmodel")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class ProductRecommendationModel : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "recommendationmodel";

		public const string ReadProductPrivelege = "prvReadProduct";

		public const int EntityTypeCode = 9933;

		public const int AttributeName_MaxLength = 100;

		public const int AttributeAzureModelId_MaxLength = 100;

		public const int AttributeProductCatalogName_MaxLength = 100;

		public const string AttributeAzureModelId = "azuremodelid";

		public const string AttributeAzureServiceConnectionId = "azureserviceconnectionid";

		public const string AttributeBasketDataLastSynchronizationStatus = "basketdatalastsynchronizationstatus";

		public const string AttributeBasketDataLastSynchronizedOn = "basketdatalastsynchronizedon";

		public const string AttributeCatalogLastSynchronizationStatus = "cataloglastsynchronizationstatus";

		public const string AttributeCatalogLastSynchronizedOn = "cataloglastsynchronizedon";

		public const string AttributeComponentState = "componentstate";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDescription = "description";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIsManaged = "ismanaged";

		public const string AttributeMaximumVersions = "maximumversions";

		public const string AttributeMaxRecommendations = "maxrecommendations";

		public const string AttributeMinRecommendationRating = "minrecommendationrating";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeOverwriteTime = "overwritetime";

		public const string AttributeProductCatalogAccessoryLinkRating = "productcatalogaccessorylinkrating";

		public const string AttributeProductCatalogCrosssellLinkRating = "productcatalogcrossselllinkrating";

		public const string AttributeProductCatalogName = "productcatalogname";

		public const string AttributeRecommendationModelId = "recommendationmodelid";

		public const string AttributeId = "recommendationmodelid";

		public const string AttributeRecommendationModelIdUnique = "recommendationmodelidunique";

		public const string AttributeRecommendationModelVersionCount = "recommendationmodelversioncount";

		public const string AttributeRecommendationModelVersionId = "recommendationmodelversionid";

		public const string AttributeSolutionId = "solutionid";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeValidUntil = "validuntil";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeRecommendationmodelversion_recommendationmodel = "recommendationmodelversionid";

		[AttributeLogicalName("azuremodelid")]
		[ExcludeFromCodeCoverage]
		public string AzureModelId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("azuremodelid");
			}
			set
			{
				OnPropertyChanging("AzureModelId");
				((Entity)this).SetAttributeValue("azuremodelid", (object)value);
				OnPropertyChanged("AzureModelId");
			}
		}

		[AttributeLogicalName("azureserviceconnectionid")]
		[ExcludeFromCodeCoverage]
		public EntityReference AzureServiceConnectionId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("azureserviceconnectionid");
			}
			set
			{
				OnPropertyChanging("AzureServiceConnectionId");
				((Entity)this).SetAttributeValue("azureserviceconnectionid", (object)value);
				OnPropertyChanged("AzureServiceConnectionId");
			}
		}

		[AttributeLogicalName("basketdatalastsynchronizationstatus")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue BasketDataLastSynchronizationStatus
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("basketdatalastsynchronizationstatus");
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
				OnPropertyChanging("BasketDataLastSynchronizationStatus");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("basketdatalastsynchronizationstatus", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("basketdatalastsynchronizationstatus", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("BasketDataLastSynchronizationStatus");
			}
		}

		[AttributeLogicalName("basketdatalastsynchronizedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? BasketDataLastSynchronizedOn
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("basketdatalastsynchronizedon");
			}
			set
			{
				OnPropertyChanging("BasketDataLastSynchronizedOn");
				((Entity)this).SetAttributeValue("basketdatalastsynchronizedon", (object)value);
				OnPropertyChanged("BasketDataLastSynchronizedOn");
			}
		}

		[AttributeLogicalName("cataloglastsynchronizationstatus")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue CatalogLastSynchronizationStatus
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("cataloglastsynchronizationstatus");
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
				OnPropertyChanging("CatalogLastSynchronizationStatus");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("cataloglastsynchronizationstatus", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("cataloglastsynchronizationstatus", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("CatalogLastSynchronizationStatus");
			}
		}

		[AttributeLogicalName("cataloglastsynchronizedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? CatalogLastSynchronizedOn
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("cataloglastsynchronizedon");
			}
			set
			{
				OnPropertyChanging("CatalogLastSynchronizedOn");
				((Entity)this).SetAttributeValue("cataloglastsynchronizedon", (object)value);
				OnPropertyChanged("CatalogLastSynchronizedOn");
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

		[AttributeLogicalName("ismanaged")]
		[ExcludeFromCodeCoverage]
		public bool? IsManaged => ((Entity)this).GetAttributeValue<bool?>("ismanaged");

		[AttributeLogicalName("maximumversions")]
		[ExcludeFromCodeCoverage]
		public int? MaximumVersions
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maximumversions");
			}
			set
			{
				OnPropertyChanging("MaximumVersions");
				((Entity)this).SetAttributeValue("maximumversions", (object)value);
				OnPropertyChanged("MaximumVersions");
			}
		}

		[AttributeLogicalName("maxrecommendations")]
		[ExcludeFromCodeCoverage]
		public int? MaxRecommendations
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("maxrecommendations");
			}
			set
			{
				OnPropertyChanging("MaxRecommendations");
				((Entity)this).SetAttributeValue("maxrecommendations", (object)value);
				OnPropertyChanged("MaxRecommendations");
			}
		}

		[AttributeLogicalName("minrecommendationrating")]
		[ExcludeFromCodeCoverage]
		public decimal? MinRecommendationRating
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("minrecommendationrating");
			}
			set
			{
				OnPropertyChanging("MinRecommendationRating");
				((Entity)this).SetAttributeValue("minrecommendationrating", (object)value);
				OnPropertyChanged("MinRecommendationRating");
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

		[AttributeLogicalName("overwritetime")]
		[ExcludeFromCodeCoverage]
		public DateTime? OverwriteTime => ((Entity)this).GetAttributeValue<DateTime?>("overwritetime");

		[AttributeLogicalName("productcatalogaccessorylinkrating")]
		[ExcludeFromCodeCoverage]
		public decimal? ProductCatalogAccessoryLinkRating
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("productcatalogaccessorylinkrating");
			}
			set
			{
				OnPropertyChanging("ProductCatalogAccessoryLinkRating");
				((Entity)this).SetAttributeValue("productcatalogaccessorylinkrating", (object)value);
				OnPropertyChanged("ProductCatalogAccessoryLinkRating");
			}
		}

		[AttributeLogicalName("productcatalogcrossselllinkrating")]
		[ExcludeFromCodeCoverage]
		public decimal? ProductCatalogCrosssellLinkRating
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("productcatalogcrossselllinkrating");
			}
			set
			{
				OnPropertyChanging("ProductCatalogCrosssellLinkRating");
				((Entity)this).SetAttributeValue("productcatalogcrossselllinkrating", (object)value);
				OnPropertyChanged("ProductCatalogCrosssellLinkRating");
			}
		}

		[AttributeLogicalName("productcatalogname")]
		[ExcludeFromCodeCoverage]
		public string ProductCatalogName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("productcatalogname");
			}
			set
			{
				OnPropertyChanging("ProductCatalogName");
				((Entity)this).SetAttributeValue("productcatalogname", (object)value);
				OnPropertyChanged("ProductCatalogName");
			}
		}

		[AttributeLogicalName("recommendationmodelid")]
		[ExcludeFromCodeCoverage]
		public Guid? RecommendationModelId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("recommendationmodelid");
			}
			set
			{
				OnPropertyChanging("RecommendationModelId");
				((Entity)this).SetAttributeValue("recommendationmodelid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("RecommendationModelId");
			}
		}

		[AttributeLogicalName("recommendationmodelid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				RecommendationModelId = value;
			}
		}

		[AttributeLogicalName("recommendationmodelidunique")]
		[ExcludeFromCodeCoverage]
		public Guid? RecommendationModelIdUnique => ((Entity)this).GetAttributeValue<Guid?>("recommendationmodelidunique");

		[AttributeLogicalName("recommendationmodelversioncount")]
		[ExcludeFromCodeCoverage]
		public int? RecommendationModelVersionCount
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("recommendationmodelversioncount");
			}
			set
			{
				OnPropertyChanging("RecommendationModelVersionCount");
				((Entity)this).SetAttributeValue("recommendationmodelversioncount", (object)value);
				OnPropertyChanged("RecommendationModelVersionCount");
			}
		}

		[AttributeLogicalName("recommendationmodelversionid")]
		[ExcludeFromCodeCoverage]
		public EntityReference RecommendationModelVersionId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("recommendationmodelversionid");
			}
			set
			{
				OnPropertyChanging("RecommendationModelVersionId");
				((Entity)this).SetAttributeValue("recommendationmodelversionid", (object)value);
				OnPropertyChanged("RecommendationModelVersionId");
			}
		}

		[AttributeLogicalName("solutionid")]
		[ExcludeFromCodeCoverage]
		public Guid? SolutionId => ((Entity)this).GetAttributeValue<Guid?>("solutionid");

		[AttributeLogicalName("statecode")]
		[ExcludeFromCodeCoverage]
		public RecommendationModelState? StateCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (RecommendationModelState)Enum.ToObject(typeof(RecommendationModelState), attributeValue.get_Value());
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

		[AttributeLogicalName("validuntil")]
		[ExcludeFromCodeCoverage]
		public DateTime? ValidUntil
		{
			get
			{
				return ((Entity)this).GetAttributeValue<DateTime?>("validuntil");
			}
			set
			{
				OnPropertyChanging("ValidUntil");
				((Entity)this).SetAttributeValue("validuntil", (object)value);
				OnPropertyChanged("ValidUntil");
			}
		}

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[RelationshipSchemaName("recommendationmodel_recommendationmodelversion")]
		[ExcludeFromCodeCoverage]
		public IEnumerable<RecommendationModelVersion> recommendationmodel_recommendationmodelversion
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<RecommendationModelVersion>("recommendationmodel_recommendationmodelversion", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("recommendationmodel_recommendationmodelversion");
				((Entity)this).SetRelatedEntities<RecommendationModelVersion>("recommendationmodel_recommendationmodelversion", (EntityRole?)null, value);
				OnPropertyChanged("recommendationmodel_recommendationmodelversion");
			}
		}

		[AttributeLogicalName("recommendationmodelversionid")]
		[RelationshipSchemaName("recommendationmodelversion_recommendationmodel")]
		[ExcludeFromCodeCoverage]
		public RecommendationModelVersion recommendationmodelversion_recommendationmodel
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<RecommendationModelVersion>("recommendationmodelversion_recommendationmodel", (EntityRole?)null);
			}
			set
			{
				OnPropertyChanging("recommendationmodelversion_recommendationmodel");
				((Entity)this).SetRelatedEntity<RecommendationModelVersion>("recommendationmodelversion_recommendationmodel", (EntityRole?)null, value);
				OnPropertyChanged("recommendationmodelversion_recommendationmodel");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public ProductRecommendationModel()
			: this("recommendationmodel")
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
