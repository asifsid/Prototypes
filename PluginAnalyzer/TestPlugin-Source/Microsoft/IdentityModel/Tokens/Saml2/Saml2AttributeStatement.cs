using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2AttributeStatement : Saml2Statement
	{
		private Collection<Saml2Attribute> _attributes = new Collection<Saml2Attribute>();

		public Collection<Saml2Attribute> Attributes => _attributes;

		public Saml2AttributeStatement()
		{
		}

		public Saml2AttributeStatement(Saml2Attribute attribute)
			: this(new Saml2Attribute[1] { attribute })
		{
		}

		public Saml2AttributeStatement(IEnumerable<Saml2Attribute> attributes)
		{
			if (attributes == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("attributes");
			}
			foreach (Saml2Attribute attribute in attributes)
			{
				if (attribute == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("attributes");
				}
				_attributes.Add(attribute);
			}
		}
	}
}
