using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Text;
using System.Web.Compilation;
using System.Xml;
using Microsoft.IdentityModel.Configuration;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SamlSecurityTokenRequirement
	{
		private static X509RevocationMode DefaultRevocationMode = X509RevocationMode.Online;

		private static X509CertificateValidationMode DefaultValidationMode = X509CertificateValidationMode.PeerOrChainTrust;

		private static StoreLocation DefaultStoreLocation = StoreLocation.LocalMachine;

		private string _nameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

		private string _roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

		private bool _mapToWindows;

		private bool _useWindowsTokenService;

		private X509CertificateValidator _certificateValidator;

		public X509CertificateValidator CertificateValidator
		{
			get
			{
				return _certificateValidator;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_certificateValidator = value;
			}
		}

		public string NameClaimType
		{
			get
			{
				return _nameClaimType;
			}
			set
			{
				_nameClaimType = value;
			}
		}

		public string RoleClaimType
		{
			get
			{
				return _roleClaimType;
			}
			set
			{
				_roleClaimType = value;
			}
		}

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

		public SamlSecurityTokenRequirement()
		{
		}

		public SamlSecurityTokenRequirement(XmlElement element)
		{
			if (element == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("element");
			}
			if (element.LocalName != "samlSecurityTokenRequirement")
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7000", "samlSecurityTokenRequirement", element.LocalName));
			}
			bool flag = false;
			X509RevocationMode revocationMode = DefaultRevocationMode;
			X509CertificateValidationMode x509CertificateValidationMode = DefaultValidationMode;
			StoreLocation trustedStoreLocation = DefaultStoreLocation;
			string text = null;
			foreach (XmlAttribute attribute in element.Attributes)
			{
				if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "mapToWindows"))
				{
					bool result = false;
					if (!bool.TryParse(attribute.Value, out result))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7022", attribute.Value));
					}
					MapToWindows = result;
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "useWindowsTokenService"))
				{
					bool result2 = false;
					if (!bool.TryParse(attribute.Value, out result2))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7023", attribute.Value));
					}
					UseWindowsTokenService = result2;
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "issuerCertificateValidator"))
				{
					text = attribute.Value.ToString();
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "issuerCertificateRevocationMode"))
				{
					flag = true;
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
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7011", attribute.LocalName, element.LocalName)));
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "issuerCertificateValidationMode"))
				{
					flag = true;
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
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7011", attribute.LocalName, element.LocalName)));
				}
				if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "issuerCertificateTrustedStoreLocation"))
				{
					flag = true;
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
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7011", attribute.LocalName, element.LocalName)));
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7004", attribute.LocalName, element.LocalName)));
			}
			List<XmlElement> xmlElements = XmlUtil.GetXmlElements(element.ChildNodes);
			foreach (XmlElement item in xmlElements)
			{
				if (StringComparer.Ordinal.Equals(item.LocalName, "nameClaimType"))
				{
					if (item.Attributes.Count != 1 || !StringComparer.Ordinal.Equals(item.Attributes[0].LocalName, "value"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7001", string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[2] { element.LocalName, item.LocalName }), "value"));
					}
					NameClaimType = item.Attributes[0].Value;
					continue;
				}
				if (StringComparer.Ordinal.Equals(item.LocalName, "roleClaimType"))
				{
					if (item.Attributes.Count != 1 || !StringComparer.Ordinal.Equals(item.Attributes[0].LocalName, "value"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7001", string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[2] { element.LocalName, item.LocalName }), "value"));
					}
					RoleClaimType = item.Attributes[0].Value;
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7002", item.LocalName, "samlSecurityTokenRequirement"));
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
			else if (flag)
			{
				_certificateValidator = X509Util.CreateCertificateValidator(x509CertificateValidationMode, revocationMode, trustedStoreLocation);
			}
		}

		public virtual bool ShouldEnforceAudienceRestriction(AudienceUriMode audienceUriMode, SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			switch (audienceUriMode)
			{
			case AudienceUriMode.Always:
				return true;
			case AudienceUriMode.Never:
				return false;
			case AudienceUriMode.BearerKeyOnly:
				if (token.SecurityKeys != null)
				{
					return 0 == token.SecurityKeys.Count;
				}
				return true;
			default:
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4025", audienceUriMode)));
			}
		}

		public virtual void ValidateAudienceRestriction(IList<Uri> allowedAudienceUris, IList<Uri> tokenAudiences)
		{
			if (allowedAudienceUris == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("allowedAudienceUris");
			}
			if (tokenAudiences == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenAudiences");
			}
			if (tokenAudiences.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new AudienceUriValidationFailedException(SR.GetString("ID1036")));
			}
			if (allowedAudienceUris.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new AudienceUriValidationFailedException(SR.GetString("ID1043")));
			}
			bool flag = false;
			foreach (Uri tokenAudience in tokenAudiences)
			{
				if (tokenAudience != null)
				{
					Uri item;
					if (tokenAudience.IsAbsoluteUri)
					{
						item = new Uri(tokenAudience.GetLeftPart(UriPartial.Path));
					}
					else
					{
						Uri uri = new Uri("http://www.example.com");
						Uri uri2 = new Uri(uri, tokenAudience);
						item = uri.MakeRelativeUri(new Uri(uri2.GetLeftPart(UriPartial.Path)));
					}
					if (allowedAudienceUris.Contains(item))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return;
			}
			if (1 == tokenAudiences.Count || null != tokenAudiences[0])
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new AudienceUriValidationFailedException(SR.GetString("ID1038", tokenAudiences[0].OriginalString)));
			}
			StringBuilder stringBuilder = new StringBuilder(SR.GetString("ID8007"));
			bool flag2 = true;
			foreach (Uri tokenAudience2 in tokenAudiences)
			{
				if (tokenAudience2 != null)
				{
					if (flag2)
					{
						flag2 = false;
					}
					else
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(tokenAudience2.OriginalString);
				}
			}
			DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Error, stringBuilder.ToString());
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new AudienceUriValidationFailedException(SR.GetString("ID1037")));
		}
	}
}
