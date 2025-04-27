using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class LocalPluginContext : PluginContextBase
	{
		public LocalPluginContext(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
