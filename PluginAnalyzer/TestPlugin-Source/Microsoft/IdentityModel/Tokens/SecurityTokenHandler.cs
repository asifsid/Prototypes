using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public abstract class SecurityTokenHandler
	{
		private SecurityTokenHandlerCollection _collection;

		private SecurityTokenHandlerConfiguration _configuration;

		public SecurityTokenHandlerConfiguration Configuration
		{
			get
			{
				return _configuration;
			}
			set
			{
				_configuration = value;
			}
		}

		public SecurityTokenHandlerCollection ContainingCollection
		{
			get
			{
				return _collection;
			}
			set
			{
				_collection = value;
			}
		}

		public virtual bool CanWriteToken => false;

		public abstract Type TokenType { get; }

		public virtual bool CanValidateToken => false;

		public virtual bool CanReadToken(XmlReader reader)
		{
			return false;
		}

		public virtual SecurityToken ReadToken(XmlReader reader)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID4008", "SecurityTokenHandler", "ReadToken")));
		}

		public virtual SecurityToken ReadToken(XmlReader reader, SecurityTokenResolver tokenResolver)
		{
			return ReadToken(reader);
		}

		protected virtual void DetectReplayedTokens(SecurityToken token)
		{
		}

		public virtual void WriteToken(XmlWriter writer, SecurityToken token)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID4008", "SecurityTokenHandler", "WriteToken")));
		}

		public virtual bool CanReadKeyIdentifierClause(XmlReader reader)
		{
			return false;
		}

		public virtual SecurityKeyIdentifierClause ReadKeyIdentifierClause(XmlReader reader)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID4008", "SecurityTokenHandler", "ReadKeyIdentifierClause")));
		}

		public virtual bool CanWriteKeyIdentifierClause(SecurityKeyIdentifierClause securityKeyIdentifierClause)
		{
			return false;
		}

		public virtual void WriteKeyIdentifierClause(XmlWriter writer, SecurityKeyIdentifierClause securityKeyIdentifierClause)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID4008", "SecurityTokenHandler", "WriteKeyIdentifierClause")));
		}

		public virtual SecurityToken CreateToken(SecurityTokenDescriptor tokenDescriptor)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID4008", "SecurityTokenHandler", "CreateToken")));
		}

		public virtual SecurityKeyIdentifierClause CreateSecurityTokenReference(SecurityToken token, bool attached)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID4008", "SecurityTokenHandler", "CreateSecurityTokenReference")));
		}

		public abstract string[] GetTokenTypeIdentifiers();

		public virtual ClaimsIdentityCollection ValidateToken(SecurityToken token)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotImplementedException(SR.GetString("ID4008", "SecurityTokenHandler", "ValidateToken")));
		}
	}
}
