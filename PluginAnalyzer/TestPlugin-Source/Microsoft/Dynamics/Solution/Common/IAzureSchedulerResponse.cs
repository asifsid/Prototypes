using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public interface IAzureSchedulerResponse
	{
		string GetJobName();
	}
}
