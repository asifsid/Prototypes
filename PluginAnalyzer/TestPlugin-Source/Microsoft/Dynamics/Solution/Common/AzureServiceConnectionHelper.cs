using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Localization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class AzureServiceConnectionHelper : IAzureServiceConnectionHelper
	{
		private AzureServiceConnectionType connectionType;

		private const string AzureTestConnectionRequestName = "TestAzureServiceConnection";

		private const string GetAzureServiceConnectionByTypeRequestName = "GetAzureServiceConnectionIdByType";

		private const string GetAzureServiceConnectionByTypeRequestParamKey = "ConnectionType";

		private const string GetAzureServiceConnectionByTypeResponseParamKey = "ConnectionId";

		public AzureServiceConnectionHelper(AzureServiceConnectionType type)
		{
			connectionType = type;
		}

		public EntityReference GetAzureServiceConnectionIdByType(IPluginContext context)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			Exceptions.ThrowIfNull(connectionType, "connectionType");
			Exceptions.ThrowIfNull(context, "context");
			ValidateAzureMachineLearningFeaturesEnabled(context);
			OrganizationRequest val = new OrganizationRequest();
			val.set_RequestName("GetAzureServiceConnectionIdByType");
			((DataCollection<string, object>)(object)val.get_Parameters()).Add("ConnectionType", (object)2);
			OrganizationResponse val2 = context.OrganizationService.Execute(val);
			EntityReference result = null;
			if (val2 != null && ((DataCollection<string, object>)(object)val2.get_Results()).ContainsKey("ConnectionId"))
			{
				result = new EntityReference("azureserviceconnection", (Guid)((DataCollection<string, object>)(object)val2.get_Results()).get_Item("ConnectionId"));
			}
			return result;
		}

		public void TestConnection(Guid azureServiceConnectionId, IPluginContext context)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			Exceptions.ThrowIfNull(context, "context");
			ValidateAzureMachineLearningFeaturesEnabled(context);
			OrganizationRequest val = new OrganizationRequest();
			val.set_RequestName("TestAzureServiceConnection");
			((DataCollection<string, object>)(object)val.get_Parameters()).Add("AzureServiceConnectionId", (object)azureServiceConnectionId);
			context.OrganizationService.Execute(val);
		}

		public bool IsConnectionActive(Guid connectionId, IPluginContext context)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ColumnSet val = new ColumnSet();
			val.AddColumn("statecode");
			Entity val2 = context.OrganizationService.Retrieve("azureserviceconnection", connectionId, val);
			OptionSetValue attributeValue = val2.GetAttributeValue<OptionSetValue>("statecode");
			int value = attributeValue.get_Value();
			return value == 0;
		}

		private QueryExpression GetAzureServiceConnectionEntityExpression(IPluginContext context)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			QueryExpression val = new QueryExpression("azureserviceconnection");
			val.get_ColumnSet().AddColumn("azureserviceconnectionid");
			val.get_ColumnSet().AddColumn("serviceuri");
			val.get_ColumnSet().AddColumn("connectiontype");
			return val;
		}

		public virtual string GetConnectionName(IPluginContext context)
		{
			Exceptions.ThrowIfNotDefined(typeof(AzureServiceConnectionType), connectionType, "connectionType");
			AzureServiceConnectionType azureServiceConnectionType = connectionType;
			AzureServiceConnectionType azureServiceConnectionType2 = azureServiceConnectionType;
			if (azureServiceConnectionType2 == AzureServiceConnectionType.TextAnalytics)
			{
				return "AzureServiceConnectionName-TextAnalytics";
			}
			throw new CrmNotSupportedAzureConnectionType(connectionType);
		}

		private void SaveAccountKey(string accountKeyValue, IPluginContext context)
		{
			_ = string.Empty;
			AzureServiceConnectionType azureServiceConnectionType = connectionType;
			AzureServiceConnectionType azureServiceConnectionType2 = azureServiceConnectionType;
			if (azureServiceConnectionType2 == AzureServiceConnectionType.TextAnalytics)
			{
				string text = "AzureMLTextAnalyticsServiceAccountKey";
				context.ExternalIntegrationSettings.SetServiceAccountKey(text, accountKeyValue);
				return;
			}
			throw new CrmNotSupportedAzureConnectionType(connectionType);
		}

		private EntityReference GetAzureServiceConnectionRecordByType(int connectionType, IPluginContext context)
		{
			QueryExpression azureServiceConnectionEntityExpression = GetAzureServiceConnectionEntityExpression(context);
			azureServiceConnectionEntityExpression.get_Criteria().AddCondition("connectiontype", (ConditionOperator)0, new object[1] { connectionType });
			EntityCollection val = context.OrganizationService.RetrieveMultiple((QueryBase)(object)azureServiceConnectionEntityExpression);
			DataCollection<Entity> entities = val.get_Entities();
			CrmException.Assert(((Collection<Entity>)(object)entities).Count <= 1, $"Expected 1 or fewer AzureServiceConnections of type {connectionType.ToString(CultureInfo.InvariantCulture)}. Found {((Collection<Entity>)(object)entities).Count.ToString(CultureInfo.InvariantCulture)}.");
			if (((Collection<Entity>)(object)entities).Count == 1)
			{
				return ((Collection<Entity>)(object)val.get_Entities())[0].ToEntityReference();
			}
			return null;
		}

		private Entity GetAzureServiceConnectionRecordById(Guid azureServiceConnectionId, IPluginContext context)
		{
			QueryExpression azureServiceConnectionEntityExpression = GetAzureServiceConnectionEntityExpression(context);
			return context.OrganizationService.Retrieve(azureServiceConnectionEntityExpression.get_EntityName(), azureServiceConnectionId, azureServiceConnectionEntityExpression.get_ColumnSet());
		}

		private void ValidateAzureMachineLearningFeaturesEnabled(IPluginContext context)
		{
			if (connectionType == AzureServiceConnectionType.TextAnalytics && !context.FeatureContext.IsFeatureEnabled("FCB.ServiceCaseTopicAnalysis"))
			{
				throw new CrmException(Labels.TextAnalyticsFeatureNotEnabled, -2147084718);
			}
		}
	}
}
