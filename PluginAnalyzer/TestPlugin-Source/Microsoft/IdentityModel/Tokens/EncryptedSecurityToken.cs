using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class EncryptedSecurityToken : SecurityToken
	{
		private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _encryptingCredentials;

		private SecurityToken _realToken;

		public Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials EncryptingCredentials => _encryptingCredentials;

		public override string Id => _realToken.Id;

		public override ReadOnlyCollection<SecurityKey> SecurityKeys => _realToken.SecurityKeys;

		public SecurityToken Token => _realToken;

		public override DateTime ValidFrom => _realToken.ValidFrom;

		public override DateTime ValidTo => _realToken.ValidTo;

		public EncryptedSecurityToken(SecurityToken token, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials encryptingCredentials)
		{
			if (token == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("token");
			}
			if (encryptingCredentials == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("encryptingCredentials");
			}
			_encryptingCredentials = encryptingCredentials;
			_realToken = token;
		}

		public override bool CanCreateKeyIdentifierClause<T>()
		{
			return _realToken.CanCreateKeyIdentifierClause<T>();
		}

		public override T CreateKeyIdentifierClause<T>()
		{
			return _realToken.CreateKeyIdentifierClause<T>();
		}

		public override bool MatchesKeyIdentifierClause(SecurityKeyIdentifierClause keyIdentifierClause)
		{
			return _realToken.MatchesKeyIdentifierClause(keyIdentifierClause);
		}

		public override SecurityKey ResolveKeyIdentifierClause(SecurityKeyIdentifierClause keyIdentifierClause)
		{
			return _realToken.ResolveKeyIdentifierClause(keyIdentifierClause);
		}
	}
}
