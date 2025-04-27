using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common.Services
{
	[ComVisible(true)]
	public class SharePointProvisioningHelperService
	{
		private static string ClassName = typeof(SharePointProvisioningHelperService).FullName;

		public static void CopyDocumentLocations(Guid sourceEntityId, string sourceEntityLogicalName, Guid targetEntityId, string targetEntityLogicalName, IPluginContext pluginContext)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Expected O, but got Unknown
			RetrieveEntityRequest val = new RetrieveEntityRequest();
			val.set_EntityFilters((EntityFilters)1);
			val.set_LogicalName(targetEntityLogicalName);
			RetrieveEntityRequest val2 = val;
			RetrieveEntityResponse val3 = (RetrieveEntityResponse)pluginContext.SystemUserOrganizationService.Execute((OrganizationRequest)(object)val2);
			EntityMetadata entityMetadata = val3.get_EntityMetadata();
			if (entityMetadata.get_IsDocumentManagementEnabled() == false)
			{
				pluginContext.PluginTelemetry.Logger.LogEvent(EventType.Warning, ClassName, "CopyDocumentLocations", "DocumentManagement is disabled for this entity. SourceEntityLogicalName: " + sourceEntityLogicalName, "IsDocumentManagementEnabled", "false");
				return;
			}
			string[] array = new string[5] { "sharepointdocumentlocationid", "createdon", "createdby", "modifiedby", "modifiedon" };
			QueryExpression val4 = new QueryExpression("sharepointdocumentlocation");
			val4.set_ColumnSet(new ColumnSet(true));
			val4.get_Criteria().AddCondition("regardingobjectid", (ConditionOperator)0, new object[1] { sourceEntityId });
			EntityCollection val5 = pluginContext.OrganizationService.RetrieveMultiple((QueryBase)(object)val4);
			pluginContext.PluginTelemetry.Logger.LogEvent(EventType.Warning, ClassName, "CopyDocumentLocations", $"Creating document locations for TargetEntityLogicalName: {targetEntityLogicalName}, TargetEntityId: {targetEntityId}");
			foreach (Entity item in (Collection<Entity>)(object)val5.get_Entities())
			{
				Entity val6 = item.Clone();
				val6.set_Item("regardingobjectid", (object)new EntityReference(targetEntityLogicalName, targetEntityId));
				for (int i = 0; i < array.Length; i++)
				{
					val6.ClearAttribute(array[i]);
				}
				pluginContext.OrganizationService.Create(val6);
			}
			pluginContext.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "CopyDocumentLocations", $"Created DocumentLocations count: {((Collection<Entity>)(object)val5.get_Entities()).Count}", new Dictionary<string, string>
			{
				{ "SPPSIsDocumentManagementEnabled", "true" },
				{ "SPPSSourceEntityLogicalName", sourceEntityLogicalName },
				{ "SPPSTargetEntityLogicalName", sourceEntityLogicalName },
				{
					"SPPSSourceEntityId",
					sourceEntityId.ToString()
				},
				{
					"SPPSTargetEntityId",
					targetEntityId.ToString()
				},
				{
					"SPPSDocumentLocationsCount",
					((Collection<Entity>)(object)val5.get_Entities()).Count.ToString()
				}
			});
		}
	}
}
