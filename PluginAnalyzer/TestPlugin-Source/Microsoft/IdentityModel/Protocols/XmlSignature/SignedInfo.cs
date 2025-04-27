using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal class SignedInfo : IDisposable
	{
		private ExclusiveCanonicalizationTransform _canonicalizationMethodElement = new ExclusiveCanonicalizationTransform(isCanonicalizationMethod: true);

		private string _id;

		private string _signatureMethodAlgorithm;

		private MemoryStream _canonicalStream;

		private MemoryStream _bufferedStream;

		private List<Reference> _references;

		private Dictionary<string, string> _context;

		private string _prefix;

		private string _defaultNamespace = string.Empty;

		private bool _sendSide = true;

		private bool _disposed;

		public virtual int ReferenceCount => _references.Count;

		public Reference this[int index] => _references[index];

		protected MemoryStream CanonicalStream
		{
			get
			{
				return _canonicalStream;
			}
			set
			{
				_canonicalStream = value;
			}
		}

		protected bool SendSide
		{
			get
			{
				return _sendSide;
			}
			set
			{
				_sendSide = value;
			}
		}

		public string CanonicalizationMethod
		{
			get
			{
				return _canonicalizationMethodElement.Algorithm;
			}
			set
			{
				if (value != _canonicalizationMethodElement.Algorithm)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6006")));
				}
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

		public string SignatureMethod
		{
			get
			{
				return _signatureMethodAlgorithm;
			}
			set
			{
				_signatureMethodAlgorithm = value;
			}
		}

		public SignedInfo()
		{
			_references = new List<Reference>();
		}

		~SignedInfo()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				if (_canonicalStream != null)
				{
					_canonicalStream.Close();
					_canonicalStream = null;
				}
				if (_bufferedStream != null)
				{
					_bufferedStream.Close();
					_bufferedStream = null;
				}
			}
			_disposed = true;
		}

		public void AddReference(Reference reference)
		{
			_references.Add(reference);
		}

		public void ComputeHash(HashAlgorithm algorithm)
		{
			if (CanonicalizationMethod != "http://www.w3.org/2001/10/xml-exc-c14n#")
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6006")));
			}
			using HashStream hashStream = new HashStream(algorithm);
			ComputeHash(hashStream);
			hashStream.FlushHash();
		}

		protected virtual void ComputeHash(HashStream hashStream)
		{
			if (_sendSide)
			{
				using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(Stream.Null, Encoding.UTF8, ownsStream: false))
				{
					xmlDictionaryWriter.StartCanonicalization(hashStream, includeComments: false, null);
					WriteTo(xmlDictionaryWriter);
					xmlDictionaryWriter.EndCanonicalization();
				}
				return;
			}
			if (_canonicalStream != null)
			{
				_canonicalStream.WriteTo(hashStream);
				return;
			}
			_bufferedStream.Position = 0L;
			using XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(_bufferedStream, XmlDictionaryReaderQuotas.Max);
			xmlDictionaryReader.MoveToContent();
			using XmlDictionaryWriter xmlDictionaryWriter2 = XmlDictionaryWriter.CreateTextWriter(Stream.Null, Encoding.UTF8, ownsStream: false);
			xmlDictionaryWriter2.WriteStartElement("a", _defaultNamespace);
			string[] inclusivePrefixes = GetInclusivePrefixes();
			for (int i = 0; i < inclusivePrefixes.Length; i++)
			{
				string namespaceForInclusivePrefix = GetNamespaceForInclusivePrefix(inclusivePrefixes[i]);
				if (namespaceForInclusivePrefix != null)
				{
					xmlDictionaryWriter2.WriteXmlnsAttribute(inclusivePrefixes[i], namespaceForInclusivePrefix);
				}
			}
			xmlDictionaryWriter2.StartCanonicalization(hashStream, includeComments: false, inclusivePrefixes);
			xmlDictionaryWriter2.WriteNode(xmlDictionaryReader, defattr: false);
			xmlDictionaryWriter2.EndCanonicalization();
			xmlDictionaryWriter2.WriteEndElement();
		}

		public virtual void ComputeReferenceDigests()
		{
			if (_references.Count == 0)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6007")));
			}
			for (int i = 0; i < _references.Count; i++)
			{
				_references[i].ComputeAndSetDigest();
			}
		}

		protected string[] GetInclusivePrefixes()
		{
			return _canonicalizationMethodElement.GetInclusivePrefixes();
		}

		protected virtual string GetNamespaceForInclusivePrefix(string prefix)
		{
			if (_context == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID3015")));
			}
			if (prefix == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("prefix");
			}
			return _context[prefix];
		}

		public virtual void EnsureAllReferencesVerified()
		{
			for (int i = 0; i < _references.Count; i++)
			{
				if (!_references[i].Verified)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6008", _references[i].Uri)));
				}
			}
		}

		public void EnsureDigestValidity(string id, object resolvedXmlSource)
		{
			if (!EnsureDigestValidityIfIdMatches(id, resolvedXmlSource))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6009", id)));
			}
		}

		public virtual bool EnsureDigestValidityIfIdMatches(string id, object resolvedXmlSource)
		{
			for (int i = 0; i < _references.Count; i++)
			{
				if (_references[i].EnsureDigestValidityIfIdMatches(id, resolvedXmlSource))
				{
					return true;
				}
			}
			return false;
		}

		protected void ReadCanonicalizationMethod(XmlDictionaryReader reader)
		{
			_canonicalizationMethodElement.ReadFrom(reader);
		}

		public virtual void ReadFrom(XmlDictionaryReader reader, TransformFactory transformFactory)
		{
			reader.MoveToStartElement("SignedInfo", "http://www.w3.org/2000/09/xmldsig#");
			_sendSide = false;
			_defaultNamespace = reader.LookupNamespace(string.Empty);
			_bufferedStream = new MemoryStream();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Encoding = Encoding.UTF8;
			xmlWriterSettings.NewLineHandling = NewLineHandling.None;
			using (XmlWriter xmlWriter = XmlWriter.Create(_bufferedStream, xmlWriterSettings))
			{
				xmlWriter.WriteNode(reader, defattr: true);
				xmlWriter.Flush();
			}
			_bufferedStream.Position = 0L;
			using (XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(_bufferedStream, XmlDictionaryReaderQuotas.Max))
			{
				CanonicalStream = new MemoryStream();
				xmlDictionaryReader.StartCanonicalization(CanonicalStream, includeComments: false, null);
				xmlDictionaryReader.MoveToStartElement("SignedInfo", "http://www.w3.org/2000/09/xmldsig#");
				_prefix = xmlDictionaryReader.Prefix;
				Id = xmlDictionaryReader.GetAttribute("Id", null);
				xmlDictionaryReader.Read();
				ReadCanonicalizationMethod(xmlDictionaryReader);
				ReadSignatureMethod(xmlDictionaryReader);
				while (xmlDictionaryReader.IsStartElement("Reference", "http://www.w3.org/2000/09/xmldsig#"))
				{
					Reference reference = new Reference();
					reference.ReadFrom(xmlDictionaryReader, transformFactory);
					AddReference(reference);
				}
				xmlDictionaryReader.ReadEndElement();
				xmlDictionaryReader.EndCanonicalization();
			}
			string[] inclusivePrefixes = GetInclusivePrefixes();
			if (inclusivePrefixes != null)
			{
				CanonicalStream = null;
				_context = new Dictionary<string, string>(inclusivePrefixes.Length);
				for (int i = 0; i < inclusivePrefixes.Length; i++)
				{
					_context.Add(inclusivePrefixes[i], reader.LookupNamespace(inclusivePrefixes[i]));
				}
			}
		}

		protected void ReadSignatureMethod(XmlDictionaryReader reader)
		{
			reader.MoveToStartElement("SignatureMethod", "http://www.w3.org/2000/09/xmldsig#");
			bool isEmptyElement = reader.IsEmptyElement;
			_signatureMethodAlgorithm = reader.GetAttribute("Algorithm", null);
			if (string.IsNullOrEmpty(_signatureMethodAlgorithm))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID0001", "Algorithm", "SignatureMethod")));
			}
			reader.Read();
			reader.MoveToContent();
			if (!isEmptyElement)
			{
				reader.ReadEndElement();
			}
		}

		protected void WriteCanonicalizationMethod(XmlDictionaryWriter writer)
		{
			_canonicalizationMethodElement.WriteTo(writer);
		}

		protected void WriteSignatureMethod(XmlDictionaryWriter writer)
		{
			writer.WriteStartElement("ds", "SignatureMethod", "http://www.w3.org/2000/09/xmldsig#");
			writer.WriteAttributeString("Algorithm", null, _signatureMethodAlgorithm);
			writer.WriteEndElement();
		}

		public virtual void WriteTo(XmlDictionaryWriter writer)
		{
			writer.WriteStartElement(_prefix, "SignedInfo", "http://www.w3.org/2000/09/xmldsig#");
			if (Id != null)
			{
				writer.WriteAttributeString("Id", null, Id);
			}
			WriteCanonicalizationMethod(writer);
			WriteSignatureMethod(writer);
			for (int i = 0; i < _references.Count; i++)
			{
				_references[i].WriteTo(writer);
			}
			writer.WriteEndElement();
		}
	}
}
