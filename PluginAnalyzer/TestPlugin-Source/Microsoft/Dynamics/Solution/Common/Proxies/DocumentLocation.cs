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
	[EntityLogicalName("sharepointdocumentlocation")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class DocumentLocation : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "sharepointdocumentlocation";

		public const int EntityTypeCode = 9508;

		public const int AttributeName_MaxLength = 160;

		public const int AttributeAbsoluteURL_MaxLength = 2000;

		public const int AttributeRelativeUrl_MaxLength = 255;

		public const string AttributeAbsoluteURL = "absoluteurl";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDescription = "description";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeLocationType = "locationtype";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeName = "name";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeOwnerId = "ownerid";

		public const string AttributeOwningBusinessUnit = "owningbusinessunit";

		public const string AttributeOwningTeam = "owningteam";

		public const string AttributeOwningUser = "owninguser";

		public const string AttributeParentSiteOrLocation = "parentsiteorlocation";

		public const string AttributeRegardingObjectId = "regardingobjectid";

		public const string AttributeRelativeUrl = "relativeurl";

		public const string AttributeServiceType = "servicetype";

		public const string AttributeSharePointDocumentLocationId = "sharepointdocumentlocationid";

		public const string AttributeId = "sharepointdocumentlocationid";

		public const string AttributeSiteCollectionId = "sitecollectionid";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeTimeZoneRuleVersionNumber = "timezoneruleversionnumber";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeUserId = "userid";

		public const string AttributeUTCConversionTimeZoneCode = "utcconversiontimezonecode";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeReferencingsharepointdocumentlocation_parent_sharepointdocumentlocation = "parentsiteorlocation";

		[AttributeLogicalName("absoluteurl")]
		[ExcludeFromCodeCoverage]
		public string AbsoluteURL
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("absoluteurl");
			}
			set
			{
				OnPropertyChanging("AbsoluteURL");
				((Entity)this).SetAttributeValue("absoluteurl", (object)value);
				OnPropertyChanged("AbsoluteURL");
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

		[AttributeLogicalName("locationtype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue LocationType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("locationtype");
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
				OnPropertyChanging("LocationType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("locationtype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("locationtype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("LocationType");
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

		[AttributeLogicalName("parentsiteorlocation")]
		[ExcludeFromCodeCoverage]
		public EntityReference ParentSiteOrLocation
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("parentsiteorlocation");
			}
			set
			{
				OnPropertyChanging("ParentSiteOrLocation");
				((Entity)this).SetAttributeValue("parentsiteorlocation", (object)value);
				OnPropertyChanged("ParentSiteOrLocation");
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

		[AttributeLogicalName("relativeurl")]
		[ExcludeFromCodeCoverage]
		public string RelativeUrl
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("relativeurl");
			}
			set
			{
				OnPropertyChanging("RelativeUrl");
				((Entity)this).SetAttributeValue("relativeurl", (object)value);
				OnPropertyChanged("RelativeUrl");
			}
		}

		[AttributeLogicalName("servicetype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue ServiceType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("servicetype");
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
				OnPropertyChanging("ServiceType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("servicetype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("servicetype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("ServiceType");
			}
		}

		[AttributeLogicalName("sharepointdocumentlocationid")]
		[ExcludeFromCodeCoverage]
		public Guid? SharePointDocumentLocationId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("sharepointdocumentlocationid");
			}
			set
			{
				OnPropertyChanging("SharePointDocumentLocationId");
				((Entity)this).SetAttributeValue("sharepointdocumentlocationid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("SharePointDocumentLocationId");
			}
		}

		[AttributeLogicalName("sharepointdocumentlocationid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				SharePointDocumentLocationId = value;
			}
		}

		[AttributeLogicalName("sitecollectionid")]
		[ExcludeFromCodeCoverage]
		public Guid? SiteCollectionId => ((Entity)this).GetAttributeValue<Guid?>("sitecollectionid");

		[AttributeLogicalName("statecode")]
		[ExcludeFromCodeCoverage]
		public SharePointDocumentLocationState? StateCode
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("statecode");
				if (attributeValue != null)
				{
					return (SharePointDocumentLocationState)Enum.ToObject(typeof(SharePointDocumentLocationState), attributeValue.get_Value());
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

		[AttributeLogicalName("transactioncurrencyid")]
		[ExcludeFromCodeCoverage]
		public EntityReference TransactionCurrencyId => ((Entity)this).GetAttributeValue<EntityReference>("transactioncurrencyid");

		[AttributeLogicalName("userid")]
		[ExcludeFromCodeCoverage]
		public Guid? UserId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("userid");
			}
			set
			{
				OnPropertyChanging("UserId");
				((Entity)this).SetAttributeValue("userid", (object)value);
				OnPropertyChanged("UserId");
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

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<DocumentLocation> Referencedsharepointdocumentlocation_parent_sharepointdocumentlocation
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<DocumentLocation>("sharepointdocumentlocation_parent_sharepointdocumentlocation", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedsharepointdocumentlocation_parent_sharepointdocumentlocation");
				((Entity)this).SetRelatedEntities<DocumentLocation>("sharepointdocumentlocation_parent_sharepointdocumentlocation", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedsharepointdocumentlocation_parent_sharepointdocumentlocation");
			}
		}

		[AttributeLogicalName("parentsiteorlocation")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public DocumentLocation Referencingsharepointdocumentlocation_parent_sharepointdocumentlocation
		{
			get
			{
				return ((Entity)this).GetRelatedEntity<DocumentLocation>("sharepointdocumentlocation_parent_sharepointdocumentlocation", (EntityRole?)(EntityRole)0);
			}
			set
			{
				OnPropertyChanging("Referencingsharepointdocumentlocation_parent_sharepointdocumentlocation");
				((Entity)this).SetRelatedEntity<DocumentLocation>("sharepointdocumentlocation_parent_sharepointdocumentlocation", (EntityRole?)(EntityRole)0, value);
				OnPropertyChanged("Referencingsharepointdocumentlocation_parent_sharepointdocumentlocation");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public DocumentLocation()
			: this("sharepointdocumentlocation")
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
