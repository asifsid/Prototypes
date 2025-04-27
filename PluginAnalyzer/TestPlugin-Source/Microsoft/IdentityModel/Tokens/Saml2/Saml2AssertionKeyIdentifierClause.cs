using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2AssertionKeyIdentifierClause : SecurityKeyIdentifierClause
	{
		private string _id;

		public new string Id => _id;

		public Saml2AssertionKeyIdentifierClause(string id)
			: this(id, null, 0)
		{
		}

		public Saml2AssertionKeyIdentifierClause(string id, byte[] derivationNonce, int derivationLength)
			: base(null, derivationNonce, derivationLength)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("id");
			}
			_id = id;
		}

		public override bool Matches(SecurityKeyIdentifierClause keyIdentifierClause)
		{
			if (!object.ReferenceEquals(this, keyIdentifierClause))
			{
				return Matches(_id, keyIdentifierClause);
			}
			return true;
		}

		public static bool Matches(string assertionId, SecurityKeyIdentifierClause keyIdentifierClause)
		{
			if (string.IsNullOrEmpty(assertionId))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertionId");
			}
			if (keyIdentifierClause == null)
			{
				return false;
			}
			Saml2AssertionKeyIdentifierClause saml2AssertionKeyIdentifierClause = keyIdentifierClause as Saml2AssertionKeyIdentifierClause;
			if (saml2AssertionKeyIdentifierClause != null && StringComparer.Ordinal.Equals(assertionId, saml2AssertionKeyIdentifierClause.Id))
			{
				return true;
			}
			SamlAssertionKeyIdentifierClause samlAssertionKeyIdentifierClause = keyIdentifierClause as SamlAssertionKeyIdentifierClause;
			if (samlAssertionKeyIdentifierClause != null && StringComparer.Ordinal.Equals(assertionId, samlAssertionKeyIdentifierClause.AssertionId))
			{
				return true;
			}
			return false;
		}

		public override string ToString()
		{
			return "Saml2AssertionKeyIdentifierClause( Id = '" + _id + "' )";
		}
	}
}
