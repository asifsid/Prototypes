using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Interfaces;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common.Services
{
	[ComVisible(true)]
	public class DefaultInitializeAndCreateUtility : IInitializeAndCreateUtility
	{
		public EntityReference InitializeAndCreate(EntityReference sourceMoniker, Entity targetInput, IPluginContext context, bool copyId = false)
		{
			return InitializeAndCreateUtility.InitializeAndCreate(sourceMoniker, targetInput, context, copyId);
		}
	}
}
