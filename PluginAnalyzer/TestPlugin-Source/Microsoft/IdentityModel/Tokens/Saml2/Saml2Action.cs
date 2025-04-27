using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2Action
	{
		private Uri _namespace;

		private string _value;

		public Uri Namespace
		{
			get
			{
				return _namespace;
			}
			set
			{
				if (null == value)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				if (!value.IsAbsoluteUri)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID0013"));
				}
				_namespace = value;
			}
		}

		public string Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_value = value;
			}
		}

		public Saml2Action(string value, Uri actionNamespace)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
			}
			if (null == actionNamespace)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("actionNamespace");
			}
			if (!actionNamespace.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("actionNamespace", SR.GetString("ID0013"));
			}
			_namespace = actionNamespace;
			_value = value;
		}
	}
}
