using System;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public interface IAzureServiceConnectionHelper
	{
		EntityReference GetAzureServiceConnectionIdByType(IPluginContext context);

		void TestConnection(Guid azureServiceConnectionId, IPluginContext context);

		bool IsConnectionActive(Guid connectionId, IPluginContext context);
	}
}
