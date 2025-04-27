using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class PrivacyNotice
	{
		private long _version;

		private string _location;

		public string Location => _location;

		public long Version => _version;

		public PrivacyNotice(string privacyNoticeLocation)
			: this(privacyNoticeLocation, 1L)
		{
		}

		public PrivacyNotice(string privacyNoticeLocation, long version)
		{
			if (string.IsNullOrEmpty(privacyNoticeLocation))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("privacyNoticeLocation");
			}
			if (version < 1 || version > uint.MaxValue)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("version", SR.GetString("ID2028", 4294967295L));
			}
			_location = privacyNoticeLocation;
			_version = version;
		}
	}
}
