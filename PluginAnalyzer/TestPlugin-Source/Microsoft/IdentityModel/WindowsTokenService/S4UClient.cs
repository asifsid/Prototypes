using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.IdentityModel.WindowsTokenService
{
	[ComVisible(true)]
	public static class S4UClient
	{
		[ServiceContract(Namespace = "http://schemas.microsoft.com/ws/2008/06/identity/wts")]
		private interface IS4UService_dup
		{
			[OperationContract(Action = "urn:IS4UService-UpnLogon", ReplyAction = "urn:IS4UService-UpnLogon-Response")]
			IntPtr UpnLogon(string upn, int pid);

			[OperationContract(Action = "urn:IS4UService-CertificateLogon", ReplyAction = "urn:IS4UService-CertificateLogon-Response")]
			IntPtr CertificateLogon(byte[] certData, int pid);
		}

		private class SafeKernelObjectHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			public SafeKernelObjectHandle()
				: base(ownsHandle: true)
			{
			}

			public SafeKernelObjectHandle(IntPtr handle)
				: this(handle, takeOwnership: true)
			{
			}

			public SafeKernelObjectHandle(IntPtr handle, bool takeOwnership)
				: base(takeOwnership)
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

		private static ChannelFactory<IS4UService_dup> _channelFactory;

		static S4UClient()
		{
			NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
			string remoteAddress = new UriBuilder
			{
				Scheme = Uri.UriSchemeNetPipe,
				Host = "localhost",
				Path = "/s4u/022694f3-9fbd-422b-b4b2-312e25dae2a2"
			}.Uri.ToString();
			_channelFactory = new ChannelFactory<IS4UService_dup>(binding, remoteAddress);
		}

		public static WindowsIdentity UpnLogon(string upn)
		{
			return CallService((IS4UService_dup channel) => channel.UpnLogon(upn, Process.GetCurrentProcess().Id));
		}

		public static WindowsIdentity CertificateLogon(X509Certificate2 certificate)
		{
			return CallService((IS4UService_dup channel) => channel.CertificateLogon(certificate.RawData, Process.GetCurrentProcess().Id));
		}

		private static WindowsIdentity CallService(Func<IS4UService_dup, IntPtr> contractOperation)
		{
			IS4UService_dup iS4UService_dup = _channelFactory.CreateChannel();
			ICommunicationObject communicationObject = (ICommunicationObject)iS4UService_dup;
			bool flag = false;
			try
			{
				IntPtr intPtr = contractOperation(iS4UService_dup);
				using (new SafeKernelObjectHandle(intPtr, takeOwnership: true))
				{
					communicationObject.Close();
					flag = true;
					return new WindowsIdentity(intPtr);
				}
			}
			finally
			{
				if (!flag)
				{
					communicationObject.Abort();
				}
			}
		}
	}
}
