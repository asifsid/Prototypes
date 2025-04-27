using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public static class ActionCardServiceFactory
	{
		public static NonActivityActionCardService CreateAccountService(IPluginContext context)
		{
			return new NonActivityActionCardService(ActionCardTypes.NonActivityWithAccount, 1, (Entity E) => E.GetAttributeValue<string>("name") ?? string.Empty, "ActionCard.NOActivityWithAccount.body", 0);
		}

		public static NonActivityActionCardService CreateContactService(IPluginContext context)
		{
			return new NonActivityActionCardService(ActionCardTypes.NonActivityWithContact, 2, delegate(Entity E)
			{
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Expected O, but got Unknown
				FullNameConventionCode fullNameConventionCode = FullNameConventionCode.LastFirst;
				Organization organization = context.SystemUserOrganizationService.Retrieve<Organization>(((IExecutionContext)context.PluginExecutionContext).get_OrganizationId(), new ColumnSet(new string[1] { "fullnameconventioncode" }));
				if (organization.FullNameConventionCode != null)
				{
					fullNameConventionCode = (FullNameConventionCode)organization.FullNameConventionCode.get_Value();
				}
				return FullNameGenerator.GenerateFullName(fullNameConventionCode, E.GetAttributeValue<string>("firstname"), string.Empty, E.GetAttributeValue<string>("lastname"));
			}, "ActionCard.NOActivityWithContact.body", 0);
		}
	}
}
