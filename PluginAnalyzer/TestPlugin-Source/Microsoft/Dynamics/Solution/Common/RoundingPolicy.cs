using System.Runtime.InteropServices;

namespace Microsoft.Dynamics.Solution.Common
{
	[ComVisible(true)]
	public sealed class RoundingPolicy
	{
		public const int None = 1;

		public const int Up = 2;

		public const int Down = 3;

		public const int Nearest = 4;
	}
}
