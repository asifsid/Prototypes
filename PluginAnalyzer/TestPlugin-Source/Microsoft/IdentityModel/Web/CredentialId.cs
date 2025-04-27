using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public static class CredentialId
	{
		public static string CreateFriendlyPPID(string rawPPID)
		{
			if (string.IsNullOrEmpty(rawPPID))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("rawPPID");
			}
			byte[] buffer;
			try
			{
				buffer = Convert.FromBase64String(rawPPID);
			}
			catch (FormatException innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3126", rawPPID), innerException));
			}
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array;
			using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.NewSha1())
			{
				array = hashAlgorithm.ComputeHash(buffer);
			}
			char[] array2 = "QL23456789ABCDEFGHJKMNPRSTUVWXYZ".ToCharArray();
			int length = "QL23456789ABCDEFGHJKMNPRSTUVWXYZ".Length;
			for (int i = 0; i < 10; i++)
			{
				if (i == 3 || i == 7)
				{
					stringBuilder.Append('-');
				}
				stringBuilder.Append(array2[(int)array[i] % length]);
			}
			return stringBuilder.ToString();
		}
	}
}
