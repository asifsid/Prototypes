using System.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	internal class WrappedSaml2AssertionKeyIdentifierClause : SamlAssertionKeyIdentifierClause
	{
		private Saml2AssertionKeyIdentifierClause _clause;

		public override bool CanCreateKey => _clause.CanCreateKey;

		public Saml2AssertionKeyIdentifierClause WrappedClause => _clause;

		public WrappedSaml2AssertionKeyIdentifierClause(Saml2AssertionKeyIdentifierClause clause)
			: base(clause.Id)
		{
			_clause = clause;
		}

		public override SecurityKey CreateKey()
		{
			return _clause.CreateKey();
		}

		public override bool Matches(SecurityKeyIdentifierClause keyIdentifierClause)
		{
			return _clause.Matches(keyIdentifierClause);
		}
	}
}
