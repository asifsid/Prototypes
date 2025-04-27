using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public abstract class LocalizedEntry
	{
		private CultureInfo _language;

		public CultureInfo Language
		{
			get
			{
				return _language;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_language = value;
			}
		}

		protected LocalizedEntry()
			: this(null)
		{
		}

		protected LocalizedEntry(CultureInfo language)
		{
			_language = language;
		}
	}
}
