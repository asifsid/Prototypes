using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public class Organization
	{
		private LocalizedEntryCollection<LocalizedName> _displayNames = new LocalizedEntryCollection<LocalizedName>();

		private LocalizedEntryCollection<LocalizedName> _names = new LocalizedEntryCollection<LocalizedName>();

		private LocalizedEntryCollection<LocalizedUri> _urls = new LocalizedEntryCollection<LocalizedUri>();

		public LocalizedEntryCollection<LocalizedName> DisplayNames => _displayNames;

		public LocalizedEntryCollection<LocalizedName> Names => _names;

		public LocalizedEntryCollection<LocalizedUri> Urls => _urls;

		public Organization()
			: this(new LocalizedEntryCollection<LocalizedName>(), new LocalizedEntryCollection<LocalizedName>(), new LocalizedEntryCollection<LocalizedUri>())
		{
		}

		public Organization(LocalizedEntryCollection<LocalizedName> names, LocalizedEntryCollection<LocalizedName> displayNames, LocalizedEntryCollection<LocalizedUri> urls)
		{
			if (names == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("names");
			}
			if (displayNames == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("displayNames");
			}
			if (urls == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("urls");
			}
			_names = names;
			_displayNames = displayNames;
			_urls = urls;
		}
	}
}
