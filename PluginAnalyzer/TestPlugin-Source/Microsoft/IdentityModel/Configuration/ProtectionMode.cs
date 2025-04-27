using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Configuration
{
	[ComVisible(true)]
	public enum ProtectionMode
	{
		Default,
		Dpapi,
		Rsa,
		None
	}
}
