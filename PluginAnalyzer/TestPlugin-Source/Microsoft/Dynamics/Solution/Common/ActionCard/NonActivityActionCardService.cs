using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Dynamics.Solution.Common.Telemetry;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public class NonActivityActionCardService : ActionCardService
	{
		private const string AttributeNameStateCode = "statecode";

		private const string AttributeNameStatusCode = "statuscode";

		private const string AttributeModifiedOn = "modifiedon";

		public const int DefaultStartFromDay = 6;

		public const int DefaultActiveDaysForACard = 30;

		public const int ActionCardDurationInDays = 90;

		public Guid AssociatedActionCardTypeId { get; private set; }

		public int EntityTypeCode { get; private set; }

		public Func<Entity, string> GetEntitySubject { get; private set; }

		public string CardBodyResourceId { get; private set; }

		public int EntityStateCodeOpen { get; private set; }

		public NonActivityActionCardService(Guid associatedActionCardTypeId, int entityTypeCode, Func<Entity, string> getEntitySubjectFunction, string cardBodyResourceId, int entityStateCodeOpen)
		{
			AssociatedActionCardTypeId = associatedActionCardTypeId;
			EntityTypeCode = entityTypeCode;
			GetEntitySubject = getEntitySubjectFunction;
			CardBodyResourceId = cardBodyResourceId;
			EntityStateCodeOpen = entityStateCodeOpen;
		}

		public override void CreateActionCard(Entity entity, IPluginContext context, bool isNoActivityCardEnabled)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (isNoActivityCardEnabled && (!((DataCollection<string, object>)(object)entity.get_Attributes()).Contains("statecode") || (((DataCollection<string, object>)(object)entity.get_Attributes()).Contains("statecode") && ((OptionSetValue)entity.get_Item("statecode")).get_Value() == EntityStateCodeOpen)))
			{
				CreateNoActivityActionCard(entity, context);
			}
		}

		public override void UpdateActionCard(Entity entity, IPluginContext context, bool isNoActivityCardEnabled)
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			if (!((DataCollection<string, object>)(object)entity.get_Attributes()).Contains("statecode") && !((DataCollection<string, object>)(object)entity.get_Attributes()).Contains("statuscode"))
			{
				return;
			}
			try
			{
				EntityCollection val = ActionCardUtility.RetrieveRelatedActiveActionCardRecords(entity.get_Id(), context);
				if (val != null)
				{
					foreach (Entity item in (Collection<Entity>)(object)val.get_Entities())
					{
						if (((DataCollection<string, object>)(object)entity.get_Attributes()).Contains("statecode") && ((OptionSetValue)entity.get_Item("statecode")).get_Value() != EntityStateCodeOpen)
						{
							Microsoft.Dynamics.Solution.Common.Proxies.ActionCard actionCard = ActionCardUtility.CreateNewActionCardEntityInstance(item.get_Id());
							actionCard.SetActionCardState(ActionCardActionCardState.Completed);
							context.SystemUserOrganizationService.Update((Entity)(object)actionCard);
							ActionCardEventSource.Instance.ActionCardInformational(ActionCardEvent.SkippedActionCardGeneration.ToString(), ((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), "CreateNoActivityActionCard", string.Format("Skipped Action Card Generation. Card Type {0}: State Code {1}", AssociatedActionCardTypeId, ((OptionSetValue)entity.get_Item("statecode")).get_Value()), string.Empty, ((IExecutionContext)context.PluginExecutionContext).get_UserId());
						}
					}
				}
				else
				{
					Entity val2 = ActionCardUtility.RetrieveEntityRecord(entity.ToEntityReference(), context);
					if (val2 != null && ((DataCollection<string, object>)(object)entity.get_Attributes()).Contains("statecode") && ((OptionSetValue)entity.get_Item("statecode")).get_Value() == EntityStateCodeOpen)
					{
						CreateActionCard(val2, context, isNoActivityCardEnabled);
					}
				}
				ActionCardEventSource.Instance.ActionCardInformational(ActionCardEvent.UpdateCard.ToString(), ((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), "UpdateActionCard", $"Card Updated. Type: {AssociatedActionCardTypeId}", string.Empty, ((IExecutionContext)context.PluginExecutionContext).get_UserId());
			}
			catch (Exception ex)
			{
				ActionCardEventSource.Instance.ActionCardError(((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), "UpdateActionCard", 0, $"Update of NoActivity Card failed for Entity : {EntityTypeCode}. Error Details: {ex.ToString()}", string.Empty, ((IExecutionContext)context.PluginExecutionContext).get_UserId());
			}
		}

		private void CreateNoActivityActionCard(Entity entity, IPluginContext context)
		{
			int actionCardOptionValueFromUserSettings = ActionCardUtility.GetActionCardOptionValueFromUserSettings(((IExecutionContext)context.PluginExecutionContext).get_UserId(), AssociatedActionCardTypeId, 30, context);
			DateTime dateTime = DateTime.UtcNow.Date.AddDays(actionCardOptionValueFromUserSettings + 1);
			ActionCardFeatureContext actionCardFeatureContext = new ActionCardFeatureContext();
			bool flag = actionCardFeatureContext.IsActionCardGenerationAsyncFeatureEnabled(context);
			if (!flag || (flag && dateTime < DateTime.UtcNow.AddDays(6.0)))
			{
				try
				{
					DateTime endDate = dateTime.AddDays(90.0);
					string entitySubject = GetEntitySubject(entity);
					Microsoft.Dynamics.Solution.Common.Proxies.ActionCard actionCard = ActionCardUtility.InitActionCardFromEntity(entity, context);
					actionCard.SetCardTypeInformation(AssociatedActionCardTypeId, visibility: false, ActionCardPriority.Medium);
					actionCard.SetActionCardDateRange(dateTime, endDate);
					actionCard.SetTokenIformation(ActionCardTokenUtility.GetReferenceTokenForNonActivityCard(entity, EntityTypeCode, entitySubject, CardBodyResourceId));
					context.SystemUserOrganizationService.Create((Entity)(object)actionCard);
					ActionCardEventSource.Instance.ActionCardInformational(ActionCardEvent.CreateCard.ToString(), ((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), "CreateNoActivityActionCard", $"Card Created. Type: {AssociatedActionCardTypeId}, AsyncFCB value {flag} and startDate is {dateTime}", string.Empty, ((IExecutionContext)context.PluginExecutionContext).get_UserId());
				}
				catch (Exception ex)
				{
					ActionCardEventSource.Instance.ActionCardError(((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), "CreateNoActivityActionCard", 0, $"Create NoActivity Card failed for Entity : {EntityTypeCode}. Error Details: {ex.ToString()}", string.Empty, ((IExecutionContext)context.PluginExecutionContext).get_UserId());
				}
			}
			else
			{
				ActionCardEventSource.Instance.ActionCardInformational(ActionCardEvent.SkippedActionCardGeneration.ToString(), ((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), "CreateNoActivityActionCard", $"Skipped Action Card Generation. Card Type {AssociatedActionCardTypeId}", string.Empty, ((IExecutionContext)context.PluginExecutionContext).get_UserId());
			}
		}
	}
}
