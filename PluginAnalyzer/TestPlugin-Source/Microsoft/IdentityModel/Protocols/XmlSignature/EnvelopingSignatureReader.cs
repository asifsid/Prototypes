using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	[ComVisible(true)]
	public sealed class EnvelopingSignatureReader : DelegatingXmlDictionaryReader
	{
		private SignedXml _signedXml;

		private SigningCredentials _signingCredentials;

		private bool _disposed;

		public SigningCredentials SigningCredentials => _signingCredentials;

		public EnvelopingSignatureReader(XmlReader innerReader, SecurityTokenSerializer securityTokenSerializer)
			: this(innerReader, securityTokenSerializer, null)
		{
		}

		public EnvelopingSignatureReader(XmlReader innerReader, SecurityTokenSerializer securityTokenSerializer, SecurityTokenResolver signingTokenResolver)
		{
			if (innerReader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("innerReader");
			}
			if (securityTokenSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenSerializer");
			}
			SecurityTokenResolver securityTokenResolver = ((signingTokenResolver != null) ? signingTokenResolver : EmptySecurityTokenResolver.Instance);
			XmlDictionaryReader reader = XmlDictionaryReader.CreateDictionaryReader(innerReader);
			_signedXml = new SignedXml(securityTokenSerializer);
			_signedXml.ReadFrom(reader);
			if (_signedXml.Signature == null || _signedXml.Signature.KeyIdentifier == null || _signedXml.Signature.KeyIdentifier.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperInvalidOperation(SR.GetString("ID3276"));
			}
			if (_signedXml.Signature.SignedObjects.Count != 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID3039")));
			}
			XmlDictionaryReader signedObjectReader = _signedXml.Signature.GetSignedObjectReader(0);
			signedObjectReader.MoveToContent();
			string attribute = signedObjectReader.GetAttribute("Id", null);
			SecurityKey key = null;
			if (!securityTokenResolver.TryResolveSecurityKey(_signedXml.Signature.KeyIdentifier[0], out key))
			{
				if (!_signedXml.Signature.KeyIdentifier.CanCreateKey)
				{
					if (_signedXml.Signature.KeyIdentifier.TryFind<EncryptedKeyIdentifierClause>(out var _))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SignatureVerificationFailedException(SR.GetString("ID4036", XmlUtil.SerializeSecurityKeyIdentifier(_signedXml.Signature.KeyIdentifier, securityTokenSerializer))));
					}
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new SignatureVerificationFailedException(SR.GetString("ID4037", _signedXml.Signature.KeyIdentifier.ToString())));
				}
				key = _signedXml.Signature.KeyIdentifier.CreateKey();
			}
			_signingCredentials = new SigningCredentials(key, _signedXml.Signature.SignedInfo.SignatureMethod, _signedXml.Signature.SignedInfo[0].DigestMethod, _signedXml.Signature.KeyIdentifier);
			_signedXml.StartSignatureVerification(_signingCredentials.SigningKey);
			_signedXml.Signature.SignedInfo.EnsureDigestValidity(attribute, _signedXml.Signature.GetSignedObjectReader(0));
			_signedXml.CompleteSignatureVerification();
			InitializeInnerReader(_signedXml.Signature.GetSignedObjectReader(0));
			base.InnerReader.ReadStartElement("Object", "http://www.w3.org/2000/09/xmldsig#");
		}

		private void OnEndOfRootElement()
		{
			((IDisposable)_signedXml).Dispose();
			_signedXml = null;
		}

		public override bool Read()
		{
			if (base.NodeType == XmlNodeType.EndElement && base.Depth == 1)
			{
				bool result = base.Read();
				OnEndOfRootElement();
				return result;
			}
			if (base.NodeType == XmlNodeType.EndElement && base.Depth == 0)
			{
				return false;
			}
			return base.Read();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!_disposed)
			{
				if (disposing && _signedXml != null)
				{
					_signedXml.Dispose();
					_signedXml = null;
				}
				_disposed = true;
			}
		}
	}
}
