using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class QueryScanFilter : PathFilter
	{
		public QueryExpression Expression { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				JContainer jContainer;
				if ((jContainer = t as JContainer) != null)
				{
					foreach (JToken item in jContainer.DescendantsAndSelf())
					{
						if (Expression.IsMatch(root, item))
						{
							yield return item;
						}
					}
				}
				else if (Expression.IsMatch(root, t))
				{
					yield return t;
				}
			}
		}
	}
}
