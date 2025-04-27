using System.Runtime.InteropServices;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class QueryExpressionHelper
	{
		public static QueryExpression DeserializeFromFetchXml(string xml, IOrganizationService organizationService)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			FetchXmlToQueryExpressionRequest val = new FetchXmlToQueryExpressionRequest();
			val.set_FetchXml(xml);
			FetchXmlToQueryExpressionRequest val2 = val;
			FetchXmlToQueryExpressionResponse val3 = (FetchXmlToQueryExpressionResponse)organizationService.Execute((OrganizationRequest)(object)val2);
			return val3.get_Query();
		}
	}
}
