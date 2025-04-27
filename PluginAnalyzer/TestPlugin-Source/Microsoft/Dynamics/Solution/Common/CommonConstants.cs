using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class CommonConstants
	{
		public static class MessageNames
		{
			public const string Update = "Update";

			public const string Create = "Create";

			public const string Delete = "Delete";

			public const string Retrieve = "Retrieve";

			public const string RetrieveMultiple = "RetrieveMultiple";
		}

		public static class EntityAttributes
		{
			public const string StateCode = "statecode";

			public const string StatusCode = "statuscode";

			public const string ModifiedBy = "modifiedby";

			public const string ModifiedOn = "modifiedon";

			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
		}

		public static class PluginExecutionStage
		{
			public const int PreValidation = 10;

			public const int PreOperationInternalChildPipeline = 15;

			public const int PreOperation = 20;

			public const int PreOperationInternal = 25;

			public const int MainOperation = 30;

			public const int PostOperationInternal = 35;

			public const int PostOperation = 40;

			public const int PostOperationInternalChildPipeline = 45;
		}

		public class PricingConstants
		{
			public const string BASE_AMOUNT_DECIMALS = "baseamountDecimals";

			public const string PRICE_PER_UNIT_DECIMALS = "priceperunitDecimals";

			public const string EXTENDED_AMOUNT_DECIMALS = "extendedamountDecimals";

			public const string VOLUME_AMOUNT_DECIMALS = "volumediscountamountDecimals";

			public const string PRICINGDECIMALPRECISION = "pricingDecimalPrecision";

			public const string VOLUMEDISCOUNTAMOUNT = "volumediscountamount";

			public const string PRICEPERUNIT = "priceperunit";

			public const string TRANSACTIONCURRENCYID = "transactioncurrencyid";

			public const string PRODUCT_PRICE_LEVELS = "productPriceLevel";

			public const string PRODUCTS = "products";

			public const string DISCOUNT_TYPES = "discountTypes";

			public const string DISCOUNTS = "discounts";

			public static string[] DISCOUNTATTRIBUTES = new string[6] { "discountid", "lowquantity", "highquantity", "amount", "percentage", "discounttypeid" };
		}

		public class MessageName
		{
			public const string Create = "Create";

			public const string Delete = "Delete";

			public const string Retrieve = "Retrieve";

			public const string Update = "Update";
		}

		public const string InputParameterTarget = "Target";
	}
}
