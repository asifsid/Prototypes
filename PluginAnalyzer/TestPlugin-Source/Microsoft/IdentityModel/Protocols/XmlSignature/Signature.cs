using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class Signature : IDisposable
	{
		private sealed class SignatureValueElement
		{
			private string _id;

			private string _prefix = "ds";

			private byte[] _signatureValue;

			private string _signatureText;

			internal byte[] Value
			{
				get
				{
					return _signatureValue;
				}
				set
				{
					_signatureValue = value;
					_signatureText = null;
				}
			}

			public void ReadFrom(XmlDictionaryReader reader)
			{
				reader.MoveToStartElement("SignatureValue", "http://www.w3.org/2000/09/xmldsig#");
				_prefix = reader.Prefix;
				_id = reader.GetAttribute("Id", null);
				reader.Read();
				_signatureText = reader.ReadString();
				_signatureValue = Convert.FromBase64String(_signatureText.Trim());
				reader.ReadEndElement();
			}

			public void WriteTo(XmlDictionaryWriter writer)
			{
				writer.WriteStartElement(_prefix, "SignatureValue", "http://www.w3.org/2000/09/xmldsig#");
				if (_id != null)
				{
					writer.WriteAttributeString("Id", null, _id);
				}
				if (_signatureText != null)
				{
					writer.WriteString(_signatureText);
				}
				else
				{
					writer.WriteBase64(_signatureValue, 0, _signatureValue.Length);
				}
				writer.WriteEndElement();
			}
		}

		private SignedXml _signedXml;

		private string _id;

		private SecurityKeyIdentifier _keyIdentifier;

		private string _prefix = "ds";

		private SignatureValueElement _signatureValueElement = new SignatureValueElement();

		private SignedInfo _signedInfo;

		private List<Stream> _signedObjects = new List<Stream>(1);

		private bool _disposed;

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

		public SecurityKeyIdentifier KeyIdentifier
		{
			get
			{
				return _keyIdentifier;
			}
			set
			{
				_keyIdentifier = value;
			}
		}

		public SignedInfo SignedInfo => _signedInfo;

		public IList<Stream> SignedObjects => _signedObjects;

		public Signature(SignedXml signedXml, SignedInfo signedInfo)
		{
			_signedXml = signedXml;
			_signedInfo = signedInfo;
		}

		~Signature()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing && !_disposed)
			{
				for (int i = 0; i < _signedObjects.Count; i++)
				{
					_signedObjects[i].Close();
				}
				_signedInfo.Dispose();
			}
			_disposed = true;
		}

		public XmlDictionaryReader GetSignedObjectReader(int index)
		{
			if (index > _signedObjects.Count)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentOutOfRange("index");
			}
			_signedObjects[index].Position = 0L;
			return XmlDictionaryReader.CreateTextReader(_signedObjects[index], XmlDictionaryReaderQuotas.Max);
		}

		public byte[] GetSignatureBytes()
		{
			return _signatureValueElement.Value;
		}

		public void ReadFrom(XmlDictionaryReader reader)
		{
			reader.MoveToStartElement("Signature", "http://www.w3.org/2000/09/xmldsig#");
			_prefix = reader.Prefix;
			Id = reader.GetAttribute("Id", null);
			reader.Read();
			_signedInfo.ReadFrom(reader, _signedXml.TransformFactory);
			_signatureValueElement.ReadFrom(reader);
			if (_signedXml.SecurityTokenSerializer.CanReadKeyIdentifier(reader))
			{
				_keyIdentifier = _signedXml.SecurityTokenSerializer.ReadKeyIdentifier(reader);
			}
			while (reader.IsStartElement("Object", "http://www.w3.org/2000/09/xmldsig#"))
			{
				MemoryStream memoryStream = new MemoryStream();
				using (XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(memoryStream, Encoding.UTF8, ownsStream: false))
				{
					xmlDictionaryWriter.WriteNode(reader, defattr: false);
					xmlDictionaryWriter.Flush();
				}
				_signedObjects.Add(memoryStream);
			}
			reader.ReadEndElement();
		}

		public void SetSignatureValue(byte[] signatureValue)
		{
			_signatureValueElement.Value = signatureValue;
		}

		public void WriteTo(XmlDictionaryWriter writer)
		{
			writer.WriteStartElement(_prefix, "Signature", "http://www.w3.org/2000/09/xmldsig#");
			if (_id != null)
			{
				writer.WriteAttributeString("Id", null, _id);
			}
			_signedInfo.WriteTo(writer);
			_signatureValueElement.WriteTo(writer);
			if (_keyIdentifier != null)
			{
				_signedXml.SecurityTokenSerializer.WriteKeyIdentifier(writer, _keyIdentifier);
			}
			if (_signedObjects.Count > 0)
			{
				for (int i = 0; i < _signedObjects.Count; i++)
				{
					XmlDictionaryReader xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(_signedObjects[i], XmlDictionaryReaderQuotas.Max);
					xmlDictionaryReader.MoveToContent();
					writer.WriteNode(xmlDictionaryReader, defattr: false);
				}
			}
			writer.WriteEndElement();
		}
	}
}
