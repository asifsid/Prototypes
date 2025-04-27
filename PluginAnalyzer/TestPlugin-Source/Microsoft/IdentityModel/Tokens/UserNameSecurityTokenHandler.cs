using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public abstract class UserNameSecurityTokenHandler : SecurityTokenHandler
	{
		private bool _retainPassword;

		public virtual bool RetainPassword
		{
			get
			{
				return _retainPassword;
			}
			set
			{
				_retainPassword = value;
			}
		}

		public override bool CanWriteToken => true;

		public override Type TokenType => typeof(UserNameSecurityToken);

		public override bool CanReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			return reader.IsStartElement("UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
		}

		public override string[] GetTokenTypeIdentifiers()
		{
			return new string[1] { "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/UserName" };
		}

		public override SecurityToken ReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (!CanReadToken(reader))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4065", "Username", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", reader.LocalName, reader.NamespaceURI)));
			}
			string text = null;
			string password = null;
			reader.MoveToContent();
			string attribute = reader.GetAttribute("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
			reader.ReadStartElement("UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
			while (reader.IsStartElement())
			{
				if (reader.IsStartElement("Username", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
				{
					text = reader.ReadElementString();
					continue;
				}
				if (reader.IsStartElement("Password", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
				{
					string attribute2 = reader.GetAttribute("Type", null);
					if (!string.IsNullOrEmpty(attribute2) && !StringComparer.Ordinal.Equals(attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID4059", attribute2, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText")));
					}
					password = reader.ReadElementString();
					continue;
				}
				if (reader.IsStartElement("Nonce", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
				{
					reader.Skip();
					continue;
				}
				if (reader.IsStartElement("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"))
				{
					reader.Skip();
					continue;
				}
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new XmlException(SR.GetString("ID4060", reader.LocalName, reader.NamespaceURI, "UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")));
			}
			reader.ReadEndElement();
			if (string.IsNullOrEmpty(text))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4061"));
			}
			if (!string.IsNullOrEmpty(attribute))
			{
				return new UserNameSecurityToken(text, password, attribute);
			}
			return new UserNameSecurityToken(text, password);
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
			UserNameSecurityToken userNameSecurityToken = token as UserNameSecurityToken;
			if (userNameSecurityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("token", SR.GetString("ID0018", typeof(UserNameSecurityToken)));
			}
			writer.WriteStartElement("UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
			if (!string.IsNullOrEmpty(token.Id))
			{
				writer.WriteAttributeString("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", token.Id);
			}
			writer.WriteElementString("Username", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", userNameSecurityToken.UserName);
			if (userNameSecurityToken.Password != null)
			{
				writer.WriteStartElement("Password", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
				writer.WriteAttributeString("Type", null, "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
				writer.WriteString(userNameSecurityToken.Password);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.Flush();
		}
	}
}
