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
	public sealed class EnvelopedSignatureWriter : DelegatingXmlDictionaryWriter
	{
		private XmlWriter _innerWriter;

		private SigningCredentials _signingCreds;

		private string _referenceId;

		private SecurityTokenSerializer _tokenSerializer;

		private HashStream _hashStream;

		private HashAlgorithm _hashAlgorithm;

		private int _elementCount;

		private MemoryStream _signatureFragment;

		private MemoryStream _endFragment;

		private bool _hasSignatureBeenMarkedForInsert;

		private MemoryStream _writerStream;

		private MemoryStream _preCanonicalTracingStream;

		private bool _disposed;

		public EnvelopedSignatureWriter(XmlWriter innerWriter, SigningCredentials signingCredentials, string referenceId, SecurityTokenSerializer securityTokenSerializer)
		{
			if (innerWriter == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("innerWriter");
			}
			if (signingCredentials == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("signingCredentials");
			}
			if (string.IsNullOrEmpty(referenceId))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentException(SR.GetString("ID0006"), "referenceId"));
			}
			if (securityTokenSerializer == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("securityTokenSerializer");
			}
			_innerWriter = innerWriter;
			_signingCreds = signingCredentials;
			_referenceId = referenceId;
			_tokenSerializer = securityTokenSerializer;
			_signatureFragment = new MemoryStream();
			_endFragment = new MemoryStream();
			_writerStream = new MemoryStream();
			XmlDictionaryWriter innerWriter2 = XmlDictionaryWriter.CreateTextWriter(_writerStream, Encoding.UTF8, ownsStream: false);
			InitializeInnerWriter(innerWriter2);
			_hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(_signingCreds.DigestAlgorithm);
			_hashStream = new HashStream(_hashAlgorithm);
			base.InnerWriter.StartCanonicalization(_hashStream, includeComments: false, null);
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
			{
				_preCanonicalTracingStream = new MemoryStream();
				InitializeTracingWriter(new XmlTextWriter(_preCanonicalTracingStream, Encoding.UTF8));
			}
		}

		private void ComputeSignature()
		{
			PreDigestedSignedInfo preDigestedSignedInfo = new PreDigestedSignedInfo();
			preDigestedSignedInfo.AddEnvelopedSignatureTransform = true;
			preDigestedSignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
			preDigestedSignedInfo.SignatureMethod = _signingCreds.SignatureAlgorithm;
			preDigestedSignedInfo.DigestMethod = _signingCreds.DigestAlgorithm;
			preDigestedSignedInfo.AddReference(_referenceId, _hashStream.FlushHashAndGetValue(_preCanonicalTracingStream));
			SignedXml signedXml = new SignedXml(preDigestedSignedInfo, _tokenSerializer);
			signedXml.ComputeSignature(_signingCreds.SigningKey);
			signedXml.Signature.KeyIdentifier = _signingCreds.SigningKeyIdentifier;
			signedXml.WriteTo(base.InnerWriter);
			((IDisposable)signedXml).Dispose();
			((IDisposable)_hashStream).Dispose();
			_hashStream = null;
		}

		private void OnEndRootElement()
		{
			if (!_hasSignatureBeenMarkedForInsert)
			{
				((IFragmentCapableXmlDictionaryWriter)base.InnerWriter).StartFragment(_endFragment, generateSelfContainedTextFragment: false);
				base.WriteEndElement();
				((IFragmentCapableXmlDictionaryWriter)base.InnerWriter).EndFragment();
			}
			else if (_hasSignatureBeenMarkedForInsert)
			{
				base.WriteEndElement();
				((IFragmentCapableXmlDictionaryWriter)base.InnerWriter).EndFragment();
			}
			EndCanonicalization();
			((IFragmentCapableXmlDictionaryWriter)base.InnerWriter).StartFragment(_signatureFragment, generateSelfContainedTextFragment: false);
			ComputeSignature();
			((IFragmentCapableXmlDictionaryWriter)base.InnerWriter).EndFragment();
			((IFragmentCapableXmlDictionaryWriter)base.InnerWriter).WriteFragment(_signatureFragment.GetBuffer(), 0, (int)_signatureFragment.Length);
			((IFragmentCapableXmlDictionaryWriter)base.InnerWriter).WriteFragment(_endFragment.GetBuffer(), 0, (int)_endFragment.Length);
			_signatureFragment.Close();
			_endFragment.Close();
			_writerStream.Position = 0L;
			_hasSignatureBeenMarkedForInsert = false;
			XmlReader xmlReader = XmlDictionaryReader.CreateTextReader(_writerStream, XmlDictionaryReaderQuotas.Max);
			xmlReader.MoveToContent();
			_innerWriter.WriteNode(xmlReader, defattr: false);
			_innerWriter.Flush();
			xmlReader.Close();
			Close();
		}

		public void WriteSignature()
		{
			Flush();
			if (_writerStream == null || _writerStream.Length == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6029")));
			}
			if (_signatureFragment.Length != 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6030")));
			}
			((IFragmentCapableXmlDictionaryWriter)base.InnerWriter).StartFragment(_endFragment, generateSelfContainedTextFragment: false);
			_hasSignatureBeenMarkedForInsert = true;
		}

		public override void WriteEndElement()
		{
			_elementCount--;
			if (_elementCount == 0)
			{
				Flush();
				OnEndRootElement();
			}
			else
			{
				base.WriteEndElement();
			}
		}

		public override void WriteFullEndElement()
		{
			_elementCount--;
			if (_elementCount == 0)
			{
				Flush();
				OnEndRootElement();
			}
			else
			{
				base.WriteFullEndElement();
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
				if (_hashStream != null)
				{
					_hashStream.Dispose();
					_hashStream = null;
				}
				if (_hashAlgorithm != null)
				{
					((IDisposable)_hashAlgorithm).Dispose();
					_hashAlgorithm = null;
				}
				if (_signatureFragment != null)
				{
					_signatureFragment.Dispose();
					_signatureFragment = null;
				}
				if (_endFragment != null)
				{
					_endFragment.Dispose();
					_endFragment = null;
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
