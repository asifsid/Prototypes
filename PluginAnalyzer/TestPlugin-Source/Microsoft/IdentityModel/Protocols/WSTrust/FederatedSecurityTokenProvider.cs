using System;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.WSTrust
{
	[ComVisible(true)]
	public class FederatedSecurityTokenProvider : IssuedSecurityTokenProvider
	{
		private FederatedClientCredentialsParameters _additionalParameters;

		private Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager _securityTokenHandlerCollectionManager;

		internal FederatedClientCredentialsParameters AdditionalParameters
		{
			get
			{
				return _additionalParameters;
			}
			set
			{
				_additionalParameters = value;
			}
		}

		public FederatedSecurityTokenProvider(FederatedClientCredentialsParameters federatedClientCredentialsParameters, IssuedSecurityTokenProvider federatedSecurityTokenProvider)
			: this(federatedClientCredentialsParameters, federatedSecurityTokenProvider, Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager.CreateDefaultSecurityTokenHandlerCollectionManager())
		{
		}

		public FederatedSecurityTokenProvider(FederatedClientCredentialsParameters federatedClientCredentialsParameters, IssuedSecurityTokenProvider federatedSecurityTokenProvider, Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollectionManager securityTokenHandlerCollectionManager)
		{
			if (federatedClientCredentialsParameters == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("federatedClientCredentialsParameters");
			}
			if (federatedSecurityTokenProvider == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("federatedSecurityTokenProvider");
			}
			if (securityTokenHandlerCollectionManager == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenHandlerCollectionManager");
			}
			_additionalParameters = federatedClientCredentialsParameters;
			CloneBase(federatedSecurityTokenProvider);
			_securityTokenHandlerCollectionManager = securityTokenHandlerCollectionManager;
		}

		internal void CloneBase(IssuedSecurityTokenProvider issuedSecurityTokenProvider)
		{
			base.IdentityVerifier = issuedSecurityTokenProvider.IdentityVerifier;
			base.IssuerBinding = issuedSecurityTokenProvider.IssuerBinding;
			base.IssuerAddress = issuedSecurityTokenProvider.IssuerAddress;
			base.TargetAddress = issuedSecurityTokenProvider.TargetAddress;
			base.KeyEntropyMode = issuedSecurityTokenProvider.KeyEntropyMode;
			base.IdentityVerifier = issuedSecurityTokenProvider.IdentityVerifier;
			base.CacheIssuedTokens = issuedSecurityTokenProvider.CacheIssuedTokens;
			base.MaxIssuedTokenCachingTime = issuedSecurityTokenProvider.MaxIssuedTokenCachingTime;
			base.MessageSecurityVersion = issuedSecurityTokenProvider.MessageSecurityVersion;
			base.SecurityTokenSerializer = issuedSecurityTokenProvider.SecurityTokenSerializer;
			base.SecurityAlgorithmSuite = issuedSecurityTokenProvider.SecurityAlgorithmSuite;
			base.IssuedTokenRenewalThresholdPercentage = issuedSecurityTokenProvider.IssuedTokenRenewalThresholdPercentage;
			if (issuedSecurityTokenProvider.IssuerChannelBehaviors != null && issuedSecurityTokenProvider.IssuerChannelBehaviors.Count > 0)
			{
				foreach (IEndpointBehavior issuerChannelBehavior in issuedSecurityTokenProvider.IssuerChannelBehaviors)
				{
					base.IssuerChannelBehaviors.Add(issuerChannelBehavior);
				}
			}
			if (issuedSecurityTokenProvider.TokenRequestParameters == null || issuedSecurityTokenProvider.TokenRequestParameters.Count <= 0)
			{
				return;
			}
			foreach (XmlElement tokenRequestParameter in issuedSecurityTokenProvider.TokenRequestParameters)
			{
				base.TokenRequestParameters.Add(tokenRequestParameter);
			}
		}

		protected override IAsyncResult BeginGetTokenCore(TimeSpan timeout, AsyncCallback callback, object state)
		{
			SetupParameters();
			return base.BeginGetTokenCore(timeout, callback, state);
		}

		protected override SecurityToken GetTokenCore(TimeSpan timeout)
		{
			SetupParameters();
			return base.GetTokenCore(timeout);
		}

		private void SetupParameters()
		{
			if (AdditionalParameters.IssuedSecurityToken != null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3024"));
			}
			if (AdditionalParameters.OnBehalfOf != null)
			{
				if (base.MessageSecurityVersion.TrustVersion == TrustVersion.WSTrust13)
				{
					if (TokenRequestParameterExists("OnBehalfOf", "http://docs.oasis-open.org/ws-sx/ws-trust/200512"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3266", "OnBehalfOf"));
					}
					base.TokenRequestParameters.Add(CreateXmlTokenElement(AdditionalParameters.OnBehalfOf, "trust", "OnBehalfOf", "http://docs.oasis-open.org/ws-sx/ws-trust/200512", "OnBehalfOf"));
				}
				else
				{
					if (base.MessageSecurityVersion.TrustVersion != TrustVersion.WSTrustFeb2005)
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3137", base.MessageSecurityVersion.TrustVersion.Namespace)));
					}
					if (TokenRequestParameterExists("OnBehalfOf", "http://schemas.xmlsoap.org/ws/2005/02/trust"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3266", "OnBehalfOf"));
					}
					base.TokenRequestParameters.Add(CreateXmlTokenElement(AdditionalParameters.OnBehalfOf, "t", "OnBehalfOf", "http://schemas.xmlsoap.org/ws/2005/02/trust", "OnBehalfOf"));
				}
			}
			if (AdditionalParameters.ActAs != null)
			{
				if (TokenRequestParameterExists("ActAs", "http://docs.oasis-open.org/ws-sx/ws-trust/200802"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3266", "ActAs"));
				}
				base.TokenRequestParameters.Add(CreateXmlTokenElement(AdditionalParameters.ActAs, "tr", "ActAs", "http://docs.oasis-open.org/ws-sx/ws-trust/200802", "ActAs"));
			}
		}

		private bool TokenRequestParameterExists(string localName, string xmlNamespace)
		{
			foreach (XmlElement tokenRequestParameter in base.TokenRequestParameters)
			{
				if (tokenRequestParameter.LocalName == localName && tokenRequestParameter.NamespaceURI == xmlNamespace)
				{
					return true;
				}
			}
			return false;
		}

		private XmlElement CreateXmlTokenElement(SecurityToken token, string prefix, string name, string ns, string usage)
		{
			Stream stream = new MemoryStream();
			using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, ownsStream: false))
			{
				xmlDictionaryWriter.WriteStartElement(prefix, name, ns);
				WriteToken(xmlDictionaryWriter, token, usage);
				xmlDictionaryWriter.WriteEndElement();
				xmlDictionaryWriter.Flush();
			}
			stream.Seek(0L, SeekOrigin.Begin);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.Load(stream);
			stream.Close();
			return xmlDocument.DocumentElement;
		}

		private void WriteToken(XmlWriter xmlWriter, SecurityToken token, string usage)
		{
			Microsoft.IdentityModel.Tokens.SecurityTokenHandlerCollection securityTokenHandlerCollection = ((!_securityTokenHandlerCollectionManager.ContainsKey(usage)) ? _securityTokenHandlerCollectionManager[""] : _securityTokenHandlerCollectionManager[usage]);
			if (securityTokenHandlerCollection != null && securityTokenHandlerCollection.CanWriteToken(token))
			{
				securityTokenHandlerCollection.WriteToken(xmlWriter, token);
			}
			else
			{
				base.SecurityTokenSerializer.WriteToken(xmlWriter, token);
			}
		}
	}
}
