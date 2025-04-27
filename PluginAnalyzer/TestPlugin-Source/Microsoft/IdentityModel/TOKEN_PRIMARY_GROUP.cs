using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct TOKEN_PRIMARY_GROUP
	{
		internal IntPtr PrimaryGroup;
	}
}
