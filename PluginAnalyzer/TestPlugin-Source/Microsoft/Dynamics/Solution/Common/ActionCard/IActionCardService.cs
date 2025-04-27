using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public interface IActionCardService
	{
		void CreateActionCard(Entity entity, IPluginContext context, bool isNonActivityCardEnabled);

		void UpdateActionCard(Entity entity, IPluginContext context, bool isNonActivityCardEnabled);
	}
}
