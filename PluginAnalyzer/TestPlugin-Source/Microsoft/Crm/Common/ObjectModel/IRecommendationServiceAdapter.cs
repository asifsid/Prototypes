using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Proxies;

namespace Microsoft.Crm.Common.ObjectModel
{
	[ComVisible(true)]
	public interface IRecommendationServiceAdapter
	{
		void TestConnection();

		string CreateModel(string modelName);

		void DeleteModel(string modelId);

		void DeleteBuild(string modelId, string buildId);

		Collection<RecommendedItem> GetRecommendations(string modelId, string[] itemIds, string numberOfResult, string buildId);

		Collection<RecommendedItem> GetRecommendations(string modelId, string[] itemIds, string numberOfResult, string buildId, int maxExecutionTimeInMilliseconds);

		bool ModelExists(string modelId);
	}
}
