using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Kernel.Contracts.Cache;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Microsoft.Dynamics.Solution.Common.Services
{
	[ComVisible(true)]
	public static class OfflineDataUtility
	{
		public static T AssignOfflineGuid<T>(this IPluginContext context, T entity, Expression<Func<T, Guid>> guidSelector) where T : Entity
		{
			string primaryIdAttributeColumnName = GetPrimaryIdAttributeColumnName(context, ((Entity)entity).get_LogicalName());
			if (((IExecutionContext)context.PluginExecutionContext).get_IsExecutingOffline() && primaryIdAttributeColumnName != null)
			{
				context.OfflineData.SetValue(primaryIdAttributeColumnName, Guid.NewGuid().ToString());
			}
			string text = null;
			if (primaryIdAttributeColumnName != null)
			{
				text = context.OfflineData.GetValue(primaryIdAttributeColumnName);
			}
			Guid guid = ((text != null) ? new Guid(text) : context.SequentialGuid.NewGuid());
			MemberExpression memberExpression = guidSelector.Body as MemberExpression;
			if (memberExpression != null)
			{
				PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(entity, guid, null);
				}
			}
			return entity;
		}

		public static string GetPrimaryIdAttributeColumnName(IPluginContext context, string entityLogicalName)
		{
			_ = string.Empty;
			if (context != null && context.FeatureContext != null && context.FeatureContext.IsFeatureEnabled("FCB.SkipMetadataFetchInSales"))
			{
				switch (entityLogicalName)
				{
				case "quotedetail":
					return "quotedetailid";
				case "quote":
					return "quoteid";
				case "invoicedetail":
					return "invoicedetailid";
				case "invoice":
					return "invoiceid";
				case "salesorderdetail":
					return "salesorderdetailid";
				case "salesorder":
					return "salesorderid";
				case "account":
					return "accountid";
				case "contact":
					return "contactid";
				case "opportunity":
					return "opportunityid";
				case "connection":
					return "connectionid";
				case "opportunityclose":
					return "activityid";
				default:
				{
					ICache<string, EntityMetadata> entityMetadataCache = context.EntityMetadataCache;
					object result;
					if (entityMetadataCache == null)
					{
						result = null;
					}
					else
					{
						EntityMetadata obj = entityMetadataCache.Get(entityLogicalName);
						result = ((obj != null) ? obj.get_PrimaryIdAttribute() : null);
					}
					return (string)result;
				}
				}
			}
			ICache<string, EntityMetadata> entityMetadataCache2 = context.EntityMetadataCache;
			object result2;
			if (entityMetadataCache2 == null)
			{
				result2 = null;
			}
			else
			{
				EntityMetadata obj2 = entityMetadataCache2.Get(entityLogicalName);
				result2 = ((obj2 != null) ? obj2.get_PrimaryIdAttribute() : null);
			}
			return (string)result2;
		}
	}
}
