using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class SymmetricProofDescriptor : ProofDescriptor
	{
		private byte[] _key;

		private int _keySizeInBits;

		private byte[] _sourceEntropy;

		private byte[] _targetEntropy;

		private SecurityKeyIdentifier _ski;

		private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _requestorWrappingCredentials;

		private Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials _targetWrappingCredentials;

		protected Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials RequestorEncryptingCredentials => _requestorWrappingCredentials;

		protected Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials TargetEncryptingCredentials => _targetWrappingCredentials;

		public override SecurityKeyIdentifier KeyIdentifier
		{
			get
			{
				if (_ski == null)
				{
					_ski = KeyGenerator.GetSecurityKeyIdentifier(_key, _targetWrappingCredentials);
				}
				return _ski;
			}
		}

		public SymmetricProofDescriptor(byte[] key, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials)
		{
			if (key == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("key");
			}
			_keySizeInBits = key.Length;
			_key = key;
			_targetWrappingCredentials = targetWrappingCredentials;
		}

		public SymmetricProofDescriptor(Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials)
			: this(256, targetWrappingCredentials)
		{
		}

		public SymmetricProofDescriptor(int keySizeInBits, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials)
			: this(keySizeInBits, targetWrappingCredentials, null)
		{
		}

		public SymmetricProofDescriptor(int keySizeInBits, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials requestorWrappingCredentials)
			: this(keySizeInBits, targetWrappingCredentials, requestorWrappingCredentials, (string)null)
		{
		}

		public SymmetricProofDescriptor(int keySizeInBits, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials requestorWrappingCredentials, string encryptWith)
		{
			_keySizeInBits = keySizeInBits;
			switch (encryptWith)
			{
			case "http://www.w3.org/2001/04/xmlenc#des-cbc":
			case "http://www.w3.org/2001/04/xmlenc#tripledes-cbc":
			case "http://www.w3.org/2001/04/xmlenc#kw-tripledes":
				_key = KeyGenerator.GenerateDESKey(_keySizeInBits);
				break;
			default:
				_key = KeyGenerator.GenerateSymmetricKey(_keySizeInBits);
				break;
			}
			_requestorWrappingCredentials = requestorWrappingCredentials;
			_targetWrappingCredentials = targetWrappingCredentials;
		}

		public SymmetricProofDescriptor(int keySizeInBits, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials requestorWrappingCredentials, byte[] sourceEntropy)
			: this(keySizeInBits, targetWrappingCredentials, requestorWrappingCredentials, sourceEntropy, null)
		{
		}

		public SymmetricProofDescriptor(int keySizeInBits, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials targetWrappingCredentials, Microsoft.IdentityModel.SecurityTokenService.EncryptingCredentials requestorWrappingCredentials, byte[] sourceEntropy, string encryptWith)
		{
			if (sourceEntropy == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("sourceEntropy");
			}
			if (sourceEntropy.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("sourceEntropy", SR.GetString("ID2058"));
			}
			_keySizeInBits = keySizeInBits;
			_sourceEntropy = sourceEntropy;
			switch (encryptWith)
			{
			case "http://www.w3.org/2001/04/xmlenc#des-cbc":
			case "http://www.w3.org/2001/04/xmlenc#tripledes-cbc":
			case "http://www.w3.org/2001/04/xmlenc#kw-tripledes":
				_key = KeyGenerator.GenerateDESKey(_keySizeInBits, _sourceEntropy, out _targetEntropy);
				break;
			default:
				_key = KeyGenerator.GenerateSymmetricKey(_keySizeInBits, _sourceEntropy, out _targetEntropy);
				break;
			}
			_requestorWrappingCredentials = requestorWrappingCredentials;
			_targetWrappingCredentials = targetWrappingCredentials;
		}

		public byte[] GetKeyBytes()
		{
			return _key;
		}

		protected byte[] GetSourceEntropy()
		{
			return _sourceEntropy;
		}

		protected byte[] GetTargetEntropy()
		{
			return _targetEntropy;
		}

		public override void ApplyTo(RequestSecurityTokenResponse response)
		{
			if (response == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("response");
			}
			if (_targetEntropy != null)
			{
				response.RequestedProofToken = new RequestedProofToken("http://schemas.microsoft.com/idfx/computedkeyalgorithm/psha1");
				response.KeySizeInBits = _keySizeInBits;
				response.Entropy = new Entropy(_targetEntropy, _requestorWrappingCredentials);
			}
			else
			{
				response.RequestedProofToken = new RequestedProofToken(_key, _requestorWrappingCredentials);
			}
		}
	}
}
