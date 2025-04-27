using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class SharedVariableNamesProvider
	{
		private const string VariableNameTemplate = "SkipPlugins_{0}_{1}_{2}";

		private const string AllPluginExecutionsStages = "AllStages";

		public static string Get(string messageName, string entityName, int? stage = null)
		{
			string arg = (stage.HasValue ? stage.Value.ToString() : "AllStages");
			return $"SkipPlugins_{messageName}_{entityName}_{arg}";
		}

		public static string Get(IPluginContext context)
		{
			string messageName = ((IExecutionContext)context.PluginExecutionContext).get_MessageName();
			string primaryEntityName = ((IExecutionContext)context.PluginExecutionContext).get_PrimaryEntityName();
			int stage = context.PluginExecutionContext.get_Stage();
			return Get(messageName, primaryEntityName, stage);
		}
	}
}
