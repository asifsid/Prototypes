using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class JobCollectionBody
	{
		public string Location { get; set; }

		public JobCollectionProperties Properties { get; set; }
	}
}
