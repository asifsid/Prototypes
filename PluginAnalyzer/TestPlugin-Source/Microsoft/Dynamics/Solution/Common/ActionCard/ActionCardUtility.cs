using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public static class ActionCardUtility
	{
		public static Microsoft.Dynamics.Solution.Common.Proxies.ActionCard CreateNewActionCardEntityInstance(Guid actionCardRecordId)
		{
			Microsoft.Dynamics.Solution.Common.Proxies.ActionCard actionCard = new Microsoft.Dynamics.Solution.Common.Proxies.ActionCard();
			actionCard.ActionCardId = actionCardRecordId;
			return actionCard;
		}

		public static Microsoft.Dynamics.Solution.Common.Proxies.ActionCard InitActionCardFromEntity(Entity entity, IPluginContext context)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			EntityReference val = entity.GetAttributeValue<EntityReference>("ownerid");
			if (val == null)
			{
				val = new EntityReference("systemuser", ((IExecutionContext)context.PluginExecutionContext).get_UserId());
			}
			Microsoft.Dynamics.Solution.Common.Proxies.ActionCard actionCard = new Microsoft.Dynamics.Solution.Common.Proxies.ActionCard();
			actionCard.RegardingObjectId = entity.ToEntityReference();
			actionCard.OwnerId = val;
			return actionCard;
		}

		public static void SetCardTypeInformation(this Microsoft.Dynamics.Solution.Common.Proxies.ActionCard actionCard, Guid cardTypeId, bool visibility, ActionCardPriority priority)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			actionCard.Visibility = visibility;
			actionCard.Priority = (int)priority;
			actionCard.CardTypeId = new EntityReference("cardtype", cardTypeId);
		}

		public static void SetActionCardDateRange(this Microsoft.Dynamics.Solution.Common.Proxies.ActionCard actionCard, DateTime startDate, DateTime endDate)
		{
			actionCard.StartDate = startDate;
			actionCard.ExpiryDate = endDate;
		}

		public static void SetTokenIformation(this Microsoft.Dynamics.Solution.Common.Proxies.ActionCard actionCard, string token)
		{
			actionCard.ReferenceTokens = token;
		}

		public static void SetActionCardState(this Microsoft.Dynamics.Solution.Common.Proxies.ActionCard actionCard, ActionCardActionCardState state)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			actionCard.State = new OptionSetValue((int)state);
		}

		public static EntityCollection RetrieveRelatedActiveActionCardRecords(Guid recordId, IPluginContext context, ColumnSet columns = null, bool activeOnly = true)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			QueryExpression val = new QueryExpression();
			val.set_EntityName("actioncard");
			if (columns == null)
			{
				columns = new ColumnSet(new string[7] { "startdate", "expirydate", "state", "cardtypeid", "data", "referencetokens", "cardtype" });
			}
			val.set_ColumnSet(columns);
			val.get_Criteria().AddCondition("regardingobjectid", (ConditionOperator)0, new object[1] { recordId });
			if (activeOnly)
			{
				val.get_Criteria().AddCondition("state", (ConditionOperator)1, new object[1] { 2 });
			}
			EntityCollection val2 = context.SystemUserOrganizationService.RetrieveMultiple((QueryBase)(object)val);
			if (val2 != null && ((Collection<Entity>)(object)val2.get_Entities()).Count > 0)
			{
				return val2;
			}
			return null;
		}

		public static int GetActionCardOptionValueFromUserSettings(Guid userId, Guid actionCardTypeId, int defaultValue, IPluginContext context)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Expected O, but got Unknown
			int result = defaultValue;
			QueryExpression val = new QueryExpression();
			val.set_EntityName("cardtype");
			val.get_ColumnSet().AddColumns(new string[2] { "intcardoption", "cardtypeid" });
			val.get_Criteria().AddCondition("cardtypeid", (ConditionOperator)0, new object[1] { actionCardTypeId });
			LinkEntity val2 = val.AddLink("actioncardusersettings", "cardtypeid", "cardtypeid");
			val2.set_JoinOperator((JoinOperator)1);
			val2.get_Columns().AddColumns(new string[2] { "cardtypeid", "intcardoption" });
			EntityCollection val3 = context.SystemUserOrganizationService.RetrieveMultiple((QueryBase)(object)val);
			foreach (Entity item in (Collection<Entity>)(object)val3.get_Entities())
			{
				if (item.get_Item("intcardoption") != null)
				{
					result = (int)item.get_Item("intcardoption");
					break;
				}
			}
			return result;
		}

		public static Entity RetrieveEntityRecord(EntityReference reference, IPluginContext context)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			ColumnSet val = new ColumnSet(true);
			return context.SystemUserOrganizationService.Retrieve(reference.get_LogicalName(), reference.get_Id(), val);
		}
	}
}
