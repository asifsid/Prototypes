using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.Web.Compilation;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class X509SecurityTokenHandler : SecurityTokenHandler
	{
		private static X509RevocationMode DefaultRevocationMode = X509RevocationMode.Online;

		private static X509CertificateValidationMode DefaultValidationMode = X509CertificateValidationMode.PeerOrChainTrust;

		private static StoreLocation DefaultStoreLocation = StoreLocation.LocalMachine;

		private bool _mapToWindows;

		private bool _useWindowsTokenService;

		private X509CertificateValidator _certificateValidator;

		private bool _writeXmlDSigDefinedClauseTypes;

		private X509DataSecurityKeyIdentifierClauseSerializer _x509DataKeyIdentifierClauseSerializer = new X509DataSecurityKeyIdentifierClauseSerializer();

		public bool MapToWindows
		{
			get
			{
				return _mapToWindows;
			}
			set
			{
				_mapToWindows = value;
			}
		}

		public bool UseWindowsTokenService
		{
			get
			{
				return _useWindowsTokenService;
			}
			set
			{
				_useWindowsTokenService = value;
			}
		}

		public X509CertificateValidator CertificateValidator
		{
			get
			{
				if (_certificateValidator == null)
				{
					if (base.Configuration != null)
					{
						return base.Configuration.CertificateValidator;
					}
					return null;
				}
				return _certificateValidator;
			}
			set
			{
				_certificateValidator = value;
			}
		}

		public bool WriteXmlDSigDefinedClauseTypes
		{
			get
			{
				return _writeXmlDSigDefinedClauseTypes;
			}
			set
			{
				_writeXmlDSigDefinedClauseTypes = value;
			}
		}

		public override bool CanValidateToken => true;

		public override bool CanWriteToken => true;

		public override Type TokenType => typeof(X509SecurityToken);

		public X509SecurityTokenHandler()
			: this(mapToWindows: false, null)
		{
		}

		public X509SecurityTokenHandler(X509CertificateValidator certificateValidator)
			: this(mapToWindows: false, certificateValidator)
		{
		}

		public X509SecurityTokenHandler(bool mapToWindows)
			: this(mapToWindows, null)
		{
		}

		public X509SecurityTokenHandler(bool mapToWindows, X509CertificateValidator certificateValidator)
		{
			_mapToWindows = mapToWindows;
			_certificateValidator = certificateValidator;
		}

		public X509SecurityTokenHandler(XmlNodeList customConfigElements)
		{
			if (customConfigElements == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("customConfigElements");
			}
			List<XmlElement> xmlElements = XmlUtil.GetXmlElements(customConfigElements);
			bool flag = false;
			bool flag2 = false;
			X509RevocationMode revocationMode = DefaultRevocationMode;
			X509CertificateValidationMode x509CertificateValidationMode = DefaultValidationMode;
			StoreLocation trustedStoreLocation = DefaultStoreLocation;
			string text = null;
			foreach (XmlElement item in xmlElements)
			{
				if (!StringComparer.Ordinal.Equals(item.LocalName, "x509SecurityTokenHandlerRequirement"))
				{
					continue;
				}
				if (flag)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7026", "x509SecurityTokenHandlerRequirement"));
				}
				foreach (XmlAttribute attribute in item.Attributes)
				{
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "mapToWindows"))
					{
						_mapToWindows = XmlConvert.ToBoolean(attribute.Value.ToLowerInvariant());
						continue;
					}
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "useWindowsTokenService"))
					{
						_useWindowsTokenService = XmlConvert.ToBoolean(attribute.Value.ToLowerInvariant());
						continue;
					}
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "certificateValidator"))
					{
						text = attribute.Value.ToString();
						continue;
					}
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "revocationMode"))
					{
						flag2 = true;
						string x = attribute.Value.ToString();
						if (StringComparer.OrdinalIgnoreCase.Equals(x, "NoCheck"))
						{
							revocationMode = X509RevocationMode.NoCheck;
							continue;
						}
						if (StringComparer.OrdinalIgnoreCase.Equals(x, "Offline"))
						{
							revocationMode = X509RevocationMode.Offline;
							continue;
						}
						if (StringComparer.OrdinalIgnoreCase.Equals(x, "Online"))
						{
							revocationMode = X509RevocationMode.Online;
							continue;
						}
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7011", attribute.LocalName, item.LocalName)));
					}
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "certificateValidationMode"))
					{
						flag2 = true;
						string x2 = attribute.Value.ToString();
						if (StringComparer.OrdinalIgnoreCase.Equals(x2, "ChainTrust"))
						{
							x509CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
							continue;
						}
						if (StringComparer.OrdinalIgnoreCase.Equals(x2, "PeerOrChainTrust"))
						{
							x509CertificateValidationMode = X509CertificateValidationMode.PeerOrChainTrust;
							continue;
						}
						if (StringComparer.OrdinalIgnoreCase.Equals(x2, "PeerTrust"))
						{
							x509CertificateValidationMode = X509CertificateValidationMode.PeerTrust;
							continue;
						}
						if (StringComparer.OrdinalIgnoreCase.Equals(x2, "None"))
						{
							x509CertificateValidationMode = X509CertificateValidationMode.None;
							continue;
						}
						if (StringComparer.OrdinalIgnoreCase.Equals(x2, "Custom"))
						{
							x509CertificateValidationMode = X509CertificateValidationMode.Custom;
							continue;
						}
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7011", attribute.LocalName, item.LocalName)));
					}
					if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "trustedStoreLocation"))
					{
						flag2 = true;
						string x3 = attribute.Value.ToString();
						if (StringComparer.OrdinalIgnoreCase.Equals(x3, "CurrentUser"))
						{
							trustedStoreLocation = StoreLocation.CurrentUser;
							continue;
						}
						if (StringComparer.OrdinalIgnoreCase.Equals(x3, "LocalMachine"))
						{
							trustedStoreLocation = StoreLocation.LocalMachine;
							continue;
						}
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7011", attribute.LocalName, item.LocalName)));
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7004", attribute.LocalName, item.LocalName)));
				}
				flag = true;
			}
			if (x509CertificateValidationMode == X509CertificateValidationMode.Custom)
			{
				if (string.IsNullOrEmpty(text))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7028"));
				}
				Type type = BuildManager.GetType(text, throwOnError: true);
				if ((object)type == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID7007", type));
				}
				_certificateValidator = CustomTypeElement.Resolve<X509CertificateValidator>(new CustomTypeElement(type), new object[0]);
			}
			else if (flag2)
			{
				_certificateValidator = X509Util.CreateCertificateValidator(x509CertificateValidationMode, revocationMode, trustedStoreLocation);
			}
		}

		public override bool CanReadKeyIdentifierClause(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return _x509DataKeyIdentifierClauseSerializer.CanReadKeyIdentifierClause(reader);
		}

		public override bool CanReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (reader.IsStartElement("BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
			{
				string attribute = reader.GetAttribute("ValueType", null);
				return StringComparer.Ordinal.Equals(attribute, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
			}
			return false;
		}

		public override bool CanWriteKeyIdentifierClause(SecurityKeyIdentifierClause securityKeyIdentifierClause)
		{
			if (securityKeyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityKeyIdentifierClause");
			}
			if (_writeXmlDSigDefinedClauseTypes)
			{
				return _x509DataKeyIdentifierClauseSerializer.CanWriteKeyIdentifierClause(securityKeyIdentifierClause);
			}
			return false;
		}

		public override SecurityKeyIdentifierClause ReadKeyIdentifierClause(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return _x509DataKeyIdentifierClauseSerializer.ReadKeyIdentifierClause(reader);
		}

		public override SecurityToken ReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			if (!xmlDictionaryReader.IsStartElement("BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4065", "BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI)));
			}
			string attribute = xmlDictionaryReader.GetAttribute("ValueType", null);
			if (!StringComparer.Ordinal.Equals(attribute, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4066", "BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", "ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3", attribute)));
			}
			string attribute2 = xmlDictionaryReader.GetAttribute("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
			string attribute3 = xmlDictionaryReader.GetAttribute("EncodingType", null);
			byte[] rawData;
			if (attribute3 == null || StringComparer.Ordinal.Equals(attribute3, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"))
			{
				rawData = xmlDictionaryReader.ReadElementContentAsBase64();
			}
			else
			{
				if (!StringComparer.Ordinal.Equals(attribute3, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#HexBinary"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4068")));
				}
				rawData = SoapHexBinary.Parse(xmlDictionaryReader.ReadElementContentAsString()).Value;
			}
			if (!string.IsNullOrEmpty(attribute2))
			{
				return new X509SecurityToken(new X509Certificate2(rawData), attribute2);
			}
			return new X509SecurityToken(new X509Certificate2(rawData));
		}

		public override string[] GetTokenTypeIdentifiers()
		{
			return new string[1] { "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/X509Certificate" };
		}

		public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			X509SecurityToken x509SecurityToken = token as X509SecurityToken;
			if (x509SecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID0018", typeof(X509SecurityToken)));
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			try
			{
				CertificateValidator.Validate(x509SecurityToken.Certificate);
			}
			catch (SecurityTokenValidationException innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4257", X509Util.GetCertificateId(x509SecurityToken.Certificate)), innerException));
			}
			if (base.Configuration.IssuerNameRegistry == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4277"));
			}
			string certificateIssuerName = X509Util.GetCertificateIssuerName(x509SecurityToken.Certificate, base.Configuration.IssuerNameRegistry);
			if (string.IsNullOrEmpty(certificateIssuerName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4175")));
			}
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(x509SecurityToken.Certificate, certificateIssuerName);
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/x509"));
			if (!_mapToWindows)
			{
				if (base.Configuration.SaveBootstrapTokens)
				{
					claimsIdentity.BootstrapToken = token;
				}
				return new ClaimsIdentityCollection(new IClaimsIdentity[1] { claimsIdentity });
			}
			X509WindowsSecurityToken x509WindowsSecurityToken = token as X509WindowsSecurityToken;
			WindowsClaimsIdentity windowsClaimsIdentity;
			if (x509WindowsSecurityToken != null)
			{
				WindowsIdentity windowsIdentity = x509WindowsSecurityToken.WindowsIdentity;
				windowsClaimsIdentity = new WindowsClaimsIdentity(windowsIdentity.Token, "X509", base.Configuration.IssuerNameRegistry.GetWindowsIssuerName());
			}
			else
			{
				X509NTAuthChainTrustValidator x509NTAuthChainTrustValidator = new X509NTAuthChainTrustValidator();
				x509NTAuthChainTrustValidator.Validate(x509SecurityToken.Certificate);
				windowsClaimsIdentity = WindowsClaimsIdentity.CreateFromCertificate(x509SecurityToken.Certificate, _useWindowsTokenService, base.Configuration.IssuerNameRegistry.GetWindowsIssuerName());
			}
			windowsClaimsIdentity.Claims.CopyRange(claimsIdentity.Claims);
			if (base.Configuration.SaveBootstrapTokens)
			{
				windowsClaimsIdentity.BootstrapToken = token;
			}
			return new ClaimsIdentityCollection(new IClaimsIdentity[1] { windowsClaimsIdentity });
		}

		internal unsafe static WindowsIdentity KerberosCertificateLogon(X509Certificate2 certificate)
		{
			SafeHGlobalHandle safeHGlobalHandle = null;
			SafeHGlobalHandle safeHGlobalHandle2 = null;
			SafeHGlobalHandle safeHGlobalHandle3 = null;
			SafeLsaLogonProcessHandle lsaHandle = null;
			SafeLsaReturnBufferHandle ProfileBuffer = null;
			SafeCloseHandle Token = null;
			try
			{
				safeHGlobalHandle = SafeHGlobalHandle.AllocHGlobal(NativeMethods.LsaSourceName.Length + 1);
				Marshal.Copy(NativeMethods.LsaSourceName, 0, safeHGlobalHandle.DangerousGetHandle(), NativeMethods.LsaSourceName.Length);
				UNICODE_INTPTR_STRING logonProcessName = new UNICODE_INTPTR_STRING(NativeMethods.LsaSourceName.Length, NativeMethods.LsaSourceName.Length + 1, safeHGlobalHandle.DangerousGetHandle());
				Privilege privilege = null;
				RuntimeHelpers.PrepareConstrainedRegions();
				int num;
				try
				{
					try
					{
						privilege = new Privilege("SeTcbPrivilege");
						privilege.Enable();
					}
					catch (PrivilegeNotHeldException exception)
					{
						DiagnosticUtil.TraceUtil.Trace(TraceEventType.Warning, TraceCode.Diagnostics, null, null, exception);
					}
					IntPtr securityMode = IntPtr.Zero;
					num = NativeMethods.LsaRegisterLogonProcess(ref logonProcessName, out lsaHandle, out securityMode);
					if (5 == NativeMethods.LsaNtStatusToWinError(num))
					{
						num = NativeMethods.LsaConnectUntrusted(out lsaHandle);
					}
					if (num < 0)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(NativeMethods.LsaNtStatusToWinError(num)));
					}
				}
				finally
				{
					int num2 = -1;
					string text = null;
					try
					{
						num2 = privilege.Revert();
						if (num2 != 0)
						{
							text = SR.GetString("ID4069", new Win32Exception(num2));
						}
					}
					finally
					{
						if (num2 != 0)
						{
							DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Critical, text);
							Environment.FailFast(text);
						}
					}
				}
				safeHGlobalHandle2 = SafeHGlobalHandle.AllocHGlobal(NativeMethods.LsaKerberosName.Length + 1);
				Marshal.Copy(NativeMethods.LsaKerberosName, 0, safeHGlobalHandle2.DangerousGetHandle(), NativeMethods.LsaKerberosName.Length);
				UNICODE_INTPTR_STRING packageName = new UNICODE_INTPTR_STRING(NativeMethods.LsaKerberosName.Length, NativeMethods.LsaKerberosName.Length + 1, safeHGlobalHandle2.DangerousGetHandle());
				uint authenticationPackage = 0u;
				num = NativeMethods.LsaLookupAuthenticationPackage(lsaHandle, ref packageName, out authenticationPackage);
				if (num < 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(NativeMethods.LsaNtStatusToWinError(num)));
				}
				TOKEN_SOURCE SourceContext = default(TOKEN_SOURCE);
				if (!NativeMethods.AllocateLocallyUniqueId(out SourceContext.SourceIdentifier))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(lastWin32Error));
				}
				SourceContext.Name = new char[8];
				SourceContext.Name[0] = 'W';
				SourceContext.Name[1] = 'C';
				SourceContext.Name[2] = 'F';
				byte[] rawData = certificate.RawData;
				int num3 = checked(KERB_CERTIFICATE_S4U_LOGON.Size + rawData.Length);
				safeHGlobalHandle3 = SafeHGlobalHandle.AllocHGlobal(num3);
				KERB_CERTIFICATE_S4U_LOGON* ptr = (KERB_CERTIFICATE_S4U_LOGON*)safeHGlobalHandle3.DangerousGetHandle().ToPointer();
				ptr->MessageType = KERB_LOGON_SUBMIT_TYPE.KerbCertificateS4ULogon;
				ptr->Flags = 2u;
				ptr->UserPrincipalName = new UNICODE_INTPTR_STRING(0, 0, IntPtr.Zero);
				ptr->DomainName = new UNICODE_INTPTR_STRING(0, 0, IntPtr.Zero);
				ptr->CertificateLength = (uint)rawData.Length;
				ptr->Certificate = new IntPtr(safeHGlobalHandle3.DangerousGetHandle().ToInt64() + KERB_CERTIFICATE_S4U_LOGON.Size);
				Marshal.Copy(rawData, 0, ptr->Certificate, rawData.Length);
				QUOTA_LIMITS Quotas = default(QUOTA_LIMITS);
				LUID LogonId = default(LUID);
				int SubStatus = 0;
				num = NativeMethods.LsaLogonUser(lsaHandle, ref logonProcessName, SecurityLogonType.Network, authenticationPackage, safeHGlobalHandle3.DangerousGetHandle(), (uint)num3, IntPtr.Zero, ref SourceContext, out ProfileBuffer, out var _, out LogonId, out Token, out Quotas, out SubStatus);
				if (num == -1073741714 && SubStatus < 0)
				{
					num = SubStatus;
				}
				if (num < 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(NativeMethods.LsaNtStatusToWinError(num)));
				}
				if (SubStatus < 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new Win32Exception(NativeMethods.LsaNtStatusToWinError(SubStatus)));
				}
				return new WindowsIdentity(Token.DangerousGetHandle());
			}
			finally
			{
				Token?.Close();
				safeHGlobalHandle3?.Close();
				ProfileBuffer?.Close();
				safeHGlobalHandle?.Close();
				safeHGlobalHandle2?.Close();
				lsaHandle?.Close();
			}
		}

		public override void WriteKeyIdentifierClause(XmlWriter writer, SecurityKeyIdentifierClause securityKeyIdentifierClause)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (securityKeyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityKeyIdentifierClause");
			}
			if (!_writeXmlDSigDefinedClauseTypes)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4261"));
			}
			_x509DataKeyIdentifierClauseSerializer.WriteKeyIdentifierClause(writer, securityKeyIdentifierClause);
		}

		public override void WriteToken(XmlWriter writer, SecurityToken token)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			X509SecurityToken x509SecurityToken = token as X509SecurityToken;
			if (x509SecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID0018", typeof(X509SecurityToken)));
			}
			writer.WriteStartElement("BinarySecurityToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
			if (!string.IsNullOrEmpty(x509SecurityToken.Id))
			{
				writer.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", x509SecurityToken.Id);
			}
			writer.WriteAttributeString("ValueType", null, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
			writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
			byte[] rawCertData = x509SecurityToken.Certificate.GetRawCertData();
			writer.WriteBase64(rawCertData, 0, rawCertData.Length);
			writer.WriteEndElement();
		}
	}
}
