using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public enum CrmMlSyncEvent
	{
		ModelDeactivated,
		TextAnalyticsMaxDocumentSizeReached,
		MaxTopicModelsReached,
		AMLTAGetKeyphrasesCount,
		AMLTAGetKeyphrasesTimeTaken,
		RetrieveKeyPhrasesForKBSearchCount,
		RetrieveKeyPhrasesForKBSearchTimeTaken,
		RetrieveKeyPhrasesForSimilaritySearchCount,
		RetrieveKeyPhrasesForSimilaritySearchTimeTaken,
		GetSimilarRecords,
		TopicAnalysisBuildTest,
		TopicAnalysisBuildOnDemand,
		TopicAnalysisBuildScheduled,
		AMLTAExternalCallFromCRM,
		RecommendationsCorrelationId,
		TopicModelActivateCorrelationId,
		TopicModelBuildCorrelationId,
		TopicModelTestCorrelationId,
		RecommendationResponseTimeout,
		LineItemsCountRetrieved,
		SuggestionsRetrievedAll,
		SuggestionsRetrievedMaxUnique,
		AzureRecommendationsRetrievedTimeTaken,
		AzureRecommendationsRetrieved,
		AzureRecommendationsAddedTimeTaken,
		AzureRecommendationsAdded,
		AzureRecommendationsActiveAdded,
		AzureRecommendationsNotAdded,
		AzureRecommendationsMinRating,
		AzureRecommendationsMinRatingFound,
		AzureRecommendationsMaxRatingFound,
		RecommendationsConflictExistingSource,
		RecommendationsConflictExistingItemRating,
		RecommendationsConflictNewSource,
		RecommendationsConflictNewItemRating,
		RecommendationsOverride,
		ProductSubstitutesAdded,
		ProductSubstitutesAddedTimeTaken,
		ProductPriceLevelsRetrieved
	}
}
