using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Dynamics.Solution.Common.Services;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public abstract class PriceService
	{
		private static string ClassName = typeof(PriceService).FullName;

		private readonly IOrganizationService organizationService;

		private readonly IOrganizationService systemUserOrganizationService;

		protected readonly PriceCalculationSettings priceCalculationSettings;

		private PrefetchEntityCache prefetchEntityCache;

		protected IOrganizationService OrganizationService => organizationService;

		protected IOrganizationService SystemUserOrganizationService => systemUserOrganizationService;

		protected PrefetchEntityCache PrefetchEntityCache => prefetchEntityCache;

		protected abstract string EntityName { get; }

		protected abstract string LineItemEntityName { get; }

		protected abstract string LineItemParentFieldName { get; }

		protected abstract string[] LineItemEntityPriceFields { get; }

		public PriceService(IOrganizationService organizationService, IOrganizationService systemUserOrganizationService, PriceCalculationSettings priceCalculationSettings)
		{
			this.organizationService = organizationService;
			this.systemUserOrganizationService = systemUserOrganizationService;
			this.priceCalculationSettings = priceCalculationSettings;
			prefetchEntityCache = new PrefetchEntityCache();
		}

		protected abstract void SummationLineItemPrices(Entity newLineItem, Entity oldLineItem, bool isErrorInlineItemPrice);

		protected abstract Entity CalculateLineItemTotals(Entity entity, decimal pricePerUnit, decimal volumeDiscount);

		protected abstract Entity CalculateTotals(Entity entity);

		protected abstract void UpdateEntity(Entity newEntity, Entity oldEntity);

		protected abstract void UpdateLineItem(Entity newEntity, Entity oldEntity);

		protected abstract bool IsPricePerUnitCalculationAllowed(bool overridePricePerUnitLock, Entity lineItem, Entity entity);

		protected abstract bool IsVolumeDiscountCalculationAllowed(bool overrideDiscountLock, Entity lineItem, Entity entity);

		protected virtual void CalculatePrice(Entity entity, Guid lineItemId, bool skipQOIDetailPricing, bool overridePricePerUnitLock, bool overrideDiscountLock, IPluginContext context)
		{
			context.PluginTelemetry.OperationEventLogger.Execute((XrmTelemetryActivityType)(object)XrmTelemetrySingletonActivityType<PriceCalculationActivityType>.get_Instance(), delegate
			{
				Dictionary<string, string> obj = new Dictionary<string, string> { 
				{
					"Message",
					((IExecutionContext)context.PluginExecutionContext).get_MessageName()
				} };
				Entity obj2 = entity;
				obj.Add("EntityId", (obj2 != null) ? obj2.get_Id().ToString() : null);
				obj.Add("LineItemId", lineItemId.ToString());
				Entity obj3 = entity;
				obj.Add("EntityLogicalName", (obj3 != null) ? obj3.get_LogicalName() : null);
				obj.Add("SkipPriceCalculation", skipQOIDetailPricing.ToString());
				obj.Add("OverrideDiscountLock", overrideDiscountLock.ToString());
				XrmTelemetryContext.AddCustomProperties(obj);
				CalculatePriceInternal(entity, lineItemId, skipQOIDetailPricing, overridePricePerUnitLock, overrideDiscountLock, context);
			});
		}

		private void CalculatePriceInternal(Entity entity, Guid lineItemId, bool skipQOIDetailPricing, bool overridePricePerUnitLock, bool overrideDiscountLock, IPluginContext context)
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_0423: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Expected O, but got Unknown
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Expected O, but got Unknown
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Expected O, but got Unknown
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0514: Expected O, but got Unknown
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0581: Unknown result type (might be due to invalid IL or missing references)
			//IL_0588: Expected O, but got Unknown
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Expected O, but got Unknown
			//IL_05b6: Expected O, but got Unknown
			//IL_066f: Unknown result type (might be due to invalid IL or missing references)
			//IL_067d: Expected O, but got Unknown
			//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_095b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0965: Expected O, but got Unknown
			//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be7: Expected O, but got Unknown
			try
			{
				if (priceCalculationSettings.isPriceListMandatory && !ValidatePriceLevelId(entity, context))
				{
					context.PluginTelemetry.Logger.LogEvent(EventType.Warning, ClassName, "CalculatePrice.ValidatePriceLevelId", "Price list is missing. Exiting price calculation.");
					return;
				}
				EntityReference val = (entity.IsAttributeNull("pricelevelid") ? ((EntityReference)null) : ((EntityReference)entity.get_Item("pricelevelid")));
				int pricingDecimalPrecision = priceCalculationSettings.pricingDecimalPrecision;
				if (pricingDecimalPrecision < 0 || pricingDecimalPrecision > 4)
				{
					throw new CrmPricingException(30);
				}
				prefetchEntityCache.Set("pricingDecimalPrecision", pricingDecimalPrecision);
				QueryExpression val2 = new QueryExpression(LineItemEntityName);
				val2.get_ColumnSet().AddColumns(LineItemEntityPriceFields);
				val2.get_ColumnSet().AddColumn("quantity");
				val2.get_ColumnSet().AddColumn("priceperunit");
				val2.get_ColumnSet().AddColumn("volumediscountamount");
				val2.get_ColumnSet().AddColumn("productdescription");
				val2.get_ColumnSet().AddColumn("productid");
				val2.get_ColumnSet().AddColumn("uomid");
				val2.get_ColumnSet().AddColumn("transactioncurrencyid");
				val2.get_ColumnSet().AddColumn("producttypecode");
				val2.get_ColumnSet().AddColumn("parentbundleid");
				val2.get_Criteria().AddCondition(LineItemParentFieldName, (ConditionOperator)0, new object[1] { entity.get_Id() });
				List<Entity> list = null;
				using (context.SetSkipRetrieveMultiplePlugins(LineItemEntityName))
				{
					list = SystemUserOrganizationService.RetrieveMultiplePaged<Entity>(val2).ToList();
				}
				bool flag = false;
				int num = 0;
				int num2 = 0;
				bool lineItemIdSpecified = lineItemId != Guid.Empty;
				LoadPrefetchCache(context, list, skipQOIDetailPricing, lineItemIdSpecified);
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CalculatePrice", $"Processing lineitems- Count: {list.Count}", new Dictionary<string, string> { 
				{
					"TotalProducts",
					list.Count.ToString()
				} });
				TelemetryMessageBuilder telemetryMessageBuilder = new TelemetryMessageBuilder(context.PluginTelemetry.Logger);
				string text = ClassName + ": CalculatePrice: ";
				foreach (Entity item in list)
				{
					decimal pricePerUnit = default(decimal);
					decimal volumeDiscount = default(decimal);
					decimal numUnitsPerDefaultUnit = 1m;
					bool flag2 = false;
					Entity val3 = item;
					bool flag3 = false;
					int num3 = 0;
					OptionSetValue val4 = null;
					if (!item.IsAttributeNull("producttypecode"))
					{
						object obj = item.get_Item("producttypecode");
						val4 = obj as OptionSetValue;
					}
					if (val4 != null && val4.get_Value() == 3)
					{
						num2++;
						continue;
					}
					if (val4 != null && val4.get_Value() == 4)
					{
						Guid guid = (item.IsAttributeNull("parentbundleid") ? Guid.Empty : ((Guid)item.get_Item("parentbundleid")));
						if (guid != Guid.Empty && guid == lineItemId)
						{
							num++;
							flag2 = true;
						}
					}
					try
					{
						if (!skipQOIDetailPricing || (skipQOIDetailPricing && lineItemId == item.get_Id()) || flag2)
						{
							if (item.IsAttributeNull("quantity"))
							{
								throw new CrmPricingException(4);
							}
							if (!item.IsAttributeNull("priceperunit"))
							{
								Money val5 = (Money)item.get_Item("priceperunit");
								pricePerUnit = val5.get_Value();
							}
							if (!item.IsAttributeNull("volumediscountamount"))
							{
								Money val6 = (Money)item.get_Item("volumediscountamount");
								volumeDiscount = val6.get_Value();
							}
							bool flag4 = IsPricePerUnitCalculationAllowed(overridePricePerUnitLock, item, entity);
							bool flag5 = IsVolumeDiscountCalculationAllowed(overrideDiscountLock, item, entity);
							if (flag4 || flag5)
							{
								if (item.IsAttributeNull("productid"))
								{
									throw new CrmPricingException(6);
								}
								EntityReference val7 = (EntityReference)item.get_Item("productid");
								Entity product = GetProduct(val7);
								if (product == null)
								{
									throw new CrmPricingException(7);
								}
								if (product.IsAttributeNull("defaultuomid"))
								{
									throw new CrmPricingException(31);
								}
								EntityReference val8 = (EntityReference)product.get_Item("defaultuomid");
								if (!item.IsAttributeNull("uomid") && ((EntityReference)item.get_Item("uomid")).get_Id() != val8.get_Id())
								{
									if (product.IsAttributeNull("defaultuomscheduleid"))
									{
										throw new CrmPricingException(32);
									}
									val8 = (EntityReference)item.get_Item("uomid");
									numUnitsPerDefaultUnit = CalculateNumerOfUnitsPerDefaultUnit(context, (EntityReference)product.get_Item("defaultuomscheduleid"), (EntityReference)product.get_Item("defaultuomid"), val8, val7);
								}
								Entity val9 = ((val != null) ? GetProductPricelevel(val7, val, val8, null) : null);
								if (val9 == null)
								{
									if (product.IsAttributeNull("pricelevelid"))
									{
										if (priceCalculationSettings.isPriceListMandatory)
										{
											throw new CrmPricingException(11);
										}
									}
									else
									{
										EntityReference transactionCurrency = null;
										if (!priceCalculationSettings.isPriceListMandatory && !entity.IsAttributeNull("transactioncurrencyid"))
										{
											object obj2 = entity.get_Item("transactioncurrencyid");
											transactionCurrency = obj2 as EntityReference;
										}
										val9 = GetProductPricelevel(val7, (EntityReference)product.get_Item("pricelevelid"), val8, transactionCurrency);
									}
									if (val9?.IsAttributeNull("transactioncurrencyid") ?? false)
									{
										throw new CrmPricingException(38);
									}
									if ((val9 == null || !val9.get_Item("transactioncurrencyid").Equals(item.get_Item("transactioncurrencyid"))) && priceCalculationSettings.isPriceListMandatory)
									{
										throw new CrmPricingException(11);
									}
									Guid guid2 = ((val9 != null) ? val9.get_Id() : Guid.Empty);
									telemetryMessageBuilder.AppendInformation($"{text}: ProductPriceLevel is null. Tried retrieving price list item for default price list of the product. ProductPriceLevelId: {guid2}");
								}
								if (flag4 && (priceCalculationSettings.isPriceListMandatory || val9 != null))
								{
									int precision = prefetchEntityCache.Get<int>("priceperunitDecimals");
									pricePerUnit = RoundPricePerUnit(val9, precision, CalculatePricePerUnit(product, val9, numUnitsPerDefaultUnit), telemetryMessageBuilder);
								}
								if (flag5)
								{
									if ((val9 != null || priceCalculationSettings.isPriceListMandatory) && !val9.IsAttributeNull("discounttypeid"))
									{
										Entity discountType = GetDiscountType(((EntityReference)val9.get_Item("discounttypeid")).get_Id());
										if (discountType?.IsAttributeNull("isamounttype") ?? true)
										{
											throw new CrmPricingException(26);
										}
										if (discountType.IsAttributeNull("statecode"))
										{
											throw new CrmPricingException(27);
										}
										object obj3 = discountType.get_Item("statecode");
										OptionSetValue val10 = obj3 as OptionSetValue;
										if (val10.get_Value() != 0)
										{
											num3 = 33;
											flag = true;
											flag3 = true;
											telemetryMessageBuilder.AppendWarning(text + ": DiscountType is inactive. Price calculation will not be done for this line item");
										}
										int precision2 = prefetchEntityCache.Get<int>("volumediscountamountDecimals");
										IEnumerable<Entity> discountEntities = GetDiscountEntities(((EntityReference)val9.get_Item("discounttypeid")).get_Id());
										if (discountEntities?.Any() ?? false)
										{
											volumeDiscount = CalculateVolumeDiscount((decimal)item.get_Item("quantity"), discountEntities, precision2, pricePerUnit, (bool)discountType.get_Item("isamounttype"));
											telemetryMessageBuilder.AppendInformation($"{text}: Discounts=s count for pricelevel of line item: {discountEntities.Count()}");
										}
										else
										{
											volumeDiscount = default(decimal);
										}
									}
									else
									{
										volumeDiscount = default(decimal);
									}
								}
							}
							val3 = CalculateLineItemTotals(item, pricePerUnit, volumeDiscount);
							val3.set_Item("pricingerrorcode", (object)new OptionSetValue(num3));
							using (context.SetSkipUpdatePlugins(item.get_LogicalName()))
							{
								UpdateLineItem(val3, item);
							}
						}
					}
					catch (CrmPricingException ex)
					{
						telemetryMessageBuilder.AppendError(text + ": Error occurred while processing LineItem, CrmPricingException: " + ex.Message + ", " + ex.InnerException?.Message);
						flag = true;
						flag3 = true;
						SetError(item, item, ex.PricingError, context);
					}
					catch (FaultException<OrganizationServiceFault> ex2)
					{
						telemetryMessageBuilder.AppendError(text + ": Error occurred while processing LineItem, FaultException: " + ex2.ToString());
						context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CalculatePrice", telemetryMessageBuilder.Flush(), new Dictionary<string, string>
						{
							{
								"RequiredProducts",
								num2.ToString()
							},
							{
								"OptionalProducts",
								num.ToString()
							}
						});
						switch (((BaseServiceFault)ex2.Detail).get_ErrorCode())
						{
						case -2147204304:
							flag = true;
							HandleCriticalError(item, item, 35, context);
							throw;
						case -2147185428:
							flag = true;
							HandleCriticalError(item, item, 36, context);
							throw;
						case -2147185429:
							flag = true;
							HandleCriticalError(item, item, 37, context);
							throw;
						default:
							throw;
						}
					}
					if (flag3)
					{
						telemetryMessageBuilder.AppendWarning(text + ": Calling SummationLineItemPrices with IsErrorInCurrentLine: True");
					}
					SummationLineItemPrices(val3, item, flag3);
				}
				telemetryMessageBuilder.AppendInformation(text + ": Price calculation for line items completed. Updating totals.");
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CalculatePrice", telemetryMessageBuilder.Flush(), new Dictionary<string, string>
				{
					{
						"RequiredProducts",
						num2.ToString()
					},
					{
						"OptionalProducts",
						num.ToString()
					}
				});
				Entity val11 = CalculateTotals(entity);
				val11.set_Item("pricingerrorcode", (object)(flag ? new OptionSetValue(1) : new OptionSetValue(0)));
				using (context.SetSkipUpdatePlugins(entity.get_LogicalName()))
				{
					UpdateEntity(val11, entity);
					context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CalculatePrice", $"Updated Entity: {entity.get_Id()} with SetSkipUpdatePlugins set for {entity.get_LogicalName()}");
				}
			}
			catch (CrmPricingException ex3)
			{
				SetError(entity, entity, ex3.PricingError, context);
			}
			catch (FaultException<OrganizationServiceFault> ex4)
			{
				switch (((BaseServiceFault)ex4.Detail).get_ErrorCode())
				{
				case -2147204304:
					HandleCriticalError(entity, entity, 35, context);
					break;
				case -2147185428:
					HandleCriticalError(entity, entity, 36, context);
					break;
				case -2147185429:
					HandleCriticalError(entity, entity, 37, context);
					break;
				default:
					throw;
				}
			}
		}

		private void LoadPrefetchCache(IPluginContext context, IEnumerable<Entity> lineItems, bool skipQOIDetailPricing, bool lineItemIdSpecified)
		{
			SetPrecisionValues(context, lineItems);
			string messageName = ((IExecutionContext)context.PluginExecutionContext).get_MessageName();
			if (!messageName.Equals("Create") && !(skipQOIDetailPricing && lineItemIdSpecified))
			{
				PrefetchProductCache(lineItems, context);
			}
		}

		private IEnumerable<Entity> GetDiscountEntities(Guid discountTypeId)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			EntityCollection value = null;
			if (prefetchEntityCache.TryGetValue("discounts", ref value))
			{
				return (value != null) ? ((IEnumerable<Entity>)value.get_Entities()).Where((Entity p) => ((EntityReference)p.get_Item("discounttypeid")).get_Id() == discountTypeId) : null;
			}
			QueryExpression val = new QueryExpression("discount");
			val.get_ColumnSet().AddColumn("discountid");
			val.get_ColumnSet().AddColumn("lowquantity");
			val.get_ColumnSet().AddColumn("highquantity");
			val.get_ColumnSet().AddColumn("amount");
			val.get_ColumnSet().AddColumn("percentage");
			val.get_Criteria().AddCondition("discounttypeid", (ConditionOperator)0, new object[1] { discountTypeId });
			return (IEnumerable<Entity>)OrganizationService.RetrieveMultiplePaged(val).get_Entities();
		}

		private Entity GetDiscountType(Guid discountTypeId)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			EntityCollection value = null;
			if (prefetchEntityCache.TryGetValue("discountTypes", ref value))
			{
				return (value != null) ? ((IEnumerable<Entity>)value.get_Entities()).FirstOrDefault((Entity p) => p.get_Id() == discountTypeId) : null;
			}
			ColumnSet val = new ColumnSet(new string[2] { "statecode", "isamounttype" });
			return OrganizationService.Retrieve("discounttype", discountTypeId, val);
		}

		private Entity GetProductPricelevel(EntityReference productRef, EntityReference priceLevelIdRef, EntityReference uomRef, EntityReference transactionCurrency)
		{
			EntityCollection value = null;
			if (prefetchEntityCache.TryGetValue("productPriceLevel", ref value))
			{
				return (value != null) ? ((IEnumerable<Entity>)value.get_Entities()).FirstOrDefault((Entity p) => ((EntityReference)p.get_Item("productid")).get_Id() == productRef.get_Id() && ((EntityReference)p.get_Item("uomid")).get_Id() == uomRef.get_Id() && ((EntityReference)p.get_Item("pricelevelid")).get_Id() == priceLevelIdRef.get_Id() && (priceCalculationSettings.isPriceListMandatory || transactionCurrency == null || ((EntityReference)p.get_Item("transactioncurrencyid")).get_Id() == transactionCurrency.get_Id())) : null;
			}
			return RetrieveProductPriceLevel(productRef, priceLevelIdRef, uomRef, transactionCurrency);
		}

		private Entity GetProduct(EntityReference productRef)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			EntityCollection value = null;
			if (prefetchEntityCache.TryGetValue("products", ref value))
			{
				return (value != null) ? ((IEnumerable<Entity>)value.get_Entities()).FirstOrDefault((Entity p) => p.get_Id() == productRef.get_Id()) : null;
			}
			ColumnSet val = new ColumnSet(new string[7] { "defaultuomscheduleid", "defaultuomid", "pricelevelid", "price", "currentcost", "standardcost", "transactioncurrencyid" });
			try
			{
				return OrganizationService.Retrieve(productRef.get_LogicalName(), productRef.get_Id(), val);
			}
			catch (FaultException<OrganizationServiceFault> ex)
			{
				if (((BaseServiceFault)ex.Detail).get_ErrorCode() == -2147220969)
				{
					throw new CrmPricingException(7);
				}
				throw;
			}
		}

		private void PrefetchProductCache(IEnumerable<Entity> lineItems, IPluginContext context)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Expected O, but got Unknown
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Expected O, but got Unknown
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0368: Expected O, but got Unknown
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ba: Expected O, but got Unknown
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Expected O, but got Unknown
			EntityCollection val = new EntityCollection();
			HashSet<object> hashSet = new HashSet<object>();
			HashSet<object> hashSet2 = new HashSet<object>();
			foreach (Entity lineItem in lineItems)
			{
				if (!lineItem.IsAttributeNull("productid"))
				{
					EntityReference val2 = (EntityReference)lineItem.get_Item("productid");
					hashSet.Add(val2.get_Id());
				}
			}
			if (hashSet.Count <= 0)
			{
				return;
			}
			QueryExpression val3 = new QueryExpression("product");
			val3.get_ColumnSet().AddColumns(new string[7] { "defaultuomscheduleid", "defaultuomid", "pricelevelid", "price", "currentcost", "standardcost", "transactioncurrencyid" });
			val3.get_Criteria().AddCondition("productid", (ConditionOperator)8, hashSet.ToArray());
			using (context.SetSkipRetrieveMultiplePlugins(LineItemEntityName))
			{
				val = context.OrganizationService.RetrieveMultiplePaged(val3);
			}
			prefetchEntityCache.Set("products", val);
			QueryExpression val4 = new QueryExpression("productpricelevel");
			val4.get_ColumnSet().AddColumn("productpricelevelid");
			val4.get_ColumnSet().AddColumn("pricingmethodcode");
			val4.get_ColumnSet().AddColumn("amount");
			val4.get_ColumnSet().AddColumn("percentage");
			val4.get_ColumnSet().AddColumn("roundingpolicycode");
			val4.get_ColumnSet().AddColumn("roundingoptioncode");
			val4.get_ColumnSet().AddColumn("roundingoptionamount");
			val4.get_ColumnSet().AddColumn("discounttypeid");
			val4.get_ColumnSet().AddColumn("transactioncurrencyid");
			val4.get_ColumnSet().AddColumn("uomid");
			val4.get_ColumnSet().AddColumn("productid");
			val4.get_ColumnSet().AddColumn("pricelevelid");
			val4.get_Criteria().AddCondition("productid", (ConditionOperator)8, hashSet.ToArray());
			EntityCollection val5 = OrganizationService.RetrieveMultiplePaged(val4);
			prefetchEntityCache.Set("productPriceLevel", val5);
			context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "PrefetchProductCache", $"Retrieved products, productpricelevels: {((val == null) ? null : ((Collection<Entity>)(object)val.get_Entities())?.Count)}, {((val5 == null) ? null : ((Collection<Entity>)(object)val5.get_Entities())?.Count)}");
			foreach (Entity item in (Collection<Entity>)(object)val5.get_Entities())
			{
				if (!item.IsAttributeNull("discounttypeid"))
				{
					EntityReference val6 = (EntityReference)item.get_Item("discounttypeid");
					hashSet2.Add(val6.get_Id());
				}
			}
			if (hashSet2.Count > 0)
			{
				QueryExpression val7 = new QueryExpression("discounttype");
				val7.get_ColumnSet().AddColumns(new string[3] { "statecode", "isamounttype", "discounttypeid" });
				val7.get_Criteria().AddCondition("discounttypeid", (ConditionOperator)8, hashSet2.ToArray());
				EntityCollection val8 = OrganizationService.RetrieveMultiplePaged(val7);
				prefetchEntityCache.Set("discountTypes", val8);
				QueryExpression val9 = new QueryExpression("discount");
				val9.get_ColumnSet().AddColumns(new string[6] { "discountid", "lowquantity", "highquantity", "amount", "percentage", "discounttypeid" });
				val9.get_Criteria().AddCondition("discounttypeid", (ConditionOperator)8, hashSet2.ToArray());
				EntityCollection val10 = OrganizationService.RetrieveMultiplePaged(val9);
				prefetchEntityCache.Set("discounts", val10);
				context.PluginTelemetry.Logger.LogEvent(EventType.Warning, ClassName, "PrefetchProductCache", $"Retrieved discounttype, discounts: {((val8 == null) ? null : ((Collection<Entity>)(object)val8.get_Entities())?.Count)}, {((val10 == null) ? null : ((Collection<Entity>)(object)val10.get_Entities())?.Count)}");
			}
		}

		private void SetPrecisionValues(IPluginContext context, IEnumerable<Entity> lineItems)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Expected O, but got Unknown
			EntityMetadata val = context.EntityMetadataCache.Get(LineItemEntityName);
			prefetchEntityCache.Set("baseamountDecimals", ((MoneyAttributeMetadata)val.get_Attributes().FirstOrDefault((AttributeMetadata a) => a.get_LogicalName() == "baseamount")).get_Precision().Value);
			prefetchEntityCache.Set("extendedamountDecimals", ((MoneyAttributeMetadata)val.get_Attributes().FirstOrDefault((AttributeMetadata a) => a.get_LogicalName() == "extendedamount")).get_Precision().Value);
			Entity val2 = lineItems.FirstOrDefault();
			if (val2 != null && !val2.IsAttributeNull("transactioncurrencyid"))
			{
				EntityReference val3 = (EntityReference)val2.get_Item("transactioncurrencyid");
				MoneyAttributeMetadata attributeMetadata = (MoneyAttributeMetadata)val.get_Attributes().FirstOrDefault((AttributeMetadata a) => a.get_LogicalName() == "volumediscountamount");
				int currencyPrecisionForAttribute = GetCurrencyPrecisionForAttribute(attributeMetadata, val3.get_Id(), context);
				prefetchEntityCache.Set("volumediscountamountDecimals", currencyPrecisionForAttribute);
				MoneyAttributeMetadata attributeMetadata2 = (MoneyAttributeMetadata)val.get_Attributes().FirstOrDefault((AttributeMetadata a) => a.get_LogicalName() == "priceperunit");
				int currencyPrecisionForAttribute2 = GetCurrencyPrecisionForAttribute(attributeMetadata2, val3.get_Id(), context);
				prefetchEntityCache.Set("priceperunitDecimals", currencyPrecisionForAttribute2);
			}
		}

		private RetrieveAttributeRequest getRetrieveAttributeRequest(string lineItemEntityName, string logicalName)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			RetrieveAttributeRequest val = new RetrieveAttributeRequest();
			val.set_EntityLogicalName(LineItemEntityName);
			val.set_LogicalName(logicalName);
			return val;
		}

		private Currency GetCurrencyDefinitionForAttribute(MoneyAttributeMetadata attributeMetadata, Guid transactionCurrencyId, IPluginContext context)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			_ = Guid.Empty;
			Guid entityId;
			if (attributeMetadata.get_IsBaseCurrency().HasValue && attributeMetadata.get_IsBaseCurrency().Value)
			{
				Entity val = context.OrganizationService.Retrieve("organization", ((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), new ColumnSet(new string[1] { "basecurrencyid" }));
				EntityReference val2 = ((val != null && !val.IsAttributeNull("basecurrencyid")) ? val.GetAttributeValue<EntityReference>("basecurrencyid") : null);
				entityId = val2.get_Id();
			}
			else
			{
				CrmException.Assert(transactionCurrencyId != Guid.Empty, "transactionCurrencyId needs to be supplied to format a transaction money field.");
				entityId = transactionCurrencyId;
			}
			return context.OrganizationService.Retrieve<Currency>(entityId, new ColumnSet(new string[1] { "currencyprecision" }));
		}

		public int GetCurrencyPrecisionForAttribute(MoneyAttributeMetadata attributeMetadata, Guid transactionCurrencyId, IPluginContext context)
		{
			if (attributeMetadata.get_PrecisionSource().Value == 2)
			{
				Currency currencyDefinitionForAttribute = GetCurrencyDefinitionForAttribute(attributeMetadata, transactionCurrencyId, context);
				return currencyDefinitionForAttribute.CurrencyPrecision.Value;
			}
			return attributeMetadata.get_Precision().Value;
		}

		protected virtual bool ValidatePriceLevelId(Entity entity, IPluginContext context)
		{
			if (entity.IsAttributeNull("pricelevelid"))
			{
				SetError(entity, entity, 2, context);
				return false;
			}
			return true;
		}

		private void HandleCriticalError(Entity entity, Entity entityMoniker, int pricingErrorCode, IPluginContext context)
		{
			try
			{
				SetError(entity, entityMoniker, pricingErrorCode, context);
			}
			catch (Exception)
			{
				throw;
			}
		}

		protected void SetError(Entity entityPreImage, Entity entityMoniker, int newPricingErrorCode, IPluginContext context)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			OptionSetValue attributeValue = entityPreImage.GetAttributeValue<OptionSetValue>("pricingerrorcode");
			XrmTelemetryContext.AddCustomProperty("CrmPricingException", newPricingErrorCode.ToString());
			if (attributeValue == null || attributeValue.get_Value() != newPricingErrorCode)
			{
				context.Trace(string.Format(Labels.UpdatePricingErrorCodeMessage, newPricingErrorCode));
				Entity val = new Entity(entityMoniker.get_LogicalName(), entityMoniker.get_Id());
				val.SetAttributeValue("pricingerrorcode", (object)new OptionSetValue(newPricingErrorCode));
				using (context.SetSkipUpdatePlugins(val.get_LogicalName()))
				{
					SystemUserOrganizationService.Update(val);
				}
			}
		}

		private decimal CalculateNumerOfUnitsPerDefaultUnit(IPluginContext context, EntityReference defaultUomScheduleId, EntityReference defaultUomRef, EntityReference uomRef, EntityReference product)
		{
			return PriceServiceUtility.CalculateNumerOfUnitsPerDefaultUnit(context, defaultUomScheduleId, defaultUomRef, uomRef, product);
		}

		private Entity RetrieveProductPriceLevel(EntityReference productRef, EntityReference priceLevelRef, EntityReference uomRef, EntityReference transactionCurrency)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			QueryExpression val = new QueryExpression("productpricelevel");
			val.get_ColumnSet().AddColumns(Array.Empty<string>());
			val.get_ColumnSet().AddColumn("productpricelevelid");
			val.get_ColumnSet().AddColumn("pricingmethodcode");
			val.get_ColumnSet().AddColumn("amount");
			val.get_ColumnSet().AddColumn("percentage");
			val.get_ColumnSet().AddColumn("roundingpolicycode");
			val.get_ColumnSet().AddColumn("roundingoptioncode");
			val.get_ColumnSet().AddColumn("roundingoptionamount");
			val.get_ColumnSet().AddColumn("discounttypeid");
			val.get_ColumnSet().AddColumn("transactioncurrencyid");
			val.get_Criteria().AddCondition("productid", (ConditionOperator)0, new object[1] { productRef.get_Id() });
			val.get_Criteria().AddCondition("uomid", (ConditionOperator)0, new object[1] { uomRef.get_Id() });
			val.get_Criteria().AddCondition("pricelevelid", (ConditionOperator)0, new object[1] { priceLevelRef.get_Id() });
			int num;
			if (!priceCalculationSettings.isPriceListMandatory && transactionCurrency != null)
			{
				transactionCurrency.get_Id();
				num = 1;
			}
			else
			{
				num = 0;
			}
			if (num != 0)
			{
				val.get_Criteria().AddCondition("transactioncurrencyid", (ConditionOperator)0, new object[1] { transactionCurrency.get_Id() });
			}
			EntityCollection val2 = OrganizationService.RetrieveMultiple((QueryBase)(object)val);
			if (((Collection<Entity>)(object)val2.get_Entities()).Count == 1)
			{
				return val2.get_Item(0);
			}
			return null;
		}

		private decimal CalculatePricePerUnit(Entity product, Entity productPriceLevel, decimal numUnitsPerDefaultUnit)
		{
			return PriceServiceUtility.CalculatePricePerUnit(product, productPriceLevel, numUnitsPerDefaultUnit);
		}

		private decimal RoundPricePerUnit(Entity productPriceLevel, int precision, decimal pricePerUnit, TelemetryMessageBuilder telemetryMessageBuilder)
		{
			return PriceServiceUtility.RoundPricePerUnit(productPriceLevel, precision, pricePerUnit, telemetryMessageBuilder);
		}

		private decimal CalculateVolumeDiscount(decimal quantity, IEnumerable<Entity> discountEntities, int precision, decimal pricePerUnit, bool isAmountType)
		{
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			decimal result = default(decimal);
			foreach (Entity discountEntity in discountEntities)
			{
				if (discountEntity.IsAttributeNull("lowquantity"))
				{
					continue;
				}
				decimal num = (decimal)discountEntity.get_Item("lowquantity");
				if (discountEntity.IsAttributeNull("highquantity"))
				{
					continue;
				}
				decimal num2 = (decimal)discountEntity.get_Item("highquantity");
				if (!(quantity >= num) || !(quantity <= num2))
				{
					continue;
				}
				if (isAmountType)
				{
					if (discountEntity.IsAttributeNull("amount"))
					{
						throw new CrmPricingException(28);
					}
					return Math.Round(((Money)discountEntity.get_Item("amount")).get_Value(), precision);
				}
				if (discountEntity.IsAttributeNull("percentage"))
				{
					throw new CrmPricingException(28);
				}
				return Math.Round(pricePerUnit * (decimal)discountEntity.get_Item("percentage") / 100m, precision);
			}
			return result;
		}
	}
}
