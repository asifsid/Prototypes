using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public sealed class ProtectedDataCookieTransform : CookieTransform
	{
		public override byte[] Decode(byte[] encoded)
		{
			if (encoded == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("encoded");
			}
			if (encoded.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("encoded", SR.GetString("ID6045"));
			}
			try
			{
				return ProtectedData.Unprotect(encoded, null, DataProtectionScope.CurrentUser);
			}
			catch (CryptographicException innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1073"), innerException));
			}
		}

		public override byte[] Encode(byte[] value)
		{
			if (value == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
			}
			if (value.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID6044"));
			}
			try
			{
				return ProtectedData.Protect(value, null, DataProtectionScope.CurrentUser);
			}
			catch (CryptographicException innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1074"), innerException));
			}
		}
	}
}
