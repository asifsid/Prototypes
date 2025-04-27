using System;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class SessionSecurityTokenCreatedEventArgs : EventArgs
	{
		private SessionSecurityToken _sessionToken;

		private bool _writeSessionCookie;

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

		public bool WriteSessionCookie
		{
			get
			{
				return _writeSessionCookie;
			}
			set
			{
				_writeSessionCookie = value;
			}
		}

		public SessionSecurityTokenCreatedEventArgs(SessionSecurityToken sessionToken)
		{
			if (sessionToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sessionToken");
			}
			_sessionToken = sessionToken;
		}
	}
}
