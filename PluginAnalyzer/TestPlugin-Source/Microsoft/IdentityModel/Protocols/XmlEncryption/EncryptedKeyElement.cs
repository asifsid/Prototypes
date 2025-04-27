using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlEncryption
{
	internal class EncryptedKeyElement : EncryptedTypeElement
	{
		private string _carriedName;

		private string _recipient;

		private List<string> _keyReferences;

		private List<string> _dataReferences;

		public string CarriedName => _carriedName;

		public IList<string> DataReferences => _dataReferences;

		public IList<string> KeyReferences => _keyReferences;

		public EncryptedKeyElement(SecurityTokenSerializer keyInfoSerializer)
			: base(keyInfoSerializer)
		{
			_keyReferences = new List<string>();
			_dataReferences = new List<string>();
		}

		public override void ReadExtensions(XmlDictionaryReader reader)
		{
			reader.MoveToContent();
			if (!reader.IsStartElement("ReferenceList", "http://www.w3.org/2001/04/xmlenc#"))
			{
				return;
			}
			reader.ReadStartElement();
			if (reader.IsStartElement("DataReference", "http://www.w3.org/2001/04/xmlenc#"))
			{
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("DataReference", "http://www.w3.org/2001/04/xmlenc#"))
					{
						string attribute = reader.GetAttribute("URI");
						if (!string.IsNullOrEmpty(attribute))
						{
							_dataReferences.Add(attribute);
						}
						reader.Skip();
						continue;
					}
					if (reader.IsStartElement("KeyReference", "http://www.w3.org/2001/04/xmlenc#"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4189"));
					}
					string text = reader.ReadOuterXml();
					if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
					{
						DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8024", reader.Name, reader.NamespaceURI, text));
					}
				}
			}
			else
			{
				if (!reader.IsStartElement("KeyReference", "http://www.w3.org/2001/04/xmlenc#"))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4191"));
				}
				while (reader.IsStartElement())
				{
					if (reader.IsStartElement("KeyReference", "http://www.w3.org/2001/04/xmlenc#"))
					{
						string attribute2 = reader.GetAttribute("URI");
						if (!string.IsNullOrEmpty(attribute2))
						{
							_keyReferences.Add(attribute2);
						}
						reader.Skip();
						continue;
					}
					if (reader.IsStartElement("DataReference", "http://www.w3.org/2001/04/xmlenc#"))
					{
						throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4190"));
					}
					string text2 = reader.ReadOuterXml();
					if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
					{
						DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8024", reader.Name, reader.NamespaceURI, text2));
					}
				}
			}
			reader.MoveToContent();
			if (reader.IsStartElement("CarriedKeyName", "http://www.w3.org/2001/04/xmlenc#"))
			{
				reader.ReadStartElement();
				_carriedName = reader.ReadString();
				reader.ReadEndElement();
			}
			reader.ReadEndElement();
		}

		public override void ReadXml(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			if (!reader.IsStartElement("EncryptedKey", "http://www.w3.org/2001/04/xmlenc#"))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperXml(reader, SR.GetString("ID4187"));
			}
			_recipient = reader.GetAttribute("Recipient", null);
			base.ReadXml(reader);
		}

		public EncryptedKeyIdentifierClause GetClause()
		{
			return new EncryptedKeyIdentifierClause(base.CipherData.CipherValue, base.Algorithm, base.KeyIdentifier);
		}
	}
}
