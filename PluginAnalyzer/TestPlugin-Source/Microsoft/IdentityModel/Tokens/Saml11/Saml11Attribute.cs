using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml11
{
	[ComVisible(true)]
	public class Saml11Attribute : SamlAttribute
	{
		private string _originalIssuer;

		private string _attributeValueXsiType = "http://www.w3.org/2001/XMLSchema#string";

		public new string OriginalIssuer
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
				_originalIssuer = StringUtil.OptimizeString(value);
			}
		}

		public new string AttributeValueXsiType
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

		public Saml11Attribute()
		{
		}

		public Saml11Attribute(string attributeNamespace, string attributeName, IEnumerable<string> attributeValues)
			: base(attributeNamespace, attributeName, attributeValues)
		{
			attributeNamespace = StringUtil.OptimizeString(attributeNamespace);
			attributeName = StringUtil.OptimizeString(attributeName);
		}
	}
}
