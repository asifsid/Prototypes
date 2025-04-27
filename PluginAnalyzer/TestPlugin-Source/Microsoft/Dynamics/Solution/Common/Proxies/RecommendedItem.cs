using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.Proxies
{
	[ComVisible(true)]
	public sealed class RecommendedItem
	{
		public string Name { get; set; }

		public double Rating { get; set; }

		public string Reasoning { get; set; }

		public string Id { get; set; }
	}
}
