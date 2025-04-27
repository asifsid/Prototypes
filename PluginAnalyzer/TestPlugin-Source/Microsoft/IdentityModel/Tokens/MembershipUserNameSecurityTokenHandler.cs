using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Web.Security;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class MembershipUserNameSecurityTokenHandler : UserNameSecurityTokenHandler
	{
		private MembershipProvider _provider;

		public override bool CanValidateToken => true;

		public MembershipProvider MembershipProvider
		{
			get
			{
				return _provider;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_provider = value;
			}
		}

		public MembershipUserNameSecurityTokenHandler()
		{
			_provider = Membership.Provider;
			if (_provider == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4040"));
			}
		}

		public MembershipUserNameSecurityTokenHandler(MembershipProvider provider)
		{
			if (provider == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("provider");
			}
			_provider = provider;
		}

		public MembershipUserNameSecurityTokenHandler(XmlNodeList customConfigElements)
		{
			if (customConfigElements == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("customConfigElements");
			}
			List<XmlElement> xmlElements = XmlUtil.GetXmlElements(customConfigElements);
			if (1 != xmlElements.Count)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7005", "userNameSecurityTokenHandlerRequirement")));
			}
			XmlElement xmlElement = xmlElements[0];
			if (!StringComparer.Ordinal.Equals(xmlElement.LocalName, "userNameSecurityTokenHandlerRequirement"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7006", "userNameSecurityTokenHandlerRequirement", xmlElement.LocalName)));
			}
			_provider = Membership.Provider;
			foreach (XmlAttribute attribute in xmlElement.Attributes)
			{
				if (StringComparer.OrdinalIgnoreCase.Equals(attribute.LocalName, "membershipProviderName"))
				{
					_provider = Membership.Providers[attribute.Value.ToString()];
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID7004", attribute.LocalName, xmlElement.LocalName)));
			}
			if (_provider == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4064"));
			}
		}

		public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			UserNameSecurityToken userNameSecurityToken = token as UserNameSecurityToken;
			if (userNameSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID0018", typeof(UserNameSecurityToken)));
			}
			if (!_provider.ValidateUser(userNameSecurityToken.UserName, userNameSecurityToken.Password))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4058", userNameSecurityToken.UserName)));
			}
			IClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[1]
			{
				new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", userNameSecurityToken.UserName)
			}, "Password");
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password"));
			if (base.Configuration.SaveBootstrapTokens)
			{
				if (RetainPassword)
				{
					claimsIdentity.BootstrapToken = userNameSecurityToken;
				}
				else
				{
					claimsIdentity.BootstrapToken = new UserNameSecurityToken(userNameSecurityToken.UserName, null);
				}
			}
			return new ClaimsIdentityCollection(new IClaimsIdentity[1] { claimsIdentity });
		}
	}
}
