using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class CustomizationService
	{
		private IPluginContext pluginContext;

		public CustomizationService(IPluginContext pluginContext)
		{
			Exceptions.ThrowIfNull(pluginContext, "pluginContext");
			this.pluginContext = pluginContext;
		}

		public Entity InitializeFrom(EntityReference sourceEntityReference, string targetEntityName, TargetFieldType targetFieldType, bool mapReadSecuredOnSourceAndTarget)
		{
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected O, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Expected O, but got Unknown
			Exceptions.ThrowIfNull(sourceEntityReference, "SourceEntityReference");
			Exceptions.ThrowIfNull(targetEntityName, "targetEntityName");
			Dictionary<string, AttributeMetadata> attributesMetadata = GetAttributesMetadata(sourceEntityReference.get_LogicalName(), pluginContext);
			Dictionary<string, AttributeMetadata> attributesMetadata2 = GetAttributesMetadata(targetEntityName, pluginContext);
			EntityCollection val = RetrieveAttributeMaps(sourceEntityReference.get_LogicalName(), targetEntityName, pluginContext);
			if (((Collection<Entity>)(object)val.get_Entities()).Count == 0)
			{
				pluginContext.Trace("InitializeFrom cannot be invoked from source entity of type " + sourceEntityReference.get_LogicalName() + " with " + $"id {sourceEntityReference.get_Id()} to target entity type {targetEntityName} because there is no entity map defined between these two entities.");
				throw new CrmException(string.Format(Labels.NoIdentityMap, sourceEntityReference.get_LogicalName(), targetEntityName));
			}
			QueryExpression val2 = new QueryExpression(sourceEntityReference.get_LogicalName());
			List<Entity> list = new List<Entity>();
			foreach (Entity item in (Collection<Entity>)(object)val.get_Entities())
			{
				if (ValidForTransform(item, attributesMetadata2, targetFieldType))
				{
					string key = (string)item.get_Item("sourceattributename");
					list.Add(item);
					AttributeMetadata val3 = attributesMetadata[key];
					if (val3.get_AttributeOf() == null)
					{
						val2.get_ColumnSet().AddColumn((string)item.get_Item("sourceattributename"));
					}
				}
			}
			Entity val4 = pluginContext.OrganizationService.Retrieve(sourceEntityReference.get_LogicalName(), sourceEntityReference.get_Id(), val2.get_ColumnSet());
			Entity val5 = new Entity(targetEntityName);
			GetValidMappings(list, val4, attributesMetadata, attributesMetadata2, out var sourceAttributes, out var targetAttributes);
			if (mapReadSecuredOnSourceAndTarget)
			{
				FieldLevelSecurityChecks(attributesMetadata, attributesMetadata2, sourceAttributes, targetAttributes, pluginContext);
			}
			for (int i = 0; i < sourceAttributes.Count; i++)
			{
				AttributeMetadata sourceAttributeMetadata = attributesMetadata[sourceAttributes[i]];
				AttributeMetadata targetAttributeMetadata = attributesMetadata2[targetAttributes[i]];
				if (val4.Contains(sourceAttributes[i]))
				{
					UpdateAttributeValue(val4, sourceAttributes[i], sourceAttributeMetadata, val5, targetAttributes[i], targetAttributeMetadata);
				}
				else
				{
					pluginContext.Trace(string.Format(Labels.NoConversionWillHappenForEntityWithoutAttributes, sourceAttributes[i]));
				}
			}
			return val5;
		}

		private EntityCollection RetrieveAttributeMaps(string sourceEntityName, string targetEntityName, IPluginContext context)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			QueryExpression val = new QueryExpression("attributemap");
			val.get_ColumnSet().AddColumns(new string[3] { "sourceattributename", "targetattributename", "parentattributemapid" });
			LinkEntity val2 = val.AddLink("entitymap", "entitymapid", "entitymapid");
			val2.get_LinkCriteria().AddCondition("sourceentityname", (ConditionOperator)0, new object[1] { sourceEntityName });
			val2.get_LinkCriteria().AddCondition("targetentityname", (ConditionOperator)0, new object[1] { targetEntityName });
			return context.OrganizationService.RetrieveMultiple((QueryBase)(object)val);
		}

		private bool ValidForTransform(Entity attributeMap, Dictionary<string, AttributeMetadata> attributesMetadataCollection, TargetFieldType fieldType)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected I4, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			bool result = false;
			string key = (string)attributeMap.get_Item("targetattributename");
			if (!attributesMetadataCollection.ContainsKey(key))
			{
				return false;
			}
			AttributeMetadata val = attributesMetadataCollection[key];
			switch ((int)fieldType)
			{
			case 0:
				result = true;
				break;
			case 3:
				result = val.get_IsValidForRead().Value;
				break;
			case 1:
				result = val.get_IsValidForCreate().Value;
				break;
			case 2:
				if (val.get_IsValidForUpdate().Value)
				{
					result = true;
				}
				break;
			default:
				throw new CrmException(string.Format(Labels.InvalidParameter, "TargetFieldType", fieldType));
			}
			return result;
		}

		private void GetValidMappings(List<Entity> attributeMaps, Entity sourceEntity, Dictionary<string, AttributeMetadata> sourceAttributeMetadataCollection, Dictionary<string, AttributeMetadata> targetAttributeMetadataCollection, out List<string> sourceAttributes, out List<string> targetAttributes)
		{
			List<string> list = new List<string>(attributeMaps.Count);
			List<string> list2 = new List<string>(attributeMaps.Count);
			List<string> list3 = new List<string>();
			foreach (Entity attributeMap in attributeMaps)
			{
				if (attributeMap.IsAttributeNull("parentattributemapid"))
				{
					string text = (string)attributeMap.get_Item("sourceattributename");
					string text2 = (string)attributeMap.get_Item("targetattributename");
					AttributeMetadata sourceAttributeMetadata = sourceAttributeMetadataCollection[text];
					AttributeMetadata targetAttributeMetadata = targetAttributeMetadataCollection[text2];
					if (IsValidPicklistMapping(sourceAttributeMetadata, targetAttributeMetadata, sourceEntity))
					{
						list.Add(text);
						list2.Add(text2);
					}
					else
					{
						list3.Add(attributeMap.get_Item("attributemapid").ToString());
					}
				}
			}
			AddCurrencyMapping(sourceAttributeMetadataCollection, targetAttributeMetadataCollection, sourceEntity, list, list2);
			foreach (Entity attributeMap2 in attributeMaps)
			{
				if (!attributeMap2.IsAttributeNull("parentattributemapid"))
				{
					object value = attributeMap2.get_Item("parentattributemapid");
					if (!list3.Contains(value))
					{
						list.Add((string)attributeMap2.get_Item("sourceattributename"));
						list2.Add((string)attributeMap2.get_Item("targetattributename"));
					}
				}
			}
			sourceAttributes = list;
			targetAttributes = list2;
		}

		private void AddCurrencyMapping(Dictionary<string, AttributeMetadata> sourceAttributeMetadataCollection, Dictionary<string, AttributeMetadata> targetAttributeMetadataCollection, Entity sourceEntity, List<string> validSourceAttributes, List<string> validTargetAttributes)
		{
			if (sourceAttributeMetadataCollection.ContainsKey("transactioncurrencyid") && targetAttributeMetadataCollection.ContainsKey("transactioncurrencyid"))
			{
				validSourceAttributes.Add("transactioncurrencyid");
				validTargetAttributes.Add("transactioncurrencyid");
			}
		}

		private bool IsValidPicklistMapping(AttributeMetadata sourceAttributeMetadata, AttributeMetadata targetAttributeMetadata, Entity sourceEntity)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Invalid comparison between Unknown and I4
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Invalid comparison between Unknown and I4
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			AttributeTypeCode value = sourceAttributeMetadata.get_AttributeType().Value;
			AttributeTypeCode value2 = targetAttributeMetadata.get_AttributeType().Value;
			if ((int)value == 11 && (int)value2 == 11 && !sourceEntity.IsAttributeNull(sourceAttributeMetadata.get_LogicalName()))
			{
				int sourcePicklistValue = ((OptionSetValue)sourceEntity.get_Item(sourceAttributeMetadata.get_LogicalName())).get_Value();
				OptionSetMetadata optionSet = ((EnumAttributeMetadata)targetAttributeMetadata).get_OptionSet();
				if (!((IEnumerable<OptionMetadata>)optionSet.get_Options()).Any((OptionMetadata x) => x.get_Value() == sourcePicklistValue))
				{
					return false;
				}
			}
			return true;
		}

		private void FieldLevelSecurityChecks(Dictionary<string, AttributeMetadata> sourceAttributeMetadataCollection, Dictionary<string, AttributeMetadata> targetAttributeMetadataCollection, List<string> validSourceAttributes, List<string> validTargetAttributes, IPluginContext context)
		{
		}

		private static EntityMetadata GetEntityMetadata(string entityName, IPluginContext pluginContext)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			RetrieveEntityRequest val = new RetrieveEntityRequest();
			val.set_EntityFilters((EntityFilters)7);
			val.set_LogicalName(entityName);
			val.set_RetrieveAsIfPublished(true);
			RetrieveEntityRequest val2 = val;
			RetrieveEntityResponse val3 = (RetrieveEntityResponse)pluginContext.OrganizationService.Execute((OrganizationRequest)(object)val2);
			return val3.get_EntityMetadata();
		}

		private static Dictionary<string, AttributeMetadata> GetAttributesMetadata(string entityName, IPluginContext pluginContext)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			EntityMetadata val = pluginContext.EntityMetadataCache.Get(entityName);
			Dictionary<string, AttributeMetadata> dictionary = new Dictionary<string, AttributeMetadata>();
			AttributeMetadata[] attributes = val.get_Attributes();
			foreach (AttributeMetadata val2 in attributes)
			{
				dictionary.Add(val2.get_LogicalName(), val2);
			}
			return dictionary;
		}

		private static void UpdateAttributeValue(Entity sourceEntity, string sourceAttributeName, AttributeMetadata sourceAttributeMetadata, Entity targetEntity, string targetAttributeName, AttributeMetadata targetAttributeMetadata)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			object obj = sourceEntity.get_Item(sourceAttributeName);
			if (((object)targetAttributeMetadata).GetType().Name != ((object)sourceAttributeMetadata).GetType().Name)
			{
				if (!(targetAttributeMetadata is LookupAttributeMetadata) || (!(obj is string) && !(obj is Guid)))
				{
					throw new CrmInvalidOperationException(string.Format(Labels.CannotConvertAttributes, ((object)sourceAttributeMetadata).GetType().Name, ((object)targetAttributeMetadata).GetType().Name));
				}
				targetEntity.set_Item(targetAttributeName, (object)new EntityReference(sourceAttributeMetadata.get_EntityLogicalName(), Guid.Parse(obj.ToString())));
			}
			else
			{
				targetEntity.set_Item(targetAttributeName, obj);
			}
		}
	}
}
