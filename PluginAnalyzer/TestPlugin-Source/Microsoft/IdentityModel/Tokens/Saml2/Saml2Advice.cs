using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2Advice
	{
		private Collection<Saml2Id> _assertionIdReferences = new Collection<Saml2Id>();

		private Collection<Saml2Assertion> _assertions = new Collection<Saml2Assertion>();

		private AbsoluteUriCollection _assertionUriReferences = new AbsoluteUriCollection();

		public Collection<Saml2Id> AssertionIdReferences => _assertionIdReferences;

		public Collection<Saml2Assertion> Assertions => _assertions;

		public Collection<Uri> AssertionUriReferences => _assertionUriReferences;
	}
}
