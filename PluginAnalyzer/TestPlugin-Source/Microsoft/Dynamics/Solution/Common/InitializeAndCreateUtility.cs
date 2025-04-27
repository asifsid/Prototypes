using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Solution.Common.ObjectModel;
using Microsoft.Xrm.Kernel.Contracts;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class InitializeAndCreateUtility
	{
		private static string ClassName = typeof(InitializeAndCreateUtility).FullName;

		public static List<EntityReference> InitializeAndCreate(Dictionary<EntityReference, Entity> sourceAndTargetDictionary, IPluginContext pluginContext, bool copyId = false)
		{
			return pluginContext.PluginTelemetry.OperationEventLogger.Execute((XrmTelemetryActivityType)(object)XrmTelemetrySingletonActivityType<BulkCreateActivityType>.get_Instance(), delegate
			{
				XrmTelemetryContext.AddCustomProperties(new Dictionary<string, string>
				{
					{
						"BulkCreateEntities",
						sourceAndTargetDictionary?.Count.ToString()
					},
					{
						"CopyIdBooleanSet",
						copyId.ToString()
					}
				});
				return InitializeAndCreateInternal(sourceAndTargetDictionary, pluginContext, copyId);
			});
		}

		private static List<EntityReference> InitializeAndCreateInternal(Dictionary<EntityReference, Entity> sourceAndTargetDictionary, IPluginContext pluginContext, bool copyId = false)
		{
			Exceptions.ThrowIfNull(sourceAndTargetDictionary, "sourceAndTargetDictionary");
			Exceptions.ThrowIfNull(pluginContext, "pluginContext");
			List<EntityReference> list = new List<EntityReference>();
			foreach (EntityReference key in sourceAndTargetDictionary.Keys)
			{
				Entity targetInput = sourceAndTargetDictionary[key];
				list.Add(InitializeAndCreate(key, targetInput, pluginContext, copyId));
			}
			return list;
		}

		public static Entity Initialize(EntityReference srcMoniker, Entity targetInput, IPluginContext pluginContext)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			InitializeFromRequest val = new InitializeFromRequest();
			val.set_EntityMoniker(srcMoniker);
			val.set_TargetEntityName(targetInput.get_LogicalName());
			InitializeFromRequest val2 = val;
			InitializeFromResponse val3 = (InitializeFromResponse)pluginContext.OrganizationService.Execute((OrganizationRequest)(object)val2);
			return val3.get_Entity().Merge(targetInput);
		}

		public static EntityReference InitializeAndCreate(EntityReference sourceMoniker, Entity targetInput, IPluginContext context, bool copyId = false)
		{
			Entity val = Initialize(sourceMoniker, targetInput, context);
			if (copyId)
			{
				val.set_Id(targetInput.get_Id());
			}
			using (new SetVariableInContextModifier(context, "DeepCopySourceId", sourceMoniker.get_Id(), (Scope)2, (Lifetime)1))
			{
				return CreateWithoutFLSChecks(val, context);
			}
		}

		public static EntityReference CreateWithoutFLSChecks(Entity entity, IPluginContext context)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			Guid guid = context.FieldLevelSecurityBehaviorService.CreateWithoutFieldLevelSecurityCheck(entity);
			return new EntityReference(entity.get_LogicalName(), guid);
		}
	}
}
