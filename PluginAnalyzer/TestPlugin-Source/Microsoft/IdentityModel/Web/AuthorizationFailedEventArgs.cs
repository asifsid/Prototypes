using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class AuthorizationFailedEventArgs : EventArgs
	{
		private bool _redirectToIdentityProvider;

		public bool RedirectToIdentityProvider
		{
			get
			{
				return _redirectToIdentityProvider;
			}
			set
			{
				_redirectToIdentityProvider = value;
			}
		}
	}
}
