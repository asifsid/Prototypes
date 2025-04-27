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
	[EntityLogicalName("recommendationcache")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class RecommendationCache : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "recommendationcache";

		public const int EntityTypeCode = 9938;

		public const int AttributeName_MaxLength = 100;

		public const string AttributeAdditionalDataRecordId = "additionaldatarecordid";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeIsRecommendationActive = "isrecommendationactive";

		public const string AttributeItemId = "itemid";

		public const string AttributeName = "name";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeRecommendationCacheId = "RecommendationCacheid";

		public const string AttributeId = "RecommendationCacheid";

		public const string AttributeRecommendationModelId = "recommendationmodelid";

		public const string AttributeRecommendationRating = "recommendationrating";

		public const string AttributeRecommendationSource = "recommendationsource";

		public const string AttributeRecommendationType = "recommendationtype";

		public const string AttributeRecommendedItemId = "recommendeditemid";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeVersionNumber = "versionnumber";

		[AttributeLogicalName("additionaldatarecordid")]
		[ExcludeFromCodeCoverage]
		public EntityReference AdditionalDataRecordId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("additionaldatarecordid");
			}
			set
			{
				OnPropertyChanging("AdditionalDataRecordId");
				((Entity)this).SetAttributeValue("additionaldatarecordid", (object)value);
				OnPropertyChanged("AdditionalDataRecordId");
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

		[AttributeLogicalName("isrecommendationactive")]
		[ExcludeFromCodeCoverage]
		public bool? IsRecommendationActive
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("isrecommendationactive");
			}
			set
			{
				OnPropertyChanging("IsRecommendationActive");
				((Entity)this).SetAttributeValue("isrecommendationactive", (object)value);
				OnPropertyChanged("IsRecommendationActive");
			}
		}

		[AttributeLogicalName("itemid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ItemId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("itemid");
			}
			set
			{
				OnPropertyChanging("ItemId");
				((Entity)this).SetAttributeValue("itemid", (object)value);
				OnPropertyChanged("ItemId");
			}
		}

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

		[AttributeLogicalName("RecommendationCacheid")]
		[ExcludeFromCodeCoverage]
		public Guid? RecommendationCacheId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("RecommendationCacheid");
			}
			set
			{
				OnPropertyChanging("RecommendationCacheId");
				((Entity)this).SetAttributeValue("RecommendationCacheid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("RecommendationCacheId");
			}
		}

		[AttributeLogicalName("RecommendationCacheid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				RecommendationCacheId = value;
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

		[AttributeLogicalName("recommendationrating")]
		[ExcludeFromCodeCoverage]
		public decimal? RecommendationRating
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("recommendationrating");
			}
			set
			{
				OnPropertyChanging("RecommendationRating");
				((Entity)this).SetAttributeValue("recommendationrating", (object)value);
				OnPropertyChanged("RecommendationRating");
			}
		}

		[AttributeLogicalName("recommendationsource")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue RecommendationSource
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("recommendationsource");
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
				OnPropertyChanging("RecommendationSource");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("recommendationsource", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("recommendationsource", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("RecommendationSource");
			}
		}

		[AttributeLogicalName("recommendationtype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue RecommendationType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("recommendationtype");
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
				OnPropertyChanging("RecommendationType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("recommendationtype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("recommendationtype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("RecommendationType");
			}
		}

		[AttributeLogicalName("recommendeditemid")]
		[ExcludeFromCodeCoverage]
		public EntityReference RecommendedItemId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("recommendeditemid");
			}
			set
			{
				OnPropertyChanging("RecommendedItemId");
				((Entity)this).SetAttributeValue("recommendeditemid", (object)value);
				OnPropertyChanged("RecommendedItemId");
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

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public RecommendationCache()
			: this("recommendationcache")
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
