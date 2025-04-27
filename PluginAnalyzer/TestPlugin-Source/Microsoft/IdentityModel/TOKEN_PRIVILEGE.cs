using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel
{
	internal struct TOKEN_PRIVILEGE
	{
		internal uint PrivilegeCount;

		internal LUID_AND_ATTRIBUTES Privilege;

		internal static readonly uint Size = (uint)Marshal.SizeOf(typeof(TOKEN_PRIVILEGE));
	}
}
