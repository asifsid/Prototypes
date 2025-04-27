using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.IdentityModel.Diagnostics
{
	[DebuggerDisplay("TraceXPathNavigator")]
	internal class TraceXPathNavigator : XPathNavigator
	{
		public abstract class TraceNode
		{
			private XPathNodeType _nodeType;

			private ElementNode _parent;

			public XPathNodeType NodeType => _nodeType;

			public abstract string NodeValue { get; }

			public ElementNode Parent => _parent;

			public abstract int Size { get; }

			public TraceNode(XPathNodeType nodeType, ElementNode parent)
			{
				_nodeType = nodeType;
				_parent = parent;
			}
		}

		public class AttributeNode : TraceNode
		{
			private string _name;

			private string _nodeValue;

			private string _prefix;

			private string _xmlns;

			public string Name => _name;

			public string NameSpace => _xmlns;

			public override string NodeValue => _nodeValue;

			public string Prefix => _prefix;

			public override int Size
			{
				get
				{
					int num = _name.Length + _nodeValue.Length + 5;
					if (!string.IsNullOrEmpty(_prefix))
					{
						num += _prefix.Length + 1;
					}
					if (!string.IsNullOrEmpty(_xmlns))
					{
						num += _xmlns.Length + 9;
					}
					return num;
				}
			}

			public AttributeNode(string name, string prefix, string nodeValue, string xmlns, ElementNode parent)
				: base(XPathNodeType.Attribute, parent)
			{
				_name = name;
				_nodeValue = nodeValue;
				_prefix = prefix;
				_xmlns = xmlns;
			}
		}

		public class CommentNode : TraceNode
		{
			public string _nodeValue;

			public override string NodeValue => _nodeValue;

			public override int Size => _nodeValue.Length + 8;

			public CommentNode(string text, ElementNode parent)
				: base(XPathNodeType.Comment, parent)
			{
				_nodeValue = text;
			}
		}

		public class ElementNode : TraceNode
		{
			private int _attributeIndex;

			private int _elementIndex;

			private string _name;

			private string _prefix;

			private string _xmlns;

			private List<TraceNode> _childNodes = new List<TraceNode>();

			private List<AttributeNode> _attributes = new List<AttributeNode>();

			private TextNode _textNode;

			private bool _movedToText;

			public List<AttributeNode> Attributes => _attributes;

			public List<TraceNode> ChildNodes => _childNodes;

			public AttributeNode CurrentAttribute => _attributes[_attributeIndex];

			public bool MovedToText
			{
				get
				{
					return _movedToText;
				}
				set
				{
					_movedToText = value;
				}
			}

			public string Name => _name;

			public string NameSpace => _xmlns;

			public override string NodeValue => string.Empty;

			public string Prefix => _prefix;

			public override int Size
			{
				get
				{
					int num = 2 * _name.Length + 6;
					if (!string.IsNullOrEmpty(_prefix))
					{
						num += _prefix.Length + 1;
					}
					if (!string.IsNullOrEmpty(_xmlns))
					{
						num += _xmlns.Length + 9;
					}
					return num;
				}
			}

			public TextNode TextNode
			{
				get
				{
					return _textNode;
				}
				set
				{
					_textNode = value;
				}
			}

			public ElementNode(string name, string prefix, ElementNode parent, string xmlns)
				: base(XPathNodeType.Element, parent)
			{
				_name = name;
				_prefix = prefix;
				_xmlns = xmlns;
			}

			public void Add(TraceNode node)
			{
				_childNodes.Add(node);
			}

			public IEnumerable<ElementNode> FindSubnodes(string[] headersPath)
			{
				if (headersPath == null)
				{
					yield break;
				}
				ElementNode node = this;
				if (string.CompareOrdinal(node._name, headersPath[0]) != 0)
				{
					node = null;
				}
				int i = 0;
				while (node != null)
				{
					int num;
					i = (num = i + 1);
					if (num >= headersPath.Length)
					{
						break;
					}
					ElementNode subNode = null;
					if (node._childNodes != null)
					{
						foreach (TraceNode child in node._childNodes)
						{
							if (child.NodeType != XPathNodeType.Element)
							{
								continue;
							}
							ElementNode childNode = child as ElementNode;
							if (childNode != null && string.CompareOrdinal(childNode._name, headersPath[i]) == 0)
							{
								if (headersPath.Length != i + 1)
								{
									subNode = childNode;
									break;
								}
								yield return childNode;
							}
						}
					}
					node = subNode;
				}
			}

			public bool MoveToFirstAttribute()
			{
				_attributeIndex = 0;
				if (_attributes != null)
				{
					return _attributes.Count > 0;
				}
				return false;
			}

			public TraceNode MoveToNext()
			{
				TraceNode result = null;
				if (_elementIndex + 1 < _childNodes.Count)
				{
					_elementIndex++;
					result = _childNodes[_elementIndex];
				}
				return result;
			}

			public bool MoveToNextAttribute()
			{
				bool result = false;
				if (_attributeIndex + 1 < _attributes.Count)
				{
					_attributeIndex++;
					result = true;
				}
				return result;
			}

			public void Reset()
			{
				_attributeIndex = 0;
				_elementIndex = 0;
				_movedToText = false;
				if (_childNodes == null)
				{
					return;
				}
				foreach (TraceNode childNode in _childNodes)
				{
					if (childNode.NodeType == XPathNodeType.Element)
					{
						(childNode as ElementNode)?.Reset();
					}
				}
			}
		}

		public class ProcessingInstructionNode : TraceNode
		{
			private string _name;

			private string _nodeValue;

			public string Name => _name;

			public override string NodeValue => _nodeValue;

			public override int Size => _name.Length + _nodeValue.Length + 12;

			public ProcessingInstructionNode(string name, string text, ElementNode parent)
				: base(XPathNodeType.ProcessingInstruction, parent)
			{
				_name = name;
				_nodeValue = text;
			}
		}

		public class TextNode : TraceNode
		{
			private string _nodeValue;

			public override string NodeValue => _nodeValue;

			public override int Size => _nodeValue.Length;

			public TextNode(string nodeValue, ElementNode parent)
				: base(XPathNodeType.Text, parent)
			{
				_nodeValue = nodeValue;
			}

			public void AddText(string text)
			{
				_nodeValue += text;
			}
		}

		private bool _closed;

		private TraceNode _current;

		private ElementNode _root;

		private XPathNodeType _state = XPathNodeType.Element;

		public override string BaseURI => string.Empty;

		private CommentNode CurrentComment => _current as CommentNode;

		private ElementNode CurrentElement => _current as ElementNode;

		private ProcessingInstructionNode CurrentProcessingInstruction => _current as ProcessingInstructionNode;

		[DebuggerDisplay("")]
		public override string LocalName => Name;

		public override bool IsEmptyElement
		{
			get
			{
				bool result = true;
				if (_current != null)
				{
					result = CurrentElement.TextNode != null || CurrentElement.ChildNodes.Count > 0;
				}
				return result;
			}
		}

		[DebuggerDisplay("")]
		public override string Name
		{
			get
			{
				if (CurrentElement != null)
				{
					switch (_state)
					{
					case XPathNodeType.Attribute:
						return CurrentElement.CurrentAttribute.Name;
					case XPathNodeType.Element:
						return CurrentElement.Name;
					case XPathNodeType.ProcessingInstruction:
						return CurrentProcessingInstruction.Name;
					}
				}
				return string.Empty;
			}
		}

		[DebuggerDisplay("")]
		public override string NamespaceURI
		{
			get
			{
				if (CurrentElement != null)
				{
					switch (_state)
					{
					case XPathNodeType.Element:
						return CurrentElement.NameSpace;
					case XPathNodeType.Attribute:
						return CurrentElement.CurrentAttribute.NameSpace;
					case XPathNodeType.Namespace:
						return null;
					}
				}
				return string.Empty;
			}
		}

		public override XmlNameTable NameTable => null;

		[DebuggerDisplay("")]
		public override XPathNodeType NodeType => _state;

		[DebuggerDisplay("")]
		public override string Prefix
		{
			get
			{
				string result = string.Empty;
				if (CurrentElement != null)
				{
					switch (_state)
					{
					case XPathNodeType.Element:
						result = CurrentElement.Prefix;
						break;
					case XPathNodeType.Attribute:
						result = CurrentElement.CurrentAttribute.Prefix;
						break;
					case XPathNodeType.Namespace:
						result = null;
						break;
					}
				}
				return result;
			}
		}

		[DebuggerDisplay("")]
		public override string Value
		{
			get
			{
				if (CurrentElement != null)
				{
					switch (_state)
					{
					case XPathNodeType.Text:
						return CurrentElement.TextNode.NodeValue;
					case XPathNodeType.Attribute:
						return CurrentElement.CurrentAttribute.NodeValue;
					case XPathNodeType.Comment:
						return CurrentComment.NodeValue;
					case XPathNodeType.ProcessingInstruction:
						return CurrentProcessingInstruction.NodeValue;
					}
				}
				return string.Empty;
			}
		}

		public WriteState WriteState
		{
			get
			{
				WriteState result = WriteState.Error;
				if (CurrentElement == null)
				{
					result = WriteState.Start;
				}
				else if (_closed)
				{
					result = WriteState.Closed;
				}
				else
				{
					switch (_state)
					{
					case XPathNodeType.Attribute:
						result = WriteState.Attribute;
						break;
					case XPathNodeType.Element:
						result = WriteState.Element;
						break;
					case XPathNodeType.Text:
						result = WriteState.Content;
						break;
					case XPathNodeType.Comment:
						result = WriteState.Content;
						break;
					}
				}
				return result;
			}
		}

		public void AddAttribute(string name, string value, string xmlns, string prefix)
		{
			if (!_closed && CurrentElement != null)
			{
				CurrentElement.Attributes.Add(new AttributeNode(name, prefix, value, xmlns, CurrentElement));
			}
		}

		public void AddComment(string text)
		{
			if (!_closed && CurrentElement != null)
			{
				CurrentElement.Add(new CommentNode(text, CurrentElement));
			}
		}

		public void AddElement(string prefix, string name, string xmlns)
		{
			if (!_closed)
			{
				ElementNode elementNode = new ElementNode(name, prefix, CurrentElement, xmlns);
				if (CurrentElement == null)
				{
					_root = elementNode;
					_current = _root;
				}
				else if (!_closed)
				{
					CurrentElement.Add(elementNode);
					_current = elementNode;
				}
			}
		}

		public void AddProcessingInstruction(string name, string text)
		{
			if (CurrentElement != null)
			{
				ProcessingInstructionNode node = new ProcessingInstructionNode(name, text, CurrentElement);
				CurrentElement.Add(node);
			}
		}

		public void AddText(string value)
		{
			if (!_closed && CurrentElement != null)
			{
				if (CurrentElement.TextNode == null)
				{
					CurrentElement.TextNode = new TextNode(value, CurrentElement);
				}
				else if (!string.IsNullOrEmpty(value))
				{
					CurrentElement.TextNode.AddText(value);
				}
			}
		}

		public void CloseElement()
		{
			if (!_closed)
			{
				_current = CurrentElement.Parent;
				if (_current == null)
				{
					_closed = true;
				}
			}
		}

		public override XPathNavigator Clone()
		{
			return this;
		}

		public override string LookupPrefix(string ns)
		{
			return LookupPrefix(ns, CurrentElement);
		}

		private string LookupPrefix(string ns, ElementNode node)
		{
			string text = null;
			if (string.Compare(ns, node.NameSpace, StringComparison.Ordinal) == 0)
			{
				text = node.Prefix;
			}
			else
			{
				foreach (AttributeNode attribute in node.Attributes)
				{
					if (string.Compare("xmlns", attribute.Prefix, StringComparison.Ordinal) == 0 && string.Compare(ns, attribute.NodeValue, StringComparison.Ordinal) == 0)
					{
						text = attribute.Name;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(text) && node.Parent != null)
			{
				text = LookupPrefix(ns, node.Parent);
			}
			return text;
		}

		public override bool IsSamePosition(XPathNavigator other)
		{
			return false;
		}

		public override bool MoveTo(XPathNavigator other)
		{
			return false;
		}

		public override bool MoveToFirstAttribute()
		{
			if (CurrentElement == null)
			{
				return false;
			}
			bool flag = CurrentElement.MoveToFirstAttribute();
			if (flag)
			{
				_state = XPathNodeType.Attribute;
			}
			return flag;
		}

		public override bool MoveToFirstChild()
		{
			if (CurrentElement == null)
			{
				return false;
			}
			bool result = false;
			if (CurrentElement.ChildNodes != null && CurrentElement.ChildNodes.Count > 0)
			{
				_current = CurrentElement.ChildNodes[0];
				_state = _current.NodeType;
				result = true;
			}
			else if ((CurrentElement.ChildNodes == null || CurrentElement.ChildNodes.Count == 0) && CurrentElement.TextNode != null)
			{
				_state = XPathNodeType.Text;
				CurrentElement.MovedToText = true;
				result = true;
			}
			return result;
		}

		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

		public override bool MoveToId(string id)
		{
			return false;
		}

		public override bool MoveToNext()
		{
			if (CurrentElement == null)
			{
				return false;
			}
			bool result = false;
			if (_state != XPathNodeType.Text)
			{
				ElementNode parent = CurrentElement.Parent;
				if (parent != null)
				{
					TraceNode traceNode = parent.MoveToNext();
					if (traceNode == null && parent.TextNode != null && !parent.MovedToText)
					{
						_state = XPathNodeType.Text;
						parent.MovedToText = true;
						_current = parent;
						result = true;
					}
					else if (traceNode != null)
					{
						_state = traceNode.NodeType;
						result = true;
						_current = traceNode;
					}
				}
			}
			return result;
		}

		public override bool MoveToNextAttribute()
		{
			if (CurrentElement == null)
			{
				return false;
			}
			bool flag = CurrentElement.MoveToNextAttribute();
			if (flag)
			{
				_state = XPathNodeType.Attribute;
			}
			return flag;
		}

		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			return false;
		}

		public override bool MoveToParent()
		{
			if (CurrentElement == null)
			{
				return false;
			}
			bool result = false;
			switch (_state)
			{
			case XPathNodeType.Element:
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				if (_current.Parent != null)
				{
					_current = _current.Parent;
					_state = _current.NodeType;
					result = true;
				}
				break;
			case XPathNodeType.Attribute:
				_state = XPathNodeType.Element;
				result = true;
				break;
			case XPathNodeType.Text:
				_state = XPathNodeType.Element;
				result = true;
				break;
			case XPathNodeType.Namespace:
				_state = XPathNodeType.Element;
				result = true;
				break;
			}
			return result;
		}

		public override bool MoveToPrevious()
		{
			return false;
		}

		public override void MoveToRoot()
		{
			_current = _root;
			_state = XPathNodeType.Element;
			_root.Reset();
		}

		public override string ToString()
		{
			MoveToRoot();
			StringBuilder stringBuilder = new StringBuilder();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.CurrentCulture));
			xmlTextWriter.WriteNode(this, defattr: false);
			return stringBuilder.ToString();
		}
	}
}
