using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.IdentityModel.Protocols.WSIdentity
{
	[ComVisible(true)]
	public class TokenServiceEndpoint
	{
		private EndpointAddress _endpointAddress;

		private UserCredentialType _userCredentialType;

		private string _displayCredentialHint;

		private SecurityKeyIdentifier _encryptingCredentials;

		private MetadataSet _mexSet;

		private MetadataReference _metadataReference;

		private X509Certificate2Collection _certificates;

		internal X509Certificate2Collection Certificates => _certificates;

		public string DisplayCredentialHint
		{
			get
			{
				return _displayCredentialHint;
			}
			set
			{
				_displayCredentialHint = value;
			}
		}

		public EndpointAddress Address => _endpointAddress;

		public MetadataSet MetadataSet => _mexSet;

		public MetadataReference MetadataReference => _metadataReference;

		public SecurityKeyIdentifier EncryptingCredentials
		{
			get
			{
				return _encryptingCredentials;
			}
			set
			{
				_encryptingCredentials = value;
			}
		}

		public UserCredentialType UserCredentialType => _userCredentialType;

		public TokenServiceEndpoint(string address)
			: this(new EndpointAddress(address))
		{
		}

		public TokenServiceEndpoint(EndpointAddress endpointAddress)
			: this(endpointAddress, TokenService.DefaultUserCredentialType)
		{
		}

		public TokenServiceEndpoint(EndpointAddress endpointAddress, UserCredentialType userCredentialType)
			: this(endpointAddress, userCredentialType, null)
		{
		}

		public TokenServiceEndpoint(EndpointAddress endpointAddress, UserCredentialType userCredentialType, string displayCredentialHint)
		{
			if (endpointAddress == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("endpointAddress");
			}
			_endpointAddress = endpointAddress;
			_userCredentialType = userCredentialType;
			_displayCredentialHint = displayCredentialHint;
			X509CertificateEndpointIdentity x509CertificateEndpointIdentity = endpointAddress.Identity as X509CertificateEndpointIdentity;
			if (x509CertificateEndpointIdentity != null)
			{
				_certificates = x509CertificateEndpointIdentity.Certificates;
			}
			else
			{
				_certificates = new X509Certificate2Collection();
			}
			XmlDictionaryReader readerAtMetadata = _endpointAddress.GetReaderAtMetadata();
			if (readerAtMetadata == null)
			{
				return;
			}
			MetadataSet metadataSet = new MetadataSet();
			((IXmlSerializable)metadataSet).ReadXml((XmlReader)readerAtMetadata);
			if (metadataSet.MetadataSections == null || metadataSet.MetadataSections.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", SR.GetString("ID2049"));
			}
			for (int i = 0; i < metadataSet.MetadataSections.Count; i++)
			{
				MetadataSection metadataSection = metadataSet.MetadataSections[i];
				if (metadataSection.Metadata == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("endpoint", SR.GetString("ID2049"));
				}
				MetadataReference metadataReference = metadataSection.Metadata as MetadataReference;
				if (metadataReference != null)
				{
					_metadataReference = metadataReference;
					break;
				}
			}
			_mexSet = metadataSet;
		}

		public TokenServiceEndpoint(string address, string mexAddress)
			: this(address, mexAddress, TokenService.DefaultUserCredentialType)
		{
		}

		public TokenServiceEndpoint(string address, string mexAddress, X509Certificate2 certificate)
			: this(address, mexAddress, TokenService.DefaultUserCredentialType, certificate)
		{
		}

		public TokenServiceEndpoint(string address, string mexAddress, UserCredentialType userCredentialType)
			: this(address, mexAddress, userCredentialType, null)
		{
		}

		public TokenServiceEndpoint(string address, string mexAddress, UserCredentialType userCredentialType, X509Certificate2 certificate)
			: this(address, mexAddress, userCredentialType, certificate, null)
		{
		}

		public TokenServiceEndpoint(string address, string mexAddress, UserCredentialType userCredentialType, X509Certificate2 certificate, string displayCredentialHint)
		{
			if (string.IsNullOrEmpty(address))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "address"));
			}
			_userCredentialType = userCredentialType;
			_displayCredentialHint = displayCredentialHint;
			_certificates = new X509Certificate2Collection();
			if (certificate != null)
			{
				_certificates.Add(certificate);
			}
			if (!string.IsNullOrEmpty(mexAddress))
			{
				MetadataReference metadataReference = new MetadataReference
				{
					Address = new EndpointAddress(mexAddress),
					AddressVersion = AddressingVersion.WSAddressing10
				};
				MetadataSection item = new MetadataSection
				{
					Dialect = "http://schemas.xmlsoap.org/ws/2004/09/mex",
					Metadata = metadataReference
				};
				_mexSet = new MetadataSet();
				_mexSet.MetadataSections.Add(item);
				MemoryStream memoryStream = new MemoryStream();
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
				{
					_mexSet.WriteTo(xmlTextWriter);
					xmlTextWriter.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					XmlDictionaryReader metadataReader = XmlDictionaryReader.CreateTextReader(memoryStream, XmlDictionaryReaderQuotas.Max);
					_endpointAddress = new EndpointAddress(new Uri(address), (certificate != null) ? new X509CertificateEndpointIdentity(certificate) : null, null, metadataReader, null);
				}
				_metadataReference = metadataReference;
			}
			else
			{
				_endpointAddress = new EndpointAddress(new Uri(address), (certificate != null) ? new X509CertificateEndpointIdentity(certificate) : null);
			}
		}
	}
}
