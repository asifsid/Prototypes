using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public enum EventType
	{
		Info,
		Error,
		Warning,
		Debug,
		Trace,
		Critical,
		Usage
	}
}
