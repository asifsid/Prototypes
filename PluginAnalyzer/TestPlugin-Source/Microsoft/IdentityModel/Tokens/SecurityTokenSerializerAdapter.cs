using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;
using Microsoft.IdentityModel.Protocols.XmlSignature;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SecurityTokenSerializerAdapter : WSSecurityTokenSerializer
	{
		private SecureConversationVersion _scVersion;

		private SecurityTokenHandlerCollection _securityTokenHandlers;

		private bool _mapExceptionsToSoapFaults;

		private ExceptionMapper _exceptionMapper = new ExceptionMapper();

		public bool MapExceptionsToSoapFaults
		{
			get
			{
				return _mapExceptionsToSoapFaults;
			}
			set
			{
				_mapExceptionsToSoapFaults = value;
			}
		}

		public SecurityTokenHandlerCollection SecurityTokenHandlers => _securityTokenHandlers;

		public ExceptionMapper ExceptionMapper
		{
			get
			{
				return _exceptionMapper;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_exceptionMapper = value;
			}
		}

		public SecurityTokenSerializerAdapter(SecurityTokenHandlerCollection securityTokenHandlerCollection)
			: this(securityTokenHandlerCollection, MessageSecurityVersion.Default.SecurityVersion)
		{
		}

		public SecurityTokenSerializerAdapter(SecurityTokenHandlerCollection securityTokenHandlerCollection, SecurityVersion securityVersion)
			: this(securityTokenHandlerCollection, securityVersion, emitBspAttributes: true, new SamlSerializer(), null, null)
		{
		}

		public SecurityTokenSerializerAdapter(SecurityTokenHandlerCollection securityTokenHandlerCollection, SecurityVersion securityVersion, bool emitBspAttributes, SamlSerializer samlSerializer, SecurityStateEncoder stateEncoder, IEnumerable<Type> knownTypes)
			: this(securityTokenHandlerCollection, securityVersion, TrustVersion.WSTrust13, SecureConversationVersion.WSSecureConversation13, emitBspAttributes, samlSerializer, stateEncoder, knownTypes)
		{
		}

		public SecurityTokenSerializerAdapter(SecurityTokenHandlerCollection securityTokenHandlerCollection, SecurityVersion securityVersion, TrustVersion trustVersion, SecureConversationVersion secureConversationVersion, bool emitBspAttributes, SamlSerializer samlSerializer, SecurityStateEncoder stateEncoder, IEnumerable<Type> knownTypes)
			: base(securityVersion, trustVersion, secureConversationVersion, emitBspAttributes, samlSerializer, stateEncoder, knownTypes)
		{
			if (securityTokenHandlerCollection == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlerCollection");
			}
			_scVersion = secureConversationVersion;
			_securityTokenHandlers = securityTokenHandlerCollection;
		}

		protected override bool CanReadTokenCore(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (_securityTokenHandlers.CanReadToken(reader))
			{
				return true;
			}
			return base.CanReadTokenCore(reader);
		}

		protected override bool CanWriteTokenCore(SecurityToken token)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if (_securityTokenHandlers.CanWriteToken(token))
			{
				return true;
			}
			return base.CanWriteTokenCore(token);
		}

		protected override SecurityToken ReadTokenCore(XmlReader reader, SecurityTokenResolver tokenResolver)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			try
			{
				foreach (SecurityTokenHandler securityTokenHandler in _securityTokenHandlers)
				{
					if (!securityTokenHandler.CanReadToken(reader))
					{
						continue;
					}
					SecurityToken securityToken = securityTokenHandler.ReadToken(reader, tokenResolver);
					SessionSecurityToken sessionSecurityToken = securityToken as SessionSecurityToken;
					if (sessionSecurityToken != null)
					{
						if (sessionSecurityToken.SecureConversationVersion != _scVersion)
						{
							throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID4053", sessionSecurityToken.SecureConversationVersion, _scVersion));
						}
						return sessionSecurityToken.SecurityContextSecurityToken;
					}
					return securityToken;
				}
				return base.ReadTokenCore(reader, tokenResolver);
			}
			catch (Exception ex)
			{
				if (!MapExceptionsToSoapFaults || !_exceptionMapper.HandleSecurityTokenProcessingException(ex))
				{
					throw;
				}
			}
			return null;
		}

		protected override void WriteTokenCore(XmlWriter writer, SecurityToken token)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			try
			{
				SecurityContextSecurityToken securityContextSecurityToken = token as SecurityContextSecurityToken;
				if (securityContextSecurityToken != null)
				{
					token = new SessionSecurityToken(securityContextSecurityToken, _scVersion);
				}
				SecurityTokenHandler securityTokenHandler = _securityTokenHandlers[token];
				if (securityTokenHandler != null && securityTokenHandler.CanWriteToken)
				{
					securityTokenHandler.WriteToken(writer, token);
				}
				else
				{
					base.WriteTokenCore(writer, token);
				}
			}
			catch (Exception ex)
			{
				if (!MapExceptionsToSoapFaults || !_exceptionMapper.HandleSecurityTokenProcessingException(ex))
				{
					throw;
				}
			}
		}

		protected override bool CanReadKeyIdentifierCore(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
			{
				return true;
			}
			return base.CanReadKeyIdentifierCore(reader);
		}

		protected override SecurityKeyIdentifier ReadKeyIdentifierCore(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
			{
				KeyInfo keyInfo = new KeyInfo(this);
				keyInfo.ReadXml(XmlDictionaryReader.CreateDictionaryReader(reader));
				return keyInfo.KeyIdentifier;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4192"));
		}

		protected override bool CanReadKeyIdentifierClauseCore(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			foreach (SecurityTokenHandler securityTokenHandler in _securityTokenHandlers)
			{
				if (securityTokenHandler.CanReadKeyIdentifierClause(reader))
				{
					return true;
				}
			}
			return base.CanReadKeyIdentifierClauseCore(reader);
		}

		protected override bool CanWriteKeyIdentifierClauseCore(SecurityKeyIdentifierClause keyIdentifierClause)
		{
			if (keyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
			}
			foreach (SecurityTokenHandler securityTokenHandler in _securityTokenHandlers)
			{
				if (securityTokenHandler.CanWriteKeyIdentifierClause(keyIdentifierClause))
				{
					return true;
				}
			}
			return base.CanWriteKeyIdentifierClauseCore(keyIdentifierClause);
		}

		protected override SecurityKeyIdentifierClause ReadKeyIdentifierClauseCore(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			try
			{
				foreach (SecurityTokenHandler securityTokenHandler in _securityTokenHandlers)
				{
					if (securityTokenHandler.CanReadKeyIdentifierClause(reader))
					{
						return securityTokenHandler.ReadKeyIdentifierClause(reader);
					}
				}
				return base.ReadKeyIdentifierClauseCore(reader);
			}
			catch (Exception ex)
			{
				if (!MapExceptionsToSoapFaults || !_exceptionMapper.HandleSecurityTokenProcessingException(ex))
				{
					throw;
				}
			}
			return null;
		}

		protected override void WriteKeyIdentifierClauseCore(XmlWriter writer, SecurityKeyIdentifierClause keyIdentifierClause)
		{
			if (writer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
			}
			if (keyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
			}
			try
			{
				foreach (SecurityTokenHandler securityTokenHandler in _securityTokenHandlers)
				{
					if (securityTokenHandler.CanWriteKeyIdentifierClause(keyIdentifierClause))
					{
						securityTokenHandler.WriteKeyIdentifierClause(writer, keyIdentifierClause);
						return;
					}
				}
				base.WriteKeyIdentifierClauseCore(writer, keyIdentifierClause);
			}
			catch (Exception ex)
			{
				if (!MapExceptionsToSoapFaults || !_exceptionMapper.HandleSecurityTokenProcessingException(ex))
				{
					throw;
				}
			}
		}
	}
}
