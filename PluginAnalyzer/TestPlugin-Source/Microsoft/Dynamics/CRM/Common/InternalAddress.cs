using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.CRM.Common
{
	[ComVisible(true)]
	public static class InternalAddress
	{
		private static string ClassName = typeof(InternalAddress).FullName;

		public const string InvoiceEntityName = "invoice";

		public const string OrderEntityName = "salesorder";

		public const string QuoteEntityName = "quote";

		public const string AccountEntityName = "account";

		public const string ContractEntityName = "contract";

		internal static string PreBusinessEntity => "PreBusinessEntity";

		internal static string AttributeBillTo_Composite => "billto_composite";

		internal static string AttributeShipTo_Composite => "shipto_composite";

		internal static string AttributeAddress1_Composite => "address1_composite";

		internal static string AttributeAddress2_Composite => "address2_composite";

		internal static string AttributeAddress3_Composite => "address3_composite";

		internal static string[] BillToAttributeList => new string[7] { "billto_line1", "billto_line2", "billto_line3", "billto_city", "billto_stateorprovince", "billto_postalcode", "billto_country" };

		internal static string[] Address1AttributeList => new string[7] { "address1_line1", "address1_line2", "address1_line3", "address1_city", "address1_stateorprovince", "address1_postalcode", "address1_country" };

		internal static string[] Address2AttributeList => new string[7] { "address2_line1", "address2_line2", "address2_line3", "address2_city", "address2_stateorprovince", "address2_postalcode", "address2_country" };

		internal static string[] Address3AttributeList => new string[7] { "address3_line1", "address3_line2", "address3_line3", "address3_city", "address3_stateorprovince", "address3_postalcode", "address3_country" };

		internal static string[] ShipToAttributeList => new string[7] { "shipto_line1", "shipto_line2", "shipto_line3", "shipto_city", "shipto_stateorprovince", "shipto_postalcode", "shipto_country" };

		internal static bool IsQOIEntity(string entityName)
		{
			if (entityName.Equals("invoice") || entityName.Equals("quote") || entityName.Equals("salesorder"))
			{
				return true;
			}
			return false;
		}

		private static bool AreConstituentAddressFieldsEmpty(string[] attributes, Entity target)
		{
			foreach (string text in attributes)
			{
				if (target.Contains(text) && target.get_Item(text) != null)
				{
					return false;
				}
			}
			return true;
		}

		private static bool DoesPreImageContainData(IPluginContext context)
		{
			if (((DataCollection<string, Entity>)(object)((IExecutionContext)context.PluginExecutionContext).get_PreEntityImages()).Contains(PreBusinessEntity) && ((DataCollection<string, Entity>)(object)((IExecutionContext)context.PluginExecutionContext).get_PreEntityImages()).get_Item(PreBusinessEntity) != null)
			{
				return true;
			}
			return false;
		}

		internal static bool IsAddressComputationNeededForUpdate(IPluginContext context)
		{
			if (DoesPreImageContainData(context))
			{
				return true;
			}
			return false;
		}

		internal static bool IsAddressComputationNeededForRetrieve(Entity qoiEntity)
		{
			string attribute;
			string attribute2;
			if (IsQOIEntity(qoiEntity.get_LogicalName()))
			{
				attribute = AttributeBillTo_Composite;
				attribute2 = AttributeShipTo_Composite;
			}
			else
			{
				attribute = AttributeAddress1_Composite;
				attribute2 = AttributeAddress2_Composite;
			}
			string value = AddressAttributeValue(attribute, qoiEntity);
			string value2 = AddressAttributeValue(attribute2, qoiEntity);
			if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value2))
			{
				return true;
			}
			return false;
		}

		private static void ComputeCompositeAddressValue(string addressName, Entity source, Entity target, IPluginContext context)
		{
			string text = ComputeStreetValue(addressName + "_", source);
			string attribute = addressName + "_city";
			string attribute2 = addressName + "_stateorprovince";
			string attribute3 = addressName + "_postalcode";
			string attribute4 = addressName + "_country";
			target.set_Item(addressName + "_composite", (object)context.AddressFormatter.Format(text, AddressAttributeValue(attribute, source), AddressAttributeValue(attribute2, source), AddressAttributeValue(attribute3, source), AddressAttributeValue(attribute4, source)));
		}

		private static string ComputeStreetValue(string addressPrefix, Entity entity)
		{
			string text = AddressAttributeValue(addressPrefix + "line1", entity);
			string text2 = AddressAttributeValue(addressPrefix + "line2", entity);
			string text3 = AddressAttributeValue(addressPrefix + "line3", entity);
			IEnumerable<string> values = new string[3] { text, text2, text3 }.Where((string s) => !string.IsNullOrEmpty(s));
			return string.Join(Environment.NewLine, values);
		}

		private static string AddressAttributeValue(string attribute, Entity source)
		{
			if (source.Contains(attribute))
			{
				if ((string)source.get_Item(attribute) == null)
				{
					return string.Empty;
				}
				return (string)source.get_Item(attribute);
			}
			return string.Empty;
		}

		internal static List<string[]> GetAttributeList(string entityName)
		{
			List<string[]> list = new List<string[]>();
			if (IsQOIEntity(entityName))
			{
				list.Add(BillToAttributeList);
				list.Add(ShipToAttributeList);
			}
			else
			{
				list.Add(Address1AttributeList);
				list.Add(Address2AttributeList);
				if (entityName == "contract")
				{
					list.Add(Address3AttributeList);
				}
			}
			return list;
		}

		internal static void CreatePostImage(Entity postImage, Entity preImage, Entity entity)
		{
			List<string[]> attributeList = GetAttributeList(entity.get_LogicalName());
			foreach (string[] item in attributeList)
			{
				string[] array = item;
				foreach (string text in array)
				{
					if (((DataCollection<string, object>)(object)entity.get_Attributes()).Contains(text))
					{
						((DataCollection<string, object>)(object)postImage.get_Attributes()).Add(text, ((DataCollection<string, object>)(object)entity.get_Attributes()).get_Item(text));
					}
				}
			}
			if (((DataCollection<string, object>)(object)postImage.get_Attributes()).get_Count() < 1)
			{
				return;
			}
			foreach (KeyValuePair<string, object> item2 in (DataCollection<string, object>)(object)preImage.get_Attributes())
			{
				if (!((DataCollection<string, object>)(object)postImage.get_Attributes()).Contains(item2.Key))
				{
					((DataCollection<string, object>)(object)postImage.get_Attributes()).Add(item2);
				}
			}
			postImage.set_Id(entity.get_Id());
		}

		private static void ComputeCompositeAddress(Entity source, Entity target, IPluginContext context)
		{
			if (IsQOIEntity(target.get_LogicalName()))
			{
				if (!AreConstituentAddressFieldsEmpty(BillToAttributeList, target))
				{
					ComputeCompositeAddressValue("billto", source, target, context);
					context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ComputeCompositeAddress", "BillToAttributes are passed, setting billto address.");
				}
				if (!AreConstituentAddressFieldsEmpty(ShipToAttributeList, target))
				{
					ComputeCompositeAddressValue("shipto", source, target, context);
					context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ComputeCompositeAddress", "ShipToAttributes are passed, setting shipto address.");
				}
				return;
			}
			if (!AreConstituentAddressFieldsEmpty(Address1AttributeList, target))
			{
				ComputeCompositeAddressValue("address1", source, target, context);
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ComputeCompositeAddress", "Address1Attributes are passed, setting address1.");
			}
			if (!AreConstituentAddressFieldsEmpty(Address2AttributeList, target))
			{
				ComputeCompositeAddressValue("address2", source, target, context);
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ComputeCompositeAddress", "Address2Attributes are passed, setting address2.");
			}
			if (!AreConstituentAddressFieldsEmpty(Address3AttributeList, target))
			{
				ComputeCompositeAddressValue("address3", source, target, context);
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ComputeCompositeAddress", "Address3Attributes are passed, setting address3.");
			}
		}

		public static void InitializeAddressForCreate(Entity entityObj, IPluginContext context)
		{
			ComputeCompositeAddress(entityObj, entityObj, context);
		}

		public static void InitializeAddressForUpdate(Entity entityObj, IPluginContext context)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			if (IsAddressComputationNeededForUpdate(context))
			{
				Entity val = new Entity(entityObj.get_LogicalName());
				Entity preImage = context.GetPreImage<Entity>(PreBusinessEntity);
				CreatePostImage(val, preImage, entityObj);
				if (((DataCollection<string, object>)(object)val.get_Attributes()).get_Count() > 0)
				{
					ComputeCompositeAddress(val, entityObj, context);
				}
			}
		}

		public static void InitializeAddressForRetrieve(Entity entityObj, IPluginContext context)
		{
			if (IsAddressComputationNeededForRetrieve(entityObj))
			{
				ComputeCompositeAddress(entityObj, entityObj, context);
			}
		}

		public static void InitializeAddressForRetrieveMultiple(EntityCollection entities, IPluginContext context)
		{
			if (entities == null)
			{
				return;
			}
			for (int i = 0; i < ((Collection<Entity>)(object)entities.get_Entities()).Count; i++)
			{
				if (IsAddressComputationNeededForRetrieve(entities.get_Item(i)))
				{
					ComputeCompositeAddress(entities.get_Item(i), entities.get_Item(i), context);
				}
			}
		}
	}
}
