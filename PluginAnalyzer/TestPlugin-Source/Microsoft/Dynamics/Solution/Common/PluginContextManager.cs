using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class PluginContextManager
	{
		private static readonly ConcurrentDictionary<int, Stack<IPluginContext>> ContextPerThreadId = new ConcurrentDictionary<int, Stack<IPluginContext>>();

		internal static void InitiatingExecution(IPluginContext currentContext)
		{
			int currentThreadId = GetCurrentThreadId();
			if (!ContextPerThreadId.ContainsKey(currentThreadId))
			{
				ContextPerThreadId[currentThreadId] = new Stack<IPluginContext>();
			}
			ContextPerThreadId[currentThreadId].Push(currentContext);
		}

		internal static void FinalizingExecution()
		{
			int currentThreadId = GetCurrentThreadId();
			if (ContextPerThreadId.ContainsKey(currentThreadId))
			{
				Stack<IPluginContext> value = ContextPerThreadId[currentThreadId];
				if (value != null && (value.Count == 0 || value.Count == 1))
				{
					ContextPerThreadId.TryRemove(currentThreadId, out value);
				}
				else
				{
					ContextPerThreadId[currentThreadId].Pop();
				}
			}
		}

		public static IPluginContext GetCurrentContext()
		{
			int currentThreadId = GetCurrentThreadId();
			IPluginContext result = null;
			if (ContextPerThreadId.ContainsKey(currentThreadId))
			{
				result = ContextPerThreadId[currentThreadId].Peek();
			}
			return result;
		}

		private static int GetCurrentThreadId()
		{
			return Thread.CurrentThread.ManagedThreadId;
		}
	}
}
