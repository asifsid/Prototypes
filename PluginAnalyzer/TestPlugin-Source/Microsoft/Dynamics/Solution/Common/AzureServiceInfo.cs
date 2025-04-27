using System;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class AzureServiceInfo
	{
		public static string GetAccountKey(IPluginContext context, AzureServiceConnectionType connectionType)
		{
			_ = string.Empty;
			if (connectionType == AzureServiceConnectionType.TextAnalytics)
			{
				string text = "AzureMLTextAnalyticsServiceAccountKey";
				return context.ExternalIntegrationSettings.GetServiceAccountKey(text);
			}
			throw new CrmNotSupportedAzureConnectionType(connectionType);
		}

		public static Uri GetServiceUri(IPluginContext context, AzureServiceConnectionType connectionType)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			Uri result = null;
			Entity val = new Entity("azureserviceconnection");
			Entity val2 = context.OrganizationService.Retrieve("azureserviceconnection", val.get_Id(), new ColumnSet(new string[1] { "serviceuri" }));
			if (val2 != null && !val2.IsAttributeNull("serviceuri"))
			{
				result = new Uri((string)((DataCollection<string, object>)(object)val2.get_Attributes()).get_Item("serviceuri"));
			}
			else
			{
				TelemetryUtilities.LogInvalidParameterAndThrowException(context, "Service Uri is null or empty");
			}
			return result;
		}
	}
}
