using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Interfaces;
using Microsoft.Xrm.Kernel.Contracts;
using Microsoft.Xrm.Kernel.Contracts.Cache;
using Microsoft.Xrm.Kernel.Contracts.CommandExecution;
using Microsoft.Xrm.Kernel.Contracts.ExternalIntegration;
using Microsoft.Xrm.Kernel.Contracts.Security;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public interface IPluginContext
	{
		IFeatureContext FeatureContext { get; }

		IEntityStore EntityStoreService { get; }

		IOrganizationService OrganizationService { get; }

		IOrganizationService SystemUserOrganizationService { get; }

		IFieldLevelSecurityBehaviorService FieldLevelSecurityBehaviorService { get; }

		bool IsInClientContext { get; }

		bool ReturnSystemUserOrganizationService { get; set; }

		IPluginExecutionContext PluginExecutionContext { get; }

		ITracingService TracingService { get; }

		IPluginTelemetry PluginTelemetry { get; }

		ICommandExecutionService CommandExecutionService { get; }

		ISequentialGuid SequentialGuid { get; }

		IOfflineDataParametersContainer OfflineData { get; }

		IAuthorizationService AuthorizationService { get; }

		IMergeOperationParameters MergeOperationParameters { get; }

		IAzureProxyFactory AzureProxyFactory { get; }

		IExternalIntegrationSettings ExternalIntegrationSettings { get; }

		ICache<string, EntityMetadata> EntityMetadataCache { get; }

		IOrganizationServiceFactoryRunAs OrganizationServiceFactoryRunAs { get; }

		IOrganizationEndpointService OrganizationEndPointService { get; }

		IDeploymentSettings DeploymentSettings { get; }

		IResourceSchedulingCommandService ResourceSchedulingCommandService { get; }

		ISharedVariablesService SharedVariablesService { get; }

		IDeveloperTestSettings DeveloperTestSettings { get; }

		IAddressFormatter AddressFormatter { get; }

		bool IsInMergeOperation { get; }

		void Trace(string message);

		T GetTargetFromInputParameters<T>() where T : Entity;

		EntityReference GetTargetReferenceFromInputParameters();

		T GetInputParameter<T>(string inputParameterName);

		void SetInputParameter<T>(string inputParameterName, T parameter, bool createParameter = true);

		T GetOutputParameter<T>(string outputParameterName);

		void SetOutputParameter<T>(string outputParameterName, T parameter, bool createParameter = true);

		T GetPreImage<T>(string preImageName) where T : Entity;

		T GetPostImage<T>(string postImageName) where T : Entity;

		T GetSharedVariable<T>(string variableName, bool retrieveInParentContextChain = false);

		bool TryGetTargetEntityFromInputParameters<T>(out T entity) where T : Entity;

		T GetSettings<T>(string key);

		bool IsPluginInvokedFromMessage(string messageName, string primaryEntityName, int stage);

		int DetermineCallingUserLanguage();
	}
}
