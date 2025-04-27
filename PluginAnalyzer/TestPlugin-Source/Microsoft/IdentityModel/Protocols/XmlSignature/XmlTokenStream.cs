using System;
using System.Xml;

namespace Microsoft.IdentityModel.Protocols.XmlSignature
{
	internal sealed class XmlTokenStream
	{
		internal class XmlTokenStreamWriter
		{
			private XmlTokenEntry[] _entries;

			private int _count;

			private int _position;

			private string _excludedElement;

			private int? _excludedElementDepth;

			private string _excludedElementNamespace;

			public int Count => _count;

			public int Position => _position;

			public XmlNodeType NodeType => _entries[_position]._nodeType;

			public bool IsEmptyElement => _entries[_position].IsEmptyElement;

			public string Prefix => _entries[_position]._prefix;

			public string LocalName => _entries[_position]._localName;

			public string NamespaceUri => _entries[_position]._namespaceUri;

			public string Value => _entries[_position].Value;

			public string ExcludedElement => _excludedElement;

			public string ExcludedElementNamespace => _excludedElementNamespace;

			public XmlTokenStreamWriter(XmlTokenEntry[] entries, int count, string excludedElement, int? excludedElementDepth, string excludedElementNamespace)
			{
				if (entries == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("entries");
				}
				_entries = entries;
				_count = count;
				_excludedElement = excludedElement;
				_excludedElementDepth = excludedElementDepth;
				_excludedElementNamespace = excludedElementNamespace;
			}

			public bool MoveToFirst()
			{
				_position = 0;
				return _count > 0;
			}

			public bool MoveToFirstAttribute()
			{
				if (_position < _count - 1 && _entries[_position + 1]._nodeType == XmlNodeType.Attribute)
				{
					_position++;
					return true;
				}
				return false;
			}

			public bool MoveToNext()
			{
				if (_position < _count - 1)
				{
					_position++;
					return true;
				}
				return false;
			}

			public bool MoveToNextAttribute()
			{
				if (_position < _count - 1 && _entries[_position + 1]._nodeType == XmlNodeType.Attribute)
				{
					_position++;
					return true;
				}
				return false;
			}

			public void WriteTo(XmlDictionaryWriter writer)
			{
				if (writer == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("writer");
				}
				if (!MoveToFirst())
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new InvalidOperationException(SR.GetString("ID6025")));
				}
				int num = 0;
				int num2 = -1;
				bool flag = true;
				do
				{
					switch (NodeType)
					{
					case XmlNodeType.Element:
					{
						bool isEmptyElement = IsEmptyElement;
						num++;
						if (flag && (!_excludedElementDepth.HasValue || _excludedElementDepth == num - 1) && LocalName == _excludedElement && NamespaceUri == _excludedElementNamespace)
						{
							flag = false;
							num2 = num;
						}
						if (flag)
						{
							writer.WriteStartElement(Prefix, LocalName, NamespaceUri);
						}
						if (MoveToFirstAttribute())
						{
							do
							{
								if (flag)
								{
									writer.WriteAttributeString(Prefix, LocalName, NamespaceUri, Value);
								}
							}
							while (MoveToNextAttribute());
						}
						if (!isEmptyElement)
						{
							break;
						}
						goto case XmlNodeType.EndElement;
					}
					case XmlNodeType.EndElement:
						if (flag)
						{
							writer.WriteEndElement();
						}
						else if (num2 == num)
						{
							flag = true;
							num2 = -1;
						}
						num--;
						break;
					case XmlNodeType.CDATA:
						if (flag)
						{
							writer.WriteCData(Value);
						}
						break;
					case XmlNodeType.Comment:
						if (flag)
						{
							writer.WriteComment(Value);
						}
						break;
					case XmlNodeType.Text:
						if (flag)
						{
							writer.WriteString(Value);
						}
						break;
					case XmlNodeType.Whitespace:
					case XmlNodeType.SignificantWhitespace:
						if (flag)
						{
							writer.WriteWhitespace(Value);
						}
						break;
					}
				}
				while (MoveToNext());
			}
		}

		internal struct XmlTokenEntry
		{
			internal XmlNodeType _nodeType;

			internal string _prefix;

			internal string _localName;

			internal string _namespaceUri;

			private string _value;

			public bool IsEmptyElement
			{
				get
				{
					return _value == null;
				}
				set
				{
					_value = (value ? null : "");
				}
			}

			public string Value => _value;

			public void Set(XmlNodeType nodeType, string value)
			{
				_nodeType = nodeType;
				_value = value;
			}

			public void SetAttribute(string prefix, string localName, string namespaceUri, string value)
			{
				_nodeType = XmlNodeType.Attribute;
				_prefix = prefix;
				_localName = localName;
				_namespaceUri = namespaceUri;
				_value = value;
			}

			public void SetElement(string prefix, string localName, string namespaceUri, bool isEmptyElement)
			{
				_nodeType = XmlNodeType.Element;
				_prefix = prefix;
				_localName = localName;
				_namespaceUri = namespaceUri;
				IsEmptyElement = isEmptyElement;
			}
		}

		private int _count;

		private XmlTokenEntry[] _entries;

		private string _excludedElement;

		private int? _excludedElementDepth;

		private string _excludedElementNamespace;

		public XmlTokenStream(int initialSize)
		{
			if (initialSize < 1)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentOutOfRangeException("initialSize", SR.GetString("ID0002")));
			}
			_entries = new XmlTokenEntry[initialSize];
		}

		public XmlTokenStream(XmlTokenStream other)
		{
			_count = other._count;
			_excludedElement = other._excludedElement;
			_excludedElementDepth = other._excludedElementDepth;
			_excludedElementNamespace = other._excludedElementNamespace;
			_entries = new XmlTokenEntry[_count];
			Array.Copy(other._entries, _entries, _count);
		}

		public void Add(XmlNodeType nodeType, string value)
		{
			EnsureCapacityToAdd();
			_entries[_count++].Set(nodeType, value);
		}

		public void AddAttribute(string prefix, string localName, string namespaceUri, string value)
		{
			EnsureCapacityToAdd();
			_entries[_count++].SetAttribute(prefix, localName, namespaceUri, value);
		}

		public void AddElement(string prefix, string localName, string namespaceUri, bool isEmptyElement)
		{
			EnsureCapacityToAdd();
			_entries[_count++].SetElement(prefix, localName, namespaceUri, isEmptyElement);
		}

		private void EnsureCapacityToAdd()
		{
			if (_count == _entries.Length)
			{
				XmlTokenEntry[] array = new XmlTokenEntry[_entries.Length * 3 / 2];
				Array.Copy(_entries, 0, array, 0, _count);
				_entries = array;
			}
		}

		public void SetElementExclusion(string excludedElement, string excludedElementNamespace)
		{
			SetElementExclusion(excludedElement, excludedElementNamespace, null);
		}

		public void SetElementExclusion(string excludedElement, string excludedElementNamespace, int? excludedElementDepth)
		{
			_excludedElement = excludedElement;
			_excludedElementDepth = excludedElementDepth;
			_excludedElementNamespace = excludedElementNamespace;
		}

		public XmlTokenStream Trim()
		{
			return new XmlTokenStream(this);
		}

		public XmlTokenStreamWriter GetWriter()
		{
			return new XmlTokenStreamWriter(_entries, _count, _excludedElement, _excludedElementDepth, _excludedElementNamespace);
		}
	}
}
