using System;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class EnvelopedSignatureTransform : Transform
	{
		private string _prefix = "ds";

		public override string Algorithm => "http://www.w3.org/2000/09/xmldsig#enveloped-signature";

		public override object Process(object input)
		{
			WrappedReader wrappedReader = input as WrappedReader;
			if (wrappedReader != null)
			{
				wrappedReader.XmlTokens.SetElementExclusion("Signature", "http://www.w3.org/2000/09/xmldsig#", 1);
				return wrappedReader;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6004", input.GetType())));
		}

		public override byte[] ProcessAndDigest(object input, string digestAlgorithm)
		{
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6027")));
		}

		public override void ReadFrom(XmlDictionaryReader reader)
		{
			reader.MoveToContent();
			reader.MoveToStartElement("Transform", "http://www.w3.org/2000/09/xmldsig#");
			bool isEmptyElement = reader.IsEmptyElement;
			_prefix = reader.Prefix;
			string attribute = reader.GetAttribute("Algorithm", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID0001", "Algorithm", reader.LocalName)));
			}
			if (attribute != Algorithm)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6028", attribute)));
			}
			reader.Read();
			reader.MoveToContent();
			if (!isEmptyElement)
			{
				reader.ReadEndElement();
			}
		}

		public override void WriteTo(XmlDictionaryWriter writer)
		{
			writer.WriteStartElement(_prefix, "Transform", "http://www.w3.org/2000/09/xmldsig#");
			writer.WriteAttributeString("Algorithm", null, Algorithm);
			writer.WriteEndElement();
		}
	}
}
