using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.SecurityTokenService
{
	[ComVisible(true)]
	public class ContextItem
	{
		private Uri _name;

		private Uri _scope;

		private string _value;

		public Uri Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public Uri Scope
		{
			get
			{
				return _scope;
			}
			set
			{
				if (value != null && !value.IsAbsoluteUri)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID0013"));
				}
				_scope = value;
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
				_value = value;
			}
		}

		public ContextItem(Uri name)
			: this(name, null)
		{
		}

		public ContextItem(Uri name, string value)
			: this(name, value, null)
		{
		}

		public ContextItem(Uri name, string value, Uri scope)
		{
			if (name == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("name");
			}
			if (!name.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("name", SR.GetString("ID0013"));
			}
			if (scope != null && !scope.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("scope", SR.GetString("ID0013"));
			}
			_name = name;
			_scope = scope;
			_value = value;
		}
	}
}
