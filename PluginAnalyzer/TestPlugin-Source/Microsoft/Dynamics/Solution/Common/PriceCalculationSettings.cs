using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class PriceCalculationSettings
	{
		public readonly OptionSetValue discountCalculationType;

		public readonly int pricingDecimalPrecision;

		public readonly bool oobPriceCalculationEnabled;

		public readonly bool isPriceListMandatory;

		public PriceCalculationSettings(IPluginContext context)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			discountCalculationType = new OptionSetValue(context.GetSettings<int>("DiscountTypeOptionMethod"));
			pricingDecimalPrecision = context.GetSettings<int>("PricingDecimalPrecision");
			oobPriceCalculationEnabled = context.GetSettings<bool>("OOBPriceCalculationEnabled");
			isPriceListMandatory = context.FeatureContext == null || !context.FeatureContext.IsFeatureEnabled("FCB.MakePriceListNonMandatory") || !context.FeatureContext.IsFeatureEnabled("FCB.October2019Update") || context.GetSettings<bool>("IsPriceListMandatory");
			XrmTelemetryContext.AddCustomProperties(new Dictionary<string, string>
			{
				{
					"DiscountTypeOptionMethod",
					((object)discountCalculationType).ToString()
				},
				{
					"PricingDecimalPrecision",
					pricingDecimalPrecision.ToString()
				},
				{
					"OobPriceCalculationEnabled",
					oobPriceCalculationEnabled.ToString()
				}
			});
		}

		public PriceCalculationSettings(int dicountCalcType, int pricingDecimalPrecision, bool oobPriceCalculationEnabled)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			discountCalculationType = new OptionSetValue(dicountCalcType);
			this.pricingDecimalPrecision = pricingDecimalPrecision;
			this.oobPriceCalculationEnabled = oobPriceCalculationEnabled;
			isPriceListMandatory = true;
		}
	}
}
