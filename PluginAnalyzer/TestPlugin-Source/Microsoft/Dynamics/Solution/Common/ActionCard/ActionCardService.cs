using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public abstract class ActionCardService : IActionCardService
	{
		public abstract void UpdateActionCard(Entity entity, IPluginContext context, bool isNonActivityCardEnabled);

		public abstract void CreateActionCard(Entity entity, IPluginContext context, bool isNonActivityCardEnabled);
	}
}
