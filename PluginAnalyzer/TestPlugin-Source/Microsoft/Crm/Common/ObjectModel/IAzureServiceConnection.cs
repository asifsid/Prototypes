using System;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common;

namespace Microsoft.Crm.Common.ObjectModel
{
	[ComVisible(true)]
	public interface IAzureServiceConnection
	{
		void SetModelState(int newState, int newStatusCode, IPluginContext context, Guid azureServiceConnectionId);

		void TestConnection(IPluginContext context);

		void ValidateAzureMachineLearningFeaturesEnabled(IPluginContext context);
	}
}
