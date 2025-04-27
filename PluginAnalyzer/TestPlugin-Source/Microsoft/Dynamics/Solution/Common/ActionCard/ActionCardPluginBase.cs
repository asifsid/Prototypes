using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public abstract class ActionCardPluginBase : PluginBase
	{
		protected ActionCardPluginBase(Type childClassName)
			: base(childClassName)
		{
		}

		protected sealed override void ExecuteCrmPlugin(LocalPluginContext localContext)
		{
			IActionCardService actionCardService = GetActionCardService(localContext);
			ActionCardFeatureContext actionCardFeatureContext = new ActionCardFeatureContext();
			ActionCardPluginService.ExecutePlugin(actionCardService, actionCardFeatureContext, localContext);
		}

		protected abstract IActionCardService GetActionCardService(IPluginContext context);
	}
}
