using System;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin.Common;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Sales.LinkedIn.PhotoAPIIntegrationPlugin
{
	[ComVisible(true)]
	public class PostOperationPhotoCheckPlugin : PluginBase
	{
		public PostOperationPhotoCheckPlugin(string unsecure, string secure)
			: base(typeof(PostOperationPhotoCheckPlugin))
		{
		}

		protected override void ExecuteCrmPlugin(LocalPluginContext localContext)
		{
			if (((IExecutionContext)localContext.PluginExecutionContext).get_Depth() > 1)
			{
				return;
			}
			Entity outputParameter = InternalUtils.GetOutputParameter<Entity>(localContext.PluginExecutionContext, "BusinessEntity");
			if (outputParameter != null)
			{
				_ = string.Empty;
				if (((DataCollection<string, object>)(object)outputParameter.get_Attributes()).ContainsKey("entityimage_url"))
				{
					string value = (string)((DataCollection<string, object>)(object)outputParameter.get_Attributes()).get_Item("entityimage_url");
					if (!string.IsNullOrEmpty(value))
					{
						localContext.TracingService.Trace("CRM Image exists for the record", Array.Empty<object>());
						return;
					}
				}
				MemberPhotoFetchOutput memberPhotoFetchOutput = InternalUtils.ValidateRecordForAliveLinkedInImage(localContext.SystemUserOrganizationService, outputParameter.get_Id(), localContext.TracingService);
				if (!string.IsNullOrEmpty(memberPhotoFetchOutput?.photoUrl))
				{
					localContext.TracingService.Trace("Copying LinkedIn photo URL to the entityimage_url", Array.Empty<object>());
					outputParameter.set_Item("entityimage_url", (object)memberPhotoFetchOutput.photoUrl);
				}
				else
				{
					localContext.TracingService.Trace("LinkedIn Photo doesn't exist for profile association", Array.Empty<object>());
				}
			}
			else
			{
				localContext.TracingService.Trace("Output Parameter contains null output.", Array.Empty<object>());
			}
		}
	}
}
