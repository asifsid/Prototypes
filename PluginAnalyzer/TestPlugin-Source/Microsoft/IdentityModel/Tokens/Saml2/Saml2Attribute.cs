using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2Attribute
	{
		private string _friendlyName;

		private string _name;

		private Uri _nameFormat;

		private Collection<string> _values = new Collection<string>();

		private string _originalIssuer;

		private string _attributeValueXsiType = "http://www.w3.org/2001/XMLSchema#string";

		public string FriendlyName
		{
			get
			{
				return _friendlyName;
			}
			set
			{
				_friendlyName = XmlUtil.NormalizeEmptyString(value);
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperError(new ArgumentNullException("value"));
				}
				_name = value;
			}
		}

		public Uri NameFormat
		{
			get
			{
				return _nameFormat;
			}
			set
			{
				if (null != value && !value.IsAbsoluteUri)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("error", SR.GetString("ID0013"));
				}
				_nameFormat = value;
			}
		}

		public string OriginalIssuer
		{
			get
			{
				return _originalIssuer;
			}
			set
			{
				if (value == string.Empty)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID4251"));
				}
				_originalIssuer = value;
			}
		}

		public string AttributeValueXsiType
		{
			get
			{
				return _attributeValueXsiType;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID4254"));
				}
				int num = value.IndexOf('#');
				if (num == -1)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID4254"));
				}
				string text = value.Substring(0, num);
				if (text.Length == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID4254"));
				}
				string text2 = value.Substring(num + 1);
				if (text2.Length == 0)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID4254"));
				}
				_attributeValueXsiType = value;
			}
		}

		public Collection<string> Values => _values;

		public Saml2Attribute(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("name");
			}
			_name = name;
		}

		public Saml2Attribute(string name, string value)
			: this(name, new string[1] { value })
		{
		}

		public Saml2Attribute(string name, IEnumerable<string> values)
			: this(name)
		{
			if (values == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("values");
			}
			foreach (string value in values)
			{
				_values.Add(value);
			}
		}
	}
}
