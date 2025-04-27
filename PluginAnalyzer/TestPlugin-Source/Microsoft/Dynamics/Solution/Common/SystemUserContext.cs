using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class SystemUserContext : IDisposable
	{
		private readonly IPluginContext context;

		public SystemUserContext(IPluginContext context)
		{
			this.context = context;
			this.context.ReturnSystemUserOrganizationService = true;
		}

		public void Dispose()
		{
			context.ReturnSystemUserOrganizationService = false;
		}
	}
}
