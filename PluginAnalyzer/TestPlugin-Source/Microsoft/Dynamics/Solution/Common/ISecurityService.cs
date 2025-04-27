using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xrm.Kernel.Contracts.Security;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public interface ISecurityService
	{
		void CheckAccess(EntityReference entityReference, EntityAction entityAction, IPluginContext context);

		void CheckAccess(EntityReference userReference, EntityReference entityReference, EntityAction entityAction, IPluginContext context);

		void CheckPrivilege(Guid userId, string entityTypeLogicalName, PrivilegeType privilegeType, IPluginContext context);

		void CheckPrivilege(Guid userId, string entityTypeLogicalName, string privilegeName, IPluginContext context);

		void CheckPrivilege(string entityTypeLogicalName, EntityAction entityAction, IPluginContext context);

		void MultipleRecordAccessCheck(IPluginContext context, string entityName, List<Guid> recordIds, EntityAction recordAction);
	}
}
