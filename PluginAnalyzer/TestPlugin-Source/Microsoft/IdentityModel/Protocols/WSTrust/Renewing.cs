using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class Renewing
	{
		private bool _allowRenewal = true;

		private bool _okForRenewalAfterExpiration;

		public bool AllowRenewal
		{
			get
			{
				return _allowRenewal;
			}
			set
			{
				_allowRenewal = value;
			}
		}

		public bool OkForRenewalAfterExpiration
		{
			get
			{
				return _okForRenewalAfterExpiration;
			}
			set
			{
				_okForRenewalAfterExpiration = value;
			}
		}

		public Renewing()
		{
		}

		public Renewing(bool allowRenewal, bool okForRenewalAfterExpiration)
		{
			_allowRenewal = allowRenewal;
			_okForRenewalAfterExpiration = okForRenewalAfterExpiration;
		}
	}
}
