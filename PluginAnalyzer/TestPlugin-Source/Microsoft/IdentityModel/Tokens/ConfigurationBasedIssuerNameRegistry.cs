using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class ConfigurationBasedIssuerNameRegistry : IssuerNameRegistry
	{
		private class ThumbprintKeyComparer : IEqualityComparer<string>
		{
			public bool Equals(string x, string y)
			{
				return StringComparer.OrdinalIgnoreCase.Equals(x, y);
			}

			public int GetHashCode(string obj)
			{
				return obj.ToUpper(CultureInfo.InvariantCulture).GetHashCode();
			}
		}

		private Dictionary<string, string> _configuredTrustedIssuers = new Dictionary<string, string>(new ThumbprintKeyComparer());

		public IDictionary<string, string> ConfiguredTrustedIssuers => _configuredTrustedIssuers;

		public ConfigurationBasedIssuerNameRegistry()
		{
		}

		public ConfigurationBasedIssuerNameRegistry(XmlNodeList customConfiguration)
		{
			if (customConfiguration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("customConfiguration");
			}
			List<XmlElement> xmlElements = XmlUtil.GetXmlElements(customConfiguration);
			if (xmlElements.Count != 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7019", typeof(ConfigurationBasedIssuerNameRegistry).Name));
			}
			XmlElement xmlElement = xmlElements[0];
			if (!StringComparer.Ordinal.Equals(xmlElement.LocalName, "trustedIssuers"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7002", xmlElement.LocalName, "trustedIssuers"));
			}
			foreach (XmlNode childNode in xmlElement.ChildNodes)
			{
				XmlElement xmlElement2 = childNode as XmlElement;
				if (xmlElement2 == null)
				{
					continue;
				}
				if (StringComparer.Ordinal.Equals(xmlElement2.LocalName, "add"))
				{
					XmlNode namedItem = xmlElement2.Attributes.GetNamedItem("thumbprint");
					XmlNode namedItem2 = xmlElement2.Attributes.GetNamedItem("name");
					if (xmlElement2.Attributes.Count != 2 || namedItem == null || namedItem2 == null || string.IsNullOrEmpty(namedItem2.Value))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7010", string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[2] { xmlElement.LocalName, xmlElement2.LocalName }), string.Format(CultureInfo.InvariantCulture, "{0} and {1}", new object[2] { "thumbprint", "name" })));
					}
					string value = namedItem.Value;
					value = value.Replace(" ", "");
					string value2 = string.Intern(namedItem2.Value);
					_configuredTrustedIssuers.Add(value, value2);
				}
				else if (StringComparer.Ordinal.Equals(xmlElement2.LocalName, "remove"))
				{
					if (xmlElement2.Attributes.Count != 1 || !StringComparer.Ordinal.Equals(xmlElement2.Attributes[0].LocalName, "thumbprint"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7010", string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[2] { xmlElement.LocalName, xmlElement2.LocalName }), "thumbprint"));
					}
					string value3 = xmlElement2.Attributes.GetNamedItem("thumbprint").Value;
					value3 = value3.Replace(" ", "");
					_configuredTrustedIssuers.Remove(value3);
				}
				else
				{
					if (!StringComparer.Ordinal.Equals(xmlElement2.LocalName, "clear"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID7002", xmlElement.LocalName, xmlElement2.LocalName));
					}
					_configuredTrustedIssuers.Clear();
				}
			}
		}

		public override string GetIssuerName(SecurityToken securityToken)
		{
			if (securityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityToken");
			}
			X509SecurityToken x509SecurityToken = securityToken as X509SecurityToken;
			if (x509SecurityToken != null)
			{
				string thumbprint = x509SecurityToken.Certificate.Thumbprint;
				if (_configuredTrustedIssuers.ContainsKey(thumbprint))
				{
					return _configuredTrustedIssuers[thumbprint];
				}
			}
			return null;
		}

		public void AddTrustedIssuer(string certificateThumbprint, string name)
		{
			if (string.IsNullOrEmpty(certificateThumbprint))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("certificateThumbprint");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("name");
			}
			if (_configuredTrustedIssuers.ContainsKey(certificateThumbprint))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4265", certificateThumbprint));
			}
			certificateThumbprint = certificateThumbprint.Replace(" ", "");
			_configuredTrustedIssuers.Add(certificateThumbprint, name);
		}
	}
}
