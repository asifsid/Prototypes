using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Diagnostics
{
	internal class TokenTraceRecord : TraceRecord
	{
		internal new const string ElementName = "TokenTraceRecord";

		internal new const string _eventId = "http://schemas.microsoft.com/2009/06/IdentityModel/TokenTraceRecord";

		private SecurityToken _securityToken;

		public override string EventId => "http://schemas.microsoft.com/2009/06/IdentityModel/TokenTraceRecord";

		public TokenTraceRecord(SecurityToken securityToken)
		{
			_securityToken = securityToken;
		}

		private void WriteSessionToken(XmlWriter writer, Microsoft.IdentityModel.Tokens.SessionSecurityToken sessionToken)
		{
			Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler orCreateSessionSecurityTokenHandler = GetOrCreateSessionSecurityTokenHandler();
			XmlDictionaryWriter writer2 = XmlDictionaryWriter.CreateDictionaryWriter(writer);
			orCreateSessionSecurityTokenHandler.WriteToken(writer2, sessionToken);
		}

		private static Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler GetOrCreateSessionSecurityTokenHandler()
		{
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlerCollection = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
			Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler sessionSecurityTokenHandler = securityTokenHandlerCollection[typeof(Microsoft.IdentityModel.Tokens.SessionSecurityToken)] as Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler;
			if (sessionSecurityTokenHandler == null)
			{
				sessionSecurityTokenHandler = new Microsoft.IdentityModel.Tokens.SessionSecurityTokenHandler();
				securityTokenHandlerCollection.AddOrReplace(sessionSecurityTokenHandler);
			}
			return sessionSecurityTokenHandler;
		}

		private static string GenerateSessionIdFromCookie(byte[] cookie)
		{
			byte[] inArray;
			using (HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.NewDefaultHash())
			{
				inArray = hashAlgorithm.ComputeHash(cookie);
			}
			return Convert.ToBase64String(inArray);
		}

		public override void WriteTo(XmlWriter writer)
		{
			writer.WriteStartElement("TokenTraceRecord");
			writer.WriteAttributeString("xmlns", EventId);
			writer.WriteStartElement("SecurityToken");
			writer.WriteAttributeString("Type", _securityToken.GetType().ToString());
			if (_securityToken is Microsoft.IdentityModel.Tokens.SessionSecurityToken)
			{
				WriteSessionToken(writer, _securityToken as Microsoft.IdentityModel.Tokens.SessionSecurityToken);
			}
			else
			{
				Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlerCollection = Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
				if (securityTokenHandlerCollection.CanWriteToken(_securityToken))
				{
					securityTokenHandlerCollection.WriteToken(writer, _securityToken);
				}
				else
				{
					writer.WriteElementString("Warning", SR.GetString("TraceUnableToWriteToken", _securityToken.GetType().ToString()));
				}
			}
			writer.WriteEndElement();
		}
	}
}
