using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2ProxyRestriction
	{
		private Collection<Uri> _audiences = new AbsoluteUriCollection();

		private int? _count;

		public Collection<Uri> Audiences => _audiences;

		public int? Count
		{
			get
			{
				return _count;
			}
			set
			{
				if (value.HasValue && value.Value < 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("value", SR.GetString("ID0002"));
				}
				_count = value;
			}
		}
	}
}
