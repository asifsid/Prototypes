using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public class Parameter
	{
		public string Name { get; set; }

		public int TypeCode { get; set; }

		public string Id { get; set; }

		public ParamType Type { get; set; }
	}
}
