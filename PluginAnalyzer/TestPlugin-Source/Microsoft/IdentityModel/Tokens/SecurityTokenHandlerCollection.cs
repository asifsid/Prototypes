using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityTokenHandlerCollection : Collection<SecurityTokenHandler>
	{
		internal static int _defaultHandlerCollectionCount = 8;

		private Dictionary<string, SecurityTokenHandler> _byIdentifier = new Dictionary<string, SecurityTokenHandler>();

		private Dictionary<Type, SecurityTokenHandler> _byType = new Dictionary<Type, SecurityTokenHandler>();

		private SecurityTokenHandlerConfiguration _configuration;

		public SecurityTokenHandler this[string tokenTypeIdentifier]
		{
			get
			{
				if (string.IsNullOrEmpty(tokenTypeIdentifier))
				{
					return null;
				}
				_byIdentifier.TryGetValue(tokenTypeIdentifier, out var value);
				return value;
			}
		}

		public SecurityTokenHandler this[SecurityToken token]
		{
			get
			{
				if (token == null)
				{
					return null;
				}
				return this[token.GetType()];
			}
		}

		public SecurityTokenHandler this[Type tokenType]
		{
			get
			{
				SecurityTokenHandler value = null;
				if ((object)tokenType != null)
				{
					_byType.TryGetValue(tokenType, out value);
				}
				return value;
			}
		}

		public SecurityTokenHandlerConfiguration Configuration => _configuration;

		public IEnumerable<Type> TokenTypes => _byType.Keys;

		public IEnumerable<string> TokenTypeIdentifiers => _byIdentifier.Keys;

		public static SecurityTokenHandlerCollection CreateDefaultSecurityTokenHandlerCollection()
		{
			return CreateDefaultSecurityTokenHandlerCollection(new SecurityTokenHandlerConfiguration());
		}

		public static SecurityTokenHandlerCollection CreateDefaultSecurityTokenHandlerCollection(SecurityTokenHandlerConfiguration configuration)
		{
			SecurityTokenHandlerCollection securityTokenHandlerCollection = new SecurityTokenHandlerCollection(new SecurityTokenHandler[8]
			{
				new KerberosSecurityTokenHandler(),
				new RsaSecurityTokenHandler(),
				new Saml11SecurityTokenHandler(),
				new Microsoft.IdentityModel.Tokens.Saml2.Saml2SecurityTokenHandler(),
				new WindowsUserNameSecurityTokenHandler(),
				new X509SecurityTokenHandler(),
				new EncryptedSecurityTokenHandler(),
				new SessionSecurityTokenHandler()
			}, configuration);
			_defaultHandlerCollectionCount = securityTokenHandlerCollection.Count;
			return securityTokenHandlerCollection;
		}

		public SecurityTokenHandlerCollection()
			: this(new SecurityTokenHandlerConfiguration())
		{
		}

		public SecurityTokenHandlerCollection(SecurityTokenHandlerConfiguration configuration)
		{
			if (configuration == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("configuration");
			}
			_configuration = configuration;
		}

		public SecurityTokenHandlerCollection(IEnumerable<SecurityTokenHandler> handlers)
			: this(handlers, new SecurityTokenHandlerConfiguration())
		{
		}

		public SecurityTokenHandlerCollection(IEnumerable<SecurityTokenHandler> handlers, SecurityTokenHandlerConfiguration configuration)
		{
			if (handlers == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("handlers");
			}
			_configuration = configuration;
			foreach (SecurityTokenHandler handler in handlers)
			{
				Add(handler);
			}
		}

		public void AddOrReplace(SecurityTokenHandler handler)
		{
			if (handler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("handler");
			}
			Type tokenType = handler.TokenType;
			if ((object)tokenType != null && _byType.ContainsKey(tokenType))
			{
				Remove(this[tokenType]);
			}
			else
			{
				string[] tokenTypeIdentifiers = handler.GetTokenTypeIdentifiers();
				if (tokenTypeIdentifiers != null)
				{
					string[] array = tokenTypeIdentifiers;
					foreach (string text in array)
					{
						if (text != null && _byIdentifier.ContainsKey(text))
						{
							Remove(this[text]);
							break;
						}
					}
				}
			}
			Add(handler);
		}

		private void AddToDictionaries(SecurityTokenHandler handler)
		{
			if (handler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("handler");
			}
			bool flag = false;
			string[] tokenTypeIdentifiers = handler.GetTokenTypeIdentifiers();
			if (tokenTypeIdentifiers != null)
			{
				string[] array = tokenTypeIdentifiers;
				foreach (string text in array)
				{
					if (text != null)
					{
						_byIdentifier.Add(text, handler);
						flag = true;
					}
				}
			}
			Type tokenType = handler.TokenType;
			if ((object)handler.TokenType != null)
			{
				try
				{
					_byType.Add(tokenType, handler);
				}
				catch
				{
					if (flag)
					{
						RemoveFromDictionaries(handler);
					}
					throw;
				}
			}
			handler.ContainingCollection = this;
			if (handler.Configuration == null)
			{
				handler.Configuration = _configuration;
			}
		}

		private void RemoveFromDictionaries(SecurityTokenHandler handler)
		{
			string[] tokenTypeIdentifiers = handler.GetTokenTypeIdentifiers();
			if (tokenTypeIdentifiers != null)
			{
				string[] array = tokenTypeIdentifiers;
				foreach (string text in array)
				{
					if (text != null)
					{
						_byIdentifier.Remove(text);
					}
				}
			}
			Type tokenType = handler.TokenType;
			if ((object)tokenType != null && _byType.ContainsKey(tokenType))
			{
				_byType.Remove(tokenType);
			}
			handler.ContainingCollection = null;
			handler.Configuration = null;
		}

		protected override void ClearItems()
		{
			base.ClearItems();
			_byIdentifier.Clear();
			_byType.Clear();
		}

		protected override void InsertItem(int index, SecurityTokenHandler item)
		{
			base.InsertItem(index, item);
			try
			{
				AddToDictionaries(item);
			}
			catch
			{
				base.RemoveItem(index);
				throw;
			}
		}

		protected override void RemoveItem(int index)
		{
			SecurityTokenHandler handler = base.Items[index];
			base.RemoveItem(index);
			RemoveFromDictionaries(handler);
		}

		protected override void SetItem(int index, SecurityTokenHandler item)
		{
			SecurityTokenHandler securityTokenHandler = base.Items[index];
			base.SetItem(index, item);
			RemoveFromDictionaries(securityTokenHandler);
			try
			{
				AddToDictionaries(item);
			}
			catch
			{
				base.SetItem(index, securityTokenHandler);
				AddToDictionaries(securityTokenHandler);
				throw;
			}
		}

		public bool CanReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			using (IEnumerator<SecurityTokenHandler> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SecurityTokenHandler current = enumerator.Current;
					if (current != null && current.CanReadToken(reader))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool CanWriteToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			SecurityTokenHandler securityTokenHandler = this[token];
			if (securityTokenHandler != null && securityTokenHandler.CanWriteToken)
			{
				return true;
			}
			return false;
		}

		public SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
		{
			if (tokenDescriptor == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenDescriptor");
			}
			SecurityTokenHandler securityTokenHandler = this[tokenDescriptor.TokenType];
			if (securityTokenHandler == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4020", tokenDescriptor.TokenType)));
			}
			return securityTokenHandler.CreateToken(tokenDescriptor);
		}

		public ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			SecurityTokenHandler securityTokenHandler = this[token];
			if (securityTokenHandler == null || !securityTokenHandler.CanValidateToken)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4011", token.GetType())));
			}
			return securityTokenHandler.ValidateToken(token);
		}

		public SecurityToken ReadToken(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			using (IEnumerator<SecurityTokenHandler> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SecurityTokenHandler current = enumerator.Current;
					if (current != null && current.CanReadToken(reader))
					{
						return current.ReadToken(reader);
					}
				}
			}
			return null;
		}

		public void WriteToken(XmlWriter writer, SecurityToken token)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			SecurityTokenHandler securityTokenHandler = this[token];
			if (securityTokenHandler == null || !securityTokenHandler.CanWriteToken)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4010", token.GetType())));
			}
			securityTokenHandler.WriteToken(writer, token);
		}
	}
}
