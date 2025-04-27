using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class CrmPricingException : Exception
	{
		private int errorCode = 0;

		public int PricingError => errorCode;

		public CrmPricingException(int errCode)
		{
			errorCode = errCode;
		}
	}
}
