using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct TOKEN_SOURCE
	{
		private const int TOKEN_SOURCE_LENGTH = 8;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		internal char[] Name;

		internal LUID SourceIdentifier;
	}
}
