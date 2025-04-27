using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public abstract class CookieTransform
	{
		public abstract byte[] Decode(byte[] encoded);

		public abstract byte[] Encode(byte[] value);
	}
}
