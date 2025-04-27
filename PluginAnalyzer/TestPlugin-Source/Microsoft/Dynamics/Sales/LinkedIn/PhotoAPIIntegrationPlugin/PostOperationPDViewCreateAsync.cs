using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Dynamics.LinkedInIntegration.Common.Localization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin
{
	[ComVisible(true)]
	public class PostOperationPDViewCreateAsync : IPlugin
	{
		private Guid linkedinCardTypeId = new Guid("d6570f98-9520-46b5-b9e3-e73f675a99de");

		private const int cardTypeNumber = 65536;

		public void Execute(IServiceProvider serviceProvider)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
			IOrganizationServiceFactory val = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
			IOrganizationService service = val.CreateOrganizationService((Guid?)null);
			ITracingService val2 = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
			Entity targetFromInputParameters = GetTargetFromInputParameters<Entity>(context);
			if (targetFromInputParameters == null)
			{
				throw new Exception("Target not found in the input parameters of context");
			}
			if (!targetFromInputParameters.Contains("regardingobjectid"))
			{
				val2.Trace("regardingObjectId is not present in pointDrivePresentationView entity", Array.Empty<object>());
				return;
			}
			EntityReference val3 = (EntityReference)targetFromInputParameters.get_Item("regardingobjectid");
			if (CanActionCardBeCreated(val3))
			{
				Entity val4 = RetrieveEntity(service, val3);
				if (val4 == null)
				{
					throw new Exception($"Entity {val3.get_LogicalName()} with Id {val3.get_Id()} is not present in database");
				}
				CreateActionCard(service, targetFromInputParameters, val4);
			}
		}

		private T GetTargetFromInputParameters<T>(IPluginExecutionContext context)
		{
			if (((DataCollection<string, object>)(object)((IExecutionContext)context).get_InputParameters()).Contains("Target"))
			{
				return (T)((DataCollection<string, object>)(object)((IExecutionContext)context).get_InputParameters()).get_Item("Target");
			}
			return default(T);
		}

		private bool CanActionCardBeCreated(EntityReference regardingobject)
		{
			string[] array = new string[2] { "lead", "contact" };
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text == regardingobject.get_LogicalName())
				{
					return true;
				}
			}
			return false;
		}

		private Entity RetrieveEntity(IOrganizationService service, EntityReference entityReference)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			return service.Retrieve(entityReference.get_LogicalName(), entityReference.get_Id(), new ColumnSet(new string[1] { "fullname" }));
		}

		private void CreateActionCard(IOrganizationService service, Entity pointDriveViewEntity, Entity regardingObjectEntity)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			string displayName = "";
			if (regardingObjectEntity.Contains("fullname"))
			{
				displayName = (string)regardingObjectEntity.get_Item("fullname");
			}
			if (!pointDriveViewEntity.Contains("ownerid"))
			{
				throw new Exception("OwnerId not found in pointDriveViewEntity");
			}
			EntityReference val = (EntityReference)pointDriveViewEntity.get_Item("ownerid");
			Entity userSetting = RetrieveUserSettings(service, val.get_Id());
			int userTimeZoneCode = GetUserTimeZoneCode(userSetting);
			string actionCardTitle = GetActionCardTitle(displayName);
			if (!pointDriveViewEntity.Contains("actualstart"))
			{
				throw new Exception("Actual Start not found in pointDriveViewEntity");
			}
			DateTime utcTime = ((DateTime)pointDriveViewEntity.get_Item("actualstart")).ToUniversalTime();
			string displayTime = RetrieveLocalTimeFromUTCTime(service, utcTime, userTimeZoneCode).ToShortTimeString();
			if (!pointDriveViewEntity.Contains("subject"))
			{
				throw new Exception("Subject not found in pointDriveViewEntity");
			}
			string subject = (string)pointDriveViewEntity.get_Item("subject");
			string actionCardDescription = GetActionCardDescription(subject, displayTime);
			EntityReference regardingObject = regardingObjectEntity.ToEntityReference();
			Entity pointDriveViewedActionCard = GetPointDriveViewedActionCard(actionCardDescription, pointDriveViewEntity, regardingObject, actionCardTitle);
			service.Create(pointDriveViewedActionCard);
		}

		private DateTime RetrieveLocalTimeFromUTCTime(IOrganizationService service, DateTime utcTime, int timeZone)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			LocalTimeFromUtcTimeRequest val = new LocalTimeFromUtcTimeRequest();
			val.set_TimeZoneCode(timeZone);
			val.set_UtcTime(utcTime);
			LocalTimeFromUtcTimeRequest val2 = val;
			LocalTimeFromUtcTimeResponse val3 = (LocalTimeFromUtcTimeResponse)service.Execute((OrganizationRequest)(object)val2);
			return val3.get_LocalTime();
		}

		private int GetUserTimeZoneCode(Entity userSetting)
		{
			if (userSetting == null)
			{
				throw new Exception("user setting should not be null");
			}
			if (!userSetting.Contains("timezonecode"))
			{
				throw new Exception($"No timezone code found for the user");
			}
			return (int)userSetting.get_Item("timezonecode");
		}

		private Entity RetrieveUserSettings(IOrganizationService service, Guid userId)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_004f: Expected O, but got Unknown
			QueryExpression val = new QueryExpression("usersettings");
			val.set_ColumnSet(new ColumnSet(new string[1] { "timezonecode" }));
			FilterExpression val2 = new FilterExpression();
			((Collection<ConditionExpression>)(object)val2.get_Conditions()).Add(new ConditionExpression("systemuserid", (ConditionOperator)0, (object)userId));
			val.set_Criteria(val2);
			DataCollection<Entity> entities = service.RetrieveMultiple((QueryBase)(object)val).get_Entities();
			if (((Collection<Entity>)(object)entities).Count == 0)
			{
				throw new Exception($"For user {userId}, entity was not found in usersettings");
			}
			return ((Collection<Entity>)(object)entities)[0];
		}

		private string GetActionCardDescription(string subject, string displayTime)
		{
			return string.Format(Labels.PDLinkedinCardDescription, subject, displayTime);
		}

		private string GetActionCardTitle(string displayName)
		{
			return string.Format(Labels.PDLinkedinCardTitle, displayName);
		}

		private Entity GetPointDriveViewedActionCard(string description, Entity pointDriveViewEntity, EntityReference regardingObject, string title)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			Entity val = new Entity("actioncard");
			val.set_Item("cardtype", (object)65536);
			val.set_Item("cardtypeid", (object)new EntityReference("cardtype", linkedinCardTypeId));
			val.set_Item("description", (object)description);
			val.set_Item("expirydate", (object)DateTime.UtcNow.AddDays(7.0));
			val.set_Item("regardingobjectid", (object)regardingObject);
			val.set_Item("recordid", (object)pointDriveViewEntity.ToEntityReference());
			val.set_Item("startdate", (object)DateTime.UtcNow.AddDays(-1.0));
			val.set_Item("title", (object)title);
			val.set_Item("ownerid", pointDriveViewEntity.get_Item("ownerid"));
			val.set_Item("priority", (object)400);
			val.set_Item("visibility", (object)true);
			return val;
		}
	}
}
