using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2SubjectConfirmation
	{
		private Saml2SubjectConfirmationData _data;

		private Uri _method;

		private Saml2NameIdentifier _nameId;

		public Uri Method
		{
			get
			{
				return _method;
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
				_method = value;
			}
		}

		public Saml2NameIdentifier NameIdentifier
		{
			get
			{
				return _nameId;
			}
			set
			{
				_nameId = value;
			}
		}

		public Saml2SubjectConfirmationData SubjectConfirmationData
		{
			get
			{
				return _data;
			}
			set
			{
				_data = value;
			}
		}

		public Saml2SubjectConfirmation(Uri method)
			: this(method, null)
		{
		}

		public Saml2SubjectConfirmation(Uri method, Saml2SubjectConfirmationData data)
		{
			if (null == method)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("method");
			}
			if (!method.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("method", SR.GetString("ID0013"));
			}
			_method = method;
			_data = data;
		}
	}
}
