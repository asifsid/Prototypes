using System.Runtime.InteropServices;
using Microsoft.Solution.Common.ObjectModel;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class PluginContextExtensions
	{
		public static SetVariableInContextModifier SetSkipPlugins(this IPluginContext context, string messageName, string entityName, int? stage = null)
		{
			string key = SharedVariableNamesProvider.Get(messageName, entityName, stage);
			return SetVariableInContextModifier.GetForPipelineControlVariable(context, key);
		}

		public static SetVariableInContextModifier SetSkipUpdatePlugins(this IPluginContext context, string entityName, int? stage = null)
		{
			return context.SetSkipPlugins("Update", entityName, stage);
		}

		public static SetVariableInContextModifier SetSkipCreatePlugins(this IPluginContext context, string entityName, int? stage = null)
		{
			return context.SetSkipPlugins("Create", entityName, stage);
		}

		public static SetVariableInContextModifier SetSkipDeletePlugins(this IPluginContext context, string entityName, int? stage = null)
		{
			return context.SetSkipPlugins("Delete", entityName, stage);
		}

		public static SetVariableInContextModifier SetSkipRetrievePlugins(this IPluginContext context, string entityName, int? stage = null)
		{
			return context.SetSkipPlugins("Retrieve", entityName, stage);
		}

		public static SetVariableInContextModifier SetSkipRetrieveMultiplePlugins(this IPluginContext context, string entityName, int? stage = null)
		{
			return context.SetSkipPlugins("RetrieveMultiple", entityName, stage);
		}
	}
}
