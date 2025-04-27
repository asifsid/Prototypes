using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Microsoft.IdentityModel.Tokens
{
	[ComVisible(true)]
	public class X509NTAuthChainTrustValidator : X509CertificateValidator
	{
		private bool _useMachineContext;

		private X509ChainPolicy _chainPolicy;

		private uint _chainPolicyOID = 6u;

		public X509NTAuthChainTrustValidator()
			: this(useMachineContext: false, null)
		{
		}

		public X509NTAuthChainTrustValidator(bool useMachineContext, X509ChainPolicy chainPolicy)
		{
			_useMachineContext = useMachineContext;
			_chainPolicy = chainPolicy;
		}

		public override void Validate(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("certificate");
			}
			X509CertificateChain x509CertificateChain = new X509CertificateChain(_useMachineContext, _chainPolicyOID);
			if (_chainPolicy != null)
			{
				x509CertificateChain.ChainPolicy = _chainPolicy;
			}
			if (!x509CertificateChain.Build(certificate))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SecurityTokenValidationException(SR.GetString("ID4070", X509Util.GetCertificateId(certificate), GetChainStatusInformation(x509CertificateChain.ChainStatus))));
			}
		}

		private static string GetChainStatusInformation(X509ChainStatus[] chainStatus)
		{
			if (chainStatus != null)
			{
				StringBuilder stringBuilder = new StringBuilder(128);
				for (int i = 0; i < chainStatus.Length; i++)
				{
					stringBuilder.Append(chainStatus[i].StatusInformation);
					stringBuilder.Append(" ");
				}
				return stringBuilder.ToString();
			}
			return string.Empty;
		}
	}
}
