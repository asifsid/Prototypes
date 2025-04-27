using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2Subject
	{
		private Saml2NameIdentifier _nameId;

		private Collection<Saml2SubjectConfirmation> _subjectConfirmations = new Collection<Saml2SubjectConfirmation>();

		public Saml2NameIdentifier NameId
		{
			get
			{
				return _nameId;
			}
			set
			{
				_nameId = value;
			}
		}

		public Collection<Saml2SubjectConfirmation> SubjectConfirmations => _subjectConfirmations;

		public Saml2Subject()
		{
		}

		public Saml2Subject(Saml2NameIdentifier nameId)
		{
			_nameId = nameId;
		}

		public Saml2Subject(Saml2SubjectConfirmation subjectConfirmation)
		{
			if (subjectConfirmation == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("subjectConfirmation");
			}
			_subjectConfirmations.Add(subjectConfirmation);
		}
	}
}
