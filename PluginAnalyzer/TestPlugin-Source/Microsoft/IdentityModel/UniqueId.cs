using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.IdentityModel
{
	[ComVisible(true)]
	public static class UniqueId
	{
		private const int RandomSaltSize = 16;

		private const string NcNamePrefix = "_";

		private const string UuidUriPrefix = "urn:uuid:";

		private static readonly string _reusableUuid = GetRandomUuid();

		private static readonly string _optimizedNcNamePrefix = "_" + _reusableUuid + "-";

		public static string CreateUniqueId()
		{
			return _optimizedNcNamePrefix + GetNextId();
		}

		public static string CreateUniqueId(string prefix)
		{
			if (string.IsNullOrEmpty(prefix))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("prefix");
			}
			return prefix + _reusableUuid + "-" + GetNextId();
		}

		public static string CreateRandomId()
		{
			return "_" + GetRandomUuid();
		}

		public static string CreateRandomId(string prefix)
		{
			if (string.IsNullOrEmpty(prefix))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("prefix");
			}
			return prefix + GetRandomUuid();
		}

		public static string CreateRandomUri()
		{
			return "urn:uuid:" + GetRandomUuid();
		}

		private static string GetNextId()
		{
			RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
			byte[] array = new byte[16];
			randomNumberGenerator.GetBytes(array);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.AppendFormat("{0:X2}", array[i]);
			}
			return stringBuilder.ToString();
		}

		private static string GetRandomUuid()
		{
			return Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
		}
	}
}
