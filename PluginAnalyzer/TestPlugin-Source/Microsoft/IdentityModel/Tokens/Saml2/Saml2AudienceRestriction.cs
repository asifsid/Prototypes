using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2AudienceRestriction
	{
		private Collection<Uri> _audiences = new Collection<Uri>();

		public Collection<Uri> Audiences => _audiences;

		public Saml2AudienceRestriction()
		{
		}

		public Saml2AudienceRestriction(Uri audience)
			: this(new Uri[1] { audience })
		{
		}

		public Saml2AudienceRestriction(IEnumerable<Uri> audiences)
		{
			if (audiences == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("audiences");
			}
			foreach (Uri audience in audiences)
			{
				if (null == audience)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("audiences");
				}
				_audiences.Add(audience);
			}
		}
	}
}
