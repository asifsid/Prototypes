using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ScanMultipleFilter : PathFilter
	{
		public List<string> Names { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken c in current)
			{
				JToken value = c;
				while (true)
				{
					JContainer container = value as JContainer;
					value = PathFilter.GetNextScanValue(c, container, value);
					if (value == null)
					{
						break;
					}
					JProperty jProperty;
					JProperty property = (jProperty = value as JProperty);
					if (jProperty == null)
					{
						continue;
					}
					foreach (string name in Names)
					{
						if (property.Name == name)
						{
							yield return property.Value;
						}
					}
				}
			}
		}
	}
}
