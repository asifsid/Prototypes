using System.Runtime.InteropServices;
using Microsoft.Xrm.Kernel.Contracts;

namespace Microsoft.Dynamics.Solution.Common.PlatformPluginHooks
{
	[ComVisible(true)]
	public static class MultiCurrencyPlugin
	{
		private const string ObjectSpecificColumns = "MultiCurrencyPlugin_ObjectSpecificColumns";

		private const string IsCurrencyRequiredByBusinessLogic = "MultiCurrencyPlugin_IsCurrencyRequered";

		public static void SetObjectSpecificColumns(IPluginContext context, params string[] columns)
		{
			context.SharedVariablesService.Set("MultiCurrencyPlugin_ObjectSpecificColumns", (object)columns, (Scope)1);
		}

		public static void SetCurrencyRequiredByBusinessLogic(IPluginContext context, bool isCurrencyRequired)
		{
			context.SharedVariablesService.Set("MultiCurrencyPlugin_IsCurrencyRequered", (object)isCurrencyRequired, (Scope)1);
		}
	}
}
