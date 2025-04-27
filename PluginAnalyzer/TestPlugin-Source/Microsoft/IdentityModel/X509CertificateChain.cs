using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.IdentityModel
{
	internal class X509CertificateChain
	{
		public const uint DefaultChainPolicyOID = 1u;

		private bool _useMachineContext;

		private X509ChainPolicy _chainPolicy;

		private uint _chainPolicyOID = 1u;

		public X509ChainPolicy ChainPolicy
		{
			get
			{
				if (_chainPolicy == null)
				{
					_chainPolicy = new X509ChainPolicy();
				}
				return _chainPolicy;
			}
			set
			{
				_chainPolicy = value;
			}
		}

		public X509ChainStatus[] ChainStatus
		{
			get
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
			}
		}

		public X509CertificateChain()
			: this(useMachineContext: false)
		{
		}

		public X509CertificateChain(bool useMachineContext)
		{
			_useMachineContext = useMachineContext;
		}

		public X509CertificateChain(bool useMachineContext, uint chainPolicyOID)
		{
			_useMachineContext = useMachineContext;
			_chainPolicyOID = chainPolicyOID;
		}

		public bool Build(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("certificate");
			}
			if (certificate.Handle == IntPtr.Zero)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("certificate", SR.GetString("ID4071"));
			}
			SafeCertChainHandle ppChainContext = SafeCertChainHandle.InvalidHandle;
			X509ChainPolicy chainPolicy = ChainPolicy;
			BuildChain(_useMachineContext ? new IntPtr(1L) : new IntPtr(0L), certificate.Handle, chainPolicy.ExtraStore, chainPolicy.ApplicationPolicy, chainPolicy.CertificatePolicy, chainPolicy.RevocationMode, chainPolicy.RevocationFlag, chainPolicy.VerificationTime, chainPolicy.UrlRetrievalTimeout, out ppChainContext);
			CryptoUtil.CAPI.CERT_CHAIN_POLICY_PARA pPolicyPara = new CryptoUtil.CAPI.CERT_CHAIN_POLICY_PARA(Marshal.SizeOf(typeof(CryptoUtil.CAPI.CERT_CHAIN_POLICY_PARA)));
			CryptoUtil.CAPI.CERT_CHAIN_POLICY_STATUS pPolicyStatus = new CryptoUtil.CAPI.CERT_CHAIN_POLICY_STATUS(Marshal.SizeOf(typeof(CryptoUtil.CAPI.CERT_CHAIN_POLICY_STATUS)));
			pPolicyPara.dwFlags = (uint)(chainPolicy.VerificationFlags | (X509VerificationFlags)4096);
			if (!CryptoUtil.CAPI.CertVerifyCertificateChainPolicy(new IntPtr(_chainPolicyOID), ppChainContext, ref pPolicyPara, ref pPolicyStatus))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(lastWin32Error));
			}
			if (pPolicyStatus.dwError != 0)
			{
				int dwError = (int)pPolicyStatus.dwError;
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4070", X509Util.GetCertificateId(certificate), new CryptographicException(dwError).Message)));
			}
			return true;
		}

		private unsafe static void BuildChain(IntPtr hChainEngine, IntPtr pCertContext, X509Certificate2Collection extraStore, OidCollection applicationPolicy, OidCollection certificatePolicy, X509RevocationMode revocationMode, X509RevocationFlag revocationFlag, DateTime verificationTime, TimeSpan timeout, out SafeCertChainHandle ppChainContext)
		{
			SafeCertStoreHandle safeCertStoreHandle = ExportToMemoryStore(extraStore, pCertContext);
			CryptoUtil.CAPI.CERT_CHAIN_PARA pChainPara = default(CryptoUtil.CAPI.CERT_CHAIN_PARA);
			pChainPara.cbSize = (uint)Marshal.SizeOf(typeof(CryptoUtil.CAPI.CERT_CHAIN_PARA));
			SafeHGlobalHandle safeHGlobalHandle = SafeHGlobalHandle.InvalidHandle;
			SafeHGlobalHandle safeHGlobalHandle2 = SafeHGlobalHandle.InvalidHandle;
			try
			{
				if (applicationPolicy != null && applicationPolicy.Count > 0)
				{
					pChainPara.RequestedUsage.dwType = 0u;
					pChainPara.RequestedUsage.Usage.cUsageIdentifier = (uint)applicationPolicy.Count;
					safeHGlobalHandle = CopyOidsToUnmanagedMemory(applicationPolicy);
					pChainPara.RequestedUsage.Usage.rgpszUsageIdentifier = safeHGlobalHandle.DangerousGetHandle();
				}
				if (certificatePolicy != null && certificatePolicy.Count > 0)
				{
					pChainPara.RequestedIssuancePolicy.dwType = 0u;
					pChainPara.RequestedIssuancePolicy.Usage.cUsageIdentifier = (uint)certificatePolicy.Count;
					safeHGlobalHandle2 = CopyOidsToUnmanagedMemory(certificatePolicy);
					pChainPara.RequestedIssuancePolicy.Usage.rgpszUsageIdentifier = safeHGlobalHandle2.DangerousGetHandle();
				}
				pChainPara.dwUrlRetrievalTimeout = (uint)timeout.Milliseconds;
				System.Runtime.InteropServices.ComTypes.FILETIME pTime = default(System.Runtime.InteropServices.ComTypes.FILETIME);
				*(long*)(&pTime) = verificationTime.ToFileTime();
				uint dwFlags = MapRevocationFlags(revocationMode, revocationFlag);
				if (!CryptoUtil.CAPI.CertGetCertificateChain(hChainEngine, pCertContext, ref pTime, safeCertStoreHandle, ref pChainPara, dwFlags, IntPtr.Zero, out ppChainContext))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(lastWin32Error));
				}
			}
			finally
			{
				safeHGlobalHandle?.Dispose();
				safeHGlobalHandle2?.Dispose();
				safeCertStoreHandle.Close();
			}
		}

		private static SafeCertStoreHandle ExportToMemoryStore(X509Certificate2Collection collection, IntPtr pCertContext)
		{
			CryptoUtil.CAPI.CERT_CONTEXT cERT_CONTEXT = (CryptoUtil.CAPI.CERT_CONTEXT)Marshal.PtrToStructure(pCertContext, typeof(CryptoUtil.CAPI.CERT_CONTEXT));
			if ((collection == null || collection.Count <= 0) && cERT_CONTEXT.hCertStore == IntPtr.Zero)
			{
				return SafeCertStoreHandle.InvalidHandle;
			}
			SafeCertStoreHandle safeCertStoreHandle = CryptoUtil.CAPI.CertOpenStore(new IntPtr(2L), 65537u, IntPtr.Zero, 8704u, null);
			if (safeCertStoreHandle == null || safeCertStoreHandle.IsInvalid)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(lastWin32Error));
			}
			if (collection != null && collection.Count > 0)
			{
				X509Certificate2Enumerator enumerator = collection.GetEnumerator();
				while (enumerator.MoveNext())
				{
					X509Certificate2 current = enumerator.Current;
					if (!CryptoUtil.CAPI.CertAddCertificateLinkToStore(safeCertStoreHandle, current.Handle, 4u, SafeCertContextHandle.InvalidHandle))
					{
						int lastWin32Error2 = Marshal.GetLastWin32Error();
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(lastWin32Error2));
					}
				}
			}
			using SafeCertContextHandle safeCertContextHandle = CryptoUtil.CAPI.CertCreateCertificateContext(cERT_CONTEXT.dwCertEncodingType, cERT_CONTEXT.pbCertEncoded, cERT_CONTEXT.cbCertEncoded);
			X509Certificate2 x509Certificate = new X509Certificate2(safeCertContextHandle.DangerousGetHandle());
			CryptoUtil.CAPI.CERT_CONTEXT cERT_CONTEXT2 = (CryptoUtil.CAPI.CERT_CONTEXT)Marshal.PtrToStructure(x509Certificate.Handle, typeof(CryptoUtil.CAPI.CERT_CONTEXT));
			if (cERT_CONTEXT2.hCertStore != IntPtr.Zero)
			{
				X509Certificate2Collection x509Certificate2Collection = null;
				X509Store x509Store = new X509Store(cERT_CONTEXT2.hCertStore);
				try
				{
					x509Certificate2Collection = x509Store.Certificates;
					X509Certificate2Enumerator enumerator2 = x509Certificate2Collection.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						X509Certificate2 current2 = enumerator2.Current;
						if (!CryptoUtil.CAPI.CertAddCertificateLinkToStore(safeCertStoreHandle, current2.Handle, 4u, SafeCertContextHandle.InvalidHandle))
						{
							int lastWin32Error3 = Marshal.GetLastWin32Error();
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(lastWin32Error3));
						}
					}
					return safeCertStoreHandle;
				}
				finally
				{
					CryptoUtil.ResetAllCertificates(x509Certificate2Collection);
					x509Store.Close();
				}
			}
			return safeCertStoreHandle;
		}

		private static SafeHGlobalHandle CopyOidsToUnmanagedMemory(OidCollection oids)
		{
			SafeHGlobalHandle invalidHandle = SafeHGlobalHandle.InvalidHandle;
			if (oids == null || oids.Count == 0)
			{
				return invalidHandle;
			}
			int num;
			int num2;
			checked
			{
				num = oids.Count * Marshal.SizeOf(typeof(IntPtr));
				num2 = 0;
				OidEnumerator enumerator = oids.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Oid current = enumerator.Current;
					num2 += current.Value.Length + 1;
				}
			}
			invalidHandle = SafeHGlobalHandle.AllocHGlobal(num + num2);
			IntPtr intPtr = new IntPtr((long)invalidHandle.DangerousGetHandle() + num);
			for (int i = 0; i < oids.Count; i++)
			{
				Marshal.WriteIntPtr(new IntPtr((long)invalidHandle.DangerousGetHandle() + i * Marshal.SizeOf(typeof(IntPtr))), intPtr);
				byte[] bytes = Encoding.ASCII.GetBytes(oids[i].Value);
				Marshal.Copy(bytes, 0, intPtr, bytes.Length);
				intPtr = new IntPtr((long)intPtr + oids[i].Value.Length + 1);
			}
			return invalidHandle;
		}

		private static uint MapRevocationFlags(X509RevocationMode revocationMode, X509RevocationFlag revocationFlag)
		{
			uint num = 0u;
			switch (revocationMode)
			{
			case X509RevocationMode.NoCheck:
				return num;
			case X509RevocationMode.Offline:
				num |= 0x80000000u;
				break;
			}
			return revocationFlag switch
			{
				X509RevocationFlag.EndCertificateOnly => num | 0x10000000u, 
				X509RevocationFlag.EntireChain => num | 0x20000000u, 
				_ => num | 0x40000000u, 
			};
		}
	}
}
