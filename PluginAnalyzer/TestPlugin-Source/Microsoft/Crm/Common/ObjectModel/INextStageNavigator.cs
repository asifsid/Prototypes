using System;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Crm.Common.ObjectModel
{
	[ComVisible(true)]
	public interface INextStageNavigator
	{
		void NavigateToNextEntity(Guid quoteProcessId, Guid processInstanceId, Entity quote, object p, Entity targetSalesOrder, bool v, IPluginContext context);
	}
}
