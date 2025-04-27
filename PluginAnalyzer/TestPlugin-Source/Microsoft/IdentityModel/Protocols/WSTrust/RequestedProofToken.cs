using System.Runtime.InteropServices;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class RequestedProofToken
	{
		private string _computedKeyAlgorithm;

		private ProtectedKey _keys;

		public string ComputedKeyAlgorithm => _computedKeyAlgorithm;

		public ProtectedKey ProtectedKey => _keys;

		public RequestedProofToken(string computedKeyAlgorithm)
		{
			if (string.IsNullOrEmpty(computedKeyAlgorithm))
			{
				DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("computedKeyAlgorithm");
			}
			_computedKeyAlgorithm = computedKeyAlgorithm;
		}

		public RequestedProofToken(byte[] secret)
		{
			_keys = new ProtectedKey(secret);
		}

		public RequestedProofToken(byte[] secret, EncryptingCredentials wrappingCredentials)
		{
			_keys = new ProtectedKey(secret, wrappingCredentials);
		}

		public RequestedProofToken(ProtectedKey protectedKey)
		{
			if (protectedKey == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("protectedKey");
			}
			_keys = protectedKey;
		}
	}
}
