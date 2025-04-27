using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin
{
	[ComVisible(true)]
	public class PostOperationFetchLISNProfileAssociations : PluginBase
	{
		public PostOperationFetchLISNProfileAssociations(string unsecure, string secure)
			: base(typeof(PostOperationFetchLISNProfileAssociations))
		{
		}

		protected override void ExecuteCrmPlugin(LocalPluginContext localContext)
		{
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			string inputParameter = InternalUtils.GetInputParameter<string>(localContext.PluginExecutionContext, "OrgUrl");
			InternalUtils.ThrowIfNullOrEmpty("OrgUrl", inputParameter);
			string inputParameter2 = InternalUtils.GetInputParameter<string>(localContext.PluginExecutionContext, "RecordType");
			InternalUtils.ThrowIfNullOrEmpty("RecordType", inputParameter2);
			string inputParameter3 = InternalUtils.GetInputParameter<string>(localContext.PluginExecutionContext, "RecordIds");
			InternalUtils.ThrowIfNullOrEmpty("RecordIds", inputParameter3);
			List<Guid> list = Array.ConvertAll(inputParameter3.Split(','), Guid.Parse).ToList();
			if (list == null || list.Count != 1)
			{
				throw new InvalidPluginExecutionException("Retrival of Photo URL is supported for exactly one LinkedIn member");
			}
			localContext.TracingService.Trace("Input parameters validated: orgIdentifier=" + inputParameter + ", recordType=" + inputParameter2 + ", recordId=" + list?.FirstOrDefault().ToString(), Array.Empty<object>());
			Guid guid = ValidateRecordWithValidCrmImage(localContext, inputParameter2, list.FirstOrDefault());
			if (guid == Guid.Empty)
			{
				MemberPhotoFetchOutput obj = new MemberPhotoFetchOutput(_shouldRefresh: false, "CRM");
				((DataCollection<string, object>)(object)((IExecutionContext)localContext.PluginExecutionContext).get_OutputParameters()).Clear();
				((DataCollection<string, object>)(object)((IExecutionContext)localContext.PluginExecutionContext).get_OutputParameters()).Add("Result", (object)InternalUtils.Serialize(obj));
				return;
			}
			MemberPhotoFetchOutput memberPhotoFetchOutput = InternalUtils.ValidateRecordForAliveLinkedInImage(localContext.SystemUserOrganizationService, guid, localContext.TracingService);
			if (string.IsNullOrEmpty(memberPhotoFetchOutput?.photoUrl))
			{
				memberPhotoFetchOutput = FetchMemberPhotosFromLinkedIn(localContext, inputParameter, guid.ToString(), memberPhotoFetchOutput?.profileAssociationId);
			}
			((DataCollection<string, object>)(object)((IExecutionContext)localContext.PluginExecutionContext).get_OutputParameters()).Clear();
			((DataCollection<string, object>)(object)((IExecutionContext)localContext.PluginExecutionContext).get_OutputParameters()).Add("Result", (object)InternalUtils.Serialize(memberPhotoFetchOutput));
		}

		private Guid ValidateRecordWithValidCrmImage(LocalPluginContext localContext, string recordType, Guid recordId)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			QueryExpression val = new QueryExpression(recordType);
			val.get_ColumnSet().AddColumn("entityimage_url");
			val.get_Criteria().AddCondition("entityimage_url", (ConditionOperator)12, Array.Empty<object>());
			val.get_Criteria().AddCondition(recordType + "id", (ConditionOperator)0, new object[1] { recordId });
			val.set_TopCount((int?)1);
			EntityCollection val2 = localContext.OrganizationService.RetrieveMultiple((QueryBase)(object)val);
			if (val2 != null && ((Collection<Entity>)(object)val2.get_Entities())?.Count == 1)
			{
				localContext.TracingService.Trace("CRM image doesn't exist", Array.Empty<object>());
				return recordId;
			}
			localContext.TracingService.Trace("Either record doesn't exist or CRM image exist for the record in the system", Array.Empty<object>());
			return Guid.Empty;
		}

		private LIAuthConfigStoreValue FetchAccessTokenAndStoreInLocalConfig(LocalPluginContext localContext, LIAppClientCredentials appSecrets, string orgId)
		{
			localContext.TracingService.Trace("Fetching access token for child app", Array.Empty<object>());
			LIAuthConfigStoreValue lIAuthConfigStoreValue = new LIAuthConfigStoreValue();
			LIAccessToken lIAccessToken = InternalUtils.FetchAccessToken(appSecrets, localContext.TracingService);
			if (lIAccessToken != null)
			{
				lIAuthConfigStoreValue.clientCredentials = appSecrets;
				lIAccessToken.expires_in = InternalUtils.GetUnixTimeStampInMilliSeconds(DateTime.Now.AddSeconds(lIAccessToken.expires_in));
				lIAuthConfigStoreValue.accessToken = lIAccessToken;
				string value = InternalUtils.Serialize(lIAuthConfigStoreValue);
				InternalUtils.StoreValueInConfigStore(localContext.LocalConfigStore, orgId, value);
				localContext.TracingService.Trace("Successfully stored the LinkedIn child app credentials in Config Store", Array.Empty<object>());
			}
			return lIAuthConfigStoreValue;
		}

		private LIAuthConfigStoreValue GetLinkedInAuthValuesFromConfigStore(LocalPluginContext localContext, string orgId)
		{
			string valueFromConfigStore = InternalUtils.GetValueFromConfigStore(localContext.LocalConfigStore, orgId);
			LIAuthConfigStoreValue lIAuthConfigStoreValue = InternalUtils.Deserialize<LIAuthConfigStoreValue>(valueFromConfigStore);
			if (lIAuthConfigStoreValue == null)
			{
				localContext.TracingService.Trace("LI child app Auth object doesn't exist in Config Store, trying KV", Array.Empty<object>());
				LIAppClientCredentials secret = InternalUtils.GetSecret<LIAppClientCredentials>(orgId, localContext.KeyVaultClient, localContext.TracingService);
				if (secret != null)
				{
					localContext.TracingService.Trace("Successfully fetched child app secrets from KV", Array.Empty<object>());
					return FetchAccessTokenAndStoreInLocalConfig(localContext, secret, orgId);
				}
				LIAppClientCredentials secret2 = InternalUtils.GetSecret<LIAppClientCredentials>("LIMRSParentAppSecrets", localContext.KeyVaultClient, localContext.TracingService);
				if (secret2 != null)
				{
					localContext.TracingService.Trace("Successfully fetched parent app secrets from KV. Fetching access token now.", Array.Empty<object>());
					secret = InternalUtils.TryCreateLinkedInChildAppAndReturnSecret(orgId, InternalUtils.FetchAccessToken(secret2, localContext.TracingService)?.access_token, localContext.TracingService, localContext.KeyVaultClient);
					if (secret != null)
					{
						lIAuthConfigStoreValue = FetchAccessTokenAndStoreInLocalConfig(localContext, secret, orgId);
						localContext.TracingService.Trace("Storing child app creds in KV", Array.Empty<object>());
						InternalUtils.SetSeret(orgId, InternalUtils.Serialize(secret), localContext.KeyVaultClient, localContext.TracingService);
					}
				}
				else
				{
					localContext.TracingService.Trace("Unable to fetch parent app secrets from KV", Array.Empty<object>());
				}
				return lIAuthConfigStoreValue;
			}
			localContext.TracingService.Trace("Successfully fetched LI child app Auth object from Config Store", Array.Empty<object>());
			LIAccessToken accessToken = lIAuthConfigStoreValue.accessToken;
			long unixTimeStampInMilliSeconds = InternalUtils.GetUnixTimeStampInMilliSeconds(DateTime.Now);
			if (unixTimeStampInMilliSeconds >= accessToken.expires_in)
			{
				return FetchAccessTokenAndStoreInLocalConfig(localContext, lIAuthConfigStoreValue.clientCredentials, orgId);
			}
			return lIAuthConfigStoreValue;
		}

		private MemberPhotoFetchOutput FetchMemberPhotosFromLinkedIn(LocalPluginContext localContext, string orgIdentifier, string recordId, string profileAssociationId)
		{
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			LIAuthConfigStoreValue linkedInAuthValuesFromConfigStore = GetLinkedInAuthValuesFromConfigStore(localContext, orgIdentifier);
			if (linkedInAuthValuesFromConfigStore?.accessToken?.access_token != null)
			{
				localContext.TracingService.Trace("Starting photo fetch from LI", Array.Empty<object>());
				string requestUri = "https://api.linkedin.com/v2/salesNavigatorProfileAssociations/(instanceId:" + orgIdentifier + ",partner:MS,recordId:" + recordId + ")";
				HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
				httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", linkedInAuthValuesFromConfigStore?.accessToken.access_token);
				httpRequestMessage.Headers.Add("x-restli-protocol-version", "2.0.0");
				HttpResponseMessage httpResponseMessage = HttpClientWrapper.Instance?.Client?.SendAsync(httpRequestMessage)?.Result;
				if (httpResponseMessage != null && !httpResponseMessage.IsSuccessStatusCode)
				{
					string text = httpResponseMessage?.Content?.ReadAsStringAsync()?.Result;
					text = text ?? $"Expected success status code, recieved {httpResponseMessage.StatusCode}";
					InternalUtils.PersistFatalErrors(text, localContext.TracingService, localContext.SystemUserOrganizationService);
					MemberPhotoFetchOutput result = new MemberPhotoFetchOutput(_shouldRefresh: false, "", "", "", text);
					localContext.TracingService.Trace("Fetching photo from LinkedIn failed with error:" + text, Array.Empty<object>());
					return result;
				}
				string serializedJson = httpResponseMessage?.Content?.ReadAsStringAsync()?.Result;
				string text2 = InternalUtils.Deserialize<ProfileAssociationOutput>(serializedJson)?.profilePhoto;
				if (!string.IsNullOrEmpty(text2))
				{
					localContext.TracingService.Trace("Successfully fetched photo URL from LI", Array.Empty<object>());
					profileAssociationId = StoreProfilePhotoDetails(localContext, text2, recordId, profileAssociationId);
					return new MemberPhotoFetchOutput(_shouldRefresh: true, "LinkedIn", text2, profileAssociationId);
				}
				throw new InvalidPluginExecutionException("Received empty or null profile photo url");
			}
			if (string.IsNullOrEmpty(linkedInAuthValuesFromConfigStore?.clientCredentials?.client_id) || string.IsNullOrEmpty(linkedInAuthValuesFromConfigStore?.clientCredentials?.client_secret))
			{
				throw new InvalidPluginExecutionException("LinkedIn Parent App credentials are empty");
			}
			throw new InvalidPluginExecutionException("LinkedIn AD access token is empty");
		}

		private string StoreProfilePhotoDetails(LocalPluginContext localContext, string profilePhotoUrl, string recordId, string profileAssociationId)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			Entity val = new Entity("sales_linkedin_profileassociations");
			val.set_Item("sales_linkedin_profilephotourl", (object)profilePhotoUrl);
			val.set_Item("sales_linkedin_profilephotorefreshtime", (object)InternalUtils.GetUnixTimeStampInMilliSeconds(DateTime.Now.AddHours(24.0)).ToString());
			if (!string.IsNullOrEmpty(profileAssociationId))
			{
				val.set_Id(new Guid(profileAssociationId));
				localContext.SystemUserOrganizationService.Update(val);
				localContext.TracingService.Trace("Successfully updated photo URL in CRM DB", Array.Empty<object>());
			}
			else
			{
				val.set_Item("sales_linkedin_objectid", (object)recordId);
				profileAssociationId = localContext.SystemUserOrganizationService.Create(val).ToString();
				localContext.TracingService.Trace("Successfully stored photo URL in CRM DB", Array.Empty<object>());
			}
			return profileAssociationId;
		}
	}
}
