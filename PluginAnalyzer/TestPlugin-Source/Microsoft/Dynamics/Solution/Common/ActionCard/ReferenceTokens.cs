using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common.ActionCard
{
	[ComVisible(true)]
	public class ReferenceTokens
	{
		public Dictionary<string, Token> Tokens;

		public ReferenceTokens()
		{
			Tokens = new Dictionary<string, Token>();
		}
	}
}
