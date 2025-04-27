using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class SigningOutEventArgs : EventArgs
	{
		private static SigningOutEventArgs _ipInitiated = new SigningOutEventArgs(isIPInitiated: true);

		private static SigningOutEventArgs _rpInitiated = new SigningOutEventArgs(isIPInitiated: false);

		private bool _isIPInitiated;

		public static SigningOutEventArgs IPInitiated => _ipInitiated;

		public static SigningOutEventArgs RPInitiated => _rpInitiated;

		public bool IsIPInitiated => _isIPInitiated;

		public SigningOutEventArgs(bool isIPInitiated)
		{
			_isIPInitiated = isIPInitiated;
		}
	}
}
