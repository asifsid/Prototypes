using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class PreDigestedSignedInfo : SignedInfo
	{
		private struct ReferenceEntry
		{
			internal string _id;

			internal byte[] _digest;

			public void Set(string id, byte[] digest)
			{
				_id = id;
				_digest = digest;
			}
		}

		private const int InitialReferenceArraySize = 8;

		private bool _addEnvelopedSignatureTransform;

		private int _count;

		private string _digestMethod;

		private ReferenceEntry[] _references;

		public bool AddEnvelopedSignatureTransform
		{
			get
			{
				return _addEnvelopedSignatureTransform;
			}
			set
			{
				_addEnvelopedSignatureTransform = value;
			}
		}

		public string DigestMethod
		{
			get
			{
				return _digestMethod;
			}
			set
			{
				_digestMethod = value;
			}
		}

		public override int ReferenceCount => _count;

		public PreDigestedSignedInfo()
		{
			_references = new ReferenceEntry[8];
		}

		public PreDigestedSignedInfo(string canonicalizationMethod, string digestMethod, string signatureMethod)
		{
			_references = new ReferenceEntry[8];
			base.CanonicalizationMethod = canonicalizationMethod;
			DigestMethod = digestMethod;
			base.SignatureMethod = signatureMethod;
		}

		public void AddReference(string id, byte[] digest)
		{
			if (_count == _references.Length)
			{
				ReferenceEntry[] array = new ReferenceEntry[_references.Length * 2];
				Array.Copy(_references, 0, array, 0, _count);
				_references = array;
			}
			_references[_count++].Set(id, digest);
		}

		protected override void ComputeHash(HashStream hashStream)
		{
			if (AddEnvelopedSignatureTransform)
			{
				base.ComputeHash(hashStream);
				return;
			}
			using XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(Stream.Null, Encoding.UTF8, ownsStream: false);
			xmlDictionaryWriter.StartCanonicalization(hashStream, includeComments: false, null);
			WriteTo(xmlDictionaryWriter);
			xmlDictionaryWriter.Flush();
			xmlDictionaryWriter.EndCanonicalization();
		}

		public override void ComputeReferenceDigests()
		{
		}

		public override void ReadFrom(XmlDictionaryReader reader, TransformFactory transformFactory)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
		}

		public override void EnsureAllReferencesVerified()
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
		}

		public override bool EnsureDigestValidityIfIdMatches(string id, object resolvedXmlSource)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException());
		}

		public override void WriteTo(XmlDictionaryWriter writer)
		{
			string prefix = "ds";
			string ns = "http://www.w3.org/2000/09/xmldsig#";
			writer.WriteStartElement(prefix, "SignedInfo", ns);
			if (base.Id != null)
			{
				writer.WriteAttributeString("Id", null, base.Id);
			}
			WriteCanonicalizationMethod(writer);
			WriteSignatureMethod(writer);
			for (int i = 0; i < _count; i++)
			{
				writer.WriteStartElement(prefix, "Reference", ns);
				writer.WriteStartAttribute("URI", null);
				writer.WriteString("#");
				writer.WriteString(_references[i]._id);
				writer.WriteEndAttribute();
				writer.WriteStartElement(prefix, "Transforms", ns);
				if (_addEnvelopedSignatureTransform)
				{
					writer.WriteStartElement(prefix, "Transform", ns);
					writer.WriteStartAttribute("Algorithm", null);
					writer.WriteString("http://www.w3.org/2000/09/xmldsig#enveloped-signature");
					writer.WriteEndAttribute();
					writer.WriteEndElement();
				}
				writer.WriteStartElement(prefix, "Transform", ns);
				writer.WriteStartAttribute("Algorithm", null);
				writer.WriteString("http://www.w3.org/2001/10/xml-exc-c14n#");
				writer.WriteEndAttribute();
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteStartElement(prefix, "DigestMethod", ns);
				writer.WriteStartAttribute("Algorithm", null);
				writer.WriteString(_digestMethod);
				writer.WriteEndAttribute();
				writer.WriteEndElement();
				byte[] digest = _references[i]._digest;
				writer.WriteStartElement(prefix, "DigestValue", ns);
				writer.WriteBase64(digest, 0, digest.Length);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
	}
}
