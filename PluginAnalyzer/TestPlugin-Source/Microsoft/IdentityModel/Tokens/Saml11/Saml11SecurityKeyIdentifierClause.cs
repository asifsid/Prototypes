using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml11
{
	[ComVisible(true)]
	public class Saml11SecurityKeyIdentifierClause : SecurityKeyIdentifierClause
	{
		private SamlAssertion _assertion;

		public SamlAssertion Assertion => _assertion;

		public Saml11SecurityKeyIdentifierClause(SamlAssertion assertion)
			: base(typeof(Saml11SecurityKeyIdentifierClause).ToString())
		{
			_assertion = assertion;
		}
	}
}
