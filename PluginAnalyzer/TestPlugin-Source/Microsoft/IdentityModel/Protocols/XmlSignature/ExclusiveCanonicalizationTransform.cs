using System;
using System.Security.Cryptography;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal class ExclusiveCanonicalizationTransform : Transform
	{
		private bool _includeComments;

		private string _inclusiveNamespacesPrefixList;

		private string[] _inclusivePrefixes;

		private string _inclusiveListElementPrefix = "ec";

		private string _prefix = "ds";

		private readonly bool _isCanonicalizationMethod;

		public override string Algorithm
		{
			get
			{
				if (!_includeComments)
				{
					return "http://www.w3.org/2001/10/xml-exc-c14n#";
				}
				return "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";
			}
		}

		public bool IncludeComments => _includeComments;

		public string InclusiveNamespacesPrefixList
		{
			get
			{
				return _inclusiveNamespacesPrefixList;
			}
			set
			{
				_inclusiveNamespacesPrefixList = value;
				_inclusivePrefixes = TokenizeInclusivePrefixList(value);
			}
		}

		public override bool NeedsInclusiveContext => GetInclusivePrefixes() != null;

		public ExclusiveCanonicalizationTransform()
			: this(isCanonicalizationMethod: false)
		{
		}

		public ExclusiveCanonicalizationTransform(bool isCanonicalizationMethod)
			: this(isCanonicalizationMethod, includeComments: false)
		{
			_isCanonicalizationMethod = isCanonicalizationMethod;
		}

		protected ExclusiveCanonicalizationTransform(bool isCanonicalizationMethod, bool includeComments)
		{
			_isCanonicalizationMethod = isCanonicalizationMethod;
			_includeComments = includeComments;
		}

		public string[] GetInclusivePrefixes()
		{
			return _inclusivePrefixes;
		}

		private CanonicalizationDriver GetConfiguredDriver()
		{
			CanonicalizationDriver canonicalizationDriver = new CanonicalizationDriver();
			canonicalizationDriver.IncludeComments = IncludeComments;
			canonicalizationDriver.SetInclusivePrefixes(_inclusivePrefixes);
			return canonicalizationDriver;
		}

		public override object Process(object input)
		{
			XmlReader xmlReader = input as XmlReader;
			if (xmlReader != null)
			{
				CanonicalizationDriver configuredDriver = GetConfiguredDriver();
				configuredDriver.SetInput(xmlReader);
				return configuredDriver.GetMemoryStream();
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6004", input.GetType())));
		}

		public override byte[] ProcessAndDigest(object input, string digestAlgorithm)
		{
			using HashAlgorithm hashAlgorithm = CryptoUtil.Algorithms.CreateHashAlgorithm(digestAlgorithm);
			ProcessAndDigest(input, hashAlgorithm);
			return hashAlgorithm.Hash;
		}

		public void ProcessAndDigest(object input, HashAlgorithm hash)
		{
			HashStream hashStream = new HashStream(hash);
			bool flag = false;
			XmlReader xmlReader = input as XmlReader;
			if (xmlReader != null)
			{
				ProcessReaderInput(xmlReader, hashStream);
				flag = true;
			}
			hashStream.FlushHash();
			((IDisposable)hashStream).Dispose();
			if (!flag)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new NotSupportedException(SR.GetString("ID6004", input.GetType())));
			}
		}

		private void ProcessReaderInput(XmlReader reader, HashStream hashStream)
		{
			reader.MoveToContent();
			CanonicalizationDriver configuredDriver = GetConfiguredDriver();
			configuredDriver.SetInput(reader);
			configuredDriver.WriteTo(hashStream);
		}

		public override void ReadFrom(XmlDictionaryReader reader)
		{
			string localName = (_isCanonicalizationMethod ? "CanonicalizationMethod" : "Transform");
			reader.MoveToStartElement(localName, "http://www.w3.org/2000/09/xmldsig#");
			_prefix = reader.Prefix;
			bool isEmptyElement = reader.IsEmptyElement;
			string attribute = reader.GetAttribute("Algorithm", null);
			if (string.IsNullOrEmpty(attribute))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID0001", "Algorithm", reader.LocalName)));
			}
			if (attribute != Algorithm)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new CryptographicException(SR.GetString("ID6005", attribute)));
			}
			reader.Read();
			reader.MoveToContent();
			if (isEmptyElement)
			{
				return;
			}
			if (reader.IsStartElement("InclusiveNamespaces", "http://www.w3.org/2001/10/xml-exc-c14n#"))
			{
				_inclusiveListElementPrefix = reader.Prefix;
				bool isEmptyElement2 = reader.IsEmptyElement;
				InclusiveNamespacesPrefixList = reader.GetAttribute("PrefixList", null);
				reader.Read();
				if (!isEmptyElement2)
				{
					reader.ReadEndElement();
				}
			}
			reader.MoveToContent();
			reader.ReadEndElement();
		}

		public override void WriteTo(XmlDictionaryWriter writer)
		{
			string localName = (_isCanonicalizationMethod ? "CanonicalizationMethod" : "Transform");
			string text = (_includeComments ? "http://www.w3.org/2001/10/xml-exc-c14n#WithComments" : "http://www.w3.org/2001/10/xml-exc-c14n#");
			writer.WriteStartElement(_prefix, localName, "http://www.w3.org/2000/09/xmldsig#");
			writer.WriteStartAttribute("Algorithm", null);
			if (text != null)
			{
				writer.WriteString(text);
			}
			writer.WriteEndAttribute();
			if (InclusiveNamespacesPrefixList != null)
			{
				writer.WriteStartElement(_inclusiveListElementPrefix, "InclusiveNamespaces", "http://www.w3.org/2001/10/xml-exc-c14n#");
				writer.WriteAttributeString("PrefixList", null, InclusiveNamespacesPrefixList);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}

		private static string[] TokenizeInclusivePrefixList(string prefixList)
		{
			if (prefixList == null)
			{
				return null;
			}
			string[] array = prefixList.Split(null);
			int num = 0;
			foreach (string text in array)
			{
				if (text == "#default")
				{
					array[num++] = string.Empty;
				}
				else if (text.Length > 0)
				{
					array[num++] = text;
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (num == array.Length)
			{
				return array;
			}
			string[] array2 = new string[num];
			Array.Copy(array, array2, num);
			return array2;
		}
	}
}
