using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class FederatedClientCredentialsParameters
	{
		private SecurityToken _actAs;

		private SecurityToken _onBehalfOf;

		private SecurityToken _issuedSecurityToken;

		public SecurityToken ActAs
		{
			get
			{
				return _actAs;
			}
			set
			{
				_actAs = value;
			}
		}

		public SecurityToken OnBehalfOf
		{
			get
			{
				return _onBehalfOf;
			}
			set
			{
				_onBehalfOf = value;
			}
		}

		public SecurityToken IssuedSecurityToken
		{
			get
			{
				return _issuedSecurityToken;
			}
			set
			{
				_issuedSecurityToken = value;
			}
		}
	}
}
