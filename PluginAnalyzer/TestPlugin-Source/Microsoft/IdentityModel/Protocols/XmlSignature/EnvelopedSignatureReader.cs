using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	[ComVisible(true)]
	public sealed class EnvelopedSignatureReader : DelegatingXmlDictionaryReader
	{
		private bool _automaticallyReadSignature;

		private int _elementCount;

		private bool _resolveIntrinsicSigningKeys;

		private bool _requireSignature;

		private SigningCredentials _signingCredentials;

		private SecurityTokenResolver _signingTokenResolver;

		private SignedXml _signedXml;

		private SecurityTokenSerializer _tokenSerializer;

		private WrappedReader _wrappedReader;

		private bool _disposed;

		public SigningCredentials SigningCredentials => _signingCredentials;

		internal XmlTokenStream XmlTokens => _wrappedReader.XmlTokens.Trim();

		public EnvelopedSignatureReader(XmlReader reader, SecurityTokenSerializer securityTokenSerializer)
			: this(reader, securityTokenSerializer, null)
		{
		}

		public EnvelopedSignatureReader(XmlReader reader, SecurityTokenSerializer securityTokenSerializer, SecurityTokenResolver signingTokenResolver)
			: this(reader, securityTokenSerializer, signingTokenResolver, requireSignature: true, automaticallyReadSignature: true, resolveIntrinsicSigningKeys: true)
		{
		}

		public EnvelopedSignatureReader(XmlReader reader, SecurityTokenSerializer securityTokenSerializer, SecurityTokenResolver signingTokenResolver, bool requireSignature, bool automaticallyReadSignature, bool resolveIntrinsicSigningKeys)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			if (securityTokenSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenSerializer");
			}
			_automaticallyReadSignature = automaticallyReadSignature;
			_tokenSerializer = securityTokenSerializer;
			_requireSignature = requireSignature;
			_signingTokenResolver = signingTokenResolver ?? EmptySecurityTokenResolver.Instance;
			_resolveIntrinsicSigningKeys = resolveIntrinsicSigningKeys;
			XmlDictionaryReader reader2 = XmlDictionaryReader.CreateDictionaryReader(reader);
			_wrappedReader = new WrappedReader(reader2);
			InitializeInnerReader(_wrappedReader);
		}

		private void OnEndOfRootElement()
		{
			if (_signedXml == null)
			{
				if (_requireSignature)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID3089")));
				}
				return;
			}
			ResolveSigningCredentials();
			_signedXml.StartSignatureVerification(_signingCredentials.SigningKey);
			_wrappedReader.XmlTokens.SetElementExclusion("Signature", "http://www.w3.org/2000/09/xmldsig#");
			_signedXml.EnsureDigestValidity(_signedXml.Signature.SignedInfo[0].ExtractReferredId(), _wrappedReader);
			_signedXml.CompleteSignatureVerification();
			((IDisposable)_signedXml).Dispose();
		}

		public override bool Read()
		{
			if (base.NodeType == XmlNodeType.Element && !base.IsEmptyElement)
			{
				_elementCount++;
			}
			if (base.NodeType == XmlNodeType.EndElement)
			{
				_elementCount--;
				if (_elementCount == 0)
				{
					OnEndOfRootElement();
				}
			}
			bool flag = base.Read();
			if (_automaticallyReadSignature && _signedXml == null && flag && base.InnerReader.IsLocalName("Signature") && base.InnerReader.IsNamespaceUri("http://www.w3.org/2000/09/xmldsig#"))
			{
				ReadSignature();
			}
			return flag;
		}

		private void ReadSignature()
		{
			_signedXml = new SignedXml(_tokenSerializer);
			_signedXml.TransformFactory = TransformFactory.Instance;
			_signedXml.ReadFrom(_wrappedReader);
			if (_signedXml.Signature.SignedInfo.ReferenceCount != 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID3057")));
			}
		}

		private void ResolveSigningCredentials()
		{
			if (_signedXml.Signature == null || _signedXml.Signature.KeyIdentifier == null || _signedXml.Signature.KeyIdentifier.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3276"));
			}
			SecurityKey key = null;
			if (!_signingTokenResolver.TryResolveSecurityKey(_signedXml.Signature.KeyIdentifier[0], out key))
			{
				if (!_resolveIntrinsicSigningKeys || !_signedXml.Signature.KeyIdentifier.CanCreateKey)
				{
					if (_signedXml.Signature.KeyIdentifier.TryFind<EncryptedKeyIdentifierClause>(out var _))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SignatureVerificationFailedException(SR.GetString("ID4036", XmlUtil.SerializeSecurityKeyIdentifier(_signedXml.Signature.KeyIdentifier, _tokenSerializer))));
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SignatureVerificationFailedException(SR.GetString("ID4037", _signedXml.Signature.KeyIdentifier.ToString())));
				}
				key = _signedXml.Signature.KeyIdentifier.CreateKey();
			}
			_signingCredentials = new SigningCredentials(key, _signedXml.Signature.SignedInfo.SignatureMethod, _signedXml.Signature.SignedInfo[0].DigestMethod, _signedXml.Signature.KeyIdentifier);
		}

		public bool TryReadSignature()
		{
			if (IsStartElement("Signature", "http://www.w3.org/2000/09/xmldsig#"))
			{
				ReadSignature();
				return true;
			}
			return false;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				if (_signedXml != null)
				{
					_signedXml.Dispose();
					_signedXml = null;
				}
				if (_wrappedReader != null)
				{
					_wrappedReader.Close();
					_wrappedReader = null;
				}
			}
			_disposed = true;
		}
	}
}
