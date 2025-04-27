using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Configuration;
using System.ServiceModel.Security;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel
{
	internal static class X509Util
	{
		internal static RSA EnsureAndGetPrivateRSAKey(X509Certificate2 certificate)
		{
			if (!certificate.HasPrivateKey)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID1001", certificate.Thumbprint)));
			}
			AsymmetricAlgorithm privateKey;
			try
			{
				privateKey = certificate.PrivateKey;
			}
			catch (CryptographicException innerException)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID1039", certificate.Thumbprint), innerException));
			}
			RSA rSA = privateKey as RSA;
			if (rSA == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID1002", certificate.Thumbprint)));
			}
			return rSA;
		}

		internal static X509Certificate2 ResolveCertificate(CertificateReferenceElement element)
		{
			return ResolveCertificate(element.StoreName, element.StoreLocation, element.X509FindType, element.FindValue);
		}

		internal static bool TryResolveCertificate(CertificateReferenceElement element, out X509Certificate2 certificate)
		{
			return TryResolveCertificate(element.StoreName, element.StoreLocation, element.X509FindType, element.FindValue, out certificate);
		}

		internal static X509Certificate2 ResolveCertificate(StoreName storeName, StoreLocation storeLocation, X509FindType findType, object findValue)
		{
			X509Certificate2 certificate = null;
			if (!TryResolveCertificate(storeName, storeLocation, findType, findValue, out certificate))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID1025", storeName, storeLocation, findType, findValue)));
			}
			return certificate;
		}

		internal static bool TryResolveCertificate(StoreName storeName, StoreLocation storeLocation, X509FindType findType, object findValue, out X509Certificate2 certificate)
		{
			X509Store x509Store = new X509Store(storeName, storeLocation);
			x509Store.Open(OpenFlags.ReadOnly);
			certificate = null;
			X509Certificate2Collection x509Certificate2Collection = null;
			X509Certificate2Collection x509Certificate2Collection2 = null;
			try
			{
				x509Certificate2Collection = x509Store.Certificates;
				x509Certificate2Collection2 = x509Certificate2Collection.Find(findType, findValue, validOnly: false);
				if (x509Certificate2Collection2.Count == 1)
				{
					certificate = new X509Certificate2(x509Certificate2Collection2[0]);
					return true;
				}
			}
			finally
			{
				CryptoUtil.ResetAllCertificates(x509Certificate2Collection2);
				CryptoUtil.ResetAllCertificates(x509Certificate2Collection);
				x509Store.Close();
			}
			return false;
		}

		internal static string GetCertificateId(X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("certificate");
			}
			string text = certificate.SubjectName.Name;
			if (string.IsNullOrEmpty(text))
			{
				text = certificate.Thumbprint;
			}
			return text;
		}

		internal static string GetCertificateIssuerName(X509Certificate2 certificate, Microsoft.IdentityModel.Tokens.IssuerNameRegistry issuerNameRegistry)
		{
			if (certificate == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("certificate");
			}
			if (issuerNameRegistry == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("issuerNameRegistry");
			}
			X509Chain x509Chain = new X509Chain();
			x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
			x509Chain.Build(certificate);
			X509ChainElementCollection chainElements = x509Chain.ChainElements;
			string result = null;
			if (chainElements.Count > 1)
			{
				using X509SecurityToken securityToken = new X509SecurityToken(chainElements[1].Certificate);
				result = issuerNameRegistry.GetIssuerName(securityToken);
			}
			else
			{
				using X509SecurityToken securityToken2 = new X509SecurityToken(certificate);
				result = issuerNameRegistry.GetIssuerName(securityToken2);
			}
			for (int i = 1; i < chainElements.Count; i++)
			{
				chainElements[i].Certificate.Reset();
			}
			return result;
		}

		internal static X509CertificateValidator CreateCertificateValidator(X509CertificateValidationMode certificateValidationMode, X509RevocationMode revocationMode, StoreLocation trustedStoreLocation)
		{
			return new X509CertificateValidatorEx(certificateValidationMode, revocationMode, trustedStoreLocation);
		}
	}
}
