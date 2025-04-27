using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class CanonicalizationDriver
	{
		private bool _closeReadersAfterProcessing;

		private XmlReader _reader;

		private string[] _inclusivePrefixes;

		private bool _includeComments;

		public bool CloseReadersAfterProcessing
		{
			get
			{
				return _closeReadersAfterProcessing;
			}
			set
			{
				_closeReadersAfterProcessing = value;
			}
		}

		public bool IncludeComments
		{
			get
			{
				return _includeComments;
			}
			set
			{
				_includeComments = value;
			}
		}

		public string[] GetInclusivePrefixes()
		{
			return _inclusivePrefixes;
		}

		public void Reset()
		{
			_reader = null;
		}

		public void SetInclusivePrefixes(string[] inclusivePrefixes)
		{
			_inclusivePrefixes = inclusivePrefixes;
		}

		public void SetInput(XmlReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("reader");
			}
			_reader = reader;
		}

		public byte[] GetBytes()
		{
			using MemoryStream memoryStream = GetMemoryStream();
			return memoryStream.ToArray();
		}

		public MemoryStream GetMemoryStream()
		{
			MemoryStream memoryStream = new MemoryStream();
			WriteTo(memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream;
		}

		public void WriteTo(HashAlgorithm hashAlgorithm)
		{
			WriteTo(new HashStream(hashAlgorithm));
		}

		public void WriteTo(Stream canonicalStream)
		{
			if (_reader != null)
			{
				XmlDictionaryReader xmlDictionaryReader = _reader as XmlDictionaryReader;
				if (xmlDictionaryReader != null && xmlDictionaryReader.CanCanonicalize)
				{
					xmlDictionaryReader.MoveToContent();
					xmlDictionaryReader.StartCanonicalization(canonicalStream, _includeComments, _inclusivePrefixes);
					xmlDictionaryReader.Skip();
					xmlDictionaryReader.EndCanonicalization();
				}
				else
				{
					using MemoryStream stream = new MemoryStream();
					using XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, ownsStream: false);
					if (_inclusivePrefixes != null)
					{
						xmlDictionaryWriter.WriteStartElement("a", _reader.LookupNamespace(string.Empty));
						for (int i = 0; i < _inclusivePrefixes.Length; i++)
						{
							string text = _reader.LookupNamespace(_inclusivePrefixes[i]);
							if (text != null)
							{
								xmlDictionaryWriter.WriteXmlnsAttribute(_inclusivePrefixes[i], text);
							}
						}
					}
					xmlDictionaryWriter.StartCanonicalization(canonicalStream, _includeComments, _inclusivePrefixes);
					if (_reader is WrappedReader)
					{
						((WrappedReader)_reader).XmlTokens.GetWriter().WriteTo(xmlDictionaryWriter);
					}
					else
					{
						xmlDictionaryWriter.WriteNode(_reader, defattr: false);
					}
					xmlDictionaryWriter.Flush();
					xmlDictionaryWriter.EndCanonicalization();
					if (_inclusivePrefixes != null)
					{
						xmlDictionaryWriter.WriteEndElement();
					}
				}
				if (_closeReadersAfterProcessing)
				{
					_reader.Close();
				}
				_reader = null;
				return;
			}
			throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6003")));
		}
	}
}
