using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace Microsoft.IdentityModel
{
	internal class X509CertificateValidatorEx : X509CertificateValidator
	{
		private X509CertificateValidationMode _certificateValidationMode;

		private X509ChainPolicy _chainPolicy;

		private X509CertificateValidator _validator;

		public X509CertificateValidatorEx(X509CertificateValidationMode certificateValidationMode, X509RevocationMode revocationMode, StoreLocation trustedStoreLocation)
		{
			_certificateValidationMode = certificateValidationMode;
			switch (_certificateValidationMode)
			{
			case X509CertificateValidationMode.None:
				_validator = X509CertificateValidator.None;
				break;
			case X509CertificateValidationMode.PeerTrust:
				_validator = X509CertificateValidator.PeerTrust;
				break;
			case X509CertificateValidationMode.ChainTrust:
			{
				bool useMachineContext2 = trustedStoreLocation == StoreLocation.LocalMachine;
				_chainPolicy = new X509ChainPolicy();
				_chainPolicy.RevocationMode = revocationMode;
				_validator = X509CertificateValidator.CreateChainTrustValidator(useMachineContext2, _chainPolicy);
				break;
			}
			case X509CertificateValidationMode.PeerOrChainTrust:
			{
				bool useMachineContext = trustedStoreLocation == StoreLocation.LocalMachine;
				_chainPolicy = new X509ChainPolicy();
				_chainPolicy.RevocationMode = revocationMode;
				_validator = X509CertificateValidator.CreatePeerOrChainTrustValidator(useMachineContext, _chainPolicy);
				break;
			}
			default:
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID4256")));
			}
		}

		public override void Validate(X509Certificate2 certificate)
		{
			if (_certificateValidationMode == X509CertificateValidationMode.ChainTrust || _certificateValidationMode == X509CertificateValidationMode.PeerOrChainTrust)
			{
				_chainPolicy.VerificationTime = DateTime.Now;
			}
			_validator.Validate(certificate);
		}
	}
}
