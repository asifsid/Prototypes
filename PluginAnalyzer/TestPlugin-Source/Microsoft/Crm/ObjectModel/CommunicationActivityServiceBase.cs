using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Crm.Common.ObjectModel;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Dynamics.Solution.Common;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Crm.ObjectModel
{
	[ComVisible(true)]
	public class CommunicationActivityServiceBase
	{
		private Dictionary<string, int> _objectTypeCodesCache;

		public void BeforeAssign(EntityReference entity, EntityReference assignee, IPluginContext context)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			Entity val = context.OrganizationService.Retrieve(entity.get_LogicalName(), entity.get_Id(), new ColumnSet(new string[2] { "ownerid", "owneridtype" }));
			PrincipalAccess val2 = new PrincipalAccess();
			val2.set_Principal(val.GetAttributeValue<EntityReference>("ownerid"));
			val2.set_AccessMask((AccessRights)1);
			if (val2.get_Principal().get_Id() != assignee.get_Id())
			{
				EntityReference attributeValue = val.GetAttributeValue<EntityReference>("ownerid");
				if (RetainOldOwner(context, entity.get_Id(), attributeValue.get_Id()))
				{
					GrantAccess(entity, val2, context);
				}
				RevokeAccess(entity, assignee, context);
			}
		}

		private bool RetainOldOwner(IPluginContext context, Guid activityId, Guid partyId)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			QueryExpression val = new QueryExpression("activityparty");
			val.get_Criteria().AddCondition("activityid", (ConditionOperator)0, new object[1] { activityId });
			val.get_Criteria().AddCondition("partyid", (ConditionOperator)0, new object[1] { partyId });
			val.get_Criteria().AddCondition("participationtypemask", (ConditionOperator)1, new object[1] { ParticipationType.Owner });
			val.set_NoLock(true);
			EntityCollection val2 = context.OrganizationService.RetrieveMultiple((QueryBase)(object)val);
			return val2.get_Entities() != null && ((Collection<Entity>)(object)val2.get_Entities()).Count > 0;
		}

		private void RevokeAccess(EntityReference entity, EntityReference assignee, IPluginContext context)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			RevokeAccessRequest val = new RevokeAccessRequest();
			val.set_Revokee(assignee);
			val.set_Target(entity);
			RevokeAccessRequest val2 = val;
			context.OrganizationService.Execute((OrganizationRequest)(object)val2);
		}

		public static void GrantAccess(EntityReference entity, PrincipalAccess assignee, IPluginContext context)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			GrantAccessRequest val = new GrantAccessRequest();
			val.set_PrincipalAccess(assignee);
			val.set_Target(entity);
			GrantAccessRequest val2 = val;
			context.OrganizationService.Execute((OrganizationRequest)(object)val2);
		}

		protected void ResolveUnresolvedParties(IEnumerable<Entity> parties, int[] objectTypes, IPluginContext context)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			if (parties == null)
			{
				return;
			}
			foreach (Entity party in parties)
			{
				ActivityParty activityParty = party.ToEntity<ActivityParty>();
				if (activityParty.PartyId != null || string.IsNullOrEmpty(activityParty.AddressUsed))
				{
					continue;
				}
				OrganizationRequest val = new OrganizationRequest("ResolveEmailAddress");
				((DataCollection<string, object>)(object)val.get_Parameters()).set_Item("EmailAddresses", (object)activityParty.AddressUsed);
				((DataCollection<string, object>)(object)val.get_Parameters()).set_Item("ObjectTypeCodes", (object)new int[0]);
				object obj = ((DataCollection<string, object>)(object)context.OrganizationService.Execute(val).get_Results()).get_Item("Entities");
				EntityCollection val2 = obj as EntityCollection;
				if (val2 != null && ((Collection<Entity>)(object)val2.get_Entities()).Count > 0)
				{
					DataCollection<Entity> entities = val2.get_Entities();
					Entity val3 = ((Collection<Entity>)(object)entities)[0];
					if (((Collection<Entity>)(object)entities).Count > 1)
					{
						val3 = GetOrderedResolvedData(entities, objectTypes, context).Values[0];
					}
					party.set_Item("partyid", (object)val3.ToEntityReference());
					party.set_Item("partyobjecttypecode", (object)GetObjectTypeCodeFromEntity(val3, context));
				}
			}
		}

		private SortedList<int, Entity> GetOrderedResolvedData(DataCollection<Entity> unsortedList, int[] objectTypes, IPluginContext context)
		{
			SortedList<int, Entity> sortedList = new SortedList<int, Entity>();
			List<int> list = objectTypes.ToList();
			int num = list.Max();
			foreach (Entity item in (Collection<Entity>)(object)unsortedList)
			{
				int objectTypeCodeFromEntity = GetObjectTypeCodeFromEntity(item, context);
				int i = list.IndexOf(objectTypeCodeFromEntity) * 100;
				if (i < 0)
				{
					i = ((objectTypeCodeFromEntity == 0) ? (num + 100) : (objectTypeCodeFromEntity * 100 + num * 100));
				}
				for (; sortedList.ContainsKey(i); i++)
				{
				}
				sortedList.Add(i, item);
			}
			return sortedList;
		}

		private int GetObjectTypeCodeFromEntity(Entity entity, IPluginContext context)
		{
			Dictionary<string, int> dictionary = _objectTypeCodesCache ?? (_objectTypeCodesCache = new Dictionary<string, int>());
			if (dictionary.ContainsKey(entity.get_LogicalName()))
			{
				return dictionary[entity.get_LogicalName()];
			}
			EntityMetadata entityMetaData = ObjectServiceUtility.GetEntityMetaData(context, entity.get_LogicalName(), (EntityFilters)2);
			int value = entityMetaData.get_ObjectTypeCode().Value;
			dictionary.Add(entity.get_LogicalName(), value);
			return value;
		}
	}
}
