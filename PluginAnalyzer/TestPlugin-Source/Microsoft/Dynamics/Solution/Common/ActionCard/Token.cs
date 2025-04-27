using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public class Token
	{
		public List<Parameter> Params;

		public string ResourceId { get; set; }

		public Token()
		{
			Params = new List<Parameter>();
		}
	}
}
