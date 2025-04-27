using System;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Protocols.XmlEncryption;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class EncryptedSecurityTokenHandler : SecurityTokenHandler
	{
		private static string[] _tokenTypeIdentifiers;

		private SecurityTokenSerializer _keyInfoSerializer;

		private object _syncObject = new object();

		public override bool CanWriteToken => true;

		public SecurityTokenSerializer KeyInfoSerializer
		{
			get
			{
				if (_keyInfoSerializer == null)
				{
					lock (_syncObject)
					{
						if (_keyInfoSerializer == null)
						{
							SecurityTokenHandlerCollection securityTokenHandlerCollection = ((base.ContainingCollection != null) ? base.ContainingCollection : SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection());
							_keyInfoSerializer = new SecurityTokenSerializerAdapter(securityTokenHandlerCollection, SecurityVersion.WSSecurity11, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, emitBspAttributes: false, null, null, null);
						}
					}
				}
				return _keyInfoSerializer;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_keyInfoSerializer = value;
			}
		}

		public override Type TokenType => typeof(EncryptedSecurityToken);

		public override bool CanReadKeyIdentifierClause(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.IsStartElement("EncryptedKey", "http://www.w3.org/2001/04/xmlenc#");
		}

		public override bool CanReadToken(XmlReader reader)
		{
			return EncryptedDataElement.CanReadFrom(reader);
		}

		public override SecurityToken ReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (base.Configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4274"));
			}
			if (base.Configuration.ServiceTokenResolver == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4276"));
			}
			EncryptedDataElement encryptedDataElement = new EncryptedDataElement(KeyInfoSerializer);
			encryptedDataElement.ReadXml(XmlDictionaryReader.CreateDictionaryReader(reader));
			SecurityKey key = null;
			foreach (SecurityKeyIdentifierClause item in encryptedDataElement.KeyIdentifier)
			{
				base.Configuration.ServiceTokenResolver.TryResolveSecurityKey(item, out key);
				if (key != null)
				{
					break;
				}
			}
			if (key == null && encryptedDataElement.KeyIdentifier.CanCreateKey)
			{
				key = encryptedDataElement.KeyIdentifier.CreateKey();
			}
			if (key == null)
			{
				if (encryptedDataElement.KeyIdentifier.TryFind<EncryptedKeyIdentifierClause>(out var _))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new EncryptedTokenDecryptionFailedException(SR.GetString("ID4036", XmlUtil.SerializeSecurityKeyIdentifier(encryptedDataElement.KeyIdentifier, WSSecurityTokenSerializer.DefaultInstance))));
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new EncryptedTokenDecryptionFailedException(SR.GetString("ID4036", encryptedDataElement.KeyIdentifier.ToString())));
			}
			SymmetricSecurityKey symmetricSecurityKey = key as SymmetricSecurityKey;
			if (symmetricSecurityKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID4023")));
			}
			byte[] buffer;
			using (SymmetricAlgorithm algorithm = symmetricSecurityKey.GetSymmetricAlgorithm(encryptedDataElement.Algorithm))
			{
				buffer = encryptedDataElement.Decrypt(algorithm);
			}
			using XmlReader xmlReader = XmlDictionaryReader.CreateTextReader(buffer, XmlDictionaryReaderQuotas.Max);
			if (base.ContainingCollection != null && base.ContainingCollection.CanReadToken(xmlReader))
			{
				return base.ContainingCollection.ReadToken(xmlReader);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4014", xmlReader.LocalName, xmlReader.NamespaceURI));
		}

		public override SecurityKeyIdentifierClause ReadKeyIdentifierClause(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (reader.IsStartElement("EncryptedKey", "http://www.w3.org/2001/04/xmlenc#"))
			{
				EncryptedKeyElement encryptedKeyElement = new EncryptedKeyElement(KeyInfoSerializer);
				encryptedKeyElement.ReadXml(XmlDictionaryReader.CreateDictionaryReader(reader));
				return new EncryptedKeyIdentifierClause(encryptedKeyElement.CipherData.CipherValue, encryptedKeyElement.Algorithm, encryptedKeyElement.KeyIdentifier);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3275", reader.Name, reader.NamespaceURI)));
		}

		[Conditional("DEBUG")]
		private static void DebugEncryptedTokenClearText(byte[] bytes, Encoding encoding)
		{
			encoding.GetString(bytes);
		}

		public override string[] GetTokenTypeIdentifiers()
		{
			return _tokenTypeIdentifiers;
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
			EncryptedSecurityToken encryptedSecurityToken = token as EncryptedSecurityToken;
			if (encryptedSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID4024"));
			}
			if (base.ContainingCollection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4279"));
			}
			EncryptedDataElement encryptedDataElement = new EncryptedDataElement(KeyInfoSerializer);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (XmlDictionaryWriter writer2 = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false))
				{
					SecurityTokenHandler securityTokenHandler = base.ContainingCollection[encryptedSecurityToken.Token.GetType()];
					if (securityTokenHandler == null)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4224", encryptedSecurityToken.Token.GetType()));
					}
					securityTokenHandler.WriteToken(writer2, encryptedSecurityToken.Token);
				}
				Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials = encryptedSecurityToken.EncryptingCredentials;
				encryptedDataElement.Type = "http://www.w3.org/2001/04/xmlenc#Element";
				encryptedDataElement.KeyIdentifier = encryptingCredentials.SecurityKeyIdentifier;
				encryptedDataElement.Algorithm = encryptingCredentials.Algorithm;
				SymmetricSecurityKey symmetricSecurityKey = encryptingCredentials.SecurityKey as SymmetricSecurityKey;
				if (symmetricSecurityKey == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenException(SR.GetString("ID3064")));
				}
				using SymmetricAlgorithm algorithm = symmetricSecurityKey.GetSymmetricAlgorithm(encryptingCredentials.Algorithm);
				byte[] buffer = memoryStream.GetBuffer();
				encryptedDataElement.Encrypt(algorithm, buffer, 0, (int)memoryStream.Length);
			}
			encryptedDataElement.WriteXml(writer, KeyInfoSerializer);
		}

		static EncryptedSecurityTokenHandler()
		{
			string[] array = (_tokenTypeIdentifiers = new string[1]);
		}
	}
}
