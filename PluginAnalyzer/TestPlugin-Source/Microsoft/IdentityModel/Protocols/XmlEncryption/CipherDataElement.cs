using System;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
	internal class CipherDataElement
	{
		private byte[] _iv;

		private byte[] _cipherText;

		public byte[] CipherValue
		{
			get
			{
				if (_iv != null)
				{
					byte[] dst = new byte[_iv.Length + _cipherText.Length];
					Buffer.BlockCopy(_iv, 0, dst, 0, _iv.Length);
					Buffer.BlockCopy(_cipherText, 0, dst, _iv.Length, _cipherText.Length);
					_iv = null;
				}
				return _cipherText;
			}
			set
			{
				_cipherText = value;
			}
		}

		public void ReadXml(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			if (!reader.IsStartElement("CipherData", "http://www.w3.org/2001/04/xmlenc#"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4188"));
			}
			reader.ReadStartElement("CipherData", "http://www.w3.org/2001/04/xmlenc#");
			reader.ReadStartElement("CipherValue", "http://www.w3.org/2001/04/xmlenc#");
			_cipherText = reader.ReadContentAsBase64();
			_iv = null;
			reader.MoveToContent();
			reader.ReadEndElement();
			reader.MoveToContent();
			reader.ReadEndElement();
		}

		public void SetCipherValueFragments(byte[] iv, byte[] cipherText)
		{
			_iv = iv;
			_cipherText = cipherText;
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement("xenc", "CipherData", "http://www.w3.org/2001/04/xmlenc#");
			writer.WriteStartElement("xenc", "CipherValue", "http://www.w3.org/2001/04/xmlenc#");
			if (_iv != null)
			{
				writer.WriteBase64(_iv, 0, _iv.Length);
			}
			writer.WriteBase64(_cipherText, 0, _cipherText.Length);
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
	}
}
