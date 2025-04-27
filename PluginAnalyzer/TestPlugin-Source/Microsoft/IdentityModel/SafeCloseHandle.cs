using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.IdentityModel
{
	internal sealed class SafeCloseHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private const string KERNEL32 = "kernel32.dll";

		private SafeCloseHandle()
			: base(ownsHandle: true)
		{
		}

		internal SafeCloseHandle(IntPtr handle, bool ownsHandle)
			: base(ownsHandle)
		{
			SetHandle(handle);
		}

		protected override bool ReleaseHandle()
		{
			return CloseHandle(handle);
		}

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(IntPtr handle);
	}
}
