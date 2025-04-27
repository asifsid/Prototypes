using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Tokens
{
	internal class WrappedSamlSecurityTokenAuthenticator : SecurityTokenAuthenticator
	{
		private WrappedSaml11SecurityTokenAuthenticator _wrappedSaml11SecurityTokenAuthenticator;

		private WrappedSaml2SecurityTokenAuthenticator _wrappedSaml2SecurityTokenAuthenticator;

		public WrappedSamlSecurityTokenAuthenticator(WrappedSaml11SecurityTokenAuthenticator wrappedSaml11SecurityTokenAuthenticator, WrappedSaml2SecurityTokenAuthenticator wrappedSaml2SecurityTokenAuthenticator)
		{
			if (wrappedSaml11SecurityTokenAuthenticator == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wrappedSaml11SecurityTokenAuthenticator");
			}
			if (wrappedSaml2SecurityTokenAuthenticator == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("wrappedSaml2SecurityTokenAuthenticator");
			}
			_wrappedSaml11SecurityTokenAuthenticator = wrappedSaml11SecurityTokenAuthenticator;
			_wrappedSaml2SecurityTokenAuthenticator = wrappedSaml2SecurityTokenAuthenticator;
		}

		protected override bool CanValidateTokenCore(SecurityToken token)
		{
			if (!_wrappedSaml11SecurityTokenAuthenticator.CanValidateToken(token))
			{
				return _wrappedSaml2SecurityTokenAuthenticator.CanValidateToken(token);
			}
			return true;
		}

		protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
		{
			if (_wrappedSaml11SecurityTokenAuthenticator.CanValidateToken(token))
			{
				return _wrappedSaml11SecurityTokenAuthenticator.ValidateToken(token);
			}
			if (_wrappedSaml2SecurityTokenAuthenticator.CanValidateToken(token))
			{
				return _wrappedSaml2SecurityTokenAuthenticator.ValidateToken(token);
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID4101", token.GetType().ToString())));
		}
	}
}
