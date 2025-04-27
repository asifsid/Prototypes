using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Kernel.Contracts.Security;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class SecurityService : ISecurityService
	{
		private static string ClassName = typeof(SecurityService).FullName;

		public void CheckAccess(EntityReference entityReference, EntityAction entityAction, IPluginContext context)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			context.AuthorizationService.AssertRecordAccess(entityReference, entityAction);
		}

		public void CheckAccess(EntityReference userReference, EntityReference entityReference, EntityAction entityAction, IPluginContext context)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			context.AuthorizationService.AssertRecordAccess(entityReference, entityAction, userReference);
		}

		public void MultipleRecordAccessCheck(IPluginContext context, string entityName, List<Guid> recordIds, EntityAction recordAction)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			context.AuthorizationService.AssertMultipleRecordAccess(entityName, (IReadOnlyCollection<Guid>)recordIds, recordAction);
		}

		public void CheckPrivilege(Guid userId, string entityTypeLogicalName, PrivilegeType privilegeType, IPluginContext context)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			EntityMetadata entityMetaData = ObjectServiceUtility.GetEntityMetaData(context, entityTypeLogicalName, (EntityFilters)4);
			SecurityPrivilegeMetadata val = entityMetaData.get_Privileges().FirstOrDefault((SecurityPrivilegeMetadata p) => p.get_PrivilegeType() == privilegeType);
			Exceptions.ThrowIfNull(val, "privilege");
			context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CheckPrivilege", $"Fetched privilages for UserId: {userId}, Entity: {entityTypeLogicalName}");
			if (!CheckPrivilege(userId, val.get_PrivilegeId(), context))
			{
				ThrowCrmSecurityException(userId, val, context);
			}
		}

		public void CheckPrivilege(Guid userId, string entityTypeLogicalName, string privilegeName, IPluginContext context)
		{
			EntityMetadata entityMetaData = ObjectServiceUtility.GetEntityMetaData(context, entityTypeLogicalName, (EntityFilters)4);
			SecurityPrivilegeMetadata val = entityMetaData.get_Privileges().FirstOrDefault((SecurityPrivilegeMetadata p) => p.get_Name().Equals(privilegeName, StringComparison.OrdinalIgnoreCase));
			Exceptions.ThrowIfNull(val, "privilege");
			if (!CheckPrivilege(userId, val.get_PrivilegeId(), context))
			{
				ThrowCrmSecurityException(userId, val, context);
			}
		}

		public bool CheckPrivilege(Guid userId, Guid privilegeId, IPluginContext context)
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Expected O, but got Unknown
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			Exceptions.ThrowIfGuidEmpty(userId, "systemuserid");
			Exceptions.ThrowIfGuidEmpty(privilegeId, "privilegeid");
			Exceptions.ThrowIfNull(context, "Plugin Context");
			context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CheckPrivilege", $"Checking privilages for UserId: {userId}, PrivilageId: {privilegeId}");
			QueryExpression val = new QueryExpression("privilege");
			LinkEntity val2 = new LinkEntity("privilege", "roleprivileges", "privilegeid", "privilegeid", (JoinOperator)0);
			LinkEntity val3 = new LinkEntity("roleprivileges", "role", "roleid", "roleid", (JoinOperator)0);
			LinkEntity val4 = new LinkEntity("role", "systemuserroles", "roleid", "roleid", (JoinOperator)0);
			LinkEntity val5 = new LinkEntity("systemuserroles", "systemuser", "systemuserid", "systemuserid", (JoinOperator)0);
			ConditionExpression val6 = new ConditionExpression("systemuserid", (ConditionOperator)0, (object)userId);
			ConditionExpression val7 = new ConditionExpression("privilegeid", (ConditionOperator)0, (object)privilegeId);
			val5.get_LinkCriteria().AddCondition(val6);
			val2.get_LinkCriteria().AddCondition(val7);
			((Collection<LinkEntity>)(object)val4.get_LinkEntities()).Add(val5);
			((Collection<LinkEntity>)(object)val3.get_LinkEntities()).Add(val4);
			((Collection<LinkEntity>)(object)val2.get_LinkEntities()).Add(val3);
			bool result = false;
			((Collection<LinkEntity>)(object)val.get_LinkEntities()).Add(val2);
			EntityCollection val8 = context.OrganizationService.RetrieveMultiple((QueryBase)(object)val);
			Exceptions.ThrowIfNull(val8, "checkPrivileges");
			if (((Collection<Entity>)(object)val8.get_Entities()).Count > 0)
			{
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CheckPrivilege", $"RetrieveMultiple privilages count: {((Collection<Entity>)(object)val8.get_Entities()).Count}. Setting userHasPrivilege: true", "SecurityServiceUserPrivilagesCount", ((Collection<Entity>)(object)val8.get_Entities()).Count.ToString());
				result = true;
			}
			return result;
		}

		public void CheckPrivilege(string entityTypeLogicalName, EntityAction entityAction, IPluginContext context)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			context.AuthorizationService.AssertEntityActionPrivilege(entityTypeLogicalName, entityAction);
		}

		private static void ThrowCrmSecurityException(Guid userId, SecurityPrivilegeMetadata privilege, IPluginContext context)
		{
			string message = string.Format(CultureInfo.InvariantCulture, "Principal user (Id={0}, type={1}) is missing {2} privilege (Id={3})", userId, 8, privilege.get_Name(), privilege.get_PrivilegeId());
			throw new CrmSecurityException(message, -2147220960);
		}

		public void CheckUserPrivilege(Guid userId, string entityTypeLogicalName, PrivilegeType privilegeType, IPluginContext context)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			try
			{
				EntityMetadata entityMetaData = ObjectServiceUtility.GetEntityMetaData(context, entityTypeLogicalName, (EntityFilters)4);
				SecurityPrivilegeMetadata privilege = entityMetaData.get_Privileges().FirstOrDefault((SecurityPrivilegeMetadata p) => p.get_PrivilegeType() == privilegeType);
				Exceptions.ThrowIfNull(privilege, "privilege");
				Exceptions.ThrowIfGuidEmpty(userId, "systemuserid");
				Exceptions.ThrowIfGuidEmpty(privilege.get_PrivilegeId(), "privilegeid");
				Exceptions.ThrowIfNull(context, "Plugin Context");
				RetrieveUserPrivilegesRequest val = new RetrieveUserPrivilegesRequest();
				val.set_UserId(userId);
				RetrieveUserPrivilegesRequest val2 = val;
				RetrieveUserPrivilegesResponse val3 = (RetrieveUserPrivilegesResponse)context.SystemUserOrganizationService.Execute((OrganizationRequest)(object)val2);
				Exceptions.ThrowIfNull(val3, "retrieveUserPrivilegesResponse");
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CheckPrivilege", $"Retrieved privilages for UserId: {userId}, PrivilageId: {privilege.get_PrivilegeId()}");
				if (!val3.get_RolePrivileges().Any((RolePrivilege x) => x.get_PrivilegeId() == privilege.get_PrivilegeId()))
				{
					ThrowCrmSecurityException(userId, privilege, context);
				}
			}
			catch (Exception ex)
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				object[] args = new string[1] { ex.ToString() };
				context.Trace(string.Format(invariantCulture, "Exception: {0}", args));
				throw;
			}
		}
	}
}
