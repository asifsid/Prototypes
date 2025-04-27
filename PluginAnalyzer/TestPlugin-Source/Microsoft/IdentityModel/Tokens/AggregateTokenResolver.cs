using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class AggregateTokenResolver : SecurityTokenResolver
	{
		private List<SecurityTokenResolver> _tokenResolvers = new List<SecurityTokenResolver>();

		public ReadOnlyCollection<SecurityTokenResolver> TokenResolvers => _tokenResolvers.AsReadOnly();

		public AggregateTokenResolver(IEnumerable<SecurityTokenResolver> tokenResolvers)
		{
			if (tokenResolvers == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("tokenResolvers");
			}
			AddNonEmptyResolvers(tokenResolvers);
		}

		protected override bool TryResolveSecurityKeyCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key)
		{
			if (keyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
			}
			key = null;
			foreach (SecurityTokenResolver tokenResolver in _tokenResolvers)
			{
				if (tokenResolver.TryResolveSecurityKey(keyIdentifierClause, out key))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool TryResolveTokenCore(SecurityKeyIdentifier keyIdentifier, out SecurityToken token)
		{
			if (keyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifer");
			}
			token = null;
			foreach (SecurityTokenResolver tokenResolver in _tokenResolvers)
			{
				if (tokenResolver.TryResolveToken(keyIdentifier, out token))
				{
					return true;
				}
			}
			return false;
		}

		protected override bool TryResolveTokenCore(SecurityKeyIdentifierClause keyIdentifierClause, out SecurityToken token)
		{
			if (keyIdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifierClause");
			}
			token = null;
			foreach (SecurityTokenResolver tokenResolver in _tokenResolvers)
			{
				if (tokenResolver.TryResolveToken(keyIdentifierClause, out token))
				{
					return true;
				}
			}
			return false;
		}

		private void AddNonEmptyResolvers(IEnumerable<SecurityTokenResolver> resolvers)
		{
			foreach (SecurityTokenResolver resolver in resolvers)
			{
				if (resolver != null && resolver != EmptySecurityTokenResolver.Instance)
				{
					_tokenResolvers.Add(resolver);
				}
			}
		}
	}
}
