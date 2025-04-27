using System.ComponentModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Web
{
	[ComVisible(true)]
	public class SecurityTokenReceivedEventArgs : CancelEventArgs
	{
		private SecurityToken _securityToken;

		private string _signInContext;

		public SecurityToken SecurityToken
		{
			get
			{
				return _securityToken;
			}
			set
			{
				_securityToken = value;
			}
		}

		public string SignInContext => _signInContext;

		public SecurityTokenReceivedEventArgs(SecurityToken securityToken)
			: this(securityToken, null)
		{
		}

		public SecurityTokenReceivedEventArgs(SecurityToken securityToken, string signInContext)
			: base(cancel: false)
		{
			if (securityToken == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityToken");
			}
			_securityToken = securityToken;
			_signInContext = signInContext;
		}
	}
}
