using System;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using Microsoft.Win32;

namespace Microsoft.IdentityModel
{
	internal static class CryptoUtil
	{
		private enum FIPS_ALGORTITHM_POLICY
		{
			Unknown,
			Enabled,
			Disabled
		}

		public static class Algorithms
		{
			[SecuritySafeCritical]
			internal static bool RequiresFipsCompliance
			{
				get
				{
					if (_fipsPolicyState == FIPS_ALGORTITHM_POLICY.Unknown)
					{
						lock (_syncObject)
						{
							if (_fipsPolicyState == FIPS_ALGORTITHM_POLICY.Unknown)
							{
								if (Environment.OSVersion.Version.Major >= 6)
								{
									if (0 == CAPI.BCryptGetFipsAlgorithmMode(out var pfEnabled) && pfEnabled)
									{
										_fipsPolicyState = FIPS_ALGORTITHM_POLICY.Enabled;
									}
									else
									{
										_fipsPolicyState = FIPS_ALGORTITHM_POLICY.Disabled;
									}
								}
								else
								{
									_fipsPolicyState = GetFipsAlgorithmPolicyKeyFromRegistry();
								}
							}
						}
					}
					return _fipsPolicyState == FIPS_ALGORTITHM_POLICY.Enabled;
				}
			}

			public static HashAlgorithm NewDefaultHash()
			{
				return NewSha256();
			}

			public static SymmetricAlgorithm NewDefaultEncryption()
			{
				SymmetricAlgorithm symmetricAlgorithm = CreateAlgorithmFromConfig("http://www.w3.org/2001/04/xmlenc#aes256-cbc") as SymmetricAlgorithm;
				if (symmetricAlgorithm == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("algorithm", SR.GetString("ID6037", "http://www.w3.org/2000/09/xmldsig#hmac-sha1"));
				}
				return symmetricAlgorithm;
			}

			public static KeyedHashAlgorithm NewHmacSha1()
			{
				KeyedHashAlgorithm keyedHashAlgorithm = CreateAlgorithmFromConfig("http://www.w3.org/2000/09/xmldsig#hmac-sha1") as KeyedHashAlgorithm;
				if (keyedHashAlgorithm == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("algorithm", SR.GetString("ID6037", "http://www.w3.org/2000/09/xmldsig#hmac-sha1"));
				}
				return keyedHashAlgorithm;
			}

			public static RandomNumberGenerator NewRandomNumberGenerator()
			{
				return RandomNumberGenerator.Create();
			}

			public static RSA NewRsa()
			{
				return RSA.Create();
			}

			public static HashAlgorithm NewSha1()
			{
				HashAlgorithm hashAlgorithm = CreateHashAlgorithm("http://www.w3.org/2000/09/xmldsig#sha1");
				if (hashAlgorithm == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("algorithm", SR.GetString("ID6037", "http://www.w3.org/2000/09/xmldsig#sha1"));
				}
				return hashAlgorithm;
			}

			public static HashAlgorithm NewSha256()
			{
				HashAlgorithm hashAlgorithm = CreateHashAlgorithm("http://www.w3.org/2001/04/xmlenc#sha256");
				if (hashAlgorithm == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("algorithm", SR.GetString("ID6037", "http://www.w3.org/2001/04/xmlenc#sha256"));
				}
				return hashAlgorithm;
			}

			public static HashAlgorithm CreateHashAlgorithm(string algorithm)
			{
				if (string.IsNullOrEmpty(algorithm))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("algorithm");
				}
				object obj = CreateAlgorithmFromConfig(algorithm);
				if (obj != null)
				{
					SignatureDescription signatureDescription = obj as SignatureDescription;
					HashAlgorithm hashAlgorithm;
					if (signatureDescription != null)
					{
						hashAlgorithm = signatureDescription.CreateDigest();
						if (hashAlgorithm == null)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6011", algorithm)));
						}
						return hashAlgorithm;
					}
					hashAlgorithm = obj as HashAlgorithm;
					if (hashAlgorithm != null)
					{
						return hashAlgorithm;
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6022", algorithm)));
				}
				return algorithm switch
				{
					"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256" => NewSha256(), 
					"http://www.w3.org/2000/09/xmldsig#rsa-sha1" => NewSha1(), 
					_ => throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6022", algorithm))), 
				};
			}

			public static object CreateAlgorithmFromConfig(string algorithm)
			{
				if (string.IsNullOrEmpty(algorithm))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("algorithm");
				}
				object obj = null;
				try
				{
					obj = CryptoConfig.CreateFromName(algorithm);
				}
				catch (TargetInvocationException ex)
				{
					if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
					{
						DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID6038", algorithm, ex.InnerException));
					}
				}
				if (obj != null)
				{
					return obj;
				}
				switch (algorithm)
				{
				case "http://www.w3.org/2001/04/xmlenc#sha256":
					obj = ((!RequiresFipsCompliance) ? HashAlgorithm.Create("http://www.w3.org/2001/04/xmlenc#sha256") : new SHA256CryptoServiceProvider());
					break;
				case "http://www.w3.org/2000/09/xmldsig#sha1":
					obj = ((!RequiresFipsCompliance) ? SHA1.Create() : new SHA1CryptoServiceProvider());
					break;
				case "http://www.w3.org/2000/09/xmldsig#hmac-sha1":
					obj = new HMACSHA1(GenerateRandomBytes(64), !RequiresFipsCompliance);
					break;
				}
				return obj;
			}
		}

		[SuppressUnmanagedCodeSecurity]
		public static class CAPI
		{
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct CERT_CONTEXT
			{
				internal uint dwCertEncodingType;

				internal IntPtr pbCertEncoded;

				internal uint cbCertEncoded;

				internal IntPtr pCertInfo;

				internal IntPtr hCertStore;
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct CERT_ENHKEY_USAGE
			{
				internal uint cUsageIdentifier;

				internal IntPtr rgpszUsageIdentifier;
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct CERT_USAGE_MATCH
			{
				internal uint dwType;

				internal CERT_ENHKEY_USAGE Usage;
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct CERT_CHAIN_PARA
			{
				internal uint cbSize;

				internal CERT_USAGE_MATCH RequestedUsage;

				internal CERT_USAGE_MATCH RequestedIssuancePolicy;

				internal uint dwUrlRetrievalTimeout;

				internal bool fCheckRevocationFreshnessTime;

				internal uint dwRevocationFreshnessTime;
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct CERT_CHAIN_POLICY_PARA
			{
				internal uint cbSize;

				internal uint dwFlags;

				internal IntPtr pvExtraPolicyPara;

				internal CERT_CHAIN_POLICY_PARA(int size)
				{
					cbSize = (uint)size;
					dwFlags = 0u;
					pvExtraPolicyPara = IntPtr.Zero;
				}
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			internal struct CERT_CHAIN_POLICY_STATUS
			{
				internal uint cbSize;

				internal uint dwError;

				internal IntPtr lChainIndex;

				internal IntPtr lElementIndex;

				internal IntPtr pvExtraPolicyStatus;

				internal CERT_CHAIN_POLICY_STATUS(int size)
				{
					cbSize = (uint)size;
					dwError = 0u;
					lChainIndex = IntPtr.Zero;
					lElementIndex = IntPtr.Zero;
					pvExtraPolicyStatus = IntPtr.Zero;
				}
			}

			internal const string CRYPT32 = "crypt32.dll";

			internal const string BCRYPT = "bcrypt.dll";

			internal const int S_OK = 0;

			internal const int S_FALSE = 1;

			internal const uint CERT_STORE_ENUM_ARCHIVED_FLAG = 512u;

			internal const uint CERT_STORE_CREATE_NEW_FLAG = 8192u;

			internal const uint CERT_CHAIN_POLICY_BASE = 1u;

			internal const uint CERT_STORE_ADD_ALWAYS = 4u;

			internal const uint CERT_CHAIN_POLICY_NT_AUTH = 6u;

			internal const uint X509_ASN_ENCODING = 1u;

			internal const uint PKCS_7_ASN_ENCODING = 65536u;

			internal const uint CERT_STORE_PROV_MEMORY = 2u;

			internal const uint CERT_INFO_ISSUER_FLAG = 4u;

			internal const uint CERT_INFO_SUBJECT_FLAG = 7u;

			internal const uint CERT_CHAIN_REVOCATION_CHECK_END_CERT = 268435456u;

			internal const uint CERT_CHAIN_REVOCATION_CHECK_CHAIN = 536870912u;

			internal const uint CERT_CHAIN_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 1073741824u;

			internal const uint CERT_CHAIN_REVOCATION_CHECK_CACHE_ONLY = 2147483648u;

			internal const uint CERT_CHAIN_POLICY_IGNORE_PEER_TRUST_FLAG = 4096u;

			internal const uint USAGE_MATCH_TYPE_AND = 0u;

			internal const uint HCCE_CURRENT_USER = 0u;

			internal const uint HCCE_LOCAL_MACHINE = 1u;

			[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern SafeCertContextHandle CertCreateCertificateContext([In] uint dwCertEncodingType, [In] IntPtr pbCertEncoded, [In] uint cbCertEncoded);

			[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern SafeCertStoreHandle CertOpenStore([In] IntPtr lpszStoreProvider, [In] uint dwMsgAndCertEncodingType, [In] IntPtr hCryptProv, [In] uint dwFlags, [In] string pvPara);

			[DllImport("crypt32.dll", SetLastError = true)]
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool CertCloseStore([In] IntPtr hCertStore, [In] uint dwFlags);

			[DllImport("crypt32.dll", SetLastError = true)]
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool CertFreeCertificateContext([In] IntPtr pCertContext);

			[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool CertAddCertificateLinkToStore([In] SafeCertStoreHandle hCertStore, [In] IntPtr pCertContext, [In] uint dwAddDisposition, [In][Out] SafeCertContextHandle ppStoreContext);

			[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool CertGetCertificateChain([In] IntPtr hChainEngine, [In] IntPtr pCertContext, [In] ref System.Runtime.InteropServices.ComTypes.FILETIME pTime, [In] SafeCertStoreHandle hAdditionalStore, [In] ref CERT_CHAIN_PARA pChainPara, [In] uint dwFlags, [In] IntPtr pvReserved, out SafeCertChainHandle ppChainContext);

			[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool CertVerifyCertificateChainPolicy([In] IntPtr pszPolicyOID, [In] SafeCertChainHandle pChainContext, [In] ref CERT_CHAIN_POLICY_PARA pPolicyPara, [In][Out] ref CERT_CHAIN_POLICY_STATUS pPolicyStatus);

			[DllImport("crypt32.dll", SetLastError = true)]
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			internal static extern void CertFreeCertificateChain(IntPtr handle);

			[DllImport("bcrypt.dll", SetLastError = true)]
			internal static extern int BCryptGetFipsAlgorithmMode([MarshalAs(UnmanagedType.U1)] out bool pfEnabled);
		}

		private const string _fipsPolicyRegistryKey = "System\\CurrentControlSet\\Control\\Lsa";

		public const int WindowsVistaMajorNumber = 6;

		private static object _syncObject = new object();

		private static FIPS_ALGORTITHM_POLICY _fipsPolicyState = FIPS_ALGORTITHM_POLICY.Unknown;

		private static RandomNumberGenerator _random = Algorithms.NewRandomNumberGenerator();

		public static bool AreEqual(byte[] a, byte[] b)
		{
			if (a == null || b == null)
			{
				if (a == null)
				{
					return null == b;
				}
				return false;
			}
			if (object.ReferenceEquals(a, b))
			{
				return true;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		public static int CeilingDivide(int dividend, int divisor)
		{
			int num = dividend % divisor;
			int num2 = dividend / divisor;
			if (num > 0)
			{
				num2++;
			}
			return num2;
		}

		internal static void CloseInvalidOutSafeHandle(SafeHandle handle)
		{
			handle?.SetHandleAsInvalid();
		}

		public static void GenerateRandomBytes(byte[] data)
		{
			_random.GetNonZeroBytes(data);
		}

		public static byte[] GenerateRandomBytes(int sizeInBits)
		{
			int num = sizeInBits / 8;
			if (sizeInBits <= 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentOutOfRangeException("sizeInBits", SR.GetString("ID6033", sizeInBits)));
			}
			if (num * 8 != sizeInBits)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID6002", sizeInBits), "sizeInBits"));
			}
			byte[] array = new byte[num];
			GenerateRandomBytes(array);
			return array;
		}

		public static byte[] CreateSignatureForSha256(AsymmetricSignatureFormatter formatter, HashAlgorithm hash)
		{
			if (Algorithms.RequiresFipsCompliance)
			{
				formatter.SetHashAlgorithm("SHA256");
				return formatter.CreateSignature(hash.Hash);
			}
			return formatter.CreateSignature(hash);
		}

		public static bool VerifySignatureForSha256(AsymmetricSignatureDeformatter deformatter, HashAlgorithm hash, byte[] signatureValue)
		{
			if (Algorithms.RequiresFipsCompliance)
			{
				deformatter.SetHashAlgorithm("SHA256");
				return deformatter.VerifySignature(hash.Hash, signatureValue);
			}
			return deformatter.VerifySignature(hash, signatureValue);
		}

		public static AsymmetricSignatureFormatter GetSignatureFormatterForSha256(AsymmetricSecurityKey key)
		{
			AsymmetricAlgorithm asymmetricAlgorithm = key.GetAsymmetricAlgorithm("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", privateKey: true);
			RSACryptoServiceProvider rSACryptoServiceProvider = asymmetricAlgorithm as RSACryptoServiceProvider;
			if (rSACryptoServiceProvider != null)
			{
				return GetSignatureFormatterForSha256(rSACryptoServiceProvider);
			}
			return new RSAPKCS1SignatureFormatter(asymmetricAlgorithm);
		}

		public static AsymmetricSignatureFormatter GetSignatureFormatterForSha256(RSACryptoServiceProvider rsaProvider)
		{
			CspParameters cspParameters = new CspParameters();
			cspParameters.ProviderType = 24;
			cspParameters.KeyContainerName = rsaProvider.CspKeyContainerInfo.KeyContainerName;
			cspParameters.KeyNumber = (int)rsaProvider.CspKeyContainerInfo.KeyNumber;
			if (rsaProvider.CspKeyContainerInfo.MachineKeyStore)
			{
				cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
			}
			rsaProvider = new RSACryptoServiceProvider(cspParameters);
			return new RSAPKCS1SignatureFormatter(rsaProvider);
		}

		public static AsymmetricSignatureDeformatter GetSignatureDeFormatterForSha256(AsymmetricSecurityKey key)
		{
			AsymmetricAlgorithm asymmetricAlgorithm = key.GetAsymmetricAlgorithm("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", privateKey: false);
			RSACryptoServiceProvider rSACryptoServiceProvider = asymmetricAlgorithm as RSACryptoServiceProvider;
			if (rSACryptoServiceProvider != null)
			{
				return GetSignatureDeFormatterForSha256(rSACryptoServiceProvider);
			}
			return new RSAPKCS1SignatureDeformatter(asymmetricAlgorithm);
		}

		public static AsymmetricSignatureDeformatter GetSignatureDeFormatterForSha256(RSACryptoServiceProvider rsaProvider)
		{
			CspParameters cspParameters = new CspParameters();
			cspParameters.ProviderType = 24;
			cspParameters.KeyNumber = (int)rsaProvider.CspKeyContainerInfo.KeyNumber;
			if (rsaProvider.CspKeyContainerInfo.MachineKeyStore)
			{
				cspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
			}
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
			rSACryptoServiceProvider.ImportCspBlob(rsaProvider.ExportCspBlob(includePrivateParameters: false));
			return new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
		}

		public static void ValidateBufferBounds(Array buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("buffer");
			}
			if (count < 0 || count > buffer.Length)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0007", 0, buffer.Length), "count"));
			}
			if (offset < 0 || offset > buffer.Length - count)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0007", 0, buffer.Length - count), "offset"));
			}
		}

		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Read = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Lsa")]
		private static FIPS_ALGORTITHM_POLICY GetFipsAlgorithmPolicyKeyFromRegistry()
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Lsa", writable: false);
			object obj = null;
			if (registryKey != null)
			{
				obj = registryKey.GetValue("FIPSAlgorithmPolicy");
			}
			if (obj == null || (int)obj == 0)
			{
				return FIPS_ALGORTITHM_POLICY.Disabled;
			}
			return FIPS_ALGORTITHM_POLICY.Enabled;
		}

		public static void ResetAllCertificates(X509Certificate2Collection certificates)
		{
			if (certificates != null)
			{
				for (int i = 0; i < certificates.Count; i++)
				{
					certificates[i].Reset();
				}
			}
		}
	}
}
