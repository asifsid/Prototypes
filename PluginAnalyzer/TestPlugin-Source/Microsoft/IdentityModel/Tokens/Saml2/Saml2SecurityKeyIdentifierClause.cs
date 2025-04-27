using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2SecurityKeyIdentifierClause : SecurityKeyIdentifierClause
	{
		private Saml2Assertion _assertion;

		public Saml2Assertion Assertion => _assertion;

		public Saml2SecurityKeyIdentifierClause(Saml2Assertion assertion)
			: base(typeof(Saml2SecurityKeyIdentifierClause).ToString())
		{
			_assertion = assertion;
		}
	}
}
