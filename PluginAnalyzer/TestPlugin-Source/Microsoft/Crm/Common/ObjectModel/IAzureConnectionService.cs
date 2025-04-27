using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Crm.Common.ObjectModel
{
	[ComVisible(true)]
	public interface IAzureConnectionService
	{
		EntityReference GetAzureServiceConnectionIdByType(int connectionType, IPluginContext context);
	}
}
