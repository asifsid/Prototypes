using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class LocalizedName : LocalizedEntry
	{
		private string _name;

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public LocalizedName()
			: this(null, null)
		{
		}

		public LocalizedName(string name, CultureInfo language)
			: base(language)
		{
			_name = name;
		}
	}
}
