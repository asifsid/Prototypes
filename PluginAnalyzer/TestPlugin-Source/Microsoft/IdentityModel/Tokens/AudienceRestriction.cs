using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class AudienceRestriction
	{
		private AudienceUriMode _audienceMode = AudienceUriMode.Always;

		private Collection<Uri> _audience = new Collection<Uri>();

		public AudienceUriMode AudienceMode
		{
			get
			{
				return _audienceMode;
			}
			set
			{
				_audienceMode = value;
			}
		}

		public Collection<Uri> AllowedAudienceUris => _audience;

		public AudienceRestriction()
		{
		}

		public AudienceRestriction(AudienceUriMode audienceMode)
		{
			_audienceMode = audienceMode;
		}
	}
}
