using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Dynamics.Solution.Common.Proxies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class DeploymentService
	{
		private readonly Dictionary<string, Guid> publisherNameMapping = new Dictionary<string, Guid>();

		public Action<string> Logger { get; set; } = Console.WriteLine;


		public bool IsSolutionImported(IOrganizationService organizationService, string solutionUniqueName, string publisherName = null)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Expected O, but got Unknown
			Guid guid = Guid.Empty;
			QueryExpression val;
			if (!string.IsNullOrEmpty(publisherName))
			{
				if (!publisherNameMapping.ContainsKey(publisherName))
				{
					val = new QueryExpression("publisher");
					val.set_ColumnSet(new ColumnSet(new string[1] { "publisherid" }));
					QueryExpression val2 = val;
					val2.get_Criteria().AddCondition("uniquename", (ConditionOperator)0, new object[1] { publisherName });
					EntityCollection val3 = organizationService.RetrieveMultiple((QueryBase)(object)val2);
					if (val3 != null && val3.get_Entities() != null && ((Collection<Entity>)(object)val3.get_Entities()).Count > 0)
					{
						Entity val4 = ((Collection<Entity>)(object)val3.get_Entities())[0];
						guid = val4.get_Id();
						publisherNameMapping[publisherName] = guid;
					}
				}
				else
				{
					guid = publisherNameMapping[publisherName];
				}
			}
			val = new QueryExpression("solution");
			val.set_ColumnSet(new ColumnSet(new string[1] { "solutionid" }));
			QueryExpression val5 = val;
			val5.get_Criteria().AddCondition("uniquename", (ConditionOperator)0, new object[1] { solutionUniqueName ?? string.Empty });
			if (guid != Guid.Empty)
			{
				val5.get_Criteria().AddCondition("publisherid", (ConditionOperator)0, new object[1] { guid });
			}
			EntityCollection val6 = organizationService.RetrieveMultiple((QueryBase)(object)val5);
			bool result = false;
			if (val6 != null && val6.get_Entities() != null)
			{
				result = ((Collection<Entity>)(object)val6.get_Entities()).Count == 1;
			}
			return result;
		}

		public Microsoft.Dynamics.Solution.Common.Proxies.Solution GetSolution(IOrganizationService organizationService, string uniqueName, string[] columns = null)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			ColumnSet val = new ColumnSet(new string[3] { "solutionid", "uniquename", "version" });
			QueryExpression val2 = new QueryExpression("solution");
			val2.set_ColumnSet((ColumnSet)((columns != null) ? ((object)new ColumnSet(columns)) : ((object)val)));
			val2.get_Criteria().AddCondition("uniquename", (ConditionOperator)0, new object[1] { uniqueName });
			val2.set_TopCount((int?)1);
			EntityCollection val3 = organizationService.RetrieveMultiple((QueryBase)(object)val2);
			Microsoft.Dynamics.Solution.Common.Proxies.Solution result = null;
			if (val3 != null && val3.get_Entities() != null)
			{
				result = ((Collection<Entity>)(object)val3.get_Entities())[0].ToEntity<Microsoft.Dynamics.Solution.Common.Proxies.Solution>();
			}
			return result;
		}

		public bool DeleteSolution(IOrganizationService organizationService, string uniqueName)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			QueryExpression val = new QueryExpression("solution");
			val.set_ColumnSet(new ColumnSet(new string[2] { "solutionid", "friendlyname" }));
			val.get_Criteria().AddCondition("uniquename", (ConditionOperator)0, new object[1] { uniqueName });
			EntityCollection val2 = organizationService.RetrieveMultiple((QueryBase)(object)val);
			DataCollection<Entity> val3 = ((val2 != null) ? val2.get_Entities() : null);
			Microsoft.Dynamics.Solution.Common.Proxies.Solution solution = ((val3 != null && ((Collection<Entity>)(object)val3).Count > 0) ? ((Collection<Entity>)(object)val3)[0].ToEntity<Microsoft.Dynamics.Solution.Common.Proxies.Solution>() : null);
			if (solution == null)
			{
				Logger("Solution " + uniqueName + " was not found.");
				return false;
			}
			organizationService.Delete("solution", solution.SolutionId.Value);
			Logger("Deleted the " + solution.FriendlyName + " solution.");
			return true;
		}
	}
}
