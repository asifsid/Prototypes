using System;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	[ComVisible(true)]
	public sealed class EnvelopingSignatureWriter : DelegatingXmlDictionaryWriter
	{
		private SigningCredentials _signingCreds;

		private XmlDictionaryWriter _innerWriter;

		private HashStream _hashStream;

		private HashAlgorithm _hashAlgorithm;

		private MemoryStream _writerStream;

		private MemoryStream _preCanonicalTracingStream;

		private string _objectId;

		private SecurityTokenSerializer _tokenSerializer;

		private int _elementCount;

		private bool _disposed;

		public EnvelopingSignatureWriter(XmlWriter innerWriter, SigningCredentials signingCredentials, string objectId, SecurityTokenSerializer securityTokenSerializer)
		{
			if (innerWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("innerWriter");
			}
			if (signingCredentials == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signingCredentials");
			}
			if (string.IsNullOrEmpty(objectId))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "objectId"));
			}
			if (securityTokenSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenSerializer");
			}
			_objectId = XmlConvert.VerifyNCName(objectId);
			_innerWriter = XmlDictionaryWriter.CreateDictionaryWriter(innerWriter);
			_signingCreds = signingCredentials;
			_hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(_signingCreds.DigestAlgorithm);
			_hashStream = new HashStream(_hashAlgorithm);
			_writerStream = new MemoryStream();
			_tokenSerializer = securityTokenSerializer;
			XmlDictionaryWriter innerWriter2 = XmlDictionaryWriter.CreateTextWriter(_writerStream, Encoding.UTF8, ownsStream: false);
			InitializeInnerWriter(innerWriter2);
			base.InnerWriter.StartCanonicalization(_hashStream, includeComments: false, null);
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
			{
				_preCanonicalTracingStream = new MemoryStream();
				InitializeTracingWriter(new XmlTextWriter(_preCanonicalTracingStream, Encoding.UTF8));
			}
			base.InnerWriter.WriteStartElement("ds", "Object", "http://www.w3.org/2000/09/xmldsig#");
			base.InnerWriter.WriteAttributeString("Id", _objectId);
		}

		private void OnEndRootElement()
		{
			base.InnerWriter.WriteEndElement();
			base.InnerWriter.Flush();
			base.InnerWriter.EndCanonicalization();
			_writerStream.Position = 0L;
			PreDigestedSignedInfo preDigestedSignedInfo = new PreDigestedSignedInfo();
			preDigestedSignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
			preDigestedSignedInfo.SignatureMethod = _signingCreds.SignatureAlgorithm;
			preDigestedSignedInfo.DigestMethod = _signingCreds.DigestAlgorithm;
			preDigestedSignedInfo.AddReference(_objectId, _hashStream.FlushHashAndGetValue(_preCanonicalTracingStream));
			SignedXml signedXml = new SignedXml(preDigestedSignedInfo, _tokenSerializer);
			signedXml.ComputeSignature(_signingCreds.SigningKey);
			signedXml.Signature.KeyIdentifier = _signingCreds.SigningKeyIdentifier;
			signedXml.Signature.SignedObjects.Add(_writerStream);
			signedXml.WriteTo(_innerWriter);
			_innerWriter.Flush();
			base.InnerWriter.Close();
			((IDisposable)_hashAlgorithm).Dispose();
			_hashAlgorithm = null;
			_hashStream.Dispose();
			_hashStream = null;
			_writerStream.Dispose();
			_writerStream = null;
		}

		public override void WriteEndElement()
		{
			base.InnerWriter.WriteEndElement();
			_elementCount--;
			if (_elementCount == 0)
			{
				OnEndRootElement();
			}
		}

		public override void WriteFullEndElement()
		{
			base.InnerWriter.WriteFullEndElement();
			_elementCount--;
			if (_elementCount == 0)
			{
				OnEndRootElement();
			}
		}

		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			_elementCount++;
			base.WriteStartElement(prefix, localName, ns);
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
				if (_hashAlgorithm != null)
				{
					((IDisposable)_hashAlgorithm).Dispose();
					_hashAlgorithm = null;
				}
				if (_hashStream != null)
				{
					_hashStream.Dispose();
					_hashStream = null;
				}
				if (_writerStream != null)
				{
					_writerStream.Dispose();
					_writerStream = null;
				}
				if (_preCanonicalTracingStream != null)
				{
					_preCanonicalTracingStream.Dispose();
					_preCanonicalTracingStream = null;
				}
			}
			_disposed = true;
		}
	}
}
