using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin
{
	[ComVisible(true)]
	public class InternalUtils
	{
		public static T GetInputParameter<T>(IPluginExecutionContext context, string inputParameterName)
		{
			T result = default(T);
			if (((DataCollection<string, object>)(object)((IExecutionContext)context).get_InputParameters()).Contains(inputParameterName))
			{
				return (T)((DataCollection<string, object>)(object)((IExecutionContext)context).get_InputParameters()).get_Item(inputParameterName);
			}
			return result;
		}

		public static T GetOutputParameter<T>(IPluginExecutionContext context, string outputParameterName)
		{
			T result = default(T);
			if (((DataCollection<string, object>)(object)((IExecutionContext)context).get_OutputParameters()).Contains(outputParameterName))
			{
				return (T)((DataCollection<string, object>)(object)((IExecutionContext)context).get_OutputParameters()).get_Item(outputParameterName);
			}
			return result;
		}

		public static void ThrowIfNull<T>(string name, T argument)
		{
			if (argument != null)
			{
				return;
			}
			throw new ArgumentNullException(name);
		}

		public static void ThrowIfNullOrEmpty(string name, string argument)
		{
			if (!string.IsNullOrEmpty(argument))
			{
				return;
			}
			throw new ArgumentNullException(name, "Null or empty value recieved for " + name + ". Value: " + argument);
		}

		public static long GetUnixTimeStampInMilliSeconds(DateTime dateObj)
		{
			return (long)dateObj.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}

		public static LIAccessToken FetchAccessToken(LIAppClientCredentials appSecrets, ITracingService tracingService)
		{
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			string requestUri = "https://www.linkedin.com/oauth/v2/accessToken?grant_type=client_credentials&client_id=" + appSecrets.client_id + "&client_secret=" + appSecrets.client_secret;
			HttpResponseMessage httpResponseMessage = HttpClientWrapper.Instance?.Client?.PostAsync(requestUri, null)?.Result;
			if (httpResponseMessage != null && !httpResponseMessage.IsSuccessStatusCode)
			{
				string text = httpResponseMessage?.Content?.ReadAsStringAsync()?.Result;
				throw new InvalidPluginExecutionException(text ?? $"Expected success status code, recieved {httpResponseMessage.StatusCode}");
			}
			string serializedJson = httpResponseMessage?.Content?.ReadAsStringAsync().Result;
			LIAccessToken lIAccessToken = Deserialize<LIAccessToken>(serializedJson);
			tracingService.Trace($"Used PostAsync call. AccessToken retrieval state={((!string.IsNullOrEmpty(lIAccessToken?.access_token)) ? true : false)}", Array.Empty<object>());
			return lIAccessToken;
		}

		public static bool ValidateLinkedInPhotoTTL(Entity profileAssociationsObj, ITracingService traceService)
		{
			if (profileAssociationsObj.Contains("sales_linkedin_profilephotorefreshtime") && profileAssociationsObj.Contains("sales_linkedin_profilephotourl"))
			{
				string value = (string)profileAssociationsObj.get_Item("sales_linkedin_profilephotourl");
				string value2 = (string)profileAssociationsObj.get_Item("sales_linkedin_profilephotorefreshtime");
				if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value2))
				{
					long num = Convert.ToInt64(value2);
					long unixTimeStampInMilliSeconds = GetUnixTimeStampInMilliSeconds(DateTime.Now);
					if (num > unixTimeStampInMilliSeconds)
					{
						return true;
					}
					traceService.Trace("Existing photo URL has expired", Array.Empty<object>());
				}
				else
				{
					traceService.Trace("Retrieved profile association contains null/empty RefreshTime or PhotoUrl", Array.Empty<object>());
				}
			}
			else
			{
				traceService.Trace("Retrieved profile association doesn't contain RefreshTime or PhotoUrl", Array.Empty<object>());
			}
			return false;
		}

		public static MemberPhotoFetchOutput ValidateRecordForAliveLinkedInImage(IOrganizationService organizationService, Guid recordId, ITracingService tracingService)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			QueryExpression val = new QueryExpression("sales_linkedin_profileassociations");
			val.set_TopCount((int?)1);
			val.get_Criteria().AddCondition("sales_linkedin_objectid", (ConditionOperator)0, new object[1] { recordId.ToString() });
			val.get_ColumnSet().AddColumns(new string[3] { "sales_linkedin_objectid", "sales_linkedin_profilephotourl", "sales_linkedin_profilephotorefreshtime" });
			EntityCollection val2 = organizationService.RetrieveMultiple((QueryBase)(object)val);
			if (val2 != null && ((Collection<Entity>)(object)val2.get_Entities())?.Count > 0)
			{
				Entity val3 = ((Collection<Entity>)(object)val2.get_Entities())[0];
				bool flag = ValidateLinkedInPhotoTTL(val3, tracingService);
				new MemberPhotoFetchOutput();
				MemberPhotoFetchOutput result = ((!flag) ? new MemberPhotoFetchOutput(_shouldRefresh: false, "", "", val3.get_Id().ToString()) : new MemberPhotoFetchOutput(_shouldRefresh: false, "LinkedIn", (string)val3.get_Item("sales_linkedin_profilephotourl"), val3.get_Id().ToString()));
				tracingService.Trace("ProfileAssociation(" + val3.get_Id().ToString() + ") found for recordId=" + recordId.ToString() + " in state Alive=" + flag, Array.Empty<object>());
				return result;
			}
			tracingService.Trace("ProfileAssociation record doesn't exist for recordId=" + recordId.ToString(), Array.Empty<object>());
			return null;
		}

		public static void PersistFatalErrors(string errorMessage, ITracingService tracingService, IOrganizationService orgService)
		{
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			if (string.IsNullOrEmpty(errorMessage))
			{
				tracingService.Trace("PersistFatalErrors: Empty errorMessage", Array.Empty<object>());
				return;
			}
			string text = string.Empty;
			ProfileAssociationError profileAssociationError = Deserialize<ProfileAssociationError>(errorMessage);
			if (Array.IndexOf(Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common.Constants.CrmSyncErrorCodes, profileAssociationError.serviceErrorCode) >= 0)
			{
				text = "ERROR_CRM_SYNC_DISABLED";
			}
			if (!string.IsNullOrEmpty(text))
			{
				tracingService.Trace("Found terminating error:" + text, Array.Empty<object>());
				Guid? guid = RetriveConfigEntityRecord(orgService);
				if (!guid.HasValue || guid == Guid.Empty)
				{
					tracingService.Trace("Failure in fetching the config entity record, cannot persist the terminating error", Array.Empty<object>());
					return;
				}
				Entity val = new Entity("sales_linkedin_configuration");
				val.set_Id(guid.Value);
				val.set_Item("sales_linkedin_profileFetch_Status", (object)text);
				orgService.Update(val);
				tracingService.Trace("Successfully persisted the terminating error in config entity:" + guid.ToString(), Array.Empty<object>());
			}
		}

		public static Guid? RetriveConfigEntityRecord(IOrganizationService orgService)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			QueryExpression val = new QueryExpression("sales_linkedin_configuration");
			val.set_ColumnSet(new ColumnSet(false));
			val.get_Criteria().AddCondition("sales_name", (ConditionOperator)0, new object[1] { "Active Sales Navigator Configuration" });
			EntityCollection val2 = orgService.RetrieveMultiple((QueryBase)(object)val);
			Guid? result;
			if (val2 == null)
			{
				result = null;
			}
			else
			{
				DataCollection<Entity> entities = val2.get_Entities();
				if (entities == null)
				{
					result = null;
				}
				else
				{
					Entity obj = ((Collection<Entity>)(object)entities)[0];
					result = ((obj != null) ? new Guid?(obj.get_Id()) : null);
				}
			}
			return result;
		}

		public static void StoreValueInConfigStore(ILocalConfigStore localConfigStore, string key, string value)
		{
			ThrowIfNull<ILocalConfigStore>("localConfigStore", localConfigStore);
			localConfigStore.SetData(key, (object)value);
		}

		public static string GetValueFromConfigStore(ILocalConfigStore localConfigStore, string key)
		{
			ThrowIfNull<ILocalConfigStore>("localConfigStore", localConfigStore);
			return localConfigStore.GetData<string>(key);
		}

		public static string Serialize<T>(T obj)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			using MemoryStream memoryStream = new MemoryStream();
			dataContractJsonSerializer.WriteObject(memoryStream, obj);
			memoryStream.Position = 0L;
			StreamReader streamReader = new StreamReader(memoryStream);
			return streamReader.ReadToEnd();
		}

		public static T Deserialize<T>(string serializedJson)
		{
			if (string.IsNullOrEmpty(serializedJson))
			{
				return default(T);
			}
			using MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(serializedJson);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			return (T)dataContractJsonSerializer.ReadObject(memoryStream);
		}

		public static LIAppClientCredentials TryCreateLinkedInChildAppAndReturnSecret(string orgId, string accessToken, ITracingService tracingService, IKeyVaultClient keyVaultClient)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(orgId) || string.IsNullOrWhiteSpace(accessToken))
				{
					tracingService.Trace("TryCreateLinkedInChildAppAndReturnSecret:Invalid inputs", Array.Empty<object>());
					return null;
				}
				tracingService.Trace("Starting LI child app creation", Array.Empty<object>());
				ChildAppProvisioningInputs obj = new ChildAppProvisioningInputs
				{
					uniqueForeignId = orgId,
					name = "D365_" + orgId,
					description = "Child Application for D365_" + orgId
				};
				string content = Serialize(obj);
				StringContent content2 = new StringContent(content, Encoding.UTF8, "application/json");
				HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/v2/provisionedApplications")
				{
					Content = content2
				};
				httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
				HttpResponseMessage httpResponseMessage = HttpClientWrapper.Instance?.Client?.SendAsync(httpRequestMessage)?.Result;
				if (httpResponseMessage?.IsSuccessStatusCode ?? false)
				{
					tracingService.Trace("Successfully created LinkedIn child app for org", Array.Empty<object>());
					string serializedJson = httpResponseMessage?.Content?.ReadAsStringAsync()?.Result;
					return Deserialize<ChildAppProvisioningOutput>(serializedJson)?.credentials;
				}
				tracingService.Trace("Creating LinkedIn child app for org failed with error:" + httpResponseMessage?.Content?.ReadAsStringAsync()?.Result, Array.Empty<object>());
				return null;
			}
			catch (Exception ex)
			{
				tracingService.Trace("Creating LinkedIn child app for org failed with error:" + ex, Array.Empty<object>());
				return null;
			}
		}

		public static T GetSecret<T>(string key, IKeyVaultClient keyVaultClient, ITracingService tracingService)
		{
			try
			{
				string serializedJson = ((keyVaultClient != null) ? keyVaultClient.GetSecret(key) : null);
				T val = Deserialize<T>(serializedJson);
				if (val != null)
				{
					return val;
				}
			}
			catch (Exception ex)
			{
				tracingService.Trace("fetching secret from key Vault failed with error:" + ex, Array.Empty<object>());
			}
			return default(T);
		}

		public static void SetSeret(string key, string value, IKeyVaultClient keyVaultClient, ITracingService tracingService)
		{
			try
			{
				if (keyVaultClient != null)
				{
					keyVaultClient.SetSecret(key, value);
				}
				tracingService.Trace("Secret '" + key + "' added sucessfully in key Vault", Array.Empty<object>());
			}
			catch (Exception ex)
			{
				tracingService.Trace("Adding secret in key Vault failed with error:" + ex, Array.Empty<object>());
			}
		}
	}
}
