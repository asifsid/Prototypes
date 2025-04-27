using System;
using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[Flags]
	[ComVisible(true)]
	public enum TelemetryMode
	{
		All = 0x0,
		OperationEvents = 0x1,
		Usage = 0x2,
		Info = 0x3,
		Error = 0x4,
		Warning = 0x5,
		Debug = 0x6,
		Trace = 0x7,
		Critical = 0x8,
		Disabled = 0x9
	}
}
