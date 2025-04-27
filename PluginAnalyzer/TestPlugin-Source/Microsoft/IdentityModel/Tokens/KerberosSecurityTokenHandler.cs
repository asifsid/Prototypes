using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class KerberosSecurityTokenHandler : SecurityTokenHandler
	{
		private static string[] _tokenTypeIdentifiers = new string[1] { "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Kerberos" };

		public override bool CanValidateToken => true;

		public override Type TokenType => typeof(KerberosReceiverSecurityToken);

		public override string[] GetTokenTypeIdentifiers()
		{
			return _tokenTypeIdentifiers;
		}

		public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			KerberosReceiverSecurityToken kerberosReceiverSecurityToken = token as KerberosReceiverSecurityToken;
			if (kerberosReceiverSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID0018", typeof(KerberosReceiverSecurityToken)));
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			string windowsIssuerName = base.Configuration.IssuerNameRegistry.GetWindowsIssuerName();
			WindowsClaimsIdentity windowsClaimsIdentity = new WindowsClaimsIdentity(kerberosReceiverSecurityToken.WindowsIdentity.Token, "Kerberos", windowsIssuerName);
			windowsClaimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/windows", "http://www.w3.org/2001/XMLSchema#string"));
			windowsClaimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime", windowsIssuerName));
			if (base.Configuration.SaveBootstrapTokens)
			{
				windowsClaimsIdentity.BootstrapToken = token;
			}
			return new ClaimsIdentityCollection(new IClaimsIdentity[1] { windowsClaimsIdentity });
		}
	}
}
