using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class UseKey
	{
		private SecurityToken _token;

		private SecurityKeyIdentifier _ski;

		public SecurityToken Token => _token;

		public SecurityKeyIdentifier SecurityKeyIdentifier => _ski;

		public UseKey()
		{
		}

		public UseKey(SecurityKeyIdentifier ski)
			: this(ski, null)
		{
		}

		public UseKey(SecurityToken token)
			: this(null, token)
		{
		}

		public UseKey(SecurityKeyIdentifier ski, SecurityToken token)
		{
			_ski = ski;
			_token = token;
		}
	}
}
