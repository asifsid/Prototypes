using System;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Microsoft.Xrm.Kernel.Contracts;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class AzureSchedulerAdapter : IAzureSchedulerAdapter
	{
		private AzureSchedulerProperties _azureProps;

		private ICrmAzureSchedulerProxy _azureProxy;

		private IPluginContext _context;

		public AzureSchedulerAdapter(IPluginContext context, AzureSchedulerProperties azureProps)
		{
			_context = context;
			_azureProps = azureProps;
			switch (AzureSchedulerUtility.GetSchedulerType(_azureProps.Url))
			{
			case AzureSchedulerType.RDFEBased:
				_azureProxy = context.AzureProxyFactory.CreateAzureRdfeBasedSchedulerProxy(_azureProps.Url);
				break;
			case AzureSchedulerType.ARMBased:
				_azureProxy = context.AzureProxyFactory.CreateAzureArmBasedSchedulerProxy(_azureProps.AADAppTenantId, _azureProps.AADAppId, azureProps.AADAppPassword, _azureProps.Url);
				break;
			}
		}

		public string CreateJobCollection(string collectionName)
		{
			string result = string.Empty;
			try
			{
				AzureSchedulerType schedulerType = AzureSchedulerUtility.GetSchedulerType(_azureProps.Url);
				result = _azureProxy.CreateJobCollection(collectionName, GetJobCollectionBody(schedulerType));
			}
			catch (Exception exception)
			{
				AzureSchedulerUtility.ProcessSchedulerError(exception, _context);
			}
			return result;
		}

		public string GetJobCollection(string collectionName)
		{
			string result = string.Empty;
			try
			{
				result = _azureProxy.GetJobCollection(collectionName);
			}
			catch (Exception exception)
			{
				AzureSchedulerUtility.ProcessSchedulerError(exception, _context);
			}
			return result;
		}

		public string DeleteJobCollection(string collectionName)
		{
			string result = string.Empty;
			try
			{
				result = _azureProxy.DeleteJobCollection(collectionName);
			}
			catch (Exception exception)
			{
				AzureSchedulerUtility.ProcessSchedulerError(exception, _context);
			}
			return result;
		}

		public IAzureSchedulerResponse CreateJob(string jobName, string message, string collectionName)
		{
			string input = string.Empty;
			try
			{
				input = _azureProxy.CreateJob(jobName, message, collectionName);
			}
			catch (Exception exception)
			{
				AzureSchedulerUtility.ProcessSchedulerError(exception, _context);
			}
			return new JavaScriptSerializer().Deserialize<SchedulerResponseData>(input);
		}

		public string DeleteJob(string jobName, string collectionName)
		{
			string result = string.Empty;
			try
			{
				result = _azureProxy.DeleteJob(jobName, collectionName);
			}
			catch (Exception exception)
			{
				AzureSchedulerUtility.ProcessSchedulerError(exception, _context);
			}
			return result;
		}

		public string GetJob(string jobName, string collectionName)
		{
			string result = string.Empty;
			try
			{
				result = _azureProxy.GetJob(jobName, collectionName);
			}
			catch (Exception exception)
			{
				AzureSchedulerUtility.ProcessSchedulerError(exception, _context);
			}
			return result;
		}

		public string UpdateJob(string jobName, string message, string collectionName)
		{
			string result = string.Empty;
			try
			{
				result = _azureProxy.UpdateJob(jobName, message, collectionName);
			}
			catch (Exception exception)
			{
				AzureSchedulerUtility.ProcessSchedulerError(exception, _context);
			}
			return result;
		}

		public string GetJSONForQueueJobBody(JobBodyQueueData jobBody)
		{
			if (AzureSchedulerUtility.GetSchedulerType(_azureProps.Url) == AzureSchedulerType.RDFEBased)
			{
				return new JavaScriptSerializer().Serialize(jobBody);
			}
			JobBodyProperties obj = new JobBodyProperties
			{
				Properties = jobBody
			};
			return new JavaScriptSerializer().Serialize(obj);
		}

		private string GetJobCollectionBody(AzureSchedulerType schedulerType)
		{
			if (schedulerType == AzureSchedulerType.ARMBased)
			{
				JobCollectionBody obj = new JobCollectionBody
				{
					Location = _azureProps.Location,
					Properties = new JobCollectionProperties
					{
						Sku = new JobCollectionSku
						{
							Name = _azureProps.Sku
						}
					}
				};
				return new JavaScriptSerializer().Serialize(obj);
			}
			return null;
		}
	}
}
