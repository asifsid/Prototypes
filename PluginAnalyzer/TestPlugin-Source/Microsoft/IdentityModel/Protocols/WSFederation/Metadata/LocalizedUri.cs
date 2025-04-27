using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class LocalizedUri : LocalizedEntry
	{
		private Uri _uri;

		public Uri Uri
		{
			get
			{
				return _uri;
			}
			set
			{
				_uri = value;
			}
		}

		public LocalizedUri()
			: this(null, null)
		{
		}

		public LocalizedUri(Uri uri, CultureInfo language)
			: base(language)
		{
			Uri = uri;
		}
	}
}
