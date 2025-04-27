using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public enum FullNameConventionCode
	{
		LastFirst,
		FirstLast,
		LastFirstMiddleInitial,
		FirstMiddleInitialLast,
		LastFirstMiddle,
		FirstMiddleLast,
		LastSpaceFirst,
		LastNoSpaceFirst
	}
}
