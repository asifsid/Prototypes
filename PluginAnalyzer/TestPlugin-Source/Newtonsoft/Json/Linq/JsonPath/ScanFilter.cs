using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
	internal class ScanFilter : PathFilter
	{
		public string Name { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken c in current)
			{
				if (Name == null)
				{
					yield return c;
				}
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
					if ((jProperty = value as JProperty) != null)
					{
						if (jProperty.Name == Name)
						{
							yield return jProperty.Value;
						}
					}
					else if (Name == null)
					{
						yield return value;
					}
				}
			}
		}
	}
}
