using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Protocols.WSTrust;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public abstract class ProofDescriptor
	{
		public abstract SecurityKeyIdentifier KeyIdentifier { get; }

		public abstract void ApplyTo(RequestSecurityTokenResponse response);
	}
}
