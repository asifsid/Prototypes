using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class SessionSecurityTokenReceivedEventArgs : CancelEventArgs
	{
		private SessionSecurityToken _sessionToken;

		private bool _reissueCookie;

		public SessionSecurityToken SessionToken
		{
			get
			{
				return _sessionToken;
			}
			set
			{
				_sessionToken = value;
			}
		}

		public bool ReissueCookie
		{
			get
			{
				return _reissueCookie;
			}
			set
			{
				_reissueCookie = value;
			}
		}

		public SessionSecurityTokenReceivedEventArgs(SessionSecurityToken sessionToken)
		{
			if (sessionToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sessionToken");
			}
			_sessionToken = sessionToken;
		}
	}
}
