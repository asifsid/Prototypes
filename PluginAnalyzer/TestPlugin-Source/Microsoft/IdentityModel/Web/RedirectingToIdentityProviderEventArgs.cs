using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Protocols.WSFederation;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class RedirectingToIdentityProviderEventArgs : CancelEventArgs
	{
		private SignInRequestMessage _signInRequestMessage;

		public SignInRequestMessage SignInRequestMessage
		{
			get
			{
				return _signInRequestMessage;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_signInRequestMessage = value;
			}
		}

		public RedirectingToIdentityProviderEventArgs(SignInRequestMessage signInRequestMessage)
		{
			if (signInRequestMessage == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signInRequestMessage");
			}
			_signInRequestMessage = signInRequestMessage;
		}
	}
}
