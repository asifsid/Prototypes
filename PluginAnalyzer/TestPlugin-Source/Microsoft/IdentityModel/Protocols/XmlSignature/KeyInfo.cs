using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal class KeyInfo
	{
		private SecurityTokenSerializer _keyInfoSerializer;

		private SecurityKeyIdentifier _ski;

		private string _retrieval;

		public string RetrievalMethod => _retrieval;

		public SecurityKeyIdentifier KeyIdentifier
		{
			get
			{
				return _ski;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_ski = value;
			}
		}

		public KeyInfo(SecurityTokenSerializer keyInfoSerializer)
		{
			_keyInfoSerializer = keyInfoSerializer;
			_ski = new SecurityKeyIdentifier();
		}

		public virtual void ReadXml(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			reader.MoveToContent();
			if (!reader.IsStartElement("KeyInfo", "http://www.w3.org/2000/09/xmldsig#"))
			{
				return;
			}
			reader.ReadStartElement();
			while (reader.IsStartElement())
			{
				if (reader.IsStartElement("RetrievalMethod", "http://www.w3.org/2000/09/xmldsig#"))
				{
					string attribute = reader.GetAttribute("URI");
					if (!string.IsNullOrEmpty(attribute))
					{
						_retrieval = attribute;
					}
					reader.Skip();
				}
				else if (_keyInfoSerializer.CanReadKeyIdentifierClause(reader))
				{
					_ski.Add(_keyInfoSerializer.ReadKeyIdentifierClause(reader));
				}
				else if (reader.IsStartElement())
				{
					string text = reader.ReadOuterXml();
					if (DiagnosticUtil.TraceUtil.ShouldTrace(TraceEventType.Warning))
					{
						DiagnosticUtil.TraceUtil.TraceString(TraceEventType.Warning, SR.GetString("ID8023", reader.Name, reader.NamespaceURI, text));
					}
				}
				reader.MoveToContent();
			}
			reader.MoveToContent();
			reader.ReadEndElement();
		}
	}
}
