using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Kernel.Contracts.CommandExecution;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class ObjectServiceUtility
	{
		public const string ErrorNoLooping = "0";

		public const string ErrorLoopExists = "1";

		public const string ErrorLoopCreated = "2";

		public const string ErrorInvalidChildId = "3";

		public const string ErrorInvalidParentId = "4";

		public const string ErrorUnknown = "5";

		public static bool IsUomInUomSchedule(Guid uomId, Guid uomScheduleId, IPluginContext context, bool isSystemUserContext = false)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			IOrganizationService val = (isSystemUserContext ? context.SystemUserOrganizationService : context.OrganizationService);
			Entity val2 = val.Retrieve("uom", uomId, new ColumnSet(new string[1] { "uomscheduleid" }));
			if (val2.IsAttributeNull("uomscheduleid"))
			{
				throw new CrmException(Labels.MissingUomScheduleId, -2147206390);
			}
			object obj = val2.get_Item("uomscheduleid");
			return uomScheduleId == (obj as EntityReference).get_Id();
		}

		public static bool IsUnitValidForProduct(Guid uomId, Guid productId, IPluginContext context, bool isSystemUser = false)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			IOrganizationService val = (isSystemUser ? context.SystemUserOrganizationService : context.OrganizationService);
			Entity val2 = val.Retrieve("product", productId, new ColumnSet(new string[1] { "defaultuomscheduleid" }));
			if (val2.IsAttributeNull("defaultuomscheduleid"))
			{
				throw new CrmException("The unit schedule id of the product is missing.", -2147206390);
			}
			object obj = val2.get_Item("defaultuomscheduleid");
			return IsUomInUomSchedule(uomId, (obj as EntityReference).get_Id(), context);
		}

		public static bool IsProductFamily(Guid productId, IPluginContext context, bool isSystemUser = false)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Entity val = RetrieveProductDetail(productId, new ColumnSet(new string[1] { "productstructure" }), context);
			return !val.IsAttributeNull("productstructure") && ((OptionSetValue)val.get_Item("productstructure")).get_Value() == 2;
		}

		public static Entity RetrieveProductDetail(Guid productId, ColumnSet columnSet, IPluginContext context, bool isSystemUser = false)
		{
			IOrganizationService val = (isSystemUser ? context.SystemUserOrganizationService : context.OrganizationService);
			return val.Retrieve("product", productId, columnSet);
		}

		public static AttributeMetadata GetAttributeMetaData(IPluginContext context, string entityName, string attributeName)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			RetrieveAttributeRequest val = new RetrieveAttributeRequest();
			val.set_EntityLogicalName(entityName);
			val.set_LogicalName(attributeName);
			RetrieveAttributeRequest val2 = val;
			IOrganizationService organizationService = context.OrganizationService;
			RetrieveAttributeResponse val3 = (RetrieveAttributeResponse)organizationService.Execute((OrganizationRequest)(object)val2);
			return val3.get_AttributeMetadata();
		}

		public static Entity CloneEntity(IPluginContext context, Entity origEntity, CloneOptions CoP)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			Entity val = new Entity(origEntity.get_LogicalName());
			val.set_RowVersion(origEntity.get_RowVersion());
			RetrieveEntityRequest val2 = new RetrieveEntityRequest();
			val2.set_EntityFilters((EntityFilters)15);
			val2.set_LogicalName(origEntity.get_LogicalName());
			RetrieveEntityRequest val3 = val2;
			RetrieveEntityResponse val4 = (RetrieveEntityResponse)context.OrganizationService.Execute((OrganizationRequest)(object)val3);
			foreach (KeyValuePair<string, object> item in (DataCollection<string, object>)(object)origEntity.get_Attributes())
			{
				AttributeMetadata[] attributes = val4.get_EntityMetadata().get_Attributes();
				foreach (AttributeMetadata val5 in attributes)
				{
					if (!(val5.get_LogicalName() != item.Key) && (CoP != CloneOptions.ValidForCreate || val5.get_IsValidForCreate() != false) && (CoP != CloneOptions.ValidForUpdate || val5.get_IsValidForUpdate() != false || val5.get_IsPrimaryId() != false))
					{
						val.set_Item(item.Key, item.Value);
					}
				}
			}
			val.set_EntityState(origEntity.get_EntityState());
			((DataCollection<Relationship, EntityCollection>)(object)val.get_RelatedEntities()).AddRange((IEnumerable<KeyValuePair<Relationship, EntityCollection>>)origEntity.get_RelatedEntities());
			return val;
		}

		public static EntityMetadata GetEntityMetaData(IPluginContext context, string entityName, EntityFilters metadataToRetrieve)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			RetrieveEntityRequest val = new RetrieveEntityRequest();
			val.set_EntityFilters(metadataToRetrieve);
			val.set_LogicalName(entityName);
			RetrieveEntityRequest val2 = val;
			RetrieveEntityResponse val3 = (RetrieveEntityResponse)context.OrganizationService.Execute((OrganizationRequest)(object)val2);
			return val3.get_EntityMetadata();
		}

		public static bool IsBundle(Guid productId, IPluginContext context)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Entity val = RetrieveProductDetail(productId, new ColumnSet(new string[1] { "productstructure" }), context);
			return !val.IsAttributeNull("productstructure") && ((OptionSetValue)val.get_Item("productstructure")).get_Value() == 3;
		}

		public static bool IsKit(Guid productId, IPluginContext context)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			Product product = RetrieveProductDetail(productId, new ColumnSet(new string[2] { "iskit", "productstructure" }), context).ToEntity<Product>();
			return IsKit(product);
		}

		public static bool IsKit(Product product)
		{
			return !((Entity)(object)product).IsAttributeNull("iskit") && !((Entity)(object)product).IsAttributeNull("productstructure") && product.IsKit.Value && product.ProductStructure.get_Value() == 1;
		}

		public static bool IsRetired(Product product)
		{
			return !((Entity)(object)product).IsAttributeNull("statecode") && product.StateCode.Value == ProductState.Inactive;
		}

		public static void ValidateProductStructure(EntityReference productReference, IPluginContext context)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			if (productReference != null && productReference.get_LogicalName().Equals("product", StringComparison.InvariantCultureIgnoreCase))
			{
				Product product = context.OrganizationService.Retrieve<Product>(productReference.get_Id(), new ColumnSet(new string[1] { "productstructure" }));
				if (product != null && product.ProductStructure != null && product.ProductStructure.get_Value() == 2)
				{
					throw new CrmException(Labels.InvalidProduct, -2147088861);
				}
			}
		}

		public static int RetrieveStateFromDatabase(EntityReference entityReference, IPluginContext context, bool isSystemUser = false)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			IOrganizationService val = ((!isSystemUser) ? context.OrganizationService : context.SystemUserOrganizationService);
			Entity val2 = val.Retrieve(entityReference.get_LogicalName(), entityReference.get_Id(), new ColumnSet(new string[1] { "statecode" }));
			return ((OptionSetValue)val2.get_Item("statecode")).get_Value();
		}

		public static bool IsBackOfficeInstalled(IPluginContext context)
		{
			return context.GetSettings<bool>("IsSOPIntegrationEnabled");
		}

		public static bool IsCurrentUserIntegrationUser(IPluginContext context)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			Organization organization = context.OrganizationService.Retrieve<Organization>(((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), new ColumnSet(new string[1] { "integrationuserid" }));
			if (!organization.IntegrationUserId.HasValue)
			{
				return false;
			}
			User user = context.OrganizationService.Retrieve<User>(((IExecutionContext)context.PluginExecutionContext).get_UserId(), new ColumnSet(new string[1] { "isintegrationuser" }));
			return ((IExecutionContext)context.PluginExecutionContext).get_UserId() == organization.IntegrationUserId.Value || user.IsIntegrationUser.GetValueOrDefault();
		}

		public static void ValidateCyclicAssociation(IPluginContext context, Guid baseProductId, Guid associatingProductId)
		{
			switch (DetectLoop("@ProductId", "@KitId", associatingProductId, baseProductId, "p_DetectProductKitLoop", context))
			{
			case "0":
				break;
			case "1":
				throw new CrmException(Labels.ProductKitLoopExists, -2147206366);
			case "2":
				throw new CrmException(Labels.ProductKitLoopBeingCreated, -2147206365);
			case "3":
				throw new CrmException(Labels.ProductDoesNotExist, -2147206364);
			case "4":
				throw new CrmException(Labels.ProductDoesNotExist, -2147206364);
			default:
				throw new CrmException(Labels.unManagedidsdataaccessunexpected, -2147212544);
			}
		}

		public static void VerifyIfAssociatedRecordsExist(string associatedEntityName, string associatedEntityPrimaryKey, Entity primaryEntity, string primaryEntityAttributeName, int errorCode, string errorMessage, IOrganizationService organizationService)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			QueryExpression val = new QueryExpression(associatedEntityName);
			val.set_ColumnSet(new ColumnSet(new string[1] { associatedEntityPrimaryKey }));
			val.get_Criteria().AddCondition(primaryEntityAttributeName, (ConditionOperator)0, new object[1] { primaryEntity.get_Id() });
			EntityCollection val2 = organizationService.RetrieveMultiple((QueryBase)(object)val);
			if (((Collection<Entity>)(object)val2.get_Entities()).Count > 0)
			{
				throw new CrmException(errorMessage, errorCode);
			}
		}

		public static string GetHierarchyPath(string parentHierarchyPath, string parentProductName, IPluginContext context)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(parentProductName))
			{
				return empty;
			}
			if (!string.IsNullOrEmpty(parentHierarchyPath))
			{
				int culture = context.DetermineCallingUserLanguage();
				CultureInfo cultureInfo = new CultureInfo(culture);
				bool isRightToLeft = cultureInfo.TextInfo.IsRightToLeft;
				string arg = (isRightToLeft ? "/" : "\\");
				int num = 450;
				if (num == 0)
				{
					return empty;
				}
				empty = ((!isRightToLeft) ? string.Format(cultureInfo, "{0}{1}{2}", parentHierarchyPath, arg, parentProductName) : string.Format(cultureInfo, "{0}{1}{2}", parentProductName, arg, parentHierarchyPath));
				string @string = Labels.ResourceManager.GetString("HierarchyPathEllipsis", cultureInfo);
				if (empty.Length > num - @string.Length)
				{
					empty = empty.Substring(0, num - @string.Length) + @string;
				}
			}
			else
			{
				empty = parentProductName;
			}
			return empty;
		}

		public static string DetectLoop(string childIdParameterName, string parentIdParameterName, Guid childId, Guid parentId, string storedProcedureName, IPluginContext context)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			ICommandExecutionService commandExecutionService = context.CommandExecutionService;
			CommandInfo val = new CommandInfo(storedProcedureName, (CommandTimeout)1, false);
			val.Add(childIdParameterName, (CommandParameterType)0, (object)childId);
			val.Add(parentIdParameterName, (CommandParameterType)0, (object)parentId);
			try
			{
				return commandExecutionService.ExecuteScalar<string>(val);
			}
			catch (DataException innerException)
			{
				throw new CrmException(Labels.LoopDetectionFailed, innerException);
			}
		}

		public static Entity RetrieveEntity(EntityReference entityReference, string[] fields, IPluginContext context, bool isSystemUserContext = false)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			IOrganizationService val = (isSystemUserContext ? context.SystemUserOrganizationService : context.OrganizationService);
			string logicalName = entityReference.get_LogicalName();
			Guid id = entityReference.get_Id();
			ColumnSet val2 = new ColumnSet(fields);
			return val.Retrieve(logicalName, id, val2);
		}

		public static Entity RetrieveEntity(EntityReference entityReference, IPluginContext context, bool isSystemUserContext = false)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			if (!isSystemUserContext)
			{
				_ = context.OrganizationService;
			}
			else
			{
				_ = context.SystemUserOrganizationService;
			}
			string logicalName = entityReference.get_LogicalName();
			Guid id = entityReference.get_Id();
			return context.OrganizationService.Retrieve(logicalName, id, new ColumnSet(true));
		}

		public static string GetEntityLogicalNameFromObjectTypeCode(int objectTypeCode, IPluginContext context)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			string result = string.Empty;
			MetadataFilterExpression val = new MetadataFilterExpression((LogicalOperator)0);
			((Collection<MetadataConditionExpression>)(object)val.get_Conditions()).Add(new MetadataConditionExpression("ObjectTypeCode", (MetadataConditionOperator)0, (object)objectTypeCode));
			MetadataPropertiesExpression val2 = new MetadataPropertiesExpression();
			val2.set_AllProperties(false);
			((Collection<string>)(object)val2.get_PropertyNames()).Add("DisplayName");
			((Collection<string>)(object)val2.get_PropertyNames()).Add("ObjectTypeCode");
			((Collection<string>)(object)val2.get_PropertyNames()).Add("PrimaryIdAttribute");
			((Collection<string>)(object)val2.get_PropertyNames()).Add("PrimaryNameAttribute");
			EntityQueryExpression val3 = new EntityQueryExpression();
			((MetadataQueryExpression)val3).set_Criteria(val);
			((MetadataQueryExpression)val3).set_Properties(val2);
			EntityQueryExpression entityQueryExpression = val3;
			RetrieveMetadataChangesResponse metadataChanges = GetMetadataChanges(entityQueryExpression, null, (DeletedMetadataFilters)16, context);
			if (((Collection<EntityMetadata>)(object)metadataChanges.get_EntityMetadata()).Count == 1)
			{
				result = ((Collection<EntityMetadata>)(object)metadataChanges.get_EntityMetadata())[0].get_LogicalName();
			}
			return result;
		}

		private static RetrieveMetadataChangesResponse GetMetadataChanges(EntityQueryExpression entityQueryExpression, string clientVersionStamp, DeletedMetadataFilters deletedMetadataFilter, IPluginContext context)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			RetrieveMetadataChangesRequest val = new RetrieveMetadataChangesRequest();
			val.set_Query(entityQueryExpression);
			val.set_ClientVersionStamp(clientVersionStamp);
			val.set_DeletedMetadataFilters(deletedMetadataFilter);
			RetrieveMetadataChangesRequest val2 = val;
			return (RetrieveMetadataChangesResponse)context.OrganizationService.Execute((OrganizationRequest)(object)val2);
		}

		public static void InheritFromTemplate(Entity entity, Entity template, string attributeName)
		{
			if (entity.IsAttributeNull(attributeName) && !template.IsAttributeNull(attributeName))
			{
				entity.set_Item(attributeName, template.get_Item(attributeName));
			}
		}

		public static bool HasServiceStage(IPluginContext context, string entityName, string attributeName)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			EnumAttributeMetadata val = (EnumAttributeMetadata)GetAttributeMetaData(context, entityName, attributeName);
			return val != null && ((Collection<OptionMetadata>)(object)val.get_OptionSet().get_Options()).Count != 0;
		}

		public static int GetFirstServiceStage(IPluginContext context, string entityName, string attributeName)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (!HasServiceStage(context, entityName, attributeName))
			{
				return 0;
			}
			EnumAttributeMetadata val = (EnumAttributeMetadata)GetAttributeMetaData(context, entityName, attributeName);
			return ((Collection<OptionMetadata>)(object)val.get_OptionSet().get_Options())[0].get_Value().GetValueOrDefault();
		}

		public static int GetLastServiceStage(IPluginContext context, string entityName, string attributeName)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (!HasServiceStage(context, entityName, attributeName))
			{
				return 0;
			}
			EnumAttributeMetadata val = (EnumAttributeMetadata)GetAttributeMetaData(context, entityName, attributeName);
			return ((Collection<OptionMetadata>)(object)val.get_OptionSet().get_Options())[((Collection<OptionMetadata>)(object)val.get_OptionSet().get_Options()).Count - 1].get_Value().GetValueOrDefault();
		}

		public static int? GetLocaleId(IPluginContext context)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			return context.OrganizationService.Retrieve("usersettings", ((IExecutionContext)context.PluginExecutionContext).get_UserId(), new ColumnSet(new string[1] { "localeid" })).ToEntity<UserSettings>().LocaleId;
		}

		public static Entity RetrieveEntityWithSkipPlugin(EntityReference entityReference, IPluginContext context, bool isSystemUserContext = false)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			IOrganizationService organizationService = (isSystemUserContext ? context.SystemUserOrganizationService : context.OrganizationService);
			string logicalName = entityReference.get_LogicalName();
			Guid id = entityReference.get_Id();
			return organizationService.RetrieveWithSkipPluginsSet(context, logicalName, id, new ColumnSet(true));
		}
	}
}
