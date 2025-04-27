using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class CrmNotSupportedException : CrmException
	{
		public CrmNotSupportedException(string message)
			: base(message, -2147220989)
		{
		}
	}
}
