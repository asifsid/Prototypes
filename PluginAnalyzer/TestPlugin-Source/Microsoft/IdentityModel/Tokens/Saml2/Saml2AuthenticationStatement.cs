using System;
using System.Runtime.InteropServices;

namespace Microsoft.IdentityModel.Tokens.Saml2
{
	[ComVisible(true)]
	public class Saml2AuthenticationStatement : Saml2Statement
	{
		private Saml2AuthenticationContext _authnContext;

		private DateTime _authnInstant;

		private string _sessionIndex;

		private DateTime? _sessionNotOnOrAfter;

		private Saml2SubjectLocality _subjectLocality;

		public Saml2AuthenticationContext AuthenticationContext
		{
			get
			{
				return _authnContext;
			}
			set
			{
				if (value == null)
				{
					throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("value");
				}
				_authnContext = value;
			}
		}

		public DateTime AuthenticationInstant
		{
			get
			{
				return _authnInstant;
			}
			set
			{
				_authnInstant = DateTimeUtil.ToUniversalTime(value);
			}
		}

		public string SessionIndex
		{
			get
			{
				return _sessionIndex;
			}
			set
			{
				_sessionIndex = XmlUtil.NormalizeEmptyString(value);
			}
		}

		public DateTime? SessionNotOnOrAfter
		{
			get
			{
				return _sessionNotOnOrAfter;
			}
			set
			{
				_sessionNotOnOrAfter = DateTimeUtil.ToUniversalTime(value);
			}
		}

		public Saml2SubjectLocality SubjectLocality
		{
			get
			{
				return _subjectLocality;
			}
			set
			{
				_subjectLocality = value;
			}
		}

		public Saml2AuthenticationStatement(Saml2AuthenticationContext authenticationContext)
			: this(authenticationContext, DateTime.UtcNow)
		{
		}

		public Saml2AuthenticationStatement(Saml2AuthenticationContext authenticationContext, DateTime authenticationInstant)
		{
			if (authenticationContext == null)
			{
				throw DiagnosticUtil.ExceptionUtil.ThrowHelperArgumentNull("authenticationContext");
			}
			_authnContext = authenticationContext;
			_authnInstant = DateTimeUtil.ToUniversalTime(authenticationInstant);
		}
	}
}
