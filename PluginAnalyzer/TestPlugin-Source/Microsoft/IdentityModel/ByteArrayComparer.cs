using System.Collections.Generic;

namespace Microsoft.IdentityModel
{
	internal class ByteArrayComparer : IEqualityComparer<byte[]>
	{
		private static ByteArrayComparer _instance = new ByteArrayComparer();

		public static ByteArrayComparer Instance => _instance;

		private ByteArrayComparer()
		{
		}

		public bool Equals(byte[] x, byte[] y)
		{
			return CryptoUtil.AreEqual(x, y);
		}

		public int GetHashCode(byte[] obj)
		{
			int num = 0;
			for (int i = 0; i < obj.Length && i < 4; i++)
			{
				num = (num << 8) | obj[i];
			}
			return num ^ obj.Length;
		}
	}
}
