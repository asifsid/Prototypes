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
	[EntityLogicalName("solution")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Solution : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "solution";

		public const int EntityTypeCode = 7100;

		public const int AttributeFriendlyName_MaxLength = 256;

		public const int AttributeSolutionPackageVersion_MaxLength = 256;

		public const int AttributePinpointSolutionDefaultLocale_MaxLength = 16;

		public const int AttributePinpointAssetId_MaxLength = 255;

		public const int AttributeVersion_MaxLength = 256;

		public const int AttributeUniqueName_MaxLength = 65;

		public const int AttributeDescription_MaxLength = 2000;

		public const string AttributeConfigurationPageId = "configurationpageid";

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeDescription = "description";

		public const string AttributeFriendlyName = "friendlyname";

		public const string AttributeInstalledOn = "installedon";

		public const string AttributeIsManaged = "ismanaged";

		public const string AttributeIsVisible = "isvisible";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeParentSolutionId = "parentsolutionid";

		public const string AttributePinpointAssetId = "pinpointassetid";

		public const string AttributePinpointPublisherId = "pinpointpublisherid";

		public const string AttributePinpointSolutionDefaultLocale = "pinpointsolutiondefaultlocale";

		public const string AttributePinpointSolutionId = "pinpointsolutionid";

		public const string AttributePublisherId = "publisherid";

		public const string AttributeSolutionId = "solutionid";

		public const string AttributeId = "solutionid";

		public const string AttributeSolutionPackageVersion = "solutionpackageversion";

		public const string AttributeSolutionType = "solutiontype";

		public const string AttributeUniqueName = "uniquename";

		public const string AttributeVersion = "version";

		public const string AttributeVersionNumber = "versionnumber";

		public const string AttributeReferencingsolution_parent_solution = "parentsolutionid";

		[AttributeLogicalName("configurationpageid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ConfigurationPageId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("configurationpageid");
			}
			set
			{
				OnPropertyChanging("ConfigurationPageId");
				((Entity)this).SetAttributeValue("configurationpageid", (object)value);
				OnPropertyChanged("ConfigurationPageId");
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

		[AttributeLogicalName("friendlyname")]
		[ExcludeFromCodeCoverage]
		public string FriendlyName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("friendlyname");
			}
			set
			{
				OnPropertyChanging("FriendlyName");
				((Entity)this).SetAttributeValue("friendlyname", (object)value);
				OnPropertyChanged("FriendlyName");
			}
		}

		[AttributeLogicalName("installedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? InstalledOn => ((Entity)this).GetAttributeValue<DateTime?>("installedon");

		[AttributeLogicalName("ismanaged")]
		[ExcludeFromCodeCoverage]
		public bool? IsManaged => ((Entity)this).GetAttributeValue<bool?>("ismanaged");

		[AttributeLogicalName("isvisible")]
		[ExcludeFromCodeCoverage]
		public bool? IsVisible => ((Entity)this).GetAttributeValue<bool?>("isvisible");

		[AttributeLogicalName("modifiedby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedby");

		[AttributeLogicalName("modifiedon")]
		[ExcludeFromCodeCoverage]
		public DateTime? ModifiedOn => ((Entity)this).GetAttributeValue<DateTime?>("modifiedon");

		[AttributeLogicalName("modifiedonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference ModifiedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("modifiedonbehalfby");

		[AttributeLogicalName("organizationid")]
		[ExcludeFromCodeCoverage]
		public EntityReference OrganizationId => ((Entity)this).GetAttributeValue<EntityReference>("organizationid");

		[AttributeLogicalName("parentsolutionid")]
		[ExcludeFromCodeCoverage]
		public EntityReference ParentSolutionId => ((Entity)this).GetAttributeValue<EntityReference>("parentsolutionid");

		[AttributeLogicalName("pinpointassetid")]
		[ExcludeFromCodeCoverage]
		public string PinpointAssetId => ((Entity)this).GetAttributeValue<string>("pinpointassetid");

		[AttributeLogicalName("pinpointpublisherid")]
		[ExcludeFromCodeCoverage]
		public long? PinpointPublisherId => ((Entity)this).GetAttributeValue<long?>("pinpointpublisherid");

		[AttributeLogicalName("pinpointsolutiondefaultlocale")]
		[ExcludeFromCodeCoverage]
		public string PinpointSolutionDefaultLocale => ((Entity)this).GetAttributeValue<string>("pinpointsolutiondefaultlocale");

		[AttributeLogicalName("pinpointsolutionid")]
		[ExcludeFromCodeCoverage]
		public long? PinpointSolutionId => ((Entity)this).GetAttributeValue<long?>("pinpointsolutionid");

		[AttributeLogicalName("publisherid")]
		[ExcludeFromCodeCoverage]
		public EntityReference PublisherId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<EntityReference>("publisherid");
			}
			set
			{
				OnPropertyChanging("PublisherId");
				((Entity)this).SetAttributeValue("publisherid", (object)value);
				OnPropertyChanged("PublisherId");
			}
		}

		[AttributeLogicalName("solutionid")]
		[ExcludeFromCodeCoverage]
		public Guid? SolutionId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("solutionid");
			}
			set
			{
				OnPropertyChanging("SolutionId");
				((Entity)this).SetAttributeValue("solutionid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("SolutionId");
			}
		}

		[AttributeLogicalName("solutionid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				SolutionId = value;
			}
		}

		[AttributeLogicalName("solutionpackageversion")]
		[ExcludeFromCodeCoverage]
		public string SolutionPackageVersion
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("solutionpackageversion");
			}
			set
			{
				OnPropertyChanging("SolutionPackageVersion");
				((Entity)this).SetAttributeValue("solutionpackageversion", (object)value);
				OnPropertyChanged("SolutionPackageVersion");
			}
		}

		[AttributeLogicalName("solutiontype")]
		[ExcludeFromCodeCoverage]
		public OptionSetValue SolutionType
		{
			get
			{
				OptionSetValue attributeValue = ((Entity)this).GetAttributeValue<OptionSetValue>("solutiontype");
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
				OnPropertyChanging("SolutionType");
				if (value == null)
				{
					((Entity)this).SetAttributeValue("solutiontype", (object)null);
				}
				else
				{
					((Entity)this).SetAttributeValue("solutiontype", (object)new OptionSetValue(value.get_Value()));
				}
				OnPropertyChanged("SolutionType");
			}
		}

		[AttributeLogicalName("uniquename")]
		[ExcludeFromCodeCoverage]
		public string UniqueName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("uniquename");
			}
			set
			{
				OnPropertyChanging("UniqueName");
				((Entity)this).SetAttributeValue("uniquename", (object)value);
				OnPropertyChanged("UniqueName");
			}
		}

		[AttributeLogicalName("version")]
		[ExcludeFromCodeCoverage]
		public string Version
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("version");
			}
			set
			{
				OnPropertyChanging("Version");
				((Entity)this).SetAttributeValue("version", (object)value);
				OnPropertyChanged("Version");
			}
		}

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public IEnumerable<Solution> Referencedsolution_parent_solution
		{
			get
			{
				return ((Entity)this).GetRelatedEntities<Solution>("solution_parent_solution", (EntityRole?)(EntityRole)1);
			}
			set
			{
				OnPropertyChanging("Referencedsolution_parent_solution");
				((Entity)this).SetRelatedEntities<Solution>("solution_parent_solution", (EntityRole?)(EntityRole)1, value);
				OnPropertyChanged("Referencedsolution_parent_solution");
			}
		}

		[AttributeLogicalName("parentsolutionid")]
		[RelationshipSchemaName(/*Could not decode attribute arguments.*/)]
		[ExcludeFromCodeCoverage]
		public Solution Referencingsolution_parent_solution => ((Entity)this).GetRelatedEntity<Solution>("solution_parent_solution", (EntityRole?)(EntityRole)0);

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Solution()
			: this("solution")
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
