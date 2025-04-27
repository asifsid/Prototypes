using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class PricingErrorCode
	{
		public const int None = 0;

		public const int DetailError = 1;

		public const int MissingPriceLevel = 2;

		public const int InactivePriceLevel = 3;

		public const int MissingQuantity = 4;

		public const int MissingUnitPrice = 5;

		public const int MissingProduct = 6;

		public const int InvalidProduct = 7;

		public const int MissingPricingCode = 8;

		public const int InvalidPricingCode = 9;

		public const int MissingUom = 10;

		public const int ProductNotInPriceLevel = 11;

		public const int MissingPriceLevelAmount = 12;

		public const int MissingPriceLevelPercentage = 13;

		public const int MissingPrice = 14;

		public const int MissingCurrentCost = 15;

		public const int MissingStandardCost = 16;

		public const int InvalidPriceLevelAmount = 17;

		public const int InvalidPriceLevelPercentage = 18;

		public const int InvalidPrice = 19;

		public const int InvalidCurrentCost = 20;

		public const int InvalidStandardCost = 21;

		public const int InvalidRoundingPolicy = 22;

		public const int InvalidRoundingOption = 23;

		public const int InvalidRoundingAmount = 24;

		public const int PriceCalculationError = 25;

		public const int InvalidDiscountType = 26;

		public const int DiscountTypeInvalidState = 27;

		public const int InvalidDiscount = 28;

		public const int InvalidQuantity = 29;

		public const int InvalidPricingPrecision = 30;

		public const int MissingProductDefaultuom = 31;

		public const int MissingProductUomschedule = 32;

		public const int InactiveDiscountType = 33;

		public const int InvalidPriceLevelCurrency = 34;

		public const int PriceAttributeOutOfRange = 35;

		public const int BaseCurrencyAttributeOverflow = 36;

		public const int BaseCurrencyAttributeUnderflow = 37;

		public const int MissingTransactionCurrency = 38;
	}
}
