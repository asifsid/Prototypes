using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Xml;
using Microsoft.IdentityModel.Protocols.XmlSignature;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
	internal abstract class EncryptedTypeElement
	{
		private KeyInfo _keyInfo;

		private EncryptionMethodElement _encryptionMethod;

		private CipherDataElement _cipherData;

		private List<string> _properties;

		private SecurityTokenSerializer _keyInfoSerializer;

		private string _id;

		private string _type;

		private string _mimeType;

		private string _encoding;

		public string Algorithm
		{
			get
			{
				if (EncryptionMethod == null)
				{
					return null;
				}
				return EncryptionMethod.Algorithm;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				EncryptionMethod.Algorithm = value;
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
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				_id = value;
			}
		}

		public EncryptionMethodElement EncryptionMethod
		{
			get
			{
				return _encryptionMethod;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_encryptionMethod = value;
			}
		}

		public CipherDataElement CipherData
		{
			get
			{
				return _cipherData;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_cipherData = value;
			}
		}

		public SecurityKeyIdentifier KeyIdentifier
		{
			get
			{
				return _keyInfo.KeyIdentifier;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_keyInfo.KeyIdentifier = value;
			}
		}

		public SecurityTokenSerializer TokenSerializer => _keyInfoSerializer;

		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNullOrEmptyString("value");
				}
				_type = value;
			}
		}

		public EncryptedTypeElement(SecurityTokenSerializer keyInfoSerializer)
		{
			_cipherData = new CipherDataElement();
			_encryptionMethod = new EncryptionMethodElement();
			_keyInfo = new KeyInfo(keyInfoSerializer);
			_properties = new List<string>();
			_keyInfoSerializer = keyInfoSerializer;
		}

		public abstract void ReadExtensions(XmlDictionaryReader reader);

		public virtual void ReadXml(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			_id = reader.GetAttribute("Id", null);
			_type = reader.GetAttribute("Type", null);
			_mimeType = reader.GetAttribute("MimeType", null);
			_encoding = reader.GetAttribute("Encoding", null);
			reader.ReadStartElement();
			reader.MoveToContent();
			if (reader.IsStartElement("EncryptionMethod", "http://www.w3.org/2001/04/xmlenc#"))
			{
				_encryptionMethod.ReadXml(reader);
			}
			reader.MoveToContent();
			if (reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
			{
				_keyInfo = new KeyInfo(_keyInfoSerializer);
				if (_keyInfoSerializer.CanReadKeyIdentifier(reader))
				{
					_keyInfo.KeyIdentifier = _keyInfoSerializer.ReadKeyIdentifier(reader);
				}
				else
				{
					_keyInfo.ReadXml(reader);
				}
			}
			reader.MoveToContent();
			_cipherData.ReadXml(reader);
			ReadExtensions(reader);
			reader.MoveToContent();
			reader.ReadEndElement();
		}
	}
}
