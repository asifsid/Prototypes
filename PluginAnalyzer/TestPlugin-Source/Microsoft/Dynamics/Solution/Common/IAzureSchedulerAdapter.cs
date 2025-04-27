using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public interface IAzureSchedulerAdapter
	{
		string CreateJobCollection(string collectionName);

		string GetJobCollection(string collectionName);

		string DeleteJobCollection(string collectionName);

		IAzureSchedulerResponse CreateJob(string jobName, string message, string collectionName);

		string UpdateJob(string jobName, string message, string collectionName);

		string GetJob(string jobName, string collectionName);

		string DeleteJob(string jobName, string collectionName);

		string GetJSONForQueueJobBody(JobBodyQueueData jobBody);
	}
}
