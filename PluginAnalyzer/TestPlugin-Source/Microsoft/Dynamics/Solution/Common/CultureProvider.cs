using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class CultureProvider
	{
		private const int InvalidLocaleId = 0;

		private IPluginContext pluginContext;

		private Guid currentUserId;

		private Guid organizationId;

		private IOrganizationService organizationService;

		private static readonly OrganizationCache<Guid, int> LocaleIdPerUser = new OrganizationCache<Guid, int>();

		private static readonly OrganizationCache<Guid, int> LocaleIdPerOrganization = new OrganizationCache<Guid, int>();

		public int NativeLocaleId => 1033;

		internal CultureProvider()
		{
			IPluginContext currentContext = PluginContextManager.GetCurrentContext();
			organizationId = ((IExecutionContext)currentContext.PluginExecutionContext).get_OrganizationId();
			currentUserId = ((IExecutionContext)currentContext.PluginExecutionContext).get_InitiatingUserId();
			organizationService = currentContext.OrganizationService;
		}

		internal CultureProvider(IPluginContext pluginContext)
		{
			this.pluginContext = pluginContext;
			organizationId = ((IExecutionContext)pluginContext.PluginExecutionContext).get_OrganizationId();
			currentUserId = ((IExecutionContext)pluginContext.PluginExecutionContext).get_InitiatingUserId();
			organizationService = pluginContext.OrganizationService;
		}

		internal IEnumerable<int> GetApplicableCultures()
		{
			int userCulture = GetCurrentUserCulture();
			if (userCulture != 0)
			{
				yield return userCulture;
			}
			int organizationCulture = GetOrganizationCulture();
			if (organizationCulture != 0)
			{
				yield return organizationCulture;
			}
			yield return NativeLocaleId;
		}

		internal IEnumerable<CultureInfo> GetApplicableCultureInfo()
		{
			foreach (int localeId in GetApplicableCultures())
			{
				yield return new CultureInfo(localeId);
			}
		}

		private int GetCurrentUserCulture()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			if (!LocaleIdPerUser.TryGetValue(currentUserId, out var value))
			{
				QueryExpression val = new QueryExpression("usersettings");
				val.set_ColumnSet(new ColumnSet(new string[1] { "uilanguageid" }));
				val.get_Criteria().AddCondition("systemuserid", (ConditionOperator)0, new object[1] { currentUserId });
				val.set_TopCount((int?)1);
				EntityCollection val2 = organizationService.RetrieveMultiple((QueryBase)(object)val);
				if (val2 != null && ((Collection<Entity>)(object)val2.get_Entities()).Count > 0)
				{
					UserSettings userSettings = ((Collection<Entity>)(object)val2.get_Entities())[0].ToEntity<UserSettings>();
					int? uILanguageId = userSettings.UILanguageId;
					if (uILanguageId.HasValue)
					{
						value = uILanguageId.Value;
						LocaleIdPerUser.TryAdd(currentUserId, value);
					}
				}
			}
			return value;
		}

		private int GetOrganizationCulture()
		{
			if (!LocaleIdPerOrganization.TryGetValue(organizationId, out var value))
			{
				value = pluginContext.GetSettings<int>("Microsoft.Xrm.Organization.LanguageCode");
				LocaleIdPerOrganization.TryAdd(organizationId, value);
			}
			return value;
		}
	}
}
