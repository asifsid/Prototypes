using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2Evidence
	{
		private Collection<Saml2Id> _assertionIdReferences = new Collection<Saml2Id>();

		private Collection<Saml2Assertion> _assertions = new Collection<Saml2Assertion>();

		private AbsoluteUriCollection _assertionUriReferences = new AbsoluteUriCollection();

		public Collection<Saml2Id> AssertionIdReferences => _assertionIdReferences;

		public Collection<Saml2Assertion> Assertions => _assertions;

		public Collection<Uri> AssertionUriReferences => _assertionUriReferences;

		public Saml2Evidence()
		{
		}

		public Saml2Evidence(Saml2Assertion assertion)
		{
			if (assertion == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("assertion");
			}
			_assertions.Add(assertion);
		}

		public Saml2Evidence(Saml2Id idReference)
		{
			if (idReference == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("idReference");
			}
			_assertionIdReferences.Add(idReference);
		}

		public Saml2Evidence(Uri uriReference)
		{
			if (null == uriReference)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("uriReference");
			}
			_assertionUriReferences.Add(uriReference);
		}
	}
}
