using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityTokenElement
	{
		private SecurityToken _securityToken;

		private XmlElement _securityTokenXml;

		private SecurityTokenHandlerCollection _securityTokenHandlers;

		private ClaimsIdentityCollection _subject;

		public XmlElement SecurityTokenXml => _securityTokenXml;

		public SecurityTokenElement(SecurityToken securityToken)
		{
			if (securityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityToken");
			}
			GenericXmlSecurityToken genericXmlSecurityToken = securityToken as GenericXmlSecurityToken;
			if (genericXmlSecurityToken != null)
			{
				_securityTokenXml = genericXmlSecurityToken.TokenXml;
			}
			_securityToken = securityToken;
		}

		public SecurityTokenElement(XmlElement securityTokenXml, SecurityTokenHandlerCollection securityTokenHandlers)
		{
			if (securityTokenXml == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenXml");
			}
			if (securityTokenHandlers == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlers");
			}
			_securityTokenXml = securityTokenXml;
			_securityTokenHandlers = securityTokenHandlers;
		}

		public SecurityToken GetSecurityToken()
		{
			if (_securityToken == null)
			{
				_securityToken = ReadSecurityToken(_securityTokenXml, _securityTokenHandlers);
			}
			return _securityToken;
		}

		public ClaimsIdentityCollection GetSubject()
		{
			if (_subject == null)
			{
				_subject = CreateSubject(_securityTokenXml, _securityTokenHandlers);
			}
			return _subject;
		}

		protected virtual ClaimsIdentityCollection CreateSubject(XmlElement securityTokenXml, SecurityTokenHandlerCollection securityTokenHandlers)
		{
			if (securityTokenXml == null || securityTokenHandlers == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4052"));
			}
			SecurityToken securityToken = GetSecurityToken();
			return securityTokenHandlers.ValidateToken(securityToken);
		}

		protected virtual SecurityToken ReadSecurityToken(XmlElement securityTokenXml, SecurityTokenHandlerCollection securityTokenHandlers)
		{
			XmlReader xmlReader = new XmlNodeReader(securityTokenXml);
			xmlReader.MoveToContent();
			SecurityToken securityToken = securityTokenHandlers.ReadToken(xmlReader);
			if (securityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4051", securityTokenXml, xmlReader.LocalName, xmlReader.NamespaceURI)));
			}
			return securityToken;
		}
	}
}
