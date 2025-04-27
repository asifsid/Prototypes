using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Web
{
	internal class TokenReceiver
	{
		private ServiceConfiguration _serviceConfiguration;

		public TimeSpan ConfiguredSessionTokenLifeTime
		{
			get
			{
				TimeSpan result = Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler.DefaultTokenLifetime;
				if (_serviceConfiguration.SecurityTokenHandlers != null)
				{
					Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler sessionSecurityTokenHandler = _serviceConfiguration.SecurityTokenHandlers[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)] as Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler;
					if (sessionSecurityTokenHandler != null)
					{
						result = sessionSecurityTokenHandler.TokenLifetime;
					}
				}
				return result;
			}
		}

		public TokenReceiver(ServiceConfiguration serviceConfiguration)
		{
			if (serviceConfiguration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("serviceConfiguration");
			}
			_serviceConfiguration = serviceConfiguration;
		}

		public SecurityToken ReadToken(XmlReader reader)
		{
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlers = _serviceConfiguration.SecurityTokenHandlers;
			if (securityTokenHandlers.CanReadToken(reader))
			{
				return securityTokenHandlers.ReadToken(reader);
			}
			return null;
		}

		public SecurityToken ReadToken(string tokenXml, XmlDictionaryReaderQuotas readerQuotas)
		{
			if (string.IsNullOrEmpty(tokenXml))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("tokenXml");
			}
			try
			{
				using StringReader input = new StringReader(tokenXml);
				using XmlDictionaryReader xmlDictionaryReader = new IdentityModelWrappedXmlDictionaryReader(XmlReader.Create(input), readerQuotas);
				xmlDictionaryReader.MoveToContent();
				string localName = xmlDictionaryReader.LocalName;
				string namespaceURI = xmlDictionaryReader.NamespaceURI;
				SecurityToken securityToken = ReadToken(xmlDictionaryReader);
				if (securityToken == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4014", localName, namespaceURI)));
				}
				return securityToken;
			}
			catch (Microsoft.IdentityModel.Tokens.EncryptedTokenDecryptionFailedException innerException)
			{
				string text = ((_serviceConfiguration.ServiceCertificate != null) ? ("[Thumbprint] " + _serviceConfiguration.ServiceCertificate.Thumbprint) : SR.GetString("NoCert"));
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID1044", text), innerException));
			}
		}

		public IClaimsPrincipal AuthenticateToken(SecurityToken token, bool ensureBearerToken, string endpointUri)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if (ensureBearerToken && token.SecurityKeys != null && token.SecurityKeys.Count != 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID1020")));
			}
			ClaimsIdentityCollection identities = _serviceConfiguration.SecurityTokenHandlers.ValidateToken(token);
			return _serviceConfiguration.ClaimsAuthenticationManager.Authenticate(endpointUri, ClaimsPrincipal.CreateFromIdentities(identities));
		}

		public void ComputeSessionTokenLifeTime(SecurityToken securityToken, out DateTime validFrom, out DateTime validTo)
		{
			TimeSpan configuredSessionTokenLifeTime = ConfiguredSessionTokenLifeTime;
			validFrom = DateTime.UtcNow;
			validTo = DateTimeUtil.AddNonNegative(validFrom, configuredSessionTokenLifeTime);
			if (securityToken != null)
			{
				if (validFrom < securityToken.ValidFrom)
				{
					validFrom = securityToken.ValidFrom;
				}
				if (validTo > securityToken.ValidTo)
				{
					validTo = securityToken.ValidTo;
				}
			}
		}
	}
}
