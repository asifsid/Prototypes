using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.IdentityModel.Diagnostics;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class Reference
	{
		private struct DigestValueElement
		{
			private byte[] digestValue;

			private string digestText;

			private string prefix;

			internal byte[] Value
			{
				get
				{
					return digestValue;
				}
				set
				{
					digestValue = value;
					digestText = null;
				}
			}

			public void ReadFrom(XmlDictionaryReader reader)
			{
				reader.MoveToStartElement("DigestValue", "http://www.w3.org/2000/09/xmldsig#");
				prefix = reader.Prefix;
				reader.Read();
				reader.MoveToContent();
				digestText = reader.ReadString();
				digestValue = Convert.FromBase64String(digestText.Trim());
				reader.MoveToContent();
				reader.ReadEndElement();
			}

			public void WriteTo(XmlDictionaryWriter writer)
			{
				writer.WriteStartElement(prefix ?? "ds", "DigestValue", "http://www.w3.org/2000/09/xmldsig#");
				if (digestText != null)
				{
					writer.WriteString(digestText);
				}
				else
				{
					writer.WriteBase64(digestValue, 0, digestValue.Length);
				}
				writer.WriteEndElement();
			}
		}

		private string _digestMethodElementAlgorithm;

		private DigestValueElement _digestValueElement = default(DigestValueElement);

		private string _id;

		private string _prefix = "ds";

		private object _resolvedXmlSource;

		private TransformChain _transformChain = new TransformChain();

		private string _type;

		private string _uri;

		private bool _verified;

		private string _referredId;

		public string DigestMethod
		{
			get
			{
				return _digestMethodElementAlgorithm;
			}
			set
			{
				_digestMethodElementAlgorithm = value;
			}
		}

		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public TransformChain TransformChain => _transformChain;

		public int TransformCount => _transformChain.TransformCount;

		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public string Uri
		{
			get
			{
				return _uri;
			}
			set
			{
				_uri = value;
			}
		}

		public bool Verified => _verified;

		public byte[] DigestValue
		{
			get
			{
				return _digestValueElement.Value;
			}
			set
			{
				_digestValueElement.Value = value;
			}
		}

		public Reference()
			: this(null)
		{
		}

		public Reference(string uri)
			: this(uri, null)
		{
		}

		public Reference(string uri, object resolvedXmlSource)
		{
			_uri = uri;
			_resolvedXmlSource = resolvedXmlSource;
		}

		public void AddTransform(Transform transform)
		{
			_transformChain.Add(transform);
		}

		public void EnsureDigestValidity(string id, byte[] computedDigest)
		{
			if (!EnsureDigestValidityIfIdMatches(id, computedDigest))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6009", id)));
			}
		}

		public void EnsureDigestValidity(string id, object resolvedXmlSource)
		{
			if (!EnsureDigestValidityIfIdMatches(id, resolvedXmlSource))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6009", id)));
			}
		}

		public bool EnsureDigestValidityIfIdMatches(string id, byte[] computedDigest)
		{
			if (_verified || id != ExtractReferredId())
			{
				return false;
			}
			if (!ByteArrayComparer.Instance.Equals(computedDigest, DigestValue))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6018", _uri)));
			}
			_verified = true;
			return true;
		}

		public bool EnsureDigestValidityIfIdMatches(string id, object resolvedXmlSource)
		{
			if (_verified || id != ExtractReferredId())
			{
				return false;
			}
			_resolvedXmlSource = resolvedXmlSource;
			if (!CheckDigest())
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6018", _uri)));
			}
			_verified = true;
			return true;
		}

		public string ExtractReferredId()
		{
			if (_referredId == null)
			{
				if (_uri == null || _uri.Length < 2 || _uri[0] != '#')
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6008", _uri)));
				}
				_referredId = _uri.Substring(1);
			}
			return _referredId;
		}

		public bool CheckDigest()
		{
			byte[] array = ComputeDigest();
			bool flag = ByteArrayComparer.Instance.Equals(array, DigestValue);
			if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Verbose))
			{
				DiagnosticUtil.TraceUtil.Trace(TraceEventType.Verbose, TraceCode.Diagnostics, SR.GetString("TraceDigestOfReference"), new ReferenceTraceRecord(flag, array, DigestValue, _uri), null);
			}
			return flag;
		}

		public void ComputeAndSetDigest()
		{
			_digestValueElement.Value = ComputeDigest();
		}

		public byte[] ComputeDigest()
		{
			if (_transformChain.TransformCount == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6020")));
			}
			if (_resolvedXmlSource == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6008", _uri)));
			}
			return _transformChain.TransformToDigest(_resolvedXmlSource, DigestMethod);
		}

		public void ReadFrom(XmlDictionaryReader reader, TransformFactory transformFactory)
		{
			reader.MoveToStartElement("Reference", "http://www.w3.org/2000/09/xmldsig#");
			_prefix = reader.Prefix;
			Id = reader.GetAttribute("Id", null);
			Uri = reader.GetAttribute("URI", null);
			Type = reader.GetAttribute("Type", null);
			reader.Read();
			if (reader.IsStartElement("Transforms", "http://www.w3.org/2000/09/xmldsig#"))
			{
				_transformChain.ReadFrom(reader, transformFactory);
			}
			reader.MoveToStartElement("DigestMethod", "http://www.w3.org/2000/09/xmldsig#");
			bool isEmptyElement = reader.IsEmptyElement;
			_digestMethodElementAlgorithm = reader.GetAttribute("Algorithm", null);
			if (string.IsNullOrEmpty(_digestMethodElementAlgorithm))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID0001", "Algorithm", "DigestMethod")));
			}
			reader.Read();
			reader.MoveToContent();
			if (!isEmptyElement)
			{
				reader.ReadEndElement();
			}
			_digestValueElement.ReadFrom(reader);
			reader.MoveToContent();
			reader.ReadEndElement();
		}

		public void SetResolvedXmlSource(object resolvedXmlSource)
		{
			_resolvedXmlSource = resolvedXmlSource;
		}

		public void WriteTo(XmlDictionaryWriter writer)
		{
			writer.WriteStartElement(_prefix, "Reference", "http://www.w3.org/2000/09/xmldsig#");
			if (_id != null)
			{
				writer.WriteAttributeString("Id", null, _id);
			}
			if (_uri != null)
			{
				writer.WriteAttributeString("URI", null, _uri);
			}
			if (_type != null)
			{
				writer.WriteAttributeString("Type", null, _type);
			}
			if (_transformChain.TransformCount > 0)
			{
				_transformChain.WriteTo(writer);
			}
			writer.WriteStartElement("ds", "DigestMethod", "http://www.w3.org/2000/09/xmldsig#");
			writer.WriteAttributeString("Algorithm", _digestMethodElementAlgorithm);
			writer.WriteEndElement();
			_digestValueElement.WriteTo(writer);
			writer.WriteEndElement();
		}
	}
}
