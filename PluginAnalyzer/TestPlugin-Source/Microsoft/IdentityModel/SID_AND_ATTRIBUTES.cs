using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct SID_AND_ATTRIBUTES
	{
		internal IntPtr Sid;

		internal uint Attributes;

		internal static readonly long SizeOf = Marshal.SizeOf(typeof(SID_AND_ATTRIBUTES));
	}
}
