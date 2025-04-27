using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public static class ActionCardPluginService
	{
		public static void ExecutePlugin(IActionCardService service, ActionCardFeatureContext actionCardFeatureContext, IPluginContext context)
		{
			if (actionCardFeatureContext.IsActionCardFeatureEnabled(context))
			{
				bool isNonActivityCardEnabled = actionCardFeatureContext.IsActionCardEnabled(context);
				Entity targetFromInputParameters = context.GetTargetFromInputParameters<Entity>();
				if (((IExecutionContext)context.PluginExecutionContext).get_MessageName().Equals("Create"))
				{
					service.CreateActionCard(targetFromInputParameters, context, isNonActivityCardEnabled);
				}
				else if (((IExecutionContext)context.PluginExecutionContext).get_MessageName().Equals("Update"))
				{
					service.UpdateActionCard(targetFromInputParameters, context, isNonActivityCardEnabled);
				}
			}
		}
	}
}
