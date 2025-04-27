using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.WSTrust.Bindings
{
	[ComVisible(true)]
	public class IssuedTokenWSTrustBinding : WSTrustBindingBase
	{
		private SecurityKeyType _keyType;

		private SecurityAlgorithmSuite _algorithmSuite;

		private string _tokenType;

		private Binding _issuerBinding;

		private EndpointAddress _issuerAddress;

		private Collection<ClaimTypeRequirement> _claimTypeRequirements = new Collection<ClaimTypeRequirement>();

		private EndpointAddress _issuerMetadataAddress;

		public Binding IssuerBinding
		{
			get
			{
				return _issuerBinding;
			}
			set
			{
				_issuerBinding = value;
			}
		}

		public EndpointAddress IssuerAddress
		{
			get
			{
				return _issuerAddress;
			}
			set
			{
				_issuerAddress = value;
			}
		}

		public EndpointAddress IssuerMetadataAddress
		{
			get
			{
				return _issuerMetadataAddress;
			}
			set
			{
				_issuerMetadataAddress = value;
			}
		}

		public SecurityKeyType KeyType
		{
			get
			{
				return _keyType;
			}
			set
			{
				_keyType = value;
			}
		}

		public SecurityAlgorithmSuite AlgorithmSuite
		{
			get
			{
				return _algorithmSuite;
			}
			set
			{
				_algorithmSuite = value;
			}
		}

		public string TokenType
		{
			get
			{
				return _tokenType;
			}
			set
			{
				_tokenType = value;
			}
		}

		public Collection<ClaimTypeRequirement> ClaimTypeRequirement => _claimTypeRequirements;

		public IssuedTokenWSTrustBinding()
			: this(null, null)
		{
		}

		public IssuedTokenWSTrustBinding(Binding issuerBinding, EndpointAddress issuerAddress)
			: this(issuerBinding, issuerAddress, SecurityMode.Message, TrustVersion.WSTrust13, null)
		{
		}

		public IssuedTokenWSTrustBinding(Binding issuerBinding, EndpointAddress issuerAddress, EndpointAddress issuerMetadataAddress)
			: this(issuerBinding, issuerAddress, SecurityMode.Message, TrustVersion.WSTrust13, issuerMetadataAddress)
		{
		}

		public IssuedTokenWSTrustBinding(Binding issuerBinding, EndpointAddress issuerAddress, SecurityMode mode, TrustVersion trustVersion, EndpointAddress issuerMetadataAddress)
			: this(issuerBinding, issuerAddress, mode, trustVersion, SecurityKeyType.SymmetricKey, SecurityAlgorithmSuite.Basic256, null, null, issuerMetadataAddress)
		{
		}

		public IssuedTokenWSTrustBinding(Binding issuerBinding, EndpointAddress issuerAddress, string tokenType, IEnumerable<ClaimTypeRequirement> claimTypeRequirements)
			: this(issuerBinding, issuerAddress, SecurityKeyType.SymmetricKey, SecurityAlgorithmSuite.Basic256, tokenType, claimTypeRequirements)
		{
		}

		public IssuedTokenWSTrustBinding(Binding issuerBinding, EndpointAddress issuerAddress, SecurityKeyType keyType, SecurityAlgorithmSuite algorithmSuite, string tokenType, IEnumerable<ClaimTypeRequirement> claimTypeRequirements)
			: this(issuerBinding, issuerAddress, SecurityMode.Message, TrustVersion.WSTrust13, keyType, algorithmSuite, tokenType, claimTypeRequirements, null)
		{
		}

		public IssuedTokenWSTrustBinding(Binding issuerBinding, EndpointAddress issuerAddress, SecurityMode mode, TrustVersion version, SecurityKeyType keyType, SecurityAlgorithmSuite algorithmSuite, string tokenType, IEnumerable<ClaimTypeRequirement> claimTypeRequirements, EndpointAddress issuerMetadataAddress)
			: base(mode, version)
		{
			if (SecurityMode.Message != mode && SecurityMode.TransportWithMessageCredential != mode)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3226", mode));
			}
			if (_keyType == SecurityKeyType.BearerKey && version == TrustVersion.WSTrustFeb2005)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3267"));
			}
			_keyType = keyType;
			_algorithmSuite = algorithmSuite;
			_tokenType = tokenType;
			_issuerBinding = issuerBinding;
			_issuerAddress = issuerAddress;
			_issuerMetadataAddress = issuerMetadataAddress;
			if (claimTypeRequirements == null)
			{
				return;
			}
			foreach (ClaimTypeRequirement claimTypeRequirement in claimTypeRequirements)
			{
				_claimTypeRequirements.Add(claimTypeRequirement);
			}
		}

		protected override SecurityBindingElement CreateSecurityBindingElement()
		{
			IssuedSecurityTokenParameters issuedParameters = new IssuedSecurityTokenParameters(_tokenType, _issuerAddress, _issuerBinding);
			issuedParameters.KeyType = _keyType;
			issuedParameters.IssuerMetadataAddress = _issuerMetadataAddress;
			if (_keyType == SecurityKeyType.SymmetricKey)
			{
				issuedParameters.KeySize = _algorithmSuite.DefaultSymmetricKeyLength;
			}
			else
			{
				issuedParameters.KeySize = 0;
			}
			if (_claimTypeRequirements != null)
			{
				foreach (ClaimTypeRequirement claimTypeRequirement in _claimTypeRequirements)
				{
					issuedParameters.ClaimTypeRequirements.Add(claimTypeRequirement);
				}
			}
			AddAlgorithmParameters(_algorithmSuite, base.TrustVersion, _keyType, ref issuedParameters);
			SecurityBindingElement securityBindingElement;
			if (SecurityMode.Message == base.SecurityMode)
			{
				securityBindingElement = SecurityBindingElement.CreateIssuedTokenForCertificateBindingElement(issuedParameters);
			}
			else
			{
				if (SecurityMode.TransportWithMessageCredential != base.SecurityMode)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3226", base.SecurityMode));
				}
				securityBindingElement = SecurityBindingElement.CreateIssuedTokenOverTransportBindingElement(issuedParameters);
			}
			securityBindingElement.DefaultAlgorithmSuite = _algorithmSuite;
			securityBindingElement.IncludeTimestamp = true;
			return securityBindingElement;
		}

		private void AddAlgorithmParameters(SecurityAlgorithmSuite algorithmSuite, TrustVersion trustVersion, SecurityKeyType keyType, ref IssuedSecurityTokenParameters issuedParameters)
		{
			issuedParameters.AdditionalRequestParameters.Insert(0, CreateEncryptionAlgorithmElement(algorithmSuite.DefaultEncryptionAlgorithm));
			issuedParameters.AdditionalRequestParameters.Insert(0, CreateCanonicalizationAlgorithmElement(algorithmSuite.DefaultCanonicalizationAlgorithm));
			string signatureAlgorithm;
			string encryptionAlgorithm;
			switch (keyType)
			{
			case SecurityKeyType.BearerKey:
				return;
			case SecurityKeyType.SymmetricKey:
				signatureAlgorithm = algorithmSuite.DefaultSymmetricSignatureAlgorithm;
				encryptionAlgorithm = algorithmSuite.DefaultEncryptionAlgorithm;
				break;
			case SecurityKeyType.AsymmetricKey:
				signatureAlgorithm = algorithmSuite.DefaultAsymmetricSignatureAlgorithm;
				encryptionAlgorithm = algorithmSuite.DefaultAsymmetricKeyWrapAlgorithm;
				break;
			default:
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("keyType");
			}
			issuedParameters.AdditionalRequestParameters.Insert(0, CreateSignWithElement(signatureAlgorithm));
			issuedParameters.AdditionalRequestParameters.Insert(0, CreateEncryptWithElement(encryptionAlgorithm));
			if (trustVersion != TrustVersion.WSTrustFeb2005)
			{
				issuedParameters.AdditionalRequestParameters.Insert(0, CreateKeyWrapAlgorithmElement(algorithmSuite.DefaultAsymmetricKeyWrapAlgorithm));
			}
		}

		protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3227")));
		}

		private XmlElement CreateSignWithElement(string signatureAlgorithm)
		{
			if (signatureAlgorithm == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signatureAlgorithm");
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = null;
			if (base.TrustVersion == TrustVersion.WSTrust13)
			{
				xmlElement = xmlDocument.CreateElement("trust", "SignatureAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
			}
			else if (base.TrustVersion == TrustVersion.WSTrustFeb2005)
			{
				xmlElement = xmlDocument.CreateElement("t", "SignatureAlgorithm", "http://schemas.xmlsoap.org/ws/2005/02/trust");
			}
			xmlElement?.AppendChild(xmlDocument.CreateTextNode(signatureAlgorithm));
			return xmlElement;
		}

		private XmlElement CreateEncryptionAlgorithmElement(string encryptionAlgorithm)
		{
			if (encryptionAlgorithm == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("encryptionAlgorithm");
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = null;
			if (base.TrustVersion == TrustVersion.WSTrust13)
			{
				xmlElement = xmlDocument.CreateElement("trust", "EncryptionAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
			}
			else if (base.TrustVersion == TrustVersion.WSTrustFeb2005)
			{
				xmlElement = xmlDocument.CreateElement("t", "EncryptionAlgorithm", "http://schemas.xmlsoap.org/ws/2005/02/trust");
			}
			xmlElement?.AppendChild(xmlDocument.CreateTextNode(encryptionAlgorithm));
			return xmlElement;
		}

		private XmlElement CreateCanonicalizationAlgorithmElement(string canonicalizationAlgorithm)
		{
			if (canonicalizationAlgorithm == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("canonicalizationAlgorithm");
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = null;
			if (base.TrustVersion == TrustVersion.WSTrust13)
			{
				xmlElement = xmlDocument.CreateElement("trust", "CanonicalizationAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
			}
			else if (base.TrustVersion == TrustVersion.WSTrustFeb2005)
			{
				xmlElement = xmlDocument.CreateElement("t", "CanonicalizationAlgorithm", "http://schemas.xmlsoap.org/ws/2005/02/trust");
			}
			xmlElement?.AppendChild(xmlDocument.CreateTextNode(canonicalizationAlgorithm));
			return xmlElement;
		}

		private XmlElement CreateEncryptWithElement(string encryptionAlgorithm)
		{
			if (encryptionAlgorithm == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("encryptionAlgorithm");
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = null;
			if (base.TrustVersion == TrustVersion.WSTrust13)
			{
				xmlElement = xmlDocument.CreateElement("trust", "EncryptWith", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
			}
			else if (base.TrustVersion == TrustVersion.WSTrustFeb2005)
			{
				xmlElement = xmlDocument.CreateElement("t", "EncryptWith", "http://schemas.xmlsoap.org/ws/2005/02/trust");
			}
			xmlElement?.AppendChild(xmlDocument.CreateTextNode(encryptionAlgorithm));
			return xmlElement;
		}

		private static XmlElement CreateKeyWrapAlgorithmElement(string keyWrapAlgorithm)
		{
			if (keyWrapAlgorithm == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("keyWrapAlgorithm");
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("trust", "KeyWrapAlgorithm", "http://docs.oasis-open.org/ws-sx/ws-trust/200512");
			xmlElement.AppendChild(xmlDocument.CreateTextNode(keyWrapAlgorithm));
			return xmlElement;
		}
	}
}
