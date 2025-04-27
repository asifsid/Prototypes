using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Protocols.WSFederation.Metadata
{
	[ComVisible(true)]
	public abstract class MetadataBase
	{
		private SigningCredentials _signingCredentials;

		public SigningCredentials SigningCredentials
		{
			get
			{
				return _signingCredentials;
			}
			set
			{
				_signingCredentials = value;
			}
		}
	}
}
