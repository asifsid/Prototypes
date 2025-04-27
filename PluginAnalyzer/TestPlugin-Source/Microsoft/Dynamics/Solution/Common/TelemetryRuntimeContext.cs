using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public class TelemetryRuntimeContext
	{
		public string CallingMethod { get; set; }

		public Guid OrgId { get; set; }
	}
}
