using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common.Services
{
	[ComVisible(true)]
	public static class PriceServiceUtility
	{
		public static decimal CalculateNumerOfUnitsPerDefaultUnit(IPluginContext context, EntityReference defaultUomScheduleId, EntityReference defaultUomRef, EntityReference uomRef, EntityReference product)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			QueryExpression val = new QueryExpression("uom");
			val.get_ColumnSet().AddColumn("uomid");
			val.get_ColumnSet().AddColumn("baseuom");
			val.get_ColumnSet().AddColumn("quantity");
			val.get_ColumnSet().AddColumn("isschedulebaseuom");
			val.get_Criteria().AddCondition("uomscheduleid", (ConditionOperator)0, new object[1] { defaultUomScheduleId.get_Id() });
			EntityCollection val2 = context.OrganizationService.RetrieveMultiple((QueryBase)(object)val);
			Hashtable hashtable = new Hashtable(((Collection<Entity>)(object)val2.get_Entities()).Count);
			foreach (Entity item in (Collection<Entity>)(object)val2.get_Entities())
			{
				if (item.IsAttributeNull("uomid"))
				{
					throw new CrmException("The UoM Id is invalid.", -2147220970);
				}
				if (item.IsAttributeNull("quantity"))
				{
					throw new CrmException("The quantity is invalid.", -2147220970);
				}
				if (item.IsAttributeNull("isschedulebaseuom"))
				{
					throw new CrmException("The isschedulebaseuom value is invalid.", -2147220970);
				}
				ArrayList arrayList = new ArrayList(3);
				arrayList.Add(item.IsAttributeNull("baseuom") ? Guid.Empty : ((EntityReference)item.get_Item("baseuom")).get_Id());
				arrayList.Add(item.get_Item("quantity"));
				arrayList.Add(item.get_Item("isschedulebaseuom"));
				hashtable.Add(item.get_Item("uomid"), arrayList);
			}
			decimal result = 1m;
			bool flag = false;
			Guid guid = uomRef.get_Id();
			while (!flag)
			{
				if (!hashtable.ContainsKey(guid))
				{
					throw new CrmException(string.Format(Labels.InvalidUnitForProduct, (uomRef != null) ? (uomRef.get_Name() + "_" + uomRef.get_Id().ToString()) : string.Empty, (defaultUomScheduleId != null) ? (defaultUomScheduleId.get_Name() + "_" + defaultUomScheduleId.get_Id().ToString()) : string.Empty, (product != null) ? (product.get_Name() + "_" + product.get_Id().ToString()) : string.Empty), -2147206380);
				}
				ArrayList arrayList = (ArrayList)hashtable[guid];
				flag = (bool)arrayList[2];
				if (!flag)
				{
					result *= (decimal)arrayList[1];
					if (arrayList[0] == null)
					{
						break;
					}
					if ((Guid)arrayList[0] == defaultUomRef.get_Id())
					{
						return result;
					}
					guid = (Guid)arrayList[0];
				}
			}
			flag = false;
			guid = defaultUomRef.get_Id();
			result = 1m;
			while (!flag)
			{
				ArrayList arrayList = (ArrayList)hashtable[guid];
				flag = (bool)arrayList[2];
				if (flag)
				{
					continue;
				}
				result *= (decimal)arrayList[1];
				if (arrayList[0] == null)
				{
					break;
				}
				if ((Guid)arrayList[0] == uomRef.get_Id())
				{
					if (result == 0m)
					{
						throw new CrmException("A UoM quantity is invalid.", -2147220970);
					}
					return 1m / result;
				}
				guid = (Guid)arrayList[0];
			}
			return 1m;
		}

		public static decimal CalculatePricePerUnit(Entity product, Entity productPriceLevel, decimal numUnitsPerDefaultUnit)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Expected O, but got Unknown
			//IL_01c3: Expected O, but got Unknown
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			if (productPriceLevel.IsAttributeNull("pricingmethodcode"))
			{
				throw new CrmPricingException(8);
			}
			int value = ((OptionSetValue)productPriceLevel.get_Item("pricingmethodcode")).get_Value();
			if (value < 1 || value > 6)
			{
				throw new CrmPricingException(9);
			}
			if (numUnitsPerDefaultUnit <= 0m)
			{
				throw new CrmException("Invalid units per default unit.", -2147220970);
			}
			decimal num = default(decimal);
			decimal result = default(decimal);
			if (value == 1)
			{
				if (productPriceLevel.IsAttributeNull("amount"))
				{
					throw new CrmPricingException(12);
				}
				return ((Money)productPriceLevel.get_Item("amount")).get_Value();
			}
			if (productPriceLevel.IsAttributeNull("percentage"))
			{
				throw new CrmPricingException(13);
			}
			num = (decimal)productPriceLevel.get_Item("percentage");
			if (num < 0m || num > 99999.99m || (num > 99.99m && (value == 4 || value == 6)))
			{
				throw new CrmPricingException(18);
			}
			if (productPriceLevel.IsAttributeNull("transactioncurrencyid"))
			{
				throw new CrmPricingException(38);
			}
			if (product.IsAttributeNull("transactioncurrencyid") || !((object)(EntityReference)productPriceLevel.get_Item("transactioncurrencyid")).Equals((object)(EntityReference)product.get_Item("transactioncurrencyid")))
			{
				throw new CrmPricingException(34);
			}
			decimal num2 = default(decimal);
			switch (value)
			{
			case 2:
				if (product.IsAttributeNull("price"))
				{
					throw new CrmPricingException(14);
				}
				num2 = ((Money)product.get_Item("price")).get_Value();
				if (num2 <= 0m)
				{
					throw new CrmPricingException(19);
				}
				return numUnitsPerDefaultUnit * num2 * num / 100m;
			case 3:
			case 4:
				if (product.IsAttributeNull("currentcost"))
				{
					throw new CrmPricingException(15);
				}
				num2 = ((Money)product.get_Item("currentcost")).get_Value();
				if (num2 <= 0m)
				{
					throw new CrmPricingException(20);
				}
				if (value == 4)
				{
					if (num >= 100m)
					{
						throw new CrmPricingException(18);
					}
					return numUnitsPerDefaultUnit * num2 + numUnitsPerDefaultUnit * num2 * num / (100m - num);
				}
				return numUnitsPerDefaultUnit * num2 * (100m + num) / 100m;
			case 5:
			case 6:
				if (product.IsAttributeNull("standardcost"))
				{
					throw new CrmPricingException(16);
				}
				num2 = ((Money)product.get_Item("standardcost")).get_Value();
				if (num2 <= 0m)
				{
					throw new CrmPricingException(21);
				}
				if (value == 6)
				{
					if (num >= 100m)
					{
						throw new CrmPricingException(18);
					}
					return numUnitsPerDefaultUnit * num2 + numUnitsPerDefaultUnit * num2 * num / (100m - num);
				}
				return numUnitsPerDefaultUnit * num2 * (100m + num) / 100m;
			default:
				return result;
			}
		}

		public static decimal RoundPricePerUnit(Entity productPriceLevel, int precision, decimal pricePerUnit, TelemetryMessageBuilder telemetryMessageBuilder)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			string arg = typeof(PriceService).FullName + "RoundPricePerUnit";
			pricePerUnit = Math.Round(pricePerUnit, precision);
			int num = -1;
			int num2 = -1;
			decimal num3 = default(decimal);
			if (!productPriceLevel.IsAttributeNull("roundingpolicycode"))
			{
				num = ((OptionSetValue)productPriceLevel.get_Item("roundingpolicycode")).get_Value();
				if (num < 1 || num > 4)
				{
					throw new CrmPricingException(22);
				}
			}
			if (!productPriceLevel.IsAttributeNull("roundingoptioncode"))
			{
				num2 = ((OptionSetValue)productPriceLevel.get_Item("roundingoptioncode")).get_Value();
				if (num2 < 1 || num2 > 2)
				{
					throw new CrmPricingException(23);
				}
			}
			if (!productPriceLevel.IsAttributeNull("roundingoptionamount"))
			{
				num3 = ((Money)productPriceLevel.get_Item("roundingoptionamount")).get_Value();
				if (num3 < 0m)
				{
					throw new CrmPricingException(24);
				}
			}
			if (pricePerUnit <= num3)
			{
				telemetryMessageBuilder.AppendInformation($"{arg}: PricePerUnit: {pricePerUnit}, RoundingOptionAmount: {num3}");
				return num3;
			}
			if (num2 == 1 && pricePerUnit != num3)
			{
				decimal num4 = 1.0m;
				for (decimal num5 = num3; num5 >= 1.0m; num5 /= 10.0m)
				{
					num4 *= 10m;
				}
				decimal num6 = Math.Truncate(pricePerUnit / num4);
				decimal num7 = pricePerUnit % num4;
				switch (num)
				{
				case 2:
					if (num7 > num3)
					{
						++num6;
					}
					pricePerUnit = num6 * num4 + num3;
					break;
				case 3:
					if (num7 < num3)
					{
						--num6;
					}
					pricePerUnit = num6 * num4 + num3;
					break;
				case 4:
				{
					decimal num8 = ((num7 > num3) ? (num6 + 1m) : num6);
					decimal num9 = ((num7 < num3) ? (num6 - 1m) : num6);
					decimal num10 = num8 * num4 + num3;
					decimal num11 = num9 * num4 + num3;
					pricePerUnit = ((pricePerUnit - num11 < num10 - pricePerUnit) ? num11 : num10);
					break;
				}
				}
			}
			else if (num2 == 2)
			{
				if (num3 == 0m)
				{
					return num3;
				}
				decimal num12 = pricePerUnit % num3;
				switch (num)
				{
				case 2:
					if (num12 == 0m)
					{
						return pricePerUnit;
					}
					pricePerUnit = ((num12 == 0m) ? pricePerUnit : (pricePerUnit + (num3 - num12)));
					break;
				case 3:
					pricePerUnit -= num12;
					break;
				case 4:
				{
					decimal num13 = ((num12 == 0m) ? pricePerUnit : (pricePerUnit + (num3 - num12)));
					decimal num14 = pricePerUnit - num12;
					pricePerUnit = ((pricePerUnit - num14 < num13 - pricePerUnit) ? num14 : num13);
					break;
				}
				}
			}
			return pricePerUnit;
		}
	}
}
