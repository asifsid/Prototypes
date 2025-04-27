using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Interfaces;
using Microsoft.Dynamics.Solution.Common.Model;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common.Services
{
	[ComVisible(true)]
	public static class PriceUtility
	{
		private const string PriceLevelIdAttributeName = "pricelevelid";

		private const string PriceLevelIdEntityName = "pricelevel";

		private static readonly Guid DefultPriceListRoleId = new Guid("C7F4A13C-9853-4806-907F-BFC3463459A9");

		public static EntityCollection GetDefaultPriceLevel(QueryExpression expPriceLevel, IPluginContext context)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			Organization organization = context.OrganizationService.Retrieve("organization", ((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), new ColumnSet(new string[1] { "useinbuiltrulefordefaultpricelistselection" })).ToEntity<Organization>();
			Guid userId = ((IExecutionContext)context.PluginExecutionContext).get_UserId();
			return (organization.UseInbuiltRuleForDefaultPricelistSelection.GetValueOrDefault() && userId != Guid.Empty) ? GetPriceList(userId, expPriceLevel, context) : null;
		}

		private static EntityCollection GetPriceList(Guid userId, QueryExpression expPriceLevel, IPluginContext context)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_0060: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			LinkEntity val = new LinkEntity();
			val.set_LinkFromEntityName("connection");
			val.set_LinkFromAttributeName("record1id");
			val.set_LinkToEntityName("systemuser");
			val.set_LinkToAttributeName("territoryid");
			((Collection<ConditionExpression>)(object)val.get_LinkCriteria().get_Conditions()).Add(new ConditionExpression("systemuserid", (ConditionOperator)0, (object)userId.ToString()));
			LinkEntity item = val;
			LinkEntity val2 = new LinkEntity();
			val2.set_LinkFromEntityName("pricelevel");
			val2.set_LinkFromAttributeName("pricelevelid");
			val2.set_LinkToEntityName("connection");
			val2.set_LinkToAttributeName("record2id");
			DataCollection<ConditionExpression> conditions = val2.get_LinkCriteria().get_Conditions();
			Guid defultPriceListRoleId = DefultPriceListRoleId;
			((Collection<ConditionExpression>)(object)conditions).Add(new ConditionExpression("record1roleid", (ConditionOperator)0, (object)defultPriceListRoleId.ToString()));
			((Collection<LinkEntity>)(object)val2.get_LinkEntities()).Add(item);
			LinkEntity item2 = val2;
			((Collection<LinkEntity>)(object)expPriceLevel.get_LinkEntities()).Add(item2);
			expPriceLevel.set_Distinct(true);
			return context.OrganizationService.RetrieveMultiple((QueryBase)(object)expPriceLevel);
		}

		public static void SetDefaultPriceLevel(Entity entity, string entityName, IPluginContext context, IAttributeUtility attributeUtility = null)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Expected O, but got Unknown
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Expected O, but got Unknown
			attributeUtility = attributeUtility ?? AttributeUtility.Instance;
			IOrganizationService organizationService = context.OrganizationService;
			RetrieveAttributeRequest val = new RetrieveAttributeRequest();
			val.set_EntityLogicalName(entityName);
			val.set_LogicalName("pricelevelid");
			val.set_RetrieveAsIfPublished(true);
			RetrieveAttributeRequest val2 = val;
			RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)organizationService.Execute((OrganizationRequest)(object)val2);
			AttributeMetadataWrapper attributeMetadataForSetDefaultPriceLevel = attributeUtility.GetAttributeMetadataForSetDefaultPriceLevel(retrieveAttributeResponse);
			if (attributeMetadataForSetDefaultPriceLevel.IsSecured.HasValue && attributeMetadataForSetDefaultPriceLevel.CanBeSecuredForCreate.HasValue && attributeMetadataForSetDefaultPriceLevel.IsValidForCreate.HasValue && (!attributeMetadataForSetDefaultPriceLevel.IsSecured.Value || (attributeMetadataForSetDefaultPriceLevel.CanBeSecuredForCreate.Value && attributeMetadataForSetDefaultPriceLevel.IsValidForCreate.Value)))
			{
				QueryExpression val3 = new QueryExpression();
				val3.set_EntityName("pricelevel");
				val3.set_ColumnSet(new ColumnSet(new string[1] { "pricelevelid" }));
				val3.set_PageInfo(new PagingInfo());
				QueryExpression val4 = val3;
				val4.get_PageInfo().set_Count(2);
				val4.get_PageInfo().set_PageNumber(1);
				val4.get_PageInfo().set_ReturnTotalRecordCount(true);
				EntityCollection defaultPriceLevel = GetDefaultPriceLevel(val4, context);
				if (defaultPriceLevel != null && defaultPriceLevel.get_TotalRecordCount() == 1 && !defaultPriceLevel.get_Item(0).IsAttributeNull("pricelevelid"))
				{
					entity.set_Item("pricelevelid", (object)new EntityReference("pricelevel", (Guid)defaultPriceLevel.get_Item(0).get_Item("pricelevelid")));
				}
			}
		}
	}
}
