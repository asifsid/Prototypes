using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.Interfaces
{
	[ComVisible(true)]
	public interface IInitializeAndCreateUtility
	{
		EntityReference InitializeAndCreate(EntityReference sourceMoniker, Entity targetInput, IPluginContext context, bool copyId = false);
	}
}
