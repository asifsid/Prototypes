using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Protocols.WSTrust;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class AsymmetricProofDescriptor : ProofDescriptor
	{
		private SecurityKeyIdentifier _keyIdentifier;

		public override SecurityKeyIdentifier KeyIdentifier => _keyIdentifier;

		public AsymmetricProofDescriptor()
		{
		}

		public AsymmetricProofDescriptor(RSA rsaAlgorithm)
		{
			if (rsaAlgorithm == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("rsaAlgorithm");
			}
			_keyIdentifier = new SecurityKeyIdentifier(new RsaKeyIdentifierClause(rsaAlgorithm));
		}

		public AsymmetricProofDescriptor(SecurityKeyIdentifier keyIdentifier)
		{
			if (keyIdentifier == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyIdentifier");
			}
			_keyIdentifier = keyIdentifier;
		}

		public override void ApplyTo(RequestSecurityTokenResponse response)
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
		}
	}
}
