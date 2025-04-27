using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2AuthenticationContext
	{
		private Collection<Uri> _authenticatingAuthorities = new AbsoluteUriCollection();

		private Uri _classReference;

		private Uri _declarationReference;

		public Collection<Uri> AuthenticatingAuthorities => _authenticatingAuthorities;

		public Uri ClassReference
		{
			get
			{
				return _classReference;
			}
			set
			{
				if (null != value && !value.IsAbsoluteUri)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID0013"));
				}
				_classReference = value;
			}
		}

		public Uri DeclarationReference
		{
			get
			{
				return _declarationReference;
			}
			set
			{
				if (null != value && !value.IsAbsoluteUri)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("value", SR.GetString("ID0013"));
				}
				_declarationReference = value;
			}
		}

		public Saml2AuthenticationContext()
			: this(null, null)
		{
		}

		public Saml2AuthenticationContext(Uri classReference)
			: this(classReference, null)
		{
		}

		public Saml2AuthenticationContext(Uri classReference, Uri declarationReference)
		{
			if (null != classReference && !classReference.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("classReference", SR.GetString("ID0013"));
			}
			if (null != declarationReference && !declarationReference.IsAbsoluteUri)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgument("declarationReference", SR.GetString("ID0013"));
			}
			_classReference = classReference;
			_declarationReference = declarationReference;
		}
	}
}
