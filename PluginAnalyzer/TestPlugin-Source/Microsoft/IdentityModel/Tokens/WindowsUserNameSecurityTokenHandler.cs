using System;
using System.ComponentModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class WindowsUserNameSecurityTokenHandler : UserNameSecurityTokenHandler
	{
		public override bool CanValidateToken => true;

		public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			UserNameSecurityToken userNameSecurityToken = token as UserNameSecurityToken;
			if (userNameSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID0018", typeof(UserNameSecurityToken)));
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			string text = userNameSecurityToken.UserName;
			string password = userNameSecurityToken.Password;
			string lpszDomain = null;
			string[] array = userNameSecurityToken.UserName.Split('\\');
			if (array.Length != 1)
			{
				if (array.Length != 2 || string.IsNullOrEmpty(array[0]))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID4062"));
				}
				text = array[1];
				lpszDomain = array[0];
			}
			SafeCloseHandle phToken = null;
			try
			{
				if (!NativeMethods.LogonUser(text, lpszDomain, password, 8u, 0u, out phToken))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4063", text), new Win32Exception(lastWin32Error)));
				}
				string windowsIssuerName = base.Configuration.IssuerNameRegistry.GetWindowsIssuerName();
				WindowsClaimsIdentity windowsClaimsIdentity = new WindowsClaimsIdentity(phToken.DangerousGetHandle(), "Password", windowsIssuerName);
				windowsClaimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime", windowsIssuerName));
				windowsClaimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password"));
				if (base.Configuration.SaveBootstrapTokens)
				{
					if (RetainPassword)
					{
						windowsClaimsIdentity.BootstrapToken = userNameSecurityToken;
					}
					else
					{
						windowsClaimsIdentity.BootstrapToken = new UserNameSecurityToken(userNameSecurityToken.UserName, null);
					}
				}
				return new ClaimsIdentityCollection(new IClaimsIdentity[1] { windowsClaimsIdentity });
			}
			finally
			{
				phToken?.Close();
			}
		}
	}
}
