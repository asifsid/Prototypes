using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2SecurityToken : SecurityToken
	{
		private Saml2Assertion _assertion;

		private ReadOnlyCollection<SecurityKey> _keys;

		private SecurityToken _issuerToken;

		public Saml2Assertion Assertion => _assertion;

		public override string Id => _assertion.Id.Value;

		public SecurityToken IssuerToken => _issuerToken;

		public override ReadOnlyCollection<SecurityKey> SecurityKeys => _keys;

		public override DateTime ValidFrom
		{
			get
			{
				if (_assertion.Conditions != null && _assertion.Conditions.NotBefore.HasValue)
				{
					return _assertion.Conditions.NotBefore.Value;
				}
				return DateTime.MinValue;
			}
		}

		public override DateTime ValidTo
		{
			get
			{
				if (_assertion.Conditions != null && _assertion.Conditions.NotOnOrAfter.HasValue)
				{
					return _assertion.Conditions.NotOnOrAfter.Value;
				}
				return DateTime.MaxValue;
			}
		}

		public Saml2SecurityToken(Saml2Assertion assertion)
			: this(assertion, EmptyReadOnlyCollection<SecurityKey>.Instance, null)
		{
		}

		public Saml2SecurityToken(Saml2Assertion assertion, ReadOnlyCollection<SecurityKey> keys, SecurityToken issuerToken)
		{
			if (assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertion");
			}
			if (keys == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keys");
			}
			_assertion = assertion;
			_keys = keys;
			_issuerToken = issuerToken;
		}

		public override bool CanCreateKeyIdentifierClause<T>()
		{
			if ((object)typeof(T) != typeof(Saml2AssertionKeyIdentifierClause))
			{
				return base.CanCreateKeyIdentifierClause<T>();
			}
			return true;
		}

		public override T CreateKeyIdentifierClause<T>()
		{
			if ((object)typeof(T) == typeof(Saml2AssertionKeyIdentifierClause))
			{
				return new Saml2AssertionKeyIdentifierClause(_assertion.Id.Value) as T;
			}
			if ((object)typeof(T) == typeof(SamlAssertionKeyIdentifierClause))
			{
				return new WrappedSaml2AssertionKeyIdentifierClause(new Saml2AssertionKeyIdentifierClause(_assertion.Id.Value)) as T;
			}
			return base.CreateKeyIdentifierClause<T>();
		}

		public override bool MatchesKeyIdentifierClause(SecurityKeyIdentifierClause keyIdentifierClause)
		{
			if (!Saml2AssertionKeyIdentifierClause.Matches(Id, keyIdentifierClause))
			{
				return base.MatchesKeyIdentifierClause(keyIdentifierClause);
			}
			return true;
		}
	}
}
