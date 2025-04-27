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
	[EntityLogicalName("transactioncurrency")]
	[GeneratedCode("CrmSvcUtil", "8.1.0.7711")]
	[ComVisible(true)]
	public class Currency : Entity, INotifyPropertyChanging, INotifyPropertyChanged
	{
		public const string EntityLogicalName = "transactioncurrency";

		public const int EntityTypeCode = 9105;

		public const int AttributeCurrencySymbol_MaxLength = 10;

		public const int AttributeISOCurrencyCode_MaxLength = 5;

		public const int AttributeCurrencyName_MaxLength = 100;

		public const int AttributeEntityImage_URL_MaxLength = 200;

		public const string AttributeCreatedBy = "createdby";

		public const string AttributeCreatedOn = "createdon";

		public const string AttributeCreatedOnBehalfBy = "createdonbehalfby";

		public const string AttributeCurrencyName = "currencyname";

		public const string AttributeCurrencyPrecision = "currencyprecision";

		public const string AttributeCurrencySymbol = "currencysymbol";

		public const string AttributeEntityImage = "entityimage";

		public const string AttributeEntityImage_Timestamp = "entityimage_timestamp";

		public const string AttributeEntityImage_URL = "entityimage_url";

		public const string AttributeEntityImageId = "entityimageid";

		public const string AttributeExchangeRate = "exchangerate";

		public const string AttributeImportSequenceNumber = "importsequencenumber";

		public const string AttributeISOCurrencyCode = "isocurrencycode";

		public const string AttributeModifiedBy = "modifiedby";

		public const string AttributeModifiedOn = "modifiedon";

		public const string AttributeModifiedOnBehalfBy = "modifiedonbehalfby";

		public const string AttributeOrganizationId = "organizationid";

		public const string AttributeOverriddenCreatedOn = "overriddencreatedon";

		public const string AttributeStateCode = "statecode";

		public const string AttributeStatusCode = "statuscode";

		public const string AttributeTransactionCurrencyId = "transactioncurrencyid";

		public const string AttributeId = "transactioncurrencyid";

		public const string AttributeVersionNumber = "versionnumber";

		[AttributeLogicalName("createdby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedBy => ((Entity)this).GetAttributeValue<EntityReference>("createdby");

		[AttributeLogicalName("createdon")]
		[ExcludeFromCodeCoverage]
		public DateTime? CreatedOn => ((Entity)this).GetAttributeValue<DateTime?>("createdon");

		[AttributeLogicalName("createdonbehalfby")]
		[ExcludeFromCodeCoverage]
		public EntityReference CreatedOnBehalfBy => ((Entity)this).GetAttributeValue<EntityReference>("createdonbehalfby");

		[AttributeLogicalName("currencyname")]
		[ExcludeFromCodeCoverage]
		public string CurrencyName
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("currencyname");
			}
			set
			{
				OnPropertyChanging("CurrencyName");
				((Entity)this).SetAttributeValue("currencyname", (object)value);
				OnPropertyChanged("CurrencyName");
			}
		}

		[AttributeLogicalName("currencyprecision")]
		[ExcludeFromCodeCoverage]
		public int? CurrencyPrecision
		{
			get
			{
				return ((Entity)this).GetAttributeValue<int?>("currencyprecision");
			}
			set
			{
				OnPropertyChanging("CurrencyPrecision");
				((Entity)this).SetAttributeValue("currencyprecision", (object)value);
				OnPropertyChanged("CurrencyPrecision");
			}
		}

		[AttributeLogicalName("currencysymbol")]
		[ExcludeFromCodeCoverage]
		public string CurrencySymbol
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("currencysymbol");
			}
			set
			{
				OnPropertyChanging("CurrencySymbol");
				((Entity)this).SetAttributeValue("currencysymbol", (object)value);
				OnPropertyChanged("CurrencySymbol");
			}
		}

		[AttributeLogicalName("entityimage")]
		[ExcludeFromCodeCoverage]
		public byte[] EntityImage
		{
			get
			{
				return ((Entity)this).GetAttributeValue<byte[]>("entityimage");
			}
			set
			{
				OnPropertyChanging("EntityImage");
				((Entity)this).SetAttributeValue("entityimage", (object)value);
				OnPropertyChanged("EntityImage");
			}
		}

		[AttributeLogicalName("entityimage_timestamp")]
		[ExcludeFromCodeCoverage]
		public long? EntityImage_Timestamp => ((Entity)this).GetAttributeValue<long?>("entityimage_timestamp");

		[AttributeLogicalName("entityimage_url")]
		[ExcludeFromCodeCoverage]
		public string EntityImage_URL => ((Entity)this).GetAttributeValue<string>("entityimage_url");

		[AttributeLogicalName("entityimageid")]
		[ExcludeFromCodeCoverage]
		public Guid? EntityImageId => ((Entity)this).GetAttributeValue<Guid?>("entityimageid");

		[AttributeLogicalName("exchangerate")]
		[ExcludeFromCodeCoverage]
		public decimal? ExchangeRate
		{
			get
			{
				return ((Entity)this).GetAttributeValue<decimal?>("exchangerate");
			}
			set
			{
				OnPropertyChanging("ExchangeRate");
				((Entity)this).SetAttributeValue("exchangerate", (object)value);
				OnPropertyChanged("ExchangeRate");
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

		[AttributeLogicalName("isocurrencycode")]
		[ExcludeFromCodeCoverage]
		public string ISOCurrencyCode
		{
			get
			{
				return ((Entity)this).GetAttributeValue<string>("isocurrencycode");
			}
			set
			{
				OnPropertyChanging("ISOCurrencyCode");
				((Entity)this).SetAttributeValue("isocurrencycode", (object)value);
				OnPropertyChanged("ISOCurrencyCode");
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

		[AttributeLogicalName("transactioncurrencyid")]
		[ExcludeFromCodeCoverage]
		public Guid? TransactionCurrencyId
		{
			get
			{
				return ((Entity)this).GetAttributeValue<Guid?>("transactioncurrencyid");
			}
			set
			{
				OnPropertyChanging("TransactionCurrencyId");
				((Entity)this).SetAttributeValue("transactioncurrencyid", (object)value);
				if (value.HasValue)
				{
					((Entity)this).set_Id(value.Value);
				}
				else
				{
					((Entity)this).set_Id(Guid.Empty);
				}
				OnPropertyChanged("TransactionCurrencyId");
			}
		}

		[AttributeLogicalName("transactioncurrencyid")]
		[ExcludeFromCodeCoverage]
		public override Guid Id
		{
			get
			{
				return ((Entity)this).get_Id();
			}
			set
			{
				TransactionCurrencyId = value;
			}
		}

		[AttributeLogicalName("versionnumber")]
		[ExcludeFromCodeCoverage]
		public long? VersionNumber => ((Entity)this).GetAttributeValue<long?>("versionnumber");

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public Currency()
			: this("transactioncurrency")
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
