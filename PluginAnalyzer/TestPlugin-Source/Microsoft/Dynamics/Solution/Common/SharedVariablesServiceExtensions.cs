using System.Runtime.InteropServices;
using Microsoft.Xrm.Kernel.Contracts;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class SharedVariablesServiceExtensions
	{
		public static T GetOrDefault<T>(this ISharedVariablesService sharedVariablesService, string key, Scope scope, T defaultValue = default(T))
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			object obj = default(object);
			if (sharedVariablesService.TryGet(key, scope, ref obj))
			{
				return (T)obj;
			}
			return defaultValue;
		}

		public static T GetOrDefault<T>(this ISharedVariablesService sharedVariablesService, string key, T defaultValue = default(T))
		{
			object obj = default(object);
			if (sharedVariablesService.TryGet(key, ref obj))
			{
				return (T)obj;
			}
			return defaultValue;
		}
	}
}
