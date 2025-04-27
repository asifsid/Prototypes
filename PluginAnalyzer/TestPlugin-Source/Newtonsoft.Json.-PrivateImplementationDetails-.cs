using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[CompilerGenerated]
internal sealed class Newtonsoft_002EJson_002E_003CPrivateImplementationDetails_003E
{
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 6)]
	private struct __StaticArrayInitTypeSize_003D6
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 10)]
	private struct __StaticArrayInitTypeSize_003D10
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 28)]
	private struct __StaticArrayInitTypeSize_003D28
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 36)]
	private struct __StaticArrayInitTypeSize_003D36
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 52)]
	private struct __StaticArrayInitTypeSize_003D52
	{
	}

	internal static readonly __StaticArrayInitTypeSize_003D6 _3DE43C11C7130AF9014115BCDC2584DFE6B50579/* Not supported: data(2E 00 45 00 65 00) */;

	internal static readonly __StaticArrayInitTypeSize_003D28 _9E31F24F64765FCAA589F589324D17C9FCF6A06D/* Not supported: data(FF FF FF FF 0A 00 00 00 64 00 00 00 E8 03 00 00 10 27 00 00 A0 86 01 00 40 42 0F 00) */;

	internal static readonly __StaticArrayInitTypeSize_003D10 D40004AB0E92BF6C8DFE481B56BE3D04ABDA76EB/* Not supported: data(22 00 27 00 3C 00 3E 00 26 00) */;

	internal static readonly __StaticArrayInitTypeSize_003D52 DD3AEFEADB1CD615F3017763F1568179FEE640B0/* Not supported: data(00 00 00 00 1F 00 00 00 3C 00 00 00 5B 00 00 00 79 00 00 00 98 00 00 00 B6 00 00 00 D5 00 00 00 F4 00 00 00 12 01 00 00 31 01 00 00 4F 01 00 00 6E 01 00 00) */;

	internal static readonly __StaticArrayInitTypeSize_003D36 E289D9D3D233BC253E8C0FA8C2AFDD86A407CE30/* Not supported: data(2E 00 20 00 27 00 2F 00 22 00 5B 00 5D 00 28 00 29 00 09 00 0A 00 0D 00 0C 00 08 00 5C 00 85 00 28 20 29 20) */;

	internal static readonly __StaticArrayInitTypeSize_003D52 E92B39D8233061927D9ACDE54665E68E7535635A/* Not supported: data(00 00 00 00 1F 00 00 00 3B 00 00 00 5A 00 00 00 78 00 00 00 97 00 00 00 B5 00 00 00 D4 00 00 00 F3 00 00 00 11 01 00 00 30 01 00 00 4E 01 00 00 6D 01 00 00) */;

	internal static uint ComputeStringHash(string s)
	{
		uint num = default(uint);
		if (s != null)
		{
			num = 2166136261u;
			for (int i = 0; i < s.Length; i++)
			{
				num = (s[i] ^ num) * 16777619;
			}
		}
		return num;
	}
}
