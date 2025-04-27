using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Interfaces;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Kernel.Contracts;
using Microsoft.Xrm.Kernel.Contracts.Cache;
using Microsoft.Xrm.Kernel.Contracts.CommandExecution;
using Microsoft.Xrm.Kernel.Contracts.ExternalIntegration;
using Microsoft.Xrm.Kernel.Contracts.Security;
using Microsoft.Xrm.Kernel.Contracts.Settings;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public abstract class PluginContextBase : IPluginContext
	{
		private const string InputParameterTarget = "Target";

		private IEntityStore entityStoreService;

		private IPluginExecutionContext pluginExecutionContext;

		private IOrganizationService organizationService;

		private IOrganizationService systemUserOrganizationService;

		private IDeploymentSettings deploymentSettings;

		private IServiceEndpointNotificationService notificationService;

		private ITracingService tracingService;

		private ITrace traceService;

		private ICommandExecutionService commandExecutionService;

		private IFeatureContext featureContext;

		private IAuthorizationService authorizationService;

		private ICache<string, EntityMetadata> entityMetadataCache;

		private ISequentialGuid sequentialGuid;

		private IFieldLevelSecurityBehaviorService fieldLevelSecurityBehaviorService;

		private IOrganizationServiceFactoryRunAs organizationServiceFactoryRunAs;

		private IOrganizationEndpointService organizationEndpointService;

		private IMergeOperationParameters mergeOperationParameters;

		private IAzureProxyFactory azureProxyFactory;

		private IExternalIntegrationSettings externalIntegrationSettings;

		private IResourceSchedulingCommandService resourceSchedulingCommandService;

		private ISharedVariablesService sharedVariablesService;

		private IDeveloperTestSettings developerTestSettings;

		private IAddressFormatter addressFormatter;

		private IOfflineDataParametersContainer offlineData;

		protected IServiceProvider ServiceProvider { get; private set; }

		protected ISettings Settings { get; private set; }

		public virtual IDeploymentSettings DeploymentSettings
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IDeploymentSettings obj = deploymentSettings;
				if (obj == null)
				{
					IDeploymentSettings val = (IDeploymentSettings)ServiceProvider.GetService(typeof(IDeploymentSettings));
					IDeploymentSettings val2 = val;
					deploymentSettings = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IOrganizationServiceFactoryRunAs OrganizationServiceFactoryRunAs
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IOrganizationServiceFactoryRunAs obj = organizationServiceFactoryRunAs;
				if (obj == null)
				{
					IOrganizationServiceFactoryRunAs val = (IOrganizationServiceFactoryRunAs)ServiceProvider.GetService(typeof(IOrganizationServiceFactoryRunAs));
					IOrganizationServiceFactoryRunAs val2 = val;
					organizationServiceFactoryRunAs = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IOrganizationEndpointService OrganizationEndPointService
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IOrganizationEndpointService obj = organizationEndpointService;
				if (obj == null)
				{
					IOrganizationEndpointService val = (IOrganizationEndpointService)ServiceProvider.GetService(typeof(IOrganizationEndpointService));
					IOrganizationEndpointService val2 = val;
					organizationEndpointService = val;
					obj = val2;
				}
				return obj;
			}
		}

		public IResourceSchedulingCommandService ResourceSchedulingCommandService => resourceSchedulingCommandService ?? (resourceSchedulingCommandService = ServiceProviderExtensions.Get<IResourceSchedulingCommandService>(ServiceProvider));

		public virtual IEntityStore EntityStoreService
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IEntityStore obj = entityStoreService;
				if (obj == null)
				{
					IEntityStore val = (IEntityStore)ServiceProvider.GetService(typeof(IEntityStore));
					IEntityStore val2 = val;
					entityStoreService = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IPluginExecutionContext PluginExecutionContext
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IPluginExecutionContext obj = pluginExecutionContext;
				if (obj == null)
				{
					IPluginExecutionContext val = (IPluginExecutionContext)ServiceProvider.GetService(typeof(IPluginExecutionContext));
					IPluginExecutionContext val2 = val;
					pluginExecutionContext = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IOrganizationService OrganizationService
		{
			get
			{
				if (ReturnSystemUserOrganizationService)
				{
					return systemUserOrganizationService ?? (systemUserOrganizationService = OrganizationServiceFactory.CreateOrganizationService((Guid?)null));
				}
				return organizationService ?? (organizationService = OrganizationServiceFactory.CreateOrganizationService((Guid?)((IExecutionContext)PluginExecutionContext).get_UserId()));
			}
		}

		public virtual IOrganizationService SystemUserOrganizationService => systemUserOrganizationService ?? (systemUserOrganizationService = OrganizationServiceFactory.CreateOrganizationService((Guid?)null));

		public virtual IFieldLevelSecurityBehaviorService FieldLevelSecurityBehaviorService
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IFieldLevelSecurityBehaviorService obj = fieldLevelSecurityBehaviorService;
				if (obj == null)
				{
					IFieldLevelSecurityBehaviorService val = (IFieldLevelSecurityBehaviorService)ServiceProvider.GetService(typeof(IFieldLevelSecurityBehaviorService));
					IFieldLevelSecurityBehaviorService val2 = val;
					fieldLevelSecurityBehaviorService = val;
					obj = val2;
				}
				return obj;
			}
		}

		public bool ReturnSystemUserOrganizationService { get; set; }

		public IOrganizationServiceFactory OrganizationServiceFactory => (IOrganizationServiceFactory)ServiceProvider.GetService(typeof(IOrganizationServiceFactory));

		public IFeatureContext FeatureContext
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IFeatureContext obj = featureContext;
				if (obj == null)
				{
					IFeatureContext val = (IFeatureContext)ServiceProvider.GetService(typeof(IFeatureContext));
					IFeatureContext val2 = val;
					featureContext = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IServiceEndpointNotificationService NotificationService
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IServiceEndpointNotificationService obj = notificationService;
				if (obj == null)
				{
					IServiceEndpointNotificationService val = (IServiceEndpointNotificationService)ServiceProvider.GetService(typeof(IServiceEndpointNotificationService));
					IServiceEndpointNotificationService val2 = val;
					notificationService = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual ITracingService TracingService
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				ITracingService obj = tracingService;
				if (obj == null)
				{
					ITracingService val = (ITracingService)ServiceProvider.GetService(typeof(ITracingService));
					ITracingService val2 = val;
					tracingService = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual ITrace TraceService
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				ITrace obj = traceService;
				if (obj == null)
				{
					ITrace val = (ITrace)ServiceProvider.GetService(typeof(ITrace));
					ITrace val2 = val;
					traceService = val;
					obj = val2;
				}
				return obj;
			}
		}

		public IPluginTelemetry PluginTelemetry
		{
			get
			{
				IPluginTelemetry instance = PluginTelemetryLogger.Instance;
				instance.LoggingMode = TelemetryMode.All;
				if (FeatureContext != null && FeatureContext.IsFeatureEnabled("FCB.PluginTelemetryDisabled"))
				{
					if (!FeatureContext.IsFeatureEnabled("FCB.PluginTelemetryOperationEventsDisabled"))
					{
						instance.LoggingMode = TelemetryMode.OperationEvents;
					}
					else
					{
						instance.LoggingMode = TelemetryMode.Disabled;
					}
				}
				return instance;
			}
		}

		public virtual ICommandExecutionService CommandExecutionService
		{
			get
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Expected O, but got Unknown
				if (commandExecutionService == null)
				{
					commandExecutionService = (ICommandExecutionService)ServiceProvider.GetService(typeof(ICommandExecutionService));
				}
				return commandExecutionService;
			}
		}

		public ISequentialGuid SequentialGuid
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				ISequentialGuid obj = sequentialGuid;
				if (obj == null)
				{
					ISequentialGuid val = (ISequentialGuid)ServiceProvider.GetService(typeof(ISequentialGuid));
					ISequentialGuid val2 = val;
					sequentialGuid = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IOfflineDataParametersContainer OfflineData
		{
			get
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Expected O, but got Unknown
				if (offlineData == null)
				{
					offlineData = (IOfflineDataParametersContainer)ServiceProvider.GetService(typeof(IOfflineDataParametersContainer));
				}
				return offlineData;
			}
		}

		public virtual IAuthorizationService AuthorizationService
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IAuthorizationService obj = authorizationService;
				if (obj == null)
				{
					IAuthorizationService val = (IAuthorizationService)ServiceProvider.GetService(typeof(IAuthorizationService));
					IAuthorizationService val2 = val;
					authorizationService = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IMergeOperationParameters MergeOperationParameters => mergeOperationParameters ?? (mergeOperationParameters = ServiceProviderExtensions.Get<IMergeOperationParameters>(ServiceProvider));

		public virtual IAzureProxyFactory AzureProxyFactory
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IAzureProxyFactory obj = azureProxyFactory;
				if (obj == null)
				{
					IAzureProxyFactory val = (IAzureProxyFactory)ServiceProvider.GetService(typeof(IAzureProxyFactory));
					IAzureProxyFactory val2 = val;
					azureProxyFactory = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IExternalIntegrationSettings ExternalIntegrationSettings
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IExternalIntegrationSettings obj = externalIntegrationSettings;
				if (obj == null)
				{
					IExternalIntegrationSettings val = (IExternalIntegrationSettings)ServiceProvider.GetService(typeof(IExternalIntegrationSettings));
					IExternalIntegrationSettings val2 = val;
					externalIntegrationSettings = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual ISharedVariablesService SharedVariablesService
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				ISharedVariablesService obj = sharedVariablesService;
				if (obj == null)
				{
					ISharedVariablesService val = (ISharedVariablesService)ServiceProvider.GetService(typeof(ISharedVariablesService));
					ISharedVariablesService val2 = val;
					sharedVariablesService = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IDeveloperTestSettings DeveloperTestSettings
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IDeveloperTestSettings obj = developerTestSettings;
				if (obj == null)
				{
					IDeveloperTestSettings val = (IDeveloperTestSettings)ServiceProvider.GetService(typeof(IDeveloperTestSettings));
					IDeveloperTestSettings val2 = val;
					developerTestSettings = val;
					obj = val2;
				}
				return obj;
			}
		}

		public virtual IAddressFormatter AddressFormatter
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				//IL_0030: Expected O, but got Unknown
				IAddressFormatter obj = addressFormatter;
				if (obj == null)
				{
					IAddressFormatter val = (IAddressFormatter)ServiceProvider.GetService(typeof(IAddressFormatter));
					IAddressFormatter val2 = val;
					addressFormatter = val;
					obj = val2;
				}
				return obj;
			}
		}

		public ICache<string, EntityMetadata> EntityMetadataCache => entityMetadataCache ?? (entityMetadataCache = (ICache<string, EntityMetadata>)ServiceProvider.GetService(typeof(ICache<string, EntityMetadata>)));

		public virtual IServiceProvider GetServiceProvider => ServiceProvider;

		public virtual bool IsInMergeOperation => GetIsInMergeOperation();

		public bool IsInClientContext => ((IExecutionContext)PluginExecutionContext).get_IsExecutingOffline() || ((IExecutionContext)PluginExecutionContext).get_IsOfflinePlayback();

		public PluginContextBase(IServiceProvider serviceProvider)
		{
			Exceptions.ThrowIfNull(serviceProvider, "serviceProvider");
			ServiceProvider = serviceProvider;
			Settings = ServiceProviderExtensions.Get<ISettings>(ServiceProvider);
		}

		public virtual void TraceOnTraceLog(TraceLevel traceLevel, string format, object[] args)
		{
			if (TraceService != null)
			{
				TraceService.Write(traceLevel, format, args);
			}
		}

		public virtual void TraceOnPlugInTraceLog(string message)
		{
			if (!string.IsNullOrWhiteSpace(message) && TracingService != null)
			{
				if (PluginExecutionContext == null)
				{
					TracingService.Trace(message, Array.Empty<object>());
					return;
				}
				TracingService.Trace("{0}, Correlation Id: {1}, Initiating User: {2}", new object[3]
				{
					message,
					((IExecutionContext)PluginExecutionContext).get_CorrelationId(),
					((IExecutionContext)PluginExecutionContext).get_InitiatingUserId()
				});
			}
		}

		[Obsolete("Trace is deprecated, please use TraceOnPlugInTraceLog or if you are First Party Solution use TraceOnTraceLog")]
		public virtual void Trace(string message)
		{
			TraceOnPlugInTraceLog(message);
		}

		public T GetSettings<T>(string key)
		{
			return Settings.Get<T>(key);
		}

		public virtual bool GetIsInMergeOperation()
		{
			bool result = false;
			for (IPluginExecutionContext parentContext = PluginExecutionContext; parentContext != null; parentContext = parentContext.get_ParentContext())
			{
				if (((IExecutionContext)parentContext).get_MessageName().Equals("Merge", StringComparison.InvariantCultureIgnoreCase))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public virtual T GetTargetFromInputParameters<T>() where T : Entity
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			T result = default(T);
			if (((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).Contains("Target"))
			{
				object obj = ((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).get_Item("Target");
				if (obj is Entity)
				{
					Entity val = (Entity)obj;
					return val.ToEntity<T>();
				}
				return (T)obj;
			}
			return result;
		}

		public virtual EntityReference GetTargetReferenceFromInputParameters()
		{
			return GetInputParameter<EntityReference>("Target");
		}

		public virtual T GetInputParameter<T>(string inputParameterName)
		{
			T result = default(T);
			if (((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).Contains(inputParameterName))
			{
				return (T)((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).get_Item(inputParameterName);
			}
			return result;
		}

		public void SetInputParameter<T>(string inputParameterName, T parameter, bool createParameter = true)
		{
			if (((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).Contains(inputParameterName))
			{
				((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).set_Item(inputParameterName, (object)parameter);
				return;
			}
			if (createParameter)
			{
				((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).Add(inputParameterName, (object)parameter);
				return;
			}
			throw new CrmInvalidOperationException(Labels.CannotSetInexistentParameter);
		}

		public T GetOutputParameter<T>(string outputParameterName)
		{
			T result = default(T);
			if (((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_OutputParameters()).Contains(outputParameterName))
			{
				return (T)((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_OutputParameters()).get_Item(outputParameterName);
			}
			return result;
		}

		public void SetOutputParameter<T>(string outputParameterName, T parameter, bool createParameter = true)
		{
			if (((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_OutputParameters()).Contains(outputParameterName))
			{
				((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_OutputParameters()).set_Item(outputParameterName, (object)parameter);
				return;
			}
			if (createParameter)
			{
				((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_OutputParameters()).Add(outputParameterName, (object)parameter);
				return;
			}
			throw new CrmInvalidOperationException(Labels.CannotSetInexistentParameter);
		}

		public T GetPreImage<T>(string preImageName) where T : Entity
		{
			T result = default(T);
			if (((DataCollection<string, Entity>)(object)((IExecutionContext)PluginExecutionContext).get_PreEntityImages()).Contains(preImageName))
			{
				Entity val = ((DataCollection<string, Entity>)(object)((IExecutionContext)PluginExecutionContext).get_PreEntityImages()).get_Item(preImageName);
				if (val != null)
				{
					Entity val2 = val;
					return val2.ToEntity<T>();
				}
				return (T)(object)val;
			}
			return result;
		}

		public T GetPostImage<T>(string postImageName) where T : Entity
		{
			T result = default(T);
			if (((DataCollection<string, Entity>)(object)((IExecutionContext)PluginExecutionContext).get_PostEntityImages()).Contains(postImageName))
			{
				return (T)(object)((DataCollection<string, Entity>)(object)((IExecutionContext)PluginExecutionContext).get_PostEntityImages()).get_Item(postImageName);
			}
			return result;
		}

		public T GetSharedVariable<T>(string variableName, bool retrieveInParentContextChain = false)
		{
			T result = default(T);
			if (!retrieveInParentContextChain)
			{
				if (((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_SharedVariables()).Contains(variableName))
				{
					return (T)((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_SharedVariables()).get_Item(variableName);
				}
			}
			else
			{
				for (IPluginExecutionContext parentContext = PluginExecutionContext; parentContext != null; parentContext = parentContext.get_ParentContext())
				{
					if (((DataCollection<string, object>)(object)((IExecutionContext)parentContext).get_SharedVariables()).Contains(variableName))
					{
						return (T)((DataCollection<string, object>)(object)((IExecutionContext)parentContext).get_SharedVariables()).get_Item(variableName);
					}
				}
			}
			return result;
		}

		public bool IsPluginInvokedFromMessage(string messageName, string primaryEntityName, int stage)
		{
			bool result = false;
			for (IPluginExecutionContext parentContext = PluginExecutionContext; parentContext != null; parentContext = parentContext.get_ParentContext())
			{
				if (parentContext.get_Stage() == stage && ((IExecutionContext)parentContext).get_MessageName().ToLower().Equals(messageName.ToLower()) && ((IExecutionContext)parentContext).get_PrimaryEntityName().ToLower().Equals(primaryEntityName.ToLower()))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public bool TryGetTargetEntityFromInputParameters<T>(out T entity) where T : Entity
		{
			entity = default(T);
			if (!((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).Contains("Target"))
			{
				return false;
			}
			if (typeof(T) == typeof(Entity))
			{
				object obj = ((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).get_Item("Target");
				entity = (T)(obj as T);
				return entity != null;
			}
			object obj2 = ((DataCollection<string, object>)(object)((IExecutionContext)PluginExecutionContext).get_InputParameters()).get_Item("Target");
			Entity val = obj2 as Entity;
			if (val == null)
			{
				return false;
			}
			entity = val.ToEntity<T>();
			return entity != null;
		}

		public T GetService<T>()
		{
			return ServiceProviderExtensions.Get<T>(ServiceProvider);
		}

		public void SetSharedVariableToSkipUpdatePlugin(string entityLogicalName, int stage)
		{
			string text = SharedVariableNamesProvider.Get("Update", entityLogicalName, stage);
			SharedVariablesService.Set(text, (object)true, (Scope)2, (Lifetime)1);
		}

		public void UnSetSharedVariableToSkipUpdatePlugin(string entityLogicalName, int stage)
		{
			string text = SharedVariableNamesProvider.Get("Update", entityLogicalName, stage);
			SharedVariablesService.Set(text, (object)false, (Scope)2, (Lifetime)1);
		}

		public int DetermineCallingUserLanguage()
		{
			CultureProvider cultureProvider = new CultureProvider(this);
			IEnumerable<int> applicableCultures = cultureProvider.GetApplicableCultures();
			foreach (int item in applicableCultures)
			{
				if (item != 0)
				{
					return item;
				}
			}
			return cultureProvider.NativeLocaleId;
		}
	}
}
