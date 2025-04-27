using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.CRM.Common;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Telemetry;

namespace Microsoft.Dynamics.Solution.Common.AddressComputation
{
	[ComVisible(true)]
	public class AddressComputationPluginBase : PluginBase
	{
		private static string ClassName = typeof(AddressComputationPluginBase).FullName;

		public AddressComputationPluginBase(Type pluginType)
			: base(pluginType)
		{
		}

		protected sealed override void ExecuteCrmPlugin(LocalPluginContext context)
		{
			string messageName = ((IExecutionContext)context.PluginExecutionContext).get_MessageName();
			XrmTelemetryContext.AddCustomProperty("AddressComputationSdkMessage", messageName);
			if (messageName.Equals("Create"))
			{
				Entity targetFromInputParameters = ((PluginContextBase)context).GetTargetFromInputParameters<Entity>();
				Exceptions.ThrowIfNull(targetFromInputParameters, Labels.ContextTargetNotPresentOrNotEntityType);
				InternalAddress.InitializeAddressForCreate(targetFromInputParameters, context);
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ExecuteCrmPlugin", $"SdkMessage: Create, TargetEntityId: {targetFromInputParameters.get_Id()}", "AddressComputationTargetEntity", targetFromInputParameters.get_LogicalName());
			}
			else if (messageName.Equals("Update"))
			{
				Entity targetFromInputParameters = ((PluginContextBase)context).GetTargetFromInputParameters<Entity>();
				Exceptions.ThrowIfNull(targetFromInputParameters, Labels.ContextTargetNotPresentOrNotEntityType);
				InternalAddress.InitializeAddressForUpdate(targetFromInputParameters, context);
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ExecuteCrmPlugin", $"SdkMessage: Update, TargetEntityId: {targetFromInputParameters.get_Id()}", "AddressComputationTargetEntity", targetFromInputParameters.get_LogicalName());
			}
			else if (messageName.Equals("Retrieve"))
			{
				Entity targetFromInputParameters = context.GetOutputParameter<Entity>("BusinessEntity");
				Exceptions.ThrowIfNull(targetFromInputParameters, string.Format(Labels.ContextOutputParameterMissingOrNull, "BusinessEntity"));
				InternalAddress.InitializeAddressForRetrieve(targetFromInputParameters, context);
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ExecuteCrmPlugin", $"SdkMessage: Retrieve, TargetEntityId: {targetFromInputParameters.get_Id()}", "AddressComputationTargetEntity", targetFromInputParameters.get_LogicalName());
			}
			else
			{
				EntityCollection outputParameter = context.GetOutputParameter<EntityCollection>("BusinessEntityCollection");
				Exceptions.ThrowIfNull(outputParameter, string.Format(Labels.ContextOutputParameterMissingOrNull, "BusinessEntityCollection"));
				InternalAddress.InitializeAddressForRetrieveMultiple(outputParameter, context);
				context.PluginTelemetry.Logger.LogEvent(EventType.Info, ClassName, "ExecuteCrmPlugin", "SdkMessage: " + messageName + ", TargetEntity: " + outputParameter.get_EntityName(), new Dictionary<string, string>
				{
					{
						"CountOfAddressComputationBusinessEntityCollection",
						((Collection<Entity>)(object)outputParameter.get_Entities()).Count.ToString()
					},
					{
						"AddressComputationTargetEntity",
						outputParameter.get_EntityName()
					}
				});
			}
		}
	}
}
