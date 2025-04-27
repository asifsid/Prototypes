using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class PricingMethod
	{
		public const int CurrencyAmount = 1;

		public const int PercentListPrice = 2;

		public const int MarkupCurrentCost = 3;

		public const int MarginCurrentCost = 4;

		public const int MarkupStandardCost = 5;

		public const int MarginStandardCost = 6;
	}
}
