using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class RsaSecurityTokenHandler : SecurityTokenHandler
	{
		private static string[] _tokenTypeIdentifiers = new string[1] { "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/Rsa" };

		public override bool CanValidateToken => true;

		public override bool CanWriteToken => true;

		public override Type TokenType => typeof(RsaSecurityToken);

		public override bool CanReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#");
		}

		public override string[] GetTokenTypeIdentifiers()
		{
			return _tokenTypeIdentifiers;
		}

		public override SecurityToken ReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateDictionaryReader(reader);
			if (!xmlDictionaryReader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4065", "KeyInfo", "http://www.w3.org/2000/09/xmldsig#", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI)));
			}
			xmlDictionaryReader.ReadStartElement();
			if (!xmlDictionaryReader.IsStartElement("KeyValue", "http://www.w3.org/2000/09/xmldsig#"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4065", "KeyValue", "http://www.w3.org/2000/09/xmldsig#", xmlDictionaryReader.LocalName, xmlDictionaryReader.NamespaceURI)));
			}
			xmlDictionaryReader.ReadStartElement();
			RSA rSA = new RSACryptoServiceProvider();
			rSA.FromXmlString(xmlDictionaryReader.ReadOuterXml());
			xmlDictionaryReader.ReadEndElement();
			xmlDictionaryReader.ReadEndElement();
			return new RsaSecurityToken(rSA);
		}

		public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			RsaSecurityToken rsaSecurityToken = (RsaSecurityToken)token;
			if (rsaSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID0018", typeof(RsaSecurityToken)));
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			IClaimsIdentity claimsIdentity = new RsaClaimsIdentity(new Claim[1]
			{
				new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa", rsaSecurityToken.Rsa.ToXmlString(includePrivateParameters: false), "http://www.w3.org/2000/09/xmldsig#RSAKeyValue", "LOCAL AUTHORITY")
			}, "Signature");
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, DateTimeFormats.Generated), "http://www.w3.org/2001/XMLSchema#dateTime"));
			claimsIdentity.Claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/signature"));
			if (base.Configuration.SaveBootstrapTokens)
			{
				claimsIdentity.BootstrapToken = token;
			}
			return new ClaimsIdentityCollection(new IClaimsIdentity[1] { claimsIdentity });
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
			RsaSecurityToken rsaSecurityToken = token as RsaSecurityToken;
			if (rsaSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID0018", typeof(RsaSecurityToken)));
			}
			RSAParameters rSAParameters = rsaSecurityToken.Rsa.ExportParameters(includePrivateParameters: false);
			writer.WriteStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#");
			writer.WriteStartElement("KeyValue", "http://www.w3.org/2000/09/xmldsig#");
			writer.WriteStartElement("RsaKeyValue", "http://www.w3.org/2000/09/xmldsig#");
			writer.WriteStartElement("Modulus", "http://www.w3.org/2000/09/xmldsig#");
			byte[] modulus = rSAParameters.Modulus;
			writer.WriteBase64(modulus, 0, modulus.Length);
			writer.WriteEndElement();
			writer.WriteStartElement("Exponent", "http://www.w3.org/2000/09/xmldsig#");
			byte[] exponent = rSAParameters.Exponent;
			writer.WriteBase64(exponent, 0, exponent.Length);
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
	}
}
