using System.ServiceModel.Security;

namespace Microsoft.IdentityModel.Tokens
{
	internal class NoOpSecurityStateEncoder : SecurityStateEncoder
	{
		protected override byte[] EncodeSecurityState(byte[] data)
		{
			return data;
		}

		protected override byte[] DecodeSecurityState(byte[] data)
		{
			return data;
		}
	}
}
