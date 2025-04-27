using System;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class X509CertificateCredential : IUserCredential
	{
		private SecurityKeyIdentifierClause _x509IdentifierClause;

		private X509Principal _x509Principal;

		private X509SubjectAndIssuer _x509SubjectAndIssuer;

		private string _x509SubjectName;

		public X509Principal X509Principal => _x509Principal;

		public SecurityKeyIdentifierClause X509SecurityTokenIdentifierClause => _x509IdentifierClause;

		public X509SubjectAndIssuer X509SubjectAndIssuer => _x509SubjectAndIssuer;

		public string X509SubjectName => _x509SubjectName;

		public UserCredentialType CredentialType => UserCredentialType.X509V3Credential;

		public X509CertificateCredential(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("certificate");
			}
			_x509IdentifierClause = new X509ThumbprintKeyIdentifierClause(certificate);
		}

		public X509CertificateCredential(SecurityKeyIdentifierClause x509IdentifierClause)
		{
			if (x509IdentifierClause == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("x509IdentifierClause");
			}
			if (!(x509IdentifierClause is X509ThumbprintKeyIdentifierClause) && !(x509IdentifierClause is X509IssuerSerialKeyIdentifierClause) && !(x509IdentifierClause is X509RawDataKeyIdentifierClause) && !(x509IdentifierClause is X509SubjectKeyIdentifierClause))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID2041"), "x509IdentifierClause"));
			}
			_x509IdentifierClause = x509IdentifierClause;
		}

		public X509CertificateCredential(X509Principal x509Principal)
		{
			if (x509Principal == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("x509Principal");
			}
			_x509Principal = x509Principal;
		}

		public X509CertificateCredential(X509SubjectAndIssuer x509SubjectAndIssuer)
		{
			if (x509SubjectAndIssuer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("x509SubjectAndIssuer");
			}
			_x509SubjectAndIssuer = x509SubjectAndIssuer;
		}

		public X509CertificateCredential(string x509SubjectName)
		{
			if (string.IsNullOrEmpty(x509SubjectName))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("x509SubjectName");
			}
			_x509SubjectName = x509SubjectName;
		}
	}
}
