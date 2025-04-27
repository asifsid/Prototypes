using System.Runtime.InteropServices;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public static class MoneyExtensions
	{
		public static decimal GetValueOrDefault(this Money money)
		{
			if (money == null)
			{
				return 0m;
			}
			return money.get_Value();
		}
	}
}
