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
	[EntityLogicalName("privilege")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Privilege : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "privilege";

		public const int EntityTypeCode = 1023;

		public const int AttributeName_MaxLength = 100;

		public const string AttributeAccessRight = "accessright";

		public const string AttributeCanBeBasic = "canbebasic";

		public const string AttributeCanBeDeep = "canbedeep";

		public const string AttributeCanBeEntityReference = "canbeentityreference";

		public const string AttributeCanBeGlobal = "canbeglobal";

		public const string AttributeCanBeLocal = "canbelocal";

		public const string AttributeCanBeParentEntityReference = "canbeparententityreference";

		public const string AttributeName = "name";

		public const string AttributePrivilegeId = "privilegeid";

		public const string AttributeId = "privilegeid";

		public const string AttributeVersionNumber = "versionnumber";

		[AttributeLogicalName("accessright")]
		[ExcludeFromCodeCoverage]
		public int? AccessRight
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("accessright");
			}
			set
			{
				OnPropertyChanging("AccessRight");
				((Entity)this).SetAttributeValue("accessright", (object)value);
				OnPropertyChanged("AccessRight");
			}
		}

		[AttributeLogicalName("canbebasic")]
		[ExcludeFromCodeCoverage]
		public bool? CanBeBasic
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("canbebasic");
			}
			set
			{
				OnPropertyChanging("CanBeBasic");
				((Entity)this).SetAttributeValue("canbebasic", (object)value);
				OnPropertyChanged("CanBeBasic");
			}
		}

		[AttributeLogicalName("canbedeep")]
		[ExcludeFromCodeCoverage]
		public bool? CanBeDeep
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("canbedeep");
			}
			set
			{
				OnPropertyChanging("CanBeDeep");
				((Entity)this).SetAttributeValue("canbedeep", (object)value);
				OnPropertyChanged("CanBeDeep");
			}
		}

		[AttributeLogicalName("canbeentityreference")]
		[ExcludeFromCodeCoverage]
		public bool? CanBeEntityReference
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("canbeentityreference");
			}
			set
			{
				OnPropertyChanging("CanBeEntityReference");
				((Entity)this).SetAttributeValue("canbeentityreference", (object)value);
				OnPropertyChanged("CanBeEntityReference");
			}
		}

		[AttributeLogicalName("canbeglobal")]
		[ExcludeFromCodeCoverage]
		public bool? CanBeGlobal
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("canbeglobal");
			}
			set
			{
				OnPropertyChanging("CanBeGlobal");
				((Entity)this).SetAttributeValue("canbeglobal", (object)value);
				OnPropertyChanged("CanBeGlobal");
			}
		}

		[AttributeLogicalName("canbelocal")]
		[ExcludeFromCodeCoverage]
		public bool? CanBeLocal
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("canbelocal");
			}
			set
			{
				OnPropertyChanging("CanBeLocal");
				((Entity)this).SetAttributeValue("canbelocal", (object)value);
				OnPropertyChanged("CanBeLocal");
			}
		}

		[AttributeLogicalName("canbeparententityreference")]
		[ExcludeFromCodeCoverage]
		public bool? CanBeParentEntityReference
		{
			get
			{
				return ((Entity)this).GetAttributeValue<bool?>("canbeparententityreference");
			}
			set
			{
				OnPropertyChanging("CanBeParentEntityReference");
				((Entity)this).SetAttributeValue("canbeparententityreference", (object)value);
				OnPropertyChanged("CanBeParentEntityReference");
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

		[AttributeLogicalName("privilegeid")]
		[ExcludeFromCodeCoverage]
		public Guid? PrivilegeId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("privilegeid");
			}
			set
			{
				OnPropertyChanging("PrivilegeId");
				((Entity)this).SetAttributeValue("privilegeid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("PrivilegeId");
			}
		}

		[AttributeLogicalName("privilegeid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				PrivilegeId = value;
			}
		}

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Privilege()
			: this("privilege")
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
