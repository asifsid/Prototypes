using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public class ActionCardFeatureContext
	{
		private const string FCBActionCard = "FCB.ActionCard";

		private const string FCBNonBaseActionCard = "FCB.NonBaseActionCard";

		private const string FCBActionCardGenerationAsync = "FCB.ActionCardGenerationAsync";

		public bool IsActionCardFeatureEnabled(IPluginContext context)
		{
			return context.FeatureContext.IsFeatureEnabled("FCB.ActionCard");
		}

		public bool IsActionCardGenerationAsyncFeatureEnabled(IPluginContext context)
		{
			return context.FeatureContext.IsFeatureEnabled("FCB.ActionCardGenerationAsync");
		}

		public bool IsNonBaseActionCardFeatureEnabled(IPluginContext context)
		{
			return context.FeatureContext.IsFeatureEnabled("FCB.NonBaseActionCard");
		}

		public bool IsActionCardEnabled(IPluginContext context)
		{
			bool result = false;
			if (IsNotOffline(context) && IsActionCardFeatureEnabled(context) && IsNonBaseActionCardFeatureEnabled(context))
			{
				Organization organization = ReadOrganizationSettings(context);
				if (organization.IsActionCardEnabled.GetValueOrDefault())
				{
					result = true;
				}
			}
			return result;
		}

		private bool IsNotOffline(IPluginContext context)
		{
			return !context.IsInClientContext;
		}

		private Organization ReadOrganizationSettings(IPluginContext context)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			ColumnSet columns = new ColumnSet(new string[2] { "isactioncardenabled", "ispreviewenabledforactioncard" });
			return context.SystemUserOrganizationService.Retrieve<Organization>(((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), columns);
		}
	}
}
