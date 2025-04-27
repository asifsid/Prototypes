using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct UNICODE_INTPTR_STRING
	{
		internal ushort Length;

		internal ushort MaxLength;

		internal IntPtr Buffer;

		internal UNICODE_INTPTR_STRING(int length, int maximumLength, IntPtr buffer)
		{
			Length = (ushort)length;
			MaxLength = (ushort)maximumLength;
			Buffer = buffer;
		}
	}
}
